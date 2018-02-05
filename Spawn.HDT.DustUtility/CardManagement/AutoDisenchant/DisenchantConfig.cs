using System.ComponentModel;

namespace Spawn.HDT.DustUtility.CardManagement.AutoDisenchant
{
    public class DisenchantConfig
    {
        private static DisenchantConfig s_instance;

        public static DisenchantConfig Instance => (s_instance ?? (s_instance = new DisenchantConfig()));

        [DefaultValue(true)]
        public bool EnableExportAutoFilter = true;

        [DefaultValue(true)]
        public bool PrioritizeGolden = true;

        [DefaultValue(true)]
        public bool AutoClearDeck = true;

        [DefaultValue(50)]
        public int DeckExportDelay = 50;

        [DefaultValue(0.06)]
        public double ExportAllButtonX = 0.06;

        [DefaultValue(0.915)]
        public double ExportAllButtonY = 0.915;

        [DefaultValue(false)]
        public bool ExportAddDeckVersionToName = false;

        [DefaultValue(0.118)]
        public double ExportZeroButtonX = 0.118;

        [DefaultValue(0.917)]
        public double ExportZeroButtonY = 0.917;

        [DefaultValue(0.108)]
        public double ExportZeroSquareX = 0.108;

        [DefaultValue(0.907)]
        public double ExportZeroSquareY = 0.907;

        [DefaultValue(0.049)]
        public double ExportSetsButtonX = 0.049;

        [DefaultValue(0.917)]
        public double ExportSetsButtonY = 0.917;

        [DefaultValue(0.067)]
        public double ExportAllSetsButtonX = 0.067;

        [DefaultValue(0.45)]
        public double ExportStandardSetButtonY = 0.45;

        [DefaultValue(0.04)]
        public double ExportCard1X = 0.04;

        [DefaultValue(0.2)]
        public double ExportCard2X = 0.2;

        [DefaultValue(0.168)]
        public double ExportCardsY = 0.168;

        [DefaultValue(0.185)]
        public double ExportClearCheckYFixed = 0.185;

        [DefaultValue(0.83)]
        public double ExportClearX = 0.83;

        [DefaultValue(0.13)]
        public double ExportClearY = 0.13;

        [DefaultValue(false)]
        public bool ExportForceClear = false;

        [DefaultValue(0.85)]
        public double ExportNameDeckX = 0.85;

        [DefaultValue(0.075)]
        public double ExportNameDeckY = 0.075;

        [DefaultValue(false)]
        public bool ExportPasteClipboard = false;

        [DefaultValue(0.5)]
        public double ExportSearchBoxX = 0.5;

        [DefaultValue(0.915)]
        public double ExportSearchBoxY = 0.915;

        [DefaultValue(true)]
        public bool ExportSetDeckName = true;

        [DefaultValue(1)]
        public int ExportStartDelay = 1;

        [DefaultValue(true)]
        public bool ShowExportingDialog = true;
    }
}