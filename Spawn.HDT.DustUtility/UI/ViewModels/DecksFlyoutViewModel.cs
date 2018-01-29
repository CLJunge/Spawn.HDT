using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Input;

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class DecksFlyoutViewModel : ViewModelBase
    {
        #region Properties
        #region ShowDeckListCommand
        public ICommand ShowDeckListCommand => new RelayCommand(ShowDeckList);
        #endregion

        #region ToggleDeckCommand
        public ICommand ToggleDeckCommand => new RelayCommand(ToggleDeck);
        #endregion
        #endregion

        #region Initialize
        public override void Initialize()
        {
            if (ReloadRequired)
            {
                ReloadRequired = false;
            }
            else { }
        }
        #endregion

        #region ShowDeckList
        private void ShowDeckList()
        {
        }
        #endregion

        #region ToggleDeck
        private void ToggleDeck()
        {
        }
        #endregion
    }
}