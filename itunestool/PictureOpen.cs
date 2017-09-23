using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ITunEsTooL
{
    public partial class PictureOpen : Form
    {
        private Boolean blnRet = true;
        private string strColor = "";       //呼び出し元から受け取った色

        public string CColor
        {
            set
            { strColor = value; }
        }
        public Boolean Ret
        {
            get { return blnRet; }

        }

        public PictureOpen()
        {
            InitializeComponent();
        }

        private void PictureOpen_Load(object sender, EventArgs e)
        {
            //クリップボードにBitmapデータがあるか調べる（調べなくても良い）
            if (Clipboard.ContainsImage())
            {
                //クリップボードにあるデータの取得
                Image img = Clipboard.GetImage();
                if (img != null)
                {
                    //データが取得できたときは表示する
                    pictureBox5.Image = img;
                }
            }
            switch (strColor)
            {
                case "SkyBlue":
                    lblSetumei.BackColor = Color.SteelBlue;
                    BackColorCange(Color.SkyBlue);
                    break;
                case "Tomato":
                    lblSetumei.BackColor = Color.OrangeRed;
                    BackColorCange(Color.Tomato);
                    break;
                case "Violet":
                    lblSetumei.BackColor = Color.DarkMagenta;
                    BackColorCange(Color.Violet);
                    break;
                case "LightGreen":
                    lblSetumei.BackColor = Color.ForestGreen;
                    BackColorCange(Color.LightGreen);
                    break;
            }
            SystemEvents.UserPreferenceChanged +=
                   new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
        }
        /// <summary>
        /// ユーザー設定が変更されたとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemEvents_UserPreferenceChanged(object sender,
                UserPreferenceChangedEventArgs e)
        {
            ClassicCorrespondence();

        }
        /// <summary>
        /// クラシックモード対応
        /// </summary>
        private void ClassicCorrespondence()
        {
            //クラシックテーマ対策
            if (System.Windows.Forms.VisualStyles.VisualStyleInformation.DisplayName == "")
            {
                this.BackColor = SystemColors.Control;
            }
            else
            {
                this.BackColor = Color.White;
            }
        }
        /// <summary>
        /// 背景色変更
        /// </summary>
        /// <param name="ColorCode"></param>
        private void BackColorCange(Color ColorCode)
        {
            Yes.BackColor = ColorCode;
            No.BackColor = ColorCode;
        }

        private void Yes_Click(object sender, EventArgs e)
        {
            blnRet = true;
            this.Close();
        }

        private void No_Click(object sender, EventArgs e)
        {
            blnRet = false;
            this.Close();
        }
    }
}
