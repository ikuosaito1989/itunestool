using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Xml;
using iTunesLib;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ITunEsTooL
{
    public partial class Picture : Form
    {
        private Color strColor = Color.AliceBlue;      //呼び出し元から受け取った色
        private string strPath = "";       //呼び出し元から受け取ったパス
        private IITTrack TTR = null;       //呼び出し元から受け取ったオブジェクト
        private string strErr = "";       //呼び出し元から受け取ったパス
        private string _strArtist = "";
        private string _strAlbum = "";
        private string _strName = "";
        public Color CColor
        {
            set
            { strColor = value; }
        }
        public string Path
        {
            set
            { strPath = value; }
        }
        public IITTrack Track
        {
            set
            { TTR = value; }
        }
        public string strRet
        {
            get { return strErr; }

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
        public Picture()
        {
            InitializeComponent();
        }

        private void Picture_Load(object sender, EventArgs e)
        {
            iTunesLib.IITArtworkCollection AWC = null;
            FileStream fs = null;
            iTunesLib.IITArtwork AW = null;

            try
            {
                int w = Screen.PrimaryScreen.WorkingArea.Width;
                int h = Screen.PrimaryScreen.WorkingArea.Height;
                this.SuspendLayout();

                AWC = TTR.Artwork;
                AW = AWC[1];
                AW.SaveArtworkToFile(strPath + "tmp.jpg");
                using (fs = new FileStream(strPath + "tmp.jpg", System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    if (System.Drawing.Image.FromStream(fs).Height < h - 50)
                    {
                        pictureBox1.Height = System.Drawing.Image.FromStream(fs).Height;
                    }
                    else
                    {
                        this.Top = 0;
                        pictureBox1.Height = h - 50;
                    }

                    if (System.Drawing.Image.FromStream(fs).Width < w - 30)
                    {

                        pictureBox1.Width = System.Drawing.Image.FromStream(fs).Width;
                    }
                    else
                    {
                        this.Left = 0;
                        pictureBox1.Width = w - 30;
                    }
                    this.Width = pictureBox1.Width + 30;
                    if ((w - this.Width) > 0)
                        this.Left = (w - this.Width) / 2;
                    this.Height = pictureBox1.Height + 50;
                    if ((h - this.Height) > 0)
                        this.Top = (h - this.Height) / 2;
                    pictureBox1.Image = System.Drawing.Image.FromStream(fs);

                    int iwidth = 0;
                    int iheight = 0;
                    iheight = System.Drawing.Image.FromStream(fs).Height;
                    iwidth = System.Drawing.Image.FromStream(fs).Width;
                    toolTip1.SetToolTip(pictureBox1, "Artist : " + _strArtist + "\n" +
                                                     "Album : " + _strAlbum + "\n" +
                                                     "Name : " + _strName + "\n" +
                                                     "Size : " + iwidth + "×" + iheight);
                    fs.Close();
                    this.ResumeLayout();

                }
            }
            catch (System.Exception ex)
            {
                strErr += "System Exception 23534 : " + ex.Message + "\n" + ex.StackTrace + "\n";
            }
            finally
            {
                if (AWC != null)
                    Marshal.ReleaseComObject(AWC);
                if (AW != null)
                    Marshal.ReleaseComObject(AW);
            }
        }

        private void No_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
