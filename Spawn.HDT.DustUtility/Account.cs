using HearthMirror.Objects;
using Hearthstone_Deck_Tracker.Enums;
using System;
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