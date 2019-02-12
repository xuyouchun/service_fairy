using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 支持刷新的列表项
    /// </summary>
    public interface IRefreshableServiceObject : IServiceObject, IDisposable
    {
        /// <summary>
        /// 出现错误
        /// </summary>
        event ErrorEventHandler Error;

        /// <summary>
        /// 内容变化事件
        /// </summary>
        event EventHandler Refresh;

        /// <summary>
        /// 是否正在刷新
        /// </summary>
        bool IsRefreshing { get; }

        /// <summary>
        /// 该服务是否已经删除
        /// </summary>
        bool IsDisposed { get; }
    }

    /// <summary>
    /// ServiceObject的刷新逻辑
    /// </summary>
    public interface IServiceObjectRefresher
    {
        /// <summary>
        /// 刷新
        /// </summary>
        /// <returns></returns>
        ServiceObjectRefreshResult Refresh();
    }

    public delegate ServiceObjectRefreshResult ServiceObjectRefreshFunc();

    /// <summary>
    /// 将刷新函数适配为接口IServiceObjectRefresher
    /// </summary>
    public class ServiceObjectRefreshFuncAdapter : IServiceObjectRefresher
    {
        public ServiceObjectRefreshFuncAdapter(ServiceObjectRefreshFunc func)
        {
            Contract.Requires(func != null);

            _func = func;
        }

        private readonly ServiceObjectRefreshFunc _func;

        public ServiceObjectRefreshResult Refresh()
        {
            return _func();
        }
    }

    /// <summary>
    /// ServiceObject的刷新结果
    /// </summary>
    [Flags]
    public enum ServiceObjectRefreshResult
    {
        /// <summary>
        /// 继续
        /// </summary>
        Continue = 0x00,

        /// <summary>
        /// 结束
        /// </summary>
        Completed = 0x01,

        /// <summary>
        /// 不刷新
        /// </summary>
        NotRefresh = 0x00,

        /// <summary>
        /// 刷新
        /// </summary>
        Refresh = 0x02,

        /// <summary>
        /// 删除
        /// </summary>
        Dispose = 0x04,

        /// <summary>
        /// 继续并刷新
        /// </summary>
        ContinueAndRefresh = Continue | Refresh,

        /// <summary>
        /// 结束并刷新
        /// </summary>
        CompletedAndRefresh = Completed | Refresh,

        /// <summary>
        /// 结束并删除
        /// </summary>
        CompletedAndDispose = Completed | Dispose,
    }
}
