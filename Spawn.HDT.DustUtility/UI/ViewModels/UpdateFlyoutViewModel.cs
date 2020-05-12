#region Using
using GalaSoft.MvvmLight.CommandWpf;
using Spawn.HDT.DustUtility.Logging;
using Spawn.HDT.DustUtility.Net;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class UpdateFlyoutViewModel : ViewModelBase
    {
        #region Member Variables
        private string m_strUpdateMessage;
        private string m_strDownloadHeaderText;
        private Visibility m_messagePanelVisibility;
        private Visibility m_downloadPanelVisibility;
        private Visibility m_downloadFinishedPanelVisibility;
        private int m_nDownloadProgress;
        #endregion

        #region Properties
        #region CanNotifyDirtyStatus
        public override bool CanNotifyDirtyStatus => false;
        #endregion

        #region UpdateMessage
        public string UpdateMessage
        {
            get => m_strUpdateMessage;
            set => Set(ref m_strUpdateMessage, value);
        }
        #endregion

        #region DownloadHeaderText
        public string DownloadHeaderText
        {
            get => m_strDownloadHeaderText;
            set => Set(ref m_strDownloadHeaderText, value);
        }
        #endregion

        #region MessagePanelVisibility
        public Visibility MessagePanelVisibility
        {
            get => m_messagePanelVisibility;
            set => Set(ref m_messagePanelVisibility, value);
        }
        #endregion

        #region DownloadPanelVisibility
        public Visibility DownloadPanelVisibility
        {
            get => m_downloadPanelVisibility;
            set => Set(ref m_downloadPanelVisibility, value);
        }
        #endregion

        #region DownloadFinishedPanelVisibility
        public Visibility DownloadFinishedPanelVisibility
        {
            get => m_downloadFinishedPanelVisibility;
            set => Set(ref m_downloadFinishedPanelVisibility, value);
        }
        #endregion

        #region DownloadProgress
        public int DownloadProgress
        {
            get => m_nDownloadProgress;
            set => Set(ref m_nDownloadProgress, value);
        }
        #endregion

        #region StartDownloadCommand
        public ICommand StartDownloadCommand => new RelayCommand(StartDownload);
        #endregion

        #region CloseFlyoutCommand
        public ICommand CloseFlyoutCommand => new RelayCommand(CloseFlyout);
        #endregion

        #region CancelDownloadCommand
        public ICommand CancelDownloadCommand => new RelayCommand(CancelDownload);
        #endregion
        #endregion

        #region Ctor
        public UpdateFlyoutViewModel()
        {
            DownloadHeaderText = "Downloading \"Spawn.HDT.DustUtility.zip\"...";

#if DEBUG
            if (IsInDesignMode)
                UpdateMessage = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod" + Environment.NewLine + Environment.NewLine + "tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo" + Environment.NewLine + "dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy" + Environment.NewLine + "eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum." + Environment.NewLine + Environment.NewLine + "Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.";
#endif

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Created new 'UpdatedFlyoutViewModel' instance");
        }
        #endregion

        #region InitializeAsync
        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            MessagePanelVisibility = Visibility.Visible;
            DownloadPanelVisibility = Visibility.Collapsed;
            DownloadFinishedPanelVisibility = Visibility.Collapsed;

            DownloadProgress = 0;

            StringBuilder sb = new StringBuilder();

            sb.Append($"Update {UpdateManager.Info.Version.ToString(3)} has been released.")
                .Append(Environment.NewLine + Environment.NewLine);

            if (UpdateManager.Info.HasReleaseNotes)
            {
                sb.Append("Release Notes:").Append(Environment.NewLine)
                .Append(UpdateManager.Info.ReleaseNotes)
                .Append(Environment.NewLine + Environment.NewLine);
            }

            sb.Append("Would you like to download it?");

            UpdateMessage = sb.ToString();

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Finished initializing");
        }
        #endregion

        #region StartDownload
        private void StartDownload()
        {
            Hearthstone_Deck_Tracker.Helper.TryOpenUrl("https://github.com/CLJunge/Spawn.HDT.DustUtility/releases/latest");

            CloseFlyout();
        }

        private void StartDownloadOld()
        {
            MessagePanelVisibility = Visibility.Collapsed;
            DownloadPanelVisibility = Visibility.Visible;

            UpdateManager.DownloadProgressChanged += OnDownloadProgressChanged;
            UpdateManager.DownloadCompleted += OnDownloadCompleted;

            UpdateManager.Download(UpdateManager.Info.Version);

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Started download... (Version={UpdateManager.Info.Version.ToString(3)})");
        }
        #endregion

        #region CloseFlyout
        private void CloseFlyout()
        {
            if (DustUtilityPlugin.MainWindow.UpdateFlyout.IsOpen)
            {
                DustUtilityPlugin.MainWindow.UpdateFlyout.IsOpen = false;

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Closed flyout");
            }

            DustUtilityPlugin.CloseUpdateDialog();
        }
        #endregion

        #region CancelDownload
        private void CancelDownload()
        {
            UpdateManager.CancelDownload();

            UpdateManager.DownloadProgressChanged -= OnDownloadProgressChanged;
            UpdateManager.DownloadCompleted -= OnDownloadCompleted;

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Canceled current download");

            CloseFlyout();

            DustUtilityPlugin.CloseUpdateDialog();
        }
        #endregion

        #region Download Events
        #region OnDownloadCompleted
        private void OnDownloadCompleted(object sender, System.Net.DownloadDataCompletedEventArgs e)
        {
            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Download finished. Extracting zip file...");

            UpdateManager.DownloadProgressChanged -= OnDownloadProgressChanged;
            UpdateManager.DownloadCompleted -= OnDownloadCompleted;

            string strPath = Path.Combine(DustUtilityPlugin.DataDirectory, "update.zip");

            using (FileStream fs = File.Open(strPath, FileMode.Create))
                fs.Write(e.Result, 0, e.Result.Length);

            DeleteOldPluginVersions();

            string strTargetDir = Path.Combine(Hearthstone_Deck_Tracker.Config.AppDataPath, "Plugins", "Spawn.HDT.DustUtility");

            if (!Directory.Exists(strTargetDir))
                Directory.CreateDirectory(strTargetDir);

            ZipFile.ExtractToDirectory(strPath, strTargetDir);

            File.Delete(strPath);

            DownloadPanelVisibility = Visibility.Collapsed;
            DownloadFinishedPanelVisibility = Visibility.Visible;
        }
        #endregion

        #region OnDownloadProgressChanged
        private void OnDownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e) => DownloadProgress = e.ProgressPercentage;
        #endregion
        #endregion

        #region DeleteOldPluginVersions
        private void DeleteOldPluginVersions()
        {
            string strHdtPluginDir = Path.Combine(Hearthstone_Deck_Tracker.Config.AppDataPath, "Plugins");

            string strSearchPattern = "*DustUtility*";

            string[] vOldFiles = Directory.GetFiles(strHdtPluginDir, strSearchPattern);

            for (int i = 0; i < vOldFiles.Length; i++)
                File.Delete(vOldFiles[i]);

            string[] vOldPluginDirs = Directory.GetDirectories(strHdtPluginDir, strSearchPattern);

            for (int i = 0; i < vOldPluginDirs.Length; i++)
                Directory.Delete(vOldPluginDirs[i], true);

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Deleted old plugin versions");
        }
        #endregion
    }
}