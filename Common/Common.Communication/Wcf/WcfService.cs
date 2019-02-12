using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Net;
using System.ServiceModel.Channels;
using Common.Contracts.Service;
using System.Reflection;
using System.Net.Sockets;
using System.ServiceModel;
using Common.Contracts;
using Common.Package;

namespace Common.Communication.Wcf
{
    /// <summary>
    /// WCF服务管理器
    /// </summary>
    public class WcfService : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WcfService()
        {
            
        }

        private readonly HashSet<WcfListener> _listeners = new HashSet<WcfListener>();
        private readonly HashSet<WcfConnection> _connections = new HashSet<WcfConnection>();
        private readonly object _thisLock = new object();
        private volatile bool _disposed = false;

        /// <summary>
        /// 创建终端
        /// </summary>
        /// <param name="address">终端地址</param>
        /// <param name="port">终端端口号</param>
        /// <param name="type">类型</param>
        /// <param name="duplex">是否支持双向通信</param>
        /// <returns>若该终端已存在，则会抛出异常</returns>
        /// <exception cref="ArgumentNullException">info为空</exception>
        /// <exception cref="InvalidOperationException">该终端已经存在</exception>
        public WcfListener CreateListener(ServiceAddress address, CommunicationType type, bool duplex = false)
        {
            Contract.Requires(address != null);

            lock (_thisLock)
            {
                if (_disposed)
                    throw new ObjectDisposedException("WcfService");

                WcfListener listener = WcfListener.Create(this, new CommunicationOption(address, type, duplex));
                lock (_listeners)
                {
                    _listeners.Add(listener);
                }

                return listener;
            }
        }

        /// <summary>
        /// 获取所有监听器
        /// </summary>
        /// <returns>所有监听器</returns>
        public WcfListener[] GetAllListeners()
        {
            lock (_listeners)
            {
                return _listeners.ToArray();
            }
        }

        /// <summary>
        /// 获取一个监听器
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public WcfListener GetListener(ServiceAddress address)
        {
            Contract.Requires(address != null);

            lock (_listeners)
            {
                return _listeners.FirstOrDefault(v => v.Option.Address == address);
            }
        }

        internal void RemoveListener(WcfListener listener)
        {
            lock (_listeners)
            {
                _listeners.Remove(listener);
            }
        }

        /// <summary>
        /// 创建一个连接
        /// </summary>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <param name="duplex">是否支持双向通信</param>
        /// <returns></returns>
        public WcfConnection Connect(ServiceAddress address, CommunicationType type, bool duplex = false)
        {
            Contract.Requires(address != null);

            return Connect(new CommunicationOption(address, type, duplex));
        }

        /// <summary>
        /// 创建一个连接
        /// </summary>
        /// <param name="communicateOption"></param>
        /// <returns></returns>
        public WcfConnection Connect(CommunicationOption communicateOption)
        {
            Contract.Requires(communicateOption != null);

            lock (_thisLock)
            {
                if (_disposed)
                    throw new ObjectDisposedException("WcfService");

                WcfConnection connection = new InitiativeWcfConnection(this, communicateOption);
                lock (_connections)
                {
                    _connections.Add(connection);
                }

                return connection;
            }
        }

        /// <summary>
        /// 创建一个连接
        /// </summary>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <param name="duplex"></param>
        /// <returns></returns>
        public WcfConnection Connect(string address, CommunicationType type, bool duplex)
        {
            Contract.Requires(address != null);

            return Connect(ServiceAddress.Parse(address), type, duplex);
        }

        /// <summary>
        /// 获取所有的连接
        /// </summary>
        /// <returns></returns>
        public WcfConnection[] GetAllConnections()
        {
            lock (_connections)
            {
                return _connections.ToArray();
            }
        }

        /// <summary>
        /// 删除一个连接
        /// </summary>
        /// <param name="connection"></param>
        internal void RemoveConnection(WcfConnection connection)
        {
            lock (_connections)
            {
                _connections.Remove(connection);
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Dispose()
        {
            lock (_thisLock)
            {
                if (_disposed)
                    return;

                _disposed = true;
                lock (_listeners)
                {
                    foreach (WcfListener listener in _listeners.ToArray())
                    {
                        listener.Dispose();
                    }
                }

                lock (_connections)
                {
                    foreach (WcfConnection connection in _connections.ToArray())
                    {
                        connection.Dispose();
                    }
                }
            }
        }
    }
}
