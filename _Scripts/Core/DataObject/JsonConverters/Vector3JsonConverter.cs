using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Core
{
    public class Vector3JsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string s = (string)reader.Value;
            string[] splits = s.Split(',');

            if (splits.Length != 3)
            {
                Debug.LogError("Parsing Vector3 failed");
                return new Vector3();
            }

            float x = float.Parse(splits[0]);
            float y = float.Parse(splits[1]);
            float z = float.Parse(splits[2]);

            return new Vector3(x, y, z);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Vector3 vector = (Vector3)value;
            string s = string.Format("{0},{1},{2}", vector.x, vector.y, vector.z);
            writer.WriteValue(s);
        }
    }
}
