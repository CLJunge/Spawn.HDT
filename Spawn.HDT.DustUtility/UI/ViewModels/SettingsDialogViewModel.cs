﻿#region Using
using GalaSoft.MvvmLight.CommandWpf;
using Spawn.HDT.DustUtility.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        #region LogLevels
#pragma warning disable S2365 // Properties should not make collection or array copies
        public IList<LogLevel> LogLevels => Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>().ToList();
#pragma warning restore S2365 // Properties should not make collection or array copies
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

        #region ViewModes
#pragma warning disable S2365 // Properties should not make collection or array copies
        public IList<ViewMode> ViewModes => Enum.GetValues(typeof(ViewMode)).Cast<ViewMode>().ToList();
#pragma warning restore S2365 // Properties should not make collection or array copies
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
            CheckForUpdates = DustUtilityPlugin.Config.CheckForUpdates;
            ColoredCardLabels = DustUtilityPlugin.Config.ColoredCardLabels;
            DustUtilityPlugin.SettingsDialog.AutoDisenchantingCheckBox.IsChecked = AutoDisenchanting = DustUtilityPlugin.Config.AutoDisenchanting;
            RememberQueryString = DustUtilityPlugin.Config.RememberQueryString;
            ShowNotifications = DustUtilityPlugin.Config.ShowNotifications;
            LogLevel = DustUtilityPlugin.Config.LogLevel;
            ViewMode = DustUtilityPlugin.Config.ViewMode;

            SetInitialPropertyValue(nameof(OfflineMode), OfflineMode);
            SetInitialPropertyValue(nameof(CheckForUpdates), CheckForUpdates);
            SetInitialPropertyValue(nameof(ColoredCardLabels), ColoredCardLabels);
            SetInitialPropertyValue(nameof(AutoDisenchanting), AutoDisenchanting);
            SetInitialPropertyValue(nameof(RememberQueryString), RememberQueryString);
            SetInitialPropertyValue(nameof(ShowNotifications), ShowNotifications);
            SetInitialPropertyValue(nameof(LogLevel), LogLevel);
            SetInitialPropertyValue(nameof(ViewMode), ViewMode);
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
            DustUtilityPlugin.Config.ShowNotifications = ShowNotifications;
            DustUtilityPlugin.Config.LogLevel = LogLevel;
            DustUtilityPlugin.Config.ViewMode = ViewMode;
        }
        #endregion

        #region LoadLabelTexts
        private void LoadLabelTexts()
        {
            OfflineModeLabelText = "Offline Mode";
            CheckForUpdatesLabelText = "Check For Updates";
            ColoredCardLabelsLabelText = "Colored Card Labels";
            AutoDisenchantingLabelText = "Auto Disenchanting";
            RememberQueryStringLabelText = "Remember Search Term";
            ShowNotificationsLabelText = "Show Notifications";
            LogLevelLabelText = "Log Level";
            ViewModeLabelText = "View Mode";
        }
        #endregion
    }
}