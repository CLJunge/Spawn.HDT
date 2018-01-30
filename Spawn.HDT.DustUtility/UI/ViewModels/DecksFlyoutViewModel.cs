#region Using
using GalaSoft.MvvmLight.CommandWpf;
using HearthMirror.Objects;
using Microsoft.Practices.ServiceLocation;
using Spawn.HDT.DustUtility.UI.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class DecksFlyoutViewModel : ViewModelBase
    {
        #region Constants
        private const string IncludeHeaderText = "Include (Search)";
        private const string ExcludeHeaderText = "Exclude (Search)";
        #endregion

        #region Member Variables
        private DeckItemModel m_selectedDeckItem;
        private string m_strToggleDeckMenuItemHeader;
        #endregion

        #region Properties
        #region DeckItems
        public ObservableCollection<DeckItemModel> DeckItems { get; set; }
        #endregion

        #region SelectedDeckItem
        public DeckItemModel SelectedDeckItem
        {
            get => m_selectedDeckItem;
            set => Set(ref m_selectedDeckItem, value);
        }
        #endregion

        #region ToggleDeckMenuItemHeader
        public string ToggleDeckMenuItemHeader
        {
            get => m_strToggleDeckMenuItemHeader;
            set => Set(ref m_strToggleDeckMenuItemHeader, value);
        }
        #endregion

        #region ShowDeckListCommand
        public ICommand ShowDeckListCommand => new RelayCommand(ShowDeckList);
        #endregion

        #region ToggleDeckCommand
        public ICommand ToggleDeckCommand => new RelayCommand(ToggleDeck);
        #endregion

        #region ContextMenuOpeningCommand
        public ICommand ContextMenuOpeningCommand => new RelayCommand(OnContextMenuOpening);
        #endregion
        #endregion

        #region Ctor
        public DecksFlyoutViewModel()
        {
            DeckItems = new ObservableCollection<DeckItemModel>();

            ToggleDeckMenuItemHeader = ExcludeHeaderText;

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals(nameof(SelectedDeckItem)))
                {
                    ServiceLocator.Current.GetInstance<DeckListFlyoutViewModel>().Deck = SelectedDeckItem.Deck;
                }
                else { }
            };
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
                    DeckItemModel item = new DeckItemModel(lstDecks[i]);

                    if (DustUtilityPlugin.CurrentAccount.IsDeckExcludedFromSearch(item.DeckId))
                    {
                        item.Opacity = .5;
                    }
                    else { }

                    DeckItems.Add(item);
                }

                ToggleDeckMenuItemHeader = ExcludeHeaderText;

                ReloadRequired = false;
            }
            else { }
        }
        #endregion

        #region ShowDeckList
        private void ShowDeckList()
        {
            ServiceLocator.Current.GetInstance<MainViewModel>().OpenFlyout(DustUtilityPlugin.MainWindow.DeckListFlyout);
        }
        #endregion

        #region ToggleDeck
        private void ToggleDeck()
        {
            if (DustUtilityPlugin.CurrentAccount.IsDeckExcludedFromSearch(SelectedDeckItem.DeckId))
            {
                DustUtilityPlugin.CurrentAccount.IncludeDeckInSearch(SelectedDeckItem.DeckId);

                SelectedDeckItem.Opacity = 1;
            }
            else
            {
                DustUtilityPlugin.CurrentAccount.ExcludeDeckInSearch(SelectedDeckItem.DeckId);

                SelectedDeckItem.Opacity = .5;
            }
        }
        #endregion

        #region OnContextMenuOpening
        private void OnContextMenuOpening()
        {
            if (DustUtilityPlugin.CurrentAccount.IsDeckExcludedFromSearch(SelectedDeckItem.DeckId))
            {
                ToggleDeckMenuItemHeader = IncludeHeaderText;
            }
            else
            {
                ToggleDeckMenuItemHeader = ExcludeHeaderText;
            }
        }
        #endregion
    }
}