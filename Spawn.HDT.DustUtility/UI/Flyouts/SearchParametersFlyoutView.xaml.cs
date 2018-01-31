namespace Spawn.HDT.DustUtility.UI.Flyouts
{
    public partial class SearchParametersFlyoutView
    {
        #region Ctor
        public SearchParametersFlyoutView()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        #region OnGoldenCheckBoxUnchecked
        private void OnGoldenCheckBoxUnchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            cbGoldenOnly.IsChecked = false;
        }
        #endregion
        #endregion
    }
}