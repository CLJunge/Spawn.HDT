﻿using System;

namespace Spawn.HDT.DustUtility.Offline
{
    public class CachedCard
    {
        #region Properties
        #region Id
        public string Id { get; set; }
        #endregion

        #region Count
        public int Count { get; set; }
        #endregion

        #region IsGolden
        public bool IsGolden { get; set; }
        #endregion
        #endregion
    }

    public class CachedCardEx : CachedCard
    {
        #region Properties
        #region Timestamp
        public DateTime Timestamp { get; set; }
        #endregion
        #endregion
    }
}