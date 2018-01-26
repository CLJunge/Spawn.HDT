using System.Threading.Tasks;

namespace Spawn.HDT.DustUtility.CardManagement
{
    public interface ICardsManager
    {
        Task<CardWrapper[]> GetCardsAsync(SearchParameters parameters);
        int GetCollectionValue();
    }
}