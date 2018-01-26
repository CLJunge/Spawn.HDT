using Spawn.HDT.DustUtility.ViewModel;

namespace Spawn.HDT.DustUtility.UI.Dialogs
{
    public partial class HistoryDialog
    {
        #region Ctor
        public HistoryDialog()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        #region OnRowDeleted
        private void OnRowDeleted(object sender, DataGridCardItemEventArgs e) => ((HistoryDialogViewModel)DataContext).RemoveItem(e);
        #endregion
        #endregion
    }
}