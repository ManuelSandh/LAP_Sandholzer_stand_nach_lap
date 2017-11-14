using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CardGame.DAL.Logic
{
    public class Helper
    {
        public static bool IsCCValid(string cc)
        {
            int totalSum = 0;
            bool odd = false;
            for (int i = cc.Length - 1; i >= 0; i--)
            {
                int num = int.Parse(cc[i].ToString());
                if (odd)
                {
                    num *= 2;
                    if (num > 9)
                        num -= 9;
                    totalSum += num;
                }
                else
                {
                    totalSum += num;
                }
                odd = !odd;
            }
            return totalSum % 10 == 0;

        }




        public static byte[] GenerateHash(string s)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(s);
            using (SHA512 sha = new SHA512Managed())
            {
                return sha.ComputeHash(bytes);
            }        
        }

        public static string GenerateSalt()
        {
            var salt = new byte[64];
            var rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(salt);

            return GetHexNotation(salt);
        }

        private static string GetHexNotation(byte[] bytes)
        {
            var hashStringBuilder = new StringBuilder(128);

            foreach (var b in bytes)
            {
                hashStringBuilder.Append(b.ToString("X2"));
            }

            return hashStringBuilder.ToString();
        }

    }
}
