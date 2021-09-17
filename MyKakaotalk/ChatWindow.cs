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
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace MyKakaotalk
{
    public partial class ChatWindow : Form
    {
        public ChatWindow(string myid,string chatters_id, Socket[] obj,int _nums)
        {
            InitializeComponent();
            this.Text = chatters_id;
            MyID = myid;
            friends_ID = chatters_id;
            links_in_chat = obj;
            connect_nums = _nums;
            Operate_Chat_History();
            AsynReceive(links_in_chat);

        }

        public FileStream Chat_history = null;
        public string MyID;
        public string friends_ID;
        Socket[] links_in_chat;
        int connect_nums;
        Color my_text_color = Color.Black;
        bool show_textbox_inprocessing = false;
        bool close_flag = false;//主动下线时为false,收到其他人下线消息时为true

        //聊天记录的读取和创建
        public void Operate_Chat_History()
        {
            if (!File.Exists(MyID + "/" + this.Text + ".txt"))//保存聊天记录
            {
                FileStream fs1 = new FileStream(MyID + "/" + this.Text + ".txt", FileMode.Create, FileAccess.Write);
                Chat_history = fs1;
            }
            else
            {
                
                StreamReader sr = new StreamReader(MyID + "/" + this.Text + ".txt", Encoding.UTF8);
                String line;
                while ((line = sr.ReadLine()) != null)
                {

                    this.Message_Display_inbox(line + "\n", my_text_color, HorizontalAlignment.Center);

                    
                }
                sr.Close();
                FileStream fileStream = new FileStream(MyID + "/" + this.Text + ".txt", FileMode.Open, FileAccess.Write);
                Chat_history = fileStream;
            }

        }
        
        //点击发送消息键
        private void button_send_Click(object sender, EventArgs e)
        {
            try
            {
                string message = richTextBox_send.Text;
                while (show_textbox_inprocessing) { }
                show_textbox_inprocessing = true;//占上
                string to_screen = MyID + "   " + DateTime.Now.ToString() + "\n" + message + "\n";
                RichBox_Show rbs = new RichBox_Show(Message_Display_inbox);
                this.Invoke(rbs, new object[] { to_screen, my_text_color, HorizontalAlignment.Right });
                byte[] msg = Encoding.UTF8.GetBytes(to_screen);
                Chat_history.Write(msg, 0, msg.Length);
                show_textbox_inprocessing = false;
                foreach (Socket inchat_client in links_in_chat)
                {
                    if (inchat_client == null) break;
                    AsyncSend(inchat_client, to_screen);
                }
                richTextBox_send.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "发送失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
        }

        //点击发送文件键
        private void button_file_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)//选择文件
            {
                string filename = openFile.FileName;
                byte[] data = Encoding.UTF8.GetBytes(">-----file---transfering-----<");
                FileStream fsRead = new FileStream(filename, FileMode.Open, FileAccess.Read);
                long fileLength = fsRead.Length;//获取文件长度
                if (fileLength >= 9000000/8)
                {
                    MessageBox.Show(this, "文件超出上限", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    MessageBox.Show(this, fileLength.ToString(), "提示");
                    fsRead.Close();
                }
                   

                //向当前聊天里的所有对象发送
                //byte[] f_name = Encoding.UTF8.GetBytes("\n" + filename);//先传输文件名字
                foreach (Socket inchat_Client in links_in_chat)
                {
                    if (inchat_Client == null)
                        break;
                    inchat_Client.Send(data);
                    inchat_Client.SendFile(filename, null, null, TransmitFileOptions.UseDefaultWorkerThread);
                }
                MessageBox.Show(this, "文件 " + filename + " 传输成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.None);
                //string str_post1 = DateTime.Now.ToString()+"\n";
                //while (show_textbox_inprocessing) { };
                //show_textbox_inprocessing = true;
                //RichBox_Show rbs = new RichBox_Show(Message_Display_inbox);
                //this.Invoke(rbs, new object[] {str_post1+"发送了文件。"+"\n", my_text_color, HorizontalAlignment.Right });
                //show_textbox_inprocessing = false;
                //string str_post2 = str_post1 + MyID + " 发送了文件。" + "\n";
                //foreach (Socket inchat_client in links_in_chat)
                //{
                //    if (inchat_client == null) break;
                //    AsyncSend(inchat_client,str_post2);
                //}

            }
            else
            {
                MessageBox.Show(this, "未选择文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        //文件接收
        private delegate void f_receive(Socket File_socket);
        private ManualResetEvent Finished = new ManualResetEvent(false);
        private void FileReceive(Socket File_socket)
        {
            DialogResult Receive_dialog = MessageBox.Show("好友发送了文件，是否接收？", "文件请求", MessageBoxButtons.OKCancel);

            if (Receive_dialog == DialogResult.OK)//同意接收文件
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Title = "选择保存的路径";
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    string Path = saveFile.FileName;
                    FileStream filestream = new FileStream(Path, FileMode.Create, FileAccess.Write);
                    int bytes_num_All = 0;
                    int buff_size = 90000000;
                    byte[] buff = new byte[buff_size];
                    int bytes_num_Received = 0;
                    while (true)
                    {
                        bytes_num_Received = File_socket.Receive(buff, buff_size, SocketFlags.None);//存入一batch进入buffer
                        
                        string string_send = Encoding.UTF8.GetString(buff, 0, bytes_num_Received);//将这个batch解码为字符串
                        foreach (Socket inchat_Client in links_in_chat)
                        {
                            if (inchat_Client == null) break;
                            if (inchat_Client != File_socket)//不是当前用户
                            {
                                //AsyncSend(inchat_Client, ">-----file---transfering-----<");
                                AsyncSend(inchat_Client, string_send);
                            }
                        }
                        filestream.Write(buff, bytes_num_All, bytes_num_Received);
                        filestream.Flush();//将所有缓冲区的数据都写入文件
                        bytes_num_All += bytes_num_Received;//计数已接受的数据量，作为offset使用
                        if (bytes_num_Received < buff_size)//最后一个batch了
                        {
                            break;
                        }
                    }
                    filestream.Close();//关闭文件流
                    MessageBox.Show(Path + "接收完成", "提示");
                }
                else
                {
                    //接收文件但未保存
                    //只转发给其他用户，不存不写。
                    int bytes_num_All = 0;
                    int buff_size = 90000000;
                    byte[] buff = new byte[buff_size];
                    int bytes_num_Received = 0;
                    while (true)
                    {
                        bytes_num_Received = File_socket.Receive(buff, buff_size, SocketFlags.None);//存入一batch进入buffer
                        string string_send = Encoding.UTF8.GetString(buff, 0, bytes_num_Received);//将这个batch解码为字符串
                        foreach (Socket inchat_Client in links_in_chat)
                        {
                            if (inchat_Client == null) break;
                            if (inchat_Client != File_socket)//不是当前用户
                                AsyncSend(inchat_Client, string_send);
                        }
                        bytes_num_All += bytes_num_Received;//计数已接受的数据量，作为offset使用
                        if (bytes_num_Received < buff_size)//最后一个batch了
                        {
                            break;
                        }
                    }
                    //复制粘贴上面的部分，把有关文件流的部分去掉。
                }//endelse
            }
            else if (Receive_dialog == DialogResult.Cancel)
            {
                //不同意接收文件
                //也是只转发
                int bytes_num_All = 0;
                int buff_size = 90000000;
                byte[] buff = new byte[buff_size];
                int bytes_num_Received = 0;
                while (true)
                {
                    bytes_num_Received = File_socket.Receive(buff, buff_size, SocketFlags.None);//存入一batch进入buffer
                    string string_send = Encoding.UTF8.GetString(buff, 0, bytes_num_Received);//将这个batch解码为字符串
                    foreach (Socket inchat_Client in links_in_chat)
                    {
                        if (inchat_Client == null) break;
                        if (inchat_Client != File_socket)//不是当前用户
                            AsyncSend(inchat_Client, string_send);
                    }
                    bytes_num_All += bytes_num_Received;//计数已接受的数据量，作为offset使用
                    if (bytes_num_Received < buff_size)//最后一个batch了
                    {
                        break;
                    }
                }
            }
            Finished.Set();
        }


        //异步接收消息
        private void AsynReceive(Socket[] links)
        {
            byte[] data = new byte[1024];
            try
            {
                foreach (Socket inchat_Client in links)
                {
                    if (inchat_Client == null)
                        break;
                    inchat_Client.BeginReceive(data,0,data.Length,SocketFlags.None,
                    asyncResult=>
                    {
                        int len = 0;
                        try
                        {
                            len = inchat_Client.EndReceive(asyncResult);
                            string msg_received = Encoding.UTF8.GetString(data, 0, len);
                            if (len == 0)//接收到的长度为0
                            {
                                MessageBox.Show(this,"对方已下线", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.button_file.Visible = false;
                                this.button_send.Visible = false;
                                this.Close();
                                return;
                            }
                            //群聊时，向其他用户转发消息
                            //发起群聊时，只发送了本人的id和ip，所以接收者们不会再对消息转发
                            foreach (Socket others_inchat in links)
                            {
                                if (others_inchat == null)
                                    break;
                                if (others_inchat != inchat_Client)
                                    AsyncSend(others_inchat, msg_received);//转发信息给其他用户
                            }
                            if (msg_received == ">-----file---transfering-----<")
                            {
                                Finished.Reset();
                                f_receive f_Receive = new f_receive(FileReceive);
                                this.Invoke(f_Receive, new object[] { inchat_Client });
                                Finished.WaitOne();
                            }
                            else if (msg_received== "---Exiting---")
                            {
                                MessageBox.Show(this,"对方已下线", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.button_file.Visible = false;
                                this.button_send.Visible = false;
                                close_flag = true;
                                this.Close();
                                return;
                            }
                            else
                            {
                                while (show_textbox_inprocessing) { };//如果显示文本框被占用，则等待使用完毕
                                show_textbox_inprocessing = true;//占上

                                RichBox_Show rbs = new RichBox_Show(Message_Display_inbox);//显示消息
                                this.Invoke(rbs, new object[] { msg_received, Color.Black, HorizontalAlignment.Left }); //接收到的消息显示在左侧
                                byte[] msg = Encoding.UTF8.GetBytes(msg_received);
                                Chat_history.Write(msg, 0, msg.Length);
                                show_textbox_inprocessing = false;

                            }
                            AsynReceive(links);//继续接收消息
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                MessageBox.Show(this, ex.ToString(), "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            catch(Exception a)
                            {

                            }
                        }
                    },null);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
        }

        //异步发送消息
        public void AsyncSend(Socket client,string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.BeginSend(data, 0, data.Length, SocketFlags.None, asyncResult =>
                {
                    try
                    {
                        int length = client.EndSend(asyncResult);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(this, e.ToString(), "发送错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }, null);
        }

        //设置显示到聊天框中的文字，颜色，对齐方式。
        private delegate void RichBox_Show(string message, Color color, HorizontalAlignment horizontalAlignment);
        public void Message_Display_inbox(string message, Color color, HorizontalAlignment horizontalAlignment)
        {
            richTextBox_display.SelectionColor = color;
            richTextBox_display.SelectionAlignment = horizontalAlignment;
            richTextBox_display.AppendText(message);
           
        }



        //Socket相当于门，聊完要关门的
        private void ChatWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (close_flag == false)
            {
                foreach (Socket client_in_chat in links_in_chat)
                {
                    if (client_in_chat == null) break;
                    string message = "---Exiting---";
                    try
                    {
                        AsyncSend(client_in_chat, message);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            Chat_history.Close();
        }





    }
}
