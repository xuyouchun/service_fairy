using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf.Service;
using System.Threading;
using Common.Package.Cache;
using Common.Package;
using System.Xml;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;

namespace Common.Communication.Wcf
{
    /// <summary>
    /// 会话状态管理器
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    class SessionStateManager : IDisposable
    {
        public SessionStateManager(int max = 10000, TimeSpan timeout = default(TimeSpan))
        {
            _storageStrategy = new FixedSizeMemoryStorageStrategy<UniqueId, SessionState>(max);
            _sessionStateCache = new Cache<UniqueId, SessionState>(_storageStrategy);

            _storageStrategy.Expired += new EventHandler<MemoryStorageExpiredEventArgs<UniqueId, SessionState>>(_storageStrategy_Expired);
            _storageStrategy.Full += new EventHandler<FixedSizeMemoryStorageStrategyFullEventArgs<UniqueId, SessionState>>(_storageStrategy_Full);

            _timeout = (timeout == default(TimeSpan)) ? _defaultTimeout : timeout;
        }

        private readonly FixedSizeMemoryStorageStrategy<UniqueId, SessionState> _storageStrategy;
        private readonly Cache<UniqueId, SessionState> _sessionStateCache;

        private readonly TimeSpan _defaultTimeout =
#if DEBUG
            TimeSpan.FromMinutes(10);
#else
        TimeSpan.FromSeconds(30);
#endif

        void _storageStrategy_Full(object sender, FixedSizeMemoryStorageStrategyFullEventArgs<UniqueId, SessionState> e)
        {
            _SetReplySessionStateExpired(e.Items);
        }

        void _storageStrategy_Expired(object sender, MemoryStorageExpiredEventArgs<UniqueId, SessionState> e)
        {
            _SetReplySessionStateExpired(e.Items);
        }

        private void _SetReplySessionStateExpired(IEnumerable<CacheItem<UniqueId, SessionState>> items)
        {
            foreach (CacheItem<UniqueId, SessionState> item in items)
            {
                item.Value.SetTimeout();
            }
        }

        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(30);

        /// <summary>
        /// 创建SessionState
        /// </summary>
        /// <param name="requestMsg"></param>
        /// <returns></returns>
        public SessionState Create(EntityMessage requestMsg)
        {
            UniqueId messageId = requestMsg.Headers.MessageId ?? (requestMsg.Headers.MessageId = new UniqueId(Guid.NewGuid()));
            SessionState sessionState = new SessionState(this, requestMsg, _timeout);
            _sessionStateCache.AddOfRelative(messageId, sessionState, _timeout);

            return sessionState;
        }

        /// <summary>
        /// 删除SessionState
        /// </summary>
        /// <param name="uniqueId"></param>
        internal void Remove(UniqueId uniqueId)
        {
            _sessionStateCache.Remove(uniqueId);
        }

        /// <summary>
        /// 接收到应答
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ApplyResponse(EntityMessage message)
        {
            UniqueId relatesTo = message.Headers.RelatesTo;
            if (relatesTo == null)
                return false;

            SessionState sessionState = _sessionStateCache.Get(relatesTo);
            if (sessionState != null)
            {
                _sessionStateCache.Remove(relatesTo);
                sessionState.SetReplyMessage(message);
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            _sessionStateCache.Dispose();
        }
    }

    #region Class SessionState ...

    /// <summary>
    /// 会话状态
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    class SessionState : IComparable<SessionState>, IDisposable
    {
        internal SessionState(SessionStateManager owner, EntityMessage reqMsg, TimeSpan timeout)
        {
            _owner = owner;
            _timeout = timeout;
            RequestMessage = reqMsg;
        }

        private readonly ManualResetEvent _event = new ManualResetEvent(false);
        private readonly TimeSpan _timeout;
        private readonly SessionStateManager _owner;

        public bool Wait()
        {
            return _event.WaitOne(_timeout);
        }

        /// <summary>
        /// 请求消息
        /// </summary>
        public EntityMessage RequestMessage { get; private set; }

        /// <summary>
        /// 应答消息
        /// </summary>
        public EntityMessage ReplyMessage { get; private set; }

        public bool TimeExpired { get; private set; }

        /// <summary>
        /// 接收到应答消息
        /// </summary>
        /// <param name="message"></param>
        public void SetReplyMessage(EntityMessage message)
        {
            ReplyMessage = message;
            _event.Set();
        }

        public void SetTimeout()
        {
            TimeExpired = true;
            _event.Set();
        }

        private readonly long _msgIndex = Interlocked.Increment(ref _staticMsgIndex);
        private static long _staticMsgIndex = 0;

        public int CompareTo(SessionState other)
        {
            return _msgIndex.CompareTo(other._msgIndex);
        }

        /// <summary>
        /// 等待应答消息，如果超时会抛出异常
        /// </summary>
        /// <returns></returns>
        public EntityMessage WaitWithException()
        {
            if (!Wait() || TimeExpired)
                throw new ServiceException(ClientErrorCode.Timeout, "在指定时间之内未收到应答");

            return ReplyMessage;
        }

        public void Dispose()
        {
            _owner.Remove(RequestMessage.Headers.MessageId);
        }
    }

    #endregion
}
