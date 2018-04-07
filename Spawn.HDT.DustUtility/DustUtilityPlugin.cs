#region Using
using GalaSoft.MvvmLight.Ioc;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Microsoft.Practices.ServiceLocation;
using Spawn.HDT.DustUtility.AccountManagement;
using Spawn.HDT.DustUtility.CardManagement.Offline;
using Spawn.HDT.DustUtility.UI.Dialogs;
using Spawn.HDT.DustUtility.UI.ViewModels;
using Spawn.HDT.DustUtility.UI.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
#endregion

namespace Spawn.HDT.DustUtility
{
    public class DustUtilityPlugin : IPlugin
    {
        #region Static Fields
        private static MainWindow s_mainWindow;
        private static bool s_blnInitialized;
        private static bool s_blnIsOffline;
#if DEBUG
        private static IAccount s_mockAcc = new MockAccount();
#endif
        #endregion

        #region Static Properties
        #region DataDirectory
        public static string DataDirectory => GetDataDirectory();
        #endregion

        #region BackupsDirectory
        public static string BackupsDirectory => GetDataDirectory("Backups");
        #endregion

        #region AccountsDirectory
        public static string AccountsDirectory => GetDataDirectory("Accounts");
        #endregion

        #region IsOffline
        public static bool IsOffline
        {
            get => s_blnIsOffline;
            private set
            {
                if (s_blnIsOffline != value)
                {
                    s_blnIsOffline = value;

                    IsOfflineChanged?.Invoke(null, EventArgs.Empty);
                }
                else { }
            }
        }
        #endregion

        #region MainWindow
        public static MainWindow MainWindow => s_mainWindow;
        #endregion

        #region HideMainWindowOnClose
        public static bool HideMainWindowOnClose { get; private set; }
        #endregion

        #region CurrentAccount
        public static IAccount CurrentAccount => ServiceLocator.Current.GetInstance<IAccount>();
        #endregion

        #region Config
        public static Configuration Config => ServiceLocator.Current.GetInstance<Configuration>();
        #endregion

        #region NumericRegex
        public static Regex NumericRegex => new Regex("^(0|[1-9][0-9]*)$");
        #endregion

        #region RarityBrushes
        public static Dictionary<int, SolidColorBrush> RarityBrushes { get; }
        #endregion
        #endregion

        #region Properties
        #region Name
        public string Name => "Dust Utility";
        #endregion

        #region Description
        public string Description => "Utility tool for dust management. Check GitHub readme for a detailed description and a list of all features.";
        #endregion

        #region ButtonText
        public string ButtonText => "Settings...";
        #endregion

        #region Author
        public string Author => "CLJunge";
        #endregion

        #region Version
        public Version Version => new Version(Assembly.GetExecutingAssembly().GetName().Version.ToString(3));
        #endregion

        #region MenuItem
        public MenuItem MenuItem => CreateMenuItem();
        #endregion
        #endregion

        #region Static Ctor
        static DustUtilityPlugin()
        {
            CreateContainer();

            RarityBrushes = new Dictionary<int, SolidColorBrush>
            {
                //Common
                { 1, new SolidColorBrush(Color.FromRgb(38, 168, 16)) },
                //Rare
                //s_dColors.Add(3, new SolidColorBrush(Color.FromRgb(18, 92, 204)));
                { 3, new SolidColorBrush(Color.FromRgb(30, 113, 255)) },
                //Epic
                //s_dColors.Add(4, new SolidColorBrush(Color.FromRgb(135, 14, 186)));
                { 4, new SolidColorBrush(Color.FromRgb(163, 58, 234)) },
                //Legendary
                //s_dColors.Add(5, new SolidColorBrush(Color.FromRgb(226, 119, 24)));
                { 5, new SolidColorBrush(Color.FromRgb(255, 153, 0)) }
            };
        }
        #endregion

