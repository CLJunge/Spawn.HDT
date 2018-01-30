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
                List<Card> lstCards = new List<Card>
                {
                    new Card("UNG_900", 2, false),
                };

                DeckItems.Add(new DeckItem(new Deck() { Id = 4323, Hero = "HERO_05", Name = "Test Deck2", Cards = lstCards }));
                DeckItems.Add(new DeckItem(new Deck() { Id = 43523, Hero = "HERO_03", Name = "Test Deck3", Cards = lstCards }));
                DeckItems.Add(new DeckItem(new Deck() { Id = 435433, Hero = "HERO_02", Name = "Test Deck4", Cards = lstCards }));
                DeckItems.Add(new DeckItem(new Deck() { Id = 466823, Hero = "HERO_08", Name = "Test Deck5", Cards = lstCards }));
                DeckItems.Add(new DeckItem(new Deck() { Id = 435923, Hero = "HERO_01", Name = "Test Deck6", Cards = lstCards }));
                DeckItems.Add(new DeckItem(new Deck() { Id = 287653, Hero = "HERO_02", Name = "Test Deck7", Cards = lstCards }));
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