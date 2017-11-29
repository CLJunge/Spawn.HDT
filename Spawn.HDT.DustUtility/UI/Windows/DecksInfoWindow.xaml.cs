namespace Spawn.HDT.DustUtility.UI.Windows
{
    public partial class DecksInfoWindow
    {
        #region Member Variables
        private Account m_account;
        #endregion

        #region Ctor
        public DecksInfoWindow()
        {
            InitializeComponent();
        }

        public DecksInfoWindow(Account account)
            : this()
        {
            m_account = account;
        }
        #endregion

        #region Events
        #region OnLoaded
        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            m_account.LoadDecks();
        }
        #endregion
        #endregion
    }
}