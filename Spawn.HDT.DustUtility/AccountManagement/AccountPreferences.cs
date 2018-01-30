#region Using
using Spawn.HDT.DustUtility.CardManagement;
using Spawn.HDT.DustUtility.CardManagement.Offline;
using System.Collections.Generic;
using System.ComponentModel;
#endregion

namespace Spawn.HDT.DustUtility.AccountManagement
{
    public class AccountPreferences
    {
        #region Properties
        #region ExcludedDecks
        [DefaultValue(typeof(List<long>))]
        public List<long> ExcludedDecks { get; set; }
        #endregion

        #region CardSelection
        [DefaultValue(typeof(List<CachedCard>))]
        public List<CachedCard> CardSelection { get; set; }
        #endregion

        #region SearchParameters
        [DefaultValue(typeof(SearchParameters))]
        public SearchParameters SearchParameters { get; set; }
        #endregion
        #endregion

        #region Ctor
        public AccountPreferences()
        {
            ExcludedDecks = new List<long>();
            CardSelection = new List<CachedCard>();
            SearchParameters = new SearchParameters(true);
        }
        #endregion

        #region Save
        public void Save(IAccount account)
        {
            FileManager.Write(DustUtilityPlugin.GetFullFileName(account, Account.PreferencesString), this);
        }
        #endregion

        #region [STATIC] Load
        public static AccountPreferences Load(IAccount account)
        {
            return FileManager.Load<AccountPreferences>(DustUtilityPlugin.GetFullFileName(account, Account.PreferencesString));
        }
        #endregion
    }
}