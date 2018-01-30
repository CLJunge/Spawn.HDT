#region Using
using Spawn.HDT.DustUtility.UI.Models;
using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.UI.Controls
{
    public partial class CardsContainer
    {
        #region Member Variables
        private bool m_blnDblClick;
        #endregion

        #region Properties
        #region ItemsSource DP
        public IEnumerable ItemsSource
        {
            get { return GetValue(ItemsSourceProperty) as IEnumerable; }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(CardsContainer));
        #endregion

        #region AllowDrag
        public bool AllowDrag { get; set; }
        #endregion

        #region ContextMenuEnabled
        public bool ContextMenuEnabled { get; set; }
        #endregion
        #endregion

        #region Custom Events
        public event EventHandler<CardItemEventArgs> RemoveCardItem;
        #endregion

        #region Ctor
        public CardsContainer()
        {
            InitializeComponent(); //TODO add more logging to this class
        }
        #endregion

        #region Events
        #region OnItemsContainerMouseDoubleClick
        private async void OnItemsContainerMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            m_blnDblClick = true;

            await OpenPopupAsync();
        }
        #endregion

        #region OnItemsContainerMouseDown
        private void OnItemsContainerMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!m_blnDblClick)
            {
                ClosePopup();
            }
            else { }

            m_blnDblClick = false;
        }
        #endregion

        #region OnItemsContainerSelectionChanged
        private void OnItemsContainerSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ClosePopup();
        }
        #endregion

        #region OnItemsContainerContextMenuOpening
        private void OnItemsContainerContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
            e.Handled = !ContextMenuEnabled;
        }
        #endregion

        #region OnMenuItemRemoveClick
        private void OnMenuItemRemoveClick(object sender, RoutedEventArgs e)
        {
            while (ItemsContainer.SelectedItems.Count > 0)
            {
                RemoveCardItem.Invoke(this, new CardItemEventArgs(ItemsContainer.SelectedItems[0] as CardItemModel, ItemsContainer.SelectedIndex));
            }
        }
        #endregion

        #region OnPopupMouseDown
        private void OnPopupMouseDown(object sender, MouseButtonEventArgs e)
        {
            ClosePopup();
        }
        #endregion
        #endregion

        #region OpenPopupAsync
        private async Task OpenPopupAsync()
        {
            if (ItemsContainer.SelectedItem is CardItemModel selectedItem)
            {
                CardImagePopup.IsOpen = true;

                await CardImageContainer.UpdateCardWrapperAsync(selectedItem.Wrapper);
            }
            else { }
        }
        #endregion

        #region ClosePopup
        private void ClosePopup()
        {
            if (CardImagePopup.IsOpen)
            {
                CardImagePopup.IsOpen = false;
            }
            else { }
        }
        #endregion
    }
}