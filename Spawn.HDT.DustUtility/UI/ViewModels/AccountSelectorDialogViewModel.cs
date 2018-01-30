#region Using
using Spawn.HDT.DustUtility.AccountManagement;
using Spawn.HDT.DustUtility.Services;
using System.Collections.ObjectModel;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class AccountSelectorDialogViewModel : ViewModelBase, IDialogResultService<string>
    {
        #region Member Variables
        private string m_strWindowTitle;
        private string m_selectedAccountString;
        #endregion

        #region Properties
        #region WindowTitle
        public string WindowTitle
        {
            get => m_strWindowTitle;
            set => Set(ref m_strWindowTitle, value);
        }
        #endregion

        #region Accounts
        public ObservableCollection<Account> Accounts { get; set; }
        #endregion

        #region SelectedAccountString
        public string SelectedAccountString
        {
            get => m_selectedAccountString;
            set => Set(ref m_selectedAccountString, value);
        }
        #endregion
        #endregion

        #region Ctor
        public AccountSelectorDialogViewModel()
        {
            WindowTitle = "Dust Utility - Select account...";

            Accounts = new ObservableCollection<Account>(DustUtilityPlugin.GetAccounts());
        }
        #endregion

        #region Initialize
        public override void Initialize()
        {
            if (!string.IsNullOrEmpty(DustUtilityPlugin.Config.LastSelectedAccount))
            {
                SelectedAccountString = DustUtilityPlugin.Config.LastSelectedAccount;
            }
            else
            {
                SelectedAccountString = Accounts[0].AccountString;
            }
        }
        #endregion

        #region GetDialogResult
        public string GetDialogResult() => SelectedAccountString;
        #endregion
    }
}