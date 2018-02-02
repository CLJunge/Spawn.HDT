using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Spawn.HDT.DustUtility.Hearthstone;
using Spawn.HDT.DustUtility.UI.Models;
using System.Collections.Generic;

namespace Spawn.HDT.DustUtility.CardManagement
{
    public class SearchResult
    {
        #region Properties
        #region DustAmount
        public int DustAmount { get; set; }
        #endregion

        #region CommonsCount
        public int CommonsCount { get; set; }
        #endregion

        #region RaresCount
        public int RaresCount { get; set; }
        #endregion

        #region EpicsCount
        public int EpicsCount { get; set; }
        #endregion

        #region LegendariesCount
        public int LegendariesCount { get; set; }
        #endregion

        #region CardItems
        public List<CardItemModel> CardItems { get; set; }
        #endregion
        #endregion

        #region Ctor
        public SearchResult()
        {
            CardItems = new List<CardItemModel>();
        }
        #endregion

        #region ToCardsInfoModel
        public void CopyToCardsInfoModel(CardsInfoModel info)
        {
            info.DustAmount = DustAmount;
            info.CommonsCount = CommonsCount;
            info.RaresCount = RaresCount;
            info.EpicsCount = EpicsCount;
            info.LegendariesCount = LegendariesCount;
        }
        #endregion

        #region [STATIC] Create
        public static SearchResult Create(List<CardWrapper> lstCards)
        {
            SearchResult retVal = new SearchResult();

            Log.WriteLine("Creating new search result...", LogType.Debug);

            for (int i = 0; i < lstCards.Count; i++)
            {
                CardWrapper wrapper = lstCards[i];

                switch (wrapper.DbCard.Rarity)
                {
                    case Rarity.COMMON:
                        retVal.CommonsCount += wrapper.Count;
                        break;

                    case Rarity.RARE:
                        retVal.RaresCount += wrapper.Count;
                        break;

                    case Rarity.EPIC:
                        retVal.EpicsCount += wrapper.Count;
                        break;

                    case Rarity.LEGENDARY:
                        retVal.LegendariesCount += wrapper.Count;
                        break;
                }

                CardItemModel item = new CardItemModel(wrapper);

                retVal.DustAmount += item.Dust;

                retVal.CardItems.Add(item);
            }

            return retVal;
        }
        #endregion
    }
}