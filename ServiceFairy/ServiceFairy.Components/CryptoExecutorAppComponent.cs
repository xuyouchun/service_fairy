using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Common.Algorithms;
using Common.Contracts.Service;
using Common.Package;
using Common.Package.Service;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 加密解密运算器
    /// </summary>
    [AppComponent("加密解密运算器", "执行加密解密的运算", AppComponentCategory.System, "Sys_CryptoExecutor")]
    public class CryptoExecutorAppComponent : AppComponent
    {
        public CryptoExecutorAppComponent(CoreAppServiceBase service)
            : base(service)
        {
            _service = service;
        }

        private readonly CoreAppServiceBase _service;
        private readonly Cache<string, LargeDataCryptoServiceProvider> _cache = new Cache<string, LargeDataCryptoServiceProvider>();

        /// <summary>
        /// 创建加密解密执行器
        /// </summary>
        /// <param name="baseKey"></param>
        /// <returns></returns>
        public LargeDataCryptoServiceProvider GetCryptoServiceProvider(string baseKey)
        {
            Contract.Requires(baseKey != null);

            return _cache.GetOrAddOfDynamic(baseKey, TimeSpan.FromSeconds(60), (key0) => new LargeDataCryptoServiceProvider(key0));
        }
    }
}
