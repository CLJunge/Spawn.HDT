#region Using
using Microsoft.Practices.ServiceLocation;
using Spawn.HDT.DustUtility.UI.ViewModels;
#endregion

namespace Spawn.HDT.DustUtility.UI.Dialogs
{
    public partial class SettingsDialogView
    {
        #region Ctor
        public SettingsDialogView()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        #region OnOkClick
        private void OnOkClick(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
        }
        #endregion

        #region OnAutoDisenchantingCheckBoxChecked
        private void OnAutoDisenchantingCheckBoxChecked(object sender, System.Windows.RoutedEventArgs e) => ServiceLocator.Current.GetInstance<SettingsDialogViewModel>().OnAutoDisenchantingCheckBoxCheckedCommand.Execute(null);
        #endregion
        #endregion
    }
}