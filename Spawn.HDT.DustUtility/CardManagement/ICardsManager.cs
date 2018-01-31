#region Using
using System.Threading.Tasks;
#endregion

namespace Spawn.HDT.DustUtility.CardManagement
{
    public interface ICardsManager
    {
        #region Methods
        Task<SearchResult> GetSearchResultAsync(SearchParameters parameters);
        int GetCollectionValue();
        #endregion
    }
}