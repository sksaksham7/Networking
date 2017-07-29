namespace Le_Chat_Client
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.messageBox = new System.Windows.Forms.TextBox();
            this.sendTextMessage = new System.Windows.Forms.Button();
            this.sendMP4 = new System.Windows.Forms.Button();
            this.bps = new System.Windows.Forms.Label();
            this.time = new System.Windows.Forms.Label();
            this.senMp3 = new System.Windows.Forms.Button();
            this.bpsMp3 = new System.Windows.Forms.Label();
            this.timeMp3 = new System.Windows.Forms.Label();
            this.sendTxt = new System.Windows.Forms.Button();
            this.bpsTxt = new System.Windows.Forms.Label();
            this.timeTxt = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // messageBox
            // 
            this.messageBox.Location = new System.Drawing.Point(12, 37);
            this.messageBox.Name = "messageBox";
            this.messageBox.Size = new System.Drawing.Size(347, 20);
            this.messageBox.TabIndex = 0;
            // 
            // sendTextMessage
            // 
            this.sendTextMessage.Location = new System.Drawing.Point(365, 35);
            this.sendTextMessage.Name = "sendTextMessage";
            this.sendTextMessage.Size = new System.Drawing.Size(75, 23);
            this.sendTextMessage.TabIndex = 1;
            this.sendTextMessage.Text = "Send Text";
            this.sendTextMessage.UseVisualStyleBackColor = true;
            this.sendTextMessage.Click += new System.EventHandler(this.sendTextMessage_Click);
            // 
            // sendMP4
            // 
            this.sendMP4.Location = new System.Drawing.Point(365, 79);
            this.sendMP4.Name = "sendMP4";
            this.sendMP4.Size = new System.Drawing.Size(75, 23);
            this.sendMP4.TabIndex = 2;
            this.sendMP4.Text = "Send MP4";
            this.sendMP4.UseVisualStyleBackColor = true;
            this.sendMP4.Click += new System.EventHandler(this.sendMP4_Click);
            // 
            // bps
            // 
            this.bps.AutoSize = true;
            this.bps.Location = new System.Drawing.Point(12, 79);
            this.bps.Name = "bps";
            this.bps.Size = new System.Drawing.Size(91, 13);
            this.bps.TabIndex = 3;
            this.bps.Text = "Bytes per Second";
            // 
            // time
            // 
            this.time.AutoSize = true;
            this.time.Location = new System.Drawing.Point(145, 79);
            this.time.Name = "time";
            this.time.Size = new System.Drawing.Size(30, 13);
            this.time.TabIndex = 4;
            this.time.Text = "Time";
            // 
            // senMp3
            // 
            this.senMp3.Location = new System.Drawing.Point(365, 128);
            this.senMp3.Name = "senMp3";
            this.senMp3.Size = new System.Drawing.Size(75, 23);
            this.senMp3.TabIndex = 2;
            this.senMp3.Text = "Send MP3";
            this.senMp3.UseVisualStyleBackColor = true;
            this.senMp3.Click += new System.EventHandler(this.senMp3_Click);
            // 
            // bpsMp3
            // 
            this.bpsMp3.AutoSize = true;
            this.bpsMp3.Location = new System.Drawing.Point(12, 128);
            this.bpsMp3.Name = "bpsMp3";
            this.bpsMp3.Size = new System.Drawing.Size(91, 13);
            this.bpsMp3.TabIndex = 3;
            this.bpsMp3.Text = "Bytes per Second";
            // 
            // timeMp3
            // 
            this.timeMp3.AutoSize = true;
            this.timeMp3.Location = new System.Drawing.Point(145, 128);
            this.timeMp3.Name = "timeMp3";
            this.timeMp3.Size = new System.Drawing.Size(30, 13);
            this.timeMp3.TabIndex = 4;
            this.timeMp3.Text = "Time";
            // 
            // sendTxt
            // 
            this.sendTxt.Location = new System.Drawing.Point(366, 174);
            this.sendTxt.Name = "sendTxt";
            this.sendTxt.Size = new System.Drawing.Size(75, 23);
            this.sendTxt.TabIndex = 2;
            this.sendTxt.Text = "Send TXT";
            this.sendTxt.UseVisualStyleBackColor = true;
            this.sendTxt.Click += new System.EventHandler(this.sendTxt_Click);
            // 
            // bpsTxt
            // 
            this.bpsTxt.AutoSize = true;
            this.bpsTxt.Location = new System.Drawing.Point(13, 174);
            this.bpsTxt.Name = "bpsTxt";
            this.bpsTxt.Size = new System.Drawing.Size(91, 13);
            this.bpsTxt.TabIndex = 3;
            this.bpsTxt.Text = "Bytes per Second";
            // 
            // timeTxt
            // 
            this.timeTxt.AutoSize = true;
            this.timeTxt.Location = new System.Drawing.Point(146, 174);
            this.timeTxt.Name = "timeTxt";
            this.timeTxt.Size = new System.Drawing.Size(30, 13);
            this.timeTxt.TabIndex = 4;
            this.timeTxt.Text = "Time";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(16, 12);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(151, 17);
            this.radioButton1.TabIndex = 5;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Compare with compression";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 205);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.timeTxt);
            this.Controls.Add(this.timeMp3);
            this.Controls.Add(this.time);
            this.Controls.Add(this.bpsTxt);
            this.Controls.Add(this.bpsMp3);
            this.Controls.Add(this.bps);
            this.Controls.Add(this.sendTxt);
            this.Controls.Add(this.senMp3);
            this.Controls.Add(this.sendMP4);
            this.Controls.Add(this.sendTextMessage);
            this.Controls.Add(this.messageBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox messageBox;
        private System.Windows.Forms.Button sendTextMessage;
        private System.Windows.Forms.Button sendMP4;
        private System.Windows.Forms.Label bps;
        private System.Windows.Forms.Label time;
        private System.Windows.Forms.Button senMp3;
        private System.Windows.Forms.Label bpsMp3;
        private System.Windows.Forms.Label timeMp3;
        private System.Windows.Forms.Button sendTxt;
        private System.Windows.Forms.Label bpsTxt;
        private System.Windows.Forms.Label timeTxt;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}

