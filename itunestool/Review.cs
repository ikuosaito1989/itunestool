using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.Configuration;
namespace ITunEsTooL
{
    public partial class Review : Form
    {
        private Color _Color = Color.SkyBlue;
        private string _strJikai = "True";
        private string _strErrMsg = string.Empty;
        private int _intNum = 3;
        private ImageOperation ImgOpe = null;
        public Color Color
        {
            set
            { _Color = value; }
        }
        public string ErrMsg
        {
            get { return _strErrMsg; }

        }
        public string Jikai
        {
            get { return _strJikai; }

        }

        public Review()
        {
            InitializeComponent();
        }

        private void Review_Load(object sender, EventArgs e)
        {
            txtReview.Text = "";
            txtFrom.Text = "";
            btnSend.BackColor = _Color;
            btnNotSend.BackColor = _Color;
            btnSyosai.BackColor = _Color;
            ImgOpe = new ImageOperation();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string strFrom = string.Empty;
            string strTo = "itunestool.jp@gmail.com";
            string strSubject = "レビューのお問い合わせ";
            string strBody = string.Empty;
            Mail objMail = new Mail();

            _strErrMsg = objMail.SendMail(txtFrom.Text, strTo, strSubject, txtReview.Text, _intNum, 1);

            this.Close();
        }

        private void btnNotSend_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void picReview_Click(object sender, EventArgs e)
        {
            _intNum = _intNum == 5 ? 1 : _intNum + 1;
            picReview.Image = ImgOpe.SetResources(_intNum.ToString());
        }

        private void chkJikai_CheckedChanged(object sender, EventArgs e)
        {
            _strJikai = chkJikai.Checked ? "False" : "True";
        }

        private void btnSyosai_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["Domain"] + "info/privacypolicy%20.html");
        }
    }
}
