using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using UnityEngine;

namespace XFramework.IO
{
    public class TextValueReader : IValueReader
    {
        private readonly TextReader reader;

        private short code;
        /// <summary>
        /// 
        /// </summary>
        public short Code 
        {
            get { return code; }
        }

        private string value;
        /// <summary>
        /// 
        /// </summary>
        public object Value
        {
            get { return value; }
        }

        private long currentPosition;
        /// <summary>
        /// 
        /// </summary>
        public long CurrentPosition
        {
            get { return currentPosition; }
        }

        public TextValueReader(TextReader reader)
        {
            this.reader = reader;
            this.code = 0;
            this.value = null;
            this.currentPosition = 0;
        }

        public void Next()
        {
            string readCode = this.reader.ReadLine();
            this.currentPosition += 1;
            if (!short.TryParse(readCode, NumberStyles.Integer, CultureInfo.InvariantCulture, out this.code))
                throw new Exception(string.Format("Code {0} not valid at line {1}", this.code, this.currentPosition));
            this.value = this.reader.ReadLine();
            this.currentPosition += 1;
        }

        public bool ReadBool()
        {
            byte result = this.ReadByte();
            return result > 0;
        }

        public byte ReadByte()
        {
            byte result;
            if (byte.TryParse(this.value, out result))
                return result;

            throw new Exception(string.Format("Value {0} not valid at line {1}", this.value, this.currentPosition));
        }

        public byte[] ReadBytes()
        {
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < this.value.Length; i++)
            {
                string hex = string.Concat(this.value[i], this.value[++i]);
                byte result;
                if (byte.TryParse(hex, NumberStyles.AllowHexSpecifier | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture, out result))
                    bytes.Add(result);
                else
                    throw new Exception(string.Format("Value {0} not valid at line {1}", hex, this.currentPosition));
            }
            return bytes.ToArray();
        }

        public float ReadFloat()
        {
            float result;
            if (float.TryParse(this.value, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
                return result;

            throw new Exception(string.Format("Value {0} not valid at line {1}", this.value, this.currentPosition));
        }

        public double ReadDouble()
        {
            double result;
            if (double.TryParse(this.value, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
                return result;

            throw new Exception(string.Format("Value {0} not valid at line {1}", this.value, this.currentPosition));
        }

        public short ReadShort()
        {
            short result;
            if (short.TryParse(this.value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
                return result;

            throw new Exception(string.Format("Value {0} not valid at line {1}", this.value, this.currentPosition));
        }

        public int ReadInt()
        {
            int result;
            if (int.TryParse(this.value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
                return result;

            throw new Exception(string.Format("Value {0} not valid at line {1}", this.value, this.currentPosition));
        }

        public long ReadLong()
        {
            long result;
            if (long.TryParse(this.value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
                return result;

            throw new Exception(string.Format("Value {0} not valid at line {1}", this.value, this.currentPosition));
        }

        public string ReadString()
        {
            return this.value;
        }

        public Vector2 ReadVector2()
        {
            Vector2 result;

            string[] splits = this.value.Split(',');
            if (splits.Length != 2)
                throw new Exception(string.Format("Value {0} not valid at line {1}", this.value, this.currentPosition));

            float x;
            if (float.TryParse(splits[0], NumberStyles.Float, CultureInfo.InvariantCulture, out x))
                result.x = x;
            else
                throw new Exception(string.Format("Value {0} not valid at line {1}", splits[0], this.currentPosition));

            float y;
            if (float.TryParse(splits[1], NumberStyles.Float, CultureInfo.InvariantCulture, out y))
                result.y = y;
            else
                throw new Exception(string.Format("Value {0} not valid at line {1}", splits[1], this.currentPosition));

            return result;
        }

        public Vector3 ReadVector3()
        {
            Vector3 result;

            string[] splits = this.value.Split(',');
            if (splits.Length != 3)
                throw new Exception(string.Format("Value {0} not valid at line {1}", this.value, this.currentPosition));

            float x;
            if (float.TryParse(splits[0], NumberStyles.Float, CultureInfo.InvariantCulture, out x))
                result.x = x;
            else
                throw new Exception(string.Format("Value {0} not valid at line {1}", splits[0], this.currentPosition));

            float y;
            if (float.TryParse(splits[1], NumberStyles.Float, CultureInfo.InvariantCulture, out y))
                result.y = y;
            else
                throw new Exception(string.Format("Value {0} not valid at line {1}", splits[1], this.currentPosition));

            float z;
            if (float.TryParse(splits[2], NumberStyles.Float, CultureInfo.InvariantCulture, out z))
                result.z = z;
            else
                throw new Exception(string.Format("Value {0} not valid at line {1}", splits[2], this.currentPosition));

            return result;
        }

        public Quaternion ReadQuaternion()
        {
            Quaternion result;

            string[] splits = this.value.Split(',');
            if (splits.Length != 4)
                throw new Exception(string.Format("Value {0} not valid at line {1}", this.value, this.currentPosition));

            float x;
            if (float.TryParse(splits[0], NumberStyles.Float, CultureInfo.InvariantCulture, out x))
                result.x = x;
            else
                throw new Exception(string.Format("Value {0} not valid at line {1}", splits[0], this.currentPosition));

            float y;
            if (float.TryParse(splits[1], NumberStyles.Float, CultureInfo.InvariantCulture, out y))
                result.y = y;
            else
                throw new Exception(string.Format("Value {0} not valid at line {1}", splits[1], this.currentPosition));

            float z;
            if (float.TryParse(splits[2], NumberStyles.Float, CultureInfo.InvariantCulture, out z))
                result.z = z;
            else
                throw new Exception(string.Format("Value {0} not valid at line {1}", splits[2], this.currentPosition));

            float w;
            if (float.TryParse(splits[3], NumberStyles.Float, CultureInfo.InvariantCulture, out w))
                result.w = w;
            else
                throw new Exception(string.Format("Value {0} not valid at line {1}", splits[3], this.currentPosition));

            return result;
        }

        public Color32 ReadColor32()
        {
            if (this.value.Length != 8)
                throw new Exception(string.Format("Value {0} not valid at line {1}", this.value, this.currentPosition));

            List<byte> bytes = new List<byte>();
            for (int i = 0; i < this.value.Length; i++)
            {
                string hex = string.Concat(this.value[i], this.value[++i]);
                byte result;
                if (byte.TryParse(hex, NumberStyles.AllowHexSpecifier | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, CultureInfo.InvariantCulture, out result))
                    bytes.Add(result);
                else
                    throw new Exception(string.Format("Value {0} not valid at line {1}", hex, this.currentPosition));
            }

            return new Color32(bytes[0], bytes[1], bytes[2], bytes[3]);
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", this.code, this.value);
        }
    }
}
