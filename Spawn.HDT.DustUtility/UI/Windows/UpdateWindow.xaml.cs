namespace Spawn.HDT.DustUtility.UI.Windows
{
    public partial class UpdateWindow
    {
        #region Ctor
        public UpdateWindow()
        {
            InitializeComponent();

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, "Created new 'UpdateWindow' instance");
        }
        #endregion
    }
}