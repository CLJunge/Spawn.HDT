namespace Spawn.HDT.DustUtility.UI.Dialogs
{
    public partial class AccountSelectorDialogView
    {
        #region Ctor
        public AccountSelectorDialogView()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        #region OnOkClick
        private void OnOkClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
        }
        #endregion
        #endregion
    }
}