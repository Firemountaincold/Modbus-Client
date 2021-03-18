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
        public Socket newclient;
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
            byte[] data = new byte[1024];

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
            try
            {
                comm.Open();
                buttonconn.BeginInvoke(new Action(() => { buttonconn.Enabled = false; }));
                output("已连接到服务器。");
                toolStripStatusLabel1.Text = "连接成功！";
                Connected = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("连接失败！", "警告", MessageBoxButtons.OK);
                toolStripStatusLabel1.Text = "连接失败！";
                output("未能连接到服务器。" + ex.Message);
                unlockradio();
                return;
            }
        }

        public void Connect_UDP()
        {

        }

        private void timersend_Tick(object sender, EventArgs e)
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
                    break;
                }
                int length = data[5];//读取数据长度
                Byte[] datashow = new byte[length + 6];//定义所要显示的接收的数据的长度
                for (int i = 0; i <= length + 5; i++)//将要显示的数据存放到数组datashow中
                    datashow[i] = data[i];
                string stringdata = BitConverter.ToString(datashow);//把数组转换成16进制字符串
                showmess(stringdata);
                if (stringdata == "00-00-00-00-00-00")
                {
                    disconnect();
                }
                else if (datashow.Length == 9)
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
        }

        public string tcp2rtu(string code)
        {
            return code.Substring(18, code.Length - 18);
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

                }
            }
            return sput;
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
                if (Clientmode == 1)
                {
                    send(data);
                }
                else if (Clientmode == 2)
                {
                    send_rtu(data);
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
                Application.Exit();
                Process.GetCurrentProcess().Kill();
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
    }
}