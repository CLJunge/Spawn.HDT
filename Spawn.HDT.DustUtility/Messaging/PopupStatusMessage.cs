namespace Spawn.HDT.DustUtility.Messaging
{
    public class PopupStatusMessage
    {
        #region Properties
        #region CloseRequest
        public bool CloseRequest { get; private set; }
        #endregion
        #endregion

        #region Ctor
        public PopupStatusMessage(bool closeRequest)
        {
            CloseRequest = closeRequest;
        }
        #endregion
    }
}