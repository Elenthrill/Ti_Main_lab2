using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamEncrypt.BackEnd
{
    class Encryption
    {
        public static byte[] plaintext;
        public static byte[] ciphertext;

        public static void Encrypt(byte[] gen_key)
        {
            for(int i = 0; i < plaintext.Length; i++)
            {
                ciphertext[i] = (byte)(plaintext[i] ^ gen_key[i]);
            }
        }
    }
}
