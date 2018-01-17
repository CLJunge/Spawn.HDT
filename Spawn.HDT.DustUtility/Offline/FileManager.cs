using Hearthstone_Deck_Tracker.Utility.Logging;
using System.IO;
using System.Xml.Serialization;

namespace Spawn.HDT.DustUtility.Offline
{
    public static class FileManager
    {
        #region Write
        public static void Write<T>(string strPath, T value) where T : class, new()
        {
            try
            {
                if (File.Exists(strPath))
                {
                    File.Delete(strPath);
                }
                else { }

                using (StreamWriter writer = new StreamWriter(strPath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    serializer.Serialize(writer, value);
                }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine($"Exception occured while writing to file \"{strPath}\": {ex}", LogType.Error);
            }
        }
        #endregion

        #region Load
        public static T Load<T>(string strPath) where T : class, new()
        {
            T retVal = default(T);

            try
            {
                if (File.Exists(strPath))
                {
                    using (StreamReader reader = new StreamReader(strPath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(T));

                        retVal = (T)serializer.Deserialize(reader);
                    }
                }
                else { }
            }
            catch (System.Exception ex)
            {
                Log.WriteLine($"Exception occured while reading from file \"{strPath}\": {ex}", LogType.Error);
            }

            if (retVal == null)
            {
                retVal = new T();
            }
            else { }

            return retVal;
        }
        #endregion
    }
}