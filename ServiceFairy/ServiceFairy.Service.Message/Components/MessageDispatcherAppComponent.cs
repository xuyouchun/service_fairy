using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Message;
using System.Diagnostics.Contracts;
using Common.Utility;
using System.Threading;
using Common.Contracts.Service;
using Common.Package.TaskDispatcher;
using Common.Package;
using ServiceFairy.SystemInvoke;
using Common.Package.Service;
using Common.Contracts;
using ServiceFairy.Entities;
using Common.Collection;

namespace ServiceFairy.Service.Message.Components
{
    /// <summary>
    /// 消息分发器
    /// </summary>
    [AppComponent("消息分发器", "分发传递到各个用户的消息")]
    partial class MessageDispatcherAppComponent : TimerAppComponentBase
    {
        public MessageDispatcherAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(2))
        {
            _service = service;
            ThreadUtility.StartNew(_DealMessageFunc, ThreadPriority.AboveNormal);
        }

        private readonly Service _service;
        private readonly LargeDictionary<int, MessageGroup> _dictOfUser = new LargeDictionary<int, MessageGroup>();
        private readonly LargeDictionary<long, UserMsgWrapper> _dictOfMsgIds = new LargeDictionary<long, UserMsgWrapper>();
        private readonly object _syncLocker = new object();
        private readonly Queue<UserMsg> _queue2 = new Queue<UserMsg>();  // 备用队列

        [ObjectProperty("队列的容量")]
        private const int QUEUE_MAX_COUNT = 10000 * 5;

        [ObjectProperty("备用队列的容量")]
        private const int QUEUE_MAX_COUNT_2 = 10000 * 200;

        #region Class MessageGroup ...

        class MessageGroup
        {
            public Queue<UserMsgWrapper> Queue = new Queue<UserMsgWrapper>();
            public LargeDictionary<long, UserMsgWrapper> Dict = new LargeDictionary<long, UserMsgWrapper>();
        }

        #endregion

        #region Properties ...

        [ObjectProperty("队列中的消息数量")]
        public int MessageCountInQueue
        {
            get { return _dictOfMsgIds.Count; }
        }

        [ObjectProperty("备用队列中的消息数量")]
        public int MessageCountInQueue2
        {
            get { return _queue2.Count; }
        }

        [ObjectProperty("消息所涉及的用户数量")]
        public int UserCount
        {
            get { return _dictOfUser.Count; }
        }

        [ObjectProperty("总共发送的消息数量（不包括重新发送）")]
        public long TotalSendMessageCount
        {
            get { return _sendCountStopwatch.TotalCount; }
        }

        [ObjectProperty("总共接收的消息数量")]
        public long TotalReceivedMessageCount
        {
            get { return _receiveCountStopwatch.TotalCount; }
        }

        private long _faultMessageCount;

        [ObjectProperty("发送失败或过期的消息数量")]
        public long FaultMessageCount
        {
            get { return _faultMessageCount; }
        }

        private long _resendMessageCount;

        [ObjectProperty("重复发送的消息次数")]
        public long ResendMessageCount
        {
            get { return _resendMessageCount; }
        }

        private long _memorySize = 0;

        [ObjectProperty("内存占用")]
        public string MemorySize
        {
            get { return StringUtility.GetSizeString(Interlocked.Read(ref _memorySize)); }
        }

        [ObjectProperty("每秒发送的消息数量(近30秒平均)")]
        public double SendCountPreSeconds
        {
            get
            {
                return Math.Round(_sendCountStopwatch.GetPreSeconds(), 2);
            }
        }

        private readonly CountStopwatch _sendCountStopwatch = CountStopwatch.StartNew(30);
        private readonly CountStopwatch _receiveCountStopwatch = CountStopwatch.StartNew(30);

        [ObjectProperty("每秒接收的消息数量(近30秒平均)")]
        public double ReceiveCountPreSeconds
        {
            get
            {
                return Math.Round(_receiveCountStopwatch.GetPreSeconds(), 2);
            }
        }

        #endregion

