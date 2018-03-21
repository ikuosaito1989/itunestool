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

namespace Installer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var clsid = Registry.ClassesRoot.OpenSubKey("CLSID");

            // インストールされていない
            if(clsid.OpenSubKey("{B83FC273-3522-4CC6-92EC-75CC86678DA4}") == null && clsid.OpenSubKey("{2ACECADE-0BC7-4c6f-95CF-A221CC161B52}") == null)
            {
                return;
            }
            clsid.Close();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("jword2setup_common.exe", @"jword2setup_common.exe/S isaito isaito__soft");
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = checkBox1.Checked;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("license.txt");
        }
    }
}
