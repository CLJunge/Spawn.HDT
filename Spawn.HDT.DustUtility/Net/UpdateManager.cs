#region Using
using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
#endregion

namespace Spawn.HDT.DustUtility.Net
{
    public static class UpdateManager
    {
        #region Constants
        public const string BaseUrl = "https://github.com/CLJunge/Spawn.HDT.DustUtility/releases";
        #endregion

        #region Static Fields
        private static Version NewVersionFormat => new Version(1, 6, 1);

        private static Regex s_versionRegex;
        private static Regex s_updateTextRegex;

        private static WebClient s_webClient;
        private static UpdateInfo s_updateInfo;
        #endregion

        #region Static Properties
        #region Info
        public static UpdateInfo Info => s_updateInfo;
        #endregion
        #endregion

        #region Custom Events
        public static event DownloadProgressChangedEventHandler DownloadProgressChanged;

        public static event DownloadDataCompletedEventHandler DownloadCompleted;
        #endregion

        #region Static Ctor
        static UpdateManager()
        {
            s_versionRegex = new Regex("[0-9]\\.[0-9]{1,2}\\.?[0-9]{0,2}");
            s_updateTextRegex = new Regex("<div class=\"markdown-body\">\\s*?<p>(?<Content>.*)</p>\\s*?</div>");
        }
        #endregion

        #region PerformUpdateCheckAsync
        public static async Task<bool> PerformUpdateCheckAsync()
        {
            bool blnRet = false;

            s_updateInfo = null;

            try
            {
                Log.WriteLine("Checking GitHub for new update...", LogType.Info);

                HttpWebRequest request = WebRequest.CreateHttp($"{BaseUrl}/latest");

                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Match versionMatch = s_versionRegex.Match(response.ResponseUri.AbsoluteUri);

                        if (versionMatch.Success)
                        {
                            Version newVersion = new Version(versionMatch.Value);

#if DEBUG
                            blnRet = newVersion > new Version(0, 0);
#else
                            blnRet = newVersion > System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
#endif

                            if (blnRet)
                            {
                                s_updateInfo = new UpdateInfo(newVersion);

                                string strResult;

                                using (WebClient webClient = new WebClient())
                                {
                                    strResult = await webClient.DownloadStringTaskAsync(response.ResponseUri);
                                }

                                //prepare for regex check
                                strResult = strResult.Trim().Replace("\n", string.Empty).Replace("\r", string.Empty);

                                Match updateTextMatch = s_updateTextRegex.Match(strResult);

                                if (updateTextMatch.Success)
                                {
                                    s_updateInfo.ReleaseNotes = updateTextMatch.Groups["Content"].Value.Replace("<br>", Environment.NewLine);
                                }
                                else { }

                                Log.WriteLine("New update available", LogType.Info);
                            }
                            else
                            {
                                Log.WriteLine("No update available", LogType.Info);
                            }
                        }
                        else { }
                    }
                    else { }
                }
            }
            catch (Exception ex)
            {
                //No internet connection or github down
                Log.WriteLine($"Couldn't perform update check: {ex}", LogType.Error);
            }

            return blnRet;
        }
        #endregion

        #region DownloadLatestRelease
        public static void DownloadLatestRelease()
        {
            if (Info != null)
            {
                Download(Info.Version);
            }
            else { }
        }
        #endregion

        #region Download
        public static void Download(Version version)
        {
            string strVersionString = version.ToString(3);

            if (version < NewVersionFormat)
            {
                strVersionString = version.ToString(2);
            }
            else { }

            using (s_webClient = new WebClient())
            {
                s_webClient.DownloadProgressChanged += (s, e) => DownloadProgressChanged?.Invoke(s, e);

                s_webClient.DownloadDataCompleted += (s, e) =>
                {
                    if (!e.Cancelled)
                    {
                        DownloadCompleted?.Invoke(s, e);
                    }
                    else
                    {
                        Log.WriteLine("Download canceled", LogType.Debug);
                    }
                };

                s_webClient.DownloadDataAsync(new Uri($"{BaseUrl}/download/{strVersionString}/Spawn.HDT.DustUtility.zip"));
            }
        }
        #endregion

        #region CancelDownload
        public static void CancelDownload()
        {
            s_webClient?.CancelAsync();
        }
        #endregion
    }
}