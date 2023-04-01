using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Directory_Structure_Mirror_Maker
{
    public partial class Form1 : Form
    {
        delegate void D(object obj);
        delegate void D2();
        delegate void D3(List<string> files, string directory, string pattern);
        bool isStop = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "请选择文件夹";
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog1.ShowNewFolderButton = true;
            if (textBox1.Text.Length > 0) folderBrowserDialog1.SelectedPath = textBox1.Text;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "请选择文件夹";
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog1.ShowNewFolderButton = true;
            if (textBox2.Text.Length > 0) folderBrowserDialog1.SelectedPath = textBox2.Text;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Console.WriteLine("start");
            string path = @"E:\教程\";
            string outPath = @"D:\test\";

            //List<string> files = MyGetFiles(path);
            //foreach (string directory in directorys)
            //{
            //    Debug.WriteLine(path+directory);
            //}

        }

        void MyGetFiles(string path, string outPath)
        {
            List<FileInfo> folder = new DirectoryInfo(path).GetFiles("*").ToList();
            List<string> files = folder.Select(x => x.Name).ToList();


            //List<long> fLength = new List<long>();
            //foreach (FileInfo fi in folder)
            //{
            //    fLength.Add(fi.Length);
            //}

            //int i = 0;
            foreach (string file in files)
            {
                //fLength[i].ToString("");
                if (isStop)
                {
                    return;
                }
                if (path.IndexOf("$") != -1)
                {
                    return;
                }
                SendMessageBox(label3, outPath + file + "\\");
                Console.WriteLine(path + file);
                File.WriteAllText(outPath + file, "");
            }
        }
        void MyGetDirectories(string path, string outPath)
        {
            if (path.IndexOf("System Volume Information") != -1)
            {
                return;
            }
            List<DirectoryInfo> directory = new DirectoryInfo(path).GetDirectories("*").ToList();
            List<string> directorys = directory.Select(x => x.Name).ToList();
            MyGetFiles(path, outPath);

            foreach (string dir in directorys)
            {
                if (isStop)
                {
                    return;
                }
                if (path.IndexOf("$") != -1)
                {
                    return;
                }

                Directory.CreateDirectory(outPath + dir + "\\");
                MyGetDirectories(path + dir + "\\", outPath + dir + "\\");
            }

        }


        private void button4_Click(object sender, EventArgs e)
        {
            isStop = true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ParameterizedThreadStart(SetTextBoxValue));
            t.Start("开始提取");

        }

        void SetTextBoxValue(object obj)
        {
            SendMessageBox(label3, obj.ToString());
            D2 d2 = new D2(ButtonModeChange1);
            Invoke(d2);
            MyGetDirectories(textBox1.Text + "\\", textBox2.Text + "\\");

            d2 = new D2(ButtonModeChange2);
            Invoke(d2);

            SendMessageBox(label3, "任务结束");
            isStop = false;
        }

        void SendMessageBox(Label targetLabel, string s)
        {
            D d = new D(DelegateSetValue);
            Invoke(d, s);
        }
        #region 按钮锁定
        void ButtonModeChange1()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true;
        }
        void ButtonModeChange2()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = false;
        }
        #endregion
        void DelegateSetValue(object obj)
        {
            this.label3.Text = obj.ToString();
        }


        #region 拖动获取路径
        private void Form1_DragEnter(object sender, DragEventArgs e)                                         //获得“信息”
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;                                                              //重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }
        private void Form1_DragDrop(object sender, DragEventArgs e)                                          //解析信息
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();       //获得路径
            textBox1.Text = path;                                                                            //由一个textBox显示路径
        }
        private void Form2_DragEnter(object sender, DragEventArgs e)                                         //获得“信息”
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;                                                              //重要代码：表明是所有类型的数据，比如文件路径
            else
                e.Effect = DragDropEffects.None;
        }
        private void Form2_DragDrop(object sender, DragEventArgs e)                                          //解析信息
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();       //获得路径
            textBox2.Text = path;                                                                            //由一个textBox显示路径
        }
        #endregion

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest_1(object sender, EventArgs e)
        {

        }


    }
}