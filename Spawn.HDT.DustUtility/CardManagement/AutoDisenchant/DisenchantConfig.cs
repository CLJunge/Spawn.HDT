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
        public bool AutoFilter = true;

        [DefaultValue(50)]
        public int Delay = 50;

        [DefaultValue(0.118)]
        public double ZeroButtonX = 0.118;

        [DefaultValue(0.917)]
        public double ZeroButtonY = 0.917;

        [DefaultValue(0.049)]
        public double SetsButtonX = 0.049;

        [DefaultValue(0.917)]
        public double SetsButtonY = 0.917;

        [DefaultValue(0.067)]
        public double AllSetsButtonX = 0.067;

        [DefaultValue(0.45)]
        public double StandardSetButtonY = 0.45;

        [DefaultValue(0.04)]
        public double Card1X = 0.04;

        [DefaultValue(0.2)]
        public double Card2X = 0.2;

        [DefaultValue(0.168)]
        public double CardsY = 0.168;

        [DefaultValue(0.83)]
        public double ClearX = 0.83;

        [DefaultValue(0.13)]
        public double ClearY = 0.13;

        [DefaultValue(false)]
        public bool ForceClear = false;

        [DefaultValue(0.5)]
        public double SearchBoxX = 0.5;

        [DefaultValue(0.915)]
        public double SearchBoxY = 0.915;

        [DefaultValue(0.387)]
        public double DisenchantButtonX = 0.387;

        [DefaultValue(0.827)]
        public double DisenchantButtonY = 0.827;

        [DefaultValue(0.427)]
        public double DialogAcceptButtonX = 0.427;

        [DefaultValue(0.595)]
        public double DialogAcceptButtonY = 0.595;

        [DefaultValue(1)]
        public int StartDelay = 1;
        #endregion
    }
}