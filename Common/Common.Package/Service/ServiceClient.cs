using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Contracts;
using Common.Utility;
using System.Runtime.Serialization;
using System.Collections.Concurrent;
using System.Reflection;

namespace Common.Package.Service
{
    /// <summary>
    /// 封装对服务的客户端调用
    /// </summary>
    public class ServiceClient : IServiceClient, IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="communicate"></param>
        /// <param name="context"></param>
        /// <param name="dataFormat"></param>
        /// <param name="target"></param>
        /// <param name="disposeIt"></param>
        public ServiceClient(ICommunicate communicate, DataFormat dataFormat = DataFormat.Unknown, bool disposeIt = false)
        {
            Contract.Requires(communicate != null);

            Communicate = communicate;
            DataFormat = dataFormat;

            _disposeIt = disposeIt;

            IConnection con = communicate as IConnection;
            if (con != null)
            {
                con.Received += new ConnectionDataReceivedEventHandler(con_Received);
            }
        }

        private readonly bool _disposeIt;
        private readonly ConcurrentDictionary<string, HashSet<ReceiverItem>> _receivers = new ConcurrentDictionary<string, HashSet<ReceiverItem>>();

        /// <summary>
        /// 通信方式
        /// </summary>
        public ICommunicate Communicate { get; private set; }

        /// <summary>
        /// 默认的数据编码方式
        /// </summary>
        public DataFormat DataFormat { get; set; }

        #region Class ReceiverItem ...

        class ReceiverItem
        {
            public ReceiverItem(string method, Type entityType, IServiceClientReceiver receiver)
            {
                Method = method;
                EntityType = entityType;
                Receiver = receiver;
            }

            public readonly Type EntityType;
            public readonly IServiceClientReceiver Receiver;
            public readonly string Method;
        }

        #endregion

        /// <summary>
        /// 调用服务
        /// </summary>
        /// <typeparam name="replyType">输出参数类型</typeparam>
        /// <param name="method">调用方法</param>
        /// <param name="input">输入参数</param>
        /// <param name="communicateData">通信数据</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public ServiceResult<object> Call(string method, object input, Type replyType, CallingSettings settings = null)
        {
            Contract.Requires(method != null && replyType != null);

            CommunicateData d = Communicate.Call(null, method, DataBufferParser.Serialize(input, DataFormat), settings);

            if (d == null)
                return null;

            return new ServiceResult<object>(DataBufferParser.Deserialize(d, replyType), d.StatusCode, d.StatusDesc, d.Sid);
        }

        /// <summary>
        /// 调用服务
        /// </summary>
        /// <param name="method">调用方法</param>
        /// <param name="input">输入参数</param>
        /// <param name="communicateData">通信数据</param>
        /// <param name="settings">调用设置</param>
        public ServiceResult Call(string method, object input, CallingSettings settings = null)
        {
            Contract.Requires(method != null);

            CommunicateData d = Communicate.Call(null, method, DataBufferParser.Serialize(input, DataFormat), settings);

            if (d == null)
                return null;

            return new ServiceResult(d.StatusCode, d.StatusDesc, d.Sid);
        }

        /// <summary>
        /// 注册接收器
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="entityType">实体类型</param>
        /// <param name="receiver">接收器</param>
        public IServiceClientReceiverHandler RegisterReceiver(string method, Type entityType, IServiceClientReceiver receiver)
        {
            Contract.Requires(method != null && entityType != null && receiver != null);

            method = method ?? string.Empty;
            HashSet<ReceiverItem> hs = _receivers.GetOrAdd(method, (key) => new HashSet<ReceiverItem>());
            lock (hs)
            {
                ReceiverItem receiverItem = new ReceiverItem(method, entityType, receiver);
                hs.Add(receiverItem);
                return new ServiceClientReceiverHandler(this, receiverItem);
            }
        }

        #region Class ServiceClientReceiverHandler ...

        class ServiceClientReceiverHandler : IServiceClientReceiverHandler
        {
            public ServiceClientReceiverHandler(ServiceClient owner, ReceiverItem receiverItem)
            {
                _owner = owner;
                _receiverItem = receiverItem;
            }

            private readonly ServiceClient _owner;
            private readonly HashSet<ReceiverItem> _hs;
            private readonly ReceiverItem _receiverItem;

            public void Unregister()
            {
                HashSet<ReceiverItem> hs;
                if (_owner._receivers.TryGetValue(_receiverItem.Method, out hs))
                {
                    lock (hs)
                    {
                        hs.Remove(_receiverItem);
                    }
                }
            }

            public void Dispose()
            {
                Unregister();
            }
        }

        #endregion

        // 接收到数据
        private void con_Received(object sender, ConnectionDataReceivedEventArgs e)
        {
            _RaiseReceivers(null, e);
            if (!string.IsNullOrEmpty(e.Method))
                _RaiseReceivers(e.Method, e);
        }

        private void _RaiseReceivers(string method, ConnectionDataReceivedEventArgs e)
        {
            method = method ?? string.Empty;
            HashSet<ReceiverItem> hs;
            if (!_receivers.TryGetValue(method, out hs) || hs.Count == 0)
                return;

            lock (hs)
            {
                CommunicateData data = e.RequestData;
                foreach (ReceiverItem item in hs)
                {
                    object entity = (item.EntityType == typeof(CommunicateData))? data
                        : DataBufferParser.Deserialize(data, item.EntityType);

                    item.Receiver.OnReceive(new ServiceClientReceiveEventArgs<object>(method, entity));
                }
            }
        }

        public virtual void Dispose()
        {
            IConnection con = Communicate as IConnection;
            if (con != null)
            {
                con.Received -= new ConnectionDataReceivedEventHandler(con_Received);
            }

            if (_disposeIt)
                Communicate.Dispose();
        }
    }
}
