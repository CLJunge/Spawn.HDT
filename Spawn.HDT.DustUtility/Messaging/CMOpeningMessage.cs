#region Using
using System.Windows.Controls;
#endregion

namespace Spawn.HDT.DustUtility.Messaging
{
    public class CMOpeningMessage
    {
        #region Properties
        #region FlyoutName
        public string FlyoutName { get; private set; }
        #endregion

        #region EventArgs
        public ContextMenuEventArgs EventArgs { get; private set; }
        #endregion
        #endregion

        #region Ctor
        public CMOpeningMessage(string flyoutName, ContextMenuEventArgs eventArgs)
        {
            FlyoutName = flyoutName;
            EventArgs = eventArgs;
        }
        #endregion
    }
}