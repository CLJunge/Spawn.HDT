#region Using
using HearthMirror;
using HearthMirror.Objects;
using Hearthstone_Deck_Tracker.Enums;
using Spawn.HDT.DustUtility.CardManagement.Offline;
using Spawn.HDT.DustUtility.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace Spawn.HDT.DustUtility.AccountManagement
{
    [DebuggerDisplay("{AccountString}")]
    public class Account : IAccount
    {
        #region Constants
        public const string CollectionString = "collection";
        public const string DecksString = "decks";
        public const string PreferencesString = "prefs";
        public const string HistoryString = "history";
        #endregion

        #region Static Properties
        #region Empty
        public static Account Empty => new Account(null, Region.UNKNOWN);
        #endregion
        #endregion

        #region Member Variables
        private AccountPreferences m_preferences;
        #endregion

        #region Properties
        #region BattleTag
        public BattleTag BattleTag { get; }
        #endregion

        #region Region
        public Region Region { get; }
        #endregion

        #region DisplayString
        public string DisplayString { get; }
        #endregion

        #region AccountString
        public string AccountString { get; }
        #endregion

        #region IsEmpty
        public bool IsEmpty => BattleTag == null && Region == Region.UNKNOWN;
        #endregion

        #region IsValid
        public bool IsValid => !IsEmpty && !string.IsNullOrEmpty(AccountString);
        #endregion

        #region Preferences
        public AccountPreferences Preferences => m_preferences ?? (m_preferences = LoadPreferences());
        #endregion

        #region HasFiles
        public bool HasFiles => System.IO.File.Exists(DustUtilityPlugin.GetFullFileName(this, CollectionString))
            || System.IO.File.Exists(DustUtilityPlugin.GetFullFileName(this, DecksString))
            || System.IO.File.Exists(DustUtilityPlugin.GetFullFileName(this, HistoryString))
            || System.IO.File.Exists(DustUtilityPlugin.GetFullFileName(this, PreferencesString));
        #endregion
        #endregion

        #region Ctor
        private Account(BattleTag battleTag, Region region)
        {
            BattleTag = battleTag;
            Region = region;

            if (BattleTag != null)
            {
                AccountString = $"{battleTag.Name}_{battleTag.Number}_{region}";

                DisplayString = $"{battleTag.Name}#{battleTag.Number} ({region})";
            }
            else
            {
                AccountString = null;

                DisplayString = "Empty";
            }

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Initialized new account instance ({DisplayString})");
        }
        #endregion

        #region GetCollection
        public List<Card> GetCollection()
        {
            List<Card> lstRet = null;

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Loading collection... ({DisplayString})");

            if (DustUtilityPlugin.IsOffline && DustUtilityPlugin.Config.OfflineMode)
            {
                lstRet = Cache.LoadCollection(this);
            }
            else
            {
                lstRet = DustUtilityPlugin.GetCollectionWrapper();
            }

            if (lstRet != null)
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Collection loaded");

                lstRet = lstRet.Where(c => HearthDb.Cards.Collectible.ContainsKey(c.Id)).ToList();
            }
            else
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Error, "Couldn't load collection!");
            }

            return lstRet;
        }
        #endregion

        #region GetDecks
        public List<Deck> GetDecks()
        {
            List<Deck> lstRet = null;

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Loading decks... ({DisplayString})");

            if (DustUtilityPlugin.IsOffline && DustUtilityPlugin.Config.OfflineMode)
            {
                lstRet = Cache.LoadDecks(this);
            }
            else
            {
                lstRet = Reflection.GetDecks();
            }

            if (lstRet != null)
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Loaded decks");
            }
            else
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Error, "Couldn't load decks!");
            }

            return lstRet;
        }
        #endregion

        #region GetHistory
        public List<CachedHistoryCard> GetHistory() => HistoryManager.GetHistory(this);
        #endregion

        #region ExcludeDeckFromSearch
        public void ExcludeDeckFromSearch(long nDeckId)
        {
            if (!IsDeckExcludedFromSearch(nDeckId))
            {
                Preferences.ExcludedDecks.Add(nDeckId);

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Excluded deck (Id={nDeckId})");
            }
            else { }
        }
        #endregion

        #region IncludeDeckInSearch
        public void IncludeDeckInSearch(long nDeckId)
        {
            if (IsDeckExcludedFromSearch(nDeckId))
            {
                Preferences.ExcludedDecks.Remove(nDeckId);

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Included deck (Id={nDeckId})");
            }
            else { }
        }
        #endregion

        #region IsDeckExcludedFromSearch
        public bool IsDeckExcludedFromSearch(long nDeckId) => Preferences.ExcludedDecks.Contains(nDeckId);
        #endregion

        #region LoadPreferences
        private AccountPreferences LoadPreferences()
        {
            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Loading preferences... ({DisplayString})");

            AccountPreferences retVal = AccountPreferences.Load(this);

            //retVal.CardSelection.CollectionChanged += (s, e) => SavePreferences();
            //retVal.ExcludedDecks.CollectionChanged += (s, e) => SavePreferences();
            //retVal.SearchParameters.PropertyChanged += (s, e) => SavePreferences();
            
            return retVal;
        }
        #endregion

        #region SavePreferences
        public void SavePreferences()
        {
            if (m_preferences != null && IsValid)
            {
                m_preferences.Save(this);

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Saved preferences for {DisplayString}");
            }
            else { }
        }
        #endregion

        #region Equals
        public override bool Equals(object obj)
        {
            bool blnRet = false;

            if (obj != null)
            {
                if (obj is Account acc)
                {
                    if (!IsEmpty && !acc.IsEmpty)
                    {
                        blnRet = true;

                        if (acc.BattleTag != null)
                        {
                            blnRet &= acc.BattleTag.Name.Equals(BattleTag.Name);

                            blnRet &= acc.BattleTag.Number == BattleTag.Number;
                        }
                        else { }

                        blnRet &= acc.Region == Region;
                    }
                    else
                    {
                        blnRet = IsEmpty && acc.IsEmpty;
                    }
                }
                else
                {
                    blnRet = base.Equals(obj);
                }
            }
            else { }

            return blnRet;
        }
        #endregion

        #region GetHashCode
        public override int GetHashCode() => base.GetHashCode();
        #endregion

        #region Static Methods
        #region Parse
        public static Account Parse(string strAccountString)
        {
            Account retVal = null;

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Parsing account string... ({strAccountString})");

            try
            {
                string[] vTemp = strAccountString.Split('_');

                BattleTag battleTag = new BattleTag()
                {
                    Name = vTemp[0],
                    Number = Convert.ToInt32(vTemp[1])
                };

                retVal = new Account(battleTag, (Region)Enum.Parse(typeof(Region), vTemp[2]));
            }
            catch (Exception ex)
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Error, $"Couldn't parse account string: {ex}");
            }

            return retVal;
        }
        #endregion

        #region GetLoggedInAccountAsync
        public static async Task<Account> GetLoggedInAccountAsync()
        {
            Account retVal = null;

            if (!DustUtilityPlugin.IsOffline)
            {
                try
                {
                    retVal = new Account(Reflection.GetBattleTag(), await Hearthstone_Deck_Tracker.Helper.GetCurrentRegion());

                    DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Retrieved logged in account ({retVal.DisplayString})");
                }
                catch (Exception ex)
                {
                    DustUtilityPlugin.Logger.Log(LogLevel.Error, $"Couldn't retrieve currently logged in account: {ex}");
                }
            }
            else { }

            return retVal;
        }
        #endregion
        #endregion
    }
}