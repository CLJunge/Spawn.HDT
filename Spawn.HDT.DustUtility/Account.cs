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
        private List<long> m_lstExcludedDecks;
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

            m_lstExcludedDecks = new List<long>();
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
                m_lstExcludedDecks.Add(nDeckId);
            }
            else { }
        }
        #endregion

        #region IncludeDeck
        public void IncludeDeck(long nDeckId)
        {
            if (IsDeckExcluded(nDeckId))
            {
                m_lstExcludedDecks.Remove(nDeckId);
            }
            else { }
        }
        #endregion

        #region IsDeckExcluded
        public bool IsDeckExcluded(long nDeckId)
        {
            return m_lstExcludedDecks.Contains(nDeckId);
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
    }
}