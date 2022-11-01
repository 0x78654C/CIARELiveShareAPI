namespace TestAPI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.liveCodeTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.settingsGb = new System.Windows.Forms.GroupBox();
            this.myPasswordTxt = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.startShareBtn = new System.Windows.Forms.Button();
            this.remoteIdTxt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.sessionIdTxt = new System.Windows.Forms.TextBox();
            this.connectBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.passwordTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.liveUrlTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.receiveTxt = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.updateLiveCode = new System.Windows.Forms.Timer(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.editorWrite = new System.Windows.Forms.Timer(this.components);
            this.settingsGb.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // liveCodeTxt
            // 
            this.liveCodeTxt.Location = new System.Drawing.Point(12, 33);
            this.liveCodeTxt.Multiline = true;
            this.liveCodeTxt.Name = "liveCodeTxt";
            this.liveCodeTxt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.liveCodeTxt.Size = new System.Drawing.Size(603, 469);
            this.liveCodeTxt.TabIndex = 0;
            this.liveCodeTxt.TextChanged += new System.EventHandler(this.liveCodeTxt_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Live Code:";
            // 
            // settingsGb
            // 
            this.settingsGb.Controls.Add(this.myPasswordTxt);
            this.settingsGb.Controls.Add(this.label6);
            this.settingsGb.Controls.Add(this.startShareBtn);
            this.settingsGb.Controls.Add(this.remoteIdTxt);
            this.settingsGb.Controls.Add(this.label5);
            this.settingsGb.Controls.Add(this.sessionIdTxt);
            this.settingsGb.Controls.Add(this.connectBtn);
            this.settingsGb.Controls.Add(this.label4);
            this.settingsGb.Controls.Add(this.passwordTxt);
            this.settingsGb.Controls.Add(this.label3);
            this.settingsGb.Controls.Add(this.liveUrlTxt);
            this.settingsGb.Controls.Add(this.label2);
            this.settingsGb.Location = new System.Drawing.Point(621, 33);
            this.settingsGb.Name = "settingsGb";
            this.settingsGb.Size = new System.Drawing.Size(391, 273);
            this.settingsGb.TabIndex = 2;
            this.settingsGb.TabStop = false;
            this.settingsGb.Text = "Settings";
            // 
            // myPasswordTxt
            // 
            this.myPasswordTxt.Location = new System.Drawing.Point(88, 107);
            this.myPasswordTxt.Name = "myPasswordTxt";
            this.myPasswordTxt.PasswordChar = '*';
            this.myPasswordTxt.Size = new System.Drawing.Size(281, 23);
            this.myPasswordTxt.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 15);
            this.label6.TabIndex = 9;
            this.label6.Text = "Password:";
            // 
            // startShareBtn
            // 
            this.startShareBtn.Location = new System.Drawing.Point(88, 147);
            this.startShareBtn.Name = "startShareBtn";
            this.startShareBtn.Size = new System.Drawing.Size(75, 23);
            this.startShareBtn.TabIndex = 8;
            this.startShareBtn.Text = "Start Share";
            this.startShareBtn.UseVisualStyleBackColor = true;
            this.startShareBtn.Click += new System.EventHandler(this.startShareBtn_Click);
            // 
            // remoteIdTxt
            // 
            this.remoteIdTxt.Location = new System.Drawing.Point(88, 187);
            this.remoteIdTxt.Name = "remoteIdTxt";
            this.remoteIdTxt.Size = new System.Drawing.Size(281, 23);
            this.remoteIdTxt.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 190);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 15);
            this.label5.TabIndex = 6;
            this.label5.Text = "R Session ID:";
            // 
            // sessionIdTxt
            // 
            this.sessionIdTxt.Location = new System.Drawing.Point(88, 65);
            this.sessionIdTxt.Name = "sessionIdTxt";
            this.sessionIdTxt.ReadOnly = true;
            this.sessionIdTxt.Size = new System.Drawing.Size(281, 23);
            this.sessionIdTxt.TabIndex = 5;
            // 
            // connectBtn
            // 
            this.connectBtn.Location = new System.Drawing.Point(294, 147);
            this.connectBtn.Name = "connectBtn";
            this.connectBtn.Size = new System.Drawing.Size(75, 23);
            this.connectBtn.TabIndex = 3;
            this.connectBtn.Text = "Connect";
            this.connectBtn.UseVisualStyleBackColor = true;
            this.connectBtn.Click += new System.EventHandler(this.connectBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "Session ID:";
            // 
            // passwordTxt
            // 
            this.passwordTxt.Location = new System.Drawing.Point(88, 227);
            this.passwordTxt.Name = "passwordTxt";
            this.passwordTxt.PasswordChar = '*';
            this.passwordTxt.Size = new System.Drawing.Size(281, 23);
            this.passwordTxt.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 230);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "R Password:";
            // 
            // liveUrlTxt
            // 
            this.liveUrlTxt.Location = new System.Drawing.Point(88, 25);
            this.liveUrlTxt.Name = "liveUrlTxt";
            this.liveUrlTxt.Size = new System.Drawing.Size(281, 23);
            this.liveUrlTxt.TabIndex = 1;
            this.liveUrlTxt.Text = "https://localhost:7142/live";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Live API URL:";
            // 
            // receiveTxt
            // 
            this.receiveTxt.Location = new System.Drawing.Point(14, 22);
            this.receiveTxt.Multiline = true;
            this.receiveTxt.Name = "receiveTxt";
            this.receiveTxt.Size = new System.Drawing.Size(371, 123);
            this.receiveTxt.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.receiveTxt);
            this.groupBox1.Location = new System.Drawing.Point(621, 341);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(391, 161);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Recive API Data:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(807, 312);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Disconnect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // updateLiveCode
            // 
            this.updateLiveCode.Tick += new System.EventHandler(this.updateLiveCode_Tick);
            // 
            // editorWrite
            // 
            this.editorWrite.Interval = 600;
            this.editorWrite.Tick += new System.EventHandler(this.editorWrite_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 517);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.settingsGb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.liveCodeTxt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CIARE Live Share API Test";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.settingsGb.ResumeLayout(false);
            this.settingsGb.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox liveCodeTxt;
        private Label label1;
        private GroupBox settingsGb;
        private TextBox sessionIdTxt;
        private Button connectBtn;
        private Label label4;
        private TextBox passwordTxt;
        private Label label3;
        private TextBox liveUrlTxt;
        private Label label2;
        private TextBox receiveTxt;
        private GroupBox groupBox1;
        private Button button1;
        private System.Windows.Forms.Timer updateLiveCode;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer editorWrite;
        private TextBox remoteIdTxt;
        private Label label5;
        private Button startShareBtn;
        private TextBox myPasswordTxt;
        private Label label6;
    }
}