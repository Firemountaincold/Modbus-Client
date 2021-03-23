﻿using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.IO.Ports;

namespace Modbus_Client
{
    public partial class Form1 : Form
    {
        //创建通信类
        public ModbusTCPClient mtcpc = new ModbusTCPClient();
        public ModbusRTUClient mrtuc = new ModbusRTUClient();
        public ModbusUDPClient mudpc = new ModbusUDPClient();
        //创建标志
        public bool Connected = false;
        public int Clientmode = 1;//客户端模式，1=TCP；2=RTU，3=UDP
        //创建线程
        public Thread connThread;
        public Thread receThread;
        //创建委托
        public delegate void MyInvoke(string str);
        public delegate void MyInvoke2();
        //创建初始值
        public int type=0x01;
        public short add=0x00;
        public short value=0x00;
        public Form1()
        {
            InitializeComponent();
        }

        public void Connect()
        {
            //创建TCP连接
            string ipaddress = textBoxip.Text.Trim();
            int port = Convert.ToInt32(textBoxport.Text.Trim());//读取ip和端口号

            //检测IP格式是否正确
            IPAddress ipa;
            if (!System.Net.IPAddress.TryParse(ipaddress, out ipa))
            {
                MessageBox.Show("IP地址格式错误！", "警告", MessageBoxButtons.OK);
                toolStripStatusLabel1.Text = "IP错误！";
                output("未提供有效的IP地址。");
            }
            //创建套接字
            else
            {
                IPEndPoint ie = new IPEndPoint(IPAddress.Parse(ipaddress), port);
                //连接服务器
                try
                {
                    mtcpc.Connect(ie);
                    buttonconn.BeginInvoke(new Action(() => { buttonconn.Enabled = false; }));
                    Connected = true;
                    toolStripStatusLabel1.Text = "连接成功！";
                    output("已连接到服务器。");
                }
                catch (SocketException e)
                {
                    MessageBox.Show("连接失败！", "警告", MessageBoxButtons.OK);
                    toolStripStatusLabel1.Text = "连接失败！";
                    output("未能连接到服务器。" + e.Message);
                    buttonconn.BeginInvoke(new Action(() => { buttonconn.Enabled = true; }));
                    buttonconn.BeginInvoke(new Action(() => { buttondisconn.Enabled = false; }));
                    unlockradio();
                    return;
                }

                //使用新线程接受信息
                ThreadStart receThreadDelegate = new ThreadStart(receivemess);
                receThread = new Thread(receThreadDelegate);
                receThread.Start();
            }
        }

        public void Connect_RTU()
        {
            //创建RTU连接
            try
            {
                mrtuc.SetReceiveEvent(receivemess_rtu);
                mrtuc.Connect(textBoxcom.Text);
                buttonconn.BeginInvoke(new Action(() => { buttonconn.Enabled = false; }));
                output("已进入串口通信模式。");
                toolStripStatusLabel1.Text = "串口开启成功！";
                Connected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("串口开启失败！", "警告", MessageBoxButtons.OK);
                toolStripStatusLabel1.Text = "串口失败！";
                output("未能开启串口。" + ex.Message);
                buttonconn.BeginInvoke(new Action(() => { buttonconn.Enabled = true; }));
                buttonconn.BeginInvoke(new Action(() => { buttondisconn.Enabled = false; }));
                unlockradio();
                return;
            }
        }

        public void Connect_UDP()
        {
            //开启UDP模式
            try
            {
                mudpc.Connect();
                buttonconn.BeginInvoke(new Action(() => { buttonconn.Enabled = false; }));
                Connected = true;
                toolStripStatusLabel1.Text = "已启动UDP模式！";
                output("已启动UDP模式。");
                receivemess_udp();
            }
            catch (Exception e)
            {
                MessageBox.Show("连接失败！", "警告", MessageBoxButtons.OK);
                toolStripStatusLabel1.Text = "连接失败！";
                output("未能连接到服务器。" + e.Message);
                unlockradio();
                buttonconn.BeginInvoke(new Action(() => { buttonconn.Enabled = true; }));
                buttonconn.BeginInvoke(new Action(() => { buttondisconn.Enabled = false; }));
                return;
            }
        }

