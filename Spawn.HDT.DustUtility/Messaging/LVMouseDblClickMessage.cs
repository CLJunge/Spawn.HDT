#region Using
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
        }
        #endregion
    }
}