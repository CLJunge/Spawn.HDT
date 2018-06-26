#region Using
using Spawn.HDT.DustUtility.CardManagement;
using Spawn.HDT.DustUtility.CardManagement.Offline;
using Spawn.HDT.DustUtility.Logging;
using Spawn.HDT.DustUtility.Util;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
#endregion

namespace Spawn.HDT.DustUtility.AccountManagement
{
    public class AccountPreferences
    {
        #region Properties
        #region ExcludedDecks
        [DefaultValue(typeof(List<long>))]
        public ObservableCollection<long> ExcludedDecks { get; set; }
        #endregion

        #region CardSelection
        [DefaultValue(typeof(List<CachedCard>))]
        public ObservableCollection<CachedCard> CardSelection { get; set; }
        #endregion

        #region SearchParameters
        [DefaultValue(typeof(SearchParameters))]
        public SearchParameters SearchParameters { get; set; }
        #endregion
        #endregion

        #region Ctor
        public AccountPreferences()
        {
            ExcludedDecks = new ObservableCollection<long>();
            CardSelection = new ObservableCollection<CachedCard>();
            SearchParameters = new SearchParameters(true);

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Created new 'AccountPreferences' instance");
        }
        #endregion

        #region Save
        public void Save(IAccount account) => FileHelper.Write(DustUtilityPlugin.GetFullFileName(account, Account.PreferencesString), this);
        #endregion

        #region [STATIC] Load
        public static AccountPreferences Load(IAccount account) => FileHelper.Load<AccountPreferences>(DustUtilityPlugin.GetFullFileName(account, Account.PreferencesString));
        #endregion
    }
}