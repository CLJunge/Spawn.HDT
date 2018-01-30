using GalaSoft.MvvmLight;

namespace Spawn.HDT.DustUtility.UI.Models
{
    public class CardsInfoModel : ObservableObject
    {
        #region Member Variables
        private int m_nDustAmount;
        private int m_nCommonsCount;
        private int m_nRaresCount;
        private int m_nEpicsCount;
        private int m_nLegendariesCount;
        #endregion

        #region Properties
        #region DustAmount
        public int DustAmount
        {
            get => m_nDustAmount;
            set => Set(ref m_nDustAmount, value);
        }
        #endregion

        #region CommonsCount
        public int CommonsCount
        {
            get => m_nCommonsCount;
            set => Set(ref m_nCommonsCount, value);
        }
        #endregion

        #region RaresCount
        public int RaresCount
        {
            get => m_nRaresCount;
            set => Set(ref m_nRaresCount, value);
        }
        #endregion

        #region EpicsCount
        public int EpicsCount
        {
            get => m_nEpicsCount;
            set => Set(ref m_nEpicsCount, value);
        }
        #endregion

        #region LegendariesCount
        public int LegendariesCount
        {
            get => m_nLegendariesCount;
            set => Set(ref m_nLegendariesCount, value);
        }
        #endregion

        #region TotalCount
        public int TotalCount => CommonsCount + RaresCount + EpicsCount + LegendariesCount;
        #endregion
        #endregion
    }
}