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
        private CardItem m_draggedItem;
        #endregion

        #region Custom Events
        public event EventHandler<CardItemEventArgs> ItemDropped;

        private void OnItemDropped(CardItem item)
        {
            if (ItemDropped != null)
            {
                ItemDropped(this, new CardItemEventArgs(item, -1));
            }
            else { }
        }
        #endregion

        #region Events
        #region OnDataGridMouseMove
        private void OnDataGridMouseMove(object sender, MouseEventArgs e)
        {
            ItemsContainer.ReleaseMouseCapture();

            if (AllowDrag && (m_startPosition != null && m_startPosition.HasValue))
            {
                if (m_draggedItem == null && ItemsContainer.SelectedIndex > -1)
                {
                    m_draggedItem = ItemsContainer.SelectedItem as CardItem;
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

        #region OnDataGridPreviewMouseLeftButtonDown
        private void OnDataGridPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_startPosition = e.GetPosition(null);
        }
        #endregion

        #region OnDataGridDragEnter
        private void OnDataGridDragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("item"))
            {
                e.Effects = DragDropEffects.None;
            }
            else { }
        }
        #endregion

        #region OnDataGridDrop
        private void OnDataGridDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("item"))
            {
                CardItem item = e.Data.GetData("item") as CardItem;

                System.Diagnostics.Debug.WriteLine($"Dropped {item.Name}");

                OnItemDropped(item);
            }
            else { }
        }
        #endregion
        #endregion
    }
}