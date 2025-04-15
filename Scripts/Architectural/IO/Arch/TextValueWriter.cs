using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XFramework.Common;

namespace XFramework.IO
{
    public class TextValueWriter : IValueWriter
    {
        private readonly TextWriter writer;

        private short code;
        /// <summary>
        /// 
        /// </summary>
        public short Code
        {
            get { return code; }
        }

        private object value;
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
        public long CurrenPosition
        {
            get { return currentPosition; }
        }

        public TextValueWriter(TextWriter writer)
        {
            this.writer = writer;
            this.currentPosition = 0;
            this.code = 0;
            this.value = null;
        }

        public void Write(short code, object value)
        {
            this.code = code;
            this.writer.WriteLine(code);
            this.currentPosition += 1;

            if (value is bool)
            {
                this.WriteBool((bool)value);
            }
            else if (value is byte)
            {
                this.WriteByte((byte)value);
            }
            else if (value is byte[])
            {
                this.WriteBytes((byte[])value);
            }
            else if (value is float)
            {
                this.WriteFloat((float)value);
            }
            else if (value is short)
            {
                this.WriteShort((short)value);
            }
            else if (value is int)
            {
                this.WriteInt((int)value);
            }
            else if (value is long)
            {
                this.WriteLong((long)value);
            }
            else if (value is string)
            {
                this.WriteString((string)value);
            }
            else if (value is Vector2)
            {
                this.WriteVector2((Vector2)value);
            }
            else if (value is Vector3)
            {
                this.WriteVector3((Vector3)value);
            }
            else if (value is Quaternion)
            {
                this.WriteQuaternion((Quaternion)value);
            }
            else if (value is Color32)
            {
                this.WriteColor32((Color32)value);
            }
            else
            {
                throw new Exception(string.Format("Code {0} not valid at line {1}", this.code, this.currentPosition));
            }

            this.value = value;
            this.currentPosition += 1;
        }

        public void WriteBool(bool value)
        {
            this.writer.WriteLine(value ? 1 : 0);
        }

        public void WriteByte(byte value)
        {
            this.writer.WriteLine(value.ToString(CultureInfo.InvariantCulture));
        }

        public void WriteBytes(byte[] value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte v in value)
            {
                sb.Append(string.Format("{0:X2}", v));
            }
            this.writer.WriteLine(sb.ToString());
        }

        public void WriteFloat(float value)
        {
            this.writer.WriteLine(value.ToString("0.0######", CultureInfo.InvariantCulture));
        }

        public void WriteDouble(double value)
        {
            this.writer.WriteLine(value.ToString("0.0###############", CultureInfo.InvariantCulture));
        }

        public void WriteShort(short value)
        {
            this.writer.WriteLine(value.ToString(CultureInfo.InvariantCulture));
        }

        public void WriteInt(int value)
        {
            this.writer.WriteLine(value.ToString(CultureInfo.InvariantCulture));
        }

        public void WriteLong(long value)
        {
            this.writer.WriteLine(value.ToString(CultureInfo.InvariantCulture));
        }

        public void WriteString(string value)
        {
            this.writer.WriteLine(value);
        }

        public void WriteVector2(Vector2 value)
        {
            this.writer.WriteLine(string.Format("{0},{1}", value.x, value.y));
        }

        public void WriteVector3(Vector3 value)
        {
            this.writer.WriteLine(string.Format("{0},{1},{2}", value.x, value.y, value.z));
        }

        public void WriteQuaternion(Quaternion value)
        {
            this.writer.WriteLine(string.Format("{0},{1},{2},{3}", value.x, value.y, value.z, value.w));
        }

        public void WriteColor32(Color32 value)
        {
            byte[] bytes = new byte[4];
            bytes[0] = value.r;
            bytes[1] = value.g;
            bytes[2] = value.b;
            bytes[3] = value.a;
            WriteBytes(bytes);
        }

        public void Flush()
        {
            this.writer.Flush();
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", this.code, this.value);
        }
    }
}
