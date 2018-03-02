using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Configuration;
namespace ITunEsTooL
{
    public partial class frmVersion : Form
    {
        private Point mousePoint;           //マウスのクリック位置を記憶
        private Form frmParentForm;         //呼び出し元
        private CommonValue COMVAL = new CommonValue();

        public new Form ParentForm

        {
            set
            { frmParentForm = value; }
        }
        const int CS_DROPSHADOW = 0x00020000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }
        /// <summary>
        /// AnimateWindow関数にて利用されるフラグを表す列挙型です。
        /// </summary>
        [Flags]
        private enum AnimateWindowFlags : uint
        {
            AW_HOR_POSITIVE = 0x00000001,
            AW_HOR_NEGATIVE = 0x00000002,
            AW_VER_POSITIVE = 0x00000004,
            AW_VER_NEGATIVE = 0x00000008,
            AW_CENTER = 0x00000010,
            AW_HIDE = 0x00010000,
            AW_ACTIVATE = 0x00020000,
            AW_SLIDE = 0x00040000,
            AW_BLEND = 0x00080000
        }
        /// <summary>
        /// AnimateWindow関数
        /// </summary>
        /// <param name="hwnd">ハンドル</param>
        /// <param name="time">アニメーションを行う時間</param>
        /// <param name="flags">挙動を表すフラグ</param>
        [DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int time, uint flags);

        public frmVersion()
        {
            InitializeComponent();
        }

        private void Version_Load(object sender, EventArgs e)
        {
            this.Location = new Point(
            frmParentForm.Location.X + (frmParentForm.Width - this.Width) / 2,
            frmParentForm.Location.Y + (frmParentForm.Height - this.Height) / 2);

            label1.Text = label1.Text + " " + COMVAL.strVer.FileVersion;
            AnimateWindow(this.Handle, 300, (uint)(AnimateWindowFlags.AW_HOR_NEGATIVE | AnimateWindowFlags.AW_BLEND));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AnimateWindow(Handle, 300, (uint)(AnimateWindowFlags.AW_HOR_NEGATIVE | AnimateWindowFlags.AW_BLEND | AnimateWindowFlags.AW_HIDE));
            this.Close();
        }

        private void pictureBox5_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Left += e.X - mousePoint.X;
                this.Top += e.Y - mousePoint.Y;
            }
        }

        private void pictureBox5_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                mousePoint = new Point(e.X, e.Y);
        }

        private void label5_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                mousePoint = new Point(e.X, e.Y);
        }

        private void label5_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Left += e.X - mousePoint.X;
                this.Top += e.Y - mousePoint.Y;
            }
        }

        private void frmVersion_Shown(object sender, EventArgs e)
        {
            frmItune Main = new frmItune();
            string strver = string.Empty;

            if (NetworkInterface.GetIsNetworkAvailable())
            {
                strver = Main.getNewVersion();
                if (strver != "")
                {
                    if (COMVAL.strVer.FileVersion == strver)
                    {
                        MessageBox.Show("ITunEs TooLは最新バージョンです。", "お知らせ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        DialogResult YorN = DialogResult.No;
                        YorN = MessageBox.Show("ITunEsTooLは最新のバージョンではありません。" + "\n" +
                            "ダウンロードサイトに遷移しますか？",
                               "お知らせ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (YorN == DialogResult.Yes)
                            System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["Domain"]);
                    }
                }
            }
        }
    }
}
