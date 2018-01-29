#region Using
using GalaSoft.MvvmLight;
using HearthDb.Enums;
using Spawn.HDT.DustUtility.Hearthstone;
using System;
using System.Diagnostics;
using System.Windows.Media.Imaging;
#endregion

namespace Spawn.HDT.DustUtility.UI.Models
{
    [DebuggerDisplay("{Name} ({Count})")]
    public class CardItem : ObservableObject
    {
        #region Member Variables
        private string m_strId;
        private CardWrapper m_wrapper;
        private bool m_blnColoredCount;
        #endregion

        #region Properties
        #region Wrapper
        public CardWrapper Wrapper
        {
            get => m_wrapper;
            set => Set(ref m_wrapper, value);
        }
        #endregion

        #region Id
        public string Id
        {
            get => (m_wrapper == null ? m_strId : m_wrapper.RawCard.Id);
            set
            {
                if (m_wrapper == null)
                {
                    Set(ref m_strId, value);
                }
                else { }
            }
        }
        #endregion

        #region Count
        public int Count => (Wrapper?.Count ?? 0);
        #endregion

        #region Name
        public string Name => (Wrapper?.DbCard.Name ?? HearthDb.Cards.All[Id].Name);
        #endregion

        #region Golden
        public bool Golden => (Wrapper?.RawCard.Premium ?? false);
        #endregion

        #region Dust
        public int Dust => (Wrapper?.DustValue ?? 0);
        #endregion

        #region Rarity
        public string Rarity => (Wrapper?.DbCard.Rarity.GetString() ?? HearthDb.Cards.All[Id].Rarity.GetString());
        #endregion

        #region CardClass
        public CardClass CardClass => (Wrapper?.DbCard.Class ?? HearthDb.Cards.All[Id].Class);
        #endregion

        #region CardClassString
        public string CardClassString => CardClass.GetString();
        #endregion

        #region CardSet
        public CardSet CardSet => (Wrapper?.DbCard.Set ?? HearthDb.Cards.All[Id].Set);
        #endregion

        #region CardSetString
        public string CardSetString => CardSet.GetString();
        #endregion

        #region ManaCost
        public int ManaCost => (m_wrapper?.DbCard.Cost ?? HearthDb.Cards.All[Id].Cost);
        #endregion

        #region Date
        public DateTime? Date => m_wrapper?.Date;
        #endregion

        #region CardImage
        public BitmapImage CardImage => Hearthstone_Deck_Tracker.Utility.ImageCache.GetCardImage(m_wrapper?.Card ?? new Hearthstone_Deck_Tracker.Hearthstone.Card(HearthDb.Cards.All[Id]));
        #endregion

        #region ColoredCount
        public bool ColoredCount
        {
            get => m_blnColoredCount;
            set => Set(ref m_blnColoredCount, value);
        }
        #endregion
        #endregion

        #region Ctor
        public CardItem()
        {
        }

        public CardItem(CardWrapper wrapper)
        {
            Wrapper = wrapper;
        }
        #endregion

        #region CreateCopy
        public CardItem CreateCopy()
        {
            return new CardItem(Wrapper);
        }
        #endregion
    }
}