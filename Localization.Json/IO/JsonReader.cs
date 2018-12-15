using System.IO;
using Localization.Core;
using Newtonsoft.Json;

namespace Localization.Json.IO
{
    public static class JsonReader
    {
        private static readonly object _lock = typeof(object);

        public static T Read<T>(string path) where T : new()
        {
            lock (_lock)
            {
                T jsonObject = new T();
                if (!File.Exists(path))
                {
                    Write(jsonObject, path);
                }
                jsonObject = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
                return jsonObject;
            }
        }

        public static void Write<T>(T jsonObject, string path)
        {
            lock (_lock)
            {
                string output = JsonConvert.SerializeObject(jsonObject);

                if(!Directory.Exists(DefaultConfiguration.AppDomain()))
                {
                    Directory.CreateDirectory(DefaultConfiguration.AppDomain());
                }

                File.WriteAllText(path, output);
            }
        }
    }
}
