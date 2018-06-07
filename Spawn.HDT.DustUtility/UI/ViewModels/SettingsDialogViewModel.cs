#region Using
using GalaSoft.MvvmLight.CommandWpf;
using System.Threading.Tasks;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class SettingsDialogViewModel : ViewModelBase
    {
        #region Member Variables
        private string m_strWindowTitle;

        private bool m_blnOfflineMode;
        private string m_strOfflineModeLabelText;
        private int m_nSaveInterval;
        private string m_strSaveIntervalLabelText;
        private bool m_blnCheckForUpdates;
        private string m_strCheckForUpdatesLabelText;
        private bool m_blnColoredCardLabels;
        private string m_strColoredCardLabelsLabelText;
        private bool m_blnAutoDisenchanting;
        private string m_strAutoDisenchantingLabelText;
        private bool m_blnRememberQueryString;
        private string m_strRememberQueryStringLabelText;
        #endregion

        #region Properties
        #region CanNotifyDirtyStatus
        public override bool CanNotifyDirtyStatus => true;
        #endregion

        #region WindowTitle
        public string WindowTitle
        {
            get => m_strWindowTitle;
            set => Set(ref m_strWindowTitle, value);
        }
        #endregion

        #region OfflineMode
        public bool OfflineMode
        {
            get => m_blnOfflineMode;
            set => Set(ref m_blnOfflineMode, value);
        }
        #endregion

        #region OfflineModeLabelText
        public string OfflineModeLabelText
        {
            get => m_strOfflineModeLabelText;
            set => Set(ref m_strOfflineModeLabelText, value);
        }
        #endregion

        #region SaveInterval
        public int SaveInterval
        {
            get => m_nSaveInterval;
            set => Set(ref m_nSaveInterval, value);
        }
        #endregion

        #region SaveIntervalLabelText
        public string SaveIntervalLabelText
        {
            get => m_strSaveIntervalLabelText;
            set => Set(ref m_strSaveIntervalLabelText, value);
        }
        #endregion

        #region CheckForUpdates
        public bool CheckForUpdates
        {
            get => m_blnCheckForUpdates;
            set => Set(ref m_blnCheckForUpdates, value);
        }
        #endregion

        #region CheckForUpdatesLabelText
        public string CheckForUpdatesLabelText
        {
            get => m_strCheckForUpdatesLabelText;
            set => Set(ref m_strCheckForUpdatesLabelText, value);
        }
        #endregion

        #region ColoredCardLabels
        public bool ColoredCardLabels
        {
            get => m_blnColoredCardLabels;
            set => Set(ref m_blnColoredCardLabels, value);
        }
        #endregion

        #region ColoredCardLabelsLabelText
        public string ColoredCardLabelsLabelText
        {
            get => m_strColoredCardLabelsLabelText;
            set => Set(ref m_strColoredCardLabelsLabelText, value);
        }
        #endregion

        #region AutoDisenchanting
        public bool AutoDisenchanting
        {
            get => m_blnAutoDisenchanting;
            set => Set(ref m_blnAutoDisenchanting, value);
        }
        #endregion

        #region AutoDisenchantingLabelText
        public string AutoDisenchantingLabelText
        {
            get => m_strAutoDisenchantingLabelText;
            set => Set(ref m_strAutoDisenchantingLabelText, value);
        }
        #endregion

        #region RememberQueryString
        public bool RememberQueryString
        {
            get => m_blnRememberQueryString;
            set => Set(ref m_blnRememberQueryString, value);
        }
        #endregion

        #region RememberQueryStringLabelText
        public string RememberQueryStringLabelText
        {
            get => m_strRememberQueryStringLabelText;
            set => Set(ref m_strRememberQueryStringLabelText, value);
        }
        #endregion

        #region SaveSettingsCommand
        public ICommand SaveSettingsCommand => new RelayCommand(SaveSettings, () => IsDirty);
        #endregion
        #endregion

        #region Ctor
        public SettingsDialogViewModel()
        {
            WindowTitle = "Dust Utility - Settings";

            NotifyDirtyStatus += OnNotifyDirtyStatus;
        }
        #endregion

        #region Events
        #region OnNotifyDirtyStatus
        private void OnNotifyDirtyStatus(object sender, NotifyDirtyStatusEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(OfflineMode):
                    if (e.IsDirty)
                    {
                        OfflineModeLabelText = $"{OfflineModeLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        OfflineModeLabelText = OfflineModeLabelText.Substring(0, OfflineModeLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(SaveInterval):
                    if (e.IsDirty)
                    {
                        if (!SaveIntervalLabelText.EndsWith(IsDirtySuffix))
                        {
                            SaveIntervalLabelText = $"{SaveIntervalLabelText}{IsDirtySuffix}"; 
                        }
                        else { }
                    }
                    else
                    {
                        SaveIntervalLabelText = SaveIntervalLabelText.Substring(0, SaveIntervalLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(CheckForUpdates):
                    if (e.IsDirty)
                    {
                        CheckForUpdatesLabelText = $"{CheckForUpdatesLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        CheckForUpdatesLabelText = CheckForUpdatesLabelText.Substring(0, CheckForUpdatesLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(ColoredCardLabels):
                    if (e.IsDirty)
                    {
                        ColoredCardLabelsLabelText = $"{ColoredCardLabelsLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        ColoredCardLabelsLabelText = ColoredCardLabelsLabelText.Substring(0, ColoredCardLabelsLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(AutoDisenchanting):
                    if (e.IsDirty)
                    {
                        AutoDisenchantingLabelText = $"{AutoDisenchantingLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        AutoDisenchantingLabelText = AutoDisenchantingLabelText.Substring(0, AutoDisenchantingLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(RememberQueryString):
                    if (e.IsDirty)
                    {
                        RememberQueryStringLabelText = $"{RememberQueryStringLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        RememberQueryStringLabelText = RememberQueryStringLabelText.Substring(0, RememberQueryStringLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;
            }
        }
        #endregion
        #endregion

        #region InitializeAsync
        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            LoadLabelTexts();

            OfflineMode = DustUtilityPlugin.Config.OfflineMode;
            SaveInterval = DustUtilityPlugin.Config.SaveInterval;
            CheckForUpdates = DustUtilityPlugin.Config.CheckForUpdates;
            ColoredCardLabels = DustUtilityPlugin.Config.ColoredCardLabels;
            DustUtilityPlugin.SettingsDialog.AutoDisenchantingCheckBox.IsChecked = AutoDisenchanting = DustUtilityPlugin.Config.AutoDisenchanting;
            RememberQueryString = DustUtilityPlugin.Config.RememberQueryString;

            SetInitialPropertyValue(nameof(OfflineMode), OfflineMode);
            SetInitialPropertyValue(nameof(SaveInterval), SaveInterval);
            SetInitialPropertyValue(nameof(CheckForUpdates), CheckForUpdates);
            SetInitialPropertyValue(nameof(ColoredCardLabels), ColoredCardLabels);
            SetInitialPropertyValue(nameof(AutoDisenchanting), AutoDisenchanting);
            SetInitialPropertyValue(nameof(RememberQueryString), RememberQueryString);
        }
        #endregion

        #region SaveSettings
        private void SaveSettings()
        {
            DustUtilityPlugin.Config.OfflineMode = OfflineMode;
            DustUtilityPlugin.Config.CheckForUpdates = CheckForUpdates;
            DustUtilityPlugin.Config.ColoredCardLabels = ColoredCardLabels;
            DustUtilityPlugin.Config.AutoDisenchanting = AutoDisenchanting;
            DustUtilityPlugin.Config.RememberQueryString = RememberQueryString;

            if (OfflineMode)
            {
                DustUtilityPlugin.Config.SaveInterval = SaveInterval;
            }
            else { }
        }
        #endregion

        #region LoadLabelTexts
        private void LoadLabelTexts()
        {
            OfflineModeLabelText = "Offline Mode";
            SaveIntervalLabelText = "Save Interval (sec.)";
            CheckForUpdatesLabelText = "Check For Updates";
            ColoredCardLabelsLabelText = "Colored Card Labels";
            AutoDisenchantingLabelText = "Auto Disenchanting";
            RememberQueryStringLabelText = "Remember Search Term";
        }
        #endregion
    }
}