        #region Custom Events
        #region [STATIC] IsOfflineChanged
        public static event EventHandler IsOfflineChanged;
        #endregion
        #endregion

        #region IPlugin Methods
        #region OnLoad
        public void OnLoad()
        {
            s_blnInitialized = false;

            HideMainWindowOnClose = true;

            CreateContainer();

            IsOfflineChanged += OnIsOfflineChanged;

            Task.Run(() => UpdatePluginFiles()).ContinueWith(t =>
            {
                if (Config.OfflineMode && Core.Game.IsRunning)
                {
                    Cache.StartTimer();
                }
                else { }

                s_blnInitialized = true;
            });
        }
        #endregion

        #region OnButtonPress
        public void OnButtonPress()
        {
            ShowSettingsDialog();
        }
        #endregion

        #region OnUnload
        public void OnUnload()
        {
            HideMainWindowOnClose = false;

            if (MainWindow != null)
            {
                MainWindow.Close();
            }
            else { }

            s_mainWindow = null;

            if (Cache.TimerEnabled)
            {
                Cache.StopTimer();
            }
            else { }

            Config.Save();

            CurrentAccount.SavePreferences();

            SimpleIoc.Default.Reset();

            ServiceLocator.SetLocatorProvider(null);
        }
        #endregion

        #region OnUpdate
        public void OnUpdate()
        {
            IsOffline = !Core.Game.IsRunning && Config.OfflineMode;
        }
        #endregion
        #endregion

        #region Events
        #region OnMenuItemClick
        private void OnMenuItemClick(object sender, RoutedEventArgs e)
        {
            if (s_blnInitialized)
            {
                if (Core.Game.IsRunning || Config.OfflineMode)
                {
                    if (!CurrentAccount.IsValid)
                    {
                        IAccount selectedAcc = SelectAccount(false);

                        if (selectedAcc != null)
                        {
                            UpdatedAccountInstance(selectedAcc);
                        }
                        else { }
                    }
                    else { }

                    if (CurrentAccount.IsValid)
                    {
                        ShowMainWindow();
                    }
                    else { }
                }
                else if (!Config.OfflineMode)
                {
                    MessageBox.Show("Hearthstone isn't running!", Name, MessageBoxButton.OK, MessageBoxImage.Warning);

                    Log.WriteLine("Hearthstone isn't running", LogType.Warning);
                }
                else { }
            }
            else
            {
                string strMessage = "Plugin is still initializing...";

                if (Config.Version == 1)
                {
                    strMessage = $"{strMessage} (Updating account files to new format)";
                }
                else { }

                MessageBox.Show(strMessage, Name, MessageBoxButton.OK, MessageBoxImage.Information);

                Log.WriteLine("Plugin is not initialized", LogType.Info);
            }
        }
        #endregion

        #region OnIsOfflineChanged
        private void OnIsOfflineChanged(object sender, EventArgs e)
        {
            if (!IsOffline && !ServiceLocator.Current.GetInstance<IAccount>().Equals(Account.LoggedInAccount))
            {
                UpdatedAccountInstance(Account.LoggedInAccount);
            }
            else { }

            if (MainWindow != null)
            {
                ServiceLocator.Current.GetInstance<MainViewModel>().Initialize();
            }
            else { }

            if (Config.OfflineMode && (Core.Game.IsRunning && !Cache.TimerEnabled))
            {
                Cache.StartTimer();
            }
            else if (!Core.Game.IsRunning && Cache.TimerEnabled)
            {
                Cache.StopTimer();
            }
            else { }
        }
        #endregion
        #endregion

        #region CreateMenuItem
        private MenuItem CreateMenuItem()
        {
            MenuItem retVal = new MenuItem()
            {
                Header = Name,
                Icon = new Image()
                {
                    Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Spawn.HDT.DustUtility;component/Resources/icon.png", UriKind.Relative))
                }
            };

            retVal.Click += OnMenuItemClick;

            return retVal;
        }
        #endregion

