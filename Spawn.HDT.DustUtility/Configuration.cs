#region Using
using GalaSoft.MvvmLight;
using Spawn.HDT.DustUtility.Logging;
using Spawn.HDT.DustUtility.UI;
using System.ComponentModel;
using System.IO;
#endregion

namespace Spawn.HDT.DustUtility
{
    public class Configuration : ObservableObject
    {
        #region Member Variables
        private bool m_blnOfflineMode;
        private string m_strSortOrder;
        private bool m_blnCheckForUpdates;
        private string m_strLastSelectedAccount;
        private bool m_blnColoredCardLabels;
        private bool m_blnAutoDisenchanting;
        private bool m_blnRememberQueryString;
        private bool m_blnShowNotifications;
        private LogLevel m_logLevel;
        private ViewMode m_viewMode;
        private int m_nVersion;
        #endregion

        #region Properties
        #region OfflineMode
        [DefaultValue(false)]
        public bool OfflineMode
        {
            get => m_blnOfflineMode;
            set => Set(ref m_blnOfflineMode, value);
        }
        #endregion

        #region SortOrder
        [DefaultValue("Rarity;Golden;CardClass;CardSet;Name")]
        public string SortOrder
        {
            get => m_strSortOrder;
            set => Set(ref m_strSortOrder, value);
        }
        #endregion

        #region CheckForUpdates
        [DefaultValue(true)]
        public bool CheckForUpdates
        {
            get => m_blnCheckForUpdates;
            set => Set(ref m_blnCheckForUpdates, value);
        }
        #endregion

        #region LastSelectedAccount
        [DefaultValue(null)]
        public string LastSelectedAccount
        {
            get => m_strLastSelectedAccount;
            set => Set(ref m_strLastSelectedAccount, value);
        }
        #endregion

        #region ColoredCardLabels
        [DefaultValue(true)]
        public bool ColoredCardLabels
        {
            get => m_blnColoredCardLabels;
            set => Set(ref m_blnColoredCardLabels, value);
        }
        #endregion

        #region AutoDisenchanting
        [DefaultValue(false)]
        public bool AutoDisenchanting
        {
            get => m_blnAutoDisenchanting;
            set => Set(ref m_blnAutoDisenchanting, value);
        }
        #endregion

        #region RememberQueryString
        [DefaultValue(false)]
        public bool RememberQueryString
        {
            get => m_blnRememberQueryString;
            set => Set(ref m_blnRememberQueryString, value);
        }
        #endregion

        #region ShowNotifications
        [DefaultValue(true)]
        public bool ShowNotifications
        {
            get => m_blnShowNotifications;
            set => Set(ref m_blnShowNotifications, value);
        }
        #endregion

        #region LogLevel
        [DefaultValue(LogLevel.Trace)]
        public LogLevel LogLevel
        {
            get => m_logLevel;
            set => Set(ref m_logLevel, value);
        }
        #endregion

        #region ViewMode
        [DefaultValue(ViewMode.Default)]
        public ViewMode ViewMode
        {
            get => m_viewMode;
            set => Set(ref m_viewMode, value);
        }
        #endregion

        #region Version
        [DefaultValue(1)]
        public int Version
        {
            get => m_nVersion;
            set => Set(ref m_nVersion, value);
        }
        #endregion
        #endregion

        #region Ctor
        public Configuration()
        {
            OfflineMode = false;
            SortOrder = "Rarity;Golden;CardClass;CardSet;Name";
            CheckForUpdates = true;
            LastSelectedAccount = null;
            ColoredCardLabels = true;
            AutoDisenchanting = false;
            RememberQueryString = false;
            ShowNotifications = true;
            LogLevel = LogLevel.Trace;
            ViewMode = ViewMode.Default;
            Version = 1;
        }
        #endregion

        #region Save
        public void Save(string strFileName = "config.xml")
        {
            FileManager.Write(Path.Combine(DustUtilityPlugin.DataDirectory, strFileName), this);
        }
        #endregion

        #region [STATIC] Load
        public static Configuration Load(string strFileName = "config.xml")
        {
            return FileManager.Load<Configuration>(Path.Combine(DustUtilityPlugin.DataDirectory, strFileName));
        }
        #endregion
    }
}