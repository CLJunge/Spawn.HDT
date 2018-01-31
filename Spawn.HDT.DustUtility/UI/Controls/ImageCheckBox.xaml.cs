using System.Windows;
using System.Windows.Media;

namespace Spawn.HDT.DustUtility.UI.Controls
{
    public partial class ImageCheckBox
    {
        #region Properties
        #region Text DP
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ImageCheckBox), new PropertyMetadata(string.Empty));
        #endregion

        #region ImageSource DP
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImageCheckBox), new PropertyMetadata(null));
        #endregion

        #region IsChecked DP
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(ImageCheckBox), new PropertyMetadata(false));
        #endregion
        #endregion

        #region Ctor
        public ImageCheckBox()
        {
            InitializeComponent();
        }
        #endregion
    }
}