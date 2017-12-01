using System.Windows;
using System.Windows.Media;

namespace Spawn.HDT.DustUtility.UI.Components
{
    public partial class CardSetInfoContainer
    {
        #region DP
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

        #region CardSetName
        public string CardSetName
        {
            get { return (string)GetValue(CardSetNameProperty); }
            set { SetValue(CardSetNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CardSetName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CardSetNameProperty =
            DependencyProperty.Register("CardSetName", typeof(string), typeof(CardSetInfoContainer), new PropertyMetadata(string.Empty));
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