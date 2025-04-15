using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using XFramework.Core;

public static class DataObjectExtension
{
    public static string ToJson(this IDataObject obj)
    {
        if (obj == null)
            throw new ArgumentNullException("obj");

        return JsonConvert.SerializeObject(obj);
    }

    public static string ToXml(this IDataObject obj)
    {
        if (obj == null)
            throw new ArgumentNullException("obj");

        using (MemoryStream ms = new MemoryStream())
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "    ";
            settings.NewLineChars = "\r\n";
            settings.Encoding = Encoding.UTF8;

            using (XmlWriter xw = XmlWriter.Create(ms, settings))
            {
                serializer.Serialize(xw, obj);
            }

            ms.Position = 0;
            using (StreamReader reader = new StreamReader(ms, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
