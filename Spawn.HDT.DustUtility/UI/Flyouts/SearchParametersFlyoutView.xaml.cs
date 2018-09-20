#region Using
using System.Windows;
using System.Windows.Controls;
#endregion

namespace Spawn.HDT.DustUtility.UI.Flyouts
{
    public partial class SearchParametersFlyoutView
    {
        #region Ctor
        public SearchParametersFlyoutView()
        {
            InitializeComponent();

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Created new 'SearchParametersFlyoutView' instance");
        }
        #endregion

        #region Events
        #region OnCheckBoxAllChecked
        private void OnCheckBoxAllChecked(object sender, RoutedEventArgs e)
        {
            GroupBox groupBox = (sender as FrameworkElement).FindParent<GroupBox>();

            if (groupBox != null && groupBox.Content is Grid gbContent)
            {
                for (int i = 0; i < gbContent.Children.Count; i++)
                {
                    if (gbContent.Children[i] is CheckBox checkBox)
                    {
                        checkBox.IsChecked = true;
                    }
                }
            }
        }
        #endregion

        #region OnCheckBoxAllUnchecked
        private void OnCheckBoxAllUnchecked(object sender, RoutedEventArgs e)
        {
            GroupBox groupBox = (sender as FrameworkElement).FindParent<GroupBox>();

            if (groupBox != null && groupBox.Content is Grid gbContent)
            {
                for (int i = 0; i < gbContent.Children.Count; i++)
                {
                    if (gbContent.Children[i] is CheckBox checkBox)
                    {
                        checkBox.IsChecked = false;
                    }
                }
            }
        }
        #endregion
        #endregion
    }
}