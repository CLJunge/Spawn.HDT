using System.IO;
using System.Xml.Serialization;

namespace Spawn.HDT.DustUtility
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
        public static T Load<T>(string strPath)
        {
            T retVal = default(T);

            using (StreamReader reader = new StreamReader(strPath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                retVal = (T)(object)serializer.Deserialize(reader);
            }

            return retVal;
        }
        #endregion
    }
}