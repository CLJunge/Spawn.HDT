#region Using
using HearthMirror.Objects;
using Spawn.HDT.DustUtility.CardManagement.Offline;
using Spawn.HDT.DustUtility.Logging;
using System;
using System.Diagnostics;
#endregion

namespace Spawn.HDT.DustUtility.Hearthstone
{
    [DebuggerDisplay("({Count}x) {DbCard.Name} Golden={RawCard.Premium}")]
    public class CardWrapper : ICloneable
    {
        #region Member Variables
        private int? m_nDustValue;
        #endregion

        #region Properties
        #region RawCard
        public Card RawCard { get; }
        #endregion

        #region Card
        public Hearthstone_Deck_Tracker.Hearthstone.Card Card => new Hearthstone_Deck_Tracker.Hearthstone.Card(DbCard);
        #endregion

        #region DbCard
        public HearthDb.Card DbCard => HearthDb.Cards.Collectible[RawCard.Id];
        #endregion

        #region MaxCountInDecks
        public int MaxCountInDecks { get; set; }
        #endregion

        #region DustValue
        public int DustValue
        {
            get => m_nDustValue ?? GetDustValue();
            set => m_nDustValue = value;
        }
        #endregion

        #region Count
        public int Count => RawCard.Count - MaxCountInDecks;
        #endregion

        #region Date
        public DateTime? Date { get; set; }
        #endregion
        #endregion

        #region Ctor
        public CardWrapper(Card card)
        {
            RawCard = card;

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Created new 'CardWrapper' instance ({Count}x {DbCard.Name})");
        }

        public CardWrapper(CachedCard cachedCard)
            : this(new Card(cachedCard.Id, cachedCard.Count, cachedCard.IsGolden)) { }

        public CardWrapper(CachedHistoryCard historyCard)
            : this(new Card(historyCard.Id, historyCard.Count, historyCard.IsGolden)) => Date = historyCard.Date;

        private CardWrapper(string id, int count, bool isGolden, DateTime? date)
            : this(new Card(id, count, isGolden)) => Date = date;
        #endregion

        #region GetDustValue
        public int GetDustValue(int nCount = -1)
        {
            int nRet = RawCard.GetDustValue();

            if (nCount == -1 && MaxCountInDecks == 0)
            {
                nRet *= Math.Abs(RawCard.Count);
            }
            else if (nCount > -1)
            {
                nRet *= nCount;
            }

            return nRet;
        }
        #endregion

        #region Clone
        public object Clone()
        {
            return new CardWrapper(RawCard.Id, RawCard.Count, RawCard.Premium, Date);
        }
        #endregion
    }
}