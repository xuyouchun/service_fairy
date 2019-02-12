using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;

namespace Common.Algorithms
{
    /// <summary>
    /// 加密解密
    /// </summary>
    public unsafe class LargeDataCryptoServiceProvider
    {
        public LargeDataCryptoServiceProvider(string baseKey)
            : this(Encoding.UTF8.GetBytes(baseKey))
        {
            
        }

        public LargeDataCryptoServiceProvider(byte[] baseKey)
        {
            _baseSecurityKey = _CreateSecurityKey(baseKey);
        }

        private readonly byte[] _baseSecurityKey;
        private const int SECURITY_KEY_LENGHT = 1024 * 1024 * 2;
        private const string PRIVATE_KEY = "E755002E79484D40B781C4F7B22BF84F2372530EE09B470C91AF0DE57E5E083FC761FE09AE524AE3BBCF86D143B9C51E5BBC84E3519C4BA9B23B47EB438DAD994D279B4931B84D728D8E42BA84BAE60C";

        private byte[] _CreateSecurityKey(byte[] baseKey)
        {
            if (baseKey.IsNullOrEmpty())
                baseKey = _emptyKey;

            byte[] buffer = new byte[SECURITY_KEY_LENGHT], privateKeyBuffer = Encoding.UTF8.GetBytes(PRIVATE_KEY);
            fixed (byte* pFixedBuffer = buffer, pFixedKey = baseKey, pFixedPrivateKey = privateKeyBuffer)
            {
                byte* pBuffer = pFixedBuffer, pBufferEnd = pFixedBuffer + buffer.Length;
                byte* pKey = pFixedKey + _ToUInt32(pFixedKey, baseKey.Length) % baseKey.Length, pKeyEnd = pFixedKey + baseKey.Length;
                byte* pPrivateKey = pFixedPrivateKey + _ToUInt32(pFixedPrivateKey, privateKeyBuffer.Length) % privateKeyBuffer.Length, pPrivateKeyEnd = pFixedPrivateKey + privateKeyBuffer.Length;

                int index = 0;
                while (pBuffer < pBufferEnd)
                {
                    *pBuffer++ = (byte)(*pKey ^ *pPrivateKey ^ index++);

                    if (++pKey > pKeyEnd)
                        pKey = pFixedKey;

                    if (++pPrivateKey > pPrivateKeyEnd)
                        pPrivateKey = pFixedPrivateKey;
                }
            }

            return SecurityUtility.DesEncrypt(buffer, SecurityUtility.DesEncrypt(PRIVATE_KEY, Convert.ToBase64String(baseKey)));
        }

        private static uint _ToUInt32(byte[] buffer)
        {
            fixed (byte* pFixedBuffer = buffer)
            {
                return _ToUInt32(pFixedBuffer, buffer.Length);
            }
        }

        private static uint _ToUInt32(byte* pBuffer, int length)
        {
            uint value = 0;

            byte* p = pBuffer;
            for (byte* pBufferEnd = pBuffer + (length & 0xFFFFFFFC); p < pBufferEnd; p += 4)
            {
                value ^= *(uint*)p;
            }

            byte* pValue = (byte*)&value;
            for (byte* pBufferEnd = pBuffer + length; p < pBufferEnd; p++)
            {
                *pValue++ ^= *p;
            }

            return value & 0xFFFFFFFC;
        }

        private static byte[] _ReviseKey(byte[] key)
        {
            if (key.Length % 16 == 0)
                return key;

            int newLen = (key.Length & unchecked((int)0xFFFFFFF0)) + 16;
            byte[] newkey = new byte[newLen];
            Buffer.BlockCopy(key, 0, newkey, 0, key.Length);

            int len = key.Length;
            while (len < newLen)
            {
                int size = Math.Min(newLen - len, key.Length);
                Buffer.BlockCopy(key, 0, newkey, len, size);
                len += size;
            }

            return newkey;
        }

        /// <summary>
        /// 加密指定的数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        public void Encrypt(byte[] data, string key)
        {
            Encrypt(data, key == null ? null : Encoding.UTF8.GetBytes(key));
        }

