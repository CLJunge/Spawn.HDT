using System.Collections.Generic;
using System.Windows;

namespace Spawn.HDT.DustUtility.UI.Dialogs
{
    public partial class AccountSelectorDialog
    {
        #region Properties
        public string SelectedAccount { get; private set; }
        #endregion

        #region Ctor
        public AccountSelectorDialog()
        {
            InitializeComponent();
        }

        public AccountSelectorDialog(List<Account> accounts)
            : this()
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                cbAccounts.Items.Add(accounts[i]);
            }

            if (accounts.Count > 0)
            {
                if (string.IsNullOrEmpty(Settings.LastSelectedAccount))
                {
                    cbAccounts.SelectedIndex = 0;
                }
                else
                {
                    cbAccounts.SelectedValue = Settings.LastSelectedAccount;
                }
            }
            else { }
        }
        #endregion

        #region Events
        #region OnOkClick
        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            SelectedAccount = cbAccounts.SelectedValue.ToString();

            DialogResult = true;

            Close();
        }
        #endregion

        #region OnCancelClick
        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion
        #endregion
    }
}