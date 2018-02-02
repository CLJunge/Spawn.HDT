#region Using
using HearthMirror.Objects;
using System;
using System.Diagnostics;
#endregion

namespace Spawn.HDT.DustUtility.Hearthstone
{
    [DebuggerDisplay("({Count}x) {DbCard.Name} Golden={RawCard.Premium}")]
    public class CardWrapper
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
        public HearthDb.Card DbCard => HearthDb.Cards.All[RawCard.Id];
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
        }

        public CardWrapper(string id, int count, bool isGolden, DateTime? date)
            : this(new Card(id, count, isGolden))
        {
            Date = date;
        }
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
            else { }

            return nRet;
        }
        #endregion
    }
}