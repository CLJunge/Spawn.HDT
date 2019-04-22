#region Using
using System;
#if DEBUG
using System.Collections.Generic;
#endif
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
#endregion

namespace Spawn.HDT.DustUtility.Logging
{
    public class Logger
    {
        #region Static Fields
        private static readonly object s_objLock = new object();
        #endregion

        #region Member Variables
        private readonly DirectoryInfo m_logDirectory = null;
        private string m_strFilePath = string.Empty;
        private FileInfo m_logFile = null;
        #endregion

        #region Properties
        #region Name
        public string Name { get; }
        #endregion

        #region LogDirectory
        public string LogDirectory => m_logDirectory.FullName;
        #endregion

        #region LogFileName
        public string LogFileName => m_logFile.FullName;
        #endregion

        #region DebugMode
        public bool DebugMode { get; set; } = false;
        #endregion

        #region WriteToConsole
        public bool WriteToConsole { get; set; } = true;
        #endregion

        #region WriteToFile
        public bool WriteToFile { get; set; } = true;
        #endregion

        #region FormatString
        public string FormatString { get; set; } = "%t [%l|%s] %m";
        #endregion
        #endregion

        #region Events
        public event EventHandler<LogEventArgs> Logging;

        private void OnLogging(LogEntry entry) => Logging?.Invoke(this, new LogEventArgs(entry));
        #endregion

        #region Ctor
        public Logger(string name = null, string logDirectory = null)
        {
            Name = name;

            m_logDirectory = !string.IsNullOrEmpty(logDirectory)
                ? new DirectoryInfo(logDirectory)
                : new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Logs"));

            if (!m_logDirectory.Exists)
                m_logDirectory.Create();

            SetLogFileName();
        }
        #endregion

        #region Log
        public LogEntry Log(LogLevel level, string strMessage, [CallerMemberName] string strMemberName = "", [CallerFilePath] string strFilePath = "") => Log(level, string.Empty, strMessage, strMemberName, strFilePath);

        public LogEntry Log(LogLevel level, string strChannel, string strMessage, [CallerMemberName] string strMemberName = "", [CallerFilePath] string strFilePath = "")
        {
            LogEntry retVal = new LogEntry();

            lock (s_objLock)
            {
                string strCallingMember = GetCallingMember(strMemberName, strFilePath);

#if DEBUG
                if (level >= DustUtilityPlugin.Config.LogLevel && IsLoggableSource(strCallingMember))
#else
                if (level >= DustUtilityPlugin.Config.LogLevel)
#endif
                {
                    SetLogFileName();

                    try
                    {
                        DateTime dtTimestamp = DateTime.Now;

                        string strAssembledMessage = FormatString
                            .Replace("%t", dtTimestamp.ToString("hh:mm:ss.fff tt"))
                            .Replace("%c", strChannel)
                            .Replace("%l", level.ToString())
                            .Replace("%s", strCallingMember)
                            .Replace("%m", strMessage);

                        retVal = new LogEntry(dtTimestamp, level, strChannel, strMessage, strAssembledMessage);

                        if (WriteToConsole)
                            LogToConsole(retVal);

                        if (WriteToFile)
                        {
                            using (StreamWriter writer = new StreamWriter(m_strFilePath, File.Exists(m_strFilePath)))
                            {
                                writer.WriteLine(strAssembledMessage);
                                writer.Flush();
                            }
                        }

                        OnLogging(retVal);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show($"Can't access log file: {ex}", "Dust Utility - Logging");
                    }
                }
            }

            if (DebugMode && !string.IsNullOrEmpty(retVal.AssembledMessage))
                System.Diagnostics.Debug.WriteLine(retVal.AssembledMessage);

            return retVal;
        }
        #endregion

        #region LogToConsole
        private void LogToConsole(LogEntry retEntry)
        {
            switch (retEntry.Level)
            {
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;

                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }

            Console.WriteLine(retEntry.AssembledMessage);

            Console.ForegroundColor = ConsoleColor.Gray;
        }
        #endregion

        #region SetLogFileName
        private void SetLogFileName()
        {
            m_strFilePath = !string.IsNullOrEmpty(Name)
                ? Path.Combine(m_logDirectory.FullName, $"{Name}_{DateTime.Now.ToString("yyyyMMdd")}.txt")
                : Path.Combine(m_logDirectory.FullName, $"{DateTime.Now.ToString("yyyyMMdd")}.txt");

            m_logFile = new FileInfo(m_strFilePath);

            if (!m_logFile.Exists)
                using (m_logFile.Create()) { /* Do nothing */ }

            m_logFile.Refresh();
        }
        #endregion

        #region GetCallingMember
        private string GetCallingMember(string strMemberName, string strFilePath)
        {
            string strRet = string.Empty;

            if (!string.IsNullOrEmpty(strMemberName) && !string.IsNullOrEmpty(strFilePath))
            {
                string strTemp = strFilePath.Split('\\').LastOrDefault();

                if (!string.IsNullOrEmpty(strTemp))
                {
                    strTemp = strTemp.Replace(".cs", string.Empty);

                    strRet = $"{strTemp}.{strMemberName.TrimStart('.')}";
                }
            }
            else if (!string.IsNullOrEmpty(strMemberName))
            {
                strRet = strMemberName;
            }

            return strRet;
        }
        #endregion

#if DEBUG
        #region IsLoggableSource
        private bool IsLoggableSource(string strSource)
        {
            bool blnRet = true;

            string[] vTemp = DustUtilityPlugin.Config.LoggableSources?.Split(';');

            List<string> lstLoggableSources = vTemp?.ToList();

            if (lstLoggableSources?.Count > 0)
                blnRet = lstLoggableSources.Contains(strSource);

            return blnRet;
        }
        #endregion
#endif
    }
}