using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;

namespace ITunEsTooL
{
    class Dialog
    {
        public string OpenFileDialog(string strTitle = "開くファイルを選択してください", string DeFPath = @"C:\")
        {
            string strPath;

            OpenFileDialog ofd = new OpenFileDialog();

            ofd.FileName = "";
            ofd.InitialDirectory = Path.GetDirectoryName(DeFPath);
            ofd.FilterIndex = 2;
            ofd.Title = strTitle;
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            strPath = ofd.ShowDialog() == DialogResult.OK ? Convert.ToString(ofd.FileName) : "";

            return strPath;
        }

        public string OpenFolderDialog(Form frmCurrentForm, string strDePath = "フォルダを指定してください。", string strpath = @"C:\")
        {
            string strPath;

            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.Description = strDePath;

            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.SelectedPath = strpath;
            fbd.ShowNewFolderButton = true;

            strPath = fbd.ShowDialog(frmCurrentForm) == DialogResult.OK ? Convert.ToString(fbd.SelectedPath) : "";

            return strPath;
        }

    }
}
