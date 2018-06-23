#region Using
using System.Windows;
#endregion

namespace Spawn.HDT.DustUtility.UI.Controls
{
    public partial class CardsInfoView
    {
        #region DP Title
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CardsInfoView), new PropertyMetadata("Info"));
        #endregion

        #region Ctor
        public CardsInfoView()
        {
            InitializeComponent();
        }
        #endregion
    }
}