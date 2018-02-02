#region Using
using GalaSoft.MvvmLight;
using System.Windows.Media.Imaging;
#endregion

namespace Spawn.HDT.DustUtility.UI.Models
{
    public class CardSetItemModel : ObservableObject
    {
        #region Member Variables
        private BitmapImage m_logo;
        private BitmapImage m_backgroundImage;
        private string m_strName;
        private int m_nCommonsCount;
        private int m_nRaresCount;
        private int m_nEpicsCount;
        private int m_nLegendariesCount;
        private int m_nDustAmount;
        private int m_nGoldenCommonsCount;
        private int m_nGoldenRaresCount;
        private int m_nGoldenEpicsCount;
        private int m_nGoldenLegendariesCount;
        private int m_nGoldenDustAmount;
        private int m_nMaxCommonsCount;
        private int m_nMaxRaresCount;
        private int m_nMaxEpicsCount;
        private int m_nMaxLegendariesCount;
        #endregion

        #region Properties
        #region Logo
        public BitmapImage Logo
        {
            get => m_logo;
            set => Set(ref m_logo, value);
        }
        #endregion

        #region BackgroundImage
        public BitmapImage BackgroundImage
        {
            get => m_backgroundImage;
            set => Set(ref m_backgroundImage, value);
        }
        #endregion

        #region Name
        public string Name
        {
            get => m_strName;
            set => Set(ref m_strName, value);
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
        public int TotalCount
        {
            get => CommonsCount + RaresCount + EpicsCount + LegendariesCount;
        }
        #endregion

        #region DustAmount
        public int DustAmount
        {
            get => m_nDustAmount;
            set => Set(ref m_nDustAmount, value);
        }
        #endregion

        #region GoldenCommonsCount
        public int GoldenCommonsCount
        {
            get => m_nGoldenCommonsCount;
            set => Set(ref m_nGoldenCommonsCount, value);
        }
        #endregion

        #region GoldenRaresCount
        public int GoldenRaresCount
        {
            get => m_nGoldenRaresCount;
            set => Set(ref m_nGoldenRaresCount, value);
        }
        #endregion

        #region GoldenEpicsCount
        public int GoldenEpicsCount
        {
            get => m_nGoldenEpicsCount;
            set => Set(ref m_nGoldenEpicsCount, value);
        }
        #endregion

        #region GoldenLegendariesCount
        public int GoldenLegendariesCount
        {
            get => m_nGoldenLegendariesCount;
            set => Set(ref m_nGoldenLegendariesCount, value);
        }
        #endregion

        #region GoldenTotalCount
        public int GoldenTotalCount
        {
            get => GoldenCommonsCount + GoldenRaresCount + GoldenEpicsCount + GoldenLegendariesCount;
        }
        #endregion

        #region GoldenDustAmount
        public int GoldenDustAmount
        {
            get => m_nGoldenDustAmount;
            set => Set(ref m_nGoldenDustAmount, value);
        }
        #endregion

        #region MaxCommonsCount
        public int MaxCommonsCount
        {
            get => m_nMaxCommonsCount;
            set => Set(ref m_nMaxCommonsCount, value);
        }
        #endregion

        #region MaxRaresCount
        public int MaxRaresCount
        {
            get => m_nMaxRaresCount;
            set => Set(ref m_nMaxRaresCount, value);
        }
        #endregion

        #region MaxEpicsCount
        public int MaxEpicsCount
        {
            get => m_nMaxEpicsCount;
            set => Set(ref m_nMaxEpicsCount, value);
        }
        #endregion

        #region MaxLegendariesCount
        public int MaxLegendariesCount
        {
            get => m_nMaxLegendariesCount;
            set => Set(ref m_nMaxLegendariesCount, value);
        }
        #endregion

        #region MaxTotalCount
        public int MaxTotalCount
        {
            get => MaxCommonsCount + MaxRaresCount + MaxEpicsCount + MaxLegendariesCount;
        }
        #endregion
        #endregion

        #region Ctor
        public CardSetItemModel()
        {
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals(nameof(CommonsCount))
                || e.PropertyName.Equals(nameof(RaresCount))
                || e.PropertyName.Equals(nameof(EpicsCount))
                || e.PropertyName.Equals(nameof(LegendariesCount)))
                {
                    RaisePropertyChanged(nameof(TotalCount));
                }
                else if (e.PropertyName.Equals(nameof(GoldenCommonsCount))
                || e.PropertyName.Equals(nameof(GoldenRaresCount))
                || e.PropertyName.Equals(nameof(GoldenEpicsCount))
                || e.PropertyName.Equals(nameof(GoldenLegendariesCount)))
                {
                    RaisePropertyChanged(nameof(GoldenTotalCount));
                }
                else if (e.PropertyName.Equals(nameof(MaxCommonsCount))
                || e.PropertyName.Equals(nameof(MaxRaresCount))
                || e.PropertyName.Equals(nameof(MaxEpicsCount))
                || e.PropertyName.Equals(nameof(MaxLegendariesCount)))
                {
                    RaisePropertyChanged(nameof(MaxTotalCount));
                }
                else { }
            };
        }
        #endregion
    }
}