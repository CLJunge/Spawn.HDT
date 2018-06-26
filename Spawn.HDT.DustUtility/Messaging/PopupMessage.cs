namespace Spawn.HDT.DustUtility.Messaging
{
    public class PopupMessage
    {
        #region Properties
        #region CloseRequest
        public bool CloseRequest { get; private set; }
        #endregion
        #endregion

        #region Ctor
        public PopupMessage(bool closeRequest)
        {
            CloseRequest = closeRequest;

            DustUtilityPlugin.Logger.Log(Logging.LogLevel.Debug, $"Initialized new 'PopupMessage' instance");
        }
        #endregion
    }
}