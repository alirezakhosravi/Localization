using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Raveshmand.Localization.Core;

namespace Raveshmand.Localization.Xml.IO
{
    public static class XmlReader
    {
        private static readonly object _lock = typeof(object);

        public static T Read<T>(string path) where T : new()
        {
            lock (_lock)
            {
                T xmlObject = new T();
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                if (!File.Exists(path))
                {
                    Write(xmlObject, path);
                }
                StringReader stringReader = new StringReader(File.ReadAllText(path));
                xmlObject = (T)xmlSerializer.Deserialize(stringReader);
                return xmlObject;
            }
        }

        public static void Write<T>(T xmlObject, string path)
        {
            lock (_lock)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                MemoryStream memoryStream = new MemoryStream();
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8)
                {
                    Formatting = Formatting.Indented
                };

                xmlSerializer.Serialize(xmlTextWriter, xmlObject);

                string output = Encoding.UTF8.GetString(memoryStream.ToArray());
                string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                if (output.StartsWith(_byteOrderMarkUtf8, StringComparison.Ordinal))
                {
                    output = output.Remove(0, _byteOrderMarkUtf8.Length);
                }

                if(!Directory.Exists(DefaultConfiguration.AppDomain()))
                {
                    Directory.CreateDirectory(DefaultConfiguration.AppDomain());
                }

                File.WriteAllText(path, output);
            }
        }
    }
}
