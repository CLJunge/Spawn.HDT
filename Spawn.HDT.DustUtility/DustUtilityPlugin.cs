using Autofac;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Spawn.HDT.DustUtility.Offline;
using Spawn.HDT.DustUtility.Services;
using Spawn.HDT.DustUtility.Services.Providers;
using Spawn.HDT.DustUtility.UI.Dialogs;
using Spawn.HDT.DustUtility.UI.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Spawn.HDT.DustUtility
{
    public class DustUtilityPlugin : IPlugin
    {
        #region Static Properties
        #region DataDirectory
        public static string DataDirectory => GetDataDirectory();
        #endregion

        #region IsOffline
        public static bool IsOffline { get; private set; }
        #endregion

        #region MainWindow
        public static Window MainWindow => s_mainWindow;
        #endregion

        #region CurrentAccount
        public static Account CurrentAccount => (s_account ?? Account.Empty);
        #endregion

        #region Config
        public static Configuration Config => s_config ?? (s_config = Configuration.Load());
        #endregion

        #region Container
        public static IContainer Container { get; private set; }
        #endregion

        #region NumericRegex
        public static Regex NumericRegex => new Regex("^(0|[1-9][0-9]*)$");
        #endregion
        #endregion

        #region Static Fields
        private static Window s_mainWindow;

        private static Account s_account;
        private static Configuration s_config;
        #endregion

        #region Properties
        #region Name
        public string Name => "Dust Utility";
        #endregion

        #region Description
        public string Description => "Enter the amount of dust you want to get and check which cards are currently not used in any deck in order to see which can be disenchanted.";
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

        #region Ctor
        public DustUtilityPlugin()
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }
        #endregion

        #region IPlugin Methods
        #region OnLoad
        public void OnLoad()
        {
            BuildContainer();

            CreateMenuItem();

            if (Config.OfflineMode && Core.Game.IsRunning)
            {
                Cache.StartTimer();
            }
            else { }
        }
        #endregion

        #region OnButtonPress
        public void OnButtonPress()
        {
            Log.WriteLine("Opening settings dialog", LogType.Debug);

            using (ILifetimeScope scope = Container.BeginLifetimeScope())
            {
                var dialogService = scope.Resolve<IDialogService>();

                if (dialogService.ShowDialog<SettingsDialog>())
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
        }
        #endregion

        #region OnUnload
        public void OnUnload()
        {
            if (Cache.TimerEnabled)
            {
                Cache.StopTimer();
            }
            else { }

            Config.Save();

            Container.Dispose();
        }
        #endregion

        #region OnUpdate
        public void OnUpdate()
        {
            IsOffline = !Core.Game.IsRunning && Config.OfflineMode;

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

        #region OnClick
        private void OnClick(object sender, RoutedEventArgs e)
        {
            if (Core.Game.IsRunning || Config.OfflineMode)
            {
                if (!s_account?.IsValid ?? true)
                {
                    SelectAccount(false);
                }
                else { }

                if (s_account?.IsValid ?? false)
                {
                    OpenMainWindow();
                }
                else { }
            }
            else if (!Config.OfflineMode)
            {
                MessageBox.Show("Hearthstone isn't running!", Name, MessageBoxButton.OK, MessageBoxImage.Warning);

                Log.WriteLine("Hearthstone isn't running!", LogType.Warning);
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

            retVal.Click += OnClick;

            return retVal;
        }
        #endregion

        #region BuildContainer
        private void BuildContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<DialogServiceProvider>().As<IDialogService>();

            Container = builder.Build();
        }
        #endregion

        #region OnAssemblyResolve
        private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly retVal = null;

            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = new AssemblyName(args.Name);

            string strPath = $"{assemblyName.Name}.dll";

            using (Stream stream = executingAssembly.GetManifestResourceStream(strPath))
            {
                if (stream != null)
                {
                    byte[] vRawBytes = new byte[stream.Length];

                    stream.Read(vRawBytes, 0, vRawBytes.Length);

                    retVal = Assembly.Load(vRawBytes);
                }
                else { }
            }

            return retVal;
        }
        #endregion

        #region Static Methods
        #region OpenMainWindow
        private static void OpenMainWindow()
        {
            if (s_mainWindow == null)
            {
                Log.WriteLine($"Opening main window for {s_account.AccountString}", LogType.Info);

                //s_mainView = new MainWindowView(s_account, GetAccounts().Length > 1);
                s_mainWindow = new MainWindow();

                s_mainWindow.Closed += (s, e) =>
                {
                    s_account.SavePreferences();

                    s_mainWindow = null;
                };

                s_mainWindow.Show();
            }
            else
            {
                BringWindowToFront(s_mainWindow);
            }
        }
        #endregion

        #region SelectAccount
        private static void SelectAccount(bool blnIsSwitching)
        {
            if (Core.Game.IsRunning && !blnIsSwitching)
            {
                s_account = Account.LoggedInAccount;
            }
            else
            {
                Account[] vAccounts = GetAccounts();

                if (vAccounts.Length == 1)
                {
                    s_account = vAccounts[0];
                }
                else if (vAccounts.Length > 1)
                {
                    using (var scope = Container.BeginLifetimeScope())
                    {
                        var dialogService = scope.Resolve<IDialogService>();

                        if (dialogService.ShowDialog<AccountSelectorDialog>())
                        {
                            s_account = Account.Parse(dialogService.GetDialogResult<string>());

                            Config.LastSelectedAccount = s_account.AccountString;
                        }
                        else { }
                    }
                }
                else
                {
                    s_account = Account.Empty;
                }
            }

            if (s_account != null)
            {
                Log.WriteLine($"Account: {s_account.AccountString}", LogType.Debug);
            }
            else
            {
                Log.WriteLine($"No account selected", LogType.Debug);
            }
        }
        #endregion

        #region GetAccounts
        public static Account[] GetAccounts()
        {
            List<Account> lstRet = new List<Account>();

            if (Directory.Exists(DataDirectory))
            {
                string[] vFiles = Directory.GetFiles(DataDirectory, $"*_{Cache.CollectionString}.xml");

                for (int i = 0; i < vFiles.Length; i++)
                {
                    string strCollectionFileName = vFiles[i];

                    string strDecksFileName = strCollectionFileName.Replace($"_{Cache.CollectionString}", $"_{Cache.DecksString}");

                    if (File.Exists(strDecksFileName))
                    {
                        string strAccountString = Path.GetFileNameWithoutExtension(strCollectionFileName).Replace($"_{Cache.CollectionString}", string.Empty);

                        lstRet.Add(Account.Parse(strAccountString));
                    }
                    else { }
                }
            }
            else { }

            return lstRet.ToArray();
        }
        #endregion

        #region SwitchAccounts
        public static void SwitchAccounts()
        {
            if (s_mainWindow != null)
            {
                Log.WriteLine("Switching accounts...", LogType.Debug);

                Account oldAcc = s_account;

                s_account = null;

                SelectAccount(true);

                if (s_account == null)
                {
                    s_account = oldAcc;
                }
                else { }

                if (!s_account.Equals(oldAcc))
                {
                    s_mainWindow.Close();
                }
                else { }

                OpenMainWindow();

                Log.WriteLine($"Switched accounts: Old={oldAcc.AccountString} New={s_account.AccountString}", LogType.Debug);
            }
            else { }
        }
        #endregion

        #region GetDataDirectory
        private static string GetDataDirectory()
        {
            string strRet = Path.Combine(Hearthstone_Deck_Tracker.Config.Instance.DataDir, "DustUtility");

            if (!Directory.Exists(strRet))
            {
                Directory.CreateDirectory(strRet);
            }
            else { }

            return strRet;
        }
        #endregion

        #region GetFullFileName
        public static string GetFullFileName(Account account, string strType)
        {
            string strRet = string.Empty;

            if (!account.IsEmpty)
            {
                strRet = Path.Combine(DataDirectory, $"{account.AccountString}_{strType}.xml");
            }
            else
            {
                strRet = Path.Combine(DataDirectory, $"{strType}.xml");
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