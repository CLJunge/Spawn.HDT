using System.IO;
using System.Xml.Serialization;

namespace Spawn.HDT.DustUtility.Offline
{
    public static class FileManager
    {
        #region Write
        public static void Write<T>(string strPath, T value)
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
        #endregion

        #region Load
        public static T Load<T>(string strPath) where T : class, new()
        {
            T retVal = default(T);

            if (File.Exists(strPath))
            {
                using (StreamReader reader = new StreamReader(strPath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    retVal = (T)serializer.Deserialize(reader);
                }
            }
            else { }

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