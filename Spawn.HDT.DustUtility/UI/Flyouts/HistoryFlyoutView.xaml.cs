#region Using
using GalaSoft.MvvmLight.Messaging;
using Spawn.HDT.DustUtility.Messaging;
#endregion

namespace Spawn.HDT.DustUtility.UI.Flyouts
{
    public partial class HistoryFlyoutView
    {
        #region Ctor
        public HistoryFlyoutView()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        #region OnRemoveCardItem
        private void OnRemoveCardItem(object sender, CardItemEventArgs e) => Messenger.Default.Send(new RemoveCardItemMessage(DustUtilityPlugin.HistoryFlyoutName, e));
        #endregion
        #endregion
    }
}