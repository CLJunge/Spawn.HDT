using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Utility.Logging;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Spawn.HDT.DustUtility.CardManagement.AutoDisenchant
{
    public static class DeckExporter
    {
        public static async Task<bool> Export(Deck deck, Func<Task<bool>> onInterrupt)
        {
            if (deck == null)
                return false;
            var currentClipboard = "";
            try
            {
                Log.Info("Exporting " + deck.GetDeckInfo());
                if (DisenchantConfig.Instance.ExportPasteClipboard && Clipboard.ContainsText())
                    currentClipboard = Clipboard.GetText();
                var info = new ExportingInfo();
                LogDebugInfo(info);
                info = await ExportingHelper.EnsureHearthstoneInForeground(info);
                if (info == null)
                    return false;
                LogDebugInfo(info);
                Log.Info($"Waiting for {DisenchantConfig.Instance.ExportStartDelay} seconds before starting the export process");
                await Task.Delay(DisenchantConfig.Instance.ExportStartDelay * 1000);
                var exporter = new ExportingActions(info, deck, onInterrupt);
                await exporter.ClearDeck();
                await exporter.SetDeckName();
                await exporter.ClearFilters();
                await exporter.CreateDeck();
                await exporter.ClearSearchBox();
                if (DisenchantConfig.Instance.ExportPasteClipboard)
                    Clipboard.Clear();
                Log.Info("Success exporting deck.");
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Error exporting deck: " + e);
                return false;
            }
            finally
            {
                try
                {
                    if (DisenchantConfig.Instance.ExportPasteClipboard && currentClipboard != "")
                        Clipboard.SetText(currentClipboard);
                }
                catch (Exception ex)
                {
                    Log.Error("Could not restore clipboard content after export: " + ex);
                }
            }
        }

        private static void LogDebugInfo(ExportingInfo info) => Log.Debug($"HsHandle={info.HsHandle} HsRect={info.HsRect} Ratio={info.Ratio} SearchBoxPosX={DisenchantConfig.Instance.ExportSearchBoxX} SearchBoxPosY={DisenchantConfig.Instance.ExportSearchBoxY} CardPosX={DisenchantConfig.Instance.ExportCard1X} Card2PosX={DisenchantConfig.Instance.ExportCard2X} CardPosY={DisenchantConfig.Instance.ExportCardsY} ExportPasteClipboard={DisenchantConfig.Instance.ExportPasteClipboard} ExportNameDeckX={DisenchantConfig.Instance.ExportNameDeckX} ExportNameDeckY={DisenchantConfig.Instance.ExportNameDeckY} PrioritizeGolden={DisenchantConfig.Instance.PrioritizeGolden} DeckExportDelay={DisenchantConfig.Instance.DeckExportDelay} EnableExportAutoFilter={DisenchantConfig.Instance.EnableExportAutoFilter} ExportZeroButtonX={DisenchantConfig.Instance.ExportZeroButtonX} ExportZeroButtonY={DisenchantConfig.Instance.ExportZeroButtonY} ForceClear={DisenchantConfig.Instance.ExportForceClear}");
    }
}