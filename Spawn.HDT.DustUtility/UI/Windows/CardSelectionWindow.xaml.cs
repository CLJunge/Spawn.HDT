using MahApps.Metro.Controls.Dialogs;
using Spawn.HDT.DustUtility.UI.Dialogs;
using System.Collections.Generic;
using System.Windows;

namespace Spawn.HDT.DustUtility.UI.Windows
{
    public partial class CardSelectionWindow
    {
        #region Member Variables
        private CustomDialog m_dialog;
        private MetroDialogSettings m_dialogSettings;
        private CardCountDialog m_cardCountDialog;
        private DataGridCardItem m_currentItem;
        #endregion

        #region DP
        #region DustAmount
        public int DustAmount
        {
            get { return (int)GetValue(DustAmountProperty); }
            set { SetValue(DustAmountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DustAmount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DustAmountProperty =
            DependencyProperty.Register("DustAmount", typeof(int), typeof(CardSelectionWindow), new PropertyMetadata(0));
        #endregion

        #region CommonsCount
        public int CommonsCount
        {
            get { return (int)GetValue(CommonsCountProperty); }
            set { SetValue(CommonsCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommonsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommonsCountProperty =
            DependencyProperty.Register("CommonsCount", typeof(int), typeof(CardSelectionWindow), new PropertyMetadata(0));
        #endregion

        #region RaresCount
        public int RaresCount
        {
            get { return (int)GetValue(RaresCountProperty); }
            set { SetValue(RaresCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RaresCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RaresCountProperty =
            DependencyProperty.Register("RaresCount", typeof(int), typeof(CardSelectionWindow), new PropertyMetadata(0));
        #endregion

        #region EpicsCount
        public int EpicsCount
        {
            get { return (int)GetValue(EpicsCountProperty); }
            set { SetValue(EpicsCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EpicsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EpicsCountProperty =
            DependencyProperty.Register("EpicsCount", typeof(int), typeof(CardSelectionWindow), new PropertyMetadata(0));
        #endregion

        #region LegendariesCount
        public int LegendariesCount
        {
            get { return (int)GetValue(LegendariesCountProperty); }
            set { SetValue(LegendariesCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LegendariesCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LegendariesCountProperty =
            DependencyProperty.Register("LegendariesCount", typeof(int), typeof(CardSelectionWindow), new PropertyMetadata(0));
        #endregion

        #region TotalAmount
        public int TotalAmount
        {
            get { return (int)GetValue(TotalAmountProperty); }
            set { SetValue(TotalAmountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalAmount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalAmountProperty =
            DependencyProperty.Register("TotalAmount", typeof(int), typeof(CardSelectionWindow), new PropertyMetadata(0));
        #endregion

        #region SaveSelection
        public bool SaveSelection
        {
            get { return (bool)GetValue(SaveSelectionProperty); }
            set { SetValue(SaveSelectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SaveSelection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SaveSelectionProperty =
            DependencyProperty.Register("SaveSelection", typeof(bool), typeof(CardSelectionWindow), new PropertyMetadata(false));
        #endregion
        #endregion

        #region Properties
        public List<DataGridCardItem> CurrentItems { get; private set; }
        #endregion

        #region Ctor
        public CardSelectionWindow()
        {
            InitializeComponent();

            CurrentItems = new List<DataGridCardItem>();

            cardsGrid.GridItems.Clear();

            m_dialogSettings = new MetroDialogSettings()
            {
                AnimateShow = true,
                AnimateHide = true,
                ColorScheme = MetroDialogColorScheme.Accented
            };
        }

        public CardSelectionWindow(List<DataGridCardItem> savedItems)
            : this()
        {
            for (int i = 0; i < savedItems.Count; i++)
            {
                AddItem(savedItems[i]);
            }

            CurrentItems = savedItems;

            cbSaveSelection.IsChecked = CurrentItems.Count > 0;
        }
        #endregion

        #region Events
        #region OnCardsGridItemDropped
        private async void OnCardsGridItemDropped(object sender, DataGridCardItemEventArgs e)
        {
            m_currentItem = e.Item;

            if (m_currentItem.Count > 1)
            {
                m_cardCountDialog = new CardCountDialog(e.Item.Name, e.Item.Count);
                m_cardCountDialog.AcceptButton.Click += OnAcceptClick;
                m_cardCountDialog.CancelButton.Click += OnCancelClick;

                m_dialog = new CustomDialog(this)
                {
                    Content = m_cardCountDialog
                };

                await this.ShowMetroDialogAsync(m_dialog, m_dialogSettings);
            }
            else if (m_currentItem.Count == 1)
            {
                AddItem(m_currentItem);
            }
            else { }
        }
        #endregion

        #region OnCardsGridRowDeleted
        private void OnCardsGridRowDeleted(object sender, DataGridCardItemEventArgs e)
        {
            RemoveItem(e.Item);
        }
        #endregion

        #region OnClearGridClick
        private void OnClearGridClick(object sender, RoutedEventArgs e)
        {
            ClearItems();
        }
        #endregion

        #region CardCountDialog Events
        #region OnAcceptClick
        private void OnAcceptClick(object sender, RoutedEventArgs e)
        {
            if (m_cardCountDialog.NumericUpDownCtrl.Value > 0)
            {
                int nNewCount = System.Convert.ToInt32(m_cardCountDialog.NumericUpDownCtrl.Value.Value);

                m_currentItem.Count = nNewCount;
                m_currentItem.Dust = m_currentItem.Tag.GetDustValue(nNewCount);

                AddItem(m_currentItem);
            }
            else { }

            this.HideMetroDialogAsync(m_dialog, m_dialogSettings);
        }
        #endregion

        #region OnCancelClick
        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.HideMetroDialogAsync(m_dialog, m_dialogSettings);
        }
        #endregion
        #endregion
        #endregion

        #region AddItem
        private void AddItem(DataGridCardItem item)
        {
            TotalAmount += item.Count;
            DustAmount += item.Dust;

            switch (item.Rarity)
            {
                case HearthDb.Enums.Rarity.COMMON:
                    CommonsCount += item.Count;
                    break;

                case HearthDb.Enums.Rarity.RARE:
                    RaresCount += item.Count;
                    break;

                case HearthDb.Enums.Rarity.EPIC:
                    EpicsCount += item.Count;
                    break;

                case HearthDb.Enums.Rarity.LEGENDARY:
                    LegendariesCount += item.Count;
                    break;
            }

            cardsGrid.GridItems.Add(item);

            CurrentItems.Add(item);
        }
        #endregion

        #region RemoveItem
        private void RemoveItem(DataGridCardItem item)
        {
            TotalAmount -= item.Count;
            DustAmount -= item.Dust;

            switch (item.Rarity)
            {
                case HearthDb.Enums.Rarity.COMMON:
                    CommonsCount -= item.Count;
                    break;

                case HearthDb.Enums.Rarity.RARE:
                    RaresCount -= item.Count;
                    break;

                case HearthDb.Enums.Rarity.EPIC:
                    EpicsCount -= item.Count;
                    break;

                case HearthDb.Enums.Rarity.LEGENDARY:
                    LegendariesCount -= item.Count;
                    break;
            }

            cardsGrid.GridItems.Remove(item);

            CurrentItems.Remove(item);
        }
        #endregion

        #region ClearItems
        private void ClearItems()
        {
            cardsGrid.GridItems.Clear();

            TotalAmount = 0;
            DustAmount = 0;

            CommonsCount = 0;
            RaresCount = 0;
            EpicsCount = 0;
            LegendariesCount = 0;

            CurrentItems.Clear();
        }
        #endregion
    }
}