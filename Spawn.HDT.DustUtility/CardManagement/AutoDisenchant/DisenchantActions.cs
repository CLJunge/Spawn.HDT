using HearthMirror;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Spawn.HDT.DustUtility.Hearthstone;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spawn.HDT.DustUtility.CardManagement.AutoDisenchant
{
    public class DisenchantActions
    {
        #region Member Variables
        private HearthstoneInfo m_info;
        private List<CardWrapper> m_lstCards;
        private MouseActions m_mouseActions;

        private Point m_clearCardPoint;
        private Point m_card1Point;
        private Point m_card2Point;
        private Point m_zeroManaCrystalPoint;
        private Point m_setFilterMenuPoint;
        private Point m_setFilterAllPoint;
        private Point m_disenchantButtonPoint;

        private const int CardPosOffset = 50;
        #endregion

        #region Ctor
        public DisenchantActions(HearthstoneInfo info, List<CardWrapper> cards, Func<Task<bool>> onInterrupt)
        {
            m_info = info;
            m_lstCards = cards;

            m_mouseActions = new MouseActions(m_info, onInterrupt);

            m_clearCardPoint = new Point(GetScaledXPos(DisenchantConfig.Instance.ClearX), GetYPos(DisenchantConfig.Instance.ClearY));
            m_card1Point = new Point((int)m_info.CardPosX + CardPosOffset, (int)m_info.CardPosY + CardPosOffset);
            m_card2Point = new Point((int)m_info.Card2PosX + CardPosOffset, (int)m_info.CardPosY + CardPosOffset);
            m_zeroManaCrystalPoint = new Point(GetScaledXPos(DisenchantConfig.Instance.ZeroButtonX), GetYPos(DisenchantConfig.Instance.ZeroButtonY));
            m_setFilterMenuPoint = new Point(GetScaledXPos(DisenchantConfig.Instance.SetsButtonX), GetYPos(DisenchantConfig.Instance.SetsButtonY));
            m_setFilterAllPoint = new Point(GetScaledXPos(DisenchantConfig.Instance.AllSetsButtonX), GetYPos(DisenchantConfig.Instance.StandardSetButtonY));
            m_disenchantButtonPoint = new Point(GetScaledXPos(DisenchantConfig.Instance.DisenchantButtonX), GetYPos(DisenchantConfig.Instance.DisenchantButtonY));
        }
        #endregion

        #region DisenchantCards
        public async Task<bool> DisenchantCards()
        {
            Log.WriteLine("Disenchanting...", LogType.Debug);

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

            if (DisenchantConfig.Instance.ForceClear)
            {
                await ClearSearchBox();
            }
            else { }

            await m_mouseActions.ClickOnPoint(m_info.SearchBoxPos);

            await Task.Delay(DisenchantConfig.Instance.Delay);

            SendKeys.SendWait(Helper.GetSearchString(wrapper.Card));
            SendKeys.SendWait("{ENTER}");

            await Task.Delay(DisenchantConfig.Instance.Delay * 2);

            await ClickOnCard(CardPosition.Left, true);

            //TODO actually disenchant the card (dangerous)

            await Task.Delay(DisenchantConfig.Instance.Delay * 10);

            await m_mouseActions.ClickOnPoint(m_disenchantButtonPoint);

            return nRet;
        }
        #endregion

        #region ClickOnCard
        private async Task ClickOnCard(CardPosition pos, bool blnUseRightMouseButton = false)
        {
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
            }
            else { }
        }
        #endregion

        #region ClearManaFilter
        private async Task ClearManaFilter()
        {
            if (Reflection.GetCurrentManaFilter() != -1)
            {
                Log.WriteLine("Clearing mana filter", LogType.Debug);

                await m_mouseActions.ClickOnPoint(m_zeroManaCrystalPoint);

                await Task.Delay(500);

                if (Reflection.GetCurrentManaFilter() == 0)
                {
                    await m_mouseActions.ClickOnPoint(m_zeroManaCrystalPoint);
                }
                else { }
            }
            else
            {
                Log.WriteLine("No mana filter set", LogType.Debug);
            }
        }
        #endregion

        #region ClearSetsFilter
        private async Task ClearSetsFilter()
        {
            HearthMirror.Objects.SetFilterItem setFilter = Reflection.GetCurrentSetFilter();

            if (!setFilter.IsAllStandard && !setFilter.IsWild)
            {
                Log.Info("Clearing set filter...");

                await m_mouseActions.ClickOnPoint(m_setFilterMenuPoint);

                await Task.Delay(500);

                await m_mouseActions.ClickOnPoint(m_setFilterAllPoint);

                await Task.Delay(500);

                await m_mouseActions.ClickOnPoint(m_setFilterMenuPoint);
            }
            else { }
        }
        #endregion

        #region ClearSearchBox
        public async Task ClearSearchBox()
        {
            await m_mouseActions.ClickOnPoint(m_info.SearchBoxPos);

            SendKeys.SendWait("{DELETE}");
            SendKeys.SendWait("{ENTER}");
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