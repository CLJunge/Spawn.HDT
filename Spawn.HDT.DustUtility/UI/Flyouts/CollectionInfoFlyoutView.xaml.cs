namespace Spawn.HDT.DustUtility.UI.Flyouts
{
    public partial class CollectionInfoFlyoutView
    {
        #region Ctor
        public CollectionInfoFlyoutView()
        {
            InitializeComponent();

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Created new 'CollectionInfoFlyoutView' instance");
        }
        #endregion
    }
}