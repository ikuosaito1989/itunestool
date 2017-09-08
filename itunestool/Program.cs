using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace ITunEsTooL
{
    static class Program
    {
       
        /// <summar1y>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (Type.GetTypeFromProgID("ITunes.Application") == null)
            {
                MessageBox.Show("ITunesがインストールされていません。" ,"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Application.Run(new frmItune());
            
        }
    }
}
