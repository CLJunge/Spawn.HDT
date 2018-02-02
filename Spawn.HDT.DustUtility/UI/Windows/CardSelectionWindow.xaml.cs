#region Using
using Microsoft.Practices.ServiceLocation;
using Spawn.HDT.DustUtility.UI.ViewModels;
#endregion

namespace Spawn.HDT.DustUtility.UI.Windows
{
    public partial class CardSelectionWindow
    {
        #region Ctor
        public CardSelectionWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        #region OnRemoveCardItem
        private void OnRemoveCardItem(object sender, CardItemEventArgs e) => ServiceLocator.Current.GetInstance<CardSelectionWindowViewModel>().OnRemoveCardItem(e);
        #endregion

        #region OnItemDropped
        private async void OnItemDropped(object sender, CardItemEventArgs e) => await ServiceLocator.Current.GetInstance<CardSelectionWindowViewModel>().OnItemDropped(e);
        #endregion

        #region OnClosing
        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e) => ServiceLocator.Current.GetInstance<CardSelectionWindowViewModel>().OnClosing(e);
        #endregion
        #endregion
    }
}