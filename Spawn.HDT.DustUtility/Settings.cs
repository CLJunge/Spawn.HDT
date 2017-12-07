using Spawn.HDT.DustUtility.Search;
using System.Diagnostics;

namespace Spawn.HDT.DustUtility
{
    [DebuggerStepThrough]
    public static class Settings
    {
        #region Properties
        #region OfflineMode
        public static bool OfflineMode
        {
            get => Properties.Settings.Default.OfflineMode;
            set
            {
                Properties.Settings.Default.OfflineMode = value;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        #region SortOrder
        public static string SortOrder
        {
            get => Properties.Settings.Default.SortOrder;
            set
            {
                Properties.Settings.Default.SortOrder = value;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        #region CheckForUpdate
        public static bool CheckForUpdate
        {
            get => Properties.Settings.Default.CheckForUpdate;
            set
            {
                Properties.Settings.Default.CheckForUpdate = value;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        #region SearchParameters
        public static Parameters SearchParameters
        {
            get => Properties.Settings.Default.SearchParameters;
            set
            {
                Properties.Settings.Default.SearchParameters = value;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        #region SaveInterval
        public static int SaveInterval
        {
            get => Properties.Settings.Default.SaveInterval;
            set
            {
                Properties.Settings.Default.SaveInterval = value;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        #region LastSelectedAccount
        public static string LastSelectedAccount
        {
            get => Properties.Settings.Default.LastSelectedAccount;
            set
            {
                Properties.Settings.Default.LastSelectedAccount = value;
                Properties.Settings.Default.Save();
            }
        }
        #endregion
        #endregion
    }
}