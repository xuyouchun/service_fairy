using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Package.Service;

namespace ServiceFairy
{
    /// <summary>
    /// 提供状态码的描述供查阅
    /// </summary>
    [AppComponent("状态码管理器", "提供状态码的描述供查阅", category: AppComponentCategory.System, name: "Sys_AppStatusCodeManager")]
    public class AppStatusCodeManagerComponent : AppComponent
    {
        public AppStatusCodeManagerComponent(IAppService service)
            : base(service)
        {

        }

        private readonly Dictionary<int, AppStatusCodeInfo> _dict = new Dictionary<int, AppStatusCodeInfo>();

        /// <summary>
        /// 添加状态码描述信息
        /// </summary>
        /// <param name="info"></param>
        public void Add(AppStatusCodeInfo info)
        {
            Contract.Requires(info != null);

            lock (_dict)
            {
                _Add(info);
            }
        }

        /// <summary>
        /// 批量添加状态码描述信息
        /// </summary>
        /// <param name="infos"></param>
        public void Add(IEnumerable<AppStatusCodeInfo> infos)
        {
            lock (_dict)
            {
                foreach (AppStatusCodeInfo info in infos)
                {
                    _Add(info);
                }
            }
        }

        private void _Add(AppStatusCodeInfo info)
        {
            _dict[info.Code] = info;
        }

        /// <summary>
        /// 删除状态码描述信息
        /// </summary>
        /// <param name="statusCode"></param>
        public void Remove(int statusCode)
        {
            lock (_dict)
            {
                _dict.Remove(statusCode);
            }
        }

        /// <summary>
        /// 获取状态码描述信息
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public AppStatusCodeInfo Get(int statusCode)
        {
            lock (_dict)
            {
                return _dict.GetOrDefault(statusCode);
            }
        }

        /// <summary>
        /// 获取全部状态码描述信息
        /// </summary>
        /// <returns></returns>
        public AppStatusCodeInfo[] GetAll()
        {
            lock (_dict)
            {
                return _dict.Values.ToArray();
            }
        }
    }
}
