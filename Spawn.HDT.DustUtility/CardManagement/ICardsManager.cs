#region Using
using Spawn.HDT.DustUtility.Hearthstone;
using System.Threading.Tasks;
#endregion

namespace Spawn.HDT.DustUtility.CardManagement
{
    public interface ICardsManager
    {
        Task<CardWrapper[]> GetCardsAsync(SearchParameters parameters);
        int GetCollectionValue();
    }
}