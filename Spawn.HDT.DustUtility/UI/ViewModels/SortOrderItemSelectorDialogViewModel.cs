#region Using
using Spawn.HDT.DustUtility.Logging;
using Spawn.HDT.DustUtility.UI.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class SortOrderItemSelectorDialogViewModel : ViewModelBase
    {
        #region Member Variables
        private string m_strWindowTitle;
        private SortOrderItemModel m_selectedSortOrderItem;
        private int m_nSelectedSortOrderItemIndex;
        #endregion

        #region Properties
        #region CanNotifyDirtyStatus
        public override bool CanNotifyDirtyStatus => false;
        #endregion

        #region WindowTitle
        public string WindowTitle
        {
            get => m_strWindowTitle;
            set => Set(ref m_strWindowTitle, value);
        }
        #endregion

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
        #endregion

        #region Ctor
        public SortOrderItemSelectorDialogViewModel()
        {
            WindowTitle = "Dust Utility - Add new item...";

            SortOrderItems = new ObservableCollection<SortOrderItemModel>();

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Created new 'SortOrderItemSelectorDialogViewModel' instance");
        }
        #endregion

        #region InitializeAsync
        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            SortOrderItems?.Clear();

            SelectedSortOrderItemIndex = 0;

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Finished initializing");
        }
        #endregion

        #region UpdateItems
        public void UpdateItems(List<SortOrderItemModel> lstItems)
        {
            SortOrderItems?.Clear();

            for (int i = 0; i < lstItems?.Count; i++)
                SortOrderItems?.Add(lstItems[i]);
        }
        #endregion
    }
}