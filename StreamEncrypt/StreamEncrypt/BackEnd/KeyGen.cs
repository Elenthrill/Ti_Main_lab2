using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamEncrypt.BackEnd
{
    class KeyGen
    {
        public static int[] polinom = {31, 3, 1};
        public static byte[] Generate(byte[] key, int length) {
            return Generate(key, length, polinom);
        }

        private static byte[] Generate(byte[] key, int length, int[] polinom) {
            byte[] new_key = new byte[length];
            for (int i = 0; i < length; i++)
            {
                new_key[i] = key[0];
                byte first_bit = GetFirstBit(key);
                LShift(key);
                key[key.Length - 1] = first_bit;
            }
            return new_key;
        }
        private static void LShift(byte[] key)
        {
            for (int i = 0; i < key.Length - 1; i++)
                key[i] = key[i + 1];
        }

        private static byte GetFirstBit (byte[] key){
            byte result = 0;
            for (int i = 1; i < polinom.Length; i++)
                result ^= key[polinom[i] - 1];
            return result;
        }
    }
}
