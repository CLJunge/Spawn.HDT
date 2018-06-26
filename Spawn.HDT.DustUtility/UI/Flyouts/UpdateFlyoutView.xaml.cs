namespace Spawn.HDT.DustUtility.UI.Flyouts
{
    public partial class UpdateFlyoutView
    {
        #region Ctor
        public UpdateFlyoutView()
        {
            InitializeComponent();

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Created new 'UpdateFlyoutView' instance");
        }
        #endregion
    }
}