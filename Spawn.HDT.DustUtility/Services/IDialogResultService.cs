namespace Spawn.HDT.DustUtility.Services
{
    public interface IDialogResultService<T>
    {
        #region Methods
        T GetDialogResult();
        #endregion
    }
}