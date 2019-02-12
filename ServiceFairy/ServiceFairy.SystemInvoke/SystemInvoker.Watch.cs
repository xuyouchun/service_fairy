using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Email;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using ServiceFairy.Entities.Watch;

namespace ServiceFairy.SystemInvoke
{
    partial class SystemInvoker
    {
        private WatchInvoker _watch;

        /// <summary>
        /// Watch Service
        /// </summary>
        public WatchInvoker Watch
        {
            get { return _watch ?? (_watch = new WatchInvoker(this)); }
        }

        /// <summary>
        /// 监控
        /// </summary>
        public class WatchInvoker : Invoker
        {
            public WatchInvoker(SystemInvoker owner)
                : base(owner)
            {

            }

            /// <summary>
            /// 获取监控信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns>监控信息</returns>
            public ServiceResult<WatchInfo> GetWatchInfoSr(CallingSettings settings = null)
            {
                var sr = WatchService.GetWatchInfo(Sc, settings);
                return CreateSr(sr, r => r.Info);
            }

            /// <summary>
            /// 获取监控信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns>监控信息</returns>
            public WatchInfo GetWatchInfo(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetWatchInfoSr(settings));
            }
        }
    }
}
