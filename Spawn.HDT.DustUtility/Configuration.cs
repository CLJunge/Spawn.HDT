using Spawn.HDT.DustUtility.CardManagement;
using System.ComponentModel;
using System.IO;

namespace Spawn.HDT.DustUtility
{
    public class Configuration
    {
        #region Properties
        #region OfflineMode
        [DefaultValue(true)]
        public bool OfflineMode { get; set; }
        #endregion

        #region SortOrder
        [DefaultValue("")]
        public string SortOrder { get; set; }
        #endregion

        #region CheckForUpdates
        [DefaultValue(true)]
        public bool CheckForUpdates { get; set; }
        #endregion

        #region SearchParameters
        [DefaultValue(null)]
        public SearchParameters SearchParameters { get; set; }
        #endregion

        #region SaveInterval
        [DefaultValue(120)]
        public int SaveInterval { get; set; }
        #endregion

        #region LastSelectedAccount
        [DefaultValue("")]
        public string LastSelectedAccount { get; set; }
        #endregion

        #region Version
        [DefaultValue(1)]
        public int Version { get; set; }
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