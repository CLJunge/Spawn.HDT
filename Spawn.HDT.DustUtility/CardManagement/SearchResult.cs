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
    }
}