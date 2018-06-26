namespace Spawn.HDT.DustUtility.UI.Dialogs
{
    public partial class SortOrderItemSelectorDialogView
    {
        #region Ctor
        public SortOrderItemSelectorDialogView()
        {
            InitializeComponent();

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Created new 'SortOrderItemSelectorDialogView' instance");
        }
        #endregion

        #region Events
        #region OnOkClick
        private void OnOkClick(object sender, System.Windows.RoutedEventArgs e) => DialogResult = true;
        #endregion
        #endregion
    }
}