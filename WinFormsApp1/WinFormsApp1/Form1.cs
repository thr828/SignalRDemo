using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private HubConnection _conn;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //建立SignalR连接
            string url = "http://localhost:5114/ChatHub";
            _conn = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();

            _conn.StartAsync();
            //SignalR客户端定义ReceiveMessage方法，中心调用客户端方法
            _conn.On<string, string>("ReceiveMessage", RecvMsg);//接收错误日志

            //断开重连方法
            _conn.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _conn.StartAsync();
            };
        }

        private void RecvMsg(string identifiy, string errMessage)
        {
            this.BeginInvoke(new Action(() =>
            {
                richTextBox1.Text += $"{identifiy}:{errMessage}" + Environment.NewLine;
                string length = string.IsNullOrEmpty(textBox3.Text) ? "0" : textBox3.Text;
                textBox3.Text = (int.Parse(length) + 1).ToString();
            }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
                 //触发SignalR客户端的发送方法，客户端调用中心方法，此处的方法名称"SendMessage"应和服务器上的方法名称保持一致
            _conn.InvokeAsync("SendMessage", this.textBox1.Text, this.textBox2.Text);
        }
    }
}
