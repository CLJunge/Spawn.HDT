#region Using
using Spawn.HDT.DustUtility.Logging;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.Messaging
{
    public class LVMouseDblClickMessage
    {
        #region Properties
        #region FlyoutName
        public string FlyoutName { get; private set; }
        #endregion

        #region EventArgs
        public MouseButtonEventArgs EventArgs { get; private set; }
        #endregion
        #endregion

        #region Ctor
        public LVMouseDblClickMessage(string flyoutName, MouseButtonEventArgs eventArgs)
        {
            FlyoutName = flyoutName;
            EventArgs = eventArgs;

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Created new 'LVMouseDblClickMessage' instance");
        }
        #endregion
    }
}