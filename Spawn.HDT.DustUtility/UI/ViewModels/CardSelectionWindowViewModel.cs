#region Using
using GalaSoft.MvvmLight.CommandWpf;
using HearthMirror;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class CardSelectionWindowViewModel : ViewModelBase
    {
        #region Member Variables
        private string m_strWindowTitle;
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

        #region CardSelection
        public CardSelectionManager CardSelection => DustUtilityPlugin.CardSelection;
        #endregion

        #region ClearCommand
        public ICommand ClearCommand => new RelayCommand(CardSelection.Clear, () => CardSelection.CardItems.Count > 0);
        #endregion

        #region ImportLatestPackCommand
        public ICommand ImportLatestPackCommand => new RelayCommand(CardSelection.ImportLatestPack, () => Reflection.GetPackCards()?.Count > 0);
        #endregion

        #region DisenchantSelectionCommand
        public ICommand DisenchantSelectionCommand => new RelayCommand(CardSelection.DisenchantSelection, () =>
        {
            return DustUtilityPlugin.Config.AutoDisenchanting && CardSelection.CardItems.Count > 0 && !DustUtilityPlugin.IsOffline;
        });
        #endregion
        #endregion

        #region Ctor
        public CardSelectionWindowViewModel()
        {
            WindowTitle = "Dust Utility - Selection";
        }
        #endregion
    }
}