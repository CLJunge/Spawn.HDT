#region Using
using Hearthstone_Deck_Tracker.Utility.Logging;
using Spawn.HDT.DustUtility.AccountManagement;
using Spawn.HDT.DustUtility.Hearthstone;
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
                    Log.WriteLine("Disenchanting card selection", LogType.Info);

                    HearthstoneInfo info = new HearthstoneInfo();

                    LogDebugInfo(info);

                    info = await Helper.EnsureHearthstoneInForeground(info);

                    if (info != null)
                    {
                        blnRet = true;

                        LogDebugInfo(info);

                        Log.WriteLine($"Waiting for {DisenchantConfig.Instance.StartDelay} seconds before starting to disenchant", LogType.Debug);

                        await Task.Delay(DisenchantConfig.Instance.StartDelay * 1000);

                        DisenchantActions actions = new DisenchantActions(account, info, lstCards, onInterrupt);

                        await actions.ClearFilters();
                        blnRet &= await actions.DisenchantCards();
                        //await actions.ClearSearchBox();

                        Log.WriteLine("Sucessfully disenchanted cards", LogType.Info);
                    }
                    else { }
                }
                catch (Exception e)
                {
                    Log.WriteLine($"Exception occured while disenchanting card selection: {e}", LogType.Error);

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