using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LOLFreeView
{
    public partial class Form2 : Form
    {
        public static float version = 0.0f;
        public Form2()
        {
            InitializeComponent();
        }
        RegistryKey runRegKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        

        private void Form2_Load(object sender, EventArgs e)
        {
            if(runRegKey.GetValue("LOLFreeView") == null)
            {
                checkBox1.Checked = false;
            }
            else
            {
                checkBox1.Checked = true;
            }
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                runRegKey.SetValue("LOLFreeView", Environment.CurrentDirectory + "\\" + AppDomain.CurrentDomain.FriendlyName);
            }
            else
            {
                runRegKey.DeleteValue("LOLFreeView", false);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var client = new System.Net.WebClient() { Encoding = Encoding.UTF8 })
            {
                string url = "https://treenest.github.io/LeagueMultiSearch/update.txt";
                string check_version_string = client.DownloadString(url);
                double check_version = Convert.ToDouble(check_version_string);
                if (version < check_version)
                {
                    button1.Text = "업데이트 있음";
                    System.Diagnostics.Process.Start("http://www.mediafire.com/file/ws8kqmgrwt4zftd/FocusWriter_1.7.1.exe/file");
                }
                else
                {
                    button1.Text = "업데이트 없음";
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Text = "아직 준비중 ㅠ.ㅠ";
        }
    }
}
