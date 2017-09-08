using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ITunEsTooL
{
    public partial class frmSplash : Form
    {

        private Point mousePoint;           //マウスのクリック位置を記憶
        private Form frmParentForm;         //呼び出し元
        private Boolean blnStop = false;    //終了フラグ
        private int pintParcent = 0;        //処理中の件数を示す
        private int intALL = 0;             //処理を行うデータの合計値
        private int intZero = 0;
        private string strDaimei = "";      //呼び出し元から受け取った題名
        private string strColor = "";       //呼び出し元から受け取った色
        private string strMessage = "";
        private string strHeader = "少々お待ち下さい...";
        private Boolean blnVisible = true;
        private Boolean blnCancel = false;

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
        
        public new Form ParentForm
        {
            set
            {frmParentForm = value;}
        }
        public Boolean Stop
        {
            set
            {blnStop = value;}
        }
        public string HeadMessage
        {
            set
            { strHeader = value;            
            }
        }
        public Boolean SendCancel
        {
            set
            { blnCancel = value; }
        }
        public Boolean Cancel
        {
            get { return blnCancel; }

        }
        public Boolean bVisible
        {
            set
            { blnVisible = value; }
        }
        public string Message
        {
            set
            { strMessage = value; }
        }
        public string Daimei
        {
            set
            {strDaimei = value;}
        }
        public string CColor
        {
            set
            {strColor = value;}
        }
        public int Parcent
        {
            set
            {pintParcent = value;}
        }
        public int ALL
        {
            set
            {intALL = value;}
        }
        public int Zero
        {
            set
            { intZero = value; }
        }
        public frmSplash()
        {
            InitializeComponent();
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
        /// 画面操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Left += e.X - mousePoint.X;
                this.Top += e.Y - mousePoint.Y;
            }
        }
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                mousePoint = new Point(e.X, e.Y);
        }

        /// <summary>
        /// フォームロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {

            this.Location = new Point(
            frmParentForm.Location.X + (frmParentForm.Width - this.Width) / 2,
            frmParentForm.Location.Y + (frmParentForm.Height - this.Height) / 2);
            timer2.Enabled = true;
            switch(strColor)
            {
                case "SkyBlue":

                    BackColorCange(Color.SkyBlue);
                    txtshori.ForeColor = Color.DarkTurquoise;
                    label2.ForeColor = Color.LightBlue;
                    label3.ForeColor = Color.LightBlue;
                    label4.ForeColor = Color.LightBlue;
                    lblALL.ForeColor = Color.LightBlue;    
                    break;
                case "Tomato":
                    BackColorCange(Color.Tomato);
                    txtshori.ForeColor = Color.OrangeRed;
                    label2.ForeColor = Color.LightSalmon;
                    label3.ForeColor = Color.LightSalmon;
                    label4.ForeColor = Color.LightSalmon;
                    lblALL.ForeColor = Color.LightSalmon;
                    break;
                case "Violet":
                    BackColorCange(Color.Violet);
                    txtshori.ForeColor = Color.Magenta;
                    label2.ForeColor = Color.Plum;
                    label3.ForeColor = Color.Plum;
                    label4.ForeColor = Color.Plum;
                    lblALL.ForeColor = Color.Plum;
                    break;
                case "LightGreen":
                    BackColorCange(Color.LightGreen);
                    txtshori.ForeColor = Color.Lime;
                    label2.ForeColor = Color.PaleGreen;
                    label3.ForeColor = Color.PaleGreen;
                    label4.ForeColor = Color.PaleGreen;
                    lblALL.ForeColor = Color.PaleGreen;
                    break;
            }

            this.TopMost = true;
            this.Activate();
            if (strDaimei.IndexOf("iTunes情報を取得中です...") != -1)
            {
                label3.Text = "曲取込み完了";
                SetVisible(true);
                button1.Visible = true;
            }
            else if (strDaimei.IndexOf("データをコピー中です...") != -1)
            {
                label3.Text = "曲コピー完了";
                SetVisible(true);
                button1.Visible = true;
            }
            else if (strDaimei.IndexOf("CSVファイルを作成しています...") != -1)
            {
                label3.Text = "曲チェック完了";
                SetVisible(true);
                button1.Visible = true;
            }
            else if (strDaimei.IndexOf("ファイルを削除中...") != -1)
            {
                label3.Text = "曲チェック完了";
                SetVisible(true);
                button1.Visible = true;
            }
            else if (strDaimei.IndexOf("アートワークをバックアップ中です") != -1)
            {
                label3.Text = "曲チェック完了";
                SetVisible(true);
                button1.Visible = true;
            }
            else if (strDaimei.IndexOf("CSVファイルを作成しています...") != -1)
            {
                label3.Text = "曲チェック完了";
                SetVisible(true);
                button1.Visible = false;
            }
            else if (strDaimei.IndexOf("画像を設定しています...") != -1)
            {
                label3.Text = "曲　設定完了";
                SetVisible(true);
                button1.Visible = false;
            }
            else if (strDaimei.IndexOf("重複曲を検索中...") != -1)
            {
                label3.Text = "曲チェック完了";
                SetVisible(true);
                button1.Visible = true;
            }
            else if (strDaimei.IndexOf("画像を自動設定しています...") != -1)
            {
                label3.Text = "曲チェック完了";
                SetVisible(true);
                button1.Visible = true;
            }
            else if (strDaimei.IndexOf("画像を削除しています...") != -1)
            {
                label3.Text = "曲,削除完了";
                SetVisible(true);
                button1.Visible = true;
            }
            else if (strDaimei.IndexOf("画像をダウンロードしています...") != -1)
            {
                SetVisible(false);
                button1.Visible = false;
            }
            
            txtshori.Text = strDaimei;
            label2.Text = Convert.ToString(intZero);
            lblALL.Text = Convert.ToString(intALL);
            AnimateWindow(this.Handle, 200, (uint)(AnimateWindowFlags.AW_HOR_NEGATIVE | AnimateWindowFlags.AW_BLEND));
        }

        private void SetVisible(Boolean blnState)
        {
            if (blnState)
            {
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                lblALL.Visible = true;
                txtshori.Top = 46;
            }
            else
            {
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                lblALL.Visible = false;
                txtshori.Top = 55;
            }
        }
        /// <summary>
        /// 背景色変更
        /// </summary>
        /// <param name="ColorCode"></param>
        private void BackColorCange(Color ColorCode)
        {
          
            picTop.BackColor = ColorCode;
            picButtom.BackColor = ColorCode;
            picLeft.BackColor = ColorCode;
            picRight.BackColor = ColorCode;
            label1.BackColor = ColorCode;
            
        }

        /// <summary>
        /// タイマー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            label2.Text = pintParcent.ToString();
            if (strMessage != "")
            {

                label3.Text = "曲　設定完了";
                SetVisible(true);
                txtshori.Text = strMessage;
                button1.Visible = true;
            }


            if (!blnVisible)
            {
                this.Top = -500;
                this.Left = -500;
            }
            else if (this.Top == -500)
            {
                this.Location = new Point(
                frmParentForm.Location.X + (frmParentForm.Width - this.Width) / 2,
                frmParentForm.Location.Y + (frmParentForm.Height - this.Height) / 2);
//                blnLocate = true;

            }
            if (blnStop)
            {
                this.TopMost = false;
                timer2.Enabled = false;
                blnStop = false;
                this.Close();
                
            }
        }

        private void frmSplash_Shown(object sender, EventArgs e)
        {
            blnCancel = false;
            label1.Text = strHeader;
            this.Activate();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            blnCancel = true;
            label1.Text = "キャンセル中です。しばらくお待ちください...";
        }

        private void frmSplash_FormClosed(object sender, FormClosedEventArgs e)
        {
            txtshori.Text = "";
        }
    }
}
