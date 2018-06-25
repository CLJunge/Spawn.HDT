#region Using
using System.ComponentModel;
#endregion

namespace Spawn.HDT.DustUtility.CardManagement.AutoDisenchant
{
    public class DisenchantConfig
    {
        #region [STATIC] Instance
        private static DisenchantConfig s_instance;

        public static DisenchantConfig Instance => (s_instance ?? (s_instance = new DisenchantConfig()));
        #endregion

        #region Properties
        [DefaultValue(true)]
        public bool AutoFilter { get; set; } = true;

        [DefaultValue(50)]
        public int Delay { get; set; } = 50;

        [DefaultValue(0.118)]
        public double ZeroButtonX { get; set; } = 0.118;

        [DefaultValue(0.917)]
        public double ZeroButtonY { get; set; } = 0.917;

        [DefaultValue(0.049)]
        public double SetsButtonX { get; set; } = 0.049;

        [DefaultValue(0.917)]
        public double SetsButtonY { get; set; } = 0.917;

        [DefaultValue(0.067)]
        public double AllSetsButtonX { get; set; } = 0.067;

        [DefaultValue(0.45)]
        public double StandardSetButtonY { get; set; } = 0.45;

        [DefaultValue(0.04)]
        public double Card1X { get; set; } = 0.04;

        [DefaultValue(0.2)]
        public double Card2X { get; set; } = 0.2;

        [DefaultValue(0.168)]
        public double CardsY { get; set; } = 0.168;

        [DefaultValue(false)]
        public bool ForceClear { get; set; } = false;

        [DefaultValue(0.5)]
        public double SearchBoxX { get; set; } = 0.5;

        [DefaultValue(0.915)]
        public double SearchBoxY { get; set; } = 0.915;

        [DefaultValue(0.387)]
        public double DisenchantButtonX { get; set; } = 0.387;

        [DefaultValue(0.827)]
        public double DisenchantButtonY { get; set; } = 0.827;

        [DefaultValue(0.427)]
        public double DialogAcceptButtonX { get; set; } = 0.427;

        [DefaultValue(0.595)]
        public double DialogAcceptButtonY { get; set; } = 0.595;

        [DefaultValue(1)]
        public int StartDelay { get; set; } = 1;
        #endregion
    }
}