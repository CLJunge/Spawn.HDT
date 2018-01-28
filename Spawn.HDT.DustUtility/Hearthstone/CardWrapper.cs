using HearthMirror.Objects;
using Spawn.HDT.DustUtility.Offline;
using System;
using System.Diagnostics;

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

        #region Timestamp
        public DateTime? Timestamp { get; set; }
        #endregion
        #endregion

        #region Ctor
        public CardWrapper(Card card)
        {
            RawCard = card;
        }

        public CardWrapper(string cardString)
            : this(ParseCardString(cardString))
        {
        }

        public CardWrapper(CachedHistoryCard cachedCard)
            : this(new Card(cachedCard.Id, cachedCard.Count, cachedCard.IsGolden))
        {
            Timestamp = cachedCard.Timestamp;
        }
        #endregion

        #region GetDustValue
        private int GetDustValue(int nCount = -1)
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

        #region ToString
        public override string ToString()
        {
            //id count premium
            return $"{RawCard.Id}:{RawCard.Count}:{RawCard.Premium}";
        }
        #endregion

        #region [STATIC] ParseCardString
        private static Card ParseCardString(string strCardString)
        {
            string[] vValues = strCardString.Split(':');

            return new Card(vValues[0], Convert.ToInt32(vValues[1]), Convert.ToBoolean(vValues[2]));
        }
        #endregion
    }
}