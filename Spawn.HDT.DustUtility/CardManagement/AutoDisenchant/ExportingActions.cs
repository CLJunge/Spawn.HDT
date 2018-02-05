using HearthMirror;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spawn.HDT.DustUtility.CardManagement.AutoDisenchant
{
    public class ExportingActions
    {
        private int _cardCount;
        private readonly ExportingInfo _info;
        private readonly Deck _deck;
        private readonly MouseActions _mouse;
        private readonly Point _clearCardPoint;
        private readonly Point _deckNamePoint;
        private readonly Point _card1Point;
        private readonly Point _card2Point;
        private readonly Point _zeroManaCrystalPoint;
        private readonly Point _setFilterMenuPoint;
        private readonly Point _setFilterAllPoint;
        private const int MaxLengthDeckName = 24;
        private const int CardPosOffset = 50;

        public ExportingActions(ExportingInfo info, Deck deck, Func<Task<bool>> onInterrupt)
        {
            _info = info;
            _deck = deck;
            _mouse = new MouseActions(info, onInterrupt);
            _clearCardPoint = new Point(GetScaledXPos(DisenchantConfig.Instance.ExportClearX), GetYPos(DisenchantConfig.Instance.ExportClearY));
            _deckNamePoint = new Point(GetScaledXPos(DisenchantConfig.Instance.ExportNameDeckX), GetYPos(DisenchantConfig.Instance.ExportNameDeckY));
            _card1Point = new Point((int)_info.CardPosX + CardPosOffset, (int)_info.CardPosY + CardPosOffset);
            _card2Point = new Point((int)_info.Card2PosX + CardPosOffset, (int)_info.CardPosY + CardPosOffset);
            _zeroManaCrystalPoint = new Point(GetScaledXPos(DisenchantConfig.Instance.ExportZeroButtonX), GetYPos(DisenchantConfig.Instance.ExportZeroButtonY));
            _setFilterMenuPoint = new Point(GetScaledXPos(DisenchantConfig.Instance.ExportSetsButtonX), GetYPos(DisenchantConfig.Instance.ExportSetsButtonY));
            _setFilterAllPoint = new Point(GetScaledXPos(DisenchantConfig.Instance.ExportAllSetsButtonX), GetYPos(DisenchantConfig.Instance.ExportStandardSetButtonY));
        }

        public async Task SetDeckName()
        {
            if (DisenchantConfig.Instance.ExportSetDeckName && !_deck.TagList.ToLower().Contains("brawl"))
            {
                var name = Regex.Replace(_deck.Name, @"[\(\)\{\}]", "");
                if (name != _deck.Name)
                    Log.Info("Removed parenthesis/braces from deck name. New name: " + name);
                if (Reflection.GetEditedDeck()?.Name == _deck.Name)
                {
                    Log.Info("Deck already has the correct name");
                    return;
                }
                if (DisenchantConfig.Instance.ExportAddDeckVersionToName)
                {
                    var version = " " + _deck.SelectedVersion.ShortVersionString;
                    if (name.Length + version.Length > MaxLengthDeckName)
                        name = name.Substring(0, MaxLengthDeckName - version.Length);
                    name += version;
                }

                Log.Info("Setting deck name...");
                await _mouse.ClickOnPoint(_deckNamePoint);
                //send enter and second click to make sure the current name gets selected
                SendKeys.SendWait("{ENTER}");
                await _mouse.ClickOnPoint(_deckNamePoint);
                if (DisenchantConfig.Instance.ExportPasteClipboard)
                {
                    Clipboard.SetText(name);
                    SendKeys.SendWait("^{v}");
                }
                else
                    SendKeys.SendWait(name);
                SendKeys.SendWait("{ENTER}");
            }
        }

        public async Task ClearDeck()
        {
            if (!DisenchantConfig.Instance.AutoClearDeck)
                return;
            var count = Reflection.GetEditedDeck()?.Cards.Sum(x => x.Count) ?? 30;
            Log.Info($"Clearing {count} cards from the deck...");
            for (var i = 0; i < count; i++)
                await _mouse.ClickOnPoint(_clearCardPoint);
            while (Reflection.GetEditedDeck()?.Cards.Sum(x => x.Count) > 0)
            {
                await _mouse.ClickOnPoint(_clearCardPoint);
                await Task.Delay(100);
            }
        }

        private int GetScaledXPos(double x) => (int)Helper.GetScaledXPos(x, _info.HsRect.Width, _info.Ratio);
        private int GetYPos(double y) => (int)(y * _info.HsRect.Height);

        public async Task<int> AddCardToDeck(Card card, List<HearthMirror.Objects.Card> collection)
        {
            var inCollection = collection.Where(x => x.Id == card.Id).ToList();
            if (!inCollection.Any())
                return card.Count;
            var golden = inCollection.FirstOrDefault(x => x.Premium)?.Count ?? 0;
            var normal = inCollection.FirstOrDefault(x => !x.Premium)?.Count ?? 0;

            if (DisenchantConfig.Instance.ExportForceClear)
                await ClearSearchBox();

            await _mouse.ClickOnPoint(_info.SearchBoxPos);
            await Task.Delay(DisenchantConfig.Instance.DeckExportDelay);

            if (DisenchantConfig.Instance.ExportPasteClipboard || !Helper.LatinLanguages.Contains(Config.Instance.SelectedLanguage))
            {
                Clipboard.SetText(ExportingHelper.GetSearchString(card));
                SendKeys.SendWait("^{v}");
            }
            else
                SendKeys.SendWait(ExportingHelper.GetSearchString(card));
            SendKeys.SendWait("{ENTER}");

            Log.Info($"Adding {card}, in collection: {normal} normal, {golden} golden");
            await Task.Delay(DisenchantConfig.Instance.DeckExportDelay * 2);

            if (DisenchantConfig.Instance.PrioritizeGolden && golden > 0)
            {
                if (normal > 0)
                    await ClickOnCard(CardPosition.Right);
                else
                    await ClickOnCard(CardPosition.Left);
                if (card.Count == 2)
                {
                    if (golden > 1 && normal > 1)
                        await ClickOnCard(CardPosition.Right);
                    else
                        await ClickOnCard(CardPosition.Left);
                }
            }
            else
            {
                await ClickOnCard(CardPosition.Left);
                if (card.Count == 2)
                {
                    if (normal + golden < 2)
                        return 1;
                    if (normal == 1)
                        await ClickOnCard(CardPosition.Right);
                    else
                        await ClickOnCard(CardPosition.Left);
                }
            }
            return 0;
        }

        public async Task ClickOnCard(CardPosition pos)
        {
            int? count = null;
            for (var i = 0; i < 2; i++)
            {
                if (pos == CardPosition.Left)
                    await _mouse.ClickOnPoint(_card1Point);
                else
                    await _mouse.ClickOnPoint(_card2Point);
                if ((count = Reflection.GetEditedDeck()?.Cards.Sum(x => x.Count)) > _cardCount)
                    break;
            }
            if (count != null)
                _cardCount = count.Value;
        }

        public async Task CreateDeck()
        {
            Log.Info("Creating deck...");
            _cardCount = 0;
            var collection = Reflection.GetCollection();
            foreach (var card in _deck.GetSelectedDeckVersion().Cards)
                await AddCardToDeck(card, collection);
        }

        public async Task ClearFilters()
        {
            if (!DisenchantConfig.Instance.EnableExportAutoFilter)
                return;
            await ClearManaFilter();
            await ClearSetsFilter();
        }

        public async Task ClearManaFilter()
        {
            if (Reflection.GetCurrentManaFilter() == -1)
            {
                Log.Info("No mana filter set");
                return;
            }
            Log.Info("Clearing mana filter");
            await _mouse.ClickOnPoint(_zeroManaCrystalPoint);
            await Task.Delay(500);
            if (Reflection.GetCurrentManaFilter() == 0)
                await _mouse.ClickOnPoint(_zeroManaCrystalPoint);
        }

        public async Task ClearSetsFilter()
        {
            var setFilter = Reflection.GetCurrentSetFilter();
            if (setFilter.IsAllStandard || setFilter.IsWild)
                return;
            Log.Info("Clearing set filter...");
            await _mouse.ClickOnPoint(_setFilterMenuPoint);
            await Task.Delay(500);
            await _mouse.ClickOnPoint(_setFilterAllPoint);
            await Task.Delay(500);
            await _mouse.ClickOnPoint(_setFilterMenuPoint);
        }

        public async Task ClearSearchBox()
        {
            await _mouse.ClickOnPoint(_info.SearchBoxPos);
            SendKeys.SendWait("{DELETE}");
            SendKeys.SendWait("{ENTER}");
        }

        public enum CardPosition
        {
            Left,
            Right
        }
    }
}