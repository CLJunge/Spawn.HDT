#region Using
using System;
#endregion

namespace Spawn.HDT.DustUtility.Logging
{
    public struct LogEntry
    {
        #region Member Variables
        private readonly object[] m_vArgs;
        #endregion

        #region Properties
        #region Timestamp
        public DateTime Timestamp { get; }
        #endregion

        #region Level
        public LogLevel Level { get; }
        #endregion

        #region Channel
        public string Channel { get; }
        #endregion

        #region Message
        public string Message { get; }
        #endregion

        #region LogMessage
        public string LogMessage { get; }
        #endregion
        #endregion

        #region Ctor
        public LogEntry(DateTime logTime, LogLevel level, string channel, string message, object[] arguments)
        {
            Timestamp = logTime;
            Level = level;
            Channel = channel;
            Message = string.Format(message, arguments);
            m_vArgs = arguments;

            if (!string.IsNullOrEmpty(channel))
            {
                LogMessage = string.Format("{0} [{1}::{2}] {3}", Timestamp.ToLongTimeString(), Level, Channel, string.Format(Message, m_vArgs));
            }
            else
            {
                LogMessage = string.Format("{0} [{1}] {2}", Timestamp.ToLongTimeString(), Level, string.Format(Message, m_vArgs));
            }
        }
        #endregion
    }
}