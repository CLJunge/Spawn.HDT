﻿namespace Spawn.HDT.DustUtility.UI.Flyouts
{
    public partial class CollectionInfoFlyout
    {
        #region Ctor
        public CollectionInfoFlyout()
        {
            InitializeComponent();

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Created new 'CollectionInfoFlyout' instance");
        }
        #endregion
    }
}