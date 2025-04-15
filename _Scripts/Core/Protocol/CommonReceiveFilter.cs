using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Core
{
    public class CommonReceiveFilter : FixedHeaderReceiveFilter<NetworkPackageInfo>
    {
        public CommonReceiveFilter() : base(Header.HEAD_LEN)
        {
        }

        public override NetworkPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            BufferStream bufStream = bufferStream as BufferStream;
            if (bufStream.Length == Header.HEAD_LEN)
            {
                bufStream.Position = 0;
            }

            byte[] array = new byte[Header.HEAD_LEN];
            bufStream.Read(array, 0, Header.HEAD_LEN);
            Header header = Header.ParseFrom(array);
            Debug.Log(header.ToString());

            byte[] body = new byte[header.Length];
            bufStream.Read(body, 0, header.Length);

            return new NetworkPackageInfo(header, body);
        }

        protected override int GetBodyLengthFromHeader(IBufferStream bufferStream, int length)
        {
            bufferStream.Skip(Header.HEAD_LEN - 4);
            return bufferStream.ReadInt32();
        }
    }
}
