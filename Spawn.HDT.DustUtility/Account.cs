using HearthMirror;
using HearthMirror.Objects;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Spawn.HDT.DustUtility.Offline;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Spawn.HDT.DustUtility
{
    [DebuggerDisplay("{AccountString}")]
    public class Account
    {
        #region Static Properties
        #region Empty
        public static Account Empty => new Account(null, Region.UNKNOWN);
        #endregion

        #region Current
        public static Account Current => GetCurrentAccount();
        #endregion
        #endregion

        #region Member Variables
        private Preferences m_preferences;
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
        public bool IsValid => !string.IsNullOrEmpty(AccountString);
        #endregion

        #region AccountPreferences
        public Preferences AccountPreferences
        {
            get => m_preferences ?? (m_preferences = Preferences.Load(this));
            set
            {
                m_preferences = value;

                Preferences.Save(this, m_preferences);
            }
        }
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
                AccountString = string.Empty;

                DisplayString = string.Empty;
            }
        }
        #endregion

        #region LoadCollection
        public List<Card> LoadCollection()
        {
            List<Card> lstRet = null;

            Log.WriteLine("Loading collection...", LogType.Debug);

            if (DustUtilityPlugin.IsOffline)
            {
                lstRet = Cache.LoadCollection(this);
            }
            else
            {
                lstRet = Reflection.GetCollection();
            }

            if (lstRet != null)
            {
                Log.WriteLine("Loaded collection", LogType.Debug);
            }
            else { }

            return lstRet;
        }
        #endregion

        #region LoadDecks
        public List<Deck> LoadDecks()
        {
            List<Deck> lstRet = null;

            Log.WriteLine("Loading decks...", LogType.Debug);

            if (DustUtilityPlugin.IsOffline)
            {
                lstRet = Cache.LoadDecks(this);
            }
            else
            {
                lstRet = Reflection.GetDecks();
            }

            if (lstRet != null)
            {
                Log.WriteLine("Loaded decks", LogType.Debug);
            }
            else { }

            return lstRet;
        }
        #endregion

        #region ExcludeDeck
        public void ExcludeDeck(long nDeckId)
        {
            if (!IsDeckExcluded(nDeckId))
            {
                AccountPreferences.ExcludedDecks.Add(nDeckId);
            }
            else { }
        }
        #endregion

        #region IncludeDeck
        public void IncludeDeck(long nDeckId)
        {
            if (IsDeckExcluded(nDeckId))
            {
                AccountPreferences.ExcludedDecks.Remove(nDeckId);
            }
            else { }
        }
        #endregion

        #region IsDeckExcluded
        public bool IsDeckExcluded(long nDeckId)
        {
            return AccountPreferences.ExcludedDecks.Contains(nDeckId);
        }
        #endregion

        #region Equals
        public override bool Equals(object obj)
        {
            bool blnRet = false;

            if (obj is Account)
            {
                Account acc = obj as Account;

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
                blnRet = base.Equals(obj);
            }

            return blnRet;
        }
        #endregion

        #region GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region Static Methods
        #region Parse
        public static Account Parse(string strAccountString)
        {
            string[] vTemp = strAccountString.Split('_');

            BattleTag battleTag = new BattleTag()
            {
                Name = vTemp[0],
                Number = Convert.ToInt32(vTemp[1])
            };

            return new Account(battleTag, (Region)Enum.Parse(typeof(Region), vTemp[2]));
        }
        #endregion

        #region GetCurrentAccount
        private static Account GetCurrentAccount()
        {
            Account retVal = Empty;

            if (Hearthstone_Deck_Tracker.API.Core.Game.IsRunning)
            {
                retVal = new Account(HearthMirror.Reflection.GetBattleTag(), Hearthstone_Deck_Tracker.Helper.GetCurrentRegion().Result);
            }
            else { }

            return retVal;
        }
        #endregion
        #endregion

        public class Preferences
        {
            #region Constants
            public const string PreferencesString = "prefs";
            #endregion

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
            public Preferences()
            {
                m_lstExcludedDecks = new List<long>();
                m_lstCardSelection = new List<CachedCard>();
            }
            #endregion

            #region Save
            public static void Save(Account account, Preferences preferences)
            {
                FileManager.Write(DustUtilityPlugin.GetFullFileName(account, PreferencesString), preferences);
            }
            #endregion

            #region Load
            public static Preferences Load(Account account)
            {
                return FileManager.Load<Preferences>(DustUtilityPlugin.GetFullFileName(account, PreferencesString));
            }
            #endregion
        }
    }
}