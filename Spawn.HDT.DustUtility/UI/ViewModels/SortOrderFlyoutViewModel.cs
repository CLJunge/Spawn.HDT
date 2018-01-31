using GalaSoft.MvvmLight.CommandWpf;
using Spawn.HDT.DustUtility.UI.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class SortOrderFlyoutViewModel : ViewModelBase
    {
        #region Member Variables
        private SortOrderItemModel m_selectedSortOrderItem;
        #endregion

        #region Properties
        #region SortOrderItems
        public ObservableCollection<SortOrderItemModel> SortOrderItems { get; set; }
        #endregion

        #region SelectedSortOrderItem
        public SortOrderItemModel SelectedSortOrderItem
        {
            get => m_selectedSortOrderItem;
            set => Set(ref m_selectedSortOrderItem, value);
        }
        #endregion

        #region AddItemCommand
        public ICommand AddItemCommand => new RelayCommand(AddItem);
        #endregion

        #region RemoveItemCommand
        public ICommand RemoveItemCommand => new RelayCommand(RemoveItem);
        #endregion

        #region MoveUpItemCommand
        public ICommand MoveUpItemCommand => new RelayCommand(MoveUpItem);
        #endregion

        #region MoveDownItemCommand
        public ICommand MoveDownItemCommand => new RelayCommand(MoveDownItem);
        #endregion

        #region SaveCommand
        public ICommand SaveCommand => new RelayCommand(SaveSortOrder);
        #endregion
        #endregion

        #region Ctor
        public SortOrderFlyoutViewModel()
        {
            SortOrderItems = new ObservableCollection<SortOrderItemModel>();
        }
        #endregion

        #region Initialize
        public override void Initialize()
        {
            SortOrderItems.Clear();

            SortOrder sortOrder = SortOrder.Parse(DustUtilityPlugin.Config.SortOrder);

            for (int i = 0; i < sortOrder.Count; i++)
            {
                SortOrderItems.Add(sortOrder[i]);
            }
        }
        #endregion

        private void AddItem()
        {
        }

        private void RemoveItem()
        {
        }

        private void MoveUpItem()
        {
        }

        private void MoveDownItem()
        {
        }

        private void SaveSortOrder()
        {
        }
    }
}