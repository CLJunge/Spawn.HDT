using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Spawn.HDT.DustUtility.UI.Components
{
    public partial class CheckBoxWithImage : UserControl
    {
        #region DP
        #region Text
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(CheckBoxWithImage), new PropertyMetadata(string.Empty));
        #endregion

        #region ImageSource
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(CheckBoxWithImage), new PropertyMetadata(null));
        #endregion

        #region ImageEffect
        public Effect ImageEffect
        {
            get { return (Effect)GetValue(ImageEffectProperty); }
            set { SetValue(ImageEffectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageEffect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageEffectProperty =
            DependencyProperty.Register("ImageEffect", typeof(Effect), typeof(CheckBoxWithImage), new PropertyMetadata());
        #endregion
        #endregion

        #region IsChecked
        public bool? IsChecked
        {
            get => checkBox.IsChecked;
            set => checkBox.IsChecked = value;
        }
        #endregion

        #region Ctor
        public CheckBoxWithImage()
        {
            InitializeComponent();
        }
        #endregion
    }
}