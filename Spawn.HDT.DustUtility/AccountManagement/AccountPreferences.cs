#region Using
using Spawn.HDT.DustUtility.CardManagement;
using Spawn.HDT.DustUtility.CardManagement.Offline;
using Spawn.HDT.DustUtility.Logging;
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

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Initialized new 'AccountPreferences' instance");
        }
        #endregion

        #region Save
        public void Save(IAccount account)
        {
            FileManager.Write(DustUtilityPlugin.GetFullFileName(account, Account.PreferencesString), this);

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Saved preferences for {account.DisplayString}");
        }
        #endregion

        #region [STATIC] Load
        public static AccountPreferences Load(IAccount account)
        {
            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Loading preferences for {account.DisplayString}...");

            return FileManager.Load<AccountPreferences>(DustUtilityPlugin.GetFullFileName(account, Account.PreferencesString));
        }
        #endregion
    }
}