        /// <summary>
        /// 批量添加消息
        /// </summary>
        /// <param name="messages"></param>
        public void AddMessages(UserMsg[] messages)
        {
            Contract.Requires(messages != null);

            if (_IsFull())  // 如果超出了容量，则放在备用队列中
                _AddToQueue2(messages);
            else
                _AddToQueue(messages);

            Interlocked.Add(ref _memorySize, messages.Sum(msg => _GetMemorySize(msg.Msg)));
            _receiveCountStopwatch.Increment(messages.Length);
        }

        private static int _GetMemorySize(Msg msg)
        {
            if (msg.Data == null)
                return 0;

            return msg.Data.Length + 40;
        }

        private void _AddToQueue(IList<UserMsg> messages)
        {
            _AddToQueue(messages.ToArray(msgItem => new UserMsgWrapper { UserMsg = msgItem }));
        }

        private void _AddToQueue(IList<UserMsgWrapper> messages, bool reAdd = false)
        {
            HashSet<int> userIds = new HashSet<int>();

            lock (_syncLocker)
            {
                DateTime now = DateTime.UtcNow;
                for (int k = 0; k < messages.Count; k++)
                {
                    UserMsgWrapper w = messages[k];
                    MessageGroup g = _dictOfUser.GetOrSet(w.UserMsg.To);
                    g.Queue.Enqueue(w);

                    if (!reAdd)
                    {
                        g.Dict.Add(w.Index, w);
                        _dictOfMsgIds.Add(w.Index, w);
                    }

                    userIds.Add(w.UserMsg.To);
                }
            }

            _AddRunningTaskUserIds(userIds.ToArray());
        }

        private void _AddRunningTaskUserIds(IList<int> userIds)
        {
            if (userIds.IsNullOrEmpty())
                return;

            lock (_runningTaskUserIds)
            {
                _runningTaskUserIds.AddRange(userIds);
                _waitForMessageEvent.Set();
            }
        }

        private void _AddToQueue2(IList<UserMsg> msgItems)
        {
            if (_queue2.Count + msgItems.Count > QUEUE_MAX_COUNT_2)
                throw new ServiceException(ServerErrorCode.InvalidOperation, "消息队列已满");

            lock (_queue2)
            {
                _queue2.EnqueueRange(msgItems);
                return;
            }
        }

        private bool _IsFull()
        {
            return _dictOfMsgIds.Count > QUEUE_MAX_COUNT || _queue2.Count > 0;
        }

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="msgItem"></param>
        public void AddMessage(UserMsg msgItem, int to)
        {
            if (_IsFull())  // 如果超出了容量，则放在备用队列中
                _AddToQueue2(new[] { msgItem });
            else
                _AddToQueue(msgItem, to);

            Interlocked.Add(ref _memorySize, _GetMemorySize(msgItem.Msg));
            _receiveCountStopwatch.Increment();
        }

