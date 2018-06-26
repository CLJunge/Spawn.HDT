namespace Spawn.HDT.DustUtility.UI.Flyouts
{
    public partial class SearchParametersFlyoutView
    {
        #region Ctor
        public SearchParametersFlyoutView()
        {
            InitializeComponent();

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Created new 'SearchParametersFlyoutView' instance");
        }
        #endregion
    }
}