using SuperSocket.ProtoBase;

using XFramework.Util;
using XFramework.Network;
namespace XFramework.Core
{
    public class NetworkPackageInfo : PackageInfo<string, byte[]>
    {
        public Header Header { get; private set; }

        public NetworkPackageInfo(Header header, byte[] body) : base(header.CommandId.ToString(), body)
        {
            Header = header;
        }

        public NetworkPackageInfo(short moduleId, short commandId, byte[] body) : base(commandId.ToString(), body)
        {
            int bodyLength = (body == null ? 0 : body.Length);
            Header = new Header(Tag.GENERAL, moduleId, commandId, 0xFFFF, 0x0000, bodyLength);
        }

        /// <summary>
        /// 将包转化成字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            ByteBuffer buffer = ByteBuffer.Allocate(Header.HEAD_LEN + Header.Length);
            buffer.Put(Header.ToArray());
            if (Body != null)
                buffer.Put(Body);

            return buffer.ToArray();
        }
    }
}
