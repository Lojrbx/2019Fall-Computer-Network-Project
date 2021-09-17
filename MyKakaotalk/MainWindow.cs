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
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Reflection;

namespace MyKakaotalk
{
    public partial class MainWindow : Form
    {
        public MainWindow(string username, Socket obj)
        {
            this.Text = username;
            InitializeComponent();
            host = obj;
            MyID = username;
            Load_Friend();
            StartListen();//上线开始准备接收消息
            if (!Directory.Exists(username))
                Directory.CreateDirectory(username);
            refresh();
        }
        Socket host;//当前用户
        string MyID;
        IPAddress LocalIP;
        bool refreshing = false;//表示正在更新好友状态，需要等待。


        bool isquerying = false;
        private string Start_Query(string ID)//查询好友在线状态，若在线，则添加到好友列表中
        {
            isquerying = true;
            string query_Data = "q" + ID;
            byte[] buffers_sent = Encoding.ASCII.GetBytes(query_Data);
            host.BeginSend(buffers_sent, 0, buffers_sent.Length, 0, new AsyncCallback(Sent_Query), host);//callback为完成时调用的方法。
            int return_length = 0;
            string return_data;
            byte[] bytes = new byte[1024];
            return_length = host.Receive(bytes);
            return_data = System.Text.Encoding.ASCII.GetString(bytes, 0, return_length);
            isquerying = false;
            return return_data;
        }

