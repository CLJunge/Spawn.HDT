#region Using
using System.Windows;
#endregion

namespace Spawn.HDT.DustUtility.UI.Dialogs
{
    public partial class SettingsDialogView
    {
        #region Ctor
        public SettingsDialogView()
        {
            InitializeComponent();

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Created new 'SettingsDialogView' instance");
        }
        #endregion

        #region Events
        #region OnOkClick
        private void OnOkClick(object sender, RoutedEventArgs e) => DialogResult = true;
        #endregion

        #region OnAutoDisenchantingCheckBoxChecked
        private void OnAutoDisenchantingCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Showing auto disenchanting message box...");

            if (Visibility == Visibility.Visible && MessageBox.Show("You are using this feature at your own risk!\r\n\r\nThere is always a slight chance, that the wrong card might get disenchanted.\r\n\r\nAre you sure you want to enable auto disenchanting?", "Dust Utility - Auto Disenchanting", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                AutoDisenchantingCheckBox.IsChecked = false;
            }
        }
        #endregion
        #endregion
    }
}