        #region ShowSettingsDialog
        private void ShowSettingsDialog()
        {
            Log.WriteLine("Opening settings dialog", LogType.Debug);

            ServiceLocator.Current.GetInstance<SettingsDialogViewModel>().Initialize();

            SettingsDialogView dialog = new SettingsDialogView()
            {
                Owner = Core.MainWindow
            };

            //TODO: still buggy, dialog pops up multiple timess
            dialog.AutoDisenchantingCheckBox.Checked += (s, e) =>
            {
                if (MessageBox.Show("You are using this feature at your own risk!\r\n\r\nThere is always a slight chance, that the wrong card might get disenchanted.\r\n\r\nAre you sure you want to enable auto disenchanting?", "Dust Utility", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    ServiceLocator.Current.GetInstance<SettingsDialogViewModel>().AutoDisenchanting = false;
                }
                else { }
            };

            if (dialog.ShowDialog().Value)
            {
                if (Config.OfflineMode && Core.Game.IsRunning)
                {
                    if (!Cache.TimerEnabled)
                    {
                        Cache.StartTimer();
                    }
                    else
                    {
                        //Reinitialize timer with new interval
                        Cache.StopTimer();

                        Cache.StartTimer();
                    }
                }
                else if (!Config.OfflineMode && Cache.TimerEnabled)
                {
                    Cache.StopTimer();
                }
                else { }
            }
            else { }
        }
        #endregion

        #region UpdatePluginFiles
        private void UpdatePluginFiles()
        {
            Log.WriteLine("Updating plugin files", LogType.Debug);

            if (Config.Version == 1)
            {
                MoveAccountFiles();

                UpdateHistoryFiles();

                MoveBackupFiles();

                Config.Version = 2;

                Log.WriteLine("Finished updating plugin files", LogType.Debug);
            }
            else
            {
                Log.WriteLine("No file update required", LogType.Debug);
            }
        }
        #endregion

        #region MoveAccountFiles
        private void MoveAccountFiles()
        {
            string[] vFiles = Directory.GetFiles(DataDirectory);

            if (vFiles.Length >= 1)
            {
                string[] vTypes = new string[]
                {
                    Account.CollectionString,
                    Account.DecksString,
                    Account.HistoryString,
                    Account.PreferencesString
                };

                for (int i = 0; i < vTypes.Length; i++)
                {
                    string[] vChunk = vFiles.Where(s => s.Contains($"_{vTypes[i]}.xml")).ToArray();

                    for (int j = 0; j < vChunk.Length; j++)
                    {
                        FileInfo fileInfo = new FileInfo(vChunk[j]);

                        string strTargetPath = Path.Combine(AccountsDirectory, fileInfo.Name);

                        if (File.Exists(strTargetPath))
                        {
                            File.Delete(strTargetPath);
                        }
                        else { }

                        fileInfo.MoveTo(strTargetPath);
                    }
                }
            }
            else { }
        }
        #endregion

        #region UpdateHistoryFiles
        private void UpdateHistoryFiles()
        {
            string[] vFiles = Directory.GetFiles(AccountsDirectory, $"*_{Account.HistoryString}.xml");

            Parallel.For(0, vFiles.Length, i =>
            {
                XmlDocument document = new XmlDocument();

                document.Load(vFiles[i]);

                List<CachedHistoryCard> lstHistory = new List<CachedHistoryCard>();

                for (int j = 0; j < document.DocumentElement.ChildNodes.Count; j++)
                {
                    XmlNode cardNode = document.DocumentElement.ChildNodes[j];

                    XmlNode idNode = cardNode.SelectSingleNode("Id");
                    XmlNode countNode = cardNode.SelectSingleNode("Count");
                    XmlNode isGoldenNode = cardNode.SelectSingleNode("IsGolden");
                    XmlNode timestampNode = cardNode.SelectSingleNode("Timestamp");

                    if (idNode != null && countNode != null && isGoldenNode != null)
                    {
                        CachedHistoryCard card = new CachedHistoryCard
                        {
                            Id = idNode.InnerText,
                            Count = Convert.ToInt32(countNode.InnerText),
                            IsGolden = Convert.ToBoolean(isGoldenNode.InnerText),
                            Date = (timestampNode != null ? DateTime.Parse(timestampNode.InnerText) : DateTime.Now)
                        };

                        lstHistory.Add(card);
                    }
                    else { }
                }

                FileManager.Write(vFiles[i], lstHistory);
            });
        }
        #endregion

        #region MoveBackupFiles
        private void MoveBackupFiles()
        {
            string strOldDir = Path.Combine(DataDirectory, "Backup");

            if (Directory.Exists(strOldDir))
            {
                Directory.Move(strOldDir, Path.Combine(DataDirectory, "Backups"));
            }
            else { }
        }
        #endregion

        #region Static Methods
        #region CreateContainer
        public static void CreateContainer()
        {
            if (!ServiceLocator.IsLocationProviderSet)
            {
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

                if (GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic)
                {
#if DEBUG
                    SimpleIoc.Default.Register(() => s_mockAcc);
#endif
                }
                else
                {
#if DEBUG
                    SimpleIoc.Default.Register(() => s_mockAcc);
#else
                    SimpleIoc.Default.Register<IAccount>(() => Account.Empty);
#endif
                }

                SimpleIoc.Default.Register(() => Configuration.Load());

                SimpleIoc.Default.Register<MainViewModel>();
                SimpleIoc.Default.Register<CardSelectionWindowViewModel>();

                SimpleIoc.Default.Register<HistoryFlyoutViewModel>();
                SimpleIoc.Default.Register<UpdateFlyoutViewModel>();
                SimpleIoc.Default.Register<DecksFlyoutViewModel>();
                SimpleIoc.Default.Register<DeckListFlyoutViewModel>();
                SimpleIoc.Default.Register<SearchParametersFlyoutViewModel>();
                SimpleIoc.Default.Register<SortOrderFlyoutViewModel>();
                SimpleIoc.Default.Register<CollectionInfoFlyoutViewModel>();

                SimpleIoc.Default.Register<AccountSelectorDialogViewModel>();
                SimpleIoc.Default.Register<SettingsDialogViewModel>();
                SimpleIoc.Default.Register<SortOrderItemSelectorDialogViewModel>();
            }
            else { }
        }
        #endregion

        #region ShowMainWindow
        private static void ShowMainWindow()
        {
            ServiceLocator.Current.GetInstance<MainViewModel>().Initialize();

            if (s_mainWindow == null)
            {
                s_mainWindow = new MainWindow();
            }
            else { }

            Log.WriteLine($"Opening main window for {CurrentAccount.AccountString}", LogType.Info);

            s_mainWindow.Show();

            BringWindowToFront(s_mainWindow);
        }
        #endregion

        #region SelectAccount
        private static IAccount SelectAccount(bool blnIsSwitching)
        {
            IAccount retVal = null;

            if (Core.Game.IsRunning && !blnIsSwitching)
            {
                retVal = Account.LoggedInAccount;
            }
            else
            {
                IAccount[] vAccounts = GetAccounts();

                if (vAccounts.Length == 1)
                {
                    retVal = vAccounts[0];
                }
                else if (vAccounts.Length > 1)
                {
                    Window owner = s_mainWindow;

                    if (!blnIsSwitching)
                    {
                        owner = Core.MainWindow;
                    }
                    else { }

                    ServiceLocator.Current.GetInstance<AccountSelectorDialogViewModel>().Initialize();

                    AccountSelectorDialogView dialog = new AccountSelectorDialogView()
                    {
                        Owner = owner
                    };

                    if (dialog.ShowDialog().Value)
                    {
                        retVal = Account.Parse(ServiceLocator.Current.GetInstance<AccountSelectorDialogViewModel>().SelectedAccountString);

                        Config.LastSelectedAccount = retVal.AccountString;
                    }
                    else { }
                }
                else
                {
                    MessageBox.Show("No account(s) available!", "Dust Utility", MessageBoxButton.OK, MessageBoxImage.Warning);

                    Log.WriteLine("No accounts available", LogType.Info);
                }
            }

            if (retVal != null)
            {
                Log.WriteLine($"Account: {retVal.AccountString}", LogType.Debug);
            }
            else
            {
                Log.WriteLine($"No account selected", LogType.Debug);
            }

            return retVal;
        }
        #endregion

        #region GetAccounts
        public static IAccount[] GetAccounts()
        {
            List<IAccount> lstRet = new List<IAccount>();

#if DEBUG
            lstRet.Add(s_mockAcc);
#endif

            if (Directory.Exists(DataDirectory))
            {
                string[] vFiles = Directory.GetFiles(AccountsDirectory, $"*_{Account.CollectionString}.xml");

                for (int i = 0; i < vFiles.Length; i++)
                {
                    string strCollectionFileName = vFiles[i];

                    string strDecksFileName = strCollectionFileName.Replace($"_{Account.CollectionString}", $"_{Account.DecksString}");

                    if (File.Exists(strDecksFileName))
                    {
                        string strAccountString = Path.GetFileNameWithoutExtension(strCollectionFileName).Replace($"_{Account.CollectionString}", string.Empty);

                        lstRet.Add(Account.Parse(strAccountString));
                    }
                    else { }
                }
            }
            else { }

            return lstRet.ToArray();
        }
        #endregion

        #region SwitchAccount
        public static bool SwitchAccount()
        {
            bool blnRet = false;

            if (s_mainWindow != null)
            {
                Log.WriteLine("Switching account...", LogType.Debug);

                IAccount oldAcc = CurrentAccount;

                IAccount selectedAcc = SelectAccount(true);

                if (selectedAcc == null)
                {
                    selectedAcc = oldAcc;
                }
                else { }

                if (!selectedAcc.Equals(oldAcc))
                {
                    UpdatedAccountInstance(selectedAcc);

                    blnRet = true;
                }
                else { }

                ShowMainWindow();

                Log.WriteLine($"Switched account: Old={oldAcc.AccountString} New={selectedAcc.AccountString}", LogType.Debug);
            }
            else { }

            return blnRet;
        }
        #endregion

        #region UpdatedAccountInstance
        private static void UpdatedAccountInstance(IAccount account)
        {
            CurrentAccount.SavePreferences();

            //Remove current account instance
            SimpleIoc.Default.Unregister<IAccount>();

            //And readd it
            SimpleIoc.Default.Register(() => account);
        }
        #endregion

        #region GetDataDirectory
        private static string GetDataDirectory(string strFolder = "")
        {
            string strRet = Path.Combine(Hearthstone_Deck_Tracker.Config.Instance.DataDir, "DustUtility", strFolder);

            if (!Directory.Exists(strRet))
            {
                Directory.CreateDirectory(strRet);
            }
            else { }

            return strRet;
        }
        #endregion

        #region GetFullFileName
        public static string GetFullFileName(IAccount account, string strType)
        {
            string strRet = string.Empty;

            if (!account.IsEmpty)
            {
                strRet = Path.Combine(DataDirectory, "Accounts", $"{account.AccountString}_{strType}.xml");
            }
            else
            {
                strRet = Path.Combine(DataDirectory, "Accounts", $"{strType}.xml");
            }

            return strRet;
        }
        #endregion

        #region BringWindowToFront
        public static void BringWindowToFront(Window window)
        {
            window.Activate();
            window.Topmost = true;
            window.Topmost = false;
            window.Focus();
        }
        #endregion
        #endregion
    }
}