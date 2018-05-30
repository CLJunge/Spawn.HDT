#region Using
using GalaSoft.MvvmLight.CommandWpf;
using System.Threading.Tasks;
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
        private bool m_blnRememberQueryString;
        #endregion

        #region Properties
        #region CanNotifyDirtyStatus
        public override bool CanNotifyDirtyStatus => false; //TODO -> true
        #endregion

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

        #region RememberQueryString
        public bool RememberQueryString
        {
            get => m_blnRememberQueryString;
            set => Set(ref m_blnRememberQueryString, value);
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
            ColoredCardItemsSettingVisibility = Visibility.Visible;
#else
            ColoredCardItemsSettingVisibility = Visibility.Collapsed;
#endif
        }
        #endregion

        #region InitializeAsync
        public override async Task InitializeAsync()
        {
            await Task.Delay(0);

            OfflineMode = DustUtilityPlugin.Config.OfflineMode;
            SaveInterval = DustUtilityPlugin.Config.SaveInterval;
            CheckForUpdates = DustUtilityPlugin.Config.CheckForUpdates;
            ColoredCardItems = DustUtilityPlugin.Config.ColoredCardItems;
            AutoDisenchanting = DustUtilityPlugin.Config.AutoDisenchanting;
            RememberQueryString = DustUtilityPlugin.Config.RememberQueryString;
        }
        #endregion

        #region SaveSettings
        private void SaveSettings()
        {
            DustUtilityPlugin.Config.OfflineMode = OfflineMode;
            DustUtilityPlugin.Config.CheckForUpdates = CheckForUpdates;
            DustUtilityPlugin.Config.ColoredCardItems = ColoredCardItems;
            DustUtilityPlugin.Config.AutoDisenchanting = AutoDisenchanting;
            DustUtilityPlugin.Config.RememberQueryString = RememberQueryString;

            if (OfflineMode)
            {
                DustUtilityPlugin.Config.SaveInterval = SaveInterval;
            }
            else { }
        }
        #endregion
    }
}