using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ITunEsTooL
{
    public class CommonValue
    {
        public string[] strPaths;

        public string[] strName;
        public string[] strArtist;
        public string[] strAlbum;
        public string[] strLocation;
        public string[] strAlbumArtist;
        public int[] iArtCnt;
        public int[] iPersistentIDHigh;
        public int[] iPersistentIDRow;
        public Boolean[] blnCompilation;
        public string DIALOG_TITLE;
        public string strColor = "";
        public FileVersionInfo strVer = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// 共通項目生成
        /// </summary>
        public void NEWVAL()
        {
            strName = new string[1];
            strAlbum = new string[1];
            strArtist = new string[1];
            strLocation = new string[1];
            strAlbumArtist = new string[1];
            iArtCnt = new int[1];
            iPersistentIDHigh = new int[1];
            iPersistentIDRow = new int[1];
            blnCompilation = new Boolean[1];
        }

        public void COMVAL_initialize()
        {
            strName[0] = "";
            strAlbum[0] = "";
            strArtist[0] = "";
            strLocation[0] = "";
            iArtCnt[0] = 0;
            iPersistentIDHigh[0] = 0;
            iPersistentIDRow[0] = 0;
            blnCompilation[0] = false;

        }
        /// <summary>
        /// 共通項目解放
        /// </summary>
        public void Release()
        {
            strName = null;
            strArtist = null;
            strAlbum = null;
            strLocation = null;
            iArtCnt = null;
        }

        /// <summary>
        /// 共通変数動的配列
        /// </summary>
        /// <param name="cnt"></param>
        public void CommonRerize(int cnt)
        {
            Array.Resize(ref strName, cnt + 1);
            Array.Resize(ref strArtist, cnt + 1);
            Array.Resize(ref strAlbum, cnt + 1);
            Array.Resize(ref strLocation, cnt + 1);
            Array.Resize(ref strAlbumArtist, cnt + 1);
            Array.Resize(ref iArtCnt, cnt + 1);
            Array.Resize(ref iPersistentIDHigh, cnt + 1);
            Array.Resize(ref iPersistentIDRow, cnt + 1);
            Array.Resize(ref blnCompilation, cnt + 1);
        }
        /// <summary>
        /// 共通変数動的配列セット
        /// </summary>
        /// <param name="cnt"></param>
        public void SetRerize(int cnt)
        {
            Array.Resize(ref strName, cnt);
            Array.Resize(ref strArtist, cnt);
            Array.Resize(ref strAlbum, cnt);
            Array.Resize(ref strLocation, cnt);
            Array.Resize(ref strAlbumArtist, cnt);
            Array.Resize(ref iArtCnt, cnt);
            Array.Resize(ref iPersistentIDHigh, cnt);
            Array.Resize(ref iPersistentIDRow, cnt);
            Array.Resize(ref blnCompilation, cnt);

        }

    }
}
