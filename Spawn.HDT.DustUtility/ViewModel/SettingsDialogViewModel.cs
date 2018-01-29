#region Using
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.ViewModel
{
    public class SettingsDialogViewModel : ViewModelBase
    {
        #region Member Variables
        private string m_strWindowTitle;
        private Visibility m_coloredCardItemsSetttingVisibility;

        private bool m_blnOfflineMode;
        private int m_nSaveInterval;
        private bool m_blnCheckForUpdates;
        private bool m_blnColoredCardItems;
        #endregion

        #region Properties
        #region WindowTitle
        public string WindowTitle
        {
            get => m_strWindowTitle;
            set => Set(ref m_strWindowTitle, value);
        }
        #endregion

        #region ColoredCardItemsSetttingVisibility
        public Visibility ColoredCardItemsSetttingVisibility
        {
            get => m_coloredCardItemsSetttingVisibility;
            set => Set(ref m_coloredCardItemsSetttingVisibility, value);
        }
        #endregion

        #region OfflineMode
        public bool OfflineMode
        {
            get => m_blnOfflineMode;
            set => Set(ref m_blnOfflineMode, value);
        }
        #endregion

        #region SaveInterval
        public int SaveInterval
        {
            get => m_nSaveInterval;
            set => Set(ref m_nSaveInterval, value);
        }
        #endregion

        #region CheckForUpdates
        public bool CheckForUpdates
        {
            get => m_blnCheckForUpdates;
            set => Set(ref m_blnCheckForUpdates, value);
        }
        #endregion

        #region ColoredCardItems
        public bool ColoredCardItems
        {
            get => m_blnColoredCardItems;
            set => Set(ref m_blnColoredCardItems, value);
        }
        #endregion

        #region SaveSettingsCommand
        public ICommand SaveSettingsCommand => new RelayCommand(SaveSettings);
        #endregion
        #endregion

        #region Ctor
        public SettingsDialogViewModel()
        {
            WindowTitle = "Dust Utility - Settings";

#if DEBUG
            ColoredCardItemsSetttingVisibility = Visibility.Visible;
#endif
        }
        #endregion

        #region Initialize
        public override void Initialize()
        {
            OfflineMode = DustUtilityPlugin.Config.OfflineMode;
            SaveInterval = DustUtilityPlugin.Config.SaveInterval;
            CheckForUpdates = DustUtilityPlugin.Config.CheckForUpdates;
            ColoredCardItems = DustUtilityPlugin.Config.ColoredCardItems;
        }
        #endregion

        #region SaveSettings
        private void SaveSettings()
        {
            DustUtilityPlugin.Config.OfflineMode = OfflineMode;
            DustUtilityPlugin.Config.CheckForUpdates = CheckForUpdates;
            DustUtilityPlugin.Config.ColoredCardItems = ColoredCardItems;

            if (OfflineMode)
            {
                DustUtilityPlugin.Config.SaveInterval = SaveInterval;
            }
            else { }
        }
        #endregion
    }
}