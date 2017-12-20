using System;
using System.Windows;
using System.Windows.Input;

namespace Spawn.HDT.DustUtility.UI.Components
{
    public partial class CardsDataGrid
    {
        #region Member Variables
        private Point? m_startPosition;
        private DataGridCardItem m_draggedItem;
        #endregion

        #region Custom Events
        public event EventHandler<DataGridCardItemEventArgs> ItemDropped;

        private void OnItemDropped(DataGridCardItem item)
        {
            if (ItemDropped != null)
            {
                ItemDropped(this, new DataGridCardItemEventArgs(item));
            }
            else { }
        }
        #endregion

        #region Events
        #region OnDataGridMouseMove
        private void OnDataGridMouseMove(object sender, MouseEventArgs e)
        {
            //if (cardImagePopup.IsOpen)
            //{
            //    Point position = e.GetPosition(dataGrid);

            //    cardImagePopup.HorizontalOffset = position.X + 20;
            //    cardImagePopup.VerticalOffset = position.Y;
            //}
            //else { }

            if (AllowDrag && (m_startPosition != null && m_startPosition.HasValue))
            {
                if (m_draggedItem == null && dataGrid.SelectedIndex > -1)
                {
                    m_draggedItem = dataGrid.SelectedItem as DataGridCardItem;
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

                    DragDrop.DoDragDrop(dataGrid, data, DragDropEffects.Copy);

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
            System.Diagnostics.Debug.WriteLine("drag enter");

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
            System.Diagnostics.Debug.WriteLine("drop");

            if (e.Data.GetDataPresent("item"))
            {
                DataGridCardItem item = e.Data.GetData("item") as DataGridCardItem;

                System.Diagnostics.Debug.WriteLine($"Dropped {item.Name}");

                OnItemDropped(item);
            }
            else { }
        }
        #endregion
        #endregion
    }
}