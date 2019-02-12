using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Framework.TrayPlatform;
using Common.Utility;
using ServiceFairy.SystemInvoke;
using Common.Package;
using System.Threading;
using ServiceFairy.Entities.UserCenter;
using System.Runtime.CompilerServices;
using Common;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 用户信息收集器
    /// </summary>
    [AppComponent("用户在线信息收集器", "收集用户的在线信息")]
    public class UserOnlineInfoCollectorAppComponent : AppComponent
    {
        public UserOnlineInfoCollectorAppComponent(SystemAppServiceBase service)
            : base(service)
        {
            _service = service;
            _invoker = _service.Invoker;
            _waitForLoadCompletedEventArray = new WaitHandle[] { _waitForLoadCompletedEvent, _waitForExitEvent };
            _waitForLoadEventArray = new WaitHandle[] { _waitForLoadEvent, _waitForExitEvent };

            _loadThread = ThreadUtility.StartNew(_LoadFunc, ThreadPriority.Highest);
        }

        private readonly SystemAppServiceBase _service;
        private readonly Cache<int, ClientInfo> _cache = new Cache<int, ClientInfo>();
        private readonly AutoResetEvent _waitForLoadCompletedEvent = new AutoResetEvent(false);
        private readonly HashSet<int> _waitForLoadCollection = new HashSet<int>();
        private readonly Thread _loadThread;
        private readonly object _syncLocker = new object();
        private readonly AutoResetEvent _waitForLoadEvent = new AutoResetEvent(false);
        private readonly ManualResetEvent _waitForExitEvent = new ManualResetEvent(false);
        private readonly SystemInvoker _invoker;
        private volatile bool _disposed = false;

        #region Class ClientInfo ...

        class ClientInfo
        {
            private ClientInfo(Guid clientId)
            {
                ClientID = clientId;
            }

            public readonly Guid ClientID;

            private static readonly Dictionary<Guid, ClientInfo> _dict = new Dictionary<Guid, ClientInfo>();

            [MethodImpl(MethodImplOptions.Synchronized)]
            public static ClientInfo Create(Guid clientId)
            {
                return _dict.GetOrSet(clientId, (key) => new ClientInfo(key));
            }

            public static readonly ClientInfo Empty = new ClientInfo(Guid.Empty);
        }

        #endregion

        public Guid GetClientID(int userId)
        {
            ClientInfo info;

            int times = 0;
            while ((info = _cache.Get(userId)) == null)
            {
                if (++times > 1)
                    return Guid.Empty;

                lock (_syncLocker)
                {
                    _waitForLoadCollection.Add(userId);
                }

                _waitForLoadEvent.Set();
                if (WaitHandle.WaitAny(_waitForLoadCompletedEventArray) == 1)
                    break;
            }

            return info == null ? Guid.Empty : info.ClientID;
        }

        private readonly WaitHandle[] _waitForLoadCompletedEventArray;

        private void _LoadFunc()
        {
            while (!_disposed)
            {
                if (WaitHandle.WaitAny(_waitForLoadEventArray) == 1)
                    break;

                int[] userIds;
                lock (_syncLocker)
                {
                    userIds = _waitForLoadCollection.ToArray();
                    _waitForLoadCollection.Clear();
                }

                HashSet<int> _loadedIds = new HashSet<int>();
                try
                {
                    UserPosition[] positions = _invoker.UserCenter.GetUserPositions(userIds);
                    foreach (UserPosition pos in positions)
                    {
                        ClientInfo clientInfo = ClientInfo.Create(pos.ClientId);
                        foreach (int userId in pos.UserIds ?? Array<int>.Empty)
                        {
                            _cache.AddOfRelative(userId, clientInfo, _cacheTimeout);
                            _loadedIds.Add(userId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }

                foreach (int userId in userIds.Except(_loadedIds))
                {
                    _cache.AddOfRelative(userId, ClientInfo.Empty, _cacheTimeout);
                }

                _waitForLoadCompletedEvent.Set();
            }
        }

        private readonly WaitHandle[] _waitForLoadEventArray;
        private static readonly TimeSpan _cacheTimeout = TimeSpan.FromSeconds(5);

        protected override void OnDispose()
        {
            base.OnDispose();

            _disposed = true;
            _waitForExitEvent.Set();
            _cache.Dispose();
        }
    }
}
