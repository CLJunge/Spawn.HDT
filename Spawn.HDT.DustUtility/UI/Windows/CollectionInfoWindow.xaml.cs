using System.Windows;

namespace Spawn.HDT.DustUtility.UI.Windows
{
    public partial class CollectionInfoWindow
    {
        #region Member Variables
        private Account m_account;
        #endregion

        #region Ctor
        public CollectionInfoWindow()
        {
            InitializeComponent();
        }

        public CollectionInfoWindow(Account account)
            : this()
        {
            m_account = account;
        }
        #endregion

        #region OnLoaded
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }
        #endregion
    }
}