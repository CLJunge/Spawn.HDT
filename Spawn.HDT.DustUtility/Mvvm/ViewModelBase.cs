namespace Spawn.HDT.DustUtility.Mvvm
{
    public class ViewModelBase : BindableBase
    {
        #region Member Variables
        private string m_strWindowTitle;
        #endregion

        #region Properties
        #region WindowTitle
        public string WindowTitle
        {
            get => m_strWindowTitle;
            set => Set(ref m_strWindowTitle, value);
        }
        #endregion
        #endregion
    }
}