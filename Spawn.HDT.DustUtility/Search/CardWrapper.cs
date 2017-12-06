using HearthMirror.Objects;
using Spawn.HDT.DustUtility.Offline;
using System;
using System.Diagnostics;

namespace Spawn.HDT.DustUtility.Search
{
    [DebuggerDisplay("{Count}x {DbCard.Name} Golden={Card.Premium}")]
    public class CardWrapper
    {
        #region Member Variables
        private Card m_card;
        private HearthDb.Card m_dbCard;
        #endregion

        #region Properties
        #region Card
        public Card Card => m_card;
        #endregion

        #region DbCard
        public HearthDb.Card DbCard => m_dbCard;
        #endregion

        #region MaxCountInDecks
        public int MaxCountInDecks { get; set; }
        #endregion

        #region Count
        public int Count => m_card.Count - MaxCountInDecks;
        #endregion
        #endregion

        #region Ctor
        public CardWrapper(Card card)
        {
            m_card = card;

            m_dbCard = HearthDb.Cards.All[m_card.Id];
        }
        #endregion

        #region GetDustValue
        public int GetDustValue(int nCount = -1)
        {
            int nRet = m_card.GetDustValue();

            if (nCount == -1 && MaxCountInDecks == 0)
            {
                nRet *= Math.Abs(m_card.Count);
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
            return $"{m_card.Id}:{m_card.Count}:{m_card.Premium}";
        }
        #endregion

        #region [STATIC] FromString
        public static CardWrapper FromString(string strValue)
        {
            string[] vValues = strValue.Split(':');

            Card card = new Card(vValues[0], Convert.ToInt32(vValues[1]), Convert.ToBoolean(vValues[2]));

            return new CardWrapper(card);
        }
        #endregion
    }

    [DebuggerDisplay("{Count}x {DbCard.Name} Golden={Card.Premium}")]
    public class CardWrapperEx : CardWrapper
    {
        #region Properties
        #region Timestamp
        public DateTime Timestamp { get; set; }
        #endregion
        #endregion

        #region Ctor
        public CardWrapperEx(Card card)
            : base(card)
        {
        }

        public CardWrapperEx(CachedCardEx card)
            : base(new Card(card.Id, card.Count, card.IsGolden))
        {
            Timestamp = card.Timestamp;
        }
        #endregion
    }
}