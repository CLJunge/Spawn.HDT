namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        #region Properties
        #region ReloadRequired
        public bool ReloadRequired { get; set; } = true;
        #endregion
        #endregion

        #region Initialize
        public abstract void Initialize();
        #endregion
    }
}