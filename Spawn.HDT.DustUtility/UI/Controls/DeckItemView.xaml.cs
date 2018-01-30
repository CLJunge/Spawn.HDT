namespace Spawn.HDT.DustUtility.UI.Controls
{
    public partial class DeckItemView
    {
        #region Ctor
        public DeckItemView()
        {
            InitializeComponent();

#if DEBUG
            if (GalaSoft.MvvmLight.ViewModelBase.IsInDesignModeStatic)
            {
                DataContext = new AccountManagement.MockAccount().GetDecks()[0];
            }
            else { }
#endif
        }
        #endregion
    }
}