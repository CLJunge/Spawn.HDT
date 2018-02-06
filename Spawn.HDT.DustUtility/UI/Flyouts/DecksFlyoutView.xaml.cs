#region Using
using Microsoft.Practices.ServiceLocation;
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
            ServiceLocator.Current.GetInstance<DecksFlyoutViewModel>().OnContextMenuOpening(e);
        }
        #endregion

        #region OnListViewMouseDoubleClick
        private void OnListViewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                ServiceLocator.Current.GetInstance<DecksFlyoutViewModel>().ShowDeckListCommand.Execute(null);
            }
            else { }
        }
        #endregion
        #endregion
    }
}