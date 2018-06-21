#region Using
using System;
#endregion

namespace Spawn.HDT.DustUtility.Logging
{
    public struct LogEntry
    {
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

        #region CallingMember
        public string CallingMember { get; }
        #endregion
        #endregion

        #region Ctor
        public LogEntry(DateTime logTime, LogLevel level, string channel, string message, string callingMember)
        {
            Timestamp = logTime;
            Level = level;
            Channel = channel;
            Message = message;
            CallingMember = callingMember;

            if (!string.IsNullOrEmpty(Channel))
            {
                LogMessage = string.Format("{0} [{1}::{2}::{3}] {4}", Timestamp.ToString("hh:mm:ss.fff tt"), Channel, Level, CallingMember, Message);
            }
            else
            {
                LogMessage = string.Format("{0} [{1}::{2}] {3}", Timestamp.ToString("hh:mm:ss.fff tt"), Level, CallingMember, Message);
            }
        }
        #endregion
    }
}