        /// <summary>
        /// 加密指定的数据
        /// </summary>
        /// <param name="pData">数据起始位置</param>
        /// <param name="length">数据长度</param>
        /// <param name="key">密钥</param>
        public void Encrypt(byte* pData, int length, string key)
        {
            Encrypt(pData, length, key == null ? null : Encoding.UTF8.GetBytes(key));
        }

        /// <summary>
        /// 加密指定的数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public void Encrypt(byte[] data, byte[] key)
        {
            if (data.IsNullOrEmpty())
                return;

            fixed (byte* pFixedData = data)
            {
                _Encrypt(pFixedData, data.Length, key);
            }
        }

        /// <summary>
        /// 加密指定的数据
        /// </summary>
        /// <param name="pData">数据起始位置</param>
        /// <param name="length">数据长度</param>
        /// <param name="key">密钥</param>
        public void Encrypt(byte* pData, int length, byte[] key)
        {
            Contract.Requires(pData != null && length >= 0);

            _Encrypt(pData, length, key);
        }

        private void _Encrypt(byte* pData, int length, byte[] key)
        {
            if (key.IsNullOrEmpty())
                key = _emptyKey;

            key = _ReviseKey(key);
            fixed (byte* pFixedKey = key, pFixedSecurityKey = _baseSecurityKey)
            {
                byte* pKey = pFixedKey, pKeyEnd = pKey + key.Length;

                byte* pSecurityKeyStart = pFixedSecurityKey + _ToUInt32(pKey, key.Length) % (_baseSecurityKey.Length >> 1),
                    pSecurityKey = pSecurityKeyStart, pSecurityKeyEnd = pSecurityKeyStart + (((_baseSecurityKey.Length >> 1) - 4) & 0xFFFFFFF0);

                byte* p = pData;

                for (byte* pDataEnd = pData + (length & 0xFFFFFFF0); p < pDataEnd; p += 4)
                {
                    *(int*)p ^= *(int*)pSecurityKey ^ *(int*)pKey;
                    *(int*)(p += 4) ^= *(int*)(pSecurityKey += 4) ^ *(int*)(pKey += 4);
                    *(int*)(p += 4) ^= *(int*)(pSecurityKey += 4) ^ *(int*)(pKey += 4);
                    *(int*)(p += 4) ^= *(int*)(pSecurityKey += 4) ^ *(int*)(pKey += 4);

                    if ((pSecurityKey += 4) >= pSecurityKeyEnd)
                        pSecurityKey = pSecurityKeyStart;

                    if ((pKey += 4) >= pKeyEnd)
                        pKey = pFixedKey;
                }

                for (byte* pDataEnd = pData + (length & 0xFFFFFFFC); p < pDataEnd; p += 4)
                {
                    *(int*)p ^= *(int*)pSecurityKey ^ *(int*)pKey;

                    if ((pSecurityKey += 4) >= pSecurityKeyEnd)
                        pSecurityKey = pSecurityKeyStart;

                    if ((pKey += 4) >= pKeyEnd)
                        pKey = pFixedKey;
                }

                for (byte* pDataEnd = pData + length; p < pDataEnd; p++)
                {
                    *p ^= (byte)(*pSecurityKey ^ *pKey);

                    if (++pSecurityKey >= pSecurityKeyEnd)
                        pSecurityKey = pSecurityKeyStart;

                    if (++pKey >= pKeyEnd)
                        pKey = pFixedKey;
                }
            }
        }

        /// <summary>
        /// 创建加密解密算法
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ICryptoTransform CreateCryptoTransform(byte[] key)
        {
            return new CryptoTransform(this, key);
        }

        /// <summary>
        /// 创建加密解密算法
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ICryptoTransform CreateCryptoTransform(string key)
        {
            return CreateCryptoTransform(key == null ? null : Encoding.UTF8.GetBytes(key));
        }

        #region Class CryptoTransform ...

        unsafe class CryptoTransform : ICryptoTransform
        {
            public CryptoTransform(LargeDataCryptoServiceProvider sp, byte[] key)
            {
                _sp = sp;
                if (key.IsNullOrEmpty())
                    key = _emptyKey;

                _key = _ReviseKey(key);
                _securityKey = sp._baseSecurityKey;

                fixed (byte* pKey = _key)
                {
                    _securityKeyBegin = _ToUInt32(pKey, _key.Length) % ((uint)_securityKey.Length >> 1);
                    _securityKeyEnd = _securityKeyBegin + (((uint)_securityKey.Length >> 1) - 4) & 0xFFFFFFF0;
                }

                _securityKeyIndex = _securityKeyBegin;
            }

