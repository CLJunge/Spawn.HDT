#region Using
using GalaSoft.MvvmLight.CommandWpf;
using HearthMirror.Objects;
using Spawn.HDT.DustUtility.UI.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class DecksFlyoutViewModel : ViewModelBase
    {
        #region Properties
        #region DeckItems
        public ObservableCollection<DeckItem> DeckItems { get; set; }
        #endregion

        #region ShowDeckListCommand
        public ICommand ShowDeckListCommand => new RelayCommand(ShowDeckList);
        #endregion

        #region ToggleDeckCommand
        public ICommand ToggleDeckCommand => new RelayCommand(ToggleDeck);
        #endregion
        #endregion

        #region Ctor
        public DecksFlyoutViewModel()
        {
            DeckItems = new ObservableCollection<DeckItem>();

#if DEBUG
            if (IsInDesignMode)
            {
                Initialize();
            }
            else { }
#endif
        }
        #endregion

        #region Initialize
        public override void Initialize()
        {
            if (ReloadRequired)
            {
                DeckItems.Clear();

                List<Deck> lstDecks = DustUtilityPlugin.CurrentAccount.GetDecks();

                for (int i = 0; i < lstDecks.Count; i++)
                {
                    DeckItems.Add(new DeckItem(lstDecks[i]));
                }

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