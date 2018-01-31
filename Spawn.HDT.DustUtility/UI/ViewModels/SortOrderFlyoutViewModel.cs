#region Using
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Practices.ServiceLocation;
using Spawn.HDT.DustUtility.Services;
using Spawn.HDT.DustUtility.UI.Dialogs;
using Spawn.HDT.DustUtility.UI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class SortOrderFlyoutViewModel : ViewModelBase
    {
        #region Member Variables
        private SortOrderItemModel m_selectedSortOrderItem;
        private int m_nSelectedSortOrderItemIndex;
        private int m_nMaxCount;
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

        #region SelectedSortOrderItemIndex
        public int SelectedSortOrderItemIndex
        {
            get => m_nSelectedSortOrderItemIndex;
            set => Set(ref m_nSelectedSortOrderItemIndex, value);
        }
        #endregion

        #region AddItemCommand
        public ICommand AddItemCommand => new RelayCommand(AddItem, () => SortOrderItems.Count < m_nMaxCount);
        #endregion

        #region RemoveItemCommand
        public ICommand RemoveItemCommand => new RelayCommand(RemoveItem, () => SelectedSortOrderItem != null);
        #endregion

        #region MoveUpItemCommand
        public ICommand MoveUpItemCommand => new RelayCommand(MoveUpItem, () =>
        {
            return SelectedSortOrderItem != null && SelectedSortOrderItemIndex > 0;
        });
        #endregion

        #region MoveDownItemCommand
        public ICommand MoveDownItemCommand => new RelayCommand(MoveDownItem, () =>
        {
            return SelectedSortOrderItem != null && SelectedSortOrderItemIndex < (SortOrderItems.Count - 1);
        });
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

            m_nMaxCount = Enum.GetValues(typeof(SortOrder.OrderItem)).Length;
        }
        #endregion

        #region AddItem
        private void AddItem()
        {
            List<SortOrderItemModel> lstUnusedItems = GetUnusedSortOrderItems();

            SortOrderItemSelectorDialogViewModel viewModel = ServiceLocator.Current.GetInstance<SortOrderItemSelectorDialogViewModel>();

            viewModel.SortOrderItems.Clear();

            for (int i = 0; i < lstUnusedItems.Count; i++)
            {
                viewModel.SortOrderItems.Add(lstUnusedItems[i]);
            }

            using (IDialogService dialogService = ServiceLocator.Current.GetInstance<IDialogService>())
            {
                if (dialogService.ShowDialog<SortOrderItemSelectorDialogView>())
                {
                    SortOrder.OrderItem orderItem = dialogService.GetDialogResult<SortOrder.OrderItem>();

                    SortOrderItems.Add(new SortOrderItemModel(orderItem));
                }
                else { }
            }
        }
        #endregion

        #region RemoveItem
        private void RemoveItem()
        {
            int nIndex = SelectedSortOrderItemIndex;

            SortOrderItems.Remove(SelectedSortOrderItem);

            SelectedSortOrderItemIndex = (nIndex > 0 ? nIndex - 1 : 0);
        }
        #endregion

        #region MoveUpItem
        private void MoveUpItem()
        {
            int nIndex = SortOrderItems.IndexOf(SelectedSortOrderItem);

            SortOrderItems.Move(nIndex, nIndex - 1);
        }
        #endregion

        #region MoveDownItem
        private void MoveDownItem()
        {
            int nIndex = SortOrderItems.IndexOf(SelectedSortOrderItem);

            SortOrderItems.Move(nIndex, nIndex + 1);
        }
        #endregion

        #region SaveSortOrder
        private void SaveSortOrder()
        {
            string strOrder = string.Empty;

            if (SortOrderItems.Count >= 1)
            {
                strOrder = SortOrderItems[0].Value.ToString();

                for (int i = 1; i < SortOrderItems.Count; i++)
                {
                    strOrder = $"{strOrder};{SortOrderItems[i].Value}";
                }
            }
            else { }

            DustUtilityPlugin.Config.SortOrder = strOrder;

            DustUtilityPlugin.MainWindow.SortOrderFlyout.IsOpen = false;
        }
        #endregion

        #region GetUnusedSortOrderItems
        private List<SortOrderItemModel> GetUnusedSortOrderItems()
        {
            List<SortOrderItemModel> lstRet = new List<SortOrderItemModel>();

            SortOrder.OrderItem[] vItems = (SortOrder.OrderItem[])Enum.GetValues(typeof(SortOrder.OrderItem));

            List<SortOrderItemModel> lstModels = new List<SortOrderItemModel>(SortOrderItems);

            for (int i = 0; i < vItems.Length; i++)
            {
                SortOrderItemModel model = new SortOrderItemModel(vItems[i]);

                if (lstModels.Find(m => m.Value == model.Value) == null)
                {
                    lstRet.Add(model);
                }
                else { }
            }

            return lstRet;
        }
        #endregion
    }
}