using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Utility;
using System.Threading;
using Common.Package;
using System.Threading.Tasks;
using ServiceFairy.Entities.Addins.MesssageSubscript;

namespace ServiceFairy.Service.Proxy.Components
{
    /// <summary>
    /// 消息分发器
    /// </summary>
    [AppComponent("消息分发器", "分发从消息中心订阅的消息")]
    class MessageDispatcherAppComponent : TimerAppComponentBase
    {
        public MessageDispatcherAppComponent(Service service)
            : base(service)
        {
            _service = service;
            ThreadUtility.StartNew(_DispatchFunc, ThreadPriority.AboveNormal);
        }

        private readonly Service _service;
        private readonly AutoResetEvent _waitForMessage = new AutoResetEvent(false);
        private readonly Queue<MessageGroup> _messages = new Queue<MessageGroup>();
        private readonly object _syncLocker = new object();

        class MessageGroup
        {
            public UserMsgItem[] UserMsgs { get; set; }

            public ServiceEndPoint Caller { get; set; }
        }

        /// <summary>
        /// 分发消息
        /// </summary>
        /// <param name="userMsgs"></param>
        /// <param name="caller"></param>
        public void DispatchMessage(UserMsgItem[] userMsgs, ServiceEndPoint caller)
        {
            if (userMsgs.IsNullOrEmpty())
                return;

            lock (_syncLocker)
            {
                _messages.Enqueue(new MessageGroup() { UserMsgs = userMsgs, Caller = caller });
                _waitForMessage.Set();
            }
        }

        private MessageGroup[] _DequeueAll()
        {
            lock (_syncLocker)
            {
                MessageGroup[] msgs = _messages.DequeueAll();
                _messages.TrimExcess();
                return msgs;
            }
        }

        // 分发线程
        private void _DispatchFunc()
        {
            while (Wait(_waitForMessage))
            {
                MessageGroup[] msgGroups;
                while ((msgGroups = _DequeueAll()).Length > 0)
                {
                    InvokeNoThrow(() => _Dispatch(msgGroups));
                }
            }
        }

        private void _Dispatch(MessageGroup[] msgGroups)
        {
            Dictionary<int, ICommunicate> communicateCache = new Dictionary<int, ICommunicate>();
            Dictionary<Guid, List<long>> msgIdsToRemove = new Dictionary<Guid, List<long>>();

            // 异步发送消息
            Parallel.ForEach<MessageGroup>(msgGroups, delegate(MessageGroup msgGroup) {
                IList<UserMsgItem> succeedMsgs = _TrySendMessageGroup(msgGroup, communicateCache);
                lock (msgIdsToRemove)
                {
                    List<long> list = msgIdsToRemove.GetOrSet(msgGroup.Caller.ClientId);
                    list.AddRange(succeedMsgs.Where(userMsg => !userMsg.Msg.Property.HasFlag(MsgProperty.NotReliable)).Select(msgItem => msgItem.MsgIndex));
                }
            });

            // 从消息中心删除发送成功的消息
            foreach (KeyValuePair<Guid, List<long>> item in msgIdsToRemove)
            {
                Guid clientId = item.Key;
                List<long> msgIds = item.Value;
                if (msgIds.IsNullOrEmpty())
                    continue;

                try
                {
                    _service.Invoker.Message.RemoveMessages(msgIds.ToArray(),
                        CallingSettings.FromTarget(clientId, CommunicateInvokeType.OneWay)
                    );
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            }
        }

        // 尝试发送消息组
        private IList<UserMsgItem> _TrySendMessageGroup(MessageGroup msgGroup, Dictionary<int, ICommunicate> communicateCache)
        {
            List<UserMsgItem> userMsgs = new List<UserMsgItem>();
            foreach (UserMsgItem msg in msgGroup.UserMsgs)
            {
                if (_TrySendMessage(msg, communicateCache))
                    userMsgs.Add(msg);
            }

            return userMsgs;
        }

        // 尝试发送消息
        private bool _TrySendMessage(UserMsgItem userMsg, Dictionary<int, ICommunicate> communicateCache)
        {
            try
            {
                return _SendMessage(userMsg, communicateCache);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return false;
            }
        }

        // 发送消息
        private bool _SendMessage(UserMsgItem userMsg, Dictionary<int, ICommunicate> communicateCache)
        {
            if (userMsg.To.IsNullOrEmpty())
                return true;

            Msg msg = userMsg.Msg;
            foreach (int to in userMsg.To)
            {
                ICommunicate communicate = communicateCache.GetOrSet(to, _service.Context.SessionStateManager.CreateCommunicate);
                if (communicate != null)
                {
                    CommunicateData data = new CommunicateData(msg.Data, msg.Format);
                    communicate.Call(null, msg.Method, data, CallingSettings.OneWay);
                    return true;
                }
            }

            return false;
        }

        protected override void OnExecuteTask(string taskName)
        {
            
        }
    }
}
