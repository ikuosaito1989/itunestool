using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ITunEsTooL
{
    class FileOperation
    {
        /// <summary>
        /// ファイル名に使用出来ない文字を変換
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ValidFileName(string s)
        {

            string valid = s;
            char[] invalidch = Path.GetInvalidFileNameChars();

            foreach (char c in invalidch)
            {
                valid = valid.Replace(c, '_');
            }
            return valid;
        }
    }
}
