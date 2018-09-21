namespace Spawn.HDT.DustUtility.Messaging
{
    public class FlyoutInitializedMessage
    {
        #region Properties
        #region FlyoutName
        public string FlyoutName { get; private set; }
        #endregion
        #endregion

        #region Ctor
        public FlyoutInitializedMessage(string flyoutName)
        {
            FlyoutName = flyoutName;

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, $"Created new 'LVMouseDblClickMessage' instance");
        }
        #endregion
    }
}