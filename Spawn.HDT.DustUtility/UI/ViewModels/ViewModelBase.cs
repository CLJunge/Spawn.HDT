#region Using
using System.Threading.Tasks;
#endregion

namespace Spawn.HDT.DustUtility.UI.ViewModels
{
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        #region Properties
        #region ReloadRequired
        public bool ReloadRequired { get; set; } = true;
        #endregion
        #endregion

        #region InitializeAsync
        public abstract Task InitializeAsync();
        #endregion
    }
}