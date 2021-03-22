using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.IO.Ports;
using System.Text;

namespace Modbus_Client
{
    public partial class Form1 : Form
    {
        public Socket newclient;
        public Socket udpclient;
        public SerialPort comm = new SerialPort();
        public bool Connected = false;
        public Thread connThread;
        public Thread receThread;
        public delegate void MyInvoke(string str);
        public delegate void MyInvoke2();
        public int Clientmode = 1;//客户端模式，1=TCP；2=RTU，3=UDP
        public Form1()
        {
            InitializeComponent();
        }

        public void Connect()
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
                newclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


                //连接服务器
                try
                {
                    newclient.Connect(ie);
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
            comm.PortName = textBoxcom.Text;
            comm.BaudRate = 9600;
            comm.Parity = Parity.None;
            comm.StopBits = StopBits.One;
            comm.DataBits = 8;
            comm.ReadBufferSize = 1024;
            comm.DataReceived += new SerialDataReceivedEventHandler(receivemess_rtu);
            try
            {
                comm.Open();
                buttonconn.BeginInvoke(new Action(() => { buttonconn.Enabled = false; }));
                output("已进入串口通信模式。");
                toolStripStatusLabel1.Text = "串口开启成功！";
                Connected = true;
            }
            catch(Exception ex)
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
                IPEndPoint ie = new IPEndPoint(IPAddress.Parse(ipaddress), port - 1);
                try
                {
                    udpclient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

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
        }

        private void timersend_Tick(object sender, EventArgs e)
        {
            if (Clientmode == 1)
            {
                //持续向服务器发送消息以确定连接是否断开
                int isecond = 5000;//以毫秒为单位
                timersend.Interval = isecond;//5秒触发一次
                byte[] data = new byte[] { 0x00, 0x0f, 0x00, 0x00, 0x00, 0x06, 0x01, 0x04, 0x00, 0x00, 0x00, 0x01 };
                try
                {
                    newclient.Send(data);
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
                    newclient.Receive(data);//接收数据到data数组

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
                    disconnect();
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
            SerialPort _comm = (SerialPort)sender;
            byte[] data = new byte[1024];//定义数据接收数组
            int length = comm.BytesToRead;
            _comm.Read(data, 0, comm.BytesToRead);          //注意，read方法运行后bytestoread会归0，所以length定义要在前面。
            byte[] datashow = new byte[length];//定义所要显示的接收的数据的长度
            for (int i = 0; i < length; i++)//将要显示的数据存放到数组datashow中
            {
                datashow[i] = data[i];
            }
            string stringdata = BitConverter.ToString(datashow);//把数组转换成16进制字符串
            showmess(stringdata);
        }

        public void receivemess_udp()
        {
            while (true)
            {
                IPEndPoint remote = new IPEndPoint(IPAddress.Any, Convert.ToInt16(textBoxport.Text) - 1);
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
                string stringdata = BitConverter.ToString(datashow);//把数组转换成16进制字符串

                showmess(stringdata);
                if (stringdata == "00-00-00-00-00-00")
                {
                    disconnect_udp();
                }
                else if (stringdata.Length == 26)
                {
                    MyInvoke output2 = new MyInvoke(output);
                    MyInvoke app = new MyInvoke(textBoxreceive.AppendText);
                    this.BeginInvoke(output2, new object[] { "返回了一个异常码。" });
                    this.BeginInvoke(app, new object[] { "（异常码）" });
                }

                udpreceive.Close();
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

        public string[] getcode()
        {
            string[] str = new string[13];
            str[0] = "00-01-00-00-00-06-01-01-00-14-00-13";
            str[1] = "00-01-00-00-00-06-01-02-00-C5-00-16";
            str[2] = "00-01-00-00-00-06-01-03-00-6C-00-03";
            str[3] = "00-01-00-00-00-06-01-04-00-09-00-01";
            str[4] = "00-01-00-00-00-06-01-05-00-AD-FF-00";
            str[5] = "00-01-00-00-00-06-01-06-00-02-00-03";
            str[6] = "00-01-00-00-00-09-01-0F-00-14-00-0A-02-CD-01";
            str[7] = "00-01-00-00-00-0A-01-10-00-02-00-02-04-00-0A-01-02";
            str[8] = "00-01-00-00-00-10-01-14-0C-06-00-04-00-01-00-02-06-00-03-00-09-00-02";
            str[9] = "00-01-00-00-00-0F-01-15-0D-06-00-04-00-07-00-03-06-AF-04-BE-10-0D";
            str[10] = "00-01-00-00-00-08-01-16-00-04-00-F2-00-25";
            str[11] = "00-01-00-00-00-10-01-17-00-04-00-06-00-0F-00-03-06-00-FF-00-FF-00-FF";
            str[12] = "00-01-00-00-00-05-01-2B-0E-01-00";
            string[] sput = new string[13];

            int j = 0;
            if (Clientmode == 1)
            {
                foreach (string i in str)
                {
                    sput[j] = i;
                    j++;
                }
            }
            else if (Clientmode == 2)
            {
                foreach (string i in str)
                {
                    sput[j] = i.Substring(18, i.Length - 18);
                    j++;
                }
            }
            else
            {
                foreach (string i in str)
                {
                    sput[j] = i;
                    j++;
                }
            }
            return sput;
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

        private void comboBoxcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //示例的功能码
            string[] sput=getcode();
            //显示功能码的功能
            switch (comboBoxcode.SelectedIndex)
            {
                case 0:
                    {
                        
                        textBoxfunc.Text = "功能码0x01：读线圈。\r\n" +
                            "示例：\r\n" + sput[0];
                        textBoxcode.Text = sput[0];
                        break;
                    }
                case 1:
                    {
                        textBoxfunc.Text = "功能码0x02：读输入离散量。\r\n" +
                            "示例：\r\n" + sput[1];
                        textBoxcode.Text = sput[1];
                        break;
                    }
                case 2:
                    {
                        textBoxfunc.Text = "功能码0x03：读多个寄存器。\r\n" +
                            "示例：\r\n" + sput[2];
                        textBoxcode.Text = sput[2];
                        break;
                    }
                case 3:
                    {
                        textBoxfunc.Text = "功能码0x04：读输入寄存器。\r\n" +
                            "示例：\r\n" + sput[3];
                        textBoxcode.Text = sput[3];
                        break;
                    }
                case 4:
                    {
                        textBoxfunc.Text = "功能码0x05：写单个线圈。\r\n" +
                            "示例：\r\n" + sput[4];
                        textBoxcode.Text = sput[4];
                        break;
                    }
                case 5:
                    {
                        textBoxfunc.Text = "功能码0x06：写单个寄存器。\r\n" +
                            "示例：\r\n" + sput[5];
                        textBoxcode.Text = sput[5];
                        break;
                    }
                case 6:
                    {
                        textBoxfunc.Text = "功能码0x0F：写多个线圈。\r\n" +
                            "示例：\r\n" + sput[6];
                        textBoxcode.Text = sput[6];
                        break;
                    }
                case 7:
                    {
                        textBoxfunc.Text = "功能码0x10：写多个寄存器。\r\n" +
                            "示例：\r\n" + sput[7];
                        textBoxcode.Text = sput[7];
                        break;
                    }
                case 8:
                    {
                        textBoxfunc.Text = "功能码0x14：读文件记录。\r\n" +
                            "示例：\r\n" + sput[8];
                        textBoxcode.Text = sput[8];
                        break;
                    }
                case 9:
                    {
                        textBoxfunc.Text = "功能码0x15：写文件记录。\r\n" +
                            "示例：\r\n" + sput[9];
                        textBoxcode.Text = sput[9];
                        break;
                    }
                case 10:
                    {
                        textBoxfunc.Text = "功能码0x16：屏蔽写寄存器。\r\n" +
                            "示例：\r\n" + sput[10];
                        textBoxcode.Text = sput[10];
                        break;
                    }
                case 11:
                    {
                        textBoxfunc.Text = "功能码0x17：读/写多个寄存器。\r\n" +
                            "示例：\r\n" + sput[11];
                        textBoxcode.Text = sput[11];
                        break;
                    }
                case 12:
                    {
                        textBoxfunc.Text = "功能码0x2B：读设备识别码。\r\n" +
                            "示例：\r\n" + sput[12];
                        textBoxcode.Text = sput[12];
                        break;
                    }
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
                //把文本框里的字符串转化成字节数组
                string strdata = textBoxcode.Text;
                string[] strarray = strdata.Split(new char[] { '-' });
                byte[] data = Array.ConvertAll<string, byte>(strarray, delegate (string s)
                {
                    return byte.Parse(s, System.Globalization.NumberStyles.HexNumber);
                });
                byte[] newdata = new byte[data.Length + 2];
                if (Clientmode == 2)
                {
                    string crcs = getCrc16Code(data);
                    data.CopyTo(newdata, 0);
                    newdata[data.Length]= byte.Parse(crcs.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    newdata[data.Length + 1] = byte.Parse(crcs.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                }
                if (Clientmode == 1)
                {
                    send(data);
                }
                else if (Clientmode == 2)
                {
                    send_rtu(newdata);
                }
                else
                {
                    send_udp(data);
                }
            }
        }

        public void send(byte[] data)
        {
            try
            {
                newclient.Send(data);
                toolStripStatusLabel1.Text = "发送成功！";
                output("已发送功能码：" + BitConverter.ToString(data));
            }
            catch (Exception)
            {
                MessageBox.Show("发送失败！", "警告", MessageBoxButtons.OK);
                toolStripStatusLabel1.Text = "发送失败！";
                output("功能码发送失败。");
            }
        }

        public void send_rtu(byte[] data)
        {
            try
            {
                comm.Write(data,0,data.Length);
                toolStripStatusLabel1.Text = "发送成功！";
                output("已发送功能码：" + BitConverter.ToString(data)+"（已自动添加CRC校验码）");
            }
            catch (Exception)
            {
                MessageBox.Show("发送失败！", "警告", MessageBoxButtons.OK);
                toolStripStatusLabel1.Text = "发送失败！";
                output("功能码发送失败。");
            }
        }

        public void send_udp(byte[] data)
        {
            try
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
                    udpclient.SendTo(data,ie);
                    toolStripStatusLabel1.Text = "发送成功！";
                    output("已向"+ipaddress+"/"+port+"发送功能码：" + BitConverter.ToString(data));
                } 
            }
            catch (Exception)
            {
                MessageBox.Show("发送失败！", "警告", MessageBoxButtons.OK);
                toolStripStatusLabel1.Text = "发送失败！";
                output("功能码发送失败。");
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
            if (Clientmode == 1)
            {
                disconnect();
            }
            else if (Clientmode == 2)
            {
                disconnect_rtu();
            }
            else
            {
                disconnect_udp();
            }
        }

        public void disconnect()
        {
            try
            {
                newclient.Close();
                MyInvoke output2 = new MyInvoke(output);
                toolStripStatusLabel1.Text = "连接已断开。";
                this.BeginInvoke(output2, new object[] { "与服务器的连接已断开。" });
                buttonconn.BeginInvoke(new Action(() => { buttonconn.Enabled = true; }));
                buttonconn.BeginInvoke(new Action(() => { buttondisconn.Enabled = false; }));
                Connected = false;
                unlockradio();
            }
            catch (Exception)
            {
                MyInvoke output2 = new MyInvoke(output);
                toolStripStatusLabel1.Text = "连接断开失败！";
                this.BeginInvoke(output2, new object[] { "与服务器断开连接失败。" });
            }
        }

        public void disconnect_rtu()
        {
            try
            {
                comm.Close();
                MyInvoke output2 = new MyInvoke(output);
                toolStripStatusLabel1.Text = "连接已断开。";
                this.BeginInvoke(output2, new object[] { "与服务器的连接已断开。" });
                buttonconn.BeginInvoke(new Action(() => { buttonconn.Enabled = true; }));
                buttonconn.BeginInvoke(new Action(() => { buttondisconn.Enabled = false; }));
                Connected = false;
                unlockradio();
            }
            catch (Exception)
            {
                MyInvoke output2 = new MyInvoke(output);
                toolStripStatusLabel1.Text = "连接断开失败！";
                this.BeginInvoke(output2, new object[] { "与服务器断开连接失败。" });
            }
        }

        public void disconnect_udp()
        {
            udpclient.Dispose();
            udpclient.Close();
            
            MyInvoke output2 = new MyInvoke(output);
            toolStripStatusLabel1.Text = "已释放资源。";
            this.BeginInvoke(output2, new object[] { "已经关闭UDP模式。" });
            buttonconn.BeginInvoke(new Action(() => { buttonconn.Enabled = true; }));
            buttonconn.BeginInvoke(new Action(() => { buttondisconn.Enabled = false; }));
            Connected = false;
            unlockradio();
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
