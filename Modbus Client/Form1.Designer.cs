
namespace Modbus_Client
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timersend = new System.Windows.Forms.Timer(this.components);
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.textBoxip = new System.Windows.Forms.TextBox();
            this.textBoxport = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttondisconn = new System.Windows.Forms.Button();
            this.buttonconn = new System.Windows.Forms.Button();
            this.radioButtontcp = new System.Windows.Forms.RadioButton();
            this.radioButtonrtu = new System.Windows.Forms.RadioButton();
            this.radioButtonudp = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxcom = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonexit = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxcode = new System.Windows.Forms.ComboBox();
            this.buttonsend = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxcode = new System.Windows.Forms.TextBox();
            this.textBoxreceive = new System.Windows.Forms.TextBox();
            this.textBoxfunc = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxadd = new System.Windows.Forms.TextBox();
            this.textBoxvalue = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxconfig = new System.Windows.Forms.TextBox();
            this.buttoncreate = new System.Windows.Forms.Button();
            this.buttonload = new System.Windows.Forms.Button();
            this.buttonexit2 = new System.Windows.Forms.Button();
            this.buttondo = new System.Windows.Forms.Button();
            this.buttontab = new System.Windows.Forms.Button();
            this.textBoxinfo = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // timersend
            // 
            this.timersend.Tick += new System.EventHandler(this.timersend_Tick);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(84, 20);
            this.toolStripStatusLabel1.Text = "欢迎使用！";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 643);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1140, 26);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // textBoxip
            // 
            this.textBoxip.Location = new System.Drawing.Point(80, 29);
            this.textBoxip.Name = "textBoxip";
            this.textBoxip.Size = new System.Drawing.Size(331, 25);
            this.textBoxip.TabIndex = 0;
            this.textBoxip.Text = "127.0.0.1";
            // 
            // textBoxport
            // 
            this.textBoxport.Location = new System.Drawing.Point(80, 74);
            this.textBoxport.Name = "textBoxport";
            this.textBoxport.Size = new System.Drawing.Size(100, 25);
            this.textBoxport.TabIndex = 1;
            this.textBoxport.Text = "502";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "IP地址：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "端口号：";
            // 
            // buttondisconn
            // 
            this.buttondisconn.Enabled = false;
            this.buttondisconn.Location = new System.Drawing.Point(186, 152);
            this.buttondisconn.Name = "buttondisconn";
            this.buttondisconn.Size = new System.Drawing.Size(132, 40);
            this.buttondisconn.TabIndex = 11;
            this.buttondisconn.Text = "断开连接";
            this.buttondisconn.UseVisualStyleBackColor = true;
            this.buttondisconn.Click += new System.EventHandler(this.buttondisconn_Click);
            // 
            // buttonconn
            // 
            this.buttonconn.Location = new System.Drawing.Point(12, 152);
            this.buttonconn.Name = "buttonconn";
            this.buttonconn.Size = new System.Drawing.Size(132, 40);
            this.buttonconn.TabIndex = 4;
            this.buttonconn.Text = "连接";
            this.buttonconn.UseVisualStyleBackColor = true;
            this.buttonconn.Click += new System.EventHandler(this.buttonconn_Click);
            // 
            // radioButtontcp
            // 
            this.radioButtontcp.AutoSize = true;
            this.radioButtontcp.Checked = true;
            this.radioButtontcp.Location = new System.Drawing.Point(97, 116);
            this.radioButtontcp.Name = "radioButtontcp";
            this.radioButtontcp.Size = new System.Drawing.Size(108, 19);
            this.radioButtontcp.TabIndex = 12;
            this.radioButtontcp.TabStop = true;
            this.radioButtontcp.Text = "Modbus TCP";
            this.radioButtontcp.UseVisualStyleBackColor = true;
            this.radioButtontcp.CheckedChanged += new System.EventHandler(this.radioButtontcp_CheckedChanged);
            // 
            // radioButtonrtu
            // 
            this.radioButtonrtu.AutoSize = true;
            this.radioButtonrtu.Location = new System.Drawing.Point(227, 116);
            this.radioButtonrtu.Name = "radioButtonrtu";
            this.radioButtonrtu.Size = new System.Drawing.Size(108, 19);
            this.radioButtonrtu.TabIndex = 13;
            this.radioButtonrtu.Text = "Modbus RTU";
            this.radioButtonrtu.UseVisualStyleBackColor = true;
            this.radioButtonrtu.CheckedChanged += new System.EventHandler(this.radioButtonrtu_CheckedChanged);
            // 
            // radioButtonudp
            // 
            this.radioButtonudp.AutoSize = true;
            this.radioButtonudp.Location = new System.Drawing.Point(365, 116);
            this.radioButtonudp.Name = "radioButtonudp";
            this.radioButtonudp.Size = new System.Drawing.Size(108, 19);
            this.radioButtonudp.TabIndex = 14;
            this.radioButtonudp.Text = "Modbus UDP";
            this.radioButtonudp.UseVisualStyleBackColor = true;
            this.radioButtonudp.CheckedChanged += new System.EventHandler(this.radioButtonudp_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 118);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 15);
            this.label7.TabIndex = 15;
            this.label7.Text = "连接模式：";
            // 
            // textBoxcom
            // 
            this.textBoxcom.Location = new System.Drawing.Point(297, 74);
            this.textBoxcom.Name = "textBoxcom";
            this.textBoxcom.Size = new System.Drawing.Size(100, 25);
            this.textBoxcom.TabIndex = 16;
            this.textBoxcom.Text = "COM3";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(226, 79);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 15);
            this.label8.TabIndex = 17;
            this.label8.Text = "串口号：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonexit);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.textBoxcom);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.radioButtonudp);
            this.groupBox1.Controls.Add(this.radioButtonrtu);
            this.groupBox1.Controls.Add(this.radioButtontcp);
            this.groupBox1.Controls.Add(this.buttonconn);
            this.groupBox1.Controls.Add(this.buttondisconn);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxport);
            this.groupBox1.Controls.Add(this.textBoxip);
            this.groupBox1.Location = new System.Drawing.Point(13, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(535, 206);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务器信息";
            // 
            // buttonexit
            // 
            this.buttonexit.Location = new System.Drawing.Point(365, 152);
            this.buttonexit.Name = "buttonexit";
            this.buttonexit.Size = new System.Drawing.Size(132, 40);
            this.buttonexit.TabIndex = 10;
            this.buttonexit.Text = "退出程序";
            this.buttonexit.UseVisualStyleBackColor = true;
            this.buttonexit.Click += new System.EventHandler(this.buttonexit_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "类别：";
            // 
            // comboBoxcode
            // 
            this.comboBoxcode.FormattingEnabled = true;
            this.comboBoxcode.Items.AddRange(new object[] {
            "0x01",
            "0x02",
            "0x03",
            "0x04",
            "0x05",
            "0x06",
            "0x0F",
            "0x10",
            "0x14",
            "0x15",
            "0x16",
            "0x17",
            "0x2B"});
            this.comboBoxcode.Location = new System.Drawing.Point(61, 35);
            this.comboBoxcode.Name = "comboBoxcode";
            this.comboBoxcode.Size = new System.Drawing.Size(80, 23);
            this.comboBoxcode.TabIndex = 1;
            this.comboBoxcode.SelectedIndexChanged += new System.EventHandler(this.comboBoxcode_SelectedIndexChanged);
            // 
            // buttonsend
            // 
            this.buttonsend.Location = new System.Drawing.Point(416, 29);
            this.buttonsend.Name = "buttonsend";
            this.buttonsend.Size = new System.Drawing.Size(96, 34);
            this.buttonsend.TabIndex = 2;
            this.buttonsend.Text = "发送";
            this.buttonsend.UseVisualStyleBackColor = true;
            this.buttonsend.Click += new System.EventHandler(this.buttonsend_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "功能码：";
            // 
            // textBoxcode
            // 
            this.textBoxcode.Location = new System.Drawing.Point(72, 75);
            this.textBoxcode.Name = "textBoxcode";
            this.textBoxcode.ReadOnly = true;
            this.textBoxcode.Size = new System.Drawing.Size(439, 25);
            this.textBoxcode.TabIndex = 4;
            // 
            // textBoxreceive
            // 
            this.textBoxreceive.Location = new System.Drawing.Point(72, 160);
            this.textBoxreceive.Name = "textBoxreceive";
            this.textBoxreceive.ReadOnly = true;
            this.textBoxreceive.Size = new System.Drawing.Size(439, 25);
            this.textBoxreceive.TabIndex = 5;
            // 
            // textBoxfunc
            // 
            this.textBoxfunc.Location = new System.Drawing.Point(72, 118);
            this.textBoxfunc.Name = "textBoxfunc";
            this.textBoxfunc.ReadOnly = true;
            this.textBoxfunc.Size = new System.Drawing.Size(439, 25);
            this.textBoxfunc.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "功能：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 15);
            this.label6.TabIndex = 8;
            this.label6.Text = "返回：";
            // 
            // textBoxadd
            // 
            this.textBoxadd.Location = new System.Drawing.Point(215, 34);
            this.textBoxadd.Name = "textBoxadd";
            this.textBoxadd.Size = new System.Drawing.Size(55, 25);
            this.textBoxadd.TabIndex = 9;
            // 
            // textBoxvalue
            // 
            this.textBoxvalue.Location = new System.Drawing.Point(342, 34);
            this.textBoxvalue.Name = "textBoxvalue";
            this.textBoxvalue.Size = new System.Drawing.Size(55, 25);
            this.textBoxvalue.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(159, 40);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 15);
            this.label9.TabIndex = 11;
            this.label9.Text = "地址：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(283, 40);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 15);
            this.label10.TabIndex = 12;
            this.label10.Text = "数据：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.textBoxvalue);
            this.groupBox2.Controls.Add(this.textBoxadd);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBoxfunc);
            this.groupBox2.Controls.Add(this.textBoxreceive);
            this.groupBox2.Controls.Add(this.textBoxcode);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.buttonsend);
            this.groupBox2.Controls.Add(this.comboBoxcode);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(13, 217);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(535, 197);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "功能码发送";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBoxconfig);
            this.groupBox3.Controls.Add(this.buttoncreate);
            this.groupBox3.Controls.Add(this.buttonload);
            this.groupBox3.Controls.Add(this.buttonexit2);
            this.groupBox3.Controls.Add(this.buttondo);
            this.groupBox3.Enabled = false;
            this.groupBox3.Location = new System.Drawing.Point(13, 421);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(535, 219);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "FTP服务器";
            // 
            // textBoxconfig
            // 
            this.textBoxconfig.Location = new System.Drawing.Point(12, 18);
            this.textBoxconfig.Multiline = true;
            this.textBoxconfig.Name = "textBoxconfig";
            this.textBoxconfig.ReadOnly = true;
            this.textBoxconfig.Size = new System.Drawing.Size(323, 187);
            this.textBoxconfig.TabIndex = 13;
            // 
            // buttoncreate
            // 
            this.buttoncreate.Location = new System.Drawing.Point(342, 117);
            this.buttoncreate.Name = "buttoncreate";
            this.buttoncreate.Size = new System.Drawing.Size(169, 40);
            this.buttoncreate.TabIndex = 12;
            this.buttoncreate.Text = "创建配置文件";
            this.buttoncreate.UseVisualStyleBackColor = true;
            this.buttoncreate.Click += new System.EventHandler(this.buttoncreate_Click);
            // 
            // buttonload
            // 
            this.buttonload.Location = new System.Drawing.Point(342, 18);
            this.buttonload.Name = "buttonload";
            this.buttonload.Size = new System.Drawing.Size(171, 40);
            this.buttonload.TabIndex = 11;
            this.buttonload.Text = "载入配置文件";
            this.buttonload.UseVisualStyleBackColor = true;
            this.buttonload.Click += new System.EventHandler(this.buttonload_Click);
            // 
            // buttonexit2
            // 
            this.buttonexit2.Location = new System.Drawing.Point(342, 165);
            this.buttonexit2.Name = "buttonexit2";
            this.buttonexit2.Size = new System.Drawing.Size(170, 40);
            this.buttonexit2.TabIndex = 9;
            this.buttonexit2.Text = "退出程序";
            this.buttonexit2.UseVisualStyleBackColor = true;
            this.buttonexit2.Click += new System.EventHandler(this.buttonexit2_Click);
            // 
            // buttondo
            // 
            this.buttondo.Location = new System.Drawing.Point(342, 68);
            this.buttondo.Name = "buttondo";
            this.buttondo.Size = new System.Drawing.Size(170, 40);
            this.buttondo.TabIndex = 8;
            this.buttondo.Text = "执行";
            this.buttondo.UseVisualStyleBackColor = true;
            this.buttondo.Click += new System.EventHandler(this.buttondo_Click);
            // 
            // buttontab
            // 
            this.buttontab.Location = new System.Drawing.Point(558, 13);
            this.buttontab.Name = "buttontab";
            this.buttontab.Size = new System.Drawing.Size(570, 43);
            this.buttontab.TabIndex = 12;
            this.buttontab.Text = "切换到FTP视图";
            this.buttontab.UseVisualStyleBackColor = true;
            this.buttontab.Click += new System.EventHandler(this.buttontab_Click);
            // 
            // textBoxinfo
            // 
            this.textBoxinfo.Location = new System.Drawing.Point(561, 62);
            this.textBoxinfo.Multiline = true;
            this.textBoxinfo.Name = "textBoxinfo";
            this.textBoxinfo.ReadOnly = true;
            this.textBoxinfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxinfo.Size = new System.Drawing.Size(567, 572);
            this.textBoxinfo.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1140, 669);
            this.Controls.Add(this.textBoxinfo);
            this.Controls.Add(this.buttontab);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Modbus Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timersend;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox textBoxip;
        private System.Windows.Forms.TextBox textBoxport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttondisconn;
        private System.Windows.Forms.Button buttonconn;
        private System.Windows.Forms.RadioButton radioButtontcp;
        private System.Windows.Forms.RadioButton radioButtonrtu;
        private System.Windows.Forms.RadioButton radioButtonudp;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxcom;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxcode;
        private System.Windows.Forms.Button buttonsend;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxcode;
        private System.Windows.Forms.TextBox textBoxreceive;
        private System.Windows.Forms.TextBox textBoxfunc;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxadd;
        private System.Windows.Forms.TextBox textBoxvalue;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonexit;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttontab;
        private System.Windows.Forms.Button buttonexit2;
        private System.Windows.Forms.Button buttondo;
        private System.Windows.Forms.Button buttoncreate;
        private System.Windows.Forms.Button buttonload;
        public System.Windows.Forms.TextBox textBoxinfo;
        private System.Windows.Forms.TextBox textBoxconfig;
    }
}