        private void timersend_Tick(object sender, EventArgs e)
        {                
            //持续向TCP服务器发送消息以确定连接是否断开
            if (Clientmode == 1)
            {
                int isecond = 5000;//以毫秒为单位
                timersend.Interval = isecond;//5秒触发一次
                byte[] data = new byte[] { 0x00, 0x0f, 0x00, 0x00, 0x00, 0x06, 0x01, 0x04, 0x00, 0x00, 0x00, 0x01 };
                try
                {
                    mtcpc.Send(data);
                }
                catch (SocketException ee)
                {
                    toolStripStatusLabel1.Text = "连接出现错误！";
                    output("与服务器的连接已断开。\r\n" + ee.Message);
                    Connected = false;
                }
            }
        }

        public void output(string str)
        {
            //使用begininvoke以避免跨线程调用出错
            textBoxinfo.BeginInvoke(new Action(() => { textBoxinfo.Text += DateTime.Now.ToString() + "  " + str + "\r\n"; }));
            textBoxinfo.BeginInvoke(new Action(() => { textBoxinfo.SelectionStart = textBoxinfo.Text.Length; }));
            textBoxinfo.BeginInvoke(new Action(() => { textBoxinfo.ScrollToCaret(); }));
        }

        public void receivemess()
        {
            while (true)
            {
                byte[] data = new byte[1024];//定义数据接收数组
                try
                {
                    data = mtcpc.ReceiveMessage();//接收数据到data数组

                }
                catch (Exception)
                {
                    MyInvoke output2 = new MyInvoke(output);
                    toolStripStatusLabel1.Text = "连接失败！";
                    this.BeginInvoke(output2, new object[] { "连接出现了错误。" });
                    buttonconn.BeginInvoke(new Action(() => { buttonconn.Enabled = true; }));
                    buttonconn.BeginInvoke(new Action(() => { buttondisconn.Enabled = false; }));
                    break;
                }

                int length = data[5];//读取数据长度
                Byte[] datashow = new byte[length + 6];//定义所要显示的接收的数据的长度
                for (int i = 0; i <= length + 5; i++)//将要显示的数据存放到数组datashow中
                {
                    datashow[i] = data[i];
                }
                string stringdata = BitConverter.ToString(datashow);//把数组转换成16进制字符串

                showmess(stringdata);
                if (stringdata == "00-00-00-00-00-00")
                {
                    mtcpc.Disconnect();
                }
                else if (stringdata.Length == 26)
                {
                    MyInvoke output2 = new MyInvoke(output);
                    MyInvoke app = new MyInvoke(textBoxreceive.AppendText);
                    this.BeginInvoke(output2, new object[] { "返回了一个异常码。" });
                    this.BeginInvoke(app, new object[] { "（异常码）" });
                }
            }
        }

        public void receivemess_rtu(object sender, SerialDataReceivedEventArgs e)
        {
            //用事件接收数据
            mrtuc._comm = (SerialPort)sender;
            byte[] data = mrtuc.ReceiveMessage(mrtuc._comm);
            string stringdata = BitConverter.ToString(data);
            if (stringdata != "")
            {
                showmess(stringdata);
            }
        }

        public void receivemess_udp()
        {
            while (true)
            {
                IPEndPoint remote = new IPEndPoint(IPAddress.Any, Convert.ToInt16(textBoxport.Text) - 1);
                byte[] datashow = mudpc.ReceiveMessage(remote);
                string stringdata = BitConverter.ToString(datashow);//把数组转换成16进制字符串

                showmess(stringdata);
                if (stringdata == "00-00-00-00-00-00")
                {
                    mudpc.Disconnect();
                }
                else if (stringdata.Length == 26)
                {
                    MyInvoke output2 = new MyInvoke(output);
                    MyInvoke app = new MyInvoke(textBoxreceive.AppendText);
                    this.BeginInvoke(output2, new object[] { "返回了一个异常码。" });
                    this.BeginInvoke(app, new object[] { "（异常码）" });
                }

                
            }
        }

