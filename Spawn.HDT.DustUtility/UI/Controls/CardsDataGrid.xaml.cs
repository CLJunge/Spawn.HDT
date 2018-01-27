using Spawn.HDT.DustUtility.UI.Converters;
using Spawn.HDT.DustUtility.UI.Models;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Spawn.HDT.DustUtility.UI.Controls
{
    public partial class CardsDataGrid
    {
        #region Member Variables
        private bool m_blnDblClick;
        private bool m_blnDateColumnVisible;
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
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(CardsDataGrid));
        #endregion

        #region DateColumnVisible
        public bool DateColumnVisible
        {
            get => m_blnDateColumnVisible;
            set
            {
                if (value)
                {
                    AddDateColumn();
                }
                else
                {
                    RemoveDateColumn();
                }

                m_blnDateColumnVisible = value;
            }
        }
        #endregion

        #region ColoredCountColumn
        public bool ColoredCountColumn { get; set; }
        #endregion

        #region AllowDrag
        public bool AllowDrag { get; set; }
        #endregion

        #region ContextMenuEnabled
        public bool ContextMenuEnabled { get; set; }
        #endregion
        #endregion

        #region Custom Events
        public event EventHandler<DataGridCardItemEventArgs> RowDeleted;
        #endregion

        #region Ctor
        public CardsDataGrid()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        #region OnDataGridMouseDoubleClick
        private void OnDataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            m_blnDblClick = true;

            OpenPopup();
        }
        #endregion

        #region OnDataGridMouseDown
        private void OnDataGridMouseDown(object sender, MouseButtonEventArgs e)
        {
            IInputElement element = dataGrid.InputHitTest(e.GetPosition(dataGrid));

            if (element is ScrollViewer)
            {
                dataGrid.SelectedIndex = -1;
            }
            else if (element is Border)
            {
                int nIndex = ((element as Border).TemplatedParent as DataGridRow).GetIndex();

                dataGrid.SelectedIndex = nIndex;
            }
            else { }

            if (!m_blnDblClick)
            {
                ClosePopup();
            }
            else { }

            m_blnDblClick = false;
        }
        #endregion

        #region OnDataGridSelectionChanged
        private void OnDataGridSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ClosePopup();
        }
        #endregion

        #region OnDeleteRowClick
        private void OnDeleteRowClick(object sender, RoutedEventArgs e)
        {
            int nIndex = dataGrid.SelectedIndex;

            System.Diagnostics.Debug.WriteLine($"Deleting row at index \"{nIndex}\"");

            if (dataGrid.SelectedItem is DataGridCardItem)
            {
                //MetroWindow window = Window.GetWindow(this) as MetroWindow;

                //MessageDialogResult result = await window.ShowMessageAsync(string.Empty, "Are you sure you want to remove the selected card?", MessageDialogStyle.AffirmativeAndNegative);

                //if (result == MessageDialogResult.Affirmative)
                //{
                DataGridCardItem item = dataGrid.SelectedItem as DataGridCardItem;

                ((IList)ItemsSource).RemoveAt(nIndex);

                if (RowDeleted != null)
                {
                    RowDeleted(this, new DataGridCardItemEventArgs(item, nIndex));
                }
                else { }
                //}
                //else { }
            }
            else { }
        }
        #endregion

        #region OnContextMenuOpening
        private void OnContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
            e.Handled = !ContextMenuEnabled;
        }
        #endregion

        #region OnPopupMouseDown
        private void OnPopupMouseDown(object sender, MouseButtonEventArgs e)
        {
            ClosePopup();
        }
        #endregion
        #endregion

        #region OpenPopup
        private void OpenPopup()
        {
            if (dataGrid.SelectedItem is DataGridCardItem)
            {
                cardImagePopup.IsOpen = true;

                //cardImageContainer.CardWrapper = (dataGrid.SelectedItem as DataGridCardItem).Tag;
            }
            else { }
        }
        #endregion

        #region ClosePopup
        private void ClosePopup()
        {
            if (cardImagePopup.IsOpen)
            {
                cardImagePopup.IsOpen = false;

                //cardImageContainer.CardWrapper = null;
            }
            else { }
        }
        #endregion

        #region AddDateColumn
        private void AddDateColumn()
        {
            DataGridTextColumn dateColumn = new DataGridTextColumn
            {
                Header = "Date",
                Binding = new Binding("Timestamp") { Converter = new TimestampConverter() }
            };

            dataGrid.Columns.Add(dateColumn);
        }
        #endregion

        #region RemoveDateColumn
        private void RemoveDateColumn()
        {
            dataGrid.Columns.RemoveAt(dataGrid.Columns.Count - 1);
        }
        #endregion
    }
}