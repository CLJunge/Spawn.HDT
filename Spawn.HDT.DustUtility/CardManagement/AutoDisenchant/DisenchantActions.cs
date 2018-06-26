#region Using
using HearthMirror;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Spawn.HDT.DustUtility.AccountManagement;
using Spawn.HDT.DustUtility.Hearthstone;
using Spawn.HDT.DustUtility.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
#endregion

namespace Spawn.HDT.DustUtility.CardManagement.AutoDisenchant
{
    public class DisenchantActions
    {
        #region Member Variables
        private readonly IAccount m_account;

        private readonly HearthstoneInfo m_info;
        private readonly List<CardWrapper> m_lstCards;
        private readonly MouseActions m_mouseActions;

        private readonly Point m_card1Point;
        private readonly Point m_card2Point;
        private readonly Point m_zeroManaCrystalPoint;
        private readonly Point m_setFilterMenuPoint;
        private readonly Point m_setFilterAllPoint;
        private readonly Point m_disenchantButtonPoint;
        private readonly Point m_dialogAcceptButtonPoint;

        private const int CardPosOffset = 50;
        #endregion

        #region Ctor
        public DisenchantActions(IAccount account, HearthstoneInfo info, List<CardWrapper> cards, Func<Task<bool>> onInterrupt)
        {
            m_account = account;
            m_info = info;
            m_lstCards = cards;
            m_mouseActions = new MouseActions(m_info, onInterrupt);

            m_card1Point = new Point((int)m_info.CardPosX + CardPosOffset, (int)m_info.CardPosY + CardPosOffset);
            m_card2Point = new Point((int)m_info.Card2PosX + CardPosOffset, (int)m_info.CardPosY + CardPosOffset);
            m_zeroManaCrystalPoint = new Point(GetScaledXPos(DisenchantConfig.Instance.ZeroButtonX), GetYPos(DisenchantConfig.Instance.ZeroButtonY));
            m_setFilterMenuPoint = new Point(GetScaledXPos(DisenchantConfig.Instance.SetsButtonX), GetYPos(DisenchantConfig.Instance.SetsButtonY));
            m_setFilterAllPoint = new Point(GetScaledXPos(DisenchantConfig.Instance.AllSetsButtonX), GetYPos(DisenchantConfig.Instance.StandardSetButtonY));
            m_disenchantButtonPoint = new Point(GetScaledXPos(DisenchantConfig.Instance.DisenchantButtonX), GetYPos(DisenchantConfig.Instance.DisenchantButtonY));
            m_dialogAcceptButtonPoint = new Point(GetScaledXPos(DisenchantConfig.Instance.DialogAcceptButtonX), GetYPos(DisenchantConfig.Instance.DialogAcceptButtonY));

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Created new 'DisenchantActions' instance");
        }
        #endregion

        #region DisenchantCards
        public async Task<bool> DisenchantCards()
        {
            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Disenchanting cards...");

            int nTotalCount = m_lstCards.Sum(c => c.Count);

            int nCount = 0;

            for (int i = 0; i < m_lstCards.Count; i++)
            {
                nCount += await DisenchantCard(m_lstCards[i]);
            }

            return nCount == nTotalCount;
        }
        #endregion

        #region DisenchantCard
        private async Task<int> DisenchantCard(CardWrapper wrapper)
        {
            int nRet = 0;

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Disenchanting {wrapper.Count}x {wrapper.Card.Name}...");

            try
            {
                bool blnIsInDecks = IsCardInDecks(wrapper);

                int nTotalCardCount = m_account.GetCollection()
                    .Where(c => c.Id.Equals(wrapper.RawCard.Id) && c.Premium == wrapper.RawCard.Premium)
                    .Sum(c => c.Count);

                if (DisenchantConfig.Instance.ForceClear)
                {
                    await ClearSearchBox();
                }

                await m_mouseActions.ClickOnPoint(m_info.SearchBoxPos);

                await Task.Delay(DisenchantConfig.Instance.Delay);

                SendKeys.SendWait(Helper.GetSearchString(wrapper.Card));
                SendKeys.SendWait("{ENTER}");

                await Task.Delay(DisenchantConfig.Instance.Delay * 2);

                await ClickOnCard((wrapper.RawCard.Premium ? CardPosition.Right : CardPosition.Left), true);

                int nDialogCardCountLimit = (wrapper.DbCard.Rarity == HearthDb.Enums.Rarity.LEGENDARY ? 1 : 2);

                while ((wrapper.RawCard.Count - nRet) > nDialogCardCountLimit)
                {
                    await Task.Delay(DisenchantConfig.Instance.Delay * 20);

                    await m_mouseActions.ClickOnPoint(m_disenchantButtonPoint);

                    nRet += 1;
                }

                if (blnIsInDecks || nTotalCardCount <= nDialogCardCountLimit)
                {
                    for (; nRet < wrapper.RawCard.Count; nRet++)
                    {
                        await Task.Delay(DisenchantConfig.Instance.Delay * 20);

                        await m_mouseActions.ClickOnPoint(m_disenchantButtonPoint);

                        await Task.Delay(DisenchantConfig.Instance.Delay * 20);

                        await m_mouseActions.ClickOnPoint(m_dialogAcceptButtonPoint);
                    }
                }

                await Task.Delay(DisenchantConfig.Instance.Delay * 20);

                await m_mouseActions.ClickOnPoint(m_dialogAcceptButtonPoint);

                await Task.Delay(DisenchantConfig.Instance.Delay * 5);

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Should have disenchanted {wrapper.Count} copies of {wrapper.Card.Name}...");
            }
            catch (Exception ex)
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Error, $"Exception occured while disenchanting '{wrapper?.Card?.Name}': {ex}");
            }

