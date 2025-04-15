using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Architectural
{
    public class EncodingUtil
    {
        public static Encoding GetType(Stream fs)
        {
            byte[] unicode = { 0xFF, 0xFE, 0x41 };
            byte[] unicodeBig = { 0xFE, 0xFF, 0x00 };
            byte[] utf8 = { 0xEF, 0xBB, 0xBF }; //with BOM
            Encoding reVal = Encoding.ASCII; //.Default;

            BinaryReader r = new BinaryReader(fs, Encoding.Default);
            int i;
            if (!int.TryParse(fs.Length.ToString(CultureInfo.InvariantCulture), out i))
                return null;

            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || (ss[0] == utf8[0] && ss[1] == utf8[1] && ss[2] == utf8[2]))
                reVal = Encoding.UTF8;
            else if (ss[0] == unicodeBig[0] && ss[1] == unicodeBig[1] && ss[2] == unicodeBig[2])
                reVal = Encoding.BigEndianUnicode;
            else if (ss[0] == unicode[0] && ss[1] == unicode[1] && ss[2] == unicode[2])
                reVal = Encoding.Unicode;
            return reVal;
        }

        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;
            for (int i = 0; i < data.Length; i++)
            {
                byte curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        while (((curByte <<= 1) & 0x80) != 0)
                            charByteCounter++;

                        if (charByteCounter == 1 || charByteCounter > 6)
                            return false;
                    }
                }
                else
                {
                    if ((curByte & 0xC0) != 0x80)
                        return false;
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
                throw new Exception("Error byte format.");

            return true;
        }
    }
}
