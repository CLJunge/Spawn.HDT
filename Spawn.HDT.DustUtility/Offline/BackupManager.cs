using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;

namespace Spawn.HDT.DustUtility.Offline
{
    public static class BackupManager
    {
        #region Create
        public static bool Create(Account account)
        {
            bool blnRet = false;

            if (Settings.OfflineMode)
            {
                DateTime date = DateTime.Now;

                if (!BackupExists(account, date))
                {
                    string strFileName = GetFileName(account, date);

                    try
                    {
                        string[] vFiles = Directory.GetFiles(DustUtilityPlugin.DataDirectory, $"{account.AccountString}*");

                        if (vFiles.Length > 0)
                        {
                            string strDir = Path.Combine(DustUtilityPlugin.DataDirectory, "temp");

                            Directory.CreateDirectory(strDir);

                            for (int i = 0; i < vFiles.Length; i++)
                            {
                                string strTemp = Path.Combine(strDir, Path.GetFileName(vFiles[i]));

                                File.Copy(vFiles[i], strTemp);
                            }

                            ZipFile.CreateFromDirectory(strDir, strFileName);

                            Directory.Delete(strDir, true);

                            Log.WriteLine($"Created backup for {account.AccountString}", LogType.Debug);

                            blnRet = File.Exists(strFileName);
                        }
                        else { }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteLine($"Exception occured while creating backup \"{strFileName}\": {ex}", LogType.Error);
                    }
                }
                else { }
            }
            else { }

            return blnRet;
        }
        #endregion

        #region Restore
        public static bool Restore(Account account, DateTime date)
        {
            bool blnRet = false;

            if (BackupExists(account, date))
            {
                string strFileName = GetFileName(account, date);

                try
                {
                    string strDir = Path.Combine(DustUtilityPlugin.DataDirectory, "extracted");

                    ZipFile.ExtractToDirectory(strFileName, strDir);

                    string[] vFiles = Directory.GetFiles(strDir);

                    for (int i = 0; i < vFiles.Length; i++)
                    {
                        string strTemp = Path.Combine(DustUtilityPlugin.DataDirectory, Path.GetFileName(vFiles[i]));

                        if (File.Exists(strTemp))
                        {
                            File.Delete(strTemp);
                        }
                        else { }

                        File.Move(vFiles[i], strTemp);
                    }

                    Directory.Delete(strDir);

                    Log.WriteLine($"Restored backup for {account.AccountString}", LogType.Debug);

                    blnRet = true;
                }
                catch (Exception ex)
                {
                    Log.WriteLine($"Exception occured while restoring backup \"{strFileName}\": {ex}", LogType.Error);
                }
            }
            else { }

            return blnRet;
        }
        #endregion

        #region BackupExists
        public static bool BackupExists(Account account, DateTime date)
        {
            string strFileName = GetFileName(account, date);

            return File.Exists(strFileName);
        }
        #endregion

        #region GetFileName
        private static string GetFileName(Account account, DateTime date)
        {
            string strDirectory = Path.Combine(DustUtilityPlugin.DataDirectory, "Backup", account.AccountString);

            if (!Directory.Exists(strDirectory))
            {
                Directory.CreateDirectory(strDirectory);
            }
            else { }

            string strFileName = $"backup_{date.ToString("yyyyMMdd", CultureInfo.InvariantCulture)}.zip";

            return Path.Combine(strDirectory, strFileName);
        }
        #endregion
    }
}