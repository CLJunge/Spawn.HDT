#region Using
using Spawn.HDT.DustUtility.AccountManagement;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public class AccountSelectorDialogViewModel : ViewModelBase
    {
        #region Member Variables
        private string m_strWindowTitle;
        private string m_selectedAccountString;
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

        #region Accounts
        public ObservableCollection<IAccount> Accounts { get; set; }
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

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Created new 'AccountSelectorDialogViewModel' instance");
        }
        #endregion

        #region InitializeAsync
        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            Accounts = new ObservableCollection<IAccount>(DustUtilityPlugin.GetAccounts());

            SelectedAccountString = !string.IsNullOrEmpty(DustUtilityPlugin.Config.LastSelectedAccount)
                ? DustUtilityPlugin.Config.LastSelectedAccount
                : Accounts[0].AccountString;

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Finished initializing");
        }
        #endregion
    }
}