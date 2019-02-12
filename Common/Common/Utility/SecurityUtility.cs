using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utility
{
    /// <summary>
    /// 安全工具函数
    /// </summary>
    public static class SecurityUtility
    {
        /// <summary>
        /// 将字符串进行md5加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public unsafe static string Md5(string s)
        {
            Contract.Requires(s != null);

            byte[] bytes = Md5(Encoding.UTF8.GetBytes(s));
            StringBuilder sb = new StringBuilder();
            fixed (byte* pBytes = bytes)
            {
                byte* p = pBytes, pEnd = p + bytes.Length;
                while (p < pEnd)
                {
                    sb.Append((*p).ToString("X").PadLeft(2, '0'));
                    p++;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 将流进行md5加密
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Md5(byte[] bytes)
        {
            Contract.Requires(bytes != null);

            return MD5.Create().ComputeHash(bytes);
        }

        private static readonly byte[] _iv = _ToBytes("06D4672E-4296-42FC-9342-2A3AB3A08108", 8);

        private unsafe static byte[] _ToBytes(string s, int length)
        {
            while (s.Length < length)
            {
                s += s;
            }

            byte[] result = new byte[length];
            fixed (byte* pFixedBytes = Encoding.UTF8.GetBytes(s), pFixedResult = result)
            {
                byte* pBytes = pFixedBytes, pBytesEnd = pBytes + length, pResult = pFixedResult, pResultEnd = pResult + length;
                while (pBytes < pBytesEnd)
                {
                    *pResult ^= *pBytes;

                    pBytes++;
                    if (++pResult >= pResultEnd)
                        pResult = pFixedResult;
                }
            }

            return result;
        }

        /// <summary>
        /// 将字符串进行DES加密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DesEncrypt(string s, string key)
        {
            Contract.Requires(s != null && key != null);

            byte[] bytes = DesEncrypt(Encoding.UTF8.GetBytes(s), key);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 将字节流进行DES加密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] DesEncrypt(byte[] bytes, string key)
        {
            Contract.Requires(bytes != null && key != null);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = _ToBytes(key, 8);
            des.IV = _iv;

            using(MemoryStream ms = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(bytes);
                cryptoStream.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 将字符串进行DES解密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DesDecrypt(string s, string key)
        {
            Contract.Requires(s != null && key != null);

            byte[] bytes = DesDecrypt(Convert.FromBase64String(s), key);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 将字节流进行DES解密
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] DesDecrypt(byte[] bytes, string key)
        {
            Contract.Requires(bytes != null && key != null);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = _ToBytes(key, 8);
            des.IV = _iv;

            using(MemoryStream ms = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(bytes);
                cryptoStream.FlushFinalBlock();

                return ms.ToArray();
            }
        }

        /// <summary>
        /// 生成一对公钥与私钥
        /// <param name="privateKey"></param>
        /// <param name="publicKey"></param>
        /// </summary>
        public static void GenerateRasKey(out string publicKey, out string privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            privateKey = rsa.ToXmlString(true);
            publicKey = rsa.ToXmlString(false);
        }

        /// <summary>
        /// 非对称加密
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static byte[] RsaEncrypt(byte[] bytes, string publicKey)
        {
            Contract.Requires(bytes != null && publicKey != null);

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);
            return rsa.Encrypt(bytes, false);
        }

        /// <summary>
        /// 非对称加密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static byte[] RsaEncryptString(string s, string publicKey)
        {
            Contract.Requires(s != null && publicKey != null);

            return RsaEncrypt(Encoding.UTF8.GetBytes(s), publicKey);
        }

        /// <summary>
        /// 非对称解密
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static byte[] RsaDecrypt(byte[] bytes, string privateKey)
        {
            Contract.Requires(bytes != null && privateKey != null);

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);
            return rsa.Decrypt(bytes, false);
        }

        /// <summary>
        /// 非对称解密
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static string RsaDecryptString(byte[] bytes, string privateKey)
        {
            Contract.Requires(bytes != null && privateKey != null);

            return Encoding.UTF8.GetString(RsaDecrypt(bytes, privateKey));
        }
    }
}
