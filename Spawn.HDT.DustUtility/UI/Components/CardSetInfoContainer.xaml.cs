using System.Windows;
using System.Windows.Media;

namespace Spawn.HDT.DustUtility.UI.Components
{
    public partial class CardSetInfoContainer
    {
        #region DP
        #region Banner
        public ImageSource Banner
        {
            get { return (ImageSource)GetValue(BannerProperty); }
            set { SetValue(BannerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Banner.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BannerProperty =
            DependencyProperty.Register("Banner", typeof(ImageSource), typeof(CardSetInfoContainer), new PropertyMetadata(null));
        #endregion

        #region Logo
        public ImageSource Logo
        {
            get { return (ImageSource)GetValue(LogoProperty); }
            set { SetValue(LogoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Logo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LogoProperty =
            DependencyProperty.Register("Logo", typeof(ImageSource), typeof(CardSetInfoContainer), new PropertyMetadata(null));
        #endregion

        #region TotalCount
        public int TotalCount
        {
            get { return (int)GetValue(TotalCountProperty); }
            set { SetValue(TotalCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalCountProperty =
            DependencyProperty.Register("TotalCount", typeof(int), typeof(CardSetInfoContainer), new PropertyMetadata(0));
        #endregion

        #region CommonsCount
        public int CommonsCount
        {
            get { return (int)GetValue(CommonsCountProperty); }
            set { SetValue(CommonsCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommonsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommonsCountProperty =
            DependencyProperty.Register("CommonsCount", typeof(int), typeof(CardSetInfoContainer), new PropertyMetadata(0));
        #endregion

        #region RaresCount
        public int RaresCount
        {
            get { return (int)GetValue(RaresCountProperty); }
            set { SetValue(RaresCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RaresCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RaresCountProperty =
            DependencyProperty.Register("RaresCount", typeof(int), typeof(CardSetInfoContainer), new PropertyMetadata(0));
        #endregion

        #region EpicsCount
        public int EpicsCount
        {
            get { return (int)GetValue(EpicsCountProperty); }
            set { SetValue(EpicsCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EpicsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EpicsCountProperty =
            DependencyProperty.Register("EpicsCount", typeof(int), typeof(CardSetInfoContainer), new PropertyMetadata(0));
        #endregion

        #region LegendariesCount
        public int LegendariesCount
        {
            get { return (int)GetValue(LegendariesCountProperty); }
            set { SetValue(LegendariesCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LegendariesCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LegendariesCountProperty =
            DependencyProperty.Register("LegendariesCount", typeof(int), typeof(CardSetInfoContainer), new PropertyMetadata(0));
        #endregion

        #region DustValue
        public string DustValue
        {
            get { return (string)GetValue(DustValueProperty); }
            set { SetValue(DustValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DustValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DustValueProperty =
            DependencyProperty.Register("DustValue", typeof(string), typeof(CardSetInfoContainer), new PropertyMetadata("0 Dust"));
        #endregion
        #endregion

        #region Ctor
        public CardSetInfoContainer()
        {
            InitializeComponent();
        }
        #endregion
    }
}