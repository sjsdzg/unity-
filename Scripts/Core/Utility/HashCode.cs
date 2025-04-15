using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework.Core
{
    public static class HashCode
    {
        public static int Calculate<T>(T value)
        {
            return Combine(value.GetHashCode(), value.ToString().GetHashCode());
        }

        //public static int CalculateID<T>(T value)
        //{
        //    return Combine(LGuiContext.GetCurrentID(), value.GetHashCode(), value.ToString().GetHashCode());
        //}

        public static int Combine<T1, T2>(T1 value1, T2 value2)
        {
            int hashCode1 = value1.GetHashCode();
            int hashCode2 = value2.GetHashCode();
            var Rol5 = ((uint)hashCode1 << 5) | ((uint)hashCode2 >> 27);
            return ((int)Rol5 + hashCode1) ^ hashCode2;
        }

        public static int Combine(int value1, int value2, int value3)
        {
            return Combine(Combine(value1, value2), value3);
        }

        public static int Combine(int value1, int value2, int value3, int value4)
        {
            return Combine(Combine(Combine(value1, value2), value3), value4);
        }

        public static int Combine(int value1, int value2, int value3, int value4, int value5)
        {
            return Combine(Combine(Combine(Combine(value1, value2), value3), value4), value5);
        }

        public static int Combine(int value1, int value2, int value3, int value4, int value5, int value6)
        {
            return Combine(Combine(Combine(Combine(Combine(value1, value2), value3), value4), value5), value6);
        }

        public static int Combine(int value1, int value2, int value3, int value4, int value5, int value6, int value7)
        {
            return Combine(Combine(Combine(Combine(Combine(Combine(value1, value2), value3), value4), value5), value6), value7);
        }

        public static int Combine(int value1, int value2, int value3, int value4, int value5, int value6, int value7, int value8)
        {
            return Combine(Combine(Combine(Combine(Combine(Combine(Combine(value1, value2), value3), value4), value5), value6), value7), value8);
        }

        public static int Combine(int value1, int value2, int value3, int value4, int value5, int value6, int value7, int value8, int value9)
        {
            return Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(value1, value2), value3), value4), value5), value6), value7), value8), value9);
        }

        public static int Combine(int value1, int value2, int value3, int value4, int value5, int value6, int value7, int value8, int value9, int value10)
        {
            return Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(Combine(value1, value2), value3), value4), value5), value6), value7), value8), value9), value10);
        }

        public static int Array<T>(T[] Items)
        {
            if (Items == null || Items.Length == 0)
            {
                return 0;
            }

            var Hash = Items[0].GetHashCode();
            for (var Index = 1; Index < Items.Length; ++Index)
            {
                Hash = Combine(Hash, Items[Index]?.GetHashCode() ?? Index);
            }

            return Hash;
        }
    }
}
