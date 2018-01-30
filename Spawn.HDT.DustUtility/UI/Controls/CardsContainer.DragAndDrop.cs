#region Using
using Spawn.HDT.DustUtility.UI.Models;
using System;
using System.Windows;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.UI.Controls
{
    public partial class CardsContainer
    {
        #region Member Variables
        private Point? m_startPosition;
        private CardItemModel m_draggedItem;
        #endregion

        #region Custom Events
        public event EventHandler<CardItemEventArgs> ItemDropped;

        private void OnItemDropped(CardItemModel item)
        {
            if (ItemDropped != null)
            {
                ItemDropped(this, new CardItemEventArgs(item, -1));
            }
            else { }
        }
        #endregion

        #region Events
        #region OnListViewMouseMove
        private void OnListViewMouseMove(object sender, MouseEventArgs e)
        {
            ItemsContainer.ReleaseMouseCapture();

            if (AllowDrag && (m_startPosition != null && m_startPosition.HasValue))
            {
                if (m_draggedItem == null && ItemsContainer.SelectedIndex > -1)
                {
                    m_draggedItem = ItemsContainer.SelectedItem as CardItemModel;
                }
                else { }

                Point position = e.GetPosition(null);

                Vector diff = m_startPosition.Value - position;

                if (m_draggedItem != null
                    && (Math.Abs(diff.X) > (SystemParameters.MinimumHorizontalDragDistance / 3)
                    && Math.Abs(diff.Y) > (SystemParameters.MinimumVerticalDragDistance / 3)))
                {
                    System.Diagnostics.Debug.WriteLine($"dragging {m_draggedItem.Name}");

                    DataObject data = new DataObject("item", m_draggedItem.CreateCopy());

                    DragDrop.DoDragDrop(ItemsContainer, data, DragDropEffects.Copy);

                    m_startPosition = null;
                    m_draggedItem = null;
                }
                else { }
            }
            else { }
        }
        #endregion

        #region OnListViewPreviewMouseLeftButtonDown
        private void OnListViewPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_startPosition = e.GetPosition(null);
        }
        #endregion

        #region OnListViewDragEnter
        private void OnListViewDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("item"))
            {
                e.Effects = DragDropEffects.None;
            }
            else { }
        }
        #endregion

        #region OnListViewDrop
        private void OnListViewDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("item"))
            {
                CardItemModel item = e.Data.GetData("item") as CardItemModel;

                System.Diagnostics.Debug.WriteLine($"Dropped {item.Name}");

                OnItemDropped(item);
            }
            else { }
        }
        #endregion
        #endregion
    }
}