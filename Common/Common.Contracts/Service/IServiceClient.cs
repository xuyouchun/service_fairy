using System;
using System.Diagnostics.Contracts;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 客户端调用接口
    /// </summary>
    public interface IServiceClient : IDisposable
    {
        /// <summary>
        /// 调用不需要返回值的方法
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="input">输入参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        ServiceResult Call(string method, object input, CallingSettings settings = null);

        /// <summary>
        /// 调用需要返回值的方法
        /// </summary>
        /// <typeparam name="replyType">输出参数类型</typeparam>
        /// <param name="method">方法</param>
        /// <param name="input">输入参数</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        ServiceResult<object> Call(string method, object input, Type replyType, CallingSettings settings = null);

        /// <summary>
        /// 注册数据接收器
        /// </summary>
        /// <typeparam name="entityType">实体类型</typeparam>
        /// <param name="method">方法</param>
        /// <param name="receiver">接收器</param>
        IServiceClientReceiverHandler RegisterReceiver(string method, Type entityType, IServiceClientReceiver receiver);
    }

    /// <summary>
    /// 数据接收器
    /// </summary>
    public interface IServiceClientReceiver
    {
        /// <summary>
        /// 接收到数据
        /// </summary>
        /// <param name="e"></param>
        void OnReceive(ServiceClientReceiveEventArgs<object> e);
    }

    /// <summary>
    /// 数据接收器
    /// </summary>
    public interface IServiceClientReceiver<TEntity>
    {
        /// <summary>
        /// 接收到数据
        /// </summary>
        /// <param name="e"></param>
        void OnReceive(ServiceClientReceiveEventArgs<TEntity> e);
    }

    /// <summary>
    /// 数据接收器句柄
    /// </summary>
    public interface IServiceClientReceiverHandler : IDisposable
    {
        /// <summary>
        /// 取消注册
        /// </summary>
        void Unregister();
    }

    /// <summary>
    /// 接收到数据事件
    /// </summary>
    /// <typeparam name="TEntity">事件参数类型</typeparam>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ServiceClientReceiveEventHandler<TEntity>(object sender, ServiceClientReceiveEventArgs<TEntity> e);

    /// <summary>
    /// 接收到数据事件参数
    /// </summary>
    public class ServiceClientReceiveEventArgs<TEntity> : EventArgs
    {
        public ServiceClientReceiveEventArgs(string method, TEntity entity)
        {
            Method = method;
            Entity = entity;
        }

        /// <summary>
        /// 方法
        /// </summary>
        public string Method { get; private set; }

        /// <summary>
        /// 事件参数
        /// </summary>
        public TEntity Entity { get; private set; }
    }

    #region Class ServiceClientReceiverEhAdapter ...

    public class ServiceClientReceiverEhAdapter<TEntity> : IServiceClientReceiver<TEntity>
    {
        public ServiceClientReceiverEhAdapter(object sender, ServiceClientReceiveEventHandler<TEntity> eh)
        {
            Contract.Requires(eh != null);
            _sender = sender;
            _eh = eh;
        }

        private readonly object _sender;
        private readonly ServiceClientReceiveEventHandler<TEntity> _eh;

        public void OnReceive(ServiceClientReceiveEventArgs<TEntity> e)
        {
            _eh(_sender, e);
        }
    }

    #endregion

    #region Class ServiceClientReceiverAdapter ...

    public class ServiceClientReceiverAdapter<TEntity> : IServiceClientReceiver
    {
        public ServiceClientReceiverAdapter(IServiceClientReceiver<TEntity> receiver)
        {
            Contract.Requires(receiver != null);

            _receiver = receiver;
        }

        private readonly IServiceClientReceiver<TEntity> _receiver;

        public void OnReceive(ServiceClientReceiveEventArgs<object> e)
        {
            _receiver.OnReceive(new ServiceClientReceiveEventArgs<TEntity>(e.Method, (TEntity)e.Entity));
        }
    }

    #endregion
}
