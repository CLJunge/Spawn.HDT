#region Using
using GalaSoft.MvvmLight;
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
        private int m_nSaveInterval;
        private string m_strLastSelectedAccount;
        private bool m_blnColoredCardItems;
        private bool m_blnAutoDisenchanting;
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

        #region ColoredCardItems
        [DefaultValue(true)]
        public bool ColoredCardItems
        {
            get => m_blnColoredCardItems;
            set => Set(ref m_blnColoredCardItems, value);
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
            SaveInterval = 120;
            LastSelectedAccount = string.Empty;
            ColoredCardItems = true;
            AutoDisenchanting = false;
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