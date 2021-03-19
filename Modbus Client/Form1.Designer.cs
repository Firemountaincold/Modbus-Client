
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
            this.buttonexit = new System.Windows.Forms.Button();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttondisconn = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.textBoxinfo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxfunc = new System.Windows.Forms.TextBox();
            this.textBoxreceive = new System.Windows.Forms.TextBox();
            this.textBoxip = new System.Windows.Forms.TextBox();
            this.textBoxcode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonsend = new System.Windows.Forms.Button();
            this.comboBoxcode = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonconn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxport = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxcom = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.radioButtonudp = new System.Windows.Forms.RadioButton();
            this.radioButtonrtu = new System.Windows.Forms.RadioButton();
            this.radioButtontcp = new System.Windows.Forms.RadioButton();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timersend
            // 
            this.timersend.Tick += new System.EventHandler(this.timersend_Tick);
            // 
            // buttonexit
            // 
            this.buttonexit.Location = new System.Drawing.Point(243, 538);
            this.buttonexit.Name = "buttonexit";
            this.buttonexit.Size = new System.Drawing.Size(132, 40);
            this.buttonexit.TabIndex = 10;
            this.buttonexit.Text = "退出";
            this.buttonexit.UseVisualStyleBackColor = true;
            this.buttonexit.Click += new System.EventHandler(this.buttonexit_Click);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(84, 20);
            this.toolStripStatusLabel1.Text = "欢迎使用！";
            // 
            // buttondisconn
            // 
            this.buttondisconn.Enabled = false;
            this.buttondisconn.Location = new System.Drawing.Point(48, 195);
            this.buttondisconn.Name = "buttondisconn";
            this.buttondisconn.Size = new System.Drawing.Size(132, 40);
            this.buttondisconn.TabIndex = 11;
            this.buttondisconn.Text = "断开连接";
            this.buttondisconn.UseVisualStyleBackColor = true;
            this.buttondisconn.Click += new System.EventHandler(this.buttondisconn_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 591);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(897, 26);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // textBoxinfo
            // 
            this.textBoxinfo.Location = new System.Drawing.Point(448, 7);
            this.textBoxinfo.Multiline = true;
            this.textBoxinfo.Name = "textBoxinfo";
            this.textBoxinfo.ReadOnly = true;
            this.textBoxinfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxinfo.Size = new System.Drawing.Size(437, 571);
            this.textBoxinfo.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 215);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 15);
            this.label6.TabIndex = 8;
            this.label6.Text = "返回：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "功能：";
            // 
            // textBoxfunc
            // 
            this.textBoxfunc.Location = new System.Drawing.Point(72, 128);
            this.textBoxfunc.Multiline = true;
            this.textBoxfunc.Name = "textBoxfunc";
            this.textBoxfunc.ReadOnly = true;
            this.textBoxfunc.Size = new System.Drawing.Size(330, 65);
            this.textBoxfunc.TabIndex = 6;
            // 
            // textBoxreceive
            // 
            this.textBoxreceive.Location = new System.Drawing.Point(72, 204);
            this.textBoxreceive.Multiline = true;
            this.textBoxreceive.Name = "textBoxreceive";
            this.textBoxreceive.Size = new System.Drawing.Size(331, 42);
            this.textBoxreceive.TabIndex = 5;
            // 
            // textBoxip
            // 
            this.textBoxip.Location = new System.Drawing.Point(80, 29);
            this.textBoxip.Name = "textBoxip";
            this.textBoxip.Size = new System.Drawing.Size(331, 25);
            this.textBoxip.TabIndex = 0;
            this.textBoxip.Text = "127.0.0.1";
            // 
            // textBoxcode
            // 
            this.textBoxcode.Location = new System.Drawing.Point(73, 71);
            this.textBoxcode.Multiline = true;
            this.textBoxcode.Name = "textBoxcode";
            this.textBoxcode.Size = new System.Drawing.Size(330, 43);
            this.textBoxcode.TabIndex = 4;
            this.textBoxcode.Text = "00-00-00-00-00-00-00-00-00-00-00-00";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "数据：";
            // 
            // buttonsend
            // 
            this.buttonsend.Location = new System.Drawing.Point(292, 25);
            this.buttonsend.Name = "buttonsend";
            this.buttonsend.Size = new System.Drawing.Size(96, 34);
            this.buttonsend.TabIndex = 2;
            this.buttonsend.Text = "发送";
            this.buttonsend.UseVisualStyleBackColor = true;
            this.buttonsend.Click += new System.EventHandler(this.buttonsend_Click);
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
            this.comboBoxcode.Location = new System.Drawing.Point(81, 34);
            this.comboBoxcode.Name = "comboBoxcode";
            this.comboBoxcode.Size = new System.Drawing.Size(150, 23);
            this.comboBoxcode.TabIndex = 1;
            this.comboBoxcode.SelectedIndexChanged += new System.EventHandler(this.comboBoxcode_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "功能码：";
            // 
            // buttonconn
            // 
            this.buttonconn.Location = new System.Drawing.Point(48, 134);
            this.buttonconn.Name = "buttonconn";
            this.buttonconn.Size = new System.Drawing.Size(132, 40);
            this.buttonconn.TabIndex = 4;
            this.buttonconn.Text = "连接";
            this.buttonconn.UseVisualStyleBackColor = true;
            this.buttonconn.Click += new System.EventHandler(this.buttonconn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "端口号：";
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
            // textBoxport
            // 
            this.textBoxport.Location = new System.Drawing.Point(80, 77);
            this.textBoxport.Name = "textBoxport";
            this.textBoxport.Size = new System.Drawing.Size(100, 25);
            this.textBoxport.TabIndex = 1;
            this.textBoxport.Text = "502";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBoxfunc);
            this.groupBox2.Controls.Add(this.textBoxreceive);
            this.groupBox2.Controls.Add(this.textBoxcode);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.buttonsend);
            this.groupBox2.Controls.Add(this.comboBoxcode);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(13, 276);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(417, 256);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "功能码发送";
            // 
            // groupBox1
            // 
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
            this.groupBox1.Size = new System.Drawing.Size(417, 263);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务器信息";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(226, 82);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 15);
            this.label8.TabIndex = 17;
            this.label8.Text = "串口号：";
            // 
            // textBoxcom
            // 
            this.textBoxcom.Location = new System.Drawing.Point(297, 77);
            this.textBoxcom.Name = "textBoxcom";
            this.textBoxcom.Size = new System.Drawing.Size(100, 25);
            this.textBoxcom.TabIndex = 16;
            this.textBoxcom.Text = "COM3";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(235, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 15);
            this.label7.TabIndex = 15;
            this.label7.Text = "连接模式：";
            // 
            // radioButtonudp
            // 
            this.radioButtonudp.AutoSize = true;
            this.radioButtonudp.Location = new System.Drawing.Point(264, 224);
            this.radioButtonudp.Name = "radioButtonudp";
            this.radioButtonudp.Size = new System.Drawing.Size(108, 19);
            this.radioButtonudp.TabIndex = 14;
            this.radioButtonudp.Text = "Modbus UDP";
            this.radioButtonudp.UseVisualStyleBackColor = true;
            this.radioButtonudp.CheckedChanged += new System.EventHandler(this.radioButtonudp_CheckedChanged);
            // 
            // radioButtonrtu
            // 
            this.radioButtonrtu.AutoSize = true;
            this.radioButtonrtu.Location = new System.Drawing.Point(264, 184);
            this.radioButtonrtu.Name = "radioButtonrtu";
            this.radioButtonrtu.Size = new System.Drawing.Size(108, 19);
            this.radioButtonrtu.TabIndex = 13;
            this.radioButtonrtu.Text = "Modbus RTU";
            this.radioButtonrtu.UseVisualStyleBackColor = true;
            this.radioButtonrtu.CheckedChanged += new System.EventHandler(this.radioButtonrtu_CheckedChanged);
            // 
            // radioButtontcp
            // 
            this.radioButtontcp.AutoSize = true;
            this.radioButtontcp.Checked = true;
            this.radioButtontcp.Location = new System.Drawing.Point(264, 144);
            this.radioButtontcp.Name = "radioButtontcp";
            this.radioButtontcp.Size = new System.Drawing.Size(108, 19);
            this.radioButtontcp.TabIndex = 12;
            this.radioButtontcp.TabStop = true;
            this.radioButtontcp.Text = "Modbus TCP";
            this.radioButtontcp.UseVisualStyleBackColor = true;
            this.radioButtontcp.CheckedChanged += new System.EventHandler(this.radioButtontcp_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 617);
            this.Controls.Add(this.buttonexit);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.textBoxinfo);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Modbus Client";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timersend;
        private System.Windows.Forms.Button buttonexit;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button buttondisconn;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox textBoxinfo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxfunc;
        private System.Windows.Forms.TextBox textBoxreceive;
        private System.Windows.Forms.TextBox textBoxip;
        private System.Windows.Forms.TextBox textBoxcode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonsend;
        private System.Windows.Forms.ComboBox comboBoxcode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonconn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxport;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton radioButtonudp;
        private System.Windows.Forms.RadioButton radioButtonrtu;
        private System.Windows.Forms.RadioButton radioButtontcp;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxcom;
    }
}

