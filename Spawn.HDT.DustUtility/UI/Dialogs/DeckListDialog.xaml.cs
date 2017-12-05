using Hearthstone_Deck_Tracker.Hearthstone;
using System.Collections.Generic;

namespace Spawn.HDT.DustUtility.UI.Dialogs
{
    public partial class DeckListDialog
    {
        #region Member Variables
        private List<Card> m_lstCards;
        #endregion

        #region Ctor
        public DeckListDialog()
        {
            InitializeComponent();
        }

        public DeckListDialog(HearthMirror.Objects.Deck deck)
            : this()
        {
            Title = $"Dust Utility - {deck.Name}";

            m_lstCards = new List<Card>();

            Convert(deck);
        }
        #endregion

        #region Events
        #region OnLoaded
        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (m_lstCards?.Count > 0)
            {
                itemsControl.ItemsSource = m_lstCards;
            }
            else { }
        }
        #endregion
        #endregion

        #region Convert
        private void Convert(HearthMirror.Objects.Deck deck)
        {
            for (int i = 0; i < deck.Cards.Count; i++)
            {
                HearthDb.Card dbCard = HearthDb.Cards.Collectible[deck.Cards[i].Id];

                Card card = new Card(dbCard)
                {
                    Count = deck.Cards[i].Count
                };

                m_lstCards.Add(card);
            }
        }
        #endregion
    }
}