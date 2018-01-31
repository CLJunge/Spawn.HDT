namespace Spawn.HDT.DustUtility.Services
{
    public interface IResultProvider<T>
    {
        #region Methods
        T GetResult();
        #endregion
    }
}