        private void buttonconn_Click(object sender, EventArgs e)
        {
            MyInvoke output2 = new MyInvoke(output);
            toolStripStatusLabel1.Text = "开始连接服务器……";
            this.BeginInvoke(output2, new object[] { "开始连接服务器……" });
            //使用新线程建立连接
            if (Clientmode == 1)
            {
                ThreadStart conn = new ThreadStart(Connect);
                var connThread = new Thread(conn);
                connThread.Start();
            }
            else if (Clientmode == 2)
            {
                ThreadStart conn = new ThreadStart(Connect_RTU);
                var connThread = new Thread(conn);
                connThread.Start();
            }
            else
            {
                ThreadStart conn = new ThreadStart(Connect_UDP);
                var connThread = new Thread(conn);
                connThread.Start();
            }
            lockradio();
            buttondisconn.Enabled = true;
        }

        private void comboBoxcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //显示功能码的功能
            switch (comboBoxcode.SelectedIndex)
            {
                case 0:
                    {
                        type = 0x01;
                        textBoxfunc.Text = "功能码0x01：读线圈。\r\n";
                        break;
                    }
                case 1:
                    {
                        type = 0x02;
                        textBoxfunc.Text = "功能码0x02：读输入离散量。\r\n";
                        break;
                    }
                case 2:
                    {
                        type = 0x03;
                        textBoxfunc.Text = "功能码0x03：读多个寄存器。\r\n";
                        break;
                    }
                case 3:
                    {
                        type = 0x04;
                        textBoxfunc.Text = "功能码0x04：读输入寄存器。\r\n";
                        break;
                    }
                case 4:
                    {
                        type = 0x05;
                        textBoxfunc.Text = "功能码0x05：写单个线圈。\r\n";
                        break;
                    }
                case 5:
                    {
                        type = 0x06;
                        textBoxfunc.Text = "功能码0x06：写单个寄存器。\r\n";
                        break;
                    }
                case 6:
                    {
                        type = 0x0F;
                        textBoxfunc.Text = "功能码0x0F：写多个线圈。\r\n";
                        break;
                    }
                case 7:
                    {
                        type = 0x10;
                        textBoxfunc.Text = "功能码0x10：写多个寄存器。\r\n";
                        break;
                    }
                case 8:
                    {
                        type = 0x14;
                        textBoxfunc.Text = "功能码0x14：读文件记录。\r\n";
                        break;
                    }
                case 9:
                    {
                        type = 0x15;
                        textBoxfunc.Text = "功能码0x15：写文件记录。\r\n";
                        break;
                    }
                case 10:
                    {
                        type = 0x16;
                        textBoxfunc.Text = "功能码0x16：屏蔽写寄存器。\r\n";
                        break;
                    }
                case 11:
                    {
                        type = 0x17;
                        textBoxfunc.Text = "功能码0x17：读/写多个寄存器。\r\n";
                        break;
                    }
                case 12:
                    {
                        type = 0x18;
                        textBoxfunc.Text = "功能码0x2B：读设备识别码。\r\n";
                        break;
                    }
            }
            refreshcode();
        }

        public void refreshcode()
        {
            //刷新功能码文本框
            if (Clientmode == 1)
            {
                textBoxcode.BeginInvoke(new Action(() => { textBoxcode.Text = BitConverter.ToString(mtcpc.GetTCPFrame(type, add, value)); }));
            }
            else if (Clientmode == 2)
            {
                textBoxcode.BeginInvoke(new Action(() => { textBoxcode.Text = BitConverter.ToString(mrtuc.GetRTUFrame(type, add, value)); }));
            }
            else
            {
                textBoxcode.BeginInvoke(new Action(() => { textBoxcode.Text = BitConverter.ToString(mudpc.GetUDPFrame(type, add, value)); }));
            }
        }

