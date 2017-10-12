using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ITunEsTooL
{
    public partial class CrashReport : Form
    {
        private string _ErrMsg = string.Empty;
        private string _Path = string.Empty;
        private Color _Color = Color.SkyBlue;

        public string Path
        {
            set
            { _Path = value; }
        }
        public Color Color
        {
            set
            { _Color = value; }
        }
        public string ErrMsg
        {
            set { _ErrMsg = value; }

        }
        public CrashReport()
        {
            InitializeComponent();
        }

        private void CrashReport_Load(object sender, EventArgs e)
        {
            btnSend.BackColor = _Color;
            btnNotSend.BackColor = _Color;
            btnSyosai.BackColor = _Color;
        }

        private void btnNotSend_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Mail objMail = new Mail();
            string strTo = "itunestool.jp@gmail.com";
            string strSubject = "エラーのお問い合わせ";
            objMail.SendMail("", strTo, strSubject, _ErrMsg);
            this.Close();
        }

        private void btnSyosai_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://itunestool.jp/info/privacypolicy%20.html");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (File.Exists(_Path))
                System.Diagnostics.Process.Start(_Path);
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://picpic.html.xdomain.jp/");
        }
    }
}
