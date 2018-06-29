#region Using
using GalaSoft.MvvmLight.CommandWpf;
using Spawn.HDT.DustUtility.Logging;
using Spawn.HDT.DustUtility.Utilities;
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

        private bool m_blnOfflineMode;
        private string m_strOfflineModeLabelText;
        private int m_nSaveDelay;
        private TimeUnit m_saveDelayUnit;
        private string m_strSaveDelayLabelText;
        private bool m_blnCheckForUpdates;
        private string m_strCheckForUpdatesLabelText;
        private bool m_blnColoredCardLabels;
        private string m_strColoredCardLabelsLabelText;
        private bool m_blnAutoDisenchanting;
        private string m_strAutoDisenchantingLabelText;
        private bool m_blnRememberQueryString;
        private string m_strRememberQueryStringLabelText;
        private bool m_blnShowNotifications;
        private string m_strShowNotificationsLabelText;
        private LogLevel m_logLevel;
        private string m_strLogLevelLabelText;
        private ViewMode m_viewMode;
        private string m_strViewModeLabelText;
        private bool m_blnHideBattleTagId;
        private string m_strHideBattleTagIdLabelText;
        private bool m_blnEnableHistory;
        private string m_strEnableHistoryLabelText;
#if DEBUG
        private string m_strLoggableSources;
        private string m_strLoggableSourcesLabelText;
