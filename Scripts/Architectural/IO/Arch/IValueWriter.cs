using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.IO
{
    public interface IValueWriter
    {
        short Code { get; }

        object Value { get; }

        long CurrenPosition { get; }

        void Write(short code, object value);

        void WriteByte(byte value);

        void WriteBytes(byte[] value);

        void WriteShort(short value);

        void WriteInt(int value);

        void WriteLong(long value);

        void WriteBool(bool value);

        void WriteFloat(float value);

        void WriteDouble(double value);

        void WriteString(string value);

        void WriteVector2(Vector2 value);

        void WriteVector3(Vector3 value);

        void WriteQuaternion(Quaternion value);

        void WriteColor32(Color32 value);

        void Flush();
    }
}
