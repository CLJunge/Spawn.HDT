#region Using
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class SettingsDialogViewModel : ViewModelBase
    {
        #region Member Variables
        private string m_strWindowTitle;
        private Visibility m_coloredCardItemsSettingVisibility;

        private bool m_blnOfflineMode;
        private int m_nSaveInterval;
        private bool m_blnCheckForUpdates;
        private bool m_blnColoredCardItems;
        private bool m_blnAutoDisenchanting;
        #endregion

        #region Properties
        #region WindowTitle
        public string WindowTitle
        {
            get => m_strWindowTitle;
            set => Set(ref m_strWindowTitle, value);
        }
        #endregion

        #region ColoredCardItemsSettingVisibility
        public Visibility ColoredCardItemsSettingVisibility
        {
            get => m_coloredCardItemsSettingVisibility;
            set => Set(ref m_coloredCardItemsSettingVisibility, value);
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

        #region AutoDisenchanting
        public bool AutoDisenchanting
        {
            get => m_blnAutoDisenchanting;
            set => Set(ref m_blnAutoDisenchanting, value);
        }
        #endregion

        #region SaveSettingsCommand
        public ICommand SaveSettingsCommand => new RelayCommand(SaveSettings);
        #endregion

        #region OnCheckedCommand
        public ICommand OnCheckedCommand => new RelayCommand(OnChecked);
        #endregion
        #endregion

        #region Ctor
        public SettingsDialogViewModel()
        {
            WindowTitle = "Dust Utility - Settings";

            ColoredCardItemsSettingVisibility = Visibility.Collapsed;

#if DEBUG
            ColoredCardItemsSettingVisibility = Visibility.Visible;
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
            AutoDisenchanting = DustUtilityPlugin.Config.AutoDisenchanting;
        }
        #endregion

        #region SaveSettings
        private void SaveSettings()
        {
            DustUtilityPlugin.Config.OfflineMode = OfflineMode;
            DustUtilityPlugin.Config.CheckForUpdates = CheckForUpdates;
            DustUtilityPlugin.Config.ColoredCardItems = ColoredCardItems;
            DustUtilityPlugin.Config.AutoDisenchanting = AutoDisenchanting;

            if (OfflineMode)
            {
                DustUtilityPlugin.Config.SaveInterval = SaveInterval;
            }
            else { }
        }
        #endregion

        #region OnChecked
        private void OnChecked()
        {
            if (MessageBox.Show("You are using this feature at your own risk!\r\n\r\nThere is always a slight chance, that the wrong card might get disenchanted.\r\n\r\nAre you sure you want to enable auto disenchanting?", "Dust Utility", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                AutoDisenchanting = false;
            }
            else { }
        }
        #endregion
    }
}