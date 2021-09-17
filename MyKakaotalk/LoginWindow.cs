using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MyKakaotalk
{
    public partial class LoginWindow : Form
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //建立到服务器的连接
            IPAddress server_ip = IPAddress.Parse("166.111.140.57");
            IPEndPoint iep = new IPEndPoint(server_ip, 8000);
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                client.Connect(iep);
            }
            catch(SocketException)
            {
                MessageBox.Show(this, "无法连接到服务器!请确认服务器或检查你的网络连接。", "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //连接完成后 发送登录信息

            //建立登录字符串
            string username = textBox1.Text;
            string password = textBox2.Text;
            string login_message = username + "_" + password;

            //发送字符串
            client.Send(Encoding.ASCII.GetBytes(login_message));

            //接受返回信息
            int data_number = 0;
            byte [] data = new byte[1024];
            string receive_message;

            data_number = client.Receive(data);
            receive_message = Encoding.ASCII.GetString(data,0,data_number);
            if (receive_message == "lol")
            {
                MessageBox.Show(this, "登录成功！", "登录成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "登录失败！请检查用户名和密码！", "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;//返回，直至输入正确的用户名和密码
            }
            //登录成功，进入主窗口
            Thread main_thread = new Thread(() => Application.Run(new MainWindow(username, client)));
            main_thread.Start();
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
