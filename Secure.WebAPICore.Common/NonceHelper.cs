using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Secure.WebAPICore.Common
{
    /// <summary>
    /// Nonce generator
    /// </summary>
    public static class  NonceHelper
    {
        private static ConcurrentDictionary<string, Tuple<int, DateTime>>
            nonces = new ConcurrentDictionary<string, Tuple<int, DateTime>>();
        /// <summary>
        /// Generates a nonce according to Digest specs (MD5)
        /// </summary>
        /// <returns></returns>
        public static string Generate()
        {
            byte[] bytes = new byte[16];
            using (var rngProvider = new RNGCryptoServiceProvider())
            {
                rngProvider.GetBytes(bytes);
            }            string nonce = bytes.ToMD5Hash();
            nonces.TryAdd(nonce, new Tuple<int, DateTime>(0, DateTime.Now.AddMinutes(10)));            return nonce;
        }
        /// <summary>
        /// HashHelper        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToMD5Hash(this byte[] bytes)
        {
            StringBuilder hash = new StringBuilder();
            MD5 md5 = MD5.Create();
            md5.ComputeHash(bytes)
            .ToList()
            .ForEach(b => hash.AppendFormat("{0:x2}", b));
            return hash.ToString();
        }
        public static string ToMD5Hash(this string inputString)
        {
            return Encoding.UTF8.GetBytes(inputString).ToMD5Hash();
        }
    }
}
