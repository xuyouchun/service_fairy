using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 网络连接
    /// </summary>
    public interface IConnection : ICommunicate
    {
        /// <summary>
        /// 接收到数据
        /// </summary>
        event ConnectionDataReceivedEventHandler Received;

        /// <summary>
        /// 是否为双向连接
        /// </summary>
        bool Duplex { get; }

        /// <summary>
        /// 连接的状态
        /// </summary>
        ConnectionState State { get; }

        /// <summary>
        /// 打开连接
        /// </summary>
        void Open();
    }

    /// <summary>
    /// 网络连接的状态
    /// </summary>
    public enum ConnectionState
    {
        /// <summary>
        /// 已创建
        /// </summary>
        Created,

        /// <summary>
        /// 已连接
        /// </summary>
        Opened,

        /// <summary>
        /// 已关闭
        /// </summary>
        Closed,
    }

    /// <summary>
    /// 网络连接接收到数据事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ConnectionDataReceivedEventHandler(object sender, ConnectionDataReceivedEventArgs e);

    #region Class ConnectionDataReceivedEventArgs ...

    /// <summary>
    /// 网络连接接收到数据事件参数
    /// </summary>
    public class ConnectionDataReceivedEventArgs : EventArgs
    {
        public ConnectionDataReceivedEventArgs(IConnection connection, CommunicateContext context, string method, CommunicateData requestData, CallingSettings settings)
        {
            Connection = connection;
            Context = context;
            Method = method;
            RequestData = requestData;
            Settings = settings;
        }

        /// <summary>
        /// Wcf连接对象
        /// </summary>
        public IConnection Connection { get; private set; }

        /// <summary>
        /// 调用者
        /// </summary>
        public CommunicateContext Context { get; private set; }

        /// <summary>
        /// 方法
        /// </summary>
        public string Method { get; private set; }

        /// <summary>
        /// 请求数据
        /// </summary>
        public CommunicateData RequestData { get; private set; }

        /// <summary>
        /// 应答数据
        /// </summary>
        public CommunicateData ReplyData { get; set; }

        /// <summary>
        /// 通信设置
        /// </summary>
        public CallingSettings Settings { get; private set; }
    }

    #endregion
}
