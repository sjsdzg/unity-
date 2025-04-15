using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SuperSocket.ProtoBase;
using XFramework.Util;

namespace XFramework.Core
{
    /// <summary>
    /// 包头
    /// </summary>
    public class Header
    {
        /// <summary>
        /// 协议头标志位 0x01:请求 0x02:响应
        /// </summary>
        private byte tag;
        /// <summary>
        /// 模块号
        /// </summary>
        private short moduleId;
        /// <summary>
        /// 命令号
        /// </summary>
        private short commandId;
        /// <summary>
        /// 校验(CRC32)
        /// </summary>
        private int check;
        /// <summary>
        /// 响应状态
        /// </summary>
        private byte status;
        /// <summary>
        /// 协议数据(data)长度
        /// </summary>
        private int length;

        /// <summary>
        /// 头长度
        /// </summary>
        public const int HEAD_LEN = 14;

        public byte Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public short ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        public short CommandId
        {
            get { return commandId; }
            set { commandId = value; }
        }

        public int Check
        {
            get { return check; }
            set { check = value; }
        }

        public byte Status
        {
            get { return status; }
            set { status = value; }
        }

        public int Length
        {
            get { return length; }
            set { length = value; }
        }

        public Header()
        {

        }

        public Header(byte tag, short moduleId, short commandId, int check, byte status, int length)
        {
            this.tag = tag;
            this.moduleId = moduleId;
            this.commandId = commandId;
            this.check = check;
            this.status = status;
            this.length = length;
        }

        /// <summary>
        /// 字节数组转化成包头
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static Header ParseFrom(byte[] array)
        {
            if (array.Length != HEAD_LEN)
                return null;

            Header header = new Header();
            ByteBuffer buffer = ByteBuffer.Wrap(array);
            header.tag = buffer.Get();
            header.moduleId = buffer.GetShort();
            header.commandId = buffer.GetShort();
            header.check = buffer.GetInt();
            header.status = buffer.Get();
            header.length = buffer.GetInt();
            return header;
        }

        /// <summary>
        /// 包头转化成字节数组
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            ByteBuffer buffer = ByteBuffer.Allocate(HEAD_LEN);
            buffer.Put(tag);
            buffer.PutShort(moduleId);
            buffer.PutShort(commandId);
            buffer.PutInt(check);
            buffer.Put(status);
            buffer.PutInt(length);
            return buffer.ToArray();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("标志：" + tag + "    ");
            sb.Append("模块号：" + moduleId + "    ");
            sb.Append("命令号：0x" + commandId.ToString("x2").PadLeft(4, '0') + "    ");
            sb.Append("校验：" + check + "    ");
            sb.Append("响应状态：" + status + "    ");
            sb.Append("长度：" + length + "    ");
            return sb.ToString();
        }
    }
}
