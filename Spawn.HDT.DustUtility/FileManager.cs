#region Using
using System.IO;
using System.Windows;
using System.Xml.Serialization;
#endregion

namespace Spawn.HDT.DustUtility
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

                using (StreamWriter writer = new StreamWriter(strPath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    serializer.Serialize(writer, value);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Exception occured while writing to file '{strPath}': {ex}", "Dust Utility - Exception");
            }
        }
        #endregion

        #region Load
        public static T Load<T>(string strPath) where T : class, new()
        {
            T retVal = null;

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
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Exception occured while reading from file '{strPath}': {ex}", "Dust Utility - Exception");
            }

            if (retVal == null)
            {
                retVal = new T();
            }

            return retVal;
        }
        #endregion
    }
}