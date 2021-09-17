namespace MyKakaotalk
{
    partial class Friend
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_chat = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label_state = new System.Windows.Forms.Label();
            this.label_id = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_ip = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_chat
            // 
            this.button_chat.Font = new System.Drawing.Font("幼圆", 9F);
            this.button_chat.Location = new System.Drawing.Point(307, 34);
            this.button_chat.Name = "button_chat";
            this.button_chat.Size = new System.Drawing.Size(82, 44);
            this.button_chat.TabIndex = 0;
            this.button_chat.Text = "和TA聊天";
            this.button_chat.UseVisualStyleBackColor = true;
            this.button_chat.Click += new System.EventHandler(this.button_chat_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(16, 85);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(59, 19);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "选中";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(16, 4);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(53, 15);
            this.label.TabIndex = 2;
            this.label.Text = "好友ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(100, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "在线状态";
            // 
            // label_state
            // 
            this.label_state.AutoSize = true;
            this.label_state.Location = new System.Drawing.Point(112, 45);
            this.label_state.Name = "label_state";
            this.label_state.Size = new System.Drawing.Size(0, 15);
            this.label_state.TabIndex = 4;
            // 
            // label_id
            // 
            this.label_id.Location = new System.Drawing.Point(6, 41);
            this.label_id.Name = "label_id";
            this.label_id.Size = new System.Drawing.Size(100, 23);
            this.label_id.TabIndex = 5;
            this.label_id.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(199, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "IP地址";
            // 
            // label_ip
            // 
            this.label_ip.Location = new System.Drawing.Point(184, 41);
            this.label_ip.Name = "label_ip";
            this.label_ip.Size = new System.Drawing.Size(117, 30);
            this.label_ip.TabIndex = 7;
            this.label_ip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Friend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_ip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_id);
            this.Controls.Add(this.label_state);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button_chat);
            this.Name = "Friend";
            this.Size = new System.Drawing.Size(392, 117);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_chat;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label_state;
        private System.Windows.Forms.Label label_id;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label_ip;
    }
}
