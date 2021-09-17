
namespace MyKakaotalk
{
    partial class MainWindow
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Button_query = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.Button_state_refresh = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button_groupchat = new System.Windows.Forms.Button();
            this.button_delete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("幼圆", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(44, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 40);
            this.label1.TabIndex = 2;
            this.label1.Text = "好友查询";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Bisque;
            this.textBox1.Font = new System.Drawing.Font("幼圆", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(174, 36);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(336, 27);
            this.textBox1.TabIndex = 3;
            // 
            // Button_query
            // 
            this.Button_query.BackColor = System.Drawing.Color.Bisque;
            this.Button_query.Font = new System.Drawing.Font("幼圆", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button_query.Location = new System.Drawing.Point(529, 26);
            this.Button_query.Name = "Button_query";
            this.Button_query.Size = new System.Drawing.Size(100, 45);
            this.Button_query.TabIndex = 4;
            this.Button_query.Text = "查询";
            this.Button_query.UseVisualStyleBackColor = false;
            this.Button_query.Click += new System.EventHandler(this.Button_query_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(24, 120);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.flowLayoutPanel1.Size = new System.Drawing.Size(429, 524);
            this.flowLayoutPanel1.TabIndex = 6;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // Button_state_refresh
            // 
            this.Button_state_refresh.BackColor = System.Drawing.Color.Gold;
            this.Button_state_refresh.Location = new System.Drawing.Point(24, 665);
            this.Button_state_refresh.Name = "Button_state_refresh";
            this.Button_state_refresh.Size = new System.Drawing.Size(104, 36);
            this.Button_state_refresh.TabIndex = 0;
            this.Button_state_refresh.Text = "刷新状态";
            this.Button_state_refresh.UseVisualStyleBackColor = false;
            this.Button_state_refresh.Click += new System.EventHandler(this.Button_state_refresh_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("幼圆", 9F);
            this.label2.Location = new System.Drawing.Point(21, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 36);
            this.label2.TabIndex = 7;
            this.label2.Text = "好友列表";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_groupchat
            // 
            this.button_groupchat.Location = new System.Drawing.Point(359, 665);
            this.button_groupchat.Name = "button_groupchat";
            this.button_groupchat.Size = new System.Drawing.Size(94, 36);
            this.button_groupchat.TabIndex = 8;
            this.button_groupchat.Text = "发起群聊";
            this.button_groupchat.UseVisualStyleBackColor = true;
            this.button_groupchat.Click += new System.EventHandler(this.button_groupchat_Click);
            // 
            // button_delete
            // 
            this.button_delete.Location = new System.Drawing.Point(192, 665);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(94, 36);
            this.button_delete.TabIndex = 9;
            this.button_delete.Text = "删除好友";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gold;
            this.ClientSize = new System.Drawing.Size(966, 757);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.button_groupchat);
            this.Controls.Add(this.Button_state_refresh);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.Button_query);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button Button_query;
        private System.Windows.Forms.Button Button_state_refresh;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button button_groupchat;
        private System.Windows.Forms.Button button_delete;
    }
}