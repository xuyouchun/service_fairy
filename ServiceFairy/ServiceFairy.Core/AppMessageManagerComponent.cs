using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Package.Service;
using System.Reflection;

namespace ServiceFairy
{
    /// <summary>
    /// 提供所有的消息属性供查阅
    /// </summary>
    [AppComponent("消息管理器", "提供所有的消息属性供查阅", AppComponentCategory.System, "Sys_AppMessageManager")]
    public class AppMessageManagerComponent : AppComponent
    {
        public AppMessageManagerComponent(IAppService service)
            : base(service)
        {

        }

        private readonly Dictionary<MessageDesc, AppMessageInfo> _msgInfos = new Dictionary<MessageDesc, AppMessageInfo>();

        /// <summary>
        /// 添加消息信息
        /// </summary>
        /// <param name="info"></param>
        public void Add(AppMessageInfo info)
        {
            Contract.Requires(info != null);

            lock (_msgInfos)
            {
                _Add(info);
            }
        }

        /// <summary>
        /// 批量添加消息信息
        /// </summary>
        /// <param name="infos"></param>
        public void Add(IEnumerable<AppMessageInfo> infos)
        {
            Contract.Requires(infos != null);

            lock (_msgInfos)
            {
                foreach (AppMessageInfo info in infos)
                {
                    _Add(info);
                }
            }
        }

        private void _Add(AppMessageInfo info)
        {
            _msgInfos[info.MessageDesc] = info;
        }

        /// <summary>
        /// 获取消息信息
        /// </summary>
        /// <param name="messageDesc"></param>
        /// <returns></returns>
        public AppMessageInfo Get(MessageDesc messageDesc)
        {
            Contract.Requires(messageDesc != null);

            lock (_msgInfos)
            {
                return _msgInfos.GetOrDefault(messageDesc);
            }
        }

        /// <summary>
        /// 获取消息信息
        /// </summary>
        /// <returns></returns>
        public AppMessageInfo[] GetAll()
        {
            lock (_msgInfos)
            {
                return _msgInfos.Values.ToArray();
            }
        }
    }
}
