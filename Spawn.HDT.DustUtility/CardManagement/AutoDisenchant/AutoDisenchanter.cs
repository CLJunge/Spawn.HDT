#region Using
using Hearthstone_Deck_Tracker.Utility.Logging;
using Spawn.HDT.DustUtility.AccountManagement;
using Spawn.HDT.DustUtility.Hearthstone;
using Spawn.HDT.DustUtility.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

namespace Spawn.HDT.DustUtility.CardManagement.AutoDisenchant
{
    public static class AutoDisenchanter
    {
        #region Disenchant
        public static async Task<bool> Disenchant(IAccount account, List<CardWrapper> lstCards, Func<Task<bool>> onInterrupt)
        {
            bool blnRet = false;

            if (lstCards?.Count > 0)
            {
                try
                {
                    DustUtilityPlugin.Logger.Log(LogLevel.Trace, $"Disenchanting current card selection...");

                    HearthstoneInfo info = new HearthstoneInfo();

                    LogDebugInfo(info);

                    info = await Helper.EnsureHearthstoneInForeground(info);

                    if (info != null)
                    {
                        blnRet = true;

                        LogDebugInfo(info);

                        DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Waiting for {DisenchantConfig.Instance.StartDelay} seconds before starting to disenchant");

                        await Task.Delay(DisenchantConfig.Instance.StartDelay * 1000);

                        DisenchantActions actions = new DisenchantActions(account, info, lstCards, onInterrupt);

                        await actions.ClearFilters();
                        blnRet &= await actions.DisenchantCards();
                        //await actions.ClearSearchBox();

                        DustUtilityPlugin.Logger.Log(LogLevel.Trace, $"Sucessfully disenchanted {lstCards?.Count} card(s)");
                    }
                    else { }
                }
                catch (Exception ex)
                {
                    DustUtilityPlugin.Logger.Log(LogLevel.Error, $"Exception occured while disenchanting card selection: {ex}");

                    blnRet = false;
                }
            }
            else { }

            return blnRet;
        }
        #endregion

        #region LogDebugInfo
        private static void LogDebugInfo(HearthstoneInfo info) => Log.Debug($"HsHandle={info.HsHandle} HsRect={info.HsRect} Ratio={info.Ratio} SearchBoxPosX={DisenchantConfig.Instance.SearchBoxX} SearchBoxPosY={DisenchantConfig.Instance.SearchBoxY} CardPosX={DisenchantConfig.Instance.Card1X} Card2PosX={DisenchantConfig.Instance.Card2X} CardPosY={DisenchantConfig.Instance.CardsY} DeckExportDelay={DisenchantConfig.Instance.Delay} EnableExportAutoFilter={DisenchantConfig.Instance.AutoFilter} ExportZeroButtonX={DisenchantConfig.Instance.ZeroButtonX} ExportZeroButtonY={DisenchantConfig.Instance.ZeroButtonY} ForceClear={DisenchantConfig.Instance.ForceClear}");
        #endregion
    }
}