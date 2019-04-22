#region Using
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Hearthstone;
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
            string strRet = string.Empty;

            if (ArtistDict.TryGetValue(Config.Instance.SelectedLanguage, out string artistStr))
                strRet = $" {artistStr}:{strArtist.Split(' ').LastOrDefault()}";

            return strRet;
        }
        #endregion

        #region GetManaSearchString
        public static string GetManaSearchString(int nCost)
        {
            string strRet = string.Empty;

            if (ManaDict.TryGetValue(Config.Instance.SelectedLanguage, out string manaStr))
                strRet = $" {manaStr}:{nCost}";

            return strRet;
        }
        #endregion

        #region GetAttackSearchString
        public static string GetAttackSearchString(int nAttack)
        {
            string strRet = string.Empty;

            if (AttackDict.TryGetValue(Config.Instance.SelectedLanguage, out string atkStr))
                strRet = $" {atkStr}:{nAttack}";

            return strRet;
        }
        #endregion

        #region GetSearchString
        public static string GetSearchString(Card card)
        {
            string strRet = $"{card.LocalizedName}{GetArtistSearchString(card.Artist)} {GetManaSearchString(card.Cost)}".ToLowerInvariant();

            if (card.Id == HearthDb.CardIds.Collectible.Neutral.Feugen || card.Id == HearthDb.CardIds.Collectible.Neutral.Stalagg)
                strRet += GetAttackSearchString(card.Attack);

            return strRet;
        }
        #endregion

        #region EnsureHearthstoneInForeground
        public static async Task<HearthstoneInfo> EnsureHearthstoneInForeground(HearthstoneInfo info)
        {
            HearthstoneInfo retVal = null;

            if (User32.IsHearthstoneInForeground())
            {
                retVal = info;
            }
            else
            {
                User32.ShowWindow(info.HsHandle, User32.GetHearthstoneWindowState() == WindowState.Minimized ? User32.SwRestore : User32.SwShow);
                User32.SetForegroundWindow(info.HsHandle);

                await Task.Delay(500);

                if (User32.IsHearthstoneInForeground())
                {
                    retVal = new HearthstoneInfo();
                }
                else
                {
                    await DustUtilityPlugin.MainWindow.ShowMessageAsync("Auto Disenchanting", "Can't find Hearthstone window!");
                    DustUtilityPlugin.Logger.Log(Logging.LogLevel.Error, "Can't find Hearthstone window!");
                }
            }

            return retVal;
        }
        #endregion
    }
}