#region Using
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
#endregion

namespace Spawn.HDT.DustUtility.Logging
{
    public class Logger
    {
        #region Static Fields
        private static int s_nGlobalLoggerCount;
        #endregion

        #region Member Variables
        private DirectoryInfo m_logDirectory = null;
        private string m_strFilePath = string.Empty;
        private FileInfo m_logFile = null;
        private static object s_objLock = null;
        #endregion

        #region Properties
        #region Name
        public string Name { get; } = $"Logger_{s_nGlobalLoggerCount++}";
        #endregion

        #region LogFileDirectory
        public string LogFileDirectory => m_logDirectory.FullName;
        #endregion

        #region LogFileName
        public string LogFileName => m_logFile.FullName;
        #endregion

        #region IsDebugMode
        public bool IsDebugMode { get; set; } = false;
        #endregion

        #region WriteToConsole
        public bool WriteToConsole { get; set; } = true;
        #endregion

        #region WriteToFile
        public bool WriteToFile { get; set; } = true;
        #endregion

        #region FormatString
        public string FormatString { get; set; } = "%t [%l::%c] %m";
        #endregion

        #region [STATIC] Default
        private static Logger s_default = null;

        public static Logger Default => s_default ?? (s_default = new Logger("Default"));
        #endregion

        #region [STATIC] Debug
        private static Logger s_debug = null;

        public static Logger Debug => s_debug ?? (s_debug = new Logger("Debug") { IsDebugMode = true });
        #endregion
        #endregion

        #region Events
        public event EventHandler<LogEvent> Logging;

        private void OnLogging(LogEntry entry) => Logging?.Invoke(this, new LogEvent(entry));
        #endregion

        #region Ctor
        public Logger(string name, string logDirectory = null)
        {
            if (Directory.Exists(Environment.CurrentDirectory))
            {
                s_objLock = new object();

                Name = name;

                if (!string.IsNullOrEmpty(logDirectory))
                {
                    m_logDirectory = new DirectoryInfo(logDirectory);
                }
                else
                {
                    m_logDirectory = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Logs"));
                }

                if (!m_logDirectory.Exists)
                {
                    m_logDirectory.Create();
                }
                else { }

                SetLogFileName();
            }
            else
            {
                throw new ArgumentException("The directory doesn't exist!", "FilePath");
            }
        }
        #endregion

        #region Log
        public LogEntry Log(LogLevel level, string strMessage, [CallerMemberName] string strMemberName = "", [CallerFilePath] string strFilePath = "")
        {
            return Log(level, string.Empty, strMessage, strMemberName, strFilePath);
        }

        public LogEntry Log(LogLevel level, string strChannel, string strMessage, [CallerMemberName] string strMemberName = "", [CallerFilePath] string strFilePath = "")
        {
            LogEntry retVal = new LogEntry();

            lock (s_objLock)
            {
                if (level >= DustUtilityPlugin.Config.LogLevel)
                {
                    SetLogFileName();

                    try
                    {
                        DateTime dtTimestamp = DateTime.Now;

                        string strAssembledMessage = AssembleMessage(dtTimestamp, strChannel, level, GetCallingMember(strMemberName, strFilePath), strMessage);

                        retVal = new LogEntry(dtTimestamp, level, strChannel, strMessage, strAssembledMessage);

                        if (WriteToConsole)
                        {
                            LogToConsole(retVal);
                        }
                        else { }

                        if (IsDebugMode)
                        {
                            System.Diagnostics.Debug.WriteLine(strAssembledMessage);
                        }
                        else { }

                        if (WriteToFile)
                        {
                            using (StreamWriter writer = new StreamWriter(m_strFilePath, File.Exists(m_strFilePath)))
                            {
                                writer.WriteLine(strAssembledMessage);
                                writer.Flush();
                            }
                        }
                        else { }
                        
                        OnLogging(retVal);
                    }
                    catch (IOException ex)
                    {
                        throw new FileAccessException("Can't access log file!", ex);
                    }
                }
            }

            return retVal;
        }
        #endregion

        #region AssembleMessage
        private string AssembleMessage(DateTime dtTimestamp, string strChannel, LogLevel level, string strCallingMember, string strMessage)
        {
            return FormatString
                .Replace("%t", dtTimestamp.ToString("hh:mm:ss.fff tt"))
                .Replace("%c", strCallingMember)
                .Replace("%l", level.ToString())
                .Replace("%s", strCallingMember)
                .Replace("%m", strMessage);
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
            m_strFilePath = Path.Combine(m_logDirectory.FullName, string.Format("{0}_{1}.txt", Name, DateTime.Now.ToString("yyyyMMdd")));

            m_logFile = new FileInfo(m_strFilePath);

            if (!m_logFile.Exists)
            {
                using (m_logFile.Create()) { }
            }
            else { }

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
                else { }
            }
            else if (!string.IsNullOrEmpty(strMemberName))
            {
                strRet = strMemberName;
            }
            else { }

            return strRet;
        }
        #endregion
    }
}