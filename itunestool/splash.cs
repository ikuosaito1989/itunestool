using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace ITunEsTooL
{
    class splash
    {
        
        private frmSplash clsSplash = null;
        private Boolean blnCancel = false;
        public Boolean Cancel
        {
            get { return blnCancel; }

        }
        public Boolean SendCacncel
        {
            set
            { blnCancel = value; }
        }
        /// <summary>
        /// スプラッシュ表示
        /// </summary>
        /// <param name="strDaimei"></param>
        /// <param name="frmParentForm"></param>
        /// <param name="strColor"></param>
        public void OpenSplash(string strDaimei,Form frmParentForm,string strColor)
        {
            if (clsSplash != null)
            {
                clsSplash.ParentForm = frmParentForm;
                clsSplash.Daimei = "";
                clsSplash.Daimei = strDaimei;
                clsSplash.CColor = strColor;


                clsSplash.ShowDialog();
            }
        }

        /// <summary>
        /// スプラッシュクローズ
        /// </summary>
        public void CloseSplash()
        {
            clsSplash.Stop = true;
            clsSplash.Daimei = "";
            clsSplash.Parcent = 0;
            clsSplash.Zero = 0;
            clsSplash = null;
        }

        /// <summary>
        /// 処理件数を送る関数
        /// </summary>
        /// <param name="cnt"></param>
        public void SendData(int cnt)
        {
            clsSplash.Parcent = cnt;
            blnCancel = clsSplash.Cancel;                         

        }

        /// <summary>
        /// スプラッシュにキャンセルメッセージを送る
        /// </summary>
        /// <param name="Cancel"></param>
        public void SendCan(Boolean Cancel)
        {
            blnCancel = Cancel;
            clsSplash.SendCancel = blnCancel;
            clsSplash.HeadMessage = "少々お待ち下さい...";

        }
        /// <summary>
        /// メッセージを渡す
        /// </summary>
        /// <param name="cnt"></param>
        public void SendMessage(string strMessage)
        {
            clsSplash.Message = strMessage;
        }
        /// <summary>
        /// 一時的な非表示
        /// </summary>
        /// <param name="cnt"></param>
        public void SendVisible(Boolean blnVIS)
        {
            clsSplash.Daimei = "";
            clsSplash.bVisible= blnVIS;
        }
        /// <summary>
        /// 処理するデータの合計値を渡す
        /// </summary>
        /// <param name="iGoukei"></param>
        public void SendDataALL(int iGoukei)
        {
            clsSplash = new frmSplash();
            clsSplash.Daimei = "";
            clsSplash.ALL = iGoukei;
            clsSplash.Zero = 0;
        }
        
    }
}
