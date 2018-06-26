namespace Spawn.HDT.DustUtility.UI.Flyouts
{
    public partial class SearchParametersFlyoutView
    {
        #region Ctor
        public SearchParametersFlyoutView()
        {
            InitializeComponent();

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Initialized new 'SearchParametersFlyoutView' instance");
        }
        #endregion
    }
}