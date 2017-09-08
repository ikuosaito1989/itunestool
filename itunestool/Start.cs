using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ITunEsTooL
{
    public partial class Start : Form
    {
        private string strErrMsg = string.Empty;
        private Color _Color;
        private Boolean _blnStop = false;
        private Boolean _blnCloseRet = false;
        private Point mousePoint;

        public Start()
        {
            InitializeComponent();
        }
        
        public Color Color
        {
            set { _Color = value; }
        
        }
        public Boolean Stop
        {
            set { _blnStop = value; }
        }
        public Boolean CloseRet
        {
            get { return _blnCloseRet; }

        }
        
        /// <summary>
        /// AnimateWindow 列挙型
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
        /// Shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Shown(object sender, EventArgs e)
        {
            try
            {
                this.Update();
                this.Activate();   
            }
            catch(Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Load(object sender, EventArgs e)
        {

            CommonValue COMVAL = new CommonValue();            

            pictureBox3.BackColor = _Color;
            pictureBox4.BackColor = _Color;
            pictureBox5.BackColor = _Color;
            pictureBox6.BackColor = _Color;
            timer1.Enabled = true;

            label5.Text = label5.Text + " " + COMVAL.strVer.FileVersion;
            AnimateWindow(this.Handle, 200, (uint)(AnimateWindowFlags.AW_HOR_NEGATIVE | AnimateWindowFlags.AW_BLEND));
            
        }

        /// <summary>
        /// Tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_blnStop)
            {
                this.TopMost = false;
                this.Close();
                _blnCloseRet = true;
            }
        }

        private void Start_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                mousePoint = new Point(e.X, e.Y);
        }

        private void Start_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Left += e.X - mousePoint.X;
                this.Top += e.Y - mousePoint.Y;
            }
        }

    }
}