        private void buttonsend_Click(object sender, EventArgs e)
        {
            //检查是否已经连接到服务器
            if (!Connected)
            {
                MessageBox.Show("请先连接服务器！", "警告", MessageBoxButtons.OK);
            }
            else
            {
                toolStripStatusLabel1.Text = "开始发送功能码……";
                output("开始发送功能码……");

                add = Convert.ToInt16(textBoxadd.Text, 16);
                value = Convert.ToInt16(textBoxvalue.Text, 16);
                refreshcode();
                try
                {
                    if (Clientmode == 1)
                    {
                        byte[] data = mtcpc.Send(type, add, value);
                        toolStripStatusLabel1.Text = "发送成功！";
                        output("已发送功能码：" + BitConverter.ToString(data));
                    }
                    else if (Clientmode == 2)
                    {
                        byte[] data = mrtuc.Send(type, add, value);
                        toolStripStatusLabel1.Text = "发送成功！";
                        output("已发送功能码：" + BitConverter.ToString(data));
                    }
                    else
                    {
                        string ipaddress = textBoxip.Text.Trim();
                        int port = Convert.ToInt32(textBoxport.Text.Trim());//读取ip和端口号

                        //检测IP格式是否正确
                        IPAddress ipa;
                        if (!System.Net.IPAddress.TryParse(ipaddress, out ipa))
                        {
                            MessageBox.Show("IP地址格式错误！", "警告", MessageBoxButtons.OK);
                            toolStripStatusLabel1.Text = "IP错误！";
                            output("未提供有效的IP地址。");
                        }
                        //创建套接字
                        else
                        {
                            IPEndPoint ie = new IPEndPoint(IPAddress.Parse(ipaddress), port);
                            byte[] data = mudpc.Send(type, add, value, ie);
                            toolStripStatusLabel1.Text = "发送成功！";
                            output("已向" + ipaddress + "/" + port + "发送功能码：" + BitConverter.ToString(data));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("发送失败！", "警告", MessageBoxButtons.OK);
                    toolStripStatusLabel1.Text = "发送失败！";
                    output("功能码发送失败。"+ex.Message);
                }
               
            }
        }

        public void showmess(string msg)
        {

            //在线程里以安全方式调用控件
            if (textBoxreceive.InvokeRequired)
            {
                MyInvoke _myinvoke = new MyInvoke(showmess);
                MyInvoke output2 = new MyInvoke(output);
                textBoxreceive.BeginInvoke(_myinvoke, new object[] { msg });
                toolStripStatusLabel1.Text = "发送成功！";
                this.BeginInvoke(output2, new object[] { "功能码发送成功。\r\n收到回复：" + msg });
            }
            else
            {
                textBoxreceive.Text = msg;
            }

        }

        private void buttonexit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否退出软件？", "询问", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Process.GetCurrentProcess().Kill();
                Application.Exit();
            }
        }

        private void buttondisconn_Click(object sender, EventArgs e)
        {
            try
            {
                if (Clientmode == 1)
                {
                    mtcpc.Disconnect();
                }
                else if (Clientmode == 2)
                {
                    mrtuc.Disconnect();
                }
                else
                {
                    mudpc.Disconnect();
                }
                
                MyInvoke output2 = new MyInvoke(output);
                toolStripStatusLabel1.Text = "连接已断开。";
                this.BeginInvoke(output2, new object[] { "与服务器的连接已断开。" });
                buttonconn.BeginInvoke(new Action(() => { buttonconn.Enabled = true; }));
                buttonconn.BeginInvoke(new Action(() => { buttondisconn.Enabled = false; }));
                Connected = false;
                unlockradio();
            }
            catch (Exception ex)
            {
                MyInvoke output2 = new MyInvoke(output);
                toolStripStatusLabel1.Text = "连接断开失败！";
                this.BeginInvoke(output2, new object[] { "与服务器断开连接失败。"+ex.Message });
            }
            
        }

