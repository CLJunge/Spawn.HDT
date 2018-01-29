using Spawn.HDT.DustUtility.CardManagement.Offline;
using System.Collections.Generic;

namespace Spawn.HDT.DustUtility
{
    public class AccountPreferences
    {
        #region Member Variables
        private List<long> m_lstExcludedDecks;
        private List<CachedCard> m_lstCardSelection;
        #endregion

        #region Properties
        #region ExcludedDecks
        public List<long> ExcludedDecks
        {
            get => m_lstExcludedDecks;
            set => m_lstExcludedDecks = value;
        }
        #endregion

        #region CardSelection
        public List<CachedCard> CardSelection
        {
            get => m_lstCardSelection;
            set => m_lstCardSelection = value;
        }
        #endregion
        #endregion

        #region Ctor
        public AccountPreferences()
        {
            m_lstExcludedDecks = new List<long>();
            m_lstCardSelection = new List<CachedCard>();
        }
        #endregion

        #region Save
        public void Save(Account account)
        {
            FileManager.Write(DustUtilityPlugin.GetFullFileName(account, Account.PreferencesString), this);
        }
        #endregion

        #region [STATIC] Load
        public static AccountPreferences Load(Account account)
        {
            return FileManager.Load<AccountPreferences>(DustUtilityPlugin.GetFullFileName(account, Account.PreferencesString));
        }
        #endregion
    }
}