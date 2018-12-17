using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace ITunEsTooL
{
    class Mail
    {
        private string _Host = "smtp.gmail.com";
        private int _Port = 587;

        /// <summary>
        /// ポート番号を指定（既定値:25）
        /// </summary>
        public int Port
        {
            set
            { _Port = value; }
        }
        /// <summary>
        /// SMTPサーバーを指定（既定値:sv1.mail.xdomain.ne.jp）
        /// </summary>
        public string Host
        {
            set
            { _Host = value; }
        }

        /// <summary>
        /// SendMail メール送信
        /// </summary>
        /// <param name="From">送信者</param>
        /// <param name="To">宛先</param>
        /// <param name="Subject">件名</param>
        /// <param name="Body">本文</param>
        /// <param name="ReviewNumber">
        ///         レビュー数
        ///         ※省略可　既定値　3
        /// </param>
        /// <param name="MessageType">
        ///         メッセージタイプ
        ///         
        ///         1　⇒　レビュー
        ///         2　⇒　エラー
        ///         ※省略可　既定値　2
        /// </param>
        /// <returns>
        ///         正常　⇒　空白
        ///         例外　⇒　エラーメッセージ
        /// </returns>
        public String SendMail(string From, string To, string Subject, string Body, int ReviewNumber = 3, int MessageType = 2)
        {
            string strFrom = string.Empty;
            string strBody = string.Empty;
            string strErrMsg = string.Empty;

            try
            {
                strFrom = !IsValidMailAddress(From) ? "itunestool.jp@gmail.com" : From;

                if (Body != "")
                {

                    strBody = MessageType == 1 ? "レビュー数 : " + ReviewNumber.ToString() + "\n" + "\n" + Body : "\n" + Body;

                    using (SmtpClient sc = new SmtpClient())
                    {
                        sc.Credentials = new NetworkCredential(To, "irxgbtffgglufwot");
                        sc.EnableSsl = true;
                        sc.Host = _Host;
                        sc.Port = _Port;
                        sc.Send(strFrom, To, Subject, strBody);
                    }
                }
            }
            catch (System.Exception ex)
            {
                strErrMsg += "System Exception 12512 : " + ex.Message + "\n" + ex.StackTrace + "\n";
            }

            return strErrMsg;
        }

        /// <summary>
        /// 指定された文字列がメールアドレスとして正しい形式か検証する
        /// </summary>
        /// <param name="address">検証する文字列</param>
        /// <returns>正しい時はTrue。正しくない時はFalse。</returns>
        public static bool IsValidMailAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return false;
            }

            try
            {
                System.Net.Mail.MailAddress a =
                    new System.Net.Mail.MailAddress(address);
            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }
    }
}
