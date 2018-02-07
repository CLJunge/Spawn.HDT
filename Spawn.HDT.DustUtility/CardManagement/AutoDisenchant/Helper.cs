#region Using
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Utility.Logging;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
#endregion

namespace Spawn.HDT.DustUtility.CardManagement.AutoDisenchant
{
    public static class Helper
    {
        #region Dicts
        private static readonly Dictionary<string, string> ArtistDict = new Dictionary<string, string>
        {
            {"enUS", "artist"},
            {"zhCN", "画家"},
            {"zhTW", "畫家"},
            {"enGB", "artist"},
            {"frFR", "artiste"},
            {"deDE", "künstler"},
            {"itIT", "artista"},
            {"jaJP", "アーティスト"},
            {"koKR", "아티스트"},
            {"plPL", "grafik"},
            {"ptBR", "artista"},
            {"ruRU", "художник"},
            {"esMX", "artista"},
            {"esES", "artista"},
        };
        private static readonly Dictionary<string, string> ManaDict = new Dictionary<string, string>
        {
            {"enUS", "mana"},
            {"zhCN", "法力值"},
            {"zhTW", "法力"},
            {"enGB", "mana"},
            {"frFR", "mana"},
            {"deDE", "mana"},
            {"itIT", "mana"},
            {"jaJP", "マナ"},
            {"koKR", "마나"},
            {"plPL", "mana"},
            {"ptBR", "mana"},
            {"ruRU", "мана"},
            {"esMX", "maná"},
            {"esES", "maná"},
        };
        private static readonly Dictionary<string, string> AttackDict = new Dictionary<string, string>
        {
            {"enUS", "attack"},
            {"zhCN", "攻击力"},
            {"zhTW", "攻擊力"},
            {"enGB", "attack"},
            {"frFR", "attaque"},
            {"deDE", "angriff"},
            {"itIT", "attacco"},
            {"jaJP", "攻撃"},
            {"koKR", "공격력"},
            {"plPL", "atak"},
            {"ptBR", "ataque"},
            {"ruRU", "атака"},
            {"esMX", "ataque"},
            {"esES", "ataque"},
        };
        #endregion

        #region GetArtistSearchString
        public static string GetArtistSearchString(string strArtist)
        {
            if (ArtistDict.TryGetValue(Config.Instance.SelectedLanguage, out string artistStr))
                return $" {artistStr}:{strArtist.Split(' ').LastOrDefault()}";
            return "";
        }
        #endregion

        #region GetManaSearchString
        public static string GetManaSearchString(int nCost)
        {
            if (ManaDict.TryGetValue(Config.Instance.SelectedLanguage, out string manaStr))
                return $" {manaStr}:{nCost}";
            return "";
        }
        #endregion

        #region GetAttackSearchString
        public static string GetAttackSearchString(int nAttack)
        {
            if (AttackDict.TryGetValue(Config.Instance.SelectedLanguage, out string atkStr))
                return $" {atkStr}:{nAttack}";
            return "";
        }
        #endregion

        #region GetSearchString
        public static string GetSearchString(Card card)
        {
            string searchString = $"{card.LocalizedName}{GetArtistSearchString(card.Artist)} {GetManaSearchString(card.Cost)}".ToLowerInvariant();
            if (card.Id == HearthDb.CardIds.Collectible.Neutral.Feugen || card.Id == HearthDb.CardIds.Collectible.Neutral.Stalagg)
                searchString += GetAttackSearchString(card.Attack);
            return searchString;
        }
        #endregion

        #region EnsureHearthstoneInForeground
        public static async Task<HearthstoneInfo> EnsureHearthstoneInForeground(HearthstoneInfo info)
        {
            if (User32.IsHearthstoneInForeground())
                return info;
            User32.ShowWindow(info.HsHandle, User32.GetHearthstoneWindowState() == WindowState.Minimized ? User32.SwRestore : User32.SwShow);
            User32.SetForegroundWindow(info.HsHandle);
            await Task.Delay(500);
            if (User32.IsHearthstoneInForeground())
                return new HearthstoneInfo();
            await DustUtilityPlugin.MainWindow.ShowMessageAsync("Exporting error", "Can't find Hearthstone window.");
            Log.Error("Can't find Hearthstone window.");
            return null;
        }
        #endregion
    }
}