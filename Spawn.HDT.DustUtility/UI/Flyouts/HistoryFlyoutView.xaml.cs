#region Using
using Spawn.HDT.DustUtility.UI.ViewModels;
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
        private void OnRemoveCardItem(object sender, CardItemEventArgs e) => this.GetViewModel<HistoryFlyoutViewModel>().RemoveCardItem(e);
        #endregion
        #endregion
    }
}