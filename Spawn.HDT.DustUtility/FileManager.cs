#region Using
using Spawn.HDT.DustUtility.Logging;
using System.IO;
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

                DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Wrote '{value.GetType()}' to '{strPath}'");
            }
            catch (System.Exception ex)
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Error, $"Exception occured while writing to file \"{strPath}\": {ex}");
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

                    DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Loaded '{retVal.GetType()}' from '{strPath}'");
                }
                else
                {
                    DustUtilityPlugin.Logger.Log(LogLevel.Warning, $"Coudln't load '{typeof(T)}'! File '{strPath}' doesn't exist.");
                }
            }
            catch (System.Exception ex)
            {
                DustUtilityPlugin.Logger.Log(LogLevel.Error, $"Exception occured while reading from file \"{strPath}\": {ex}");
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