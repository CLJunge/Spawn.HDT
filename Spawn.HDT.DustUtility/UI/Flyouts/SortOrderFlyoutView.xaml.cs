﻿namespace Spawn.HDT.DustUtility.UI.Flyouts
{
    public partial class SortOrderFlyoutView
    {
        #region Ctor
        public SortOrderFlyoutView()
        {
            InitializeComponent();

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Created new 'SortOrderFlyoutView' instance");
        }
        #endregion
    }
}