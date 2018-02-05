﻿using HearthMirror;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Utility.Logging;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Spawn.HDT.DustUtility.CardManagement.AutoDisenchant
{
    public class ExportingHelper
    {
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

        public static string GetArtistSearchString(string artist)
        {
            string artistStr;
            if (ArtistDict.TryGetValue(Config.Instance.SelectedLanguage, out artistStr))
                return $" {artistStr}:{artist.Split(' ').LastOrDefault()}";
            return "";
        }

        public static string GetManaSearchString(int cost)
        {
            string manaStr;
            if (ManaDict.TryGetValue(Config.Instance.SelectedLanguage, out manaStr))
                return $" {manaStr}:{cost}";
            return "";
        }

        public static string GetAttackSearchString(int atk)
        {
            string atkStr;
            if (AttackDict.TryGetValue(Config.Instance.SelectedLanguage, out atkStr))
                return $" {atkStr}:{atk}";
            return "";
        }

        public static string GetSearchString(Card card)
        {
            var searchString = $"{card.LocalizedName}{GetArtistSearchString(card.Artist)} {GetManaSearchString(card.Cost)}".ToLowerInvariant();
            if (card.Id == HearthDb.CardIds.Collectible.Neutral.Feugen || card.Id == HearthDb.CardIds.Collectible.Neutral.Stalagg)
                searchString += GetAttackSearchString(card.Attack);
            return searchString;
        }

        public static async Task<ExportingInfo> EnsureHearthstoneInForeground(ExportingInfo info)
        {
            if (User32.IsHearthstoneInForeground())
                return info;
            User32.ShowWindow(info.HsHandle, User32.GetHearthstoneWindowState() == WindowState.Minimized ? User32.SwRestore : User32.SwShow);
            User32.SetForegroundWindow(info.HsHandle);
            await Task.Delay(500);
            if (User32.IsHearthstoneInForeground())
                return new ExportingInfo();
            await DustUtilityPlugin.MainWindow.ShowMessageAsync("Exporting error", "Can't find Hearthstone window.");
            Log.Error("Can't find Hearthstone window.");
            return null;
        }

        public static IEnumerable<Card> GetMissingCards(Deck deck)
        {
            var collection = Reflection.GetCollection()
                .GroupBy(x => x.Id)
                .Select(x => new { Id = x.Key, Count = x.Sum(c => c.Count) })
                .ToList();
            foreach (var card in deck.GetSelectedDeckVersion().Cards)
            {
                var collectionCard = collection.FirstOrDefault(cCard => cCard.Id == card.Id);
                if (collectionCard == null)
                    yield return (Card)card.Clone();
                else if (collectionCard.Count < card.Count)
                {
                    var missing = (Card)card.Clone();
                    missing.Count = card.Count - collectionCard.Count;
                    yield return missing;
                }
            }
        }
    }
}