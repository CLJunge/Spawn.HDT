using GalaSoft.MvvmLight;
using Spawn.HDT.DustUtility.CardManagement;
using System.ComponentModel;
using System.IO;

namespace Spawn.HDT.DustUtility
{
    public class Configuration : ObservableObject
    {
        #region Member Variables
        private bool m_blnOfflineMode;
        private string m_strSortOrder;
        private bool m_blnCheckForUpdates;
        private SearchParameters m_searchParameters;
        private int m_nSaveInterval;
        private string m_strLastSelectedAccount;
        private int m_nVersion;
        #endregion

        #region Properties
        #region OfflineMode
        [DefaultValue(true)]
        public bool OfflineMode
        {
            get => m_blnOfflineMode;
            set => Set(ref m_blnOfflineMode, value);
        }
        #endregion

        #region SortOrder
        [DefaultValue("")]
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

        #region SearchParameters
        [DefaultValue(null)]
        public SearchParameters SearchParameters
        {
            get => m_searchParameters;
            set => Set(ref m_searchParameters, value);
        }
        #endregion

        #region SaveInterval
        [DefaultValue(120)]
        public int SaveInterval
        {
            get => m_nSaveInterval;
            set => Set(ref m_nSaveInterval, value);
        }
        #endregion

        #region LastSelectedAccount
        [DefaultValue("")]
        public string LastSelectedAccount
        {
            get => m_strLastSelectedAccount;
            set => Set(ref m_strLastSelectedAccount, value);
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
            OfflineMode = true;
            SortOrder = string.Empty;
            CheckForUpdates = true;
            SearchParameters = null;
            SaveInterval = 120;
            LastSelectedAccount = string.Empty;
            Version = 1;

            PropertyChanged += (s, e) => Save();
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