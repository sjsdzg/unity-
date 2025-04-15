using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.IO
{
    public interface IValueReader
    {
        short Code { get; }

        object Value { get; }

        long CurrentPosition { get; }

        void Next();

        byte ReadByte();

        byte[] ReadBytes();

        short ReadShort();

        int ReadInt();

        long ReadLong();

        bool ReadBool();

        float ReadFloat();

        double ReadDouble();

        string ReadString();

        Vector2 ReadVector2();

        Vector3 ReadVector3();

        Quaternion ReadQuaternion();

        Color32 ReadColor32();
    }
}
