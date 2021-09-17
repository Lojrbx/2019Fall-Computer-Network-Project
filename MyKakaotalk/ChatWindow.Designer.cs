namespace MyKakaotalk
{
    partial class ChatWindow
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
            this.richTextBox_send = new System.Windows.Forms.RichTextBox();
            this.button_send = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button_file = new System.Windows.Forms.Button();
            this.richTextBox_display = new System.Windows.Forms.RichTextBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox_send
            // 
            this.richTextBox_send.BackColor = System.Drawing.SystemColors.Info;
            this.richTextBox_send.Location = new System.Drawing.Point(31, 457);
            this.richTextBox_send.Name = "richTextBox_send";
            this.richTextBox_send.Size = new System.Drawing.Size(692, 144);
            this.richTextBox_send.TabIndex = 0;
            this.richTextBox_send.Text = "";
            // 
            // button_send
            // 
            this.button_send.Location = new System.Drawing.Point(613, 607);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(110, 36);
            this.button_send.TabIndex = 1;
            this.button_send.Text = "发送消息";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.button_file);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(31, 414);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(692, 37);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // button_file
            // 
            this.button_file.BackColor = System.Drawing.Color.Wheat;
            this.button_file.Location = new System.Drawing.Point(568, 3);
            this.button_file.Name = "button_file";
            this.button_file.Size = new System.Drawing.Size(121, 30);
            this.button_file.TabIndex = 0;
            this.button_file.Text = "发送文件";
            this.button_file.UseVisualStyleBackColor = false;
            this.button_file.Click += new System.EventHandler(this.button_file_Click);
            // 
            // richTextBox_display
            // 
            this.richTextBox_display.BackColor = System.Drawing.SystemColors.Info;
            this.richTextBox_display.Location = new System.Drawing.Point(31, 12);
            this.richTextBox_display.Name = "richTextBox_display";
            this.richTextBox_display.Size = new System.Drawing.Size(692, 396);
            this.richTextBox_display.TabIndex = 3;
            this.richTextBox_display.Text = "";
            // 
            // ChatWindow
            // 
            this.AcceptButton = this.button_send;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gold;
            this.ClientSize = new System.Drawing.Size(760, 655);
            this.Controls.Add(this.richTextBox_display);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.richTextBox_send);
            this.Name = "ChatWindow";
            this.Text = "ChatWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatWindow_FormClosing);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_send;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RichTextBox richTextBox_display;
        private System.Windows.Forms.Button button_file;
    }
}