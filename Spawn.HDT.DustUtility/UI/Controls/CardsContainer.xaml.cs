using Spawn.HDT.DustUtility.UI.Models;
using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
        public event EventHandler<CardItemEventArgs> RowDeleted;
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
            IInputElement element = ItemsContainer.InputHitTest(e.GetPosition(ItemsContainer));

            //TODO
            //if (element is ScrollViewer)
            //{
            //    ItemsContainer.SelectedIndex = -1;
            //}
            //else if (element is Border)
            //{
            //    int nIndex = ((element as Border).TemplatedParent as DataGridRow).GetIndex();

            //    ItemsContainer.SelectedIndex = nIndex;
            //}
            //else { }

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

        #region OnDeleteRowClick
        private void OnDeleteRowClick(object sender, RoutedEventArgs e)
        {
            int nIndex = ItemsContainer.SelectedIndex;

            if (ItemsContainer.SelectedItem is CardItem selectedItem)
            {
                if (RowDeleted != null)
                {
                    RowDeleted(this, new CardItemEventArgs(selectedItem, nIndex));
                }
                else { }
            }
            else { }
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
            if (ItemsContainer.SelectedItem is CardItem selectedItem)
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