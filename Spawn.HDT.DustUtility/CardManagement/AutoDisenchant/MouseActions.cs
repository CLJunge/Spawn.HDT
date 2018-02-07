#region Using
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

#endregion
namespace Spawn.HDT.DustUtility.CardManagement.AutoDisenchant
{
    public class MouseActions
    {
        #region Member Variables
        private HearthstoneInfo m_info;
        private readonly Func<Task<bool>> m_onUnexpectedMousePos;
        private Point m_previousCursorPos;
        #endregion

        #region Ctor
        public MouseActions(HearthstoneInfo info, Func<Task<bool>> onUnexpectedMousePos)
        {
            m_info = info;
            m_onUnexpectedMousePos = onUnexpectedMousePos;
            m_previousCursorPos = Point.Empty;
        }
        #endregion

        #region ClickOnPoint
        public async Task ClickOnPoint(Point clientPoint, bool blnUseRightMouseButton = false)
        {
            if (!User32.IsHearthstoneInForeground() || (m_onUnexpectedMousePos != null && m_previousCursorPos != Point.Empty &&
                (Math.Abs(m_previousCursorPos.X - Cursor.Position.X) > 10 || Math.Abs(m_previousCursorPos.Y - Cursor.Position.Y) > 10)))
            {
                if (!(m_onUnexpectedMousePos == null || await m_onUnexpectedMousePos()))
                    throw new Exception("Export interrupted, not continuing");
                if ((m_info = await Helper.EnsureHearthstoneInForeground(m_info)) == null)
                    throw new Exception("Export interrupted - could not re-focus hearthstone");
                await Task.Delay(500);
            }

            User32.ClientToScreen(m_info.HsHandle, ref clientPoint);
            Cursor.Position = m_previousCursorPos = new Point(clientPoint.X, clientPoint.Y);
            Log.Debug("Clicking " + Cursor.Position);

            //mouse down
            if (SystemInformation.MouseButtonsSwapped || blnUseRightMouseButton)
                User32.mouse_event((uint)User32.MouseEventFlags.RightDown, 0, 0, 0, UIntPtr.Zero);
            else
                User32.mouse_event((uint)User32.MouseEventFlags.LeftDown, 0, 0, 0, UIntPtr.Zero);

            await Task.Delay(DisenchantConfig.Instance.Delay);

            //mouse up
            if (SystemInformation.MouseButtonsSwapped || blnUseRightMouseButton)
                User32.mouse_event((uint)User32.MouseEventFlags.RightUp, 0, 0, 0, UIntPtr.Zero);
            else
                User32.mouse_event((uint)User32.MouseEventFlags.LeftUp, 0, 0, 0, UIntPtr.Zero);

            await Task.Delay(DisenchantConfig.Instance.Delay);
        }
        #endregion
    }
}