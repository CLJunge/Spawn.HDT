#region Using
using GalaSoft.MvvmLight.Messaging;
using Spawn.HDT.DustUtility.Logging;
using Spawn.HDT.DustUtility.Messaging;
#endregion

namespace Spawn.HDT.DustUtility.UI.Flyouts
{
    public partial class DecksFlyoutView
    {
        #region Ctor
        public DecksFlyoutView()
        {
            InitializeComponent();

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Created new 'DecksFlyoutView' instance");
        }
        #endregion

        #region Events
        #region OnListViewMouseMove
        private void OnListViewMouseMove(object sender, System.Windows.Input.MouseEventArgs e) => (sender as System.Windows.Controls.ListView).ReleaseMouseCapture();
        #endregion

        #region OnListViewContextMenuOpening
        private void OnListViewContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e) => Messenger.Default.Send(new CMOpeningMessage(DustUtilityPlugin.DecksFlyoutName, e));
        #endregion

        #region OnListViewMouseDoubleClick
        private void OnListViewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) => Messenger.Default.Send(new LVMouseDblClickMessage(DustUtilityPlugin.DecksFlyoutName, e));
        #endregion
        #endregion
    }
}