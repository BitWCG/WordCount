using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace InterFace
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //设置进程的重定向输出
        public void ProcessOutDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (this.Output.InvokeRequired)
            {
                this.Output.Invoke(new Action(() =>
                {
                    this.Output.AppendText(e.Data + "\r\n");
                }));
            }
            else
            {
                this.Output.AppendText(e.Data + "\r\n");
            }
        }
        //C#启动EXE文件
        public bool StartProcess(string filename, string[] args)
        {
            try
            {
                string s = "";
                foreach (string arg in args)
                {
                    s = s + arg + " ";
                }
                s = s.Trim();
                Process myprocess = new Process();
                myprocess.OutputDataReceived -= new DataReceivedEventHandler(ProcessOutDataReceived);
                ProcessStartInfo startInfo = new ProcessStartInfo(filename, s);
                startInfo.FileName = "WordCount.exe";
                startInfo.UseShellExecute = false;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.CreateNoWindow = true;
                startInfo.RedirectStandardError = true;
                startInfo.RedirectStandardInput = true;//重定向输入
                startInfo.RedirectStandardOutput = true;//重定向输出
                myprocess.StartInfo = startInfo;
                myprocess.StartInfo.UseShellExecute = false;
                myprocess.Start();
                myprocess.BeginOutputReadLine();
                myprocess.OutputDataReceived += new DataReceivedEventHandler(ProcessOutDataReceived);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("启动应用程序时出错！原因：" + ex.Message);
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();   //显示选择文件对话框 
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.FilePath.Text = openFileDialog1.FileName;     //显示文件路径 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.FilePath.Text.Trim()))
            {
                MessageBox.Show("请选择文件");
            }
            string[] arg = new string[10];
            arg[0] = "-c"; arg[1] = "-w"; arg[2] = "-l"; arg[3] = "-a";
            arg[4] = this.FilePath.Text.Trim();
            StartProcess(@"WordCount.exe", arg);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