#endif
        private Visibility m_loggableSourcesVisibility = Visibility.Collapsed;
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

        #region SaveDelay
        public int SaveDelay
        {
            get => m_nSaveDelay;
            set => Set(ref m_nSaveDelay, value);
        }
        #endregion

        #region SaveDelayUnit
        public TimeUnit SaveDelayUnit
        {
            get => m_saveDelayUnit;
            set => Set(ref m_saveDelayUnit, value);
        }
        #endregion

        #region SaveDelayLabelText
        public string SaveDelayLabelText
        {
            get => m_strSaveDelayLabelText;
            set => Set(ref m_strSaveDelayLabelText, value);
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

        #region ShowNotifications
        public bool ShowNotifications
        {
            get => m_blnShowNotifications;
            set => Set(ref m_blnShowNotifications, value);
        }
        #endregion

        #region ShowNotificationsLabelText
        public string ShowNotificationsLabelText
        {
            get => m_strShowNotificationsLabelText;
            set => Set(ref m_strShowNotificationsLabelText, value);
        }
        #endregion

        #region LogLevel
        public LogLevel LogLevel
        {
            get => m_logLevel;
            set => Set(ref m_logLevel, value);
        }
        #endregion

        #region LogLevelLabelText
        public string LogLevelLabelText
        {
            get => m_strLogLevelLabelText;
            set => Set(ref m_strLogLevelLabelText, value);
        }
        #endregion

        #region ViewMode
        public ViewMode ViewMode
        {
            get => m_viewMode;
            set => Set(ref m_viewMode, value);
        }
        #endregion

        #region ViewModeLabelText
        public string ViewModeLabelText
        {
            get => m_strViewModeLabelText;
            set => Set(ref m_strViewModeLabelText, value);
        }
        #endregion

        #region HideBattleTagId
        public bool HideBattleTagId
        {
            get => m_blnHideBattleTagId;
            set => Set(ref m_blnHideBattleTagId, value);
        }
        #endregion

        #region HideBattleTagIdLabelText
        public string HideBattleTagIdLabelText
        {
            get => m_strHideBattleTagIdLabelText;
            set => Set(ref m_strHideBattleTagIdLabelText, value);
        }
        #endregion

        #region EnableHistory
        public bool EnableHistory
        {
            get => m_blnEnableHistory;
            set => Set(ref m_blnEnableHistory, value);
        }
        #endregion

        #region EnableHistoryLabelText
        public string EnableHistoryLabelText
        {
            get => m_strEnableHistoryLabelText;
            set => Set(ref m_strEnableHistoryLabelText, value);
        }
        #endregion

#if DEBUG
        #region LoggableSources
        public string LoggableSources
        {
            get => m_strLoggableSources;
            set => Set(ref m_strLoggableSources, value);
        }
        #endregion

        #region LoggableSourcesLabelText
        public string LoggableSourcesLabelText
        {
            get => m_strLoggableSourcesLabelText;
            set => Set(ref m_strLoggableSourcesLabelText, value);
        }
        #endregion
#endif

        #region LoggableSourcesVisibility
        public Visibility LoggableSourcesVisibility
        {
            get => m_loggableSourcesVisibility;
            set => Set(ref m_loggableSourcesVisibility, value);
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

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Created new 'SettingsDialogViewModel' instance");

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals(nameof(OfflineMode)) && !OfflineMode)
                {
                    SaveDelay = DustUtilityPlugin.Config.SaveDelay;
                    SaveDelayUnit = DustUtilityPlugin.Config.SaveDelayUnit;
                    EnableHistory = DustUtilityPlugin.Config.EnableHistory;
                }
            };
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

                case nameof(SaveDelay):
                case nameof(SaveDelayUnit):
                    if (e.IsDirty && !SaveDelayLabelText.EndsWith(IsDirtySuffix))
                    {
                        SaveDelayLabelText = $"{SaveDelayLabelText}{IsDirtySuffix}";
                    }
                    else if (!e.IsDirty && SaveDelayLabelText.EndsWith(IsDirtySuffix))
                    {
                        SaveDelayLabelText = SaveDelayLabelText.Substring(0, SaveDelayLabelText.Length - IsDirtySuffix.Length);
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

                case nameof(ShowNotifications):
                    if (e.IsDirty)
                    {
                        ShowNotificationsLabelText = $"{ShowNotificationsLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        ShowNotificationsLabelText = ShowNotificationsLabelText.Substring(0, ShowNotificationsLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(LogLevel):
                    if (e.IsDirty && !LogLevelLabelText.EndsWith(IsDirtySuffix))
                    {
                        LogLevelLabelText = $"{LogLevelLabelText}{IsDirtySuffix}";
                    }
                    else if (!e.IsDirty)
                    {
                        LogLevelLabelText = LogLevelLabelText.Substring(0, LogLevelLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(ViewMode):
                    if (e.IsDirty)
                    {
                        ViewModeLabelText = $"{ViewModeLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        ViewModeLabelText = ViewModeLabelText.Substring(0, ViewModeLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(HideBattleTagId):
                    if (e.IsDirty)
                    {
                        HideBattleTagIdLabelText = $"{HideBattleTagIdLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        HideBattleTagIdLabelText = HideBattleTagIdLabelText.Substring(0, HideBattleTagIdLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(EnableHistory):
                    if (e.IsDirty)
                    {
                        EnableHistoryLabelText = $"{EnableHistoryLabelText}{IsDirtySuffix}";
                    }
                    else
                    {
                        EnableHistoryLabelText = EnableHistoryLabelText.Substring(0, EnableHistoryLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

#if DEBUG
                case nameof(LoggableSources):
                    if (e.IsDirty && !LoggableSourcesLabelText.EndsWith(IsDirtySuffix))
                    {
                        LoggableSourcesLabelText = $"{LoggableSourcesLabelText}{IsDirtySuffix}";
                    }
                    else if (!e.IsDirty)
                    {
                        LoggableSourcesLabelText = LoggableSourcesLabelText.Substring(0, LoggableSourcesLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;
#endif
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
            SaveDelay = DustUtilityPlugin.Config.SaveDelay;
            SaveDelayUnit = DustUtilityPlugin.Config.SaveDelayUnit;
            CheckForUpdates = DustUtilityPlugin.Config.CheckForUpdates;
            ColoredCardLabels = DustUtilityPlugin.Config.ColoredCardLabels;
            DustUtilityPlugin.SettingsDialog.AutoDisenchantingCheckBox.IsChecked = AutoDisenchanting = DustUtilityPlugin.Config.AutoDisenchanting;
            RememberQueryString = DustUtilityPlugin.Config.RememberQueryString;
            ShowNotifications = DustUtilityPlugin.Config.ShowNotifications;
            LogLevel = DustUtilityPlugin.Config.LogLevel;
            ViewMode = DustUtilityPlugin.Config.ViewMode;
            HideBattleTagId = DustUtilityPlugin.Config.HideBattleTagId;
            EnableHistory = DustUtilityPlugin.Config.EnableHistory;

            SetInitialPropertyValue(nameof(OfflineMode), OfflineMode);
            SetInitialPropertyValue(nameof(SaveDelay), SaveDelay);
            SetInitialPropertyValue(nameof(SaveDelayUnit), SaveDelayUnit);
            SetInitialPropertyValue(nameof(CheckForUpdates), CheckForUpdates);
            SetInitialPropertyValue(nameof(ColoredCardLabels), ColoredCardLabels);
            SetInitialPropertyValue(nameof(AutoDisenchanting), AutoDisenchanting);
            SetInitialPropertyValue(nameof(RememberQueryString), RememberQueryString);
            SetInitialPropertyValue(nameof(ShowNotifications), ShowNotifications);
            SetInitialPropertyValue(nameof(LogLevel), LogLevel);
            SetInitialPropertyValue(nameof(ViewMode), ViewMode);
            SetInitialPropertyValue(nameof(HideBattleTagId), HideBattleTagId);
            SetInitialPropertyValue(nameof(EnableHistory), EnableHistory);

#if DEBUG
            LoggableSourcesVisibility = Visibility.Visible;
            LoggableSources = DustUtilityPlugin.Config.LoggableSources;
            SetInitialPropertyValue(nameof(LoggableSources), LoggableSources);
#endif

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Finished initializing");
        }
        #endregion

        #region SaveSettings
        private void SaveSettings()
        {
            DustUtilityPlugin.Config.OfflineMode = OfflineMode;
            DustUtilityPlugin.Config.SaveDelay = SaveDelay;
            DustUtilityPlugin.Config.SaveDelayUnit = SaveDelayUnit;
            DustUtilityPlugin.Config.CheckForUpdates = CheckForUpdates;
            DustUtilityPlugin.Config.ColoredCardLabels = ColoredCardLabels;
            DustUtilityPlugin.Config.AutoDisenchanting = AutoDisenchanting;
            DustUtilityPlugin.Config.RememberQueryString = RememberQueryString;
            DustUtilityPlugin.Config.ShowNotifications = ShowNotifications;
            DustUtilityPlugin.Config.LogLevel = LogLevel;
            DustUtilityPlugin.Config.ViewMode = ViewMode;
            DustUtilityPlugin.Config.HideBattleTagId = HideBattleTagId;
            DustUtilityPlugin.Config.EnableHistory = EnableHistory;
#if DEBUG
            DustUtilityPlugin.Config.LoggableSources = LoggableSources;
#endif

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Saved settings");
        }
        #endregion

        #region LoadLabelTexts
        private void LoadLabelTexts()
        {
            OfflineModeLabelText = "Enable offline mode";
            SaveDelayLabelText = "Save delay";
            CheckForUpdatesLabelText = "Check for updates";
            ColoredCardLabelsLabelText = "Use colored card labels";
            AutoDisenchantingLabelText = "Enable auto disenchanting";
            RememberQueryStringLabelText = "Remember search term";
            ShowNotificationsLabelText = "Show notifications";
            LogLevelLabelText = "Log level";
            ViewModeLabelText = "View mode";
            HideBattleTagIdLabelText = "Hide BattleTag id";
            EnableHistoryLabelText = "Enable history";
#if DEBUG
            LoggableSourcesLabelText = "Loggable sources";
#endif

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Loaded label texts");
        }
        #endregion
    }
}