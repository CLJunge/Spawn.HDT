namespace Spawn.HDT.DustUtility.UI.Flyouts
{
    public partial class DeckListFlyoutView
    {
        #region Ctor
        public DeckListFlyoutView()
        {
            InitializeComponent();

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Initialized new 'DeckListFlyoutView' instance");
        }
        #endregion
    }
}