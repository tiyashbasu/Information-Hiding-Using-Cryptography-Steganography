using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Security_SteganographyProject
{
    public static class Converter
    {
        public static byte[] GetBits(byte byt)
        {
            byte[] bits = new byte[8];
            bits[0] = (byte)(byt & 1);
            bits[1] = (byte)((byt >> 1) & 1);
            bits[2] = (byte)((byt >> 2) & 1);
            bits[3] = (byte)((byt >> 3) & 1);
            bits[4] = (byte)((byt >> 4) & 1);
            bits[5] = (byte)((byt >> 5) & 1);
            bits[6] = (byte)((byt >> 6) & 1);
            bits[7] = (byte)((byt >> 7) & 1);
            return bits;
        }

        public static byte[] GetBytes(string str)
        {
            byte[] strBytes = new byte[str.Length];
            char[] strChars = str.ToCharArray();
            for (int i = 0; i < str.Length; i++)
                strBytes[i] = (byte)strChars[i];
            return strBytes;
        }

        public static byte[] GetBytes(long num)
        {
            string str = Convert.ToString(num, 2);
            string substr;
            byte[] longBytes = new byte[(str.Length + 8) / 8];
            int deficit;
            deficit = 8 - (str.Length % 8);
            for (int i = 0; i < deficit; i++)
            {
                str = String.Concat("0", str);
            }
            for (int i = 0; i < longBytes.Length; i++)
            {
                substr = str.Substring(str.Length - (8 * (i + 1)), 8);
                longBytes[i] = Convert.ToByte(substr, 2);
            }
            return longBytes;
        }

        public static long GetLong(byte[] bytes)
        {
            if (bytes == null)
                return -1;
            string str, str2;
            string strx = "00000000";
            int num, sum = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                str = "";
                for (int j = 0; j < i; j++)
                    str = str + strx;
                str2 = Convert.ToString((long)bytes[i], 2) + str;
                num = Convert.ToInt32(str2, 2);
                sum += num;
            }
            return sum;
        }

        public static string GetString(byte[] bytes)
        {
            if (bytes == null)
                return null;
            char[] chars = new char[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
                chars[i] = (char)bytes[i];
            return (new string(chars));
        }
    }
}