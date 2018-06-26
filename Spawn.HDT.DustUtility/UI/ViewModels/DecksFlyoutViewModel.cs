#region Using
using CommonServiceLocator;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HearthMirror.Objects;
using Spawn.HDT.DustUtility.Logging;
#if DEBUG
using Hearthstone_Deck_Tracker.Utility.Extensions;
#endif
using Spawn.HDT.DustUtility.Messaging;
using Spawn.HDT.DustUtility.UI.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
        #region CanNotifyDirtyStatus
        public override bool CanNotifyDirtyStatus => false;
        #endregion

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
        #endregion

        #region Ctor
        public DecksFlyoutViewModel()
        {
            DeckItems = new ObservableCollection<DeckItemModel>();

            ToggleDeckMenuItemHeader = ExcludeHeaderText;

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals(nameof(SelectedDeckItem)) && SelectedDeckItem != null)
                {
                    ServiceLocator.Current.GetInstance<DeckListFlyoutViewModel>().Deck = SelectedDeckItem.Deck;
                }
            };

#if DEBUG
            if (IsInDesignMode)
            {
                InitializeAsync().Forget();
            }
#endif

            Messenger.Default.Register<CMOpeningMessage>(this, OnContextMenuOpening);
            Messenger.Default.Register<LVMouseDblClickMessage>(this, OnListViewMouseDoubleClick);

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Created new 'DecksFlyoutViewModel' instance");
        }
        #endregion

        #region Dtor
        ~DecksFlyoutViewModel() => Messenger.Default.Unregister(this);
        #endregion

        #region InitializeAsync
        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

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

                    DeckItems.Add(item);
                }

                ToggleDeckMenuItemHeader = ExcludeHeaderText;

                ReloadRequired = false;
            }

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Finished initializing");
        }
        #endregion

        #region ShowDeckList
        private void ShowDeckList()
        {
            ServiceLocator.Current.GetInstance<MainViewModel>().OpenFlyout(DustUtilityPlugin.MainWindow.DeckListFlyout);

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Showing list for '{SelectedDeckItem.Name}'");
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
                DustUtilityPlugin.CurrentAccount.ExcludeDeckFromSearch(SelectedDeckItem.DeckId);

                SelectedDeckItem.Opacity = .5;
            }

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Toggled '{SelectedDeckItem.Name}'");
        }
        #endregion

        #region OnContextMenuOpening
        private void OnContextMenuOpening(CMOpeningMessage message)
        {
            if (message.FlyoutName?.Equals(DustUtilityPlugin.DecksFlyoutName) ?? false)
            {
                bool blnDeckSelected = SelectedDeckItem != null;

                message.EventArgs.Handled = !blnDeckSelected;

                if (blnDeckSelected)
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
            }
        }
        #endregion

        #region OnListViewMouseDoubleClick
        private void OnListViewMouseDoubleClick(LVMouseDblClickMessage message)
        {
            if ((message.FlyoutName?.Equals(DustUtilityPlugin.DecksFlyoutName) ?? false) && message.EventArgs?.LeftButton == MouseButtonState.Pressed)
            {
                ShowDeckList();
            }
        }
        #endregion
    }
}