        private void radioButtontcp_CheckedChanged(object sender, EventArgs e)
        {
            Clientmode = 1;
        }

        private void radioButtonrtu_CheckedChanged(object sender, EventArgs e)
        {
            Clientmode = 2;
        }

        private void radioButtonudp_CheckedChanged(object sender, EventArgs e)
        {
            Clientmode = 3;
        }

        public void lockradio()
        {
            if (radioButtonrtu.Checked)
            {
                radioButtontcp.Enabled = false;
                radioButtonudp.Enabled = false;
            }
            else if (radioButtontcp.Checked)
            {
                radioButtonudp.Enabled = false;
                radioButtonrtu.Enabled = false;
            }
            else
            {
                radioButtonrtu.Enabled = false;
                radioButtontcp.Enabled = false;
            }
        }

        public void unlockradio()
        {
            radioButtonrtu.BeginInvoke(new Action(() => { radioButtonrtu.Enabled = true; }));
            radioButtontcp.BeginInvoke(new Action(() => { radioButtontcp.Enabled = true; }));
            radioButtonudp.BeginInvoke(new Action(() => { radioButtonudp.Enabled = true; }));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否退出软件？", "询问", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Process.GetCurrentProcess().Kill();
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }

    }

    public class ModbusClient
    {
        //Modbus协议的基础类
        public byte[] GetPDU(int type, short add, short value)
        {
            bool isbigendian = false;//windows是小字节序
            //生成PDU，其他形式的继续重载就行
            byte[] byteadd = BitConverter.GetBytes(Convert.ToInt16(add + 1));//地址从零开始不用+1
            byte[] bytevalue = BitConverter.GetBytes(value);
            if (!isbigendian)//如果是小字节序，需要调换一下位置
            {
                byte temp = byteadd[0];
                byteadd[0] = byteadd[1];
                byteadd[1] = temp;
                temp = bytevalue[0];
                bytevalue[0] = bytevalue[1];
                bytevalue[1] = temp;
            }
            byte[] pdu = new byte[byteadd.Length + bytevalue.Length + 1];
            pdu[0] = Convert.ToByte(type);
            Buffer.BlockCopy(byteadd, 0, pdu, 1, byteadd.Length);
            Buffer.BlockCopy(bytevalue, 0, pdu, byteadd.Length + 1, bytevalue.Length);            
            return pdu;
        }
    }

    public class ModbusTCPClient : ModbusClient
    {
        //用于ModbusTCP的类
        public Socket newclient;

        public void Connect(IPEndPoint ie)
        {
            //建立TCP连接
            newclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            newclient.Connect(ie);
        }

        public void Disconnect()
        {
            //关闭TCP连接
            newclient.Close();
        }

        public byte[] GetTCPFrame(int type, short add, short value)
        {
            //组装TCP帧
            byte[] pdu = this.GetPDU(type, add, value);
            byte pdulength = Convert.ToByte(pdu.Length + 1);
            byte[] tcphead = { 0x00, 0x01, 0x00, 0x00, 0x00, pdulength, 0x01 };
            byte[] tcpframe = new byte[pdu.Length + tcphead.Length];
            Buffer.BlockCopy(tcphead, 0, tcpframe, 0, tcphead.Length);
            Buffer.BlockCopy(pdu, 0, tcpframe, tcphead.Length, pdu.Length);
            return tcpframe;
        }

        public byte[] Send(int type, short add, short value)
        {
            //发送功能码，并返回生成的功能码
            byte[] data = GetTCPFrame(type, add, value);
            newclient.Send(data);
            return (data);
        }

        public void Send(byte[] data)
        {
            //重载一个可以发送任何数组的方法
            newclient.Send(data);
        }

        public byte[] ReceiveMessage()
        {
            //接受信息
            byte[] data = new byte[1024];
            newclient.Receive(data);
            return data;
        }
    }

    public class ModbusRTUClient : ModbusClient
    {
        //用于ModbusRTU的类
        public SerialPort comm = new SerialPort();
        public SerialPort _comm = new SerialPort();

        public void Connect(string com)
        {
            //建立RTU连接
            comm.PortName = com;
            comm.BaudRate = 9600;
            comm.Parity = Parity.None;
            comm.StopBits = StopBits.One;
            comm.DataBits = 8;
            comm.ReadBufferSize = 1024;
            
            comm.Open();
        }

        public void SetReceiveEvent(Action<object,SerialDataReceivedEventArgs> ReceMes)
        {
            //传入用于接收数据的方法
            comm.DataReceived += new SerialDataReceivedEventHandler(ReceMes);
        }

        public void Disconnect()
        {
            //关闭RTU连接
            comm.Close();
        }

        public byte[] GetRTUFrame(int type, short add, short value)
        {
            //组装RTU帧
            byte[] pdu = this.GetPDU(type, add, value);
            byte[] frame = new byte[pdu.Length + 1];
            frame[0] = 1;
            Buffer.BlockCopy(pdu, 0, frame, 1, pdu.Length);
            byte[] rtuframe = new byte[frame.Length + 2];
            string crcs = getCrc16Code(frame);
            frame.CopyTo(rtuframe, 0);
            rtuframe[frame.Length] = byte.Parse(crcs.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            rtuframe[frame.Length + 1] = byte.Parse(crcs.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            
            return rtuframe;
        }

        public byte[] Send(int type, short add, short value)
        {
            //发送功能码，并返回生成的功能码
            byte[] data = GetRTUFrame(type, add, value);
            comm.Write(data, 0, data.Length);
            return data;
        }

        public void Send(byte[] data)
        {
            //重载一个可以发送任何数组的方法
            comm.Write(data, 0, data.Length);
        }

        public byte[] ReceiveMessage(SerialPort com)
        {
            byte[] data = new byte[1024];//定义数据接收数组
            int length = comm.BytesToRead;
            com.Read(data, 0, comm.BytesToRead);  //注意，read方法运行后bytestoread会归0，所以length定义要在前面。
            byte[] datashow = new byte[length];//定义所要显示的接收的数据的长度
            for (int i = 0; i < length; i++)//将要显示的数据存放到数组datashow中
            {
                datashow[i] = data[i];
            }
            return datashow;
        }

        public String getCrc16Code(byte[] crcbyte)
        {
            // 生成CRC校验码
            // 转换成字节数组  
            // 开始crc16校验码计算  
            CRC16Util crc16 = new CRC16Util();
            crc16.update(crcbyte);
            uint crc = crc16.getCrcValue();//使用uint是因为使用int时最高位为1会变为负数。
            // 16进制的CRC码  
            String crcCode = Convert.ToString(crc, 16).ToUpper();
            // 补足到4位  
            if (crcCode.Length < 4)
            {
                crcCode = crcCode.PadLeft(4, '0');
            }
            return crcCode;
        }
    }

    public class ModbusUDPClient : ModbusClient
    {
        //用于ModbusUDP的类
        public Socket udpclient;

        public void Connect()
        {
            //建立UDP Socket
            udpclient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        }

        public void Disconnect()
        {
            //关闭UDP连接
            udpclient.Dispose();
            udpclient.Close();
        }

        public byte[] GetUDPFrame(int type, short add, short value)
        {
            //组装UDP帧
            byte[] pdu = this.GetPDU(type, add, value);
            byte pdulength = Convert.ToByte(pdu.Length + 1);
            byte[] udphead = { 0x00, 0x01, 0x00, 0x00, 0x00, pdulength, 0x01 };
            byte[] udpframe = new byte[pdu.Length + udphead.Length];
            Buffer.BlockCopy(udphead, 0, udpframe, 0, udphead.Length);
            Buffer.BlockCopy(pdu, 0, udpframe, udphead.Length, pdu.Length);
            return udpframe;
        }

        public byte[] Send(int type, short add, short value, IPEndPoint ie)
        {
            //发送功能码，并返回生成的功能码
            byte[] data = GetUDPFrame(type, add, value);
            udpclient.SendTo(data, ie);
            return (data);
        }

        public void Send(byte[] data, IPEndPoint ie)
        {
            //重载一个可以发送任何数组的方法
            udpclient.SendTo(data, ie);
        }

        public byte[] ReceiveMessage(IPEndPoint remote)
        {
            //接受信息
            UdpClient udpreceive = new UdpClient(remote);
            EndPoint remoteEP = (EndPoint)remote;
            byte[] data = udpreceive.Receive(ref remote);
            int length = data[5];//读取数据长度
            Byte[] datashow = new byte[length + 6];//定义所要显示的接收的数据的长度
            for (int i = 0; i < length + 6; i++)//将要显示的数据存放到数组datashow中
            {
                if (i == data.Length)
                {
                    break;
                }
                datashow[i] = data[i];
            }
            udpreceive.Close();
            return data;
        }
    }
    public class CRC16Util
    {

        private uint value = 0x0000;

        static byte[] ArrayCRCHigh =
        {
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
        0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
        0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
        0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
        0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
        0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
        0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
        0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
        0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40
        };

        static byte[] checkCRCLow =
        {
        0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06,
        0x07, 0xC7, 0x05, 0xC5, 0xC4, 0x04, 0xCC, 0x0C, 0x0D, 0xCD,
        0x0F, 0xCF, 0xCE, 0x0E, 0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09,
        0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9, 0x1B, 0xDB, 0xDA, 0x1A,
        0x1E, 0xDE, 0xDF, 0x1F, 0xDD, 0x1D, 0x1C, 0xDC, 0x14, 0xD4,
        0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
        0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3,
        0xF2, 0x32, 0x36, 0xF6, 0xF7, 0x37, 0xF5, 0x35, 0x34, 0xF4,
        0x3C, 0xFC, 0xFD, 0x3D, 0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A,
        0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38, 0x28, 0xE8, 0xE9, 0x29,
        0xEB, 0x2B, 0x2A, 0xEA, 0xEE, 0x2E, 0x2F, 0xEF, 0x2D, 0xED,
        0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
        0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60,
        0x61, 0xA1, 0x63, 0xA3, 0xA2, 0x62, 0x66, 0xA6, 0xA7, 0x67,
        0xA5, 0x65, 0x64, 0xA4, 0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F,
        0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB, 0x69, 0xA9, 0xA8, 0x68,
        0x78, 0xB8, 0xB9, 0x79, 0xBB, 0x7B, 0x7A, 0xBA, 0xBE, 0x7E,
        0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
        0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71,
        0x70, 0xB0, 0x50, 0x90, 0x91, 0x51, 0x93, 0x53, 0x52, 0x92,
        0x96, 0x56, 0x57, 0x97, 0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C,
        0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E, 0x5A, 0x9A, 0x9B, 0x5B,
        0x99, 0x59, 0x58, 0x98, 0x88, 0x48, 0x49, 0x89, 0x4B, 0x8B,
        0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
        0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42,
        0x43, 0x83, 0x41, 0x81, 0x80, 0x40
        };
        //计算一个字节数组的CRC值 
        public void update(byte[] data)
        {
            byte CRCHigh = 0xFF;
            byte CRCLow = 0xFF;
            byte index;
            int i = 0;
            int al = data.Length;
            while (al-- > 0)
            {
                index = (Byte)(CRCHigh ^ data[i++]);
                CRCHigh = (Byte)(CRCLow ^ ArrayCRCHigh[index]);
                CRCLow = checkCRCLow[index];
            }
            value = (UInt16)(CRCHigh << 8 | CRCLow);
        }
        
        public uint getCrcValue()
        {
            return value;
        }
    }
}
