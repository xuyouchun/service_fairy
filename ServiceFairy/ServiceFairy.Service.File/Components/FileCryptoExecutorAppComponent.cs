using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Algorithms;
using Common.Package;
using Common.File.UnionFile;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;

namespace ServiceFairy.Service.File.Components
{
    /// <summary>
    /// 文件安全组件
    /// </summary>
    [AppComponent("文件安全组件", "实现文件的加密解密")]
    class FileCryptoExecutorAppComponent : AppComponent
    {
        public FileCryptoExecutorAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;
        private const string DEFAULT_BASEKEY = "1EF951F96EEA427193E3A11BD81FE76F46086EBC6AC8402BBF78403A3920D7E66AAE23244C0448F1A45B61B61116396B5574833F62C24F4D975FDBC6E7FBFCDC";

        private LargeDataCryptoServiceProvider _GetCryptoServiceProvider(string baseKey)
        {
            return _service.CryptoExecutor.GetCryptoServiceProvider(baseKey ?? DEFAULT_BASEKEY);
        }

        /// <summary>
        /// 为写入的文件流添加加密适配器
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public UnionFileStream CreateEncryptUnionFileStreamAdapter(UnionFileStream stream, string key, string baseKey = null)
        {
            Contract.Requires(stream != null);

            CryptoStream cryptoStream = new CryptoStream(stream, _GetCryptoServiceProvider(baseKey).CreateCryptoTransform(key), CryptoStreamMode.Write);
            return new UnionFileStream(cryptoStream);
        }

        /// <summary>
        /// 为读取的文件流添加解密适配器
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="key"></param>
        /// <param name="baseKey"></param>
        /// <returns></returns>
        public UnionFileStream CreateDecryptUnionFileStreamAdapter(UnionFileStream stream, string key, string baseKey = null)
        {
            Contract.Requires(stream != null);

            CryptoStream cryptoStream = new CryptoStream(stream, _GetCryptoServiceProvider(baseKey).CreateCryptoTransform(key), CryptoStreamMode.Read);
            return new UnionFileStream(cryptoStream);
        }
    }
}