        private void Sent_Query(IAsyncResult asyncResult)//发送完进行的操作（回调函数）
        {
            try
            {
                host = (Socket)asyncResult.AsyncState;
                host.EndSend(asyncResult);
            }
            catch (SocketException exception)
            {
                MessageBox.Show(this, exception.ToString(), "查询错误!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        public void StartListen()//监听，等待聊天连接
        {
            IPHostEntry LocalEntry = Dns.GetHostEntry(Dns.GetHostName());//主机IP
            int i;
            for (i = 0;i!=LocalEntry.AddressList.Length;i++)
            {
                if (AddressFamily.InterNetwork == LocalEntry.AddressList[i].AddressFamily)//取IPV4地址为IP地址
                    break;
            }
            int LocalPort = 50000 + int.Parse(MyID.Substring(MyID.Length - 4));//端口号设为50000+学号后四位
            LocalIP = LocalEntry.AddressList[i];//这段是获取与主机关联的IP地址列表，找到IPV4的地址，设定为本机IP地址
            IPEndPoint ServerIP = new IPEndPoint(LocalIP, LocalPort);//设定服务器IP和端口号
            Socket TCPServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            TCPServer.Bind(ServerIP);
            TCPServer.Listen(20);
            Asyn_Accept(TCPServer);//接收到连接后的行为
        }

        //接收连接(异步)
        public void Asyn_Accept(Socket TCPServer)
        {
            TCPServer.BeginAccept(asyncResult =>
            {
                Socket TCPClient = TCPServer.EndAccept(asyncResult);//为连接建立新的Socket
                Asyn_Accept(TCPServer);
                Asyn_Receive(TCPClient);//建立连接后，开始接收消息
            }, null);
        }

        //接收连接的client消息(异步)
        public void Asyn_Receive(Socket TCPClient)
        {
            byte[] data = new byte[1024];
            try
            {
                TCPClient.BeginReceive(data, 0, data.Length, SocketFlags.None,
                    asynResult =>
                    {
                        int length = TCPClient.EndReceive(asynResult);
                        string Users_Broadcast_Received = Encoding.UTF8.GetString(data, 0, length);//对话者的ID

                        Socket[] Connect_received = new Socket[1];
                        Connect_received[0] = TCPClient;
                        Thread Thread_Chat = new Thread(() =>
                           System.Windows.Forms.Application.Run(new ChatWindow(MyID,Users_Broadcast_Received, Connect_received, 1)));
                        Thread_Chat.SetApartmentState(System.Threading.ApartmentState.STA);//设定线程状态为单线程
                        Thread_Chat.IsBackground = true;
                        Thread_Chat.Start();
                        //Asyn_Receive(TCPClient);
                    }, null);
            }
            catch(Exception ex)
                {
                MessageBox.Show(ex.ToString(), "接收聊天连接失败！", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                    
                    
            
        }

        //刷新好友列表
        public void List_refresh()
        {

            Thread th = new Thread(() =>
            {
                while (true)
                {
                    refresh();
                    Thread.Sleep(60000);
                }
            });
            th.IsBackground = true;
            th.Start();
        }


        
        //查询好友按键
        private void Button_query_Click(object sender, EventArgs e)
        {
            while (isquerying==true) { }
            string query_return = Start_Query(this.textBox1.Text);
            if (query_return == "n")//查询不在线
            {
                MessageBox.Show(this, "该用户当前不在线喔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (query_return == "Please send the correct message.")
            {
                MessageBox.Show(this, "用户不存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //MessageBox.Show(this, query_return, "提示",MessageBoxButtons.OK, MessageBoxIcon.Information);
                foreach (Friend item in this.flowLayoutPanel1.Controls)
                {
                    if (item.LocalID == textBox1.Text)
                    {
                        MessageBox.Show(this, "用户已在好友列表中", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                MessageBox.Show(this, textBox1.Text + "[在线].已添加到好友列表中", "添加成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //将好友信息添加到Excel中
                int rows = Write_Friend(textBox1.Text, query_return);
                //添加Friend控件
                Friend friendItem = new Friend(textBox1.Text, true, query_return, MyID,rows);
                friendItem.Size = new Size(324, 126);
                this.flowLayoutPanel1.Controls.Add(friendItem);
            }
        }

        



        //窗口关闭时尝试下线，下线失败则窗口不关闭。
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            while (isquerying) { };
            while (refreshing) { };
            refreshing = true;
            string logout_data = "logout" + this.MyID;
            byte[] buffers = Encoding.ASCII.GetBytes(logout_data);
            host.Send(buffers);

            byte[] bytes = new byte[1024];
            int bytes_length = host.Receive(bytes);
            string return_data = System.Text.Encoding.ASCII.GetString(bytes, 0, bytes_length);
            if (return_data == "loo")
            {
                MessageBox.Show(this, "已下线。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "下线异常!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            System.Environment.Exit(0);
        }


        //Excel 读取操作
        private void Load_Friend()
        {
            
            while (refreshing) { };
            refreshing = true;
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Workbooks wbks = app.Workbooks;
            
            _Workbook _wbk = wbks.Add(System.Windows.Forms.Application.StartupPath+"/Friends");
            Sheets sheet = _wbk.Sheets;
            _Worksheet _Worksheet = (_Worksheet)sheet.get_Item(1);
            int rows = _Worksheet.UsedRange.Cells.Rows.Count;
            for (int i = 1; i <= rows; i++)
            {
                Microsoft.Office.Interop.Excel.Range cell_id = _Worksheet.Cells[i, 1];
                Microsoft.Office.Interop.Excel.Range cell_ip = _Worksheet.Cells[i, 2];
                Microsoft.Office.Interop.Excel.Range cell_state = _Worksheet.Cells[i, 3];
                if (cell_id.Value2 != null)
                {
                    string id = cell_id.Value2.ToString();
                    string ip = cell_ip.Value2.ToString();
                    string state = cell_state.Value2.ToString();
                    bool present_state = false;
                    if (state == "1")
                    {
                        present_state = true;
                    }
                    Friend friendItem1 = new Friend(id, present_state, ip, MyID,i);
                    //friendItem1.Location = new Point(50, 50);
                    friendItem1.Size = new Size(324, 126);
                    this.flowLayoutPanel1.Controls.Add(friendItem1);
                }

            }
            _wbk.Close();
            wbks.Close();
            refreshing = false;
        }

        //Excel 写入好友
        private int Write_Friend(string id,string ip)
        {
            while (refreshing) { };
            refreshing = true;
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Workbooks wbks = app.Workbooks;
            _Workbook _Workbook = wbks.Open(System.Windows.Forms.Application.StartupPath + "/Friends");
            //_Workbook _wbk = wbks.Add("Friends");
            Sheets sheet = _Workbook.Sheets;
            _Worksheet _Worksheet = (_Worksheet)sheet.get_Item(1);
            int rows = _Worksheet.UsedRange.Cells.Rows.Count;
            int i = rows + 1;
            _Worksheet.Cells[i, 1] = id;
            _Worksheet.Cells[i, 2] = ip;
            _Worksheet.Cells[i, 3] = "1";
            app.AlertBeforeOverwriting = false;
            _Workbook.Save();
            _Workbook.Close();
            wbks.Close();
            refreshing = false;
            return rows+1;
        }

        //Excel 删除好友
        private void Delete_Friend(int num)
        {
            while (isquerying) { };
            isquerying = true;
            while (refreshing) { };
            refreshing = true;
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            Workbooks wbks = app.Workbooks;
            _Workbook _Workbook = wbks.Open(System.Windows.Forms.Application.StartupPath + "/Friends");
            //_Workbook _wbk = wbks.Add("Friends");
            Sheets sheet = _Workbook.Sheets;
            _Worksheet _Worksheet = (_Worksheet)sheet.get_Item(1);
            Microsoft.Office.Interop.Excel.Range range1 = _Worksheet.Cells[num, 1];
            Microsoft.Office.Interop.Excel.Range range2 = _Worksheet.Cells[num, 2];
            Microsoft.Office.Interop.Excel.Range range3 = _Worksheet.Cells[num, 3];
            range1.Delete();
            range2.Delete();
            range3.Delete();
            app.AlertBeforeOverwriting = false;
            _Workbook.Save();
            _Workbook.Close();
            wbks.Close();
            refreshing = false;
            isquerying = false;
        }

        //删除好友按键
        private void button_delete_Click(object sender, EventArgs e)
        {
            int Count = 0;
            foreach (Friend item in this.flowLayoutPanel1.Controls)
            {
                if (item.check_state == true)
                    Count++;
            }
            if (Count == 0)
            {
                MessageBox.Show(this, "未选择要删除的好友!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DialogResult dialogResult = MessageBox.Show(this, "确认删除好友？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.OK)
            {
                foreach (Friend item in this.flowLayoutPanel1.Controls)
                {
                    if (item.check_state == true)
                    {
                        Delete_Friend(item.Mynum);
                        this.flowLayoutPanel1.Controls.Remove(item);
                    }
                }
                MessageBox.Show(this, "删除成功！","提示", MessageBoxButtons.OK);
            }


        }
        //发起群聊按键
        private void button_groupchat_Click(object sender, EventArgs e)
        {
            refresh();
            string message = "";
            string AllID = "";
            int member_count = 0;
            foreach (Friend Item in this.flowLayoutPanel1.Controls)
            {
                if (Item.check_state == true)
                {
                    if (Item.label_state.Text == "[离线]")
                    {
                        MessageBox.Show(this, "选择了不在线的聊天对象，请重新选择!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (Item.LocalID == MyID)
                    {
                        MessageBox.Show(this, "群聊里不可以包括自己！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    member_count += 1;
                    AllID = AllID + Item.LocalID + ",";
                }
            }
            
            if (member_count == 0)
            {
                MessageBox.Show(this, "未选择聊天对象", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            message = MyID + "," + AllID.Substring(0, AllID.Length - 1);
            Socket[] sockets = new Socket[member_count];
            int i = 0;
            foreach (Friend Item in this.flowLayoutPanel1.Controls)
            {
                if (Item.check_state == true)
                {
                    //与每个聊天者建立连接
                    string LocalIP = Item.label_ip.Text;
                    int LocalPort = 50000 + int.Parse(Item.LocalID.Substring(Item.LocalID.Length - 4));

                    IPEndPoint serverIP = new IPEndPoint(IPAddress.Parse(LocalIP), LocalPort);
                    Socket chat_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    try
                    {
                        chat_socket.Connect(serverIP);                      
                        byte[] data = Encoding.UTF8.GetBytes(message);
                        chat_socket.Send(data);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(this, "好友状态有误,请刷新", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    sockets[i] = chat_socket;
                    i += 1;
                }
                
            }
            try
            {
                Thread thread_groupchat = new Thread(() => System.Windows.Forms.Application.Run(new ChatWindow(MyID, message, sockets, member_count)));
                thread_groupchat.SetApartmentState(System.Threading.ApartmentState.STA);
                thread_groupchat.Start();
                }
            catch (Exception)
            {
                MessageBox.Show(this, "群聊发起失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //更新好友在线信息
        public void refresh()
        {
            while (refreshing == true) { };
            refreshing = true;
            //对每个好友状态和IP地址进行更新
            foreach (Friend Item in this.flowLayoutPanel1.Controls)
            {

                string query_return = Start_Query(Item.LocalID);
                if (query_return == "n")
                {
                    Control.CheckForIllegalCrossThreadCalls = false;
                    Item.state_refresh(false);
                    int Num = Item.Mynum;
                    Item.LocalIP = "NULL";
                    Item.ip_refresh();
                }
                else if (query_return == "Please send the correct message.")
                {

                }
                else
                {
                    Control.CheckForIllegalCrossThreadCalls = false;
                    Item.state_refresh(true);
                    Item.LocalIP = query_return;
                    Item.ip_refresh();
                    int Num = Item.Mynum;
                }
            }
            refreshing = false;
        }

        private void Button_state_refresh_Click(object sender, EventArgs e)
        {
            refresh();
        }
    }
    
}
