using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Windows;

namespace Spawn.HDT.DustUtility.UI.Dialogs
{
    public partial class SettingsDialog
    {
        #region Ctor
        public SettingsDialog()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        #region OnWindowLoaded
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            cbOfflineMode.IsChecked = Settings.OfflineMode;
            tbSaveInterval.Text = Settings.SaveInterval.ToString();
            cbCheckForUpdates.IsChecked = Settings.CheckForUpdate;
        }
        #endregion

        #region OnOkClick
        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbOfflineMode.IsChecked.Value)
                {
                    Settings.SaveInterval = Math.Abs(Convert.ToInt32(tbSaveInterval.Text));
                }
                else { }

                Settings.OfflineMode = cbOfflineMode.IsChecked.Value;
                Settings.CheckForUpdate = cbCheckForUpdates.IsChecked.Value;

                Log.WriteLine($"OfflineMode={Settings.OfflineMode}", LogType.Debug);
                Log.WriteLine($"SaveInterval={Settings.SaveInterval}", LogType.Debug);
                Log.WriteLine($"CheckForUpdate={Settings.CheckForUpdate}", LogType.Debug);

                Log.WriteLine("Saved settings", LogType.Info);

                DialogResult = true;

                Close();
            }
            catch (Exception ex)
            {
                Log.WriteLine($"Exception occured while saving settings: {ex}", LogType.Error);

                MessageBox.Show("Error occured while saving settings! Check log for more information.", "Dust Utility - Settings");
            }
        }
        #endregion

        #region OnCancelClick
        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Log.WriteLine("No settings saved", LogType.Debug);

            Close();
        }
        #endregion

        #region OnSaveIntervalPreviewTextInput
        private void OnSaveIntervalPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !Search.CardManager.NumericRegex.IsMatch(e.Text);
        }
        #endregion
        #endregion
    }
}