        private void _AddToQueue(UserMsg msgItem, int to)
        {
            lock (_syncLocker)
            {
                UserMsgWrapper w = new UserMsgWrapper() { UserMsg = msgItem };
                MessageGroup g = _dictOfUser.GetOrSet(w.UserMsg.To);

                g.Queue.Enqueue(w);
                g.Dict.Add(w.Index, w);
            }

            lock (_runningTaskUserIds)
            {
                _runningTaskUserIds.Add(to);
                _waitForMessageEvent.Set();
            }
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="msgIndex"></param>
        public void RemoveMessage(long msgIndex)
        {
            _AddRemovedMessageIds(new[] { msgIndex });
        }

        private UserMsgWrapper _RemoveMessage(long msgIndex)
        {
            UserMsgWrapper w;
            if (_dictOfMsgIds.TryGetValue(msgIndex, out w))
            {
                MessageGroup g;
                if (_dictOfUser.TryGetValue(w.UserMsg.To, out g))
                    g.Dict.Remove(msgIndex, true);

                _dictOfMsgIds.Remove(msgIndex);
                Interlocked.Add(ref _memorySize, -_GetMemorySize(w.UserMsg.Msg));
            }

            return w;
        }

        /// <summary>
        /// 批量删除消息
        /// </summary>
        /// <param name="msgIndexes">消息索引号</param>
        public void RemoveMessages(IList<long> msgIndexes)
        {
            Contract.Requires(msgIndexes != null);

            _AddRemovedMessageIds(msgIndexes);
        }

        private readonly List<long> _removedMsgIndexes = new List<long>();

        /// <summary>
        /// 删除过期的消息
        /// </summary>
        /// <param name="ts"></param>
        private UserMsgWrapper[] _GetExpiredMessages(TimeSpan ts)
        {
            DateTime time = DateTime.UtcNow - ts;
            UserMsgWrapper[] ws = _dictOfMsgIds
                .Where(item => item.Value.State == MsgItemState.Sending && item.Value.LastSendTime < time)
                .OrderByDescending(item => item.Value.Index)
                .Select(item => item.Value).ToArray();

            return ws;
        }

        /// <summary>
        /// 获取指定用户的消息
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="remove"></param>
        /// <returns></returns>
        public Msg[] GetMessages(int[] userIds, bool remove = true)
        {
            lock (_syncLocker)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取指定用户的消息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="remove">是否将其删除</param>
        /// <returns></returns>
        public Msg[] GetMessages(int userId, bool remove = true)
        {
            return GetMessages(new[] { userId }, remove);
        }

        private void _AddRemovedMessageIds(IList<long> msgIndexes)
        {
            lock (_removedMsgIndexes)
            {
                _removedMsgIndexes.AddRange(msgIndexes);
                _waitForMessageEvent.Set();
            }
        }

        private DateTime _lastCheckExpired;

        protected override void OnExecuteTask(string taskName)
        {
            // 过期检查
            if((DateTime.UtcNow - _lastCheckExpired > TimeSpan.FromSeconds(10)))
            {
                List<long> faultMsgIndexes = new List<long>();
                List<UserMsgWrapper> resendMessageItems = new List<UserMsgWrapper>();
                lock (_syncLocker)
                {
                    UserMsgWrapper[] ws = _GetExpiredMessages(TimeSpan.FromSeconds(30));  // 超过30秒将重复发送
                    DateTime realExpiredTime = DateTime.UtcNow - TimeSpan.FromSeconds(120);  // 超过120秒，则强制删除

                    for (int k = 0; k < ws.Length; k++)
                    {
                        UserMsgWrapper w = ws[k];
                        Msg msg = w.UserMsg.Msg;

                        if (w.TryTimes >= 3 || w.LastSendTime < realExpiredTime || msg.Property.HasFlag(MsgProperty.NotReliable))
                        {
                            faultMsgIndexes.Add(w.Index);
                        }
                        else
                        {
                            w.State = MsgItemState.Wait;
                            resendMessageItems.Add(w);
                        }
                    }

                    if (resendMessageItems.Count > 0)
                    {
                        Interlocked.Add(ref _resendMessageCount, resendMessageItems.Count);
                        _AddToQueue(resendMessageItems, true);
                        LogManager.LogWarning(string.Format("重新发送{0}个消息", resendMessageItems.Count), "");
                    }

                    _dictOfUser.RemoveWhere(item => item.Value.Dict.Count == 0 && item.Value.Queue.Count == 0);
                    _dictOfUser.TrimExcess();
                    _lastCheckExpired = DateTime.UtcNow;

                    _AddRunningTaskUserIds(_dictOfUser.Keys.ToArray());
                }

                if (faultMsgIndexes.Count > 0)
                {
                    Interlocked.Add(ref _faultMessageCount, faultMsgIndexes.Count);
                    RemoveMessages(faultMsgIndexes);
                    LogManager.LogWarning(string.Format("强制删除{0}个过期或发送失败的消息", faultMsgIndexes.Count), "");
                }
            }

            // 从备用队列中添加消息
            if (_queue2.Count > 0 && _dictOfMsgIds.Count < QUEUE_MAX_COUNT / 3 * 2)
            {
                int max = Math.Min(QUEUE_MAX_COUNT - _dictOfMsgIds.Count, _queue2.Count);
                lock (_queue2)
                {
                    UserMsg[] msgs = _queue2.DequeueRange(max);
                    _queue2.TrimExcess();
                    _AddToQueue(msgs);
                }
            }
        }
    }
}
