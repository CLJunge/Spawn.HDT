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

        #region AssembledMessage
        public string AssembledMessage { get; }
        #endregion
        #endregion

        #region Ctor
        public LogEntry(DateTime logTime, LogLevel level, string channel, string message, string assembledMessage)
        {
            Timestamp = logTime;
            Level = level;
            Channel = channel;
            Message = message;
            AssembledMessage = assembledMessage;
        }
        #endregion
    }
}