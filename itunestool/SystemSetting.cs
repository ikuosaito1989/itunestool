using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NetFwTypeLib;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;

namespace ITunEsTooL
{
    class SystemSetting
    {

        /// <summary>
        /// シャットダウン
        /// </summary>
        /// <param name="ShutdownParameter">
        ///     1　⇒　シャットダウン
        ///     2　⇒　再起動
        /// </param>
        public string Shutdown(int ShutdownParameter = 1)
        {
            string strErrMsg = string.Empty;
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "shutdown.exe";
                psi.Arguments = ShutdownParameter == 1 ? "-s -f -t 0" : "-r -f -t 0";
                psi.CreateNoWindow = true;
                Process p = Process.Start(psi);
            }
            catch (Exception ex)
            {
                strErrMsg += "System Exception 14411 : " + ex.Message + "\n";
            }
            return strErrMsg;
        }

        /// <summary>
        /// アプリケーションを指定してファイアウォールを通過させる。
        /// </summary>
        /// <param name="title"></param>
        /// <param name="filePath"></param>
        static public void AuthorizeApplication(string title, string filePath)
        {
            try
            {
                Type type = Type.GetTypeFromProgID("HNetCfg.FwAuthorizedApplication");
                INetFwAuthorizedApplication fwAuth = (INetFwAuthorizedApplication)Activator.CreateInstance(type);
                fwAuth.Name = title;
                fwAuth.ProcessImageFileName = Path.GetFullPath(filePath);
                fwAuth.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL;
                fwAuth.IpVersion = NET_FW_IP_VERSION_.NET_FW_IP_VERSION_ANY;
                fwAuth.Enabled = true;

                INetFwMgr manager = GetFirewallManager();
                manager.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(fwAuth);
            }
            catch (System.Exception)
            {
            }
        }
        private static INetFwMgr GetFirewallManager()
        {
            Type type = Type.GetTypeFromProgID("HNetCfg.FwMgr");
            return (INetFwMgr)Activator.CreateInstance(type);
        }

        /// <summary>
        /// MouseDown時に座標を取得（MouseDownイベントに使用）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns>座標（Point）</returns>
        public void GetPoint(object sender,MouseEventArgs e, ref Point mousePoint)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                mousePoint = new Point(e.X, e.Y);
            }
        }
        /// <summary>
        /// MouseMove時にフォームを移動（MouseMoveイベントに使用）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="me"></param>
        /// <param name="mousePoint"></param>
        public Point MoveForm(object sender,MouseEventArgs e,Point mousePoint,ref Point mePoint)
        {
            int intLeft = 0;
            int intTop = 0;

            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {

                intLeft += e.X - mousePoint.X;
                intTop += e.Y - mousePoint.Y;
                mePoint = new Point(intLeft, intTop);
            }

            return mePoint;
        }

        /// <summary>
        /// UACが有効になっているかを調べる
        /// </summary>
        /// <returns>UACが有効になっている時はtrue。</returns>
        public bool IsUacEnabled()
        {
            Microsoft.Win32.RegistryKey regKey =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
            if (regKey == null)
                return false;
            int val = (int)regKey.GetValue("EnableLUA", 0);
            return val != 0;
        }

        /// <summary>
        /// 管理者権限がチェックする
        /// </summary>
        /// <returns></returns>
        public bool IsAdministrator()
        {
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            WindowsPrincipal wp = (WindowsPrincipal)Thread.CurrentPrincipal;
            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// Win7以降であるかチェック
        /// </summary>
        /// <returns></returns>
        public Boolean Win7Check()
        {
            //OSのバージョン情報
            System.OperatingSystem os = System.Environment.OSVersion;

            //Windows NT
            if (os.Platform == PlatformID.Win32NT)
            {
                if (os.Version.Major >= 6)
                {
                    if (os.Version.Minor >= 2)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// エラーファイルをサーバにアップロードする
        /// </summary>
        /// <param name="strErrLogPath"></param>
        private void ErrMsgUpload(string strErrLogPath)
        {
            try
            {
                string upFile = strErrLogPath;

                Uri u = new Uri("ftp://aaa" + Path.GetFileName(strErrLogPath));

                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);

                ftpReq.Credentials = new System.Net.NetworkCredential("aaaa", "aaaa");
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
                ftpReq.KeepAlive = false;
                ftpReq.UseBinary = false;
                ftpReq.UsePassive = false;

                System.IO.Stream reqStrm = ftpReq.GetRequestStream();

                using (System.IO.FileStream fs = new System.IO.FileStream(
                    upFile, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {

                    byte[] buffer = new byte[1024];
                    while (true)
                    {
                        int readSize = fs.Read(buffer, 0, buffer.Length);
                        if (readSize == 0)
                            break;
                        reqStrm.Write(buffer, 0, readSize);
                    }
                    fs.Close();
                }
                reqStrm.Close();

                System.Net.FtpWebResponse ftpRes =
                    (System.Net.FtpWebResponse)ftpReq.GetResponse();

                Console.WriteLine("{0}: {1}", ftpRes.StatusCode, ftpRes.StatusDescription);

                ftpRes.Close();
            }
            catch (System.Exception)
            {
            }
        }

    }
}
