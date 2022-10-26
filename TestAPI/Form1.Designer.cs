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
            this.liveCodeTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.settingsGb = new System.Windows.Forms.GroupBox();
            this.liveKeyTxt = new System.Windows.Forms.TextBox();
            this.connectBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.userNameTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.liveUrlTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.receiveTxt = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.settingsGb.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // liveCodeTxt
            // 
            this.liveCodeTxt.Enabled = false;
            this.liveCodeTxt.Location = new System.Drawing.Point(12, 33);
            this.liveCodeTxt.Multiline = true;
            this.liveCodeTxt.Name = "liveCodeTxt";
            this.liveCodeTxt.Size = new System.Drawing.Size(363, 405);
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
            this.settingsGb.Controls.Add(this.liveKeyTxt);
            this.settingsGb.Controls.Add(this.connectBtn);
            this.settingsGb.Controls.Add(this.label4);
            this.settingsGb.Controls.Add(this.userNameTxt);
            this.settingsGb.Controls.Add(this.label3);
            this.settingsGb.Controls.Add(this.liveUrlTxt);
            this.settingsGb.Controls.Add(this.label2);
            this.settingsGb.Location = new System.Drawing.Point(397, 33);
            this.settingsGb.Name = "settingsGb";
            this.settingsGb.Size = new System.Drawing.Size(391, 182);
            this.settingsGb.TabIndex = 2;
            this.settingsGb.TabStop = false;
            this.settingsGb.Text = "Settings";
            // 
            // liveKeyTxt
            // 
            this.liveKeyTxt.Location = new System.Drawing.Point(88, 109);
            this.liveKeyTxt.Name = "liveKeyTxt";
            this.liveKeyTxt.ReadOnly = true;
            this.liveKeyTxt.Size = new System.Drawing.Size(281, 23);
            this.liveKeyTxt.TabIndex = 5;
            // 
            // connectBtn
            // 
            this.connectBtn.Location = new System.Drawing.Point(160, 145);
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
            this.label4.Location = new System.Drawing.Point(29, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "Live Key:";
            // 
            // userNameTxt
            // 
            this.userNameTxt.Location = new System.Drawing.Point(88, 67);
            this.userNameTxt.Name = "userNameTxt";
            this.userNameTxt.Size = new System.Drawing.Size(281, 23);
            this.userNameTxt.TabIndex = 3;
            this.userNameTxt.Text = "Test_user";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "User Name:";
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
            this.groupBox1.Location = new System.Drawing.Point(397, 277);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(391, 161);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Recive API Data:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.settingsGb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.liveCodeTxt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CIARE Live Share API Test";
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
        private TextBox liveKeyTxt;
        private Button connectBtn;
        private Label label4;
        private TextBox userNameTxt;
        private Label label3;
        private TextBox liveUrlTxt;
        private Label label2;
        private TextBox receiveTxt;
        private GroupBox groupBox1;
    }
}