            private readonly LargeDataCryptoServiceProvider _sp;
            private readonly byte[] _securityKey, _key;
            private readonly uint _securityKeyBegin, _securityKeyEnd;
            private uint _securityKeyIndex, _keyIndex;

            public bool CanReuseTransform
            {
                get { return false; }
            }

            public bool CanTransformMultipleBlocks
            {
                get { return false; }
            }

            public int InputBlockSize
            {
                get { return 1024 * 16; }  // 必须为16的倍数
            }

            public int OutputBlockSize
            {
                get { return 1024 * 16; }  // 必须为16的倍数
            }

            public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
            {
                if (outputBuffer.Length - outputOffset < inputCount)
                    throw new ArgumentOutOfRangeException("outputBuffer", "输出流缓冲区空间不足");

                fixed (byte* pFixedInput = inputBuffer, pFixedOutput = outputBuffer)
                {
                    byte* pInputStart = pFixedInput + inputOffset, pInput = pInputStart;
                    byte* pOutput = pFixedOutput + outputOffset;

                    fixed (byte* pFixedSecurityKey = _securityKey, pFixedKey = _key)
                    {
                        byte* pSecurityKey = pFixedSecurityKey + _securityKeyIndex, pSecurityKeyEnd = pFixedSecurityKey + _securityKeyEnd;
                        byte* pKey = pFixedKey + _keyIndex, pKeyEnd = pFixedKey + _key.Length;

                        for (byte* pInputEnd = pInputStart + (inputCount & 0xFFFFFFF0); pInput < pInputEnd; pInput += 4, pOutput += 4)
                        {
                            *(int*)pOutput = *(int*)pInput ^ *(int*)pSecurityKey ^ *(int*)pKey;
                            *(int*)(pOutput += 4) = *(int*)(pInput += 4) ^ *(int*)(pSecurityKey += 4) ^ *(int*)(pKey += 4);
                            *(int*)(pOutput += 4) = *(int*)(pInput += 4) ^ *(int*)(pSecurityKey += 4) ^ *(int*)(pKey += 4);
                            *(int*)(pOutput += 4) = *(int*)(pInput += 4) ^ *(int*)(pSecurityKey += 4) ^ *(int*)(pKey += 4);

                            if ((pSecurityKey += 4) >= pSecurityKeyEnd)
                                pSecurityKey = pFixedSecurityKey + _securityKeyBegin;

                            if ((pKey += 4) >= pKeyEnd)
                                pKey = pFixedKey;
                        }

                        for (byte* pInputEnd = pInputStart + (inputCount & 0xFFFFFFFC); pInput < pInputEnd; pInput += 4, pOutput += 4)
                        {
                            *(int*)pOutput = *(int*)pInput ^ *(int*)pSecurityKey ^ *(int*)pKey;

                            if ((pSecurityKey += 4) >= pSecurityKeyEnd)
                                pSecurityKey = pFixedSecurityKey + _securityKeyBegin;

                            if ((pKey += 4) >= pKeyEnd)
                                pKey = pFixedKey;
                        }

                        for (byte* pInputEnd = pFixedInput + inputOffset + inputCount; pInput < pInputEnd; pInput++, pOutput++)
                        {
                            *pOutput = (byte)(*pInput ^ *pSecurityKey ^ *pKey);

                            if (++pSecurityKey >= pSecurityKeyEnd)
                                pSecurityKey = pFixedSecurityKey + _securityKeyBegin;

                            if (++pKey >= pKeyEnd)
                                pKey = pFixedKey;
                        }

                        _securityKeyIndex = (uint)(pSecurityKey - pFixedSecurityKey);
                        _keyIndex = (uint)(pKey - pFixedKey);
                    }
                }

                return inputCount;
            }

            public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
            {
                byte[] outputBuffer = new byte[inputCount];
                TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0);
                return outputBuffer;
            }

            public void Dispose()
            {
                
            }
        }

        #endregion

        private static readonly byte[] _emptyKey = new byte[] { 28, 12, 135, 71, 13, 201, 62, 223, 80, 21, 52, 156, 9, 12, 23, 129 };
    }
}
