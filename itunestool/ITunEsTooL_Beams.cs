using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITunEsTooL
{
    class ITunEsTooL_Beams
    {
        /// <summary>
        /// @@@一致した曲添え字を取得@@@
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strAlbum"></param>
        /// <param name="strArtist"></param>
        /// <param name="strAlbumArtist"></param>
        /// <param name="blnCompilation"></param>
        /// <returns>Machi -> Index Non Machi -> -1</returns>
        public int GetMusicMachiIndex(ref CommonValue COMMON, int iPersistentIDHigh, int iPersistentIDRow)
        {
            int index1 = 0;
            int index2 = 0;

            int i = 0;
            for (i = 0; i < COMMON.strName.Length; i++)
            {

                index1 = Array.IndexOf(COMMON.iPersistentIDHigh, iPersistentIDHigh, i);
                index2 = Array.IndexOf(COMMON.iPersistentIDRow, iPersistentIDRow, i);
                if (index1 == index2)
                {
                    return index1;
                }
                //小さい方に合わせる
                if (index1 < index2)
                    i = i + index1;
                else
                    i = i + index2;
            }
            return -1;
        }
        ///<summary>
        /// @@@一致したアルバム添え字を取得@@@
        /// </summary>
        /// <param name="strAlbum[]"></param>
        /// <param name="strAlbumArtist[]"></param>
        /// <param name="blnCompilation[]"></param>
        /// <param name="strSchAlbum"></param>
        /// <param name="strSchAlbumArtist"></param>
        /// <param name="blnSchCompilation"></param>
        /// <returns>Machi -> Index Non Machi -> -1</returns>
        public int GetAlbumMachiIndex(string[] strAlbum, string[] strArtist, string[] strAlbumArtist, Boolean[] blnCompilation,
                                        string strSchAlbum, string strSchArtist, string strSchAlbumArtist, Boolean blnSchCompilation)
        {
            int icnt;

            for (icnt = 0; icnt < strAlbum.Length; icnt++)
            {
                icnt = Array.IndexOf(strAlbum, strSchAlbum, icnt);
                if (icnt == -1)
                    return -1;

                if (strAlbum[icnt] == strSchAlbum)
                {
                    if (blnSchCompilation && blnCompilation[icnt] == blnSchCompilation)
                    {
                        return icnt;
                    }
                    else if (strSchAlbumArtist != "" && strAlbumArtist[icnt] == strSchAlbumArtist)
                    {
                        return icnt;
                    }
                    else if (strArtist[icnt] == strSchArtist)
                    {
                        return icnt;
                    }
                }
            }
            return -1;
        }
        /// <summary>
        /// アルバムの曲数を返すメソッド
        /// </summary>
        /// <param name="AlbumName"></param>
        /// <param name="ArtistName"></param>
        /// <returns>ある　→　曲数、ない　→　-1</returns>
        public int GetAlbumCount(string AlbumName, string strArtist, string strAlbumArtist, Boolean blnCompilation, ref CommonValue COMMON)
        {
            int icnt = 0;
            int imax = 0;

            for (icnt = 0; icnt < COMMON.strName.Length; icnt++)
            {
                icnt = Array.IndexOf(COMMON.strAlbum, AlbumName, icnt);
                if (icnt == -1)
                    break;
                if (COMMON.strAlbum[icnt] == AlbumName)
                {
                    if (blnCompilation && COMMON.blnCompilation[icnt] == blnCompilation)
                    {
                        imax++;
                    }
                    else if (strAlbumArtist != "" && COMMON.strAlbumArtist[icnt] == strAlbumArtist)
                    {
                        imax++;
                    }
                    else if (COMMON.strArtist[icnt] == strArtist)
                    {
                        imax++;
                    }
                }
            }
            return imax;
        }
        /// <summary>
        /// 検索対象のアルバムかチェック
        /// </summary>
        /// <param name="tgtAlbum"></param>
        /// <param name="tgtArtist"></param>
        /// <param name="tgtAlbumArtist"></param>
        /// <param name="tgtCompilation"></param>
        /// <param name="schAlbum"></param>
        /// <param name="schArtist"></param>
        /// <param name="schAlbumArtist"></param>
        /// <param name="schCompilation"></param>
        /// <param name="icnt"></param>
        /// <returns></returns>
        public Boolean MachiAlbum(CommonValue COMMON, string schAlbum, string schArtist,
                                                    string schAlbumArtist, Boolean schCompilation, ref int icnt)
        {
            Boolean blnret = false;

            if (COMMON.strAlbum[icnt] == schAlbum)
            {
                if (schCompilation && COMMON.blnCompilation[icnt] == schCompilation)
                {
                    blnret = true;
                }
                else if (schAlbumArtist != "" && COMMON.strAlbumArtist[icnt] == schAlbumArtist)
                {
                    blnret = true;
                }
                else if (COMMON.strArtist[icnt] == schArtist)
                {
                    blnret = true;
                }
            }

            return blnret;
        }
    }
}
