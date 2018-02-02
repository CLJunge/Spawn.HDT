#region Using
using Spawn.HDT.DustUtility.UI.ViewModels;
#endregion

namespace Spawn.HDT.DustUtility.UI.Flyouts
{
    public partial class DecksFlyoutView
    {
        #region Ctor
        public DecksFlyoutView()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        #region OnListViewMouseMove
        private void OnListViewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as System.Windows.Controls.ListView).ReleaseMouseCapture();
        }
        #endregion

        #region OnListViewContextMenuOpening
        private void OnListViewContextMenuOpening(object sender, System.Windows.Controls.ContextMenuEventArgs e)
        {
            ((DecksFlyoutViewModel)DataContext).ContextMenuOpeningCommand.Execute(e);
        }
        #endregion
        #endregion
    }
}