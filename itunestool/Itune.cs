using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Xml;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.NetworkInformation;
using System.Drawing.Imaging;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using iTunesLib;
using Microsoft.Win32;
using ITunEsTooL.Model;
using RestSharp;
using Newtonsoft.Json;
using System.Configuration;

namespace ITunEsTooL
{
    public partial class frmItune : Form
    {
        
#region 定数宣言用領域

        private int pSplashNum = 1;
        private int pSelectCnt = 0;
        private string Message = string.Empty;
        private string pErrMessage = string.Empty;
        private const string TRUE = "True";
        private const string FALSE = "False";
        private Boolean pAutoShutdown = false;
        private Boolean pDoukiFlg = false;
        private Boolean blnSaishin = true;
        private Boolean blnshorichu = true;
        private Boolean PicCheckFlag = false;
        private Point mousePoint;
        private FileVersionInfo ver = null;
        private IITTrack TR = null;
        private iTunesApp itunesD;
        private SystemSetting SysSet = null;
        private ImageOperation ImgOpe = null;
        private FileOperation FileOpe = null;
        private CommonValue COMVAL;
        private splash splash;
        private Start StartSplash;
        private string Domain = ConfigurationManager.AppSettings["Domain"];
        private struct ConfData
        {
            public string pxmlPath;
            public string pArtWork;
            public string pDelArt;
            public string pColor;
            public string pSaveArtwork;
            public string pSavePicture;
            public string pSmallArtwork;
            public string pComp;
            public string ppath;
            public string pbkup;
            public string pSync;
            public string pTrackCnt;
            public string pErrLog;
            public string pAutoShutdown;
            public string pDate;
            public string pReview;
        }

        private ConfData CONF;

        /// <summary>
        /// メッセージのヘッダ文字
        /// </summary>
        private struct HeadMsg
        {
            public const string HEAD_EXCEPTION = "例外";
            public const string ATTENTION = "注意";
            public const string COMPLETE = "完了";
            public const string NOTICE = "お知らせ";
            public const string CHECK = "確認";
            public const string CONNECT_ITUNES = "iTunesと同期";
            public const string MAKE_PLAYLIST = "プレイリスト作成";
        }

        private struct HeaderName
        {
            
            public const string TITLE = "ITunEsTooL - iTunesの不満解消ツール";
            public const string ICHIRAN = "曲名一覧";
            public const string GET_ITUNEDATA = "iTunesのデータを取得しています...";
            public const string CSV = "ＣＳＶファイルを作成しています...";
            public const string EXCEPTION = "予期せぬエラーが発生しました。";
        
        }

        private struct frmColor
        {
            public const string SKYBLUE = "SkyBlue";
            public const string LIGHTGREEN = "LightGreen";
            public const string TOMATO = "Tomato";
            public const string VIOLET = "Violet";
        }

        

#endregion

        /// <summary>
        /// イニシャライズ
        /// </summary>
        public frmItune()
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
        /// AnimateWindow 列挙型
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

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmItune_Load(object sender, EventArgs e)
        {
           try
            {
                string strErrMsg = string.Empty;
                string strNewVer = string.Empty;
                Assembly myAssembly = Assembly.GetEntryAssembly();

                if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
                {
                    Msgbox("ITunEsTooLは既に起動しています。", HeadMsg.NOTICE, 2);
                    this.Close();
                    return;
                }

                ver = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                CONF.ppath = Path.GetDirectoryName(myAssembly.Location);
                                
                Constructor();

                SetConf();

                switch (CONF.pColor)
                {
                    case frmColor.SKYBLUE:
                        StartSplash.Color = Color.SkyBlue;
                        break;
                    case frmColor.LIGHTGREEN:
                        StartSplash.Color = Color.LightGreen;
                        break;
                    case frmColor.TOMATO:
                        StartSplash.Color = Color.Tomato;
                        break;
                    case frmColor.VIOLET:
                        StartSplash.Color = Color.Violet;
                        break;
                }

                backgroundWorker1.RunWorkerAsync();

                Initialize(StartSplash);

                if (!SysSet.IsAdministrator())
                {
                    Msgbox("ITunEsTooLは管理者のみが使用することが可能です。" + "\n" +
                           "「管理者として実行(A)...」で起動して下さい。",HeadMsg.NOTICE, 7);
                    this.Close();
                    return;
                }                

                ClassicCorrespondence();

                strErrMsg = itunesObject();
                if (strErrMsg != "")
                {
                    pErrMessage += "System Exception 0001 : " + strErrMsg + "\n";
                    Msgbox("iTunesが起動出来ませんでした。" + "\n" + "既に起動されている場合は、iTunesを閉じてからITunEsTooLを起動して下さい。", HeadMsg.ATTENTION, 5);
                    this.Close();
                    return;
                }

                lblTitle.Text = HeaderName.TITLE;

                strNewVer = getNewVersion();

                StartSplash.Stop = true;
                while (true)
                {
                    if (StartSplash.CloseRet)
                        break;
                    Thread.Sleep(100);
                }

                AnimateWindow(this.Handle, 300, (uint)(AnimateWindowFlags.AW_HOR_NEGATIVE | AnimateWindowFlags.AW_BLEND));
                this.Activate();

                if (strNewVer != "")
                {
                    if (NetworkInterface.GetIsNetworkAvailable() && strNewVer != ver.FileVersion)
                    {
                        DialogResult YorN = DialogResult.No;
                        YorN = Msgbox("ITunEsTooLは最新のバージョンではありません。" + "\n" + "ダウンロードサイトに遷移しますか？", HeadMsg.NOTICE, 3);
                        if (YorN == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["Domain"]);
                        }
                    }
                }

                pSplashNum = 1;

                StartEventHandler();
            }
            catch (System.Exception ex)
            {
                if (File.Exists(CONF.pxmlPath))
                    DeleteFile(CONF.pxmlPath);
                Msgbox("System Exception 1001" + "\n" + "ITunEs TooLを終了します。", HeadMsg.HEAD_EXCEPTION,4);
                pErrMessage += "System Exception 1001 : " + ex.Message + "\n";
                this.Close();
            }
        }

        private void StartEventHandler()
        {
            SystemEvents.UserPreferenceChanged +=
                    new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
            itunesD.OnDatabaseChangedEvent += new _IiTunesEvents_OnDatabaseChangedEventEventHandler(itunes_OnDatabaseChangedEvent);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private void Constructor()
        {
            COMVAL = new CommonValue();
            splash = new splash();
            StartSplash = new Start();
            SysSet = new SystemSetting();
            ImgOpe = new ImageOperation();
            FileOpe = new FileOperation();
            pSplashNum = 2;
        }
        
        /// <summary>
        /// iTunesオブジェクト生成
        /// </summary>
        /// <returns></returns>
        private string itunesObject()
        {
            string strErrMsg = string.Empty;

            try{
                if (itunesD == null)
                {
                    itunesD = new iTunesAppClass();
                }
            }
            catch (System.Exception ex)
            {
                strErrMsg = ex.Message;    
            }
            return strErrMsg;

        }

        /// <summary>
        /// iTunes上のデータが変更された時
        /// </summary>
        /// <param name="deletedObjectIDs"></param>
        /// <param name="changedObjectIDs"></param>
        private void itunes_OnDatabaseChangedEvent(object deletedObjectIDs, object changedObjectIDs)
        {
            
            try{
                if (blnshorichu == false && itunesD.LibraryPlaylist.Tracks.Count != Convert.ToDecimal(CONF.pTrackCnt))
                    blnSaishin = false;
                Thread.Sleep(1000);
            }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 3046 : " + ex.Message + "\n";
                return;
            }

        }

        /// <summary>
        /// 必須ファイルチェック
        /// </summary>
        /// <returns></returns>
        private string CheckExistsFiles()
        {
            string strErrMsg = string.Empty;
            if (!File.Exists(CONF.ppath + @"\ItunEsTooL.exe"))
                strErrMsg += "ItunEsTooL.exe" + "\n";
            if(!File.Exists(CONF.ppath + @"\Interop.iTunesLib.dll"))
                strErrMsg += "Interop.iTunesLib.dll" + "\n";
            if(!Directory.Exists(CONF.ppath + @"\Log"))
                strErrMsg += "Log" + "\n";
            if(!Directory.Exists(CONF.ppath + @"\Artwork"))
                strErrMsg += "Artwork" + "\n";

            return strErrMsg;
            
        }
       
