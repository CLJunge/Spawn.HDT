#region Using
using GalaSoft.MvvmLight.Messaging;
using Spawn.HDT.DustUtility.Logging;
using Spawn.HDT.DustUtility.Messaging;
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
        #region Static Fields
        private static System.Windows.Controls.Primitives.Popup s_openedPopup;
        #endregion

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
            InitializeComponent();

            Messenger.Default.Register<PopupMessage>(this, OnPopupMessage);

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Created new 'CardsContainer' instance");
        }
        #endregion

        #region Dtor
        ~CardsContainer() => Messenger.Default.Unregister(this);
        #endregion

        #region Events
        #region OnListViewMouseDoubleClick
        private async void OnListViewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                m_blnDblClick = true;

                await OpenPopupAsync();
            }
        }
        #endregion

        #region OnListViewMouseDown
        private void OnListViewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!m_blnDblClick)
            {
                CardImagePopup.IsOpen = false;

                if (DustUtilityPlugin.Config.ViewMode == ViewMode.Split
                    && s_openedPopup != null)
                {
                    s_openedPopup.IsOpen = false;
                }

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Closed popup");
            }

            m_blnDblClick = false;
        }
        #endregion

        #region OnListViewSelectionChanged
        private void OnListViewSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) => CardImagePopup.IsOpen = false;
        #endregion

        #region OnListViewContextMenuOpening
        private void OnListViewContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e) => e.Handled = !(ContextMenuEnabled && ItemsContainer.SelectedItem != null);
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
            CardImagePopup.IsOpen = false;

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Closed popup");
        }
        #endregion

        #region OnPopupMessage
        private void OnPopupMessage(PopupMessage message)
        {
            if (message.CloseRequest && CardImagePopup.IsOpen)
            {
                CardImagePopup.IsOpen = false;

                if (DustUtilityPlugin.Config.ViewMode == ViewMode.Split
                    && s_openedPopup != null)
                {
                    s_openedPopup.IsOpen = false;
                }

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Closed popup");
            }
        }
        #endregion
        #endregion

        #region OpenPopupAsync
        private async Task OpenPopupAsync()
        {
            if (DustUtilityPlugin.Config.ViewMode == ViewMode.Split
                && s_openedPopup != null)
            {
                s_openedPopup.IsOpen = false;
            }

            if (ItemsContainer.SelectedItem is CardItemModel selectedItem)
            {
                CardImagePopup.IsOpen = true;

#pragma warning disable S2696 // Instance members should not write to "static" fields
                s_openedPopup = CardImagePopup;
#pragma warning restore S2696 // Instance members should not write to "static" fields

                await CardImageContainer.UpdateCardWrapperAsync(selectedItem.Wrapper);

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Opened popup for '{selectedItem.Name}'");
            }
        }
        #endregion
    }
}