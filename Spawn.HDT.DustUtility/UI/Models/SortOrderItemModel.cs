using GalaSoft.MvvmLight;
using System.Diagnostics;

namespace Spawn.HDT.DustUtility.UI.Models
{
    [DebuggerDisplay("Name={Name} Value={Value}")]
    public class SortOrderItemModel : ObservableObject
    {
        #region Member Variables
        private SortOrder.Item m_item;
        #endregion

        #region Properties
        #region Value
        public SortOrder.Item Value
        {
            get => m_item;
            set => Set(ref m_item, value);
        }
        #endregion

        #region Name
        public string Name => SortOrder.ItemToString(Value);
        #endregion
        #endregion

        #region Ctor
        public SortOrderItemModel()
        {
            PropertyChanged += (s, e) => RaisePropertyChanged(nameof(Name));
        }

        public SortOrderItemModel(SortOrder.Item item)
            : this()
        {
            Value = item;
        }
        #endregion
    }
}