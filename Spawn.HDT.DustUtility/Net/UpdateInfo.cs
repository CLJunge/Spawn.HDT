using System;

namespace Spawn.HDT.DustUtility.Net
{
    public class UpdateInfo
    {
        #region Member Variables
        private string m_strReleaseNotes;
        private Version m_version;
        #endregion

        #region Properties
        #region ReleaseNotes
        public string ReleaseNotes
        {
            get => m_strReleaseNotes;
            set => m_strReleaseNotes = value;
        }
        #endregion

        #region Version
        public Version Version => m_version;
        #endregion

        #region HasReleaseNotes
        public bool HasReleaseNotes => !string.IsNullOrEmpty(ReleaseNotes);
        #endregion
        #endregion

        #region Ctor
        public UpdateInfo(Version version, string releaseNotes = "")
        {
            m_version = version;
            m_strReleaseNotes = releaseNotes;
        }
        #endregion
    }
}