using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace ITunEsTooL
{
    public partial class PlayListName : Form
    {
        //エラーメッセージ戻り値
        private string strMsg = string.Empty;
        private string strSetumei = "作成するプレイリストの名前を設定して下さい。";
        private Boolean blnRet = true;
        private string strColor = string.Empty;

        //0 → CSV :1 → プレイリスト
        private int iSelectEvent = 0;

        public string setSetumei
        {
            set { strSetumei = value; }

        }
        public int SelectEvent
        {
            set { iSelectEvent = value; }

        }
        public string CColor
        {
            set
            { strColor = value; }
        }
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public string getMassage
        {
            get { return strMsg; }

        }
        public Boolean Ret
        {
            get { return blnRet; }

        }
        public PlayListName()
        {
            InitializeComponent();
        }

        private void PlayListName_Load(object sender, EventArgs e)
        {
            txtMessage.Text = "";
            lblSetumei.Text = strSetumei;

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

            ClassicCorrespondence();
            SystemEvents.UserPreferenceChanged +=
                   new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
        }
        /// <summary>
        /// 背景色変更
        /// </summary>
        /// <param name="ColorCode"></param>
        private void BackColorCange(Color ColorCode)
        {
            OK.BackColor = ColorCode;
            button1.BackColor = ColorCode;
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
                txtMessage.BackColor = Color.White;
            }
            else
            {
                this.BackColor = Color.White;
                txtMessage.BackColor = Color.WhiteSmoke;
            }
        }
        private void OK_Click(object sender, EventArgs e)
        {
            strMsg = txtMessage.Text;
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult YorN = DialogResult.No;

            switch (iSelectEvent)
            {
                case 0:
                    YorN = MessageBox.Show("CSVの作成を中止しますか？", "プレイリスト作成",
                        @MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    break;
                case 1:
                    YorN = MessageBox.Show("プレイリストの作成を中止しますか？", "プレイリスト作成",
                       @MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    break;
                case 2:
                    YorN = MessageBox.Show("アートワークのバックアップを中止しますか？", "バックアップ",
                       @MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    break;
            }

            if (YorN == DialogResult.Yes)
            {
                blnRet = false;
                strMsg = "";
                this.Close();
            }
        }

        private void PlayListName_FormClosed(object sender, FormClosedEventArgs e)
        {
            SystemEvents.UserPreferenceChanged -=
                new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
        }

    }
}
