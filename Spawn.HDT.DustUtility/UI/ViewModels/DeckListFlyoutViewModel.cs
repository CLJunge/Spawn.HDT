#region Using
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Utility.Extensions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class DeckListFlyoutViewModel : ViewModelBase
    {
        #region Member Variables
        private HearthMirror.Objects.Deck m_deck;
        #endregion

        #region Properties
        #region CanNotifyDirtyStatus
        public override bool CanNotifyDirtyStatus => false;
        #endregion

        #region Cards
        public ObservableCollection<Card> Cards { get; set; }
        #endregion

        #region Deck
        public HearthMirror.Objects.Deck Deck
        {
            get => m_deck;
            set => Set(ref m_deck, value);
        }
        #endregion
        #endregion

        #region Ctor
        public DeckListFlyoutViewModel()
        {
            Cards = new ObservableCollection<Card>();

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName.Equals(nameof(Deck)))
                {
                    InitializeAsync().Forget();
                }
            };

            if (IsInDesignMode)
            {
                Deck = DustUtilityPlugin.CurrentAccount.GetDecks()[0];
            }

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Created new 'DeckListFlyoutViewModel' instance");
        }
        #endregion

        #region InitializeAsync
        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            Cards.Clear();

            for (int i = 0; i < Deck?.Cards.Count; i++)
            {
                if (HearthDb.Cards.Collectible.ContainsKey(Deck.Cards[i].Id))
                {
                    HearthDb.Card dbCard = HearthDb.Cards.Collectible[Deck.Cards[i].Id];

                    Card card = new Card(dbCard)
                    {
                        Count = Deck.Cards[i].Count
                    };

                    Cards.Add(card);
                }
            }

            DustUtilityPlugin.MainWindow.DeckListFlyout.Header = Deck?.Name ?? "Deck List";

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Finished initializing");
        }
        #endregion
    }
}