using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace MyKakaotalk
{
    public partial class Friend : UserControl
    {
        public Friend(string id,bool state0,string ip,string Myid,int _num)
        {
            InitializeComponent();
            LocalID = id;
            LocalIP = ip;
            state_refresh(state0);
            this.label_id.Text = LocalID;
            this.label_ip.Text = LocalIP;
            check_state = false;
            MyID = Myid;
            Mynum = _num;
            
        }
        public string MyID;//当前用户的ID
        public string LocalID;//好友的ID
        public string LocalIP;//好友的IP地址
        public bool check_state;
        public int Mynum;
        
        public void state_refresh(bool state)
        {
            if (state == true)
                this.label_state.Text = "[在线]";
            else
                this.label_state.Text = "[离线]";
        }

        public void ip_refresh()
        {
            this.label_ip.Text = LocalIP;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                check_state = true;
            else
                check_state = false;
        }


        private void button_chat_Click(object sender, EventArgs e)
        {
            if (this.label_state.Text == "[离线]")
            {
                MessageBox.Show(this, "您聊天的对象不在线！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (MyID == LocalID)
            {
                MessageBox.Show(this, "不可以和自己聊天喔！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //根据当前Item的ip,id建立连接，端口号与主函数端口号设计相同，学号后四位+50000
                int LocalPort = 50000 + int.Parse(LocalID.Substring(LocalID.Length - 4));
                string ip = LocalIP;
                bool connected;
                Socket[] sockets = new Socket[1];
                IPEndPoint chat_ip = new IPEndPoint(IPAddress.Parse(ip), LocalPort);
                sockets[0] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    sockets[0].Connect(chat_ip);
                    byte[] data = Encoding.UTF8.GetBytes(MyID);
                    sockets[0].Send(data);
                    connected = true;
                }
                catch (Exception)
                {
                    MessageBox.Show(this, "无法建立连接！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (connected == true)
                {
                    try
                    {
                        Thread Thread_chat = new Thread(() => Application.Run(new ChatWindow(MyID, LocalID, sockets, 1)));
                        Thread_chat.SetApartmentState(System.Threading.ApartmentState.STA);
                        Thread_chat.Start();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "聊天发起失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show(this, "聊天发起失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }

        }
        //建立连接的函数
        private Socket chat_setup()
        {
            //根据当前Item的ip,id建立连接，端口号与主函数端口号设计相同，学号后四位+50000
            int LocalPort = 50000 + int.Parse(LocalID.Substring(LocalID.Length - 4));
            string ip = LocalIP;
            IPEndPoint chat_ip = new IPEndPoint(IPAddress.Parse(ip), LocalPort);
            Socket chat_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                chat_socket.Connect(chat_ip);
                byte[] data = Encoding.UTF8.GetBytes(MyID);
                chat_socket.Send(data);
            }
            catch(Exception)
            {
                MessageBox.Show(this, "好友状态有误，请刷新", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);               
            }
            return chat_socket;
        }

    }
}
