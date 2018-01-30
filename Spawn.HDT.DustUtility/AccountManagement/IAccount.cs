using HearthMirror.Objects;
using Hearthstone_Deck_Tracker.Enums;
using Spawn.HDT.DustUtility.CardManagement.Offline;
using System.Collections.Generic;

namespace Spawn.HDT.DustUtility.AccountManagement
{
    public interface IAccount
    {
        BattleTag BattleTag { get; }
        Region Region { get; }
        string DisplayString { get; }
        string AccountString { get; }
        bool IsEmpty { get; }
        bool IsValid { get; }
        AccountPreferences Preferences { get; }
        bool HasFiles { get; }

        List<Card> GetCollection();
        List<Deck> GetDecks();
        List<CachedHistoryCard> GetHistory();
        void ExcludeDeckInSearch(long nDeckId);
        void IncludeDeckInSearch(long nDeckId);
        bool IsDeckExcludedFromSearch(long nDeckId);
        void SavePreferences();
        bool Equals(object obj);
        int GetHashCode();
    }
}