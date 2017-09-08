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

    public partial class Ichiran : Form
    {
        private CommonValue COMVAL = new CommonValue();
        private Boolean DelFlg = false;
        private string strColor = "";       //呼び出し元から受け取った色
        //private string[] strALLName;
        
        //共通変数
        public CommonValue CLSCOMVAL
        {
            set { COMVAL = value; }
            get { return COMVAL; }
        }
        public string CColor
        {
            set
            { strColor = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Ichiran()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ichiran_Load(object sender, EventArgs e)
        {
            this.Text = COMVAL.DIALOG_TITLE;
            string[] strName = new string[1];
            

            lblSetumei.Text = "コピーする曲名の一覧を表示します。" + "\n" + "削除するには、選択後、削除ボタンを押して下さい。";
  
            lstIchiran.Items.AddRange(COMVAL.strName);
            string strChkAlbum = string.Empty;

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
                lstIchiran.BackColor = Color.White;
                txtSerch.BackColor = Color.White;
            }
            else
            {
                this.BackColor = Color.White;
                lstIchiran.BackColor = Color.WhiteSmoke;
                txtSerch.BackColor = Color.WhiteSmoke;
            }
        }
         /// <summary>
        /// 背景色変更
        /// </summary>
        /// <param name="ColorCode"></param>
        private void BackColorCange(Color ColorCode)
        {
            btnDelete.BackColor = ColorCode;
            OK.BackColor = ColorCode;
            button1.BackColor = ColorCode;
            btnSerch.BackColor = ColorCode;
        }
        /// <summary>
        /// OKクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OK_Click(object sender, EventArgs e)
        {

            string[] strpath1 = new string[1];
            strpath1 = new string[1];
            int i = 0;
            int index = 0;

            if (!DelFlg)
            {
                this.Close();
                return;
            }
            
            foreach (string item in lstIchiran.Items)
            {
                Array.Resize(ref strpath1, i + 1);
                index = Array.IndexOf(COMVAL.strName, item);
                strpath1[i] = COMVAL.strLocation[index];
                i++;
                    
            }

            COMVAL.strPaths = strpath1;
            if (i == 0)
                COMVAL.strPaths = null;
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            int[] intcnt = new int[lstIchiran.SelectedIndices.Count];

            DialogResult YorN;

            if (lstIchiran.SelectedIndices.Count == 0)
                return;
            YorN = MessageBox.Show("選択した曲を削除してもよろしいですか？", "削除",
               @MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (YorN == DialogResult.No)
                return;

            this.Text = "削除中です...";
            lstIchiran.BeginUpdate();

            while (lstIchiran.SelectedItems.Count != 0)
            {
                lstIchiran.Items.RemoveAt(lstIchiran.SelectedIndices[0]);

            }
            
            this.Text = "曲名一覧";
            lstIchiran.EndUpdate();
            DelFlg = true;
            intcnt = null;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Ichiran_FormClosed(object sender, FormClosedEventArgs e)
        {
            SystemEvents.UserPreferenceChanged -=
                new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
        }

        private void lblSetumei_Click(object sender, EventArgs e)
        {

        }

        private int Ichi = -1;
        /// <summary>
        /// 検索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSerch_Click(object sender, EventArgs e)
        {
            string[] strUpName = new string[1];
            const int SCROLLVAL = 5;
           
            if (txtSerch.Text == "")
                return;
            lstIchiran.BeginUpdate();
            int i = 0;
            foreach (string item in lstIchiran.Items)
            {
                if (item.StartsWith(txtSerch.Text) || item.StartsWith(txtSerch.Text.ToUpper()))
                {

                    if (i > Ichi){

                        if (lstIchiran.Items.Count > (i + SCROLLVAL))
                        {
                            lstIchiran.SetSelected(i + SCROLLVAL, true);
                            lstIchiran.SetSelected(i + SCROLLVAL, false);
                        }
                        if(!chkSentaku.Checked)
                            lstIchiran.ClearSelected();
                        lstIchiran.SetSelected(i, true);
                        Ichi = i;
                        break;
                    }
                }
                i++;
            }

            if (i == lstIchiran.Items.Count)
            {
                System.Media.SystemSounds.Beep.Play();
                Ichi = -1;
            }
         
            lstIchiran.EndUpdate();
            strUpName = null;
        }

        private void lstIchiran_Click(object sender, EventArgs e)
        {
            Ichi = lstIchiran.SelectedIndex;
        }

        private void txtSerch_KeyDown(object sender, KeyEventArgs e)
        {
            //押されたキーがエンターキーかどうかの条件分岐
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                btnSerch.PerformClick();
                if (!e.Control)
                {
                    this.SelectNextControl(this.ActiveControl, !e.Shift, true, true, true);
                }
                //エンターキーが押されたときの処理をここに書く
            }
        }

        private void txtSerch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
        }

    }
}
