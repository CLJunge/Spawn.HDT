namespace Spawn.HDT.DustUtility.UI.Dialogs
{
    public partial class SortOrderItemSelectorDialogView
    {
        #region Ctor
        public SortOrderItemSelectorDialogView()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        #region OnOkClick
        private void OnOkClick(object sender, System.Windows.RoutedEventArgs e) => DialogResult = true;
        #endregion
        #endregion
    }
}