using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Core
{
    public class Vector2JsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType.Equals(typeof(Vector2)))
            {
                return true;
            }

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string s = (string)reader.Value;
            string[] splits = s.Split(',');

            if (splits.Length != 2)
            {
                Debug.LogError("Parsing Vector2 failed");
                return new Vector2();
            }

            float x = float.Parse(splits[0]);
            float y = float.Parse(splits[1]);

            return new Vector2(x, y);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Vector2 vector = (Vector2)value;
            string s = string.Format("{0},{1}", vector.x, vector.y);
            writer.WriteValue(s);
        }
    }
}