            return nRet;
        }
        #endregion

        #region IsCardInDecks
        private bool IsCardInDecks(CardWrapper wrapper)
        {
            bool blnRet = false;

            List<HearthMirror.Objects.Deck> lstDecks = m_account.GetDecks();

            for (int i = 0; i < lstDecks.Count; i++)
            {
                if (lstDecks[i].ContainsCard(wrapper.RawCard.Id, wrapper.RawCard.Premium))
                {
                    blnRet = true;

                    int nCountInDecks = lstDecks[i].GetCard(wrapper.RawCard.Id, wrapper.RawCard.Premium).Count;

                    if (nCountInDecks > wrapper.MaxCountInDecks)
                    {
                        wrapper.MaxCountInDecks = nCountInDecks;
                    }
                }
            }

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"IsCardInDecks: Name={wrapper.Card.Name} > Result={blnRet}");

            return blnRet;
        }
        #endregion

        #region ClickOnCard
        private async Task ClickOnCard(CardPosition pos, bool blnUseRightMouseButton = false)
        {
            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"ClickOnCard: Pos={pos} RightMouseButton={blnUseRightMouseButton}");

            if (pos == CardPosition.Left)
            {
                await m_mouseActions.ClickOnPoint(m_card1Point, blnUseRightMouseButton);
            }
            else
            {
                await m_mouseActions.ClickOnPoint(m_card2Point, blnUseRightMouseButton);
            }
        }
        #endregion

        #region ClearFilters
        public async Task ClearFilters()
        {
            if (DisenchantConfig.Instance.AutoFilter)
            {
                await ClearManaFilter();
                await ClearSetsFilter();

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Cleared filters");
            }
        }
        #endregion

        #region ClearManaFilter
        private async Task ClearManaFilter()
        {
            if (Reflection.GetCurrentManaFilter() != -1)
            {
                await m_mouseActions.ClickOnPoint(m_zeroManaCrystalPoint);

                await Task.Delay(500);

                if (Reflection.GetCurrentManaFilter() == 0)
                {
                    await m_mouseActions.ClickOnPoint(m_zeroManaCrystalPoint);
                }

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Cleared mana filter");
            }
            else
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Debug, "No mana filter set");
            }
        }
        #endregion

        #region ClearSetsFilter
        private async Task ClearSetsFilter()
        {
            HearthMirror.Objects.SetFilterItem setFilter = Reflection.GetCurrentSetFilter();

            if (!setFilter.IsAllStandard && !setFilter.IsWild)
            {
                await m_mouseActions.ClickOnPoint(m_setFilterMenuPoint);

                await Task.Delay(500);

                await m_mouseActions.ClickOnPoint(m_setFilterAllPoint);

                await Task.Delay(500);

                await m_mouseActions.ClickOnPoint(m_setFilterMenuPoint);

                Log.Info("Cleared sets filter");
            }
        }
        #endregion

        #region ClearSearchBox
        public async Task ClearSearchBox()
        {
            await Task.Delay(DisenchantConfig.Instance.Delay * 2);

            await m_mouseActions.ClickOnPoint(m_info.SearchBoxPos);

            await Task.Delay(DisenchantConfig.Instance.Delay * 2);

            SendKeys.SendWait("{DELETE}");
            SendKeys.SendWait("{ENTER}");

            await Task.Delay(DisenchantConfig.Instance.Delay * 2);

            Log.Info("Cleared search box");
        }
        #endregion

        #region GetScaledXPos
        private int GetScaledXPos(double x) => (int)Hearthstone_Deck_Tracker.Helper.GetScaledXPos(x, m_info.HsRect.Width, m_info.Ratio);
        #endregion

        #region GetYPos
        private int GetYPos(double y) => (int)(y * m_info.HsRect.Height);
        #endregion

        public enum CardPosition
        {
            Left,
            Right
        }
    }
}