        /// <summary>
        /// 設定ファイル読み込み
        /// </summary>
        private void SetConf()
        {

            CONF.pTrackCnt = "0";
            if (!Read_Conf() || !File.Exists(CONF.ppath + @"\ITunEsTooL.config"))
            {
                Make_Conf();
                Read_Conf();
            }

        }
        /// <summary>
        /// バージョン取得
        /// </summary>
        public string getNewVersion()
        {
            string strVer = string.Empty;
            WebClient wc = new WebClient();

            try{

                if(!NetworkInterface.GetIsNetworkAvailable())
                    throw new Exception();

                Stream st = wc.OpenRead(Domain);
                
                Encoding enc = Encoding.GetEncoding("utf-8");
                StreamReader sr = new StreamReader(st, enc);
                string html = sr.ReadToEnd();
                sr.Close();
                st.Close();

                Regex reg = null;
                reg = new Regex(@"【.*】", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                strVer = Convert.ToString(reg.Match(html));
                strVer = strVer.Replace("【ver ", "");
                strVer = strVer.Replace("+?】", "");
                strVer = strVer.Substring(0, 7);
                if (!Regex.IsMatch(strVer, @"^\d.\d.\d.\d",
                   System.Text.RegularExpressions.RegexOptions.ECMAScript))
                {
                    throw new Exception();
                }
                return strVer;
            }
            catch (System.Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 初期設定
        /// </summary>
        private void Initialize(Start StartSplash)
        {
            string strErrMsg = string.Empty;
            Decimal ihanten = 0;

            if (File.Exists(CONF.pxmlPath))
                if (!Read_Music())
                    DeleteFile(CONF.pxmlPath);

            txtErrLog.Text = CONF.pErrLog;
            txtxmlPath.Text = CONF.pxmlPath;
            txtArtWork.Text = CONF.pArtWork;

            if (CONF.pbkup == "1")
                rdoUwagaki.Checked = true;
            else
                rdoSabun.Checked = true;


            if (CONF.pDelArt == FALSE)
            {
                chkArtworkDel.Checked = false;
                ChangeCheckBoxParameter(ref chkArtworkDel, ref CONF.pDelArt);
            }
            if (CONF.pComp == FALSE)
            {
                chkComp.Checked = false;
                ChangeCheckBoxParameter(ref chkComp, ref CONF.pComp);
            }
            if (CONF.pAutoShutdown == FALSE)
            {
                chkAutoShutdown.Checked = false;
                ChangeCheckBoxParameter(ref chkAutoShutdown, ref CONF.pAutoShutdown);
            }

            switch (CONF.pSaveArtwork)
            {
                case "1":
                    rdoArtName.Checked = true;
                    break;
                case "2":
                    rdoArtAlbum.Checked = true;
                    break;
                case "3":
                    rdoArtArtist.Checked = true;
                    break;
                case "4":
                    rdoNmArb.Checked = true;
                    break;
                case "5":
                    rdoArbArt.Checked = true;
                    break;
            }

            switch (CONF.pSavePicture)
            {
                case "jpg":
                    rdoJpg.Checked = true;
                    break;
                case "png":
                    rdoPng.Checked = true;
                    break;
                case "bmp":
                    rdoBmp.Checked = true;
                    break;
            }

            ihanten = Convert.ToDecimal(CONF.pSmallArtwork);
            if (ihanten > 0)
            {
                numUDSize.Value = ihanten;
                lblNum.Text = ihanten.ToString();
                chkSmalGazou.Checked = true;
            }
            else
            {
                ihanten = -ihanten;
                numUDSize.Value = ihanten;
                lblNum.Text = ihanten.ToString();
                SmalGazou();
            }

            DoukiColor();
            pictureBox5.AllowDrop = true;
            groupBox4.AllowDrop = true;
            if (SysSet.Win7Check() || SysSet.IsUacEnabled())
                toolTip1.SetToolTip(pictureBox5, "右クリックメニューで様々な設定が出来ます。"+ "\n" + "【Ctrl + V】で画像の貼り付けが可能です。");
            else
                toolTip1.SetToolTip(pictureBox5, "画像ファイルをドラッグして設定することが可能です。" + "\n" + "また、右クリックメニューで様々な設定が出来ます。"+ "\n" + "【Ctrl + V】で画像の貼り付けが可能です。");

            toolTip1.SetToolTip(tabControl1, "F1キーでマニュアルが表示されます（インターネット接続時のみ）");
            toolTip1.SetToolTip(txtBefor, "ダブルクリックでドラッグした曲の" + "\n" + "詳細確認が出来ます。");
            toolTip1.SetToolTip(txtSerchChr, "【.*】と入力するとすべての曲が" + "\n"
                                + "CSVに出力されます。" + "\n"
                                + "ダブルクリックすると【.*】が自動入力されます。");
            toolTip1.SetToolTip(grpName, "アーティスト＋アルバムは" + "\n"
                                + "アーティスト名のフォルダ内に" + "\n"
                                + "アルバム名のフォルダが作成されます。");

            SetName();
        }

        /// <summary>
        /// ファイル削除　読み取り専用外す
        /// </summary>
        /// <param name="stFilePath"></param>
        private void DeleteFile(string stFilePath)
        {
            int retry = 0;
            FileInfo cFileInfo = new FileInfo(stFilePath);


            try{
                if (cFileInfo.Exists)
                {

                    while (retry <= 2)
                    {
                        if ((cFileInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                            cFileInfo.Attributes = FileAttributes.Normal;
                        try
                        {
                            cFileInfo.Delete();
                        }
                        catch (SystemException)
                        { 
                            if(retry == 2) throw new Exception();
                        }
                        Thread.Sleep(200);
                        retry++;
                    }
                }
            }
            catch (SystemException ex)
            {
                pErrMessage += "System Exception 5624 : " + ex.Message + "\n";
            }
        }
        
        /// <summary>
        /// フォームクローズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmItune_FormClosed(object sender, FormClosedEventArgs e)
        {
            StreamWriter swErrLog = null;
            string strTime;
            string strErrLogPath;

            try
            {
                this.Visible = false;
                this.ShowInTaskbar = false;

                //try{int i = itunesD.SoundVolume;}
                //catch (Exception) { return; }

                if (COMVAL == null)
                    return;

                if (!pAutoShutdown)
                {
                    int intDay = int.Parse(DateTime.Now.ToString("dd"));
                    if (CONF.pReview == TRUE && (intDay % 2) == 0 && SysSet.IsAdministrator())
                    {
                        Review frmReview = new Review();
                        frmReview.Color = pictureBox1.BackColor;
                        frmReview.ShowDialog();
                        CONF.pReview = frmReview.Jikai;
                    }
                }
                                
                WriteXML();                
                Update_Conf();                

                if (pAutoShutdown)
                {
                    SysSet.Shutdown(1);
                    Application.Exit();
                }

                if (pErrMessage != "" && Directory.Exists(CONF.pErrLog))
                {
                    
                    if (pErrMessage.Contains("System Exception"))
                    {
                        OperatingSystem os = Environment.OSVersion;
                        pErrMessage += "\n";
                        pErrMessage += (itunesD != null) ? "iTunes Versin : " + itunesD.Version + "\n" : "";
                        pErrMessage += "OS Versin : " + os.Version.ToString() + "\n";
                        pErrMessage += "UAC Enabled : " + SysSet.IsUacEnabled().ToString() + "\n";
                        pErrMessage += "App Versin : " + ver.FileVersion;
                        strTime = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
                        strErrLogPath = CONF.pErrLog + @"\" + "." + strTime + "(ErrorLog).log";
                        swErrLog = new StreamWriter(strErrLogPath, true, System.Text.Encoding.GetEncoding("Shift_Jis"));
                        swErrLog.Write(pErrMessage);
                        swErrLog.Close();
                        swErrLog = null;

                        string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                        CrashReport frmReport = new CrashReport();
                        frmReport.Color = pictureBox1.BackColor;
                        frmReport.Path = strErrLogPath;
                        frmReport.ErrMsg = pErrMessage;
                        DialogSaizenmen();
                        frmReport.ShowDialog();

                        DeleteFile(strErrLogPath);
                    }
                    
                }
                SystemEvents.UserPreferenceChanged -=
                    new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
            }
            catch (System.Exception ex)
            {
                
                pErrMessage += "System Exception 1012 : " + ex.Message + "\n";
                Msgbox("System Exception 1012" + "\n" + "ITunEs TooLを終了します。", HeadMsg.HEAD_EXCEPTION, 4);
            }
            finally
            {

                if (itunesD != null)
                    Marshal.ReleaseComObject(itunesD);
                if (TR != null)
                    Marshal.ReleaseComObject(TR);
            }
        }

        /// <summary>
        /// XML更新
        /// </summary>
        private Boolean WriteXML()
        {
            int icnt = 0;
            string strDir = string.Empty;
            string strFile = string.Empty;
            string strXmlPath = string.Empty;
            XmlTextWriter xtw = null;

            try
            {
                if (COMVAL == null  || COMVAL.strName == null ||  !SysSet.IsAdministrator())
                    return true;

                strDir = Path.GetDirectoryName(CONF.pxmlPath);
                strFile = @"\" + Path.GetFileName(CONF.pxmlPath);
                strXmlPath = strDir + strFile;
          
                xtw = new XmlTextWriter(strXmlPath, Encoding.Unicode);
                WriteHeaderXml(ref xtw, "ITunesDataLibrary");

                foreach (string strName in COMVAL.strName)
                {
                    WriteElementXml(ref xtw, COMVAL.iPersistentIDHigh[icnt], COMVAL.iPersistentIDRow[icnt], COMVAL.blnCompilation[icnt],
                                    COMVAL.strName[icnt], COMVAL.strAlbum[icnt], COMVAL.strArtist[icnt],
                                    COMVAL.strLocation[icnt], COMVAL.strAlbumArtist[icnt],Convert.ToString(COMVAL.iArtCnt[icnt]));
                    icnt++;
                }

                CloseElementXml(ref xtw);
                xtw.Close();
                //File.Copy(strXmlPath, CONF.pxmlPath, true);
                //DeleteFile(strXmlPath);
                return true;
            }
            catch (System.Exception ex)
            {        
                if(xtw != null)
                    xtw.Close();
//                try { DeleteFile(strXmlPath); }catch (Exception) { }
                
                COMVAL.Release();
                COMVAL = null;
                pErrMessage += "System Exception 1187 : " + ex.Message + "\n";
                return false;
            }
        }
        /// <summary>
        /// ヘッダ生成(xml)
        /// </summary>
        /// <param name="xtw"></param>
        /// <param name="title"></param>
        /// <param name="StartTag"></param>
        /// <param name="StartTag2"></param>
        private void WriteHeaderXml(ref XmlTextWriter xtw, string title, string StartTag = "SongList", string StartTag2 = "head")
        {
            try{
                xtw.Formatting = System.Xml.Formatting.Indented;

                xtw.WriteStartDocument();
                xtw.WriteStartElement(StartTag);
                xtw.WriteStartElement(StartTag2);
                xtw.WriteElementString("title", title);
                xtw.WriteEndElement();
            }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 2355 : " + ex.Message + "\n";
            }
        }
        /// <summary>
        /// 曲要素設定(xml)
        /// </summary>
        /// <param name="xtw"></param>
        /// <param name="ITunesData"></param>
        private void WriteElementXml(ref XmlTextWriter xtw, int high, int row, Boolean blnCompilation, params string[] ITunesData)
        {
            string[] separator = { "\r\n" };

            try{
                xtw.WriteStartElement("Track");                                     //開始タグ
                xtw.WriteElementString("Name", ITunesData[0]);                      //曲名
                xtw.WriteElementString("Album", ITunesData[1]);                     //アルバム
                xtw.WriteElementString("Artist", ITunesData[2]);                    //アーティスト
                xtw.WriteElementString("Location", ITunesData[3]);                  //パス
                xtw.WriteElementString("AlbumArtist", ITunesData[4]);               //アルバムアーティスト
                xtw.WriteElementString("ArtworkCount", ITunesData[5]);              //アートワーク数
                xtw.WriteElementString("PersistentIDHigh", high.ToString());        //High Number
                xtw.WriteElementString("PersistentIDrow", row.ToString());          //Low  Number
                xtw.WriteElementString("Compilation", blnCompilation.ToString());   //Compilation
                xtw.WriteEndElement();                                               //Trackタグを閉じる
            }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 9845 : " + ex.Message + "\n";
            }
        }
        /// <summary>
        /// フッダ作成
        /// </summary>
        /// <param name="xtw"></param>
        /// <param name="?"></param>
        private void CloseElementXml(ref XmlTextWriter xtw)
        {
            try{
                xtw.WriteEndElement();
                xtw.WriteEndDocument();
            }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 9846 : " + ex.Message + "\n";
            }
        }

        /// <summary>
        /// 設定ファイル読み込み
        /// </summary>
        private Boolean Read_Conf()
        {
            XmlTextReader Conf_reader = null;

            try{
                Conf_reader = new XmlTextReader(CONF.ppath + @"\ITunEsTooL.config");
                while (Conf_reader.Read())
                {
                    if (Conf_reader.NodeType == XmlNodeType.Element)
                    {
                        switch (Conf_reader.LocalName)
                        {
                            case "ErrorLogPath":
                                CONF.pErrLog = Conf_reader.ReadString();
                                break;
                            case "XmlPath":
                                CONF.pxmlPath = Conf_reader.ReadString();
                                break;
                            case "DeleteArtwork":
                                CONF.pDelArt = Conf_reader.ReadString();
                                break;
                            case "ArtworkPath":
                                CONF.pArtWork = Conf_reader.ReadString();
                                break;
                            case "SaveArtworkName":
                                CONF.pSaveArtwork = Conf_reader.ReadString();
                                break;
                            case "SavePictureName":
                                CONF.pSavePicture = Conf_reader.ReadString();
                                break;
                            case "SmallArtworkSize":
                                CONF.pSmallArtwork = Conf_reader.ReadString();
                                break;
                            case "InCompilation":
                                CONF.pComp = Conf_reader.ReadString();
                                break;
                            case "BackupType":
                                CONF.pbkup = Conf_reader.ReadString();
                                break;                                
                            case "iTunesSync":
                                CONF.pSync = Conf_reader.ReadString();
                                break;
                            case "Color":
                                CONF.pColor = Conf_reader.ReadString();
                                break;
                            case "TrackCount":
                                CONF.pTrackCnt = Conf_reader.ReadString();
                                break;
                            case "UpdateDate":
                                CONF.pDate = Conf_reader.ReadString();
                                lblkoushin.Text = CONF.pDate == "なし" ? "" : CONF.pDate;
                                break;
                            case "Review":
                                CONF.pReview = Conf_reader.ReadString();
                                break;
                            case "AutoShutdown":
                                CONF.pAutoShutdown = Conf_reader.ReadString();
                                break;
                           
                        }
                    }
                }
                switch (CONF.pColor)
                {

                    case frmColor.SKYBLUE:
                        SetColor(CONF.pColor);
                        rdoskyBlue.Checked = true;
                        break;

                    case frmColor.LIGHTGREEN:
                        SetColor(CONF.pColor);
                        rdoGreen.Checked = true;
                        break;
                    case frmColor.TOMATO:
                        SetColor(CONF.pColor);
                        rdoTomato.Checked = true;
                        break;
                    case frmColor.VIOLET:
                        SetColor(CONF.pColor);
                        rdoviolet.Checked = true;
                        break;
                }

                Conf_reader.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                Conf_reader.Close();
                pErrMessage += "System Exception 1014 : " + ex.Message + "\n";
                return false;
            }
        }
        
        /// <summary>
        /// 音楽情報取得
        /// </summary>
        private Boolean Read_Music(string strpath = "")
        {
            int cnt = 0;
            int Trackcnt = 0;
            string strRead = string.Empty;

            XmlTextReader reader = null;

            if (strpath == "")
                strpath = CONF.pxmlPath;
            try
            {

                COMVAL.NEWVAL();                
                COMVAL.COMVAL_initialize();
                reader = new XmlTextReader(strpath);
                COMVAL.CommonRerize(cnt);

                while (reader.Read())
                {
                    if (reader.LocalName == "Track")
                        Trackcnt++;
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.LocalName)
                        {
                            case "Name":
                                COMVAL.strName[cnt] = reader.ReadString();
                                break;
                            case "Album":
                                COMVAL.strAlbum[cnt] = reader.ReadString();
                                break;
                            case "Artist":                                
                                COMVAL.strArtist[cnt] = reader.ReadString();
                                break;
                            case "Location":
                                COMVAL.strLocation[cnt] = reader.ReadString();
                                break;
                            case "AlbumArtist":
                                COMVAL.strAlbumArtist[cnt] = reader.ReadString();
                                break;
                            case "ArtworkCount":                                
                                COMVAL.iArtCnt[cnt] = int.Parse(reader.ReadString());
                                break;
                            case "PersistentIDHigh":                                
                                COMVAL.iPersistentIDHigh[cnt] = int.Parse(reader.ReadString());
                                break;
                            case "PersistentIDrow":
                                COMVAL.iPersistentIDRow[cnt] = int.Parse(reader.ReadString());
                                break;
                            case "Compilation":
                                if (reader.ReadString() == TRUE)
                                    COMVAL.blnCompilation[cnt] = true;
                                else
                                    COMVAL.blnCompilation[cnt] = false;
                                break;    

                        }
                    }
                    if (Trackcnt == 2)
                    {
                        cnt++;
                        Trackcnt = 0;
                        COMVAL.CommonRerize(cnt);
                    }
                }
                Array.Resize(ref COMVAL.strName, COMVAL.strName.Length - 1);
                Array.Resize(ref COMVAL.strArtist, COMVAL.strArtist.Length - 1);
                Array.Resize(ref COMVAL.strAlbum, COMVAL.strAlbum.Length - 1);
                Array.Resize(ref COMVAL.strLocation, COMVAL.strLocation.Length - 1);
                Array.Resize(ref COMVAL.iArtCnt, COMVAL.iArtCnt.Length - 1);
                Array.Resize(ref COMVAL.iPersistentIDHigh, COMVAL.iPersistentIDHigh.Length - 1);
                Array.Resize(ref COMVAL.iPersistentIDRow, COMVAL.iPersistentIDRow.Length - 1);
                Array.Resize(ref COMVAL.blnCompilation, COMVAL.blnCompilation.Length - 1);

                reader.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                if(reader != null)
                    reader.Close();
                COMVAL = null;
                pErrMessage += "System Exception 1013 : " + ex.Message + "\n";
                return false;
            }
        }
        /// <summary>
        /// 設定ファイル作成
        /// </summary>
        /// <param name="Make"></param>
        private Boolean Make_Conf()
        {

            try{
                XmlTextWriter xtw = null;

                if (File.Exists(CONF.ppath + @"\ITunEsTooL.config"))
                    DeleteFile(CONF.ppath + @"\ITunEsTooL.config");

                xtw = new XmlTextWriter(CONF.ppath + @"\ITunEsTooL.config", Encoding.Default);
                WriteHeaderXml(ref xtw, "ItunEsTooL_Configure");
                xtw.WriteStartElement("Default");
                xtw.WriteElementString("ErrorLogPath", @"C:\Program Files\ITunEsTooL\Log");
                xtw.WriteElementString("XmlPath", @"C:\Program Files\ITunEsTooL\ITunesDataLibrary.xml");
                xtw.WriteElementString("DeleteArtwork", TRUE);
                xtw.WriteElementString("ArtworkPath", @"C:\Program Files\ITunEsTooL\ArtWork");
                xtw.WriteElementString("SaveArtworkName", "2");
                xtw.WriteElementString("SavePictureName", "jpg");
                xtw.WriteElementString("SmallArtworkSize", "-300");
                xtw.WriteElementString("InCompilation", FALSE);
                xtw.WriteElementString("BackupType", "1");
                xtw.WriteElementString("iTunesSync", FALSE);
                xtw.WriteElementString("TrackCount", "0");
                xtw.WriteElementString("Color", "SkyBlue");
                xtw.WriteElementString("UpdateDate", "なし");
                xtw.WriteElementString("Review", TRUE);
                xtw.WriteElementString("AutoShutdown", FALSE);
                xtw.WriteEndElement();
                xtw.WriteEndElement();
                xtw.WriteEndDocument();
                xtw.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 2398 : " + ex.Message + "\n";
                return false;
            }
        }
        /// <summary>
        /// 設定ファイル作成、更新
        /// </summary>
        /// <param name="Make"></param>
        private Boolean Update_Conf(){

           XmlTextWriter xtw = null;
           string strColor = "";
           string strArtworkName = "";
           string strKakuchoushiName = "";
           string strSmall = "0";
           string strtrackcnt = string.Empty;
           Decimal ihanten = 0;
           try
           {
                xtw = new XmlTextWriter(CONF.ppath + @"\ITunEsTooL.config", Encoding.Default);

                WriteHeaderXml(ref xtw, "ItunEsTooL_Configure");
                xtw.WriteStartElement("Default");
                xtw.WriteElementString("ErrorLogPath", CONF.pErrLog);
                xtw.WriteElementString("XmlPath", CONF.pxmlPath);
                xtw.WriteElementString("DeleteArtwork", chkArtworkDel.Checked.ToString());
                xtw.WriteElementString("ArtworkPath", CONF.pArtWork);

                if (rdoArtName.Checked)
                    strArtworkName = "1";
                if (rdoArtAlbum.Checked)
                    strArtworkName = "2";
                if (rdoArtArtist.Checked)
                    strArtworkName = "3";
                if (rdoNmArb.Checked)
                    strArtworkName = "4";
                if (rdoArbArt.Checked)
                    strArtworkName = "5";

                xtw.WriteElementString("SaveArtworkName", strArtworkName);
                
                if (rdoJpg.Checked)
                    strKakuchoushiName = "jpg";
                if (rdoPng.Checked)
                    strKakuchoushiName = "png";
                if (rdoBmp.Checked)
                    strKakuchoushiName = "bmp";

                xtw.WriteElementString("SavePictureName", strKakuchoushiName);

                ihanten = numUDSize.Value;

                if (!chkSmalGazou.Checked)
                    ihanten = -ihanten;
                strSmall = ihanten.ToString();
                xtw.WriteElementString("SmallArtworkSize", strSmall);

                xtw.WriteElementString("InCompilation", CONF.pComp);

                if(rdoUwagaki.Checked)
                    xtw.WriteElementString("BackupType", "1");
                else
                    xtw.WriteElementString("BackupType", "2");

                xtw.WriteElementString("iTunesSync", CONF.pSync);

                if (COMVAL.strName == null)
                    strtrackcnt = "0";
                else
                    strtrackcnt = COMVAL.strName.Length.ToString();

                CONF.pTrackCnt = strtrackcnt;

                xtw.WriteElementString("TrackCount", CONF.pTrackCnt);

                if (rdoskyBlue.Checked)
                    strColor = frmColor.SKYBLUE;
                if (rdoGreen.Checked)
                    strColor = frmColor.LIGHTGREEN;
                if (rdoTomato.Checked)
                    strColor = frmColor.TOMATO;
                if (rdoviolet.Checked)
                    strColor = frmColor.VIOLET;

                xtw.WriteElementString("Color", strColor);
                xtw.WriteElementString("UpdateDate", CONF.pDate);
                xtw.WriteElementString("Review", CONF.pReview);
                xtw.WriteElementString("AutoShutdown", CONF.pAutoShutdown);
                xtw.WriteEndElement();
                xtw.WriteEndElement();
                xtw.WriteEndDocument();
                xtw.Close();
                return true;

            }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 1011 : " + ex.Message + "\n";
                return false;
            }
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
        /// 名前の初期値
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetName()
        {
            try{
                btnExistFile.Text = "存在しない" + "\n" + "ファイルを削除";
                btnMakePList.Text = "検索する曲の" + "\n" + "プレイリストを作成";
                label1.Text = "iTunesから曲をドラッグ＆ドロップで追加します。" + "\n" + "複数データの詳細を見たい場合はテキストをダブルクリックして下さい。";
                btnSerchChr.Text = "検索する曲の" + "\n" + "ファイルを作成";
                txtAfter.Text = "";
                txtBefor.Text = "";
                txtNumber.Text = "0";
                label5.Text = label5.Text + " " + ver.FileVersion;
            }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 5332 : " + ex.Message + "\n";
            }

        }

        /// <summary>
        /// 同期ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDouki_Click(object sender, EventArgs e)
        {
            DialogResult YorN = DialogResult.No;
            iTunesApp itunes = null;
            IITLibraryPlaylist libraryPlaylist;
            iTunesApp app;
            int cnt = 0;
            string strMikoushin = string.Empty;

            COMVAL.NEWVAL();

            itunes = new iTunesAppClass();

            try
            {
                app = new iTunesApp();
                libraryPlaylist = app.LibraryPlaylist;

            }
            catch (System.Exception)
            {
                Msgbox("iTunesが起動できません。", HeadMsg.CONNECT_ITUNES, 2);
                return;
            }

            try
            {
                lblTitle.Text = HeaderName.TITLE;
                if (!pDoukiFlg)
                {
                    if (COMVAL.strName != null)
                    {
                        YorN = Msgbox("iTunesの情報を更新しますか？", HeadMsg.CONNECT_ITUNES, 3);
                        if (YorN == DialogResult.No)
                            return;
                    }
                    else
                    {

                        YorN = Msgbox("iTunesの情報を取得します。", HeadMsg.CONNECT_ITUNES, 3);

                        if (YorN == DialogResult.No)
                            return;
                    }
                }
                this.Enabled = false;
                splash.SendDataALL(libraryPlaylist.Tracks.Count);
                Message = "iTunes情報を取得中です...";
                DialogSaizenmen();
                splash.SendVisible(true);

                backgroundWorker1.RunWorkerAsync();

                lblTitle.Text = HeaderName.GET_ITUNEDATA;
                IITFileOrCDTrack file = null;

                foreach (IITTrack files in libraryPlaylist.Tracks)
                {
                    try
                    {
                        try
                        {
                            file = (IITFileOrCDTrack)files;
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                        COMVAL.CommonRerize(cnt);
                        COMVAL.strName[cnt] = file.Name;
                        COMVAL.strAlbum[cnt] = file.Album;
                        COMVAL.strArtist[cnt] = file.Artist;
                        COMVAL.strLocation[cnt] = file.Location;
                        COMVAL.strAlbumArtist[cnt] = file.AlbumArtist;
                        COMVAL.iArtCnt[cnt] = file.Artwork.Count;
                        COMVAL.iPersistentIDHigh[cnt] = itunes.get_ITObjectPersistentIDHigh(file);
                        COMVAL.iPersistentIDRow[cnt] = itunes.get_ITObjectPersistentIDLow(file);
                        COMVAL.blnCompilation[cnt] = file.Compilation;
                        nullConv(COMVAL.strName[cnt], COMVAL.strAlbum[cnt], COMVAL.strArtist[cnt], COMVAL.strLocation[cnt], COMVAL.strAlbumArtist[cnt], cnt);
                        cnt++;
                        splash.SendData(cnt);
                        if (splash.Cancel)
                        {
                            Msgbox("処理を中断します。" + "\n" +
                                   "※完全に同期していないと、ITunEsTooLの機能が利用出来ない場合" + "\n" +
                                   "があります。", HeadMsg.NOTICE, 7);
                            break;

                        }
                    }
                    catch (COMException ex)
                    {
                        YorN = Msgbox("iTunesの情報が取得出来ませんでした。iTunesに問題がある可能性があります。処理を続行しますか？", HeadMsg.CONNECT_ITUNES, 8);
                        if (YorN == DialogResult.No)
                        {
                            pErrMessage += "System Exception 14202 : " + ex.Message + "\n";
                            break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        YorN = Msgbox("iTunesの情報が取得出来ませんでした。iTunesに問題がある可能性があります。処理を続行しますか？", HeadMsg.CONNECT_ITUNES, 8);
                        if (YorN == DialogResult.No)
                        {
                            pErrMessage += "System Exception 14203 : " + ex.Message + "\n";
                            break;
                        }
                    }
                }

                CONF.pDate = DateTime.Now.ToString() + " 更新";
                CONF.pTrackCnt = cnt.ToString();
                lblkoushin.Text = CONF.pDate;
                splash.CloseSplash();
                Msgbox("同期が完了しました。", HeadMsg.COMPLETE, 1);
                btnDouki.ForeColor = Color.White;
                CONF.pxmlPath = txtxmlPath.Text;
                this.Update();
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {

                Msgbox("iTunesの情報が取得出来ません。iTunesの設定を確認してください。", HeadMsg.HEAD_EXCEPTION, 2);
                pErrMessage += "System Exception 14201 : " + ex.Message + "\n";
            }
            catch (System.Exception ex)
            {
                splash.CloseSplash();
                Msgbox("iTunesと同期が出来ませんでした。iTunesを再起動して下さい。", HeadMsg.HEAD_EXCEPTION, 2);
                pErrMessage += "System Exception 1002 : " + ex.Message + "\n";
            }
            finally
            {
                CheckSaizenmen();
                this.Enabled = true;
                lblTitle.Text = HeaderName.TITLE;
                if (app != null) { Marshal.ReleaseComObject(app); }
                if (libraryPlaylist != null) { Marshal.ReleaseComObject(libraryPlaylist); }
                if (itunes != null) { Marshal.ReleaseComObject(itunes); }
            }
        }
        /// <summary>
        /// null文字を空白に変換
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strAlbum"></param>
        /// <param name="strArtist"></param>
        /// <param name="strLocatuin"></param>
        /// <param name="strAlbumArtist"></param>
        private void nullConv(string strName,string strAlbum,string strArtist,string strLocation, string strAlbumArtist,int icnt)
        {
            COMVAL.strName[icnt] = strName == null ? "" : strName;
            COMVAL.strAlbum[icnt] = strAlbum == null ? "" : strAlbum;
            COMVAL.strArtist[icnt] = strArtist == null ? "" : strArtist;
            COMVAL.strLocation[icnt] = strLocation == null ? "" : strLocation;
            COMVAL.strAlbumArtist[icnt] = strAlbumArtist == null ? "" : strAlbumArtist;           
        }
        /// <summary>
        /// 設定ファイル保存
        /// </summary>
        /// <param name="Make"></param>
        /// <returns></returns>
        private Boolean Update_ConfTrack(Boolean Make = false)
        {

            XmlTextWriter xtw = null;
            string strErr = "";
            string strxml = "";
            string strColor = "";
            string strTrack = string.Empty;
            iTunesApp app = new iTunesApp();
            IITLibraryPlaylist libraryPlaylist = app.LibraryPlaylist;

            try
            {
                if (Make)
                {
                    strxml = CONF.ppath + @"\ITunesDataLibrary.xml";
                    strErr = CONF.ppath + @"\ITunEsTooL\Log";
                }
                else
                {
                    strxml = CONF.pxmlPath;
                }

                xtw = new XmlTextWriter(CONF.ppath + @"\ITunEsTooL.config", Encoding.Default);

                WriteHeaderXml(ref xtw, "ItunEsTooL_Configure");
                xtw.WriteStartElement("Default");

                if (Directory.Exists(strErr))
                    xtw.WriteElementString("ErrorLogPath", strErr);
                xtw.WriteElementString("XmlPath", strxml);

                CONF.pTrackCnt = libraryPlaylist.Tracks.Count.ToString();
                xtw.WriteElementString("TrackCount", CONF.pTrackCnt);

                if (rdoskyBlue.Checked)
                    strColor = frmColor.SKYBLUE;
                if (rdoGreen.Checked)
                    strColor = frmColor.LIGHTGREEN;
                if (rdoTomato.Checked)
                    strColor = frmColor.TOMATO;
                if (rdoviolet.Checked)
                    strColor = frmColor.VIOLET;

                xtw.WriteElementString("Color", strColor);
                xtw.WriteEndElement();
                xtw.WriteEndElement();
                xtw.WriteEndDocument();
                return true;
            }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 1011 : " + ex.Message + "\n";
                return false;
            }
            finally
            {
                if (app != null)
                    Marshal.ReleaseComObject(app);
                if (libraryPlaylist != null)
                    Marshal.ReleaseComObject(libraryPlaylist);
                if(xtw != null)
                    xtw.Close();
            }
        }

        
        /// <summary>
        /// ドラッグドロップ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabSerch_DragDrop(object sender, DragEventArgs e)
        {

            try
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                    return;
                if (COMVAL.strLocation == null)
                {
                    Msgbox("iTunesと同期して下さい。", HeadMsg.ATTENTION,5);
                    return;
                }
                lblKeikoku1.Visible = false;
                lblKeikoku2.Visible = false;
                lblKeikokuNum.Visible = false;

                if (COMVAL.strPaths == null)
                    COMVAL.strPaths = new string[1];

                int iExistCnt = 0;
                foreach (string filePath in (string[])e.Data.GetData(DataFormats.FileDrop))
                {
                    if (Array.IndexOf(COMVAL.strLocation, filePath) != -1)
                    {
                        if (pSelectCnt < 1)
                            txtBefor.Text = filePath;
                        if (Array.IndexOf(COMVAL.strPaths, filePath) == -1)
                        {
                            Array.Resize(ref COMVAL.strPaths, pSelectCnt + 1);
                            COMVAL.strPaths[pSelectCnt] = filePath;
                            pSelectCnt++;
                        }
                    }
                    else
                    {
                        iExistCnt++;
                    }
                }
                if (iExistCnt != 0)
                {                    
                    lblKeikoku1.Visible = true;
                    lblKeikoku2.Visible = true;
                    lblKeikokuNum.Visible = true;
                    lblKeikokuNum.Text = iExistCnt.ToString();
                }
                txtNumber.Text = Convert.ToString(COMVAL.strPaths.Length - 1);
            }
            catch (System.Exception ex)
            {
                if (File.Exists(CONF.pxmlPath))
                    DeleteFile(CONF.pxmlPath);
                Msgbox("System Exception 1003" + "\n" + "ITunEs TooLを終了します。", HeadMsg.HEAD_EXCEPTION,4);
                pErrMessage += "System Exception 1003 : " + ex.Message + "\n";
                this.Close();
            }
            finally
            {
             
            }
        }

        /// <summary>
        /// タイトルバーのないフォームに対して処理を行う
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabSerch_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        /// <summary>
        ///クリアボタン 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (COMVAL.strPaths == null)
                return;

            DialogResult YorN;
            YorN = Msgbox("指定した曲をクリアしてもよろしいですか？", "クリア",3);

            if (YorN == DialogResult.No)
                return;

            txtNumber.Text = "0";
            txtBefor.Text = "";
            pSelectCnt = 0;
            COMVAL.strPaths = null;
            lblKeikoku1.Visible = false;
            lblKeikoku2.Visible = false;
            lblKeikokuNum.Visible = false;

        }

        /// <summary>
        ///ダブルクリック 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBefor_DoubleClick_1(object sender, EventArgs e)
        {
            if (txtBefor.Text == "")
                return;
            if (COMVAL.strLocation == null)
            {
                Msgbox("iTunesと同期して下さい。", HeadMsg.ATTENTION,5);
                return;
            }
            
            try
            {
                Ichiran frmIchiran = new Ichiran();
                
                string name = string.Empty;

                COMVAL.DIALOG_TITLE = HeaderName.ICHIRAN;
                frmIchiran.CLSCOMVAL = COMVAL;
                frmIchiran.CColor = COMVAL.strColor;
                
                DialogSaizenmen();
                frmIchiran.ShowDialog();

                if (COMVAL.strPaths != null)
                {
                    if (Array.IndexOf(COMVAL.strPaths, txtBefor.Text) == -1)
                        txtBefor.Text = COMVAL.strPaths[0];
                    txtNumber.Text = Convert.ToString(COMVAL.strPaths.Length - 1);

                    pSelectCnt = COMVAL.strPaths.Length;

                    COMVAL = frmIchiran.CLSCOMVAL;
                }
                else
                {
                    txtNumber.Text = "0";
                    txtBefor.Text = "";
                    pSelectCnt = 0;
                    COMVAL.strPaths = null;
                }

                frmIchiran = null;
            }
            catch (System.Exception ex)
            {
                if (File.Exists(CONF.pxmlPath))
                    DeleteFile(CONF.pxmlPath);
                Msgbox("System Exception 1004" + "\n" + "ITunEs TooLを終了します。", HeadMsg.HEAD_EXCEPTION,4);
                pErrMessage += "System Exception 1004 : " + ex.Message + "\n";
                this.Close();
            }
            finally
            {
                CheckSaizenmen();
            }
        }

        /// <summary>
        /// コピーボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopy_Click(object sender, EventArgs e)
        {
            int Index = 0;
            int cnt = 0;
            string Album_or_Artist = string.Empty;
            string ConvName = string.Empty;
            string ConvName2 = string.Empty;
            Boolean blnCancel = false;
            DialogResult YorN = DialogResult.No;

            if(!ErrCheck())
                return;
            
            YorN = Msgbox("コピーを開始します。よろしいですか？" + "\n" + "※同じファイル名が存在する場合は上書きします。"
                                      + "\n" + "iTunesと同期直後でない場合、正しい結果が得られない場合があります。",
                                      "ファイルをコピー",
                                      3);

            if (YorN == DialogResult.No)
                return;

            this.Enabled = false;
            if (rdoAlbum.Checked)
            {
                Album_or_Artist = "Album";
            }else if(rdoArtist.Checked)
            {
                Album_or_Artist = "Artist";
            }
            else if (rdoDouble.Checked)
            {
                Album_or_Artist = "Double";
            }

            lblTitle.Text = "コピー中です...";
            splash.SendDataALL(COMVAL.strPaths.Length);
            splash.SendData(0);
            Message = "データをコピー中です...";
            DialogSaizenmen();
            splash.SendVisible(true);
            backgroundWorker1.RunWorkerAsync();

            if (Album_or_Artist == "Album")
            {
                foreach (string file in COMVAL.strPaths)
                {
                    Application.DoEvents();
                    Index = Array.IndexOf(COMVAL.strLocation, file);
                    if (Index != -1)
                    {
                        ConvName = FileOpe.ValidFileName(COMVAL.strAlbum[Index]);
                        try
                        {
                            if (ConvName == "")
                            {
                                if (Directory.Exists(txtAfter.Text + @"\" + "アルバム名不明") == false)
                                    Directory.CreateDirectory(txtAfter.Text + @"\" + "アルバム名不明");

                                if (!File.Exists(txtAfter.Text + @"\" + "アルバム名不明" + @"\" + Path.GetFileName(COMVAL.strLocation[Index])))
                                {
                                    File.Copy(COMVAL.strLocation[Index], txtAfter.Text + @"\" + "アルバム名不明" + @"\" + Path.GetFileName(COMVAL.strLocation[Index]));
                                }
                                else
                                {
                                    if (rdoUwagaki.Checked)
                                        File.Copy(COMVAL.strLocation[Index], txtAfter.Text + @"\" + "アルバム名不明" + @"\" + Path.GetFileName(COMVAL.strLocation[Index]), true);
                                }
                            }
                            else
                            {
                                if (Directory.Exists(txtAfter.Text + @"\" + ConvName) == false)
                                    Directory.CreateDirectory(txtAfter.Text + @"\" + ConvName);

                                if (!File.Exists(txtAfter.Text + @"\" + ConvName + @"\" + Path.GetFileName(COMVAL.strLocation[Index])))
                                {
                                    File.Copy(COMVAL.strLocation[Index], txtAfter.Text + @"\" + ConvName + @"\" + Path.GetFileName(COMVAL.strLocation[Index]));

                                }
                                else
                                {
                                    if(rdoUwagaki.Checked)
                                        File.Copy(COMVAL.strLocation[Index], txtAfter.Text + @"\" + ConvName + @"\" + Path.GetFileName(COMVAL.strLocation[Index]), true);
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                           pErrMessage +=  "System Exception 1005 : " + ex.Message + "\n";
                        }

                    }
                    cnt++;
                    splash.SendData(cnt);
                    blnCancel = splash.Cancel;
                    if (blnCancel)
                    {
                        Msgbox("処理を中断します。", HeadMsg.NOTICE,7);
                        splash.SendCacncel = false;                        
                        splash.SendCan(false);
                        break;
                    }
                }
            }
            else if (Album_or_Artist == "Artist")
            {
                foreach (string file in COMVAL.strPaths)
                {
                    Application.DoEvents();
                    Index = Array.IndexOf(COMVAL.strLocation, file);
                    if (Index != -1)
                    {
                        ConvName = FileOpe.ValidFileName(COMVAL.strArtist[Index]);
                        try
                        {
                            if (ConvName == "")
                            {
                                if (Directory.Exists(txtAfter.Text + @"\" + "アーティスト名不明") == false)
                                    Directory.CreateDirectory(txtAfter.Text + @"\" + "アーティスト名不明");

                                if (!File.Exists(txtAfter.Text + @"\" + "アーティスト名不明" + @"\" + Path.GetFileName(COMVAL.strLocation[Index])))
                                {
                                    File.Copy(COMVAL.strLocation[Index], txtAfter.Text + @"\" + "アーティスト名不明" + @"\" + Path.GetFileName(COMVAL.strLocation[Index]));
                                }
                                else
                                {
                                    if (rdoUwagaki.Checked)
                                        File.Copy(COMVAL.strLocation[Index], txtAfter.Text + @"\" + "アーティスト名不明" + @"\" + Path.GetFileName(COMVAL.strLocation[Index]), true);
                                }
                            }
                            else
                            {
                                if (Directory.Exists(txtAfter.Text + @"\" + ConvName) == false)
                                    Directory.CreateDirectory(txtAfter.Text + @"\" + ConvName);

                                if (!File.Exists(txtAfter.Text + @"\" + ConvName + @"\" + Path.GetFileName(COMVAL.strLocation[Index])))
                                {
                                    File.Copy(COMVAL.strLocation[Index], txtAfter.Text + @"\" + ConvName + @"\" + Path.GetFileName(COMVAL.strLocation[Index]));
                                }
                                else
                                {
                                    if (rdoUwagaki.Checked)
                                        File.Copy(COMVAL.strLocation[Index], txtAfter.Text + @"\" + ConvName + @"\" + Path.GetFileName(COMVAL.strLocation[Index]), true);
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                           pErrMessage +=  "System Exception 1006 : " + ex.Message + "\n";
                        }
                    }
                    cnt++;
                    splash.SendData(cnt);
                    blnCancel = splash.Cancel;
                    if (blnCancel)
                    {
                        Msgbox("処理を中断します。", HeadMsg.NOTICE, 7);
                        splash.SendCacncel = false;
                        splash.SendCan(false);
                        break;
                    }
                }
            }
            else if (Album_or_Artist == "Double")
            {
                foreach (string file in COMVAL.strPaths)
                {
                    Index = Array.IndexOf(COMVAL.strLocation, file);
                    if (Index != -1)
                    {
                        ConvName = FileOpe.ValidFileName(COMVAL.strArtist[Index]);
                        ConvName2 = FileOpe.ValidFileName(COMVAL.strAlbum[Index]);
                        try
                        {
                            
                            if (ConvName == "")
                            {
                                if (Directory.Exists(txtAfter.Text + @"\" + "アーティスト名不明") == false)
                                    Directory.CreateDirectory(txtAfter.Text + @"\" + "アーティスト名不明");

                                ConvName2 = SelectAlbumName(txtAfter.Text + @"\" + "アーティスト名不明", ConvName2);

                                if (!File.Exists(ConvName2 + @"\" + Path.GetFileName(COMVAL.strLocation[Index])))
                                {
                                    File.Copy(COMVAL.strLocation[Index], ConvName2 + @"\" + Path.GetFileName(COMVAL.strLocation[Index]));
                                }
                                else
                                {
                                    if (rdoUwagaki.Checked)
                                        File.Copy(COMVAL.strLocation[Index], ConvName2 + @"\" + Path.GetFileName(COMVAL.strLocation[Index]), true);
                                }
                            }
                            else
                            {
                                if (Directory.Exists(txtAfter.Text + @"\" + ConvName) == false)
                                    Directory.CreateDirectory(txtAfter.Text + @"\" + ConvName);

                                ConvName2 = SelectAlbumName(txtAfter.Text + @"\" + ConvName, ConvName2);

                                if (!File.Exists(ConvName2 + @"\" + Path.GetFileName(COMVAL.strLocation[Index])))
                                {
                                    File.Copy(COMVAL.strLocation[Index], ConvName2 + @"\" + Path.GetFileName(COMVAL.strLocation[Index]));
                                }
                                else
                                {
                                    if (rdoUwagaki.Checked)
                                        File.Copy(COMVAL.strLocation[Index], ConvName2 + @"\" + Path.GetFileName(COMVAL.strLocation[Index]), true);
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            pErrMessage += "System Exception 1006 : " + ex.Message + "\n";
                        }
                    }
                    cnt++;
                    splash.SendData(cnt);
                    blnCancel = splash.Cancel;
                    if (blnCancel)
                    {
                        Msgbox("処理を中断します。", HeadMsg.NOTICE,7);
                        splash.SendCacncel = false;
                        splash.SendCan(false);
                        break;
                    }
                }
            }

            splash.CloseSplash();
            Msgbox("コピーが完了しました。", HeadMsg.COMPLETE, 1);
            this.Enabled = true;
            CheckSaizenmen();
            lblTitle.Text = HeaderName.TITLE;
        }

        /// <summary>
        /// アルバム名を調べて作成するフォルダ名を返す
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string SelectAlbumName(string strpath, string strAlbum)
        {
            string strRet = string.Empty;
            if (strAlbum != "")
            {
                if(!Directory.Exists(strpath + @"\" + strAlbum))
                    Directory.CreateDirectory(strpath + @"\" + strAlbum);
                strRet = strpath + @"\" + strAlbum;
            }
            else
            {
                if (!Directory.Exists(strpath + @"\" + @"\アルバム名不明"))
                    Directory.CreateDirectory(strpath + @"\アルバム名不明");
                strRet = strpath + @"\アルバム名不明";
            }
            return strRet;
        }

        /// <summary>
        /// エラーチェック
        /// </summary>
        private Boolean ErrCheck()
        {
            if (COMVAL.strName == null)
            {
                Msgbox("設定にて、iTunesの情報を取得して下さい。", HeadMsg.ATTENTION,5);
                return false;
            }

            if (txtBefor.Text == "")
            {
                Msgbox("曲名を指定して下さい。", HeadMsg.ATTENTION,5);
                txtBefor.Focus();
                return false;
            }
            if (txtAfter.Text == "")
            {
                Msgbox("パスを指定して下さい。", HeadMsg.ATTENTION, 5);
                txtAfter.Focus();
                return false;
            }
            if (!File.Exists(txtBefor.Text))
            {
                Msgbox("パスが存在しません。指定し直して下さい。", HeadMsg.ATTENTION, 5);
                txtBefor.Focus();
                return false;
            }

            if (!Directory.Exists(txtAfter.Text))
            {
                Msgbox("パスが存在しません。指定し直して下さい。", HeadMsg.ATTENTION, 5);
                txtAfter.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// タイトルバーない画面の操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                mousePoint = new Point(e.X, e.Y);
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Left += e.X - mousePoint.X;
                this.Top += e.Y - mousePoint.Y;
            }
        }

        /// <summary>
        /// ×ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult YorN = DialogResult.No;

            YorN = Msgbox("ITunEs TooLを終了してもよろしいですか？","確認",3);

            if (YorN == DialogResult.No)
                return;
            this.Close();
        }

        /// <summary>
        /// 最小化ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click_1(object sender, EventArgs e)
        {
            AnimateWindow(Handle, 500, (uint)(AnimateWindowFlags.AW_HOR_NEGATIVE | AnimateWindowFlags.AW_BLEND | AnimateWindowFlags.AW_HIDE));
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = true;
        }
        private void frmItune_Activated(object sender, EventArgs e)
        {
            this.Update();            
        }

        private void frmItune_Click(object sender, EventArgs e)
        {
            CheckSaishin();
        }
        private void CheckSaishin()
        {
            if (lblkoushin.Text == "")
                return;
            try{
            
                if (!blnSaishin || itunesD.LibraryPlaylist.Tracks.Count  != Convert.ToDecimal(CONF.pTrackCnt))
                {
                    Msgbox("iTunesの最新の情報が取得出来ていない可能性があります。" + "\n" + "iTunesと同期して下さい。", HeadMsg.NOTICE, 2);
                    KeikokuDouki();
                }
            }
            catch (System.Exception)
            {
                Msgbox("iTunesが閉じられているか、その他の問題が発生しました。" + "\n" +
                       "ITunEsTooLを終了します。", HeadMsg.NOTICE, 2);
                this.Close();
            }
            
        }
        private void KeikokuDouki()
        {
            blnSaishin = true;
            blnshorichu = true;
            tabControl1.SelectedTab = tabControl1.TabPages["tabSetting"];
            btnDouki.ForeColor = Color.Red;
            lblkoushin.Text = "最新の情報が取得されていない可能性があります！";
            blnshorichu = false;
        }
        /// <summary>
        /// マルチスレッド処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            switch (pSplashNum)
            {
                case 1:
                    splash.OpenSplash(Message, this, COMVAL.strColor);
                    break;
                case 2:
                    StartSplash.ShowDialog();
                    break;
            }            
        }      

        /// <summary>
        /// フォーム表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmItune_Shown(object sender, EventArgs e)
        {
            this.Activate();
            this.Update();
            picBottum.Update();
            picLeft.Update();
            picRight.Update();
            pictureBox1.Update();
          
            chkArtworkDel.Text = "ダウンロードした画像のみ設定する";
            btnBackArt.Text = "iTunesの" + "\n" + "アートワークを" + "\n" + "バックアップ";

            if (CONF.pSync == TRUE)
            {
                tabControl1.SelectedTab = tabControl1.TabPages["tabSetting"]; 
                chkDouki.Checked = true;
                pDoukiFlg = true;
                btnDouki.PerformClick();
                pDoukiFlg = false;
            }
            ChangeCheckBoxParameter(ref  chkDouki, ref CONF.pSync);
            CheckSaishin();

            if (SysSet.Win7Check() || SysSet.IsUacEnabled())
            {
                //Msgbox("【Windows7】 以降のOS、又は【UAC】が有効になっている場合、アートワークの保存場所をデスクトップ等に変更して下さい。 " + "\n" + "一部機能が動作しない可能性があります。", HeadMsg.ATTENTION, 2);
                //tabControl1.SelectedTab = tabControl1.TabPages["tabSetting"];
                //label26.Text += "※デスクトップ等のローカルフォルダを指定して下さい。";
                //label26.ForeColor = Color.Red;
            }
            else
            {
                画像ファイルを設定するToolStripMenuItem.Visible = false;
            }
            if (!Directory.Exists(txtArtWork.Text))
            {
                Msgbox("アートワークの保存先がが見つかりません。設定タブにて設定し直してください。", HeadMsg.ATTENTION, 2);
                tabControl1.SelectedTab = tabControl1.TabPages["tabSetting"];
                label26.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// 問題点検索ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSerchChr_Click(object sender, EventArgs e)
        {
            iTunesApp app = null;
            IITLibraryPlaylist libraryPlaylist = null;
            Regex reg = null;
            string strMatch = string.Empty;
            string strArtist = string.Empty;
            string strAlbum = string.Empty;
            string strName = string.Empty;
            string strTime = string.Empty;
            StreamWriter sw;

            if (COMVAL.strLocation == null)
            {
                Msgbox("設定にて、iTunesの情報を取得して下さい。", HeadMsg.ATTENTION, 2);
                return;
            }

            if (txtLogPath.Text == "")
            {
                Msgbox("パスを指定して下さい。", HeadMsg.ATTENTION, 2);
                txtLogPath.Focus();
                return;
            }
            if (!Directory.Exists(txtLogPath.Text))
            {
                Msgbox("パスが存在しません。指定し直して下さい。", HeadMsg.ATTENTION, 2);
                txtLogPath.Focus();
                return;
            }

            DialogResult YorN = DialogResult.No;
            if (!CheckChkControl())
            {
                Msgbox("チェックボックスを選択して下さい。", HeadMsg.ATTENTION, 2);
                chkAlbum.Select();
                return;
            }
            
            YorN = Msgbox("問題点を検索します。よろしいですか？" + "\n" + "※iTunesと同期直後でない場合、正しい結果が得られない場合があります。", "検索",3);
            if (YorN == DialogResult.No)
                return;
            
            this.Enabled = false;
            
            if (txtSerchChr.Text != "")
            {
                try
                {
                    reg = new Regex(txtSerchChr.Text, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                }
                catch (System.Exception)
                {
                    Msgbox("正規表現の指定方法が間違っています。", HeadMsg.ATTENTION, 2);
                    this.Enabled = true;
                    return;
                }
            }

            try
            {

                PlayListName GetName = new PlayListName();
                GetName.CColor = COMVAL.strColor;
                GetName.SelectEvent = 0;
                GetName.setSetumei = "作成するCSVの名前を設定して下さい。";
                DialogSaizenmen();
                GetName.ShowDialog();
                
                string strget = string.Empty;
                if (!GetName.Ret)
                    return;
                strget = GetName.getMassage;

                app = new iTunesApp();
                libraryPlaylist = app.LibraryPlaylist;
                Message = "CSVファイルを作成しています...";
                splash.SendDataALL(libraryPlaylist.Tracks.Count);
                backgroundWorker1.RunWorkerAsync();

                lblTitle.Text = HeaderName.CSV;
                sw = new System.IO.StreamWriter(
                    @txtLogPath.Text + @"\" + FileOpe.ValidFileName(strget) + "(検索).csv", false,
                    @System.Text.Encoding.GetEncoding("Shift_Jis"));

                string strtxt = string.Empty;
                strtxt =  txtSerchChr.Text.Replace(" ", "␣");
                if (txtSerchChr.Text != "")
                    sw.WriteLine("\"" + "検索文字列" + "\"" + "," + "\"" + strtxt + "\"");

                sw.WriteLine("\"" + "曲名"           + "\"" + "," 
                           + "\"" + "アルバム"     + "\"" + ","
                           + "\"" + "アーティスト" + "\"" + ","
                           + "\"" + "アルバムアーティスト" + "\"" + ","
                           + "\"" + "抽出内容"       +"\"");

                string strErr = string.Empty;
                int cnt = 0;
                foreach (string file in COMVAL.strName)
                {
                    this.Update();

                    if (String.Compare(txtSerchChr.Text, ".*", true) == 0)
                    {
                        strMatch = strMatch + "\"" + COMVAL.strName[cnt] + "\"" + "," +
                                              "\"" + COMVAL.strAlbum[cnt] + "\"" + "," +
                                              "\"" + COMVAL.strArtist[cnt] + "\"" +
                                              "\"" + COMVAL.strAlbumArtist[cnt] + "\"" + "\n";
                        cnt++;
                        splash.SendData(cnt);
                        continue;
                    }

                    strErr = "";
                    if (txtSerchChr.Text != "")
                    {
                        if (chkName.Checked)
                        {
                            if (Convert.ToString(reg.Match(file)) != "")
                                strErr = "検索文字列一致（曲名）";
                        }

                        if (chkArtist.Checked)
                        {
                            if (Convert.ToString(reg.Match(COMVAL.strArtist[cnt])) != "")
                            {
                                if (strErr == "")
                                {
                                    strErr = "検索文字列一致（アーティスト）";
                                }
                                else
                                {
                                    strErr = strErr + " + 検索文字列一致（アーティスト）";
                                }
                            }
                        }

                        if (chkAlbum.Checked)
                        {
                            if (Convert.ToString(reg.Match(COMVAL.strAlbum[cnt])) != "")
                            {
                                if (strErr == "")
                                {
                                    strErr = "検索文字列一致（アルバム）";
                                }
                                else
                                {
                                    strErr = strErr + " + 検索文字列一致（アルバム）";
                                }
                            }
                        }

                        if (chkAlbumArtist.Checked)
                        {
                            if (Convert.ToString(reg.Match(COMVAL.strAlbumArtist[cnt])) != "")
                            {
                                if (strErr == "")
                                {
                                    strErr = "検索文字列一致（アルバムアーティスト）";
                                }
                                else
                                {
                                    strErr = strErr + " + 検索文字列一致（アルバムアーティスト）";
                                }
                            }
                        }
                    }

                    if (chkName.Checked)
                    {
                        if (COMVAL.strName[cnt] == "" && chkBlank.Checked)
                            if (strErr == "")
                            {

                                strErr = strErr + "曲名未設定";
                            }
                            else
                            {
                                strErr = strErr + " + " + "曲名未設定";
                            }

                    }
                    if (chkArtist.Checked)
                    {
                        if (COMVAL.strArtist[cnt] == "" && chkBlank.Checked)
                        {
                            if (strErr == "")
                            {
                                strErr = strErr + "アーティスト未設定";
                            }
                            else
                            {
                                strErr = strErr + " + " + "アーティスト未設定";
                            }
                        }
                    }

                    if (chkAlbum.Checked)
                    {
                        if (COMVAL.strAlbum[cnt] == "" && chkBlank.Checked)
                        {
                            if (strErr == "")
                            {
                                strErr = strErr + "アルバム未設定";
                            }
                            else
                            {
                                strErr = strErr + " + " + "アルバム未設定";
                            }
                        }
                    }

                   
                    if (chkAlbumArtist.Checked)
                    {
                        if (COMVAL.strAlbumArtist[cnt] == "" && chkBlank.Checked)
                            if (strErr == "")
                            {

                                strErr = strErr + "アルバムアーティスト未設定";
                            }
                            else
                            {
                                strErr = strErr + " + " + "アルバムアーティスト未設定";
                            }

                    }
                    if (chkArtwork.Checked)
                    {
                        if (COMVAL.iArtCnt[cnt] == 0 && chkBlank.Checked)
                            if (strErr == "")
                            {

                                strErr = strErr + "アートワーク未設定";
                            }
                            else
                            {
                                strErr = strErr + " + " + "アートワーク未設定";
                            }

                    }

                    if (strErr != "")
                    {
                        strMatch = strMatch + "\"" + COMVAL.strName[cnt]   + "\"" + "," +
                                              "\"" + COMVAL.strAlbum[cnt]  + "\"" + "," +
                                              "\"" + COMVAL.strArtist[cnt] + "\"" + "," +
                                              "\"" + COMVAL.strAlbumArtist[cnt] + "\"" + "," +
                                              "\"" + strErr                + "\"" + "\n";
                    }
                    cnt++;
                    splash.SendData(cnt);
                    if (splash.Cancel)
                {
                    Msgbox("処理を中断します。", HeadMsg.NOTICE, 7);
                    splash.SendCacncel = false;
                    splash.SendCan(false);
                    break;
                }
                }
                
                sw.Write(strMatch);
                sw.Close();
                splash.CloseSplash();
                Msgbox("CSVファイルを作成しました。", HeadMsg.COMPLETE, 1);
                this.Enabled = true;
                
            }
            catch (System.Exception ex)
            {
                if (File.Exists(CONF.pxmlPath))
                    DeleteFile(CONF.pxmlPath);
                Msgbox("System Exception 1007" + "\n" + "ITunEs TooLを終了します。", HeadMsg.HEAD_EXCEPTION, 4);
                pErrMessage += "System Exception 1007 : " + ex.Message + "\n";
                this.Close();

            }
            finally
            {
                CheckSaizenmen();
                this.Enabled = true;
                lblTitle.Text = HeaderName.TITLE;
            }
        }

        /// <summary>
        /// 問題のある曲のプレイリストを作成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            iTunesApp itunes = null;
            iTunesApp app = null;
            IITLibraryPlaylist libraryPlaylist = null;
            IITTrackCollection trackCol = null;
            IITUserPlaylist playList = null;
            IITTrack track = null;
            PlayListName GetName = null;
            Regex reg = null;
            object target = null;
            string[] Chouhuku = new string[1];
            string strget = string.Empty;
            int icnt = 0;
            Boolean blnChkBlank = false;
            Boolean SetPlayLst = false;
            DialogResult YorN = DialogResult.No;
             
            try
            {
                if (!CheckChkControl())
                {
                    Msgbox("チェックボックスを選択して下さい。", HeadMsg.ATTENTION, 5);
                    chkAlbum.Select();
                    return;
                }
                if (COMVAL.strName == null)
                {
                    Msgbox("設定にて、iTunesの情報を取得して下さい。", HeadMsg.ATTENTION, 5);
                    return;
                }
                YorN = Msgbox("チェックボックスで選択した項目のチェックを行います。" + "\n" + "その結果がプレイリストに反映されますがよろしいですか？" 
                                        + "\n" + "※iTunesと同期直後でない場合、正しい結果が得られない場合があります。",
                    HeadMsg.MAKE_PLAYLIST,3);
                if (YorN == DialogResult.No)
                    return;
                
                app = new iTunesApp();
                libraryPlaylist = app.LibraryPlaylist;
                itunes = new iTunesAppClass();
                
                if (txtSerchChr.Text != "")
                {
                    try
                    {
                        reg = new Regex(txtSerchChr.Text, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    }
                    catch (System.Exception)
                    {
                        Msgbox("正規表現の指定方法が間違っています。", HeadMsg.ATTENTION, 2);
                        this.Enabled = true;
                        return;
                    }
                }

                //全メディアのコレクション取得  
                trackCol = itunes.LibraryPlaylist.Tracks;

                splash.SendDataALL(libraryPlaylist.Tracks.Count);
                Message = "プレイリストを作成中...";
                this.Enabled = false;

                GetName = new PlayListName();
                GetName.CColor = COMVAL.strColor;
                GetName.SelectEvent = 1;
                DialogSaizenmen();
                GetName.ShowDialog();
                
                if (!GetName.Ret)
                    return;
                strget = GetName.getMassage;
                lblTitle.Text = "プレイリストを作成しています...";
                
                //プレイリストの新規作成  
                playList = itunes.CreatePlaylist(strget + "(検索)") as IITUserPlaylist;                
                DialogSaizenmen();
                splash.SendVisible(true);
                //マルチスレッド処理
                backgroundWorker1.RunWorkerAsync();

                for (icnt = 0; icnt < COMVAL.strName.Length; icnt++)
                {
                    if (txtSerchChr.Text != "")
                        blnChkBlank = true;

                    //アルバム
                    if (chkAlbum.Checked)
                    {
                        if (COMVAL.strAlbum[icnt] == "" && chkBlank.Checked)
                        {
                            AddPlayList(ref track, ref target, ref playList, ref trackCol, icnt);
                            SetPlayLst = true;
                        }
                        if (blnChkBlank && !SetPlayLst)
                        {
                            if (Convert.ToString(reg.Match(COMVAL.strAlbum[icnt])) != "")
                            {
                                AddPlayList(ref track, ref target, ref playList, ref trackCol, icnt);
                                SetPlayLst = true;
                            }
                        }
                    }
                    //アーティスト
                    if (chkArtist.Checked)
                    {
                        if (COMVAL.strArtist[icnt] == "" && !SetPlayLst && chkBlank.Checked)
                        {
                            AddPlayList(ref track, ref target, ref playList, ref trackCol, icnt);
                            SetPlayLst = true;
                        }
                        if (blnChkBlank && !SetPlayLst)
                        {
                            if (Convert.ToString(reg.Match(COMVAL.strArtist[icnt])) != "")
                            {
                                AddPlayList(ref track, ref target, ref playList, ref trackCol, icnt);
                                SetPlayLst = true;
                            }
                        }
                    }

                    //曲名
                    if (chkName.Checked)
                    {
                        if (COMVAL.strName[icnt] == "" && !SetPlayLst && chkBlank.Checked)
                        {
                            AddPlayList(ref track, ref target, ref playList, ref trackCol, icnt);
                            SetPlayLst = true;
                        }
                        if (blnChkBlank && !SetPlayLst)
                        {
                            if (Convert.ToString(reg.Match(COMVAL.strName[icnt])) != "")
                            {
                                AddPlayList(ref track, ref target, ref playList, ref trackCol, icnt);
                                SetPlayLst = true;
                            }
                        }
                    }

                    //アルバムアーティスト
                    if (chkAlbumArtist.Checked)
                    {
                        if (COMVAL.strAlbumArtist[icnt] == "" && !SetPlayLst && chkBlank.Checked)
                        {
                            AddPlayList(ref track, ref target, ref playList, ref trackCol, icnt);
                            SetPlayLst = true;
                        }
                        if (blnChkBlank && !SetPlayLst)
                        {
                            if (Convert.ToString(reg.Match(COMVAL.strAlbumArtist[icnt])) != "")
                            {
                                AddPlayList(ref track, ref target, ref playList, ref trackCol, icnt);
                                SetPlayLst = true;
                            }
                        }
                    }

                    //アートワーク
                    if (chkArtwork.Checked && !SetPlayLst)
                    {
                        
                        if (COMVAL.iArtCnt[icnt] == 0 && !SetPlayLst && chkBlank.Checked)
                        {
                            AddPlayList(ref track, ref target, ref playList, ref trackCol, icnt);
                            SetPlayLst = true;
                        }
                    }
                    if (splash.Cancel)
                    {
                        Msgbox("処理を中断します。", HeadMsg.NOTICE, 7);
                        splash.SendCacncel = false;
                        splash.SendCan(false);
                        break;
                    }
                    splash.SendData(icnt);
                    SetPlayLst = false;
                }

                splash.CloseSplash();
                Msgbox("プレイリストの作成が完了しました。", HeadMsg.COMPLETE, 1);

            }
            catch (System.Exception ex)
            {
                if (File.Exists(CONF.pxmlPath))
                    DeleteFile(CONF.pxmlPath);
                splash.CloseSplash();
                Msgbox("System Exception 1009" + "\n" + "ITunEs TooLを終了します。", HeadMsg.HEAD_EXCEPTION, 2);
                pErrMessage += "System Exception 1009 : " + ex.Message + "\n";
                this.Close();
            }
            finally
            {
                CheckSaizenmen();
                this.Enabled = true;
                lblTitle.Text = HeaderName.TITLE;
                ReleaseITunesObject(ref itunes, ref app, ref libraryPlaylist, ref trackCol, ref playList, ref track);
            }

        }

        /// <summary>
        /// リソース解放
        /// </summary>
        /// <param name="itunes"></param>
        /// <param name="app"></param>
        /// <param name="libraryPlaylist"></param>
        /// <param name="trackCol"></param>
        /// <param name="playList"></param>
        /// <param name="track"></param>
        /// <returns></returns>
        private Boolean ReleaseITunesObject(ref iTunesApp itunes, ref  iTunesApp app,
                        ref IITLibraryPlaylist libraryPlaylist, ref IITTrackCollection trackCol,
                        ref IITUserPlaylist playList, ref IITTrack track)
        {
            try
            {
                if (itunes != null)
                    Marshal.ReleaseComObject(itunes);
                if (app != null)
                    Marshal.ReleaseComObject(app);
                if (libraryPlaylist != null)
                    Marshal.ReleaseComObject(libraryPlaylist);
                if (trackCol != null)
                    Marshal.ReleaseComObject(trackCol);
                if (playList != null)
                    Marshal.ReleaseComObject(playList);
                if (track != null)
                    Marshal.ReleaseComObject(track);

                itunes = null;
                app = null;
                libraryPlaylist = null;
                trackCol = null;
                playList = null;
                track = null;
            }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 1014 : " + ex.Message + "\n";
                return false;
            }
            return true;

        }
        /// <summary>
        /// チェックボックス使用チェック
        /// </summary>
        /// <returns></returns>
        private Boolean CheckChkControl()
        {
            if (!chkAlbum.Checked && !chkArtist.Checked && !chkArtwork.Checked && !chkName.Checked && !chkAlbumArtist.Checked)
                return false;
            return true;

        }

        /// <summary>
        /// プレイリスト作成
        /// </summary>
        /// <param name="track"></param>
        /// <param name="target"></param>
        /// <param name="playList"></param>
        /// <param name="trackCol"></param>
        /// <param name="icnt"></param>
        private void AddPlayList(ref IITTrack track,ref object target, ref IITUserPlaylist playList, ref IITTrackCollection trackCol, int icnt)
        {
            try
            {
                track = trackCol.get_ItemByPersistentID(COMVAL.iPersistentIDHigh[icnt], COMVAL.iPersistentIDRow[icnt]);
                target = track;
                if(target != null)
                    playList.AddTrack(ref target);
            }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 1009 : " + ex.Message + "\n";
            }
        }
        /// <summary>
        /// 存在しないファイルを削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            XmlTextWriter xtw = null;
            iTunesApp app= null;
            IITLibraryPlaylist libraryPlaylist = null;
            iTunesApp itunes = null;
            DialogResult YorN = DialogResult.No;
            int icnt = 0;
            int iDelCnt = 0;
            StreamWriter swDelSong = null;
            string strTime = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            string strDelLogPath = string.Empty;
            string strDelSong = string.Empty;
            try
            {
                //itunesObject();
                lblTitle.Text = "iTunesを起動しています...";
                lblTitle.Text = HeaderName.TITLE;
                app = new iTunesApp();
                //全メディアのコレクション取得  
                itunes = new iTunesAppClass();
                libraryPlaylist = app.LibraryPlaylist;

                if (COMVAL.strName == null)
                {
                    Msgbox("設定にて、iTunesの情報を取得して下さい。", HeadMsg.ATTENTION, 5);
                    return;
                }
                YorN = Msgbox("ファイルが存在しない曲を削除します。よろしいですか？" + "\n" + "※iTunesと同期直後でない場合、正しい結果が得られない場合があります。",
                "ファイル削除",
                3);

                if (YorN == DialogResult.No)
                    return;
                
            
                this.Enabled = false;
                splash.SendDataALL(libraryPlaylist.Tracks.Count);
                Message = "ファイルを削除中...";
                DialogSaizenmen();
                splash.SendVisible(true);
                //マルチスレッド処理
                backgroundWorker1.RunWorkerAsync();

                lblTitle.Text = HeaderName.GET_ITUNEDATA;
                IITTrack track = null;
                IITTrackCollection trackCol = null;

                trackCol = libraryPlaylist.Tracks;

                string strDir = string.Empty;
                string strFile = string.Empty;
                string strXmlPath = string.Empty;

                strDir = Path.GetDirectoryName(CONF.pxmlPath);
                strFile = @"\." + Path.GetFileName(CONF.pxmlPath);
                strXmlPath = strDir + strFile;

                xtw = new XmlTextWriter(strXmlPath, Encoding.Unicode);

                WriteHeaderXml(ref xtw, "iTunesDataLibrary");
                int iNewCnt = 0;
                string strHead = string.Empty;
                strHead = "\"" + "曲名" +  "\"" + "," +  "\"" + "アーティスト" +  "\""　+ "," +  "\"" + "アルバム名" +"\"" + "," +  "\"" + "場所" + "\"" + "\n";

                for (icnt = 0; icnt < COMVAL.strName.Length; icnt++)
                {
                    if (!File.Exists(COMVAL.strLocation[icnt]))
                    {
                        iDelCnt++;
                        strDelSong +=    "\"" + COMVAL.strName[icnt] + "\"" + "," 
                                                + "\"" + COMVAL.strArtist[icnt] +  "\"" + "," 
                                                + "\""　+ COMVAL.strAlbum[icnt] +  "\"" + ","
                                                + "\""　+ COMVAL.strLocation[icnt] +  "\"" 
                                                + "\n";
                        track = trackCol.get_ItemByPersistentID(COMVAL.iPersistentIDHigh[icnt], COMVAL.iPersistentIDRow[icnt]);
                        if(track != null)
                            track.Delete();

                    }
                    else
                    {
                        COMVAL.strName[iNewCnt] = COMVAL.strName[icnt];
                        COMVAL.strAlbum[iNewCnt] = COMVAL.strAlbum[icnt];
                        COMVAL.strArtist[iNewCnt] = COMVAL.strArtist[icnt];
                        COMVAL.strLocation[iNewCnt] = COMVAL.strLocation[icnt];
                        COMVAL.strAlbumArtist[iNewCnt] = COMVAL.strAlbumArtist[icnt];
                        COMVAL.iArtCnt[iNewCnt] = COMVAL.iArtCnt[icnt];
                        COMVAL.iPersistentIDHigh[iNewCnt] = COMVAL.iPersistentIDHigh[icnt];
                        COMVAL.iPersistentIDRow[iNewCnt] = COMVAL.iPersistentIDRow[icnt];
                        COMVAL.blnCompilation[iNewCnt] = COMVAL.blnCompilation[icnt];

                        WriteElementXml(ref xtw, COMVAL.iPersistentIDHigh[icnt], COMVAL.iPersistentIDRow[icnt],COMVAL.blnCompilation[icnt],
                            COMVAL.strName[icnt], COMVAL.strAlbum[icnt], COMVAL.strArtist[icnt],
                            COMVAL.strLocation[icnt],COMVAL.strAlbumArtist[icnt], Convert.ToString(COMVAL.iArtCnt[icnt]));
                        iNewCnt++;
                    }
                    if (splash.Cancel)
                    {
                        Msgbox("処理を中断します。", HeadMsg.NOTICE, 7);
                        splash.SendCacncel = false;
                        splash.SendCan(false);
                        break;
                    }
                    splash.SendData(icnt);
                }

                COMVAL.SetRerize(iNewCnt);

                CloseElementXml(ref xtw);
                xtw.Close();
                
                splash.CloseSplash();
                CONF.pTrackCnt = itunesD.LibraryPlaylist.Tracks.Count.ToString();
                if (strDelSong != "")
                {
                    strDelLogPath = CONF.pErrLog + @"\" + strTime + "(DeleteSong).log";
                    swDelSong = new System.IO.StreamWriter(strDelLogPath, true, System.Text.Encoding.GetEncoding("Shift_Jis"));
                    swDelSong.Write(strHead + strDelSong);
                    swDelSong.Close();
                    swDelSong = null;
                }

                File.Copy(strXmlPath, CONF.pxmlPath, true);
                DeleteFile(strXmlPath);
                if (iDelCnt != 0)
                {
                    Msgbox(iDelCnt + " 件の削除が完了しました。" + "\n" + "ログフォルダにて削除したファイルを確認することが出来ます。", HeadMsg.COMPLETE, 1);
                }
                else
                {
                    Msgbox("削除する曲はありませんでした。", HeadMsg.COMPLETE, 1);
                }
                this.Enabled = true;
                lblTitle.Text = HeaderName.TITLE;
            }
            catch (System.Exception ex)
            {
                if (File.Exists(CONF.pxmlPath))
                    DeleteFile(CONF.pxmlPath);
                splash.CloseSplash();
                Msgbox("System Exception 1010" + "\n" + "ITunEs TooLを終了します。", HeadMsg.HEAD_EXCEPTION, 2);
                pErrMessage += "System Exception 1010 : " + ex.Message + "\n";
                this.Close();
            }
            finally
            {
                CheckSaizenmen();
                this.Enabled = true;
                lblTitle.Text = HeaderName.TITLE;
                if (itunes != null)
                    Marshal.ReleaseComObject(itunes);
                if (app != null)
                    Marshal.ReleaseComObject(app);
                if (libraryPlaylist != null)
                    Marshal.ReleaseComObject(libraryPlaylist);
                
            }

        }

        /// <summary>
        /// 青
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoskyBlue_CheckedChanged(object sender, EventArgs e)
        {
            SetColor(frmColor.SKYBLUE);
        }

        /// <summary>
        /// 緑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoGreen_CheckedChanged(object sender, EventArgs e)
        {
            SetColor(frmColor.LIGHTGREEN);
        }

        /// <summary>
        /// 赤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoTomato_CheckedChanged(object sender, EventArgs e)
        {
            SetColor(frmColor.TOMATO);
        }

        /// <summary>
        /// 紫
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoviolet_CheckedChanged(object sender, EventArgs e)
        {
            SetColor(frmColor.VIOLET);
        }

        /// <summary>
        /// カラーを設定
        /// </summary>
        /// <param name="strColor"></param>
        private void SetColor(string strColor)
        {
            switch (strColor)
            {
                case frmColor.SKYBLUE:
                    label1.BackColor = Color.SteelBlue;
                    lblTitle.BackColor = Color.SkyBlue;
                    pictureBox1.BackColor = Color.SkyBlue;
                    picBottum.BackColor = Color.SkyBlue;
                    picLeft.BackColor = Color.SkyBlue;
                    picRight.BackColor = Color.SkyBlue;
                    button3.BackColor = Color.DeepSkyBlue;
                    button5.BackColor = Color.DeepSkyBlue;
                    COMVAL.strColor = frmColor.SKYBLUE;
                    pictureBox4.BackColor = Color.SkyBlue;
                    label19.BackColor = Color.SteelBlue;
                    break;

                case frmColor.LIGHTGREEN:
                    label1.BackColor = Color.ForestGreen;
                    lblTitle.BackColor = Color.LightGreen;
                    pictureBox1.BackColor = Color.LightGreen;
                    picBottum.BackColor = Color.LightGreen;
                    picLeft.BackColor = Color.LightGreen;
                    picRight.BackColor = Color.LightGreen;
                    button3.BackColor = Color.LimeGreen;
                    button5.BackColor = Color.LimeGreen;
                    COMVAL.strColor = frmColor.LIGHTGREEN;
                    pictureBox4.BackColor = Color.LightGreen;
                    label19.BackColor = Color.ForestGreen;
                    break;
                case frmColor.TOMATO:
                    label1.BackColor = Color.OrangeRed;
                    lblTitle.BackColor = Color.Tomato;
                    pictureBox1.BackColor = Color.Tomato;
                    picBottum.BackColor = Color.Tomato;
                    picLeft.BackColor = Color.Tomato;
                    picRight.BackColor = Color.Tomato;
                    button3.BackColor = Color.OrangeRed;
                    button5.BackColor = Color.OrangeRed;
                    COMVAL.strColor = frmColor.TOMATO;
                    pictureBox4.BackColor = Color.Tomato;
                    label19.BackColor = Color.OrangeRed;
                    break;
                case frmColor.VIOLET:
                    label1.BackColor = Color.DarkMagenta;
                    lblTitle.BackColor = Color.Violet;
                    pictureBox1.BackColor = Color.Violet;
                    picBottum.BackColor = Color.Violet;
                    picLeft.BackColor = Color.Violet;
                    picRight.BackColor = Color.Violet;
                    button3.BackColor = Color.Fuchsia;
                    button5.BackColor = Color.Fuchsia;
                    COMVAL.strColor = frmColor.VIOLET;
                    pictureBox4.BackColor = Color.Violet;
                    label19.BackColor = Color.DarkMagenta;
                    break;

            }
        }

        /// <summary>
        /// クラシックモード対応
        /// </summary>
        private void ClassicCorrespondence()
        {
            Bitmap bitPic = null;

            if (System.Windows.Forms.VisualStyles.VisualStyleInformation.DisplayName == "")
            {
                this.BackColor = Color.LightGray;
                tabSerch.BackColor = SystemColors.Control;
                tabSetting.BackColor = SystemColors.Control;
                tabCopy.BackColor = SystemColors.Control;
                groupBox2.BackColor = SystemColors.Control;
                txtAfter.BackColor = Color.White;
                txtBefor.BackColor = Color.White;
                txtErrLog.BackColor = Color.White;
                txtLogPath.BackColor = Color.White;
                txtSerchChr.BackColor = Color.White;
                txtxmlPath.BackColor = Color.White;
                txtArtWork.BackColor = Color.White;
                tabSettei.BackColor = SystemColors.Control;
                tabSonota.BackColor = SystemColors.Control;
                tabHyouji.BackColor = SystemColors.Control;
                label37.BackColor = SystemColors.Control; 
                pictureBox6.Top = 46;
                bitPic = ImgOpe.SetResources("ロゴ３");
                if (bitPic != null)
                    pictureBox6.Image = bitPic;
            }
            else
            {
                this.BackColor = Color.WhiteSmoke;
                tabSerch.BackColor = Color.White;
                tabSetting.BackColor = Color.White;
                tabCopy.BackColor = Color.White;
                groupBox2.BackColor = Color.White;
                txtAfter.BackColor = Color.WhiteSmoke;
                txtBefor.BackColor = Color.WhiteSmoke;
                txtErrLog.BackColor = Color.WhiteSmoke;
                txtLogPath.BackColor = Color.WhiteSmoke;
                txtSerchChr.BackColor = Color.WhiteSmoke;
                txtxmlPath.BackColor = Color.WhiteSmoke;
                txtArtWork.BackColor = Color.WhiteSmoke;
                tabSettei.BackColor = Color.White;
                tabSonota.BackColor = Color.White;
                tabHyouji.BackColor = Color.White;
                label37.BackColor = Color.White;
                pictureBox6.Top = 49;
                bitPic = ImgOpe.SetResources("ロゴ");
                if (bitPic != null)
                    pictureBox6.Image = bitPic;
                    
            }
        }

        /// <summary>
        /// xmlダイアログ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnxml_Click(object sender, EventArgs e)
        {
            string strPath;
            Dialog clsDialog;

            clsDialog = new Dialog();

            strPath = clsDialog.OpenFileDialog("XMLファイルを選択して下さい。",CONF.pxmlPath);
            if (strPath != "")
            {
                if (Path.GetFileName(strPath) != "ITunesDataLibrary.xml")
                {
                    Msgbox("ITunesDataLibrary.xmlを指定して下さい。", HeadMsg.ATTENTION,2);
                    return;
                }
                if (!CheckRead_Music(strPath))
                {
                    Msgbox("ITunesDataLibrary.xmlのデータが読み込めません。", HeadMsg.ATTENTION, 2);
                    return;
                }
                txtxmlPath.Text = strPath;
                Read_Music(strPath);

                DateTime dtUpdate = System.IO.File.GetLastWriteTime(strPath);

                lblkoushin.Text = dtUpdate.ToString() + " 更新";
                btnDouki.ForeColor = Color.White;
                CONF.pxmlPath = txtxmlPath.Text;
            }
            
            clsDialog = null;
        }

        /// <summary>
        /// xmlを読み込めるか確認
        /// </summary>
        /// <param name="strpath"></param>
        /// <returns></returns>
        private Boolean CheckRead_Music(string strpath)
        {
            Boolean blnret = false;


            XmlTextReader reader = null;

            try
            {
                reader = new XmlTextReader(strpath);

                //ストリームからノードを読み取る
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.LocalName)
                        {
                            case "Name":
                                blnret = true;
                                break;
                            case "Album":
                                blnret = true;
                                break;
                            case "Artist":
                                blnret = true;
                                break;
                            case "Location":
                                blnret = true;
                                break;
                            case "ArtworkCount":
                                blnret = true;
                                break;
                            case "PersistentIDHigh":
                                blnret = true;
                                break;
                            case "PersistentIDrow":
                                blnret = true;
                                break;
                        }
                    }
                }
                return blnret;
            }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 7411 : " + ex.Message + "\n";
                return false;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        ///フォルダ選択ダイヤログ 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnADialog_Click(object sender, EventArgs e)
        {
            string strPath;

            strPath = OpenDialog("フォルダを指定してください。", txtAfter.Text);
            if (strPath != "")
                txtAfter.Text = strPath;
        }
        
        /// <summary>
        /// エラーチェックダイアログ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBDialog_Click(object sender, EventArgs e)
        {
            string strPath;
            
            strPath = OpenDialog("エラーチェックファイルを保存するフォルダを指定して下さい。", txtLogPath.Text);
            if (strPath != "")
                txtLogPath.Text = strPath;
        }

        /// <summary>
        /// エラーログダイアログ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnErrLog_Click(object sender, EventArgs e)
        {
            string strPath;

            strPath = OpenDialog("エラーログを保存するフォルダを指定して下さい。", CONF.pErrLog);
            if (strPath != "")
                txtErrLog.Text = CONF.pErrLog = strPath;
        }

        /// <summary>
        /// ダイアログオープン
        /// </summary>
        /// <param name="strDescription"></param>
        /// <param name="strHead"></param>
        /// <returns></returns>
        private string OpenDialog(string strDescription, string strHead)
        {
            string strPath;
            Dialog clsDialog;

            clsDialog = new Dialog();

            strPath = clsDialog.OpenFolderDialog(this, strDescription, strHead);
            return strPath;
        }

        /// <summary>
        /// 最前面チェックボックスの状態
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSaizenmen_CheckedChanged(object sender, EventArgs e)
        {
            chkSaizenmen.ForeColor = chkSaizenmen.Checked ? Color.Black : Color.DimGray;
            CheckSaizenmen();
        }

        /// <summary>
        /// チェック最前面
        /// </summary>
        private void CheckSaizenmen()
        {
            this.TopMost = chkSaizenmen.Checked ? true : false;
        }
        /// <summary>
        /// 最前面
        /// </summary>
        private void DialogSaizenmen()
        {
            if (this.TopMost)
                this.TopMost = false;
        }
       
        /// <summary>
        /// 設定されてない項目を検索（チェックボックス）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBlank_CheckedChanged(object sender, EventArgs e)
        {
            chkBlank.ForeColor = chkBlank.Checked ? Color.Black : Color.DimGray;
        }

        /// <summary>
        /// ITunesから曲をドラッグ＆ドロップする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabProperty_DragDrop(object sender, DragEventArgs e)
        {
            iTunesLib.IITTrackCollection trackCol = null;
            iTunesLib.IITArtworkCollection AWC = null;
            iTunesLib.IITArtwork AW = null;

            int index = 0;
            try{

                trackCol = itunesD.LibraryPlaylist.Tracks;

                if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                    return;
           
                if (COMVAL.strLocation == null)
                {
                    Msgbox("iTunesと同期して下さい。", HeadMsg.ATTENTION, 5);
                    return;
                }
                 
                foreach (string filePath in (string[])e.Data.GetData(DataFormats.FileDrop))
                {
                    index = Array.IndexOf(COMVAL.strLocation, filePath);

                    if (index == -1)
                    {
                        Msgbox("ITunEsTooLにドラッグした曲の情報がありません。"+ "\n" + "再度、同期を行って下さい。", HeadMsg.ATTENTION, 5);
                        return;
                    }

                    TR = trackCol.get_ItemByPersistentID(COMVAL.iPersistentIDHigh[index], COMVAL.iPersistentIDRow[index]);
                    if(TR == null)
                    {
                        Msgbox("ドラッグした曲の情報がありません。" + "\n" + "再度、同期を行って下さい。", HeadMsg.ATTENTION, 5);
                        return;
                    }
                    SetArtWork(TR);
                    break;
                }
            }
            catch (System.Exception ex)
            {
                if (File.Exists(CONF.pxmlPath))
                    DeleteFile(CONF.pxmlPath);
                DialogSaizenmen();
                Msgbox("System Exception 1027" + "\n" + "ITunEs TooLを終了します。", HeadMsg.HEAD_EXCEPTION, 4);
                pErrMessage += "System Exception 1027 : " + ex.Message + "\n";
                this.Close();
            }
            finally
            {
                if (AWC != null)
                    Marshal.ReleaseComObject(AWC);
                if (AW != null)
                    Marshal.ReleaseComObject(AW);
                if (trackCol != null)
                    Marshal.ReleaseComObject(trackCol);
            }
        }

        /// <summary>
        /// アートワーク設定
        /// </summary>
        /// <param name="TTR">IITTrack</param>
        private void SetArtWork(iTunesLib.IITTrack TTR,Boolean NextTrack = false)
        {
            FileStream fs = null;
            IITArtwork AW = null;
            IITArtworkCollection AWC = null;
            Image imgTmp = null;

            try
            {
                if (TTR == null)
                {
                    SetNoImage(false);
                    return;
                }

                AWC = TTR.Artwork;

                lblName.Text = TTR.Name;
                lblAlbum.Text = TTR.Album;
                lblArtist.Text = TTR.Artist;
                if (AWC.Count > 0)
                {

                    AW = AWC[1];
                    //AW.
                    //imgTmp = (Image)AW.Format; ;

                    AW.SaveArtworkToFile(CONF.ppath + @"\tmp.jpg");
                    using (fs = new FileStream(CONF.ppath + @"\tmp.jpg", FileMode.Open, FileAccess.Read))
                    using (Image imgSrc = Image.FromStream(fs))
                    { 
                        pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
                        try
                        {
                            imgTmp = new Bitmap(imgSrc);
                            
                            if (imgTmp != null)
                                pictureBox5.Image = imgTmp;
                        }
                        catch (ArgumentException)
                        {
                            return;
                        }
                    }
                }
                else
                {
                    SetNoImage(true);
                }

                if (NextTrack) 
                { 
                    if (itunesD.CurrentTrack != null)
                        TR = itunesD.CurrentTrack;
                }
            }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 1028 : " + ex.Message + "\n";
                SetNoImage(false);
                return;
            }
            finally
            {
                if (AWC != null) Marshal.ReleaseComObject(AWC);
                if (AW != null)  Marshal.ReleaseComObject(AW);
                if (fs != null)  fs.Close();
                DeleteFile(CONF.ppath + @"\tmp.jpg");
            }
        }

        /// <summary>
        /// NoImageの画像をセットする
        /// </summary>
        private void SetNoImage(Boolean ImageOnly)
        {
            Bitmap bitPic = null;

            if(!ImageOnly)
            {
                lblName.Text = "";
                lblAlbum.Text = "";
                lblArtist.Text = "";
            }
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            bitPic = ImgOpe.SetResources("NoImage");
            if (bitPic != null)
                pictureBox5.Image = bitPic;
        }

        /// <summary>
        /// 曲にアートワークを設定
        /// </summary>
        /// <param name="strname"></param>
        /// <param name="strAlbum"></param>
        /// <param name="strArtist"></param>
        /// <returns></returns>
        private Boolean AddArtworkFromFile(ref IITTrack TTrack, string strPicPath)
        {
            
            try{
                
                if(TTrack != null)
                    TTrack.AddArtworkFromFile(strPicPath); 
                return true;
            }
            catch (System.Exception ex)
            {

                pErrMessage += "AddArt Exception 1035 : " + ex.Message + "\n";
                return false;
            }
        }

        /// <summary>
        /// ダウンロードしたファイル名を付ける
        /// </summary>
        /// <param name="strname"></param>
        /// <param name="strAlbum"></param>
        /// <param name="strArtist"></param>
        /// <returns></returns>
        private string SetArtworkName(string strname, string strAlbum, string strArtist)
        {
            string strArtworkName = string.Empty;

            if (rdoArtName.Checked)
                strArtworkName = strname;
            if (rdoArtAlbum.Checked)
                strArtworkName = strAlbum;
            if (rdoArtArtist.Checked)
                strArtworkName = strArtist;
            if (rdoNmArb.Checked)
                strArtworkName = strname + "_" + strAlbum;
            if (rdoArbArt.Checked)
                strArtworkName = strAlbum + "_" + strArtist;
            if (strArtworkName == "")
                strArtworkName = "NoName";
            return strArtworkName;
        }

        /// <summary>
        /// 拡張子を設定
        /// </summary>
        /// <returns></returns>
        private string SetKakuchoushi()
        {
            string strPicture = string.Empty;

            if (rdoJpg.Checked)
                strPicture = "jpg";
            if (rdoPng.Checked)
                strPicture = "png";
            if (rdoBmp.Checked)
                strPicture = "bmp";
            return strPicture;
        }
      
        /// <summary>
        /// 設定されているアートワーク削除
        /// </summary>
        /// <param name="AW"></param>
        private void DeleteArtwork(IITArtworkCollection ArtworkCollection)
        {
            int i = 0;
            try
            {
                for (i = ArtworkCollection.Count; i > 0; i--)
                    ArtworkCollection[i].Delete();
            }
            catch (System.Exception ex)
            {
                pErrMessage += "track_Dlt Exception 1034 : " + ex.Message + "\n";
            }

        }

        /// <summary>
        /// 重複ファイルがあったら、連番を付加して文字列を返す。
        /// </summary>
        /// <param name="strFilePath"></param>
        private string RepeatedFile(string strFilePath)
        {
            string strFile = string.Empty;
            int iRenban = 1;

            try{
                strFile = strFilePath;
                if (File.Exists(strFilePath + "." + CONF.pSavePicture))
                {
                    for (iRenban = 2; ; iRenban++)
                    {
                        strFile = strFilePath + "(" + iRenban.ToString() + ")";
                        if (!File.Exists(strFile + "." + CONF.pSavePicture))
                            break;
                    }
                }
                return strFile + "." + CONF.pSavePicture;
             }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 1030 : " + ex.Message + "\n";
                return "";
            }
        }
        
        /// <summary>
        /// 再生ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaisei_Click(object sender, EventArgs e)
        {
            try{

                if (COMVAL.strName == null)
                    return;
		        //両方のインスタンスがない場合
		        if (TR == null && itunesD.CurrentTrack == null)
                {
                    IITTrackCollection trackCol = null;
                    trackCol = itunesD.LibraryPlaylist.Tracks;
                    TR = trackCol.get_ItemByPersistentID(COMVAL.iPersistentIDHigh[1], COMVAL.iPersistentIDRow[1]);
                    TR.Play();
                    SetArtWork(TR);

		        //曲が設定されてない且つ、iTunesの局が流れてる場合
                }
                else if (TR == null && itunesD.CurrentTrack != null )
                {
				    if(!itunesD.CurrentTrack.Enabled)
					    itunesD.CurrentTrack.Play();
				    else
					    itunesD.PlayPause();
				    SetArtWork(itunesD.CurrentTrack);
                    TR = itunesD.CurrentTrack;

		        //局が設定されていて且つ、iTunesの曲と設定されている曲が異なる場合
                }
                else if ((itunesD.CurrentTrack == null && TR != null) )
                {
				    TR.Play();
				    SetArtWork(TR);
		        }else if (itunesD.CurrentTrack.PlayOrderIndex == TR.PlayOrderIndex)
                {
                    if (!itunesD.CurrentTrack.Enabled)
                        itunesD.CurrentTrack.Play();
                    else
                        itunesD.PlayPause();
                    SetArtWork(itunesD.CurrentTrack);
                    TR = itunesD.CurrentTrack;
                }else if (itunesD.CurrentTrack.PlayOrderIndex != TR.PlayOrderIndex)
                {
                    TR.Play();
                    SetArtWork(TR);
                }

		    }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 1029 : " + ex.Message + "\n";
            }
        }

        /// <summary>
        /// 曲次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            try{
                if (COMVAL.strName == null)
                    return;
                itunesD.NextTrack();
                SetArtWork(itunesD.CurrentTrack,true);
             }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 1032 : " + ex.Message + "\n";
            }
        }

        /// <summary>
        /// 曲戻る
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrev_Click(object sender, EventArgs e)
        {
            try{
                if (COMVAL.strName == null)
                    return;
                itunesD.BackTrack();
                SetArtWork(itunesD.CurrentTrack, true);
            }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 1033 : " + ex.Message + "\n";
            }
        }

        /// <summary>
        /// ダイヤログ（アートワーク）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnArtWork_Click(object sender, EventArgs e)
        {
            string strPath;
            
            strPath = OpenDialog("アートワークを保存するフォルダを指定して下さい。", CONF.pxmlPath);
            if (strPath != "")
                CONF.pArtWork = txtArtWork.Text = strPath;
            label26.ForeColor = Color.Black;
        }

        /// <summary>
        /// クリップボードにコピーする。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGoogle_Click(object sender, EventArgs e)
        {
            try{
                if (TR != null)
                {
                    if (TR.Album != null)
                        Clipboard.SetDataObject(TR.Album, true);
                }
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                Msgbox("ITunEsTooLに設定されている曲は" + "\n" +"iTunesに登録されていない可能性があります。" + "\n" + "再度、同期を行ってください。", HeadMsg.NOTICE, 2);
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            try{
                if (TR != null)
                {
                    if (TR.Name != null)
                        Clipboard.SetDataObject(TR.Name, true);
                }
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                Msgbox("ITunEsTooLに設定されている曲は" + "\n" +"iTunesに登録されていない可能性があります。" + "\n" + "再度、同期を行ってください。", HeadMsg.NOTICE, 2);
            }
        }
        private void button6_Click_1(object sender, EventArgs e)
        {
            try{
                if (TR != null)
                {
                    if (TR.Artist != null)
                        Clipboard.SetDataObject(TR.Artist, true);
                }
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                Msgbox("ITunEsTooLに設定されている曲は" + "\n" +"iTunesに登録されていない可能性があります。" + "\n" + "再度、同期を行ってください。", HeadMsg.NOTICE, 2);
            }
        }

        /// <summary>
        /// ボリューム
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try{
                this.Update();
            }
            catch (System.Exception ex)
            {                
                Msgbox("System Exception 3045" + "\n" + "iTunesが起動されていないか、予期せぬエラーが発生しました。", HeadMsg.HEAD_EXCEPTION, 4);
                pErrMessage += "System Exception 3045 : " + ex.Message + "\n";
                this.Close();
            }
        }

        /// <summary>
        /// 同期状況によってボタン色を変更する
        /// </summary>
        private void DoukiColor()
        {
            if (!File.Exists(CONF.pxmlPath))
            {
                btnDouki.ForeColor = Color.Red;
                lblkoushin.Text = "";
                tabControl1.SelectedTab = tabControl1.TabPages["tabSetting"];
            }
            else
            {
                btnDouki.ForeColor = Color.White;
                lblkoushin.Text = CONF.pDate == "なし" ? "" : CONF.pDate;
            }
        }
        /// <summary>
        /// アートワークチェックボックス
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkArtworkDel_CheckedChanged(object sender, EventArgs e)
        {
            ChangeCheckBoxParameter(ref chkArtworkDel, ref CONF.pDelArt);
        }

        /// <summary>
        /// アートワークバックアップ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBackArt_Click(object sender, EventArgs e)
        {
            IITLibraryPlaylist libraryPlaylist = null;
            iTunesApp app = null;
            DialogResult YorN = DialogResult.No;
            Boolean blnCancel = false;
            string strPath;
            Dialog clsDialog;;
            int icnt = 0;
            int iChkCnt = 0;
            string[] strChkAlbum = new string[1];
            string[] strChkArtist = new string[1];
            string[] strChkAlbumArtist = new string[1];
            Boolean[] blnChkCompilation = new Boolean[1];
            IITTrack Track = null;
            IITTrackCollection trackCol = null;
            ITunEsTooL_Beams IBeans = new ITunEsTooL_Beams();

            if (COMVAL.strName == null)
            {
                Msgbox("設定にて、iTunesの情報を取得して下さい。", HeadMsg.ATTENTION, 5);
                return;
            }

            YorN = Msgbox("アートワークのバックアップを行いますか？"
                                                        + "\n" 
                                                        + "※iTunesと同期直後でない場合、正しい結果が得られない場合があります。",
                                                        HeadMsg.CONNECT_ITUNES, 3);
            if (YorN == DialogResult.No)
                return;

            try
            {
                app = new iTunesApp();
                libraryPlaylist = app.LibraryPlaylist;               
                clsDialog = new Dialog();
                strPath = clsDialog.OpenFolderDialog(this,"バックアップするフォルダを指定して下さい。");
                if (strPath == "")
                    return;
                clsDialog = null;

                splash.SendDataALL(libraryPlaylist.Tracks.Count);
                Message = "アートワークをバックアップ中です...";
                DialogSaizenmen();
                splash.SendVisible(true);
                backgroundWorker1.RunWorkerAsync();
                
                trackCol = itunesD.LibraryPlaylist.Tracks;
                
                for (icnt = 0; icnt < COMVAL.strName.Length; icnt++)
                {
                    if (COMVAL.iArtCnt[icnt] != 0 && IBeans.GetAlbumMachiIndex(strChkAlbum,strChkArtist, strChkAlbumArtist, blnChkCompilation,
                        COMVAL.strAlbum[icnt],COMVAL.strArtist[icnt], COMVAL.strAlbumArtist[icnt], COMVAL.blnCompilation[icnt]) == -1)
                    {

                        Track = trackCol.get_ItemByPersistentID(COMVAL.iPersistentIDHigh[icnt], COMVAL.iPersistentIDRow[icnt]);
                        MakeArtWork(Track, strPath);
                        strChkAlbum[iChkCnt] = COMVAL.strAlbum[icnt];
                        strChkAlbumArtist[iChkCnt] = COMVAL.strAlbumArtist[icnt];
                        blnChkCompilation[iChkCnt] = COMVAL.blnCompilation[icnt];
                        strChkArtist[iChkCnt] = COMVAL.strArtist[icnt];
                        iChkCnt++;
                        Array.Resize(ref strChkAlbum, iChkCnt + 1);
                        Array.Resize(ref strChkAlbumArtist, iChkCnt + 1);
                        Array.Resize(ref blnChkCompilation, iChkCnt + 1);
                        Array.Resize(ref strChkArtist, iChkCnt + 1);
                        blnCancel = splash.Cancel;
                        if (blnCancel)
                        {
                            Msgbox("処理を中断します。", HeadMsg.NOTICE,7);
                            splash.SendCacncel = false;
                            splash.SendCan(false);
                            break;
                        }
                    }
                    splash.SendData(icnt);
                }               

                splash.CloseSplash();
                Msgbox("バックアップが完了しました。", HeadMsg.COMPLETE, 1);

            }
            catch (System.Exception ex)
            {
                if (File.Exists(CONF.pxmlPath))
                    DeleteFile(CONF.pxmlPath);
                splash.CloseSplash();
                Msgbox("System Exception 1054" + "\n" + "ITunEs TooLを終了します。", HeadMsg.HEAD_EXCEPTION, 4);
                pErrMessage += "System Exception 1054 : " + ex.Message + "\n";
                this.Close();
            }
            finally
            {
                CheckSaizenmen();
                if (app != null)
                    Marshal.ReleaseComObject(app);
                if (libraryPlaylist != null)
                    Marshal.ReleaseComObject(libraryPlaylist);
            }
        }

        /// <summary>
        /// アートワーク設定
        /// </summary>
        /// <param name="TTR">IITTrack</param>
        private void MakeArtWork(iTunesLib.IITTrack TTR,string strDirName)
        {
            IITArtwork AW = null;
            IITArtworkCollection AWC = null;
            FileStream fs = null;
            Image imgTmp = null;
            Image imgSrc = null;
            ImageFormat format = ImageFormat.Jpeg;
            string strPath = string.Empty;
            string strAlbum = string.Empty;

            try
            {
                if (TTR == null)
                    return;
                AWC = TTR.Artwork;       
         
                foreach (IITArtwork Art in AWC)
                {
                    strAlbum = TTR.Album;
                    strPath = strDirName + @"\" + FileOpe.ValidFileName(strAlbum);
                    strPath = RepeatedFile(strPath);
                    Art.SaveArtworkToFile(CONF.ppath + @"\tmp.jpg");
                    using (fs = new FileStream(CONF.ppath + @"\tmp.jpg", FileMode.Open, FileAccess.Read))
                    {
                        using (imgSrc = Bitmap.FromStream(fs)) 
                        {
                            try{
                                imgTmp = new Bitmap(imgSrc);
                            }
                            catch (ArgumentException)
                            {
                                break;
                            }
                        }   
                    }

                    if (rdoPng.Checked)
                        format = ImageFormat.Png;
                    else if (rdoBmp.Checked)
                        format = ImageFormat.Bmp;
                    imgTmp.Save(strPath, format);
                }
            }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 1040 : " + ex.Message + "\n";
            }
            finally
            {
                if (AWC != null) { Marshal.ReleaseComObject(AWC); }
                if (AW != null) { Marshal.ReleaseComObject(AW); }
                if (fs != null) { fs.Close(); }
                DeleteFile(CONF.ppath + @"tmp.jpg");
            }

        }

        /// <summary>
        /// ALL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllSong_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult YorN;
                YorN = Msgbox("iTunesに登録されている曲を全て追加しますか？", HeadMsg.NOTICE,3);

                if (YorN == DialogResult.No)
                    return;
                if (COMVAL.strLocation == null)
                {
                    Msgbox("iTunesと同期して下さい。", HeadMsg.ATTENTION, 5);
                    return;
                }
                if (COMVAL.strPaths == null)
                    COMVAL.strPaths = new string[1];

                COMVAL.strPaths = COMVAL.strLocation;
                txtBefor.Text = COMVAL.strPaths[0];
                pSelectCnt = COMVAL.strPaths.Length;
                txtNumber.Text = Convert.ToString(COMVAL.strPaths.Length - 1);
            }
            catch (System.Exception ex)
            {
                if (File.Exists(CONF.pxmlPath))
                    DeleteFile(CONF.pxmlPath);
                Msgbox("System Exception 1052" + "\n" + "ITunEs TooLを終了します。", HeadMsg.HEAD_EXCEPTION, 4);
                pErrMessage += "System Exception 1052 : " + ex.Message + "\n";
                this.Close();
            }
            finally
            {

            }
        }

        /// <summary>
        /// バージョン画面表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            frmVersion GetName = new frmVersion();

            GetName.ParentForm = this;
            DialogSaizenmen();
            GetName.ShowDialog();
            CheckSaizenmen();
        }

        /// <summary>
        /// 検索（テキスト）ダブルクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSerchChr_DoubleClick(object sender, EventArgs e)
        {
            txtSerchChr.Text = ".*";
        }

        /// <summary>
        /// タブ切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Update();
        }

        /// <summary>
        /// タブ切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Update();
        }

        /// <summary>
        /// 同期チェックボックスクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkDouki_CheckedChanged(object sender, EventArgs e)
        {
            ChangeCheckBoxParameter(ref  chkDouki, ref CONF.pSync);
        }

        /// <summary>
        /// ○アートワーク設定（画像から）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void groupBox4_DragDrop(object sender, DragEventArgs e)
        {
            string strName = string.Empty;
            string strArtist = string.Empty;
            string strAlbum = string.Empty;
            string strAlbumArtist = string.Empty;
            string strPath = string.Empty;
            string strExt = string.Empty;
            int icnt = 0;
            int imax = 0;
            int imusic = 0;
            DialogResult YorN = DialogResult.No;
            IITTrackCollection trackCol = null;
            IITFileOrCDTrack FileOrCDTrack = null;
            Boolean blnCompilation = false;
            ITunEsTooL_Beams IBeans = new ITunEsTooL_Beams();
            IITTrack track = null;
            Boolean blnNotAlbum = false;

            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            foreach (string filePath in (string[])e.Data.GetData(DataFormats.FileDrop))
            {
                strExt = Path.GetExtension(filePath);
                if (strExt == ".jpg" || strExt == ".png" || strExt == ".bng")
                {
                    strPath = filePath;
                    break;
                }
                else
                {
                    Msgbox("画像ファイルをドラッグしてください。", HeadMsg.NOTICE,2);
                    return;
                }
            }

            if (TR == null)
            {
                Msgbox("iTunesから曲をドラッグしてください。", HeadMsg.NOTICE,2);
                return;
            }
            if (TR.Album == null)
            {
                blnNotAlbum = true;
            }
            if (chkArtworkDel.Checked && TR.Artwork.Count != 0)
            {
                if (Msgbox("現在設定されている画像が削除されます。" + "\n" + "画像のバックアップを取りますか？", HeadMsg.ATTENTION, 3) == DialogResult.Yes)
                {
                    PicBackup(TR);
                }
            }
            try
            {
                if (TR.Name != null)
                    strName = TR.Name;
                if (TR.Album != null)
                    strAlbum = TR.Album;
                if (TR.Artist != null)
                    strArtist = TR.Artist;              
                blnCompilation = TR.Compilation;
                FileOrCDTrack = (IITFileOrCDTrack)TR;
                if (FileOrCDTrack.AlbumArtist != null)
                    strAlbumArtist = FileOrCDTrack.AlbumArtist;

                imax = IBeans.GetAlbumCount(strAlbum,strArtist, strAlbumArtist, blnCompilation, ref COMVAL);
                DialogSaizenmen();
                splash.SendDataALL(imax);
                Message = "画像を設定しています...";
                
                splash.SendVisible(true);

                trackCol = itunesD.LibraryPlaylist.Tracks;

                splash.SendMessage("アートワークを設定しています。");
                splash.SendVisible(true);
                if (!blnNotAlbum)
                {
                    YorN = Msgbox("ドラッグしたの画像を【 " + strAlbum + " 】のすべての曲に設定しますか？", HeadMsg.NOTICE, 3);
                }
                else
                {
                    YorN = Msgbox("クリップボードの画像を設定してもよろしいですか？", HeadMsg.NOTICE, 3);
                    if (YorN == DialogResult.No)
                        return;

                    YorN = DialogResult.No;
                }
                if (YorN == DialogResult.No)
                    splash.SendDataALL(1);

                backgroundWorker1.RunWorkerAsync();
                if (YorN == DialogResult.No)
                {
                    icnt = IBeans.GetMusicMachiIndex(ref COMVAL,itunesD.get_ITObjectPersistentIDHigh(TR),itunesD.get_ITObjectPersistentIDLow(TR));
                    if (!SetArtworkMusic(ref track, strPath, icnt))
                    {
                        splash.CloseSplash();
                        Msgbox("【 " + COMVAL.strName[icnt] + " 】に画像を設定することが出来ませんでした。",HeadMsg.ATTENTION,5);
                        return;
                    }
                    
                    imusic++;
                }
                else
                {

                    for (icnt = 0; icnt < COMVAL.strName.Length; icnt++)
                    {
                        icnt = Array.IndexOf(COMVAL.strAlbum, strAlbum, icnt);
                        if (icnt == -1)
                            break;

                        if (IBeans.MachiAlbum(COMVAL,strAlbum, strArtist, strAlbumArtist, blnCompilation,ref icnt))
                        {
                            if (!SetArtworkMusic(ref track, strPath, icnt))
                            {
                                splash.CloseSplash();
                                Msgbox("【 " + COMVAL.strName[icnt] + " 】に画像を設定することが出来ませんでした。",HeadMsg.ATTENTION,5);
                                return;
                            }
                            SetArtWork(track);
                            imusic++;
                            splash.SendData(imusic);
                        }
                        if (splash.Cancel)
                        {
                            Msgbox("処理を中断します。", HeadMsg.NOTICE, 7);
                            splash.SendCacncel = false;
                            splash.SendCan(false);
                            break;
                        }                                                                   
                    }
                }
                splash.CloseSplash();
                Msgbox("画像設定が完了しました。", HeadMsg.COMPLETE,1);
                SetArtWork(TR);
            }
            catch (System.Exception ex)
            {
                splash.CloseSplash();
                Msgbox("System Exception 1248" + "\n" + "ITunEs TooLを終了します。", HeadMsg.HEAD_EXCEPTION, 4);
                pErrMessage += "System Exception 1248 : " + ex.Message + "\n";
            }
            finally
            {
                CheckSaizenmen();
                this.Enabled = true;
                lblTitle.Text = HeaderName.TITLE;

            }
        }

        /// <summary>
        /// クリップボードの画像を設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            int icnt = 0;
            int imax = 0;
            int iArtCnt = 0;
            int idownload = 0;
            string strPath = string.Empty;
            string strName = string.Empty;
            string strArtist = string.Empty;
            string strAlbum = string.Empty;
            string strtmpAlbum = string.Empty;
            string strtmpsch = string.Empty;
            string strAlbumArtist = string.Empty;
            string strArtworkName = string.Empty;
            byte[] imgChkData = null;
            Boolean blnCompilation = false;
            Boolean blnCancel = false;
            DialogResult YorN = DialogResult.No;

            iTunesApp itunes = null;
            IITTrack track = null;
            IITFileOrCDTrack FileOrCDTrack = null;

            ITunEsTooL_Beams IBeans = new ITunEsTooL_Beams();
            PicCheck frmPicChk = new PicCheck();
            
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                Msgbox("インターネットに接続されていません。", HeadMsg.NOTICE, 2);
                return;
            }
            if (COMVAL.strLocation == null)
            {
                Msgbox("設定にて、iTunesの情報を取得して下さい。", HeadMsg.ATTENTION,2);
                return;
            }
            if (TR== null)
            {
                Msgbox("曲をITunEsTooLにドラッグして下さい。", HeadMsg.ATTENTION,2);
                return;
            }
            if (TR.Album == null)
            {
                Msgbox("アルバム名が設定されていない曲は画像を自動取得出来ません。"+ "\n"+ "アルバム名を設定して下さい。", HeadMsg.ATTENTION,2);                
                return;
            }

            if (!Directory.Exists(CONF.pArtWork))
            {
                Msgbox("画像の保存場所が存在しません。設定タブにて保存場所の設定を行ってください。", HeadMsg.ATTENTION, 2);
                tabControl1.SelectedTab = tabControl1.TabPages["tabSetting"];
                return;
            }
            if (chkArtworkDel.Checked && TR.Artwork.Count != 0)
            {
                if (Msgbox("現在設定されている画像が削除されます。" + "\n" + "画像のバックアップを取りますか？", HeadMsg.ATTENTION, 3) == DialogResult.Yes)
                {
                    PicBackup(TR);
                } 
            }
            
            YorN = Msgbox("ドラッグした曲のアートワークの自動設定を行います。"  +  "\n" + "※iTunesと同期直後でない場合、正しい結果が得られない場合があります。","自動設定",3);           

            if (YorN == DialogResult.No)
                return;

            if (TR.Name != null)
                strName = TR.Name;
            if (TR.Album != null)
                strAlbum = TR.Album;
            if (TR.Artist != null)
                strArtist = TR.Artist;
            blnCompilation = TR.Compilation;
            FileOrCDTrack = (IITFileOrCDTrack)TR;
            if (FileOrCDTrack.AlbumArtist != null)
                strAlbumArtist = FileOrCDTrack.AlbumArtist;
 
            try
            {
                itunes = new iTunesAppClass();

                imax = IBeans.GetAlbumCount(strAlbum,strArtist, strAlbumArtist, blnCompilation, ref COMVAL);

                splash.SendDataALL(imax);
                Message = "画像をダウンロードしています...";
                DialogSaizenmen();
                splash.SendVisible(true);
                
                backgroundWorker1.RunWorkerAsync();
               
                //曲名のみの場合の検索を考える。
                strtmpAlbum = DelDisc(strAlbum);

                strtmpsch = GoogleKensaku(strtmpAlbum, strArtist, strAlbumArtist, blnCompilation);
                
                //results = GoogleSerch(strtmpsch);
                var iTunesSearchApiModel = GetItunesSearchApiObject(strtmpsch);
                if (iTunesSearchApiModel == null)
                {
                    Msgbox("画像を検索することが出来ませんでした。", HeadMsg.NOTICE, 2);
                    return;
                }

                foreach (var result in iTunesSearchApiModel.results)
                {
                    strArtworkName = SetArtworkName(strName, strAlbum, strArtist);
                    strPath = RepeatedFile(txtArtWork.Text + @"\" + FileOpe.ValidFileName(strArtworkName));

                    idownload = DownloadData(result.artworkUrl30.Replace("30x30bb", "500x500bb"), ref imgChkData);
                    if (idownload >= 2) { continue; }

                    if (PicCheckFlag)
                    {
                        splash.SendVisible(false);
                        frmPicChk.Artist = strArtist;
                        frmPicChk.Album = strAlbum;
                        frmPicChk.MusicName = strName;
                        frmPicChk.Url = strPath;
                        frmPicChk.SetEvent = 2;
                        frmPicChk.SetColor = pictureBox1.BackColor;
                        frmPicChk.Img = imgChkData;
                        DialogSaizenmen();
                        frmPicChk.ShowDialog();
                        if (!frmPicChk.Ret)
                        {
                            splash.SendVisible(true);
                            continue;
                        }
                    }

                    if (!SaveImg(imgChkData, strPath)) {
                        Msgbox("画像が設定出来ませんでした。再検索を行います。", HeadMsg.ATTENTION, 5); 
                        continue;
                    }
                    
                    splash.SendMessage("画像を自動設定しています...");
                    splash.SendVisible(true);
               
                    for (icnt = 0; icnt < COMVAL.strName.Length; icnt++)
                    {
                        icnt = Array.IndexOf(COMVAL.strAlbum, strAlbum, icnt);
                        if (icnt == -1)
                            break;
                        if (IBeans.MachiAlbum(COMVAL,strAlbum, strArtist, strAlbumArtist, blnCompilation,ref  icnt))
                        {
                            if (SetArtworkMusic(ref track, strPath, icnt))
                            {
                                iArtCnt++;
                                splash.SendData(iArtCnt);
                                if (splash.Cancel)
                                {
                                    Msgbox("処理を中断します。", HeadMsg.NOTICE, 7);                                    
                                    blnCancel = true;
                                    splash.SendCacncel = false;
                                    splash.SendCan(false);
                                    break;
                                }
                            }
                            COMVAL.iArtCnt[icnt] = track.Artwork.Count;
                            SetArtWork(track);
                        }
                    }

                    if (!blnCancel)
                    {
                        if (splash.Cancel)
                        {
                            Msgbox("処理を中断します。", HeadMsg.NOTICE,7);
                            splash.SendCacncel = false;
                            splash.SendCan(false);
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                    break;                                   
                }

                splash.CloseSplash();
                if(iArtCnt > 0)
                    Msgbox("自動設定が完了しました。", HeadMsg.COMPLETE, 1);
                else
                    Msgbox("アートワークの自動取得が出来ませんでした" + "\n" + "指定のサイズの画像が見つからない可能性があります。", HeadMsg.NOTICE, 2);
                SetArtWork(TR);

            }
            catch (System.Exception ex)
            {
                splash.CloseSplash();
                Msgbox("System Exception 1153" + "\n" + "ITunEs TooLを終了します。", HeadMsg.HEAD_EXCEPTION, 4);
                pErrMessage += "System Exception 1153 : " + ex.Message + "\n";
                this.Close();
            }
            finally
            {
                if (track != null) { Marshal.ReleaseComObject(itunes); }
                if (itunes != null) { Marshal.ReleaseComObject(itunes); }
                if (FileOrCDTrack != null) { Marshal.ReleaseComObject(FileOrCDTrack); }
                CheckSaizenmen();
            }
        
        }

        /// <summary>
        /// 画像を保存
        /// </summary>
        /// <param name="btpic">画像データ（byte）</param>
        /// <param name="strPath">保存先のパス</param>
        /// <returns></returns>
        private Boolean SaveImg(byte[] btpic,string strPath)
        {
            MemoryStream st = null;
            Image imgTmp = null;
            ImageFormat format = ImageFormat.Jpeg;

            try
            {
                using (st = new MemoryStream(btpic))
                using (Image imgSrc = Image.FromStream(st))
                {
                    imgTmp = new Bitmap(imgSrc);
                }

                if (rdoPng.Checked)
                    format = ImageFormat.Png;
                else if (rdoBmp.Checked)
                    format = ImageFormat.Bmp;

                imgTmp.Save(strPath, format);
                
                return true;
            }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 13457 : " + ex.Message + "\n";
                return false;
            }
            finally
            {
                st.Close();
            }
        }

        ///// <summary>
        ///// google検索
        ///// </summary>
        ///// <param name="strSerchWord"></param>
        ///// <returns></returns>
        //private SearchResults GoogleSerch(string strSerchWord)
        //{
          
        //        SearchResults results = null;
        //        int iSchCnt = 0;
        //        //検索出来るまで繰り返す（ただし2回まで）
        //        while (results == null)
        //        {
        //            try
        //            {
        //                results = Searcher.Search(SearchType.Image, strSerchWord);
        //                if (results != null)
        //                    break;
        //            }
        //            catch (System.Exception ex)
        //            {
        //                if(pErrMessage.IndexOf("Web Exception 4123 : " + ex.Message + ": " + strSerchWord +  "\n") != -1)
        //                    pErrMessage += "Web Exception 4123 : " + ex.Message + ": " + strSerchWord +  "\n";                        
        //            }
                    
        //            iSchCnt++;
        //            Thread.Sleep(1000);
        //            if(iSchCnt == 2)
        //                break;
        //        }
        //        return results;
            
        //}

        private ITunesSearchAPI GetItunesSearchApiObject(string serchWord)
        {
            var client = new RestClient(string.Format("https://itunes.apple.com/search?lang=ja_jp&entry=music&media=music&country=JP&term={0}", serchWord));
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);
            var iTunesSearchApiModel = (response.StatusCode == HttpStatusCode.OK) ? JsonConvert.DeserializeObject<ITunesSearchAPI>(response.Content) : new ITunesSearchAPI();
            return iTunesSearchApiModel;
        }

        /// <summary>
        /// アルバムの状態により、ググる文字列を変える
        /// </summary>
        /// <param name="strAlbum"></param>
        /// <param name="strArtist"></param>
        /// <param name="strAlbumArtist"></param>
        /// <param name="blnCompilation"></param>
        /// <returns></returns>
        private string GoogleKensaku(string strAlbum, string strArtist, string strAlbumArtist, Boolean blnCompilation)
        {
            string strtmpsch = string.Empty;

            if (!chkComp.Checked && blnCompilation)
            {
                strtmpsch = strAlbum;
            }
            else 
            if (strAlbumArtist != "")
            {
                strtmpsch = strAlbum + " " + strAlbumArtist;
            }
            else
            {
                strtmpsch = strAlbum + " " + strArtist;
            }
            return strtmpsch;
        }

        /// <summary>
        /// DISCって付いてる文字列を消す
        /// </summary>
        /// <param name="strAlbum"></param>
        /// <returns></returns>
        private string DelDisc(string strAlbum)
        {
            string strDiscName = "Disc";
            string strtmpAlbum = strAlbum;

            if (strAlbum.IndexOf(strDiscName) >= 0 || strAlbum.IndexOf(strDiscName.ToLower()) >= 0 || strAlbum.IndexOf(strDiscName.ToUpper()) >= 0)
            {
                for (int i = 1; i <= 10; i++)
                {
                    strtmpAlbum = strAlbum.Replace("[" + strDiscName + " " + i.ToString() + "]", "");
                    strtmpAlbum = strtmpAlbum.Replace("[" + strDiscName.ToLower() + " " + i.ToString() + "]", "");
                    strtmpAlbum = strtmpAlbum.Replace("[" + strDiscName.ToUpper() + " " + i.ToString() + "]", "");
                    strtmpAlbum = strtmpAlbum.Replace("[" + strDiscName + i.ToString() + "]", "");
                    strtmpAlbum = strtmpAlbum.Replace("[" + strDiscName.ToLower() + i.ToString() + "]", "");
                    strtmpAlbum = strtmpAlbum.Replace("[" + strDiscName.ToUpper() + i.ToString() + "]", "");
                    if (strAlbum != strtmpAlbum)
                        break;
                }
            }

            return strtmpAlbum;
        }
        /// <summary>
        /// @@@アートワークをセット@@@
        /// </summary>
        /// <param name="IITTrack">アートワークを設定するオブジェクト</param>
        /// <param name="strPath">設定する画像のパス</param>
        /// <param name="iComIndex">COMVALの添え字（IITTrackと同じ位置にする）</param>
        /// <returns>Success -> True Failure -> False</returns>
        private Boolean SetArtworkMusic(ref IITTrack TTR, string strPath, int iComIndex = -1)
    	{
            IITTrackCollection trackCol = null;

		    try{
			    trackCol = itunesD.LibraryPlaylist.Tracks;
                TTR = trackCol.get_ItemByPersistentID(COMVAL.iPersistentIDHigh[iComIndex], COMVAL.iPersistentIDRow[iComIndex]);
			    if (chkArtworkDel.Checked && TTR != null)
                    DeleteArtwork(TTR.Artwork);
                if (!AddArtworkFromFile(ref TTR, strPath))
                    return false;
                if(TTR != null)
                    COMVAL.iArtCnt[iComIndex] = TTR.Artwork.Count;
                return true;
		    }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 8451 : " + ex.Message + "\n";
                return false;
            }
            finally
            {
			    if (trackCol != null)
				Marshal.ReleaseComObject(trackCol);
		    }
	    }


        private void rdoBmp_CheckedChanged(object sender, EventArgs e)
        {
            CONF.pSavePicture = "bmp";
        }

        private void rdoJpg_CheckedChanged(object sender, EventArgs e)
        {
            CONF.pSavePicture = "jpg";
        }

        private void rdoPng_CheckedChanged(object sender, EventArgs e)
        {
            CONF.pSavePicture = "png";

        }

        private void btnChoufuku_Click(object sender, EventArgs e)
        {
            iTunesApp itunes = null;
            IITTrackCollection trackCol = null;
            IITUserPlaylist playList = null;
            IITTrack track = null;
            object target = null;
            int icnt = 0;
            int foundIndex = 0;
            int FristfoundIndex = 0;
            int iChoufuku = 0;
            Boolean blnflg = false;
            string[] strDoubleName = new string[1];
            string strget = string.Empty;
            PlayListName GetName = null;

            DialogResult YorN = DialogResult.No;

            if (COMVAL.strName == null)
            {
                Msgbox("設定にて、iTunesの情報を取得して下さい。", HeadMsg.ATTENTION, 5);
                return;
            }

            YorN = Msgbox("重複曲を検索し、プレイリストを作成します。よろしいですか？"
                                      + "\n" + "※iTunesと同期直後でない場合、正しい結果が得られない場合があります。", "検索",
                                    3);
            if (YorN == DialogResult.No)
                return;

            try{

                itunes = new iTunesAppClass();
                trackCol = itunes.LibraryPlaylist.Tracks;

                GetName = new PlayListName();
                GetName.CColor = COMVAL.strColor;
                GetName.SelectEvent = 1;
                DialogSaizenmen();
                GetName.ShowDialog();

                if (!GetName.Ret)
                    return;
                strget = GetName.getMassage;

                playList = itunes.CreatePlaylist(strget + "(重複)") as IITUserPlaylist;
           
                splash.SendDataALL(itunes.LibraryPlaylist.Tracks.Count);
                Message = "重複曲を検索中...";
                DialogSaizenmen();
                splash.SendVisible(true);
                backgroundWorker1.RunWorkerAsync();

                foreach(var strName in COMVAL.strName) {

                    foundIndex = icnt;              

                    if (Array.IndexOf(strDoubleName, strName) == -1)
                    {                    
                        while (foundIndex != -1)
                        {                            
                            FristfoundIndex = Array.IndexOf(COMVAL.strName, strName, foundIndex);
                            foundIndex = Array.IndexOf(COMVAL.strName, strName, foundIndex+1);                        

                            if (FristfoundIndex != -1 && foundIndex != -1 &&
                                COMVAL.strArtist[FristfoundIndex] == COMVAL.strArtist[foundIndex] &&
                                blnflg == false)
                            {
                                AddPlayList(ref track, ref target, ref playList, ref trackCol, FristfoundIndex);
                                blnflg = true;
                            }
                            if (foundIndex != -1 && COMVAL.strArtist[FristfoundIndex] == COMVAL.strArtist[foundIndex])
                            {
                                AddPlayList(ref track, ref target, ref playList, ref trackCol, foundIndex);
                            }
                            else
                            {
                                break;
                            }
                        }
                        Array.Resize(ref strDoubleName, iChoufuku + 1);
                        strDoubleName[iChoufuku] = strName;
                        iChoufuku++;
                    }
                    if (splash.Cancel)
                    {
                        Msgbox("処理を中断します。", HeadMsg.NOTICE, 7);
                        splash.SendCacncel = false;
                        splash.SendCan(false);
                        break;
                    }
                    blnflg = false;
                    splash.SendData(icnt);
                    icnt++;
                }
                splash.CloseSplash();
                CheckSaizenmen();
                Msgbox("重複曲一覧のプレイリストを作成しました。", HeadMsg.COMPLETE, 1);

            }
            catch (System.Exception ex)
            {
                splash.CloseSplash();
                Msgbox("System Exception 1132" + "\n" + "ITunEs TooLを終了します。", HeadMsg.HEAD_EXCEPTION, 4);
                pErrMessage += "System Exception 1132 : " + ex.Message + "\n";
            }
            finally
            {
                if (track != null)
                    Marshal.ReleaseComObject(track);
                if (playList != null)
                    Marshal.ReleaseComObject(playList);
                if (trackCol != null)
                    Marshal.ReleaseComObject(trackCol);
                CheckSaizenmen();                

            }
            
        }

        /// <summary>
        /// 画像自動設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoSet_Click(object sender, EventArgs e)
        {
            int icnt = 0;
            int iArtCnt = 0;
            int idownload = 0;
            string strPic = string.Empty;
            string strPath = string.Empty;
            string strArtworkName = string.Empty;
            string strExe = string.Empty;
            string strtmpAlbum = string.Empty;
            string strtmpsch = string.Empty;
            
            Boolean blnCancel = false;
            DialogResult YorN = DialogResult.No;
            
            iTunesApp itunes = null;
            IITTrack track = null;

            ITunEsTooL_Beams IBeans = new ITunEsTooL_Beams();
            PicCheck frmPicChk = new PicCheck();
            
            byte[] imgChkData = null;

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                Msgbox("インターネットに接続されていません。", HeadMsg.NOTICE, 5);
                return;
            }
            if (COMVAL.strLocation == null)
            {
                Msgbox("設定にて、iTunesの情報を取得して下さい。", HeadMsg.ATTENTION, 5);
                return;
            }
            if (!Directory.Exists(CONF.pArtWork))
            {
                Msgbox("画像の保存場所が存在しません。設定タブにて保存場所の設定を行ってください。", HeadMsg.ATTENTION, 2);
                tabControl1.SelectedTab = tabControl1.TabPages["tabSetting"];
                return;
            }
            icnt = Array.IndexOf(COMVAL.iArtCnt, 0, icnt);
            if (icnt == -1)
            {
                Msgbox("画像が設定されてない曲はありません。", HeadMsg.COMPLETE, 1);
                return;
            }
            YorN = Msgbox("アートワークが設定されていない曲の自動設定を行います。"
                                      + "\n" 
                                      + "項目が多い場合は時間が掛かる場合があります。"
                                       + "\n" 
                                      + "※iTunesと同期直後でない場合、正しい結果が得られない場合があります。", "自動設定",
                                        3);
            if (YorN == DialogResult.No)
                return;
            tabControl1.SelectedTab = tabControl1.TabPages["tabProperty"]; 
            try{
                itunes = new iTunesAppClass();
            
                splash.SendDataALL(itunes.LibraryPlaylist.Tracks.Count);
                Message = "画像をダウンロードしています...";
                DialogSaizenmen();
                splash.SendVisible(true);
                backgroundWorker1.RunWorkerAsync();

                for (iArtCnt = 0; iArtCnt < COMVAL.iArtCnt.Length; iArtCnt++)
                {
                    if (COMVAL.iArtCnt[iArtCnt] == 0 && COMVAL.strAlbum[iArtCnt] != "")
                    {
                        strtmpAlbum = DelDisc(COMVAL.strAlbum[iArtCnt]);                    
                        strtmpsch = GoogleKensaku(strtmpAlbum, COMVAL.strArtist[iArtCnt], COMVAL.strAlbumArtist[iArtCnt], COMVAL.blnCompilation[iArtCnt]);
                        //results = GoogleSerch(strtmpsch);
                        var iTunesSearchApiModel = GetItunesSearchApiObject(strtmpsch);
                        if (iTunesSearchApiModel == null)
                            continue;

                        foreach (var result in iTunesSearchApiModel.results)
                        {
                            strArtworkName = SetArtworkName(COMVAL.strName[iArtCnt], COMVAL.strAlbum[iArtCnt], COMVAL.strArtist[iArtCnt]);
                            strPath = RepeatedFile(txtArtWork.Text + @"\" + FileOpe.ValidFileName(strArtworkName));

                            idownload = DownloadData(result.artworkUrl30.Replace("30x30bb", "500x500bb"), ref imgChkData);
                            if (idownload >= 2) { continue; }

                            if (PicCheckFlag)
                            {
                                splash.SendVisible(false);
                                frmPicChk.Artist = COMVAL.strArtist[iArtCnt];
                                frmPicChk.Album = COMVAL.strAlbum[iArtCnt];
                                frmPicChk.MusicName = COMVAL.strName[iArtCnt];
                                frmPicChk.Url = strPath;
                                frmPicChk.SetEvent = 1;
                                frmPicChk.SetColor = pictureBox1.BackColor;
                                frmPicChk.Img = imgChkData;
                                DialogSaizenmen();
                                frmPicChk.ShowDialog();
                                if (frmPicChk.Jikai)
                                    PicCheckFlag = false;
                                
                                if (frmPicChk.Jump)
                                {
                                    splash.SendVisible(true);
                                    SetMinus(ref COMVAL, iArtCnt);
                                    break;
                                }

                                if (!frmPicChk.Ret)
                                {
                                    splash.SendVisible(true);
                                    if (splash.Cancel)
                                    {
                                        Msgbox("処理を中断します。", HeadMsg.NOTICE, 7);
                                        blnCancel = true;
                                        splash.SendCacncel = false;
                                        splash.SendCan(false);
                                        break;
                                    }
                                    continue;
                                }
                            }

                            if (!SaveImg(imgChkData, strPath)) {
                                Msgbox("画像が設定出来ませんでした。再検索を行います。", HeadMsg.ATTENTION, 5);
                                continue; 
                            }

                            splash.SendMessage("画像を自動設定しています...");
                            splash.SendVisible(true);

                            for (icnt = 0; icnt < COMVAL.strName.Length; icnt++)
                            {
                                icnt = Array.IndexOf(COMVAL.strAlbum, COMVAL.strAlbum[iArtCnt], icnt);
                                if (icnt == -1)
                                    break;
                                if (IBeans.MachiAlbum(COMVAL,COMVAL.strAlbum[iArtCnt], COMVAL.strArtist[iArtCnt],
                                                                    COMVAL.strAlbumArtist[iArtCnt], COMVAL.blnCompilation[iArtCnt], ref icnt))
                                {
                                     if (SetArtworkMusic(ref track, strPath, icnt))
                                     {
	                                        splash.SendData(iArtCnt);	                                
	                                        if (splash.Cancel)
	                                        {
	                                            Msgbox("処理を中断します。", HeadMsg.NOTICE, 7);
	                                            blnCancel = true;
	                                            splash.SendCacncel = false;
	                                            splash.SendCan(false);
	                                            break;                                                                            
	                                        }
                                      }
                                     COMVAL.iArtCnt[icnt] = track.Artwork.Count;
                                     SetArtWork(track);
                                }
                            }
                            break;
                        }
                    }
               
                    splash.SendData(iArtCnt);
                    if (!blnCancel)
                    {
                        if (splash.Cancel)
                        {
                            Msgbox("処理を中断します。", HeadMsg.NOTICE, 7);
                            blnCancel = true;
                            splash.SendCacncel = false;
                            splash.SendCan(false);
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                SetZero(ref COMVAL);

                splash.CloseSplash();
                if (chkAutoShutdown.Checked && !blnCancel)
                {
                    pAutoShutdown = true;
                    this.Close();   
                }
                else
                {
                    Msgbox("自動設定が完了しました。" + "\n" + "※自動設定されない画像がある場合は、個別に自動設定を行って下さい。", HeadMsg.COMPLETE, 1);   
                }
                SetArtWork(TR);
            
            }
            catch (System.Exception ex)
            {
                splash.CloseSplash();
                Msgbox("System Exception 1152" + "\n" + "ITunEs TooLを終了します。", HeadMsg.HEAD_EXCEPTION, 4);
                pErrMessage += "System Exception 1152 : " + ex.Message + "\n";
                this.Close();
            }
            finally
            {
                if (track != null) { Marshal.ReleaseComObject(itunes); }
                if (itunes != null){ Marshal.ReleaseComObject(itunes); }
                
                CheckSaizenmen();
            }
        }

        /// <summary>
        /// -1を入れたところを0に戻す。
        /// </summary>
        /// <param name="COMVAL"></param>
        private void SetZero(ref CommonValue COMVAL)
        {
            int icnt = 0;

            for (icnt = 0; icnt < COMVAL.strName.Length; icnt++)
            {
                icnt = Array.IndexOf(COMVAL.iArtCnt, -1, icnt);
                if (icnt == -1)
                    break;
                COMVAL.iArtCnt[icnt] = 0;
            }
        }

        /// <summary>
        /// 再検索しないよう、-1を入れる。
        /// </summary>
        /// <param name="COMVAL"></param>
        /// <param name="iArtCnt"></param>
        private void SetMinus(ref CommonValue COMVAL,int iArtCnt)
        {
            int icnt = 0;
            ITunEsTooL_Beams IBeans = new ITunEsTooL_Beams();

            for (icnt = 0; icnt < COMVAL.strName.Length; icnt++)
            {
                icnt = Array.IndexOf(COMVAL.strAlbum, COMVAL.strAlbum[iArtCnt], icnt);
                if (icnt == -1)
                    break;
                if (IBeans.MachiAlbum(COMVAL, COMVAL.strAlbum[iArtCnt], COMVAL.strArtist[iArtCnt],
                                                    COMVAL.strAlbumArtist[iArtCnt], COMVAL.blnCompilation[iArtCnt], ref icnt))
                {
                    if (COMVAL.iArtCnt[icnt] == 0)
                        COMVAL.iArtCnt[icnt] = -1;
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int icnt = 0;
            IITTrack track = null;
            iTunesApp itunes = null;
            string strAlbum = string.Empty;
            string strArtist = string.Empty;

            itunes = new iTunesAppClass();

            DialogResult YorN = DialogResult.No;

            if (COMVAL.strLocation == null)
            {
                Msgbox("設定にて、iTunesの情報を取得して下さい。", HeadMsg.ATTENTION, 5);
                return;
            }
            YorN = Msgbox("ITunEsTooLに設定されている曲の画像を削除します。よろしいですか"
                                    + "\n" + "※iTunesと同期直後でない場合、正しい結果が得られない場合があります。"
                                    + "\n" + "また、画像のバックアップしておくことをお勧めします。",
                                    HeadMsg.ATTENTION, 3);

            if (YorN == DialogResult.No)
                return;

            splash.SendDataALL(itunes.LibraryPlaylist.Tracks.Count);
            Message = "画像を削除しています...";
            DialogSaizenmen();
            splash.SendVisible(true);

            backgroundWorker1.RunWorkerAsync();

            for (icnt = 0; icnt < COMVAL.strName.Length; icnt++)
            {
                track = itunes.LibraryPlaylist.Tracks.get_ItemByPersistentID(COMVAL.iPersistentIDHigh[icnt], COMVAL.iPersistentIDRow[icnt]);
                DeleteArtwork(track.Artwork);
                COMVAL.iArtCnt[icnt] = track.Artwork.Count;
                splash.SendData(icnt);
            }
            splash.CloseSplash();
            Msgbox("画像の削除が完了しました。", HeadMsg.COMPLETE, 1);
            if (itunes != null)
                Marshal.ReleaseComObject(itunes);
            if (track != null)
                Marshal.ReleaseComObject(track);
        }

        /// <summary>
        /// ヘルプ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_KeyDown(object sender, KeyEventArgs e)
        {
            try{
                //画像貼り付け
                if (e.KeyData == (Keys.Control | Keys.V) && tabControl1.SelectedTab == tabControl1.TabPages["tabProperty"])
                {
                    if (!lblName.ReadOnly)
                    {
                        if(!lblName.Focused && !lblAlbum.Focused && !lblArtist.Focused)
                            Msgbox("編集モードでは、Ctrl + Vで貼り付けは行えません。", HeadMsg.ATTENTION, 2);
                        return;
                    }
                    tsetToolStripMenuItem_Click(null, null);
                }

                if ((e.KeyData) == Keys.F1)
                {
                    if (tabControl1.SelectedTab == tabControl1.TabPages["tabSerch"])
                    {
                        if (NetworkInterface.GetIsNetworkAvailable())
                            System.Diagnostics.Process.Start(Domain + "/info/Backup.html");
                    }
                    else if (tabControl1.SelectedTab == tabControl1.TabPages["tabProperty"])
                    {
                        if (NetworkInterface.GetIsNetworkAvailable())
                            System.Diagnostics.Process.Start(Domain + "/info/Artwork.html");
                    }
                    else if (tabControl1.SelectedTab == tabControl1.TabPages["tabAutoSetArt"])
                    {
                        if (NetworkInterface.GetIsNetworkAvailable())
                            System.Diagnostics.Process.Start(Domain + "/info/Artwork.html");
                    }
                    else if (tabControl1.SelectedTab == tabControl1.TabPages["tabCopy"]) 
                    {
                        if (NetworkInterface.GetIsNetworkAvailable())
                            System.Diagnostics.Process.Start(Domain + "/info/mainte.html");
                    }
                    else if (tabControl1.SelectedTab == tabControl1.TabPages["tabSetting"])
                    {
                        if (NetworkInterface.GetIsNetworkAvailable())
                            System.Diagnostics.Process.Start(Domain + "/info/Settei.html");
                    }               
                }
            }
            catch (SystemException ex)
            {
                pErrMessage += "System Exception 7643 : " + ex.Message + "\n";
            }
        }

        /// <summary>
        /// 画像のデータをダウンロード
        /// </summary>
        /// <param name="DownloadURL"></param>
        /// <param name="Filename"></param>
        /// <param name="chkimgdata"></param>
        /// <returns></returns>
        private int DownloadData(string DownloadURL, ref byte[] chkimgdata)
        {
            WebClient wc = null;
            MemoryStream st = null;
            Image IMG = null;
            byte[] pic = null;

            try
            {                 
                wc = new WebClient();
                pic = wc.DownloadData(DownloadURL);

                if (pic.Length > 10000)
                {
                    using (st = new MemoryStream(pic))
                        try
                        {
                            using (Image imgSrc = Image.FromStream(st))
                            {
                                IMG = new Bitmap(imgSrc);
                            }
                        }
                        catch (ArgumentException)
                        {
                            return 2;
                        }
                    if (chkSmalGazou.Checked)
                    {
                        if (IMG.Height < numUDSize.Value || IMG.Width < numUDSize.Value)
                            return 2;
                    }
                    chkimgdata = pic;
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            catch (WebException we)
            {
		        pErrMessage += "Web Exception 2102 : " + we.Message + "\n";
		        return 2;
            }
            catch (SystemException ex)
            {
		        pErrMessage += "System Exception 2102 : " + ex.Message + "\n";
		        return 3;
		    }
            finally
            {
                if (st != null) { st.Close(); }
                if (IMG != null) { IMG.Dispose(); }
                if (wc != null) { wc.Dispose(); }
		    }
	    }        

        /// <summary>
        /// クリップボードの画像を設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IITTrackCollection trackCol = null;
            PictureOpen frmPicture = new PictureOpen();

            string strArtworkName = string.Empty;
            string strName = string.Empty;
            string strAlbum = string.Empty;
            string strAlbumArtist = string.Empty;
            string strArtist = string.Empty;
            Boolean blnCompilation = false;
            string strPath = string.Empty;
            int icnt;
            int imax = 0;
            int imusic = 0;
            DialogResult YorN = DialogResult.No;
            IITFileOrCDTrack FileOrCDTrack = null;
            COMVAL.DIALOG_TITLE = HeaderName.ICHIRAN;
            ITunEsTooL_Beams IBeans = new ITunEsTooL_Beams();
            IITTrack track = null;
            Boolean blnNotAlbum = false;

            if (!Clipboard.ContainsImage())
            {
                Msgbox("クリップボードに画像が設定されていません。", HeadMsg.NOTICE, 2);
                return;
            }

            if (COMVAL.strAlbum == null)
            {
                Msgbox("設定にて、iTunesの情報を取得して下さい。", HeadMsg.NOTICE, 2);
                return;
            }
            if (TR == null)
            {
                Msgbox("iTunesから曲をドラッグしてください。", HeadMsg.NOTICE, 2);
                return;
            }
            if (TR.Album == null)
            {
                blnNotAlbum = true;
            }
            if (!Directory.Exists(CONF.pArtWork))
            {
                Msgbox("画像の保存場所が存在しません。設定タブにて保存場所の設定を行ってください。", HeadMsg.ATTENTION, 2);
                tabControl1.SelectedTab = tabControl1.TabPages["tabSetting"];
                return;
            }
            try
            {
                frmPicture.CColor = COMVAL.strColor;
                DialogSaizenmen();
                frmPicture.ShowDialog();
                
                if (!frmPicture.Ret)
                    return;
                if (TR.Name != null)
                    strName = TR.Name;
                if (TR.Album != null)
                    strAlbum = TR.Album;
                if (TR.Artist != null)
                    strArtist = TR.Artist;
                blnCompilation = TR.Compilation;
                FileOrCDTrack = (IITFileOrCDTrack)TR;
                if (FileOrCDTrack.AlbumArtist != null)
                    strAlbumArtist = FileOrCDTrack.AlbumArtist;

                if (Clipboard.ContainsImage())
                {
                    Bitmap img = (Bitmap)Clipboard.GetImage();
                    ImageFormat fmt = ImageFormat.Bmp;
                    if (rdoBmp.Checked) fmt = ImageFormat.Bmp;
                    if (rdoJpg.Checked) fmt = ImageFormat.Jpeg;
                    if (rdoPng.Checked) fmt = ImageFormat.Png;
                    strArtworkName = SetArtworkName(strName, strAlbum, strArtist);
                    strPath = RepeatedFile(txtArtWork.Text + @"\" + FileOpe.ValidFileName(strArtworkName));
                    img.Save(strPath, fmt);

                }

                imax = IBeans.GetAlbumCount(strAlbum, strArtist,strAlbumArtist, blnCompilation, ref COMVAL);

                splash.SendDataALL(imax);
                Message = "画像を設定しています...";
                DialogSaizenmen();
                splash.SendVisible(true);

                trackCol = itunesD.LibraryPlaylist.Tracks;

                splash.SendMessage("アートワークを設定しています。");
                splash.SendVisible(true);
                if (!blnNotAlbum){
                    YorN = Msgbox("クリップボードの画像を【 " + strAlbum + " 】のすべての曲に設定しますか？", HeadMsg.NOTICE, 3);
                }
                else
                {
                    YorN = Msgbox("クリップボードの画像を設定してもよろしいですか？", HeadMsg.NOTICE, 3);
                    if (YorN == DialogResult.No)
                        return;

                    YorN = DialogResult.No;
                }
                   
                if (YorN == DialogResult.No)
                    splash.SendDataALL(1);

                backgroundWorker1.RunWorkerAsync();
                if (YorN == DialogResult.No)
                {
                    icnt = IBeans.GetMusicMachiIndex(ref COMVAL, itunesD.get_ITObjectPersistentIDHigh(TR), itunesD.get_ITObjectPersistentIDLow(TR));
                    if (!SetArtworkMusic(ref track, strPath, icnt))
                    {
                        splash.CloseSplash();
                        Msgbox("【 " + COMVAL.strName[icnt] + " 】に画像を設定することが出来ませんでした。",
                                                HeadMsg.ATTENTION, 5);
                        return;
                    }
                    
                    imusic++;
                }
                else
                {

                    for (icnt = 0; icnt < COMVAL.strName.Length; icnt++)
                    {
                        icnt = Array.IndexOf(COMVAL.strAlbum, strAlbum, icnt);
                        if (icnt == -1)
                            break;

                        if (IBeans.MachiAlbum(COMVAL, strAlbum, strArtist, strAlbumArtist, blnCompilation, ref icnt))
                        {
                            if (!SetArtworkMusic(ref track, strPath, icnt))
                            {
                                splash.CloseSplash();
                                Msgbox("【 " + COMVAL.strName[icnt] + " 】に画像を設定することが出来ませんでした。",
                                                        HeadMsg.ATTENTION, 5);
                                return;
                            }
                            SetArtWork(track);
                            imusic++;
                            splash.SendData(imusic);
                        }
                        if (splash.Cancel)
                        {
                            Msgbox("処理を中断します。", HeadMsg.NOTICE, 7);
                            splash.SendCacncel = false;
                            splash.SendCan(false);
                            break;
                        }
                    }
                }

                splash.CloseSplash();
                Msgbox("画像設定が完了しました。", HeadMsg.COMPLETE, 1);
                SetArtWork(TR);

            }
            catch (System.Exception ex)
            {
                splash.CloseSplash();
                Msgbox("System Exception 1088" + "\n" + "ITunEs TooLを終了します。", HeadMsg.HEAD_EXCEPTION, 4);
                pErrMessage += "System Exception 1088 : " + ex.Message + "\n";
            }
            finally
            {
                CheckSaizenmen();
                this.Enabled = true;
                lblTitle.Text = HeaderName.TITLE;

            }
        }

        /// <summary>
        /// 画像削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void この画像を削除するToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int icnt = 0;
            int iDelcnt = 0;
            IITTrack track = null;
            iTunesApp itunes = null;
            string strAlbum = string.Empty;
            string strArtist = string.Empty;
            string strAlbumArtist = string.Empty;
            Boolean blnCompilation = false;
            ITunEsTooL_Beams IBeans = new ITunEsTooL_Beams();
            IITFileOrCDTrack FileOrCDTrack = null;

            itunes = new iTunesAppClass();
            DialogResult YorN = DialogResult.No;
            try
            {
                if (COMVAL.strLocation == null)
                {
                    Msgbox("設定にて、iTunesの情報を取得して下さい。", HeadMsg.ATTENTION, 5);
                    return;
                }
                if (TR == null)
                {
                    Msgbox("iTunesから曲をドラッグしてください。", HeadMsg.NOTICE, 2);
                    return;
                }
                if (TR.Artwork.Count == 0)
                {
                    Msgbox("削除する画像がありません。", HeadMsg.NOTICE, 2);
                    return;
                }
               
                if (Msgbox("現在設定されている画像が削除されます。" + "\n"  + "画像のバックアップを取りますか？", HeadMsg.ATTENTION, 3)  == DialogResult.Yes)
                {
                    PicBackup(TR);
                }

                if (TR.Album != null)
                    strAlbum = TR.Album;
                if (TR.Artist != null)
                    strArtist = TR.Artist;
                blnCompilation = TR.Compilation;
                FileOrCDTrack = (IITFileOrCDTrack)TR;
                if (FileOrCDTrack.AlbumArtist != null)
                    strAlbumArtist = FileOrCDTrack.AlbumArtist;

                YorN = Msgbox("【" + strAlbum + "】の画像を削除します。よろしいですか"
                                        + "\n" + "※iTunesと同期直後でない場合、正しい結果が得られない場合があります。",                                        
                                        HeadMsg.ATTENTION, 3);

                if (YorN == DialogResult.No)
                    return;

                splash.SendDataALL(IBeans.GetAlbumCount(strAlbum, strArtist,strAlbumArtist, blnCompilation, ref COMVAL));
                Message = "画像を削除しています...";
                DialogSaizenmen();
                splash.SendVisible(true);
                backgroundWorker1.RunWorkerAsync();

                for (icnt = 0; icnt < COMVAL.strName.Length; icnt++)
                {
                    icnt = Array.IndexOf(COMVAL.strAlbum, strAlbum, icnt);
                    if (icnt == -1)
                        break;                  
                    if (IBeans.MachiAlbum(COMVAL, strAlbum, strArtist, strAlbumArtist, blnCompilation, ref  icnt))
                    {
                        track = itunes.LibraryPlaylist.Tracks.get_ItemByPersistentID(COMVAL.iPersistentIDHigh[icnt], COMVAL.iPersistentIDRow[icnt]);
                        DeleteArtwork(track.Artwork);
                        COMVAL.iArtCnt[icnt] = track.Artwork.Count;
                        iDelcnt++;
                        splash.SendData(iDelcnt);
                    }
                    if (splash.Cancel)
                    {
                        Msgbox("処理を中断します。", HeadMsg.NOTICE, 7);
                        splash.SendCacncel = false;
                        splash.SendCan(false);
                        break;
                    }

                }
                splash.CloseSplash();
                Msgbox("画像の削除が完了しました。", HeadMsg.COMPLETE, 1);
                SetArtWork(TR);
            }
            catch (System.Exception ex)
            {
                splash.CloseSplash();
                Msgbox("System Exception 1355" + "\n" + "ITunEs TooLを終了します。", HeadMsg.HEAD_EXCEPTION,4);
                pErrMessage += "System Exception 1355 : " + ex.Message + "\n";
                this.Close();
            }
            finally
            {
                if (itunes != null)
                    Marshal.ReleaseComObject(itunes);
                if (track != null)
                    Marshal.ReleaseComObject(track);
                CheckSaizenmen();
            }
        }

        private void この画像のバックアップをとるToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            try{
                if (TR == null)
                {
                    Msgbox("iTunesから曲をドラッグしてください。", HeadMsg.NOTICE, 2);
                    return;
                }
                if (TR.Artwork.Count == 0)
                {
                    Msgbox("バックアップする画像がありません。", HeadMsg.NOTICE, 2);
                    return;
                }
                if (PicBackup(TR) != 0)
                    return;
                
                Msgbox("バックアップが完了しました。", HeadMsg.COMPLETE, 1);
                
                
            }
            catch (System.Exception ex)
            {
                Msgbox("System Exception 3325" + "\n" + "ITunEs TooLを終了します。", HeadMsg.HEAD_EXCEPTION,4);
                pErrMessage += "System Exception 3325 : " + ex.Message + "\n";
                this.Close();
            }
        }

        /// <summary>
        /// バックアップさぶるーちん
        /// </summary>
        /// <param name="TR_"></param>
        /// <param name="Path"></param>
        /// <returns>0　→　正常 </returns>
        /// <returns>1　→　キャンセル</returns>
        /// <returns>2　→　異常</returns>
        public int PicBackup(IITTrack TR_ )
        {
            try{
                Dialog clsDialog = null;
                string strPath = string.Empty;
                clsDialog = new Dialog();
                strPath = clsDialog.OpenFolderDialog(this, "画像をバックアップするフォルダを指定して下さい。");
                if (strPath == "")
                    return 1;
                MakeArtWork(TR_, strPath);
                return 0;
            }
            catch (System.Exception ex)
            {
                pErrMessage += "System Exception 6325 : " + ex.Message + "\n";
                return 2;
            }
        }
        /// <summary>
        /// メッセージ
        /// </summary>
        /// <param name="Msg"></param>
        /// <param name="HeadMsg"></param>
        /// <param name="MsgType">１→Information,2→Warning,3→Question,4→Error,5→Exclamation,→Question</param>      
        /// <returns></returns>
        private DialogResult Msgbox(string Msg,string HeadMsg, int MsgType)
        {
            DialogResult YorN = DialogResult.No;

            DialogSaizenmen();
            switch (MsgType)
            {
                case 1:
                    MessageBox.Show(Msg, HeadMsg, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 2:
                    MessageBox.Show(Msg, HeadMsg, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 3:
                    YorN = MessageBox.Show(Msg, HeadMsg, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    break;
                case 4:
                    MessageBox.Show(Msg, HeadMsg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case 5:
                    MessageBox.Show(Msg, HeadMsg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                case 6:
                    MessageBox.Show(Msg, HeadMsg, MessageBoxButtons.OK, MessageBoxIcon.Question);
                    break;
                case 7:
                    MessageBox.Show(Msg,HeadMsg, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    break;
                case 8:
                    YorN = MessageBox.Show(Msg, HeadMsg, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    break;
            }
            CheckSaizenmen();
            return YorN;
        }
       
        private void 再生中の曲をITunEsTooLに設定するToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (COMVAL.strAlbum == null)
            {
                Msgbox("設定にて、iTunesの情報を取得して下さい。", HeadMsg.ATTENTION, 2);
                return;
            }
            if (itunesD.CurrentTrack == null)
            {
                Msgbox("現在、再生中の曲がありません。", HeadMsg.NOTICE, 2);
                return;
            }
            else
            {
                TR = itunesD.CurrentTrack;
                SetArtWork(TR);
            }
        }     

        private void txtErrLog_DoubleClick(object sender, EventArgs e)
        {
            if (Directory.Exists(txtErrLog.Text))
                System.Diagnostics.Process.Start(txtErrLog.Text);
        }

        private void txtArtWork_DoubleClick(object sender, EventArgs e)
        {
            if (Directory.Exists(txtArtWork.Text))
                System.Diagnostics.Process.Start(txtArtWork.Text);
        }

        private void txtxmlPath_DoubleClick(object sender, EventArgs e)
        {
            if (File.Exists(txtxmlPath.Text))
                System.Diagnostics.Process.Start(txtxmlPath.Text);
        }

        private void 画像ファイルを設定するToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strName = string.Empty;
            string strArtist = string.Empty;
            string strAlbum = string.Empty;
            string strAlbumArtist = string.Empty;
            string strPath = string.Empty;
            string strExt = string.Empty;
            int icnt = 0;
            int imax = 0;
            int imusic = 0;
            DialogResult YorN = DialogResult.No;
            IITTrackCollection trackCol = null;
            IITFileOrCDTrack FileOrCDTrack = null;
            Boolean blnCompilation = false;
            ITunEsTooL_Beams IBeans = new ITunEsTooL_Beams();
            IITTrack track = null;
            Boolean blnNotAlbum = false;
            string strPath1;
            Dialog clsDialog;

            if (TR == null)
            {
                Msgbox("iTunesから曲をドラッグしてください。", HeadMsg.NOTICE, 2);
                return;
            }
            if (TR.Album == null)
            {
                blnNotAlbum = true;
            }
            if (chkArtworkDel.Checked && TR.Artwork.Count != 0)
            {
                if (Msgbox("現在設定されている画像が削除されます。" + "\n" + "画像のバックアップを取りますか？", HeadMsg.ATTENTION, 3) == DialogResult.Yes)
                {
                    PicBackup(TR);
                }
            }
            clsDialog = new Dialog();

            strPath1 = clsDialog.OpenFileDialog("アルバムに設定する画像ファイルを選択して下さい。", System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            if (strPath1 == "")
            {
                return;
            }

            strExt = Path.GetExtension(strPath1);
            if (strExt == ".jpg" || strExt == ".png" || strExt == ".bng")
            {
                strPath = strPath1;
            }
            else
            {
                Msgbox("画像ファイルをドラッグしてください。", HeadMsg.NOTICE, 2);
                return;
            }            

            try
            {
                if (TR.Name != null)
                    strName = TR.Name;
                if (TR.Album != null)
                    strAlbum = TR.Album;
                if (TR.Artist != null)
                    strArtist = TR.Artist;
                blnCompilation = TR.Compilation;
                FileOrCDTrack = (IITFileOrCDTrack)TR;
                if (FileOrCDTrack.AlbumArtist != null)
                    strAlbumArtist = FileOrCDTrack.AlbumArtist;

                imax = IBeans.GetAlbumCount(strAlbum, strArtist, strAlbumArtist, blnCompilation, ref COMVAL);
                DialogSaizenmen();
                splash.SendDataALL(imax);
                Message = "画像を設定しています...";
                
                splash.SendVisible(true);

                trackCol = itunesD.LibraryPlaylist.Tracks;

                splash.SendMessage("アートワークを設定しています。");
                splash.SendVisible(true);

                if (!blnNotAlbum)
                {
                    YorN = Msgbox("ドラッグしたの画像を【 " + strAlbum + " 】のすべての曲に設定しますか？",
                           HeadMsg.NOTICE, 3);
                }
                else
                {
                    YorN = Msgbox("ドラッグした画像を設定してもよろしいですか？", HeadMsg.NOTICE, 3);
                    if (YorN == DialogResult.No)
                        return;

                    YorN = DialogResult.No;
                }
                if (YorN == DialogResult.No)
                    splash.SendDataALL(1);
                backgroundWorker1.RunWorkerAsync();
                if (YorN == DialogResult.No)
                {
                    icnt = IBeans.GetMusicMachiIndex(ref COMVAL, itunesD.get_ITObjectPersistentIDHigh(TR), itunesD.get_ITObjectPersistentIDLow(TR));
                    if (!SetArtworkMusic(ref track, strPath, icnt))
                    {
                        splash.CloseSplash();
                        Msgbox("【 " + COMVAL.strName[icnt] + " 】に画像を設定することが出来ませんでした。",
                                                HeadMsg.ATTENTION, 5);
                        return;
                    }
                    imusic++;
                }
                else
                {

                    for (icnt = 0; icnt < COMVAL.strName.Length; icnt++)
                    {
                        icnt = Array.IndexOf(COMVAL.strAlbum, strAlbum, icnt);
                        if (icnt == -1)
                            break;

                        if (IBeans.MachiAlbum(COMVAL, strAlbum, strArtist, strAlbumArtist, blnCompilation, ref icnt))
                        {
                            if (!SetArtworkMusic(ref track, strPath, icnt))
                            {
                                splash.CloseSplash();
                                Msgbox("【 " + COMVAL.strName[icnt] + " 】に画像を設定することが出来ませんでした。",
                                                        HeadMsg.ATTENTION, 5);
                                return;
                            }
                            imusic++;
                            splash.SendData(imusic);
                        }
                        if (splash.Cancel)
                        {
                            Msgbox("処理を中断します。", HeadMsg.NOTICE, 7);
                            splash.SendCacncel = false;
                            splash.SendCan(false);
                            break;
                        }
                    }
                }
                splash.CloseSplash();
                Msgbox("画像設定が完了しました。", HeadMsg.COMPLETE, 1);
                SetArtWork(TR);
            }
            catch (System.Exception ex)
            {
                splash.CloseSplash();
                Msgbox("System Exception 1248" + "\n" + "ITunEs TooLを終了します。", HeadMsg.HEAD_EXCEPTION, 4);
                pErrMessage += "System Exception 1248 : " + ex.Message + "\n";
            }
            finally
            {
                CheckSaizenmen();
                this.Enabled = true;
                lblTitle.Text = HeaderName.TITLE;

            }

            

        }

        private void 曲アルバムアーティスト名を編集モードにするToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try{
                int index = 0;

                if (COMVAL.strAlbum == null)
                {
                    Msgbox("設定にて、iTunesの情報を取得して下さい。", HeadMsg.NOTICE, 2);
                    return;
                }
                if (TR == null)
                {
                    Msgbox("曲をITunEsTooLにドラッグして下さい。", HeadMsg.ATTENTION, 2);
                    return;
                }
                if (lblName.ReadOnly)
                {
                    lblName.ReadOnly = false;
                    lblAlbum.ReadOnly = false;
                    lblArtist.ReadOnly = false;
                    lblName.BackColor = Color.MistyRose;
                    lblAlbum.BackColor = Color.MistyRose;
                    lblArtist.BackColor = Color.MistyRose;
                    曲アルバムアーティスト名を編集モードにするToolStripMenuItem1.Text = "編集モードを終了する";
                    編集モードを取り消すToolStripMenuItem.Visible = true;
                    Msgbox("編集モードに変更します。" + "\n" + "曲、アルバム、アーティスト名を変更することが可能です。" + "\n" + "※「編集モードを終了する」を押下しない限り変更はされません。", HeadMsg.NOTICE, 1);
                }
                else
                {
                    ITunEsTooL_Beams IBeans = new ITunEsTooL_Beams();
                    if (lblName.Text == "")
                    {
                        Msgbox("曲名は削除出来ません。やり直して下さい。", HeadMsg.NOTICE, 2);
                        return;
                    }
                    if (Msgbox("編集モードを終了すると変更した曲、アルバム、アーティスト名が登録されますがよろしいですか？", HeadMsg.NOTICE, 3) == DialogResult.No)
                        return;
                    TR.Name = lblName.Text;
                    TR.Album = lblAlbum.Text;
                    TR.Artist = lblArtist.Text;

                    index = IBeans.GetMusicMachiIndex(ref COMVAL, itunesD.get_ITObjectPersistentIDHigh(TR), itunesD.get_ITObjectPersistentIDLow(TR));

                    COMVAL.strName[index] = lblName.Text;
                    COMVAL.strAlbum[index] = lblAlbum.Text;
                    COMVAL.strArtist[index] = lblArtist.Text;

                    lblName.ReadOnly = true;
                    lblAlbum.ReadOnly = true;
                    lblArtist.ReadOnly = true;
                    lblName.BackColor = Color.WhiteSmoke;
                    lblAlbum.BackColor = Color.WhiteSmoke;
                    lblArtist.BackColor = Color.WhiteSmoke;
                    Msgbox("編集モードを終了します。" + "\n" + "曲名、アルバム名、アーティスト名が変更されました。", HeadMsg.NOTICE, 1);
                    曲アルバムアーティスト名を編集モードにするToolStripMenuItem1.Text = "曲アルバムアーティスト名を編集モードにする";
                    編集モードを取り消すToolStripMenuItem.Visible = false;
                }
            }
            catch (System.Exception ex)
            {
                lblName.ReadOnly = true;
                lblAlbum.ReadOnly = true;
                lblArtist.ReadOnly = true;
                lblName.BackColor = Color.WhiteSmoke;
                lblAlbum.BackColor = Color.WhiteSmoke;
                lblArtist.BackColor = Color.WhiteSmoke;
                Msgbox("System Exception 8534" + "\n" + "ITunEs TooLを終了します。", HeadMsg.HEAD_EXCEPTION, 4);
                pErrMessage += "System Exception 8534 : " + ex.Message + "\n";
                this.Close();
            }
        }

        private void 編集モードを取り消すToolStripMenuItem_Click(object sender, EventArgs e)
        {

             lblName.Text = TR.Name;
             lblAlbum.Text = TR.Album;
             lblArtist.Text = TR.Artist;
             lblName.ReadOnly = true;
             lblAlbum.ReadOnly = true;
             lblArtist.ReadOnly = true;
             lblName.BackColor = Color.WhiteSmoke;
             lblAlbum.BackColor = Color.WhiteSmoke;
             lblArtist.BackColor = Color.WhiteSmoke;
             曲アルバムアーティスト名を編集モードにするToolStripMenuItem1.Text = "曲アルバムアーティスト名を編集モードにする";
             編集モードを取り消すToolStripMenuItem.Visible = false;
             Msgbox("取り消しが完了しました。", HeadMsg.NOTICE, 1);
        }

       

        private void アートワークToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iTunesApp itunes = null;
            IITTrack track = null;
            int icnt = 0;

            if (COMVAL.strAlbum == null)
            {
                Msgbox("設定にて、iTunesの情報を取得して下さい。", HeadMsg.ATTENTION, 2);
                return;
            }
            itunes = new iTunesAppClass();

            icnt = Array.IndexOf(COMVAL.iArtCnt, 0, icnt);
            if (icnt == -1)
            {
                Msgbox("画像が設定されてない曲はありません。", HeadMsg.COMPLETE, 1);
                return;
            }

            TR = itunes.LibraryPlaylist.Tracks.get_ItemByPersistentID(COMVAL.iPersistentIDHigh[icnt], COMVAL.iPersistentIDRow[icnt]);
            if (TR != null)
            {
                SetArtWork(TR);
            }
            else
            {
                Msgbox("最新の情報を取得出来ていない可能性があります。同期して下さい。", HeadMsg.ATTENTION, 2);
                KeikokuDouki();
            }
            if (itunes != null)
                Marshal.ReleaseComObject(itunes);
            if (track != null)
                Marshal.ReleaseComObject(track);
        }

        private void アルバム名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iTunesApp itunes = null;
            IITTrack track = null;
            int icnt = 0;

            if (COMVAL.strAlbum == null)
            {
                Msgbox("設定にて、iTunesの情報を取得して下さい。", HeadMsg.ATTENTION, 2);
                return;
            }
            itunes = new iTunesAppClass();

            icnt = Array.IndexOf(COMVAL.strAlbum, "", icnt);
            if (icnt == -1)
            {
                Msgbox("アルバム名が設定されてない曲はありません。", HeadMsg.COMPLETE, 1);
                return;
            }

            TR = itunes.LibraryPlaylist.Tracks.get_ItemByPersistentID(COMVAL.iPersistentIDHigh[icnt], COMVAL.iPersistentIDRow[icnt]);
            if (TR != null)
            {
                SetArtWork(TR);
            }
            else
            {
                Msgbox("最新の情報を取得出来ていない可能性があります。同期して下さい。", HeadMsg.ATTENTION, 2);
                KeikokuDouki();
            }
            if (itunes != null)
                Marshal.ReleaseComObject(itunes);
            if (track != null)
                Marshal.ReleaseComObject(track);
        }

        private void アーtぇイスト名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iTunesApp itunes = null;
            IITTrack track = null;
            int icnt = 0;

            if (COMVAL.strArtist == null)
            {
                Msgbox("設定にて、iTunesの情報を取得して下さい。", HeadMsg.ATTENTION, 2);
                return;
            }
            itunes = new iTunesAppClass();

            icnt = Array.IndexOf(COMVAL.strArtist, "", icnt);
            if (icnt == -1)
            {
                Msgbox("アーティスト名が設定されてない曲はありません。", HeadMsg.COMPLETE, 1);
                return;
            }

            TR = itunes.LibraryPlaylist.Tracks.get_ItemByPersistentID(COMVAL.iPersistentIDHigh[icnt], COMVAL.iPersistentIDRow[icnt]);
            if (TR != null)
            {
                SetArtWork(TR);
            }
            else
            {
                Msgbox("最新の情報を取得出来ていない可能性があります。同期して下さい。", HeadMsg.ATTENTION, 2);
                KeikokuDouki();
            }
            if (itunes != null)
                Marshal.ReleaseComObject(itunes);
            if (track != null)
                Marshal.ReleaseComObject(track);
        }

        private void 画像を拡大表示するToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (TR == null)
            {
                Msgbox("曲をITunEsTooLにドラッグして下さい。", HeadMsg.ATTENTION, 2);
                return;
            }
            if (TR.Artwork.Count == 0)
            {
                Msgbox("アートワークがありません。", HeadMsg.ATTENTION, 2);
                return;
            }

            Picture frmPicture = new Picture();
            string strErr = string.Empty;

            frmPicture.CColor = pictureBox1.BackColor;
            frmPicture.Path = CONF.ppath;
            if (TR.Artist != null)
                frmPicture.Artist = TR.Artist;
            if (TR.Album != null)
                frmPicture.Album = TR.Album;
            if (TR.Name != null)
                frmPicture.MusicName = TR.Name;
            frmPicture.Track = TR;
            DialogSaizenmen();
            frmPicture.ShowDialog();
            strErr = frmPicture.strRet;
            if (strErr != "")
            {
                pErrMessage += strErr;
            }
            CheckSaizenmen();
        }


        private void tabProperty_MouseEnter(object sender, EventArgs e)
        {
            string strChk = string.Empty;
            try
            {
                if (TR != null)
                    strChk = TR.Album;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                TR = null;
                SetArtWork(TR);
            }
            catch (System.Exception)
            {
                TR = null;
                SetArtWork(TR);
            }
        }

        /// <summary>
        /// 小さい画像の設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSmalGazou_CheckedChanged(object sender, EventArgs e)
        {
            SmalGazou();
        }
     
        /// <summary>
        /// 小さい画像の設定（チェックボックス）
        /// </summary>
        private void SmalGazou()
        {
            Decimal ihanten = 0;
            if (!chkSmalGazou.Checked)
            {
                chkSmalGazou.ForeColor = Color.DimGray;
                label40.Enabled = false;
                label41.Enabled = false;
                lblNum.Enabled = false;
                numUDSize.Enabled = false;
                ihanten = -numUDSize.Value;
                CONF.pSmallArtwork = ihanten.ToString();
            }
            else
            {
                chkSmalGazou.ForeColor = Color.Black;
                label40.Enabled = true;
                label41.Enabled = true;
                lblNum.Enabled = true;
                numUDSize.Enabled = true;
                CONF.pSmallArtwork = numUDSize.Value.ToString();
            }
        }

        private void numUDSize_ValueChanged(object sender, EventArgs e)
        {
            lblNum.Text = numUDSize.Value.ToString();
        }

        private void chkComp_CheckedChanged(object sender, EventArgs e)
        {
            ChangeCheckBoxParameter(ref chkComp, ref CONF.pComp);
        }
        
        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
                System.Diagnostics.Process.Start(Domain);
        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
                System.Diagnostics.Process.Start("http://itunestool.php.xdomain.jp/contact.html");
        }

        /// <summary>
        /// シャットダウンチェックボックスクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAutoShutdown_CheckedChanged(object sender, EventArgs e)
        {
            ChangeCheckBoxParameter(ref chkAutoShutdown, ref CONF.pAutoShutdown);
        }

        /// <summary>
        /// チェックボックスのパラメーターを変更する
        /// </summary>
        private void ChangeCheckBoxParameter(ref CheckBox CheckBox,ref string ConfParam)
        {
            ConfParam = CheckBox.Checked ? TRUE : FALSE;
            CheckBox.ForeColor = CheckBox.Checked ? Color.Black : Color.DimGray;
        }

        private void btnRelieve_Click(object sender, EventArgs e)
        {
            PicCheckFlag = true;
            btnAutoSet_Click(null, null);
            PicCheckFlag = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PicCheckFlag = true;
            button9_Click(null,null);
            PicCheckFlag = false;
        }
    }     
}
