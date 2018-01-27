using GalaSoft.MvvmLight;
using HearthDb.Enums;
using Spawn.HDT.DustUtility.CardManagement;
using System;
using System.Diagnostics;

namespace Spawn.HDT.DustUtility.UI.Models
{
    [DebuggerDisplay("{Name} ({Count})")]
    public class DataGridCardItem : ObservableObject
    {
        #region Member Variables
        private int m_nCount;
        private string m_strName;
        private bool m_blnGolden;
        private int m_nDust;
        private Rarity m_rarity;
        private string m_strRarityString;
        private string m_strCardClass;
        private CardSet m_cardSet;
        private string m_strCardSetString;
        private int m_nManaCost;
        private CardWrapper m_tag;
        #endregion

        #region Properties
        #region Count
        public int Count
        {
            get => m_nCount;
            set => Set(ref m_nCount, value);
        }
        #endregion

        #region Name
        public string Name
        {
            get => m_strName;
            set => Set(ref m_strName, value);
        }
        #endregion

        #region Golden
        public bool Golden
        {
            get => m_blnGolden;
            set => Set(ref m_blnGolden, value);
        }
        #endregion

        #region Dust
        public int Dust
        {
            get => m_nDust;
            set => Set(ref m_nDust, value);
        }
        #endregion

        #region Rarity
        public Rarity Rarity
        {
            get => m_rarity;
            set => Set(ref m_rarity, value);
        }
        #endregion

        #region RarityString
        public string RarityString
        {
            get => m_strRarityString;
            set => Set(ref m_strRarityString, value);
        }
        #endregion

        #region CardClass
        public string CardClass
        {
            get => m_strCardClass;
            set => Set(ref m_strCardClass, value);
        }
        #endregion

        #region CardSet
        public CardSet CardSet
        {
            get => m_cardSet;
            set => Set(ref m_cardSet, value);
        }
        #endregion

        #region CardSetString
        public string CardSetString
        {
            get => m_strCardSetString;
            set => Set(ref m_strCardSetString, value);
        }
        #endregion

        #region ManaCost
        public int ManaCost
        {
            get => m_nManaCost;
            set => Set(ref m_nManaCost, value);
        }
        #endregion

        #region Tag
        public CardWrapper Tag
        {
            get => m_tag;
            set => Set(ref m_tag, value);
        }
        #endregion
        #endregion

        #region CreateCopy
        public DataGridCardItem CreateCopy()
        {
            return new DataGridCardItem
            {
                Count = Count,
                Name = Name,
                Golden = Golden,
                Dust = Dust,
                Rarity = Rarity,
                RarityString = RarityString,
                CardClass = CardClass,
                CardSet = CardSet,
                CardSetString = CardSetString,
                ManaCost = ManaCost,
                Tag = Tag
            };
        }
        #endregion

        #region [STATIC] FromCardWrapper
        public static DataGridCardItem FromCardWrapper(CardWrapper wrapper)
        {
            return new DataGridCardItem()
            {
                Count = wrapper.Count,
                Dust = wrapper.GetDustValue(),
                Golden = wrapper.Card.Premium,
                Name = wrapper.DbCard.Name,
                Rarity = wrapper.DbCard.Rarity,
                RarityString = wrapper.DbCard.Rarity.GetString(),
                CardClass = wrapper.DbCard.Class.GetString(),
                CardSet = wrapper.DbCard.Set,
                CardSetString = wrapper.DbCard.Set.GetString(),
                ManaCost = wrapper.DbCard.Cost,
                Tag = wrapper
            }; ;
        }
        #endregion
    }

    [DebuggerDisplay("{Name} ({Count})")]
    public class DataGridCardItemEx : DataGridCardItem
    {
        #region Member Variables
        private DateTime m_dtTimestamp;
        #endregion

        #region Properties
        public DateTime Timestamp
        {
            get => m_dtTimestamp;
            set => Set(ref m_dtTimestamp, value);
        }
        #endregion

        #region CreateCopy
        public new DataGridCardItemEx CreateCopy()
        {
            return new DataGridCardItemEx
            {
                Count = Count,
                Name = Name,
                Golden = Golden,
                Dust = Dust,
                Rarity = Rarity,
                RarityString = RarityString,
                CardClass = CardClass,
                CardSet = CardSet,
                CardSetString = CardSetString,
                ManaCost = ManaCost,
                Tag = Tag,
                Timestamp = Timestamp
            };
        }
        #endregion

        #region [STATIC] FromCardWrapperEx
        public static DataGridCardItemEx FromCardWrapperEx(CardWrapperEx wrapper)
        {
            DataGridCardItemEx retVal = new DataGridCardItemEx()
            {
                Count = wrapper.Count,
                Dust = (wrapper.Count > 0 ? wrapper.Card.GetCraftingCost() : wrapper.GetDustValue()),
                Golden = wrapper.Card.Premium,
                Name = wrapper.DbCard.Name,
                Rarity = wrapper.DbCard.Rarity,
                RarityString = wrapper.DbCard.Rarity.GetString(),
                CardClass = wrapper.DbCard.Class.GetString(),
                CardSet = wrapper.DbCard.Set,
                CardSetString = wrapper.DbCard.Set.GetString(),
                ManaCost = wrapper.DbCard.Cost,
                Timestamp = wrapper.Timestamp,
                Tag = wrapper
            };

            return retVal;
        }
        #endregion
    }
}