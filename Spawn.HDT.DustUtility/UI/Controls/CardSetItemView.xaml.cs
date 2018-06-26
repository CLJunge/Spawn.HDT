namespace Spawn.HDT.DustUtility.UI.Controls
{
    public partial class CardSetItemView
    {
        #region Ctor
        public CardSetItemView()
        {
            InitializeComponent();

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Initialized new 'CardSetItemView' instance");
        }
        #endregion
    }
}