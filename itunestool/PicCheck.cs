using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ITunEsTooL
{
    public partial class PicCheck : Form
    {
        private string _strUrl = "";
        private string _strArtist = "";
        private string _strAlbum = "";
        private string _strName = "";
        private Boolean _blnRet = false;
        private Boolean _blnJikai = false;
        private Boolean _blnJump = false;
        private Color _iSetColor;
        private byte[] _imgData = null;
        private int _iSetEventNum = 0;
        private int iWidth = 0;
        private int iHeight = 0;
        
        public string Url
        {
            set
            { _strUrl = value; }
        }
        public string Artist
        {
            set
            { _strArtist = value; }
        }
        public string Album
        {
            set
            { _strAlbum = value; }
        }
        public string MusicName
        {
            set
            { _strName = value; }
        }
        public int SetEvent
        {
            set
            { _iSetEventNum = value; }
        }
        public Color SetColor
        {
            set
            { _iSetColor = value; }
        }
        public Boolean Ret
        {
            get { return _blnRet; }

        }
        public Boolean Jikai
        {
            get { return _blnJikai; }

        }
        public Boolean Jump
        {
            get { return _blnJump; }

        }
        public byte[] Img
        {
            set
            { _imgData = value; }

        }
        public PicCheck()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _blnJikai = chkJikai.Checked;
            _blnJump = false;
            _blnRet = false;
            this.Close();
            pictureBox1.Image = null;
        }

        private void Yes_Click(object sender, EventArgs e)
        {
            DialogResult YorN = DialogResult.No;

            YorN = MessageBox.Show("Artist : " + _strArtist + "\n" +
                                   "Album : " + _strAlbum + "\n" +
                                   "Name : " + _strName + "\n" +
                                   "Size : " + iWidth + "×" + iHeight + "\n" +
                                   "この画像を設定します。よろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (YorN == DialogResult.Yes)
            {
                _blnJikai = chkJikai.Checked;
                _blnRet = true;
                _blnJump = false;
                this.Close();
                pictureBox1.Image = null;

            }
            
        }

        private void PicCheck_Load(object sender, EventArgs e)
        {

            using (MemoryStream st = new MemoryStream(_imgData))
            {
                pictureBox1.Image = new Bitmap(st);
                st.Close();
            }
            iWidth = pictureBox1.Image.Width;
            iHeight = pictureBox1.Image.Height;
            toolTip1.SetToolTip(pictureBox1, "Artist : " + _strArtist + "\n" +
                                             "Album : " + _strAlbum + "\n" +
                                             "Name : " + _strName + "\n" +
                                             "Size : " + iWidth + "×" + iHeight);

            lblAlbum.Text = _strAlbum;
            lblArtist.Text = _strArtist;
            lblName.Text = _strName;
            Yes.BackColor = _iSetColor;
            No.BackColor = _iSetColor;
            btnNextArtwork.BackColor = _iSetColor;

            switch (_iSetEventNum)
            {
                case 1:
                    Yes.Location = new Point(12, 461);
                    Yes.Size = new Size(116, 27);
                    No.Location = new Point(134, 461);
                    No.Size = new Size(116, 27);
                    btnNextArtwork.Location = new Point(256, 461);
                    btnNextArtwork.Size = new Size(220, 27);
                    break;
                case 2:
                    Yes.Location = new Point(12, 461);
                    Yes.Size = new Size(226, 27);
                    No.Location = new Point(250, 461);
                    No.Size = new Size(226, 27);
                    btnNextArtwork.Visible = false;
                    chkJikai.Visible = false;
                    label43.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void PicCheck_Shown(object sender, EventArgs e)
        {
            this.Update();
            this.Activate();
        }

        private void label43_Click(object sender, EventArgs e)
        {
            chkJikai.Checked = !chkJikai.Checked;
        }

        private void btnNextArtwork_Click(object sender, EventArgs e)
        {
            _blnJump = true;
            this.Close();
            pictureBox1.Image = null;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

    }
}
