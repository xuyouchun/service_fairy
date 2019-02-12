using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Framework.TrayPlatform;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Package;
using ServiceFairy.Entities;
using ServiceFairy.Entities.User;
using ServiceFairy.DbEntities.User;
using Common.Data.UnionTable;
using ServiceFairy.Entities.UserCenter;
using Common.Data.SqlExpressions;
using ServiceFairy.Entities.MessageCenter;
using ServiceFairy.Entities.Message;

namespace ServiceFairy.Service.UserCenter.Components
{
    /// <summary>
    /// 用户在线状态管理器
    /// </summary>
    [AppComponent("用户在线状态管理器", "记录用户的在线状态")]
    class UserConnectionManagerAppComponent : TimerAppComponentBase
    {
        public UserConnectionManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
        }

        private readonly Service _service;

        protected override void OnExecuteTask(string taskName)
        {
            _CheckExpired();
        }

        private readonly Dictionary<int, Wrapper> _dictOfUserId = new Dictionary<int, Wrapper>();
        private readonly Dictionary<Guid, HashSet<Wrapper>> _dictOfClientId = new Dictionary<Guid, HashSet<Wrapper>>();
        private readonly object _syncLocker = new object();

        #region Class Wrapper ...

        class Wrapper
        {
            public Wrapper(UserConnectionInfo conInfo)
            {
                ConnectionInfo = conInfo;
                UserId = conInfo.UserId;
                LastKeepTime = QuickTime.UtcNow;
            }

            public UserConnectionInfo ConnectionInfo;
            public DateTime LastKeepTime;
            public int UserId;

            public override bool Equals(object obj)
            {
                Wrapper info = (Wrapper)obj;
                return UserId == info.UserId;
            }

            public override int GetHashCode()
            {
                return UserId;
            }

            public void CopyTo(Wrapper w)
            {
                Contract.Requires(w != null);

                w.ConnectionInfo = ConnectionInfo;
                w.UserId = UserId;
            }
        }

        #endregion

        /// <summary>
        /// 保持用户的连接状态
        /// </summary>
        /// <param name="conInfos"></param>
        public void KeepUserConnection(UserConnectionInfo[] conInfos)
        {
            if (conInfos.IsNullOrEmpty())
                return;

            List<UserConnectionInfo> connectedUsers = new List<UserConnectionInfo>();

            lock (_syncLocker)
            {
                Wrapper[] ws = conInfos.ToArray(conInfo => new Wrapper(conInfo));

                DateTime now = DateTime.UtcNow;
                for (int k = 0; k < ws.Length; k++)
                {
                    Wrapper w = ws[k], w0;
                    if (_dictOfUserId.TryGetValue(w.UserId, out w0))
                    {
                        if (w0.ConnectionInfo.LastAccessTime <= w.ConnectionInfo.LastAccessTime)
                        {
                            w.CopyTo(w0);
                            w0.LastKeepTime = now;
                        }
                    }
                    else
                    {
                        _dictOfUserId[w.UserId] = w;
                        w.LastKeepTime = now;
                        _dictOfClientId.GetOrSet(w.ConnectionInfo.ClientId).Add(w);

                        connectedUsers.Add(w.ConnectionInfo);
                    }
                }
            }

            if (connectedUsers.Count > 0)
                _OnUserConnected(connectedUsers.ToArray());
        }

        /// <summary>
        /// 用户连接断开通知
        /// </summary>
        /// <param name="conInfos">连接信息</param>
        public void UserDisconnectedNotify(UserDisconnectedInfo[] conInfos)
        {
            lock (_syncLocker)
            {
                List<UserConnectionInfo> disConInfos = new List<UserConnectionInfo>();
                foreach (UserDisconnectedInfo conInfo in conInfos)
                {
                    Wrapper w;
                    if (_dictOfUserId.TryGetValue(conInfo.UserId, out w) && conInfo.ClientId == w.ConnectionInfo.ClientId)
                    {
                        _dictOfUserId.Remove(conInfo.UserId);
                        HashSet<Wrapper> hs;
                        if (_dictOfClientId.TryGetValue(conInfo.ClientId, out hs))
                        {
                            hs.Remove(w);
                        }

                        disConInfos.Add(w.ConnectionInfo);
                    }
                }

                if (disConInfos.Count > 0)
                    _OnUserDisconnected(disConInfos.ToArray());
            }
        }

        /// <summary>
        /// 获取用户的连接状态
        /// </summary>
        /// <param name="userIds">用户ID</param>
        /// <returns></returns>
        public UserConnectionInfo[] GetUserConnectionInfos(int[] userIds)
        {
            lock (_syncLocker)
            {
                return userIds.ToArrayNotNull(_GetUserConnectionInfos);
            }
        }

        /// <summary>
        /// 获取用户的连接状态
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public UserConnectionInfo GetUserConnectionInfo(int userId)
        {
            lock (_syncLocker)
            {
                return _GetUserConnectionInfos(userId);
            }
        }

        private UserConnectionInfo _GetUserConnectionInfos(int userId)
        {
            Wrapper w;
            if (_dictOfUserId.TryGetValue(userId, out w))
                return w.ConnectionInfo;

            return null;
        }

        /// <summary>
        /// 过期检查
        /// </summary>
        private void _CheckExpired()
        {
            List<UserConnectionInfo> disconnectedUsers = new List<UserConnectionInfo>();

            lock (_syncLocker)
            {
                DateTime time = DateTime.UtcNow - TimeSpan.FromSeconds(30);
                foreach (var item in _dictOfClientId)
                {
                    HashSet<Wrapper> hs = item.Value;
                    Wrapper[] ws = hs.RemoveWhereWithReturn(w => w.LastKeepTime < time);

                    if (!ws.IsNullOrEmpty())
                    {
                        disconnectedUsers.AddRange(ws.Select(w => w.ConnectionInfo));
                        _dictOfUserId.RemoveRange(ws.Select(w => w.UserId));
                    }
                }

                _dictOfClientId.RemoveWhere(item => item.Value.Count == 0);
            }

            _RemoveOnline(disconnectedUsers);
            if (disconnectedUsers.Count > 0)
                _OnUserDisconnected(disconnectedUsers.ToArray());
        }

        private void _RemoveOnline(List<UserConnectionInfo> list)
        {
            HashSet<int> onlineUsers = _service.RoutableUserConnectionManager.SelectOnlineUsers(list.ToArray(item => item.UserId)).ToHashSet();
            if (onlineUsers.Count > 0)
            {
                list.RemoveWhere(item => onlineUsers.Contains(item.UserId));
            }
        }

        /// <summary>
        /// 获取所有的在线用户
        /// </summary>
        /// <returns></returns>
        public int[] GetAllOnlineUsers()
        {
            lock (_syncLocker)
            {
                return _dictOfUserId.Keys.ToArray();
            }
        }

        private Dictionary<int, UserBasicInfo> _GetUserBasicInfos(UserConnectionInfo[] conInfos)
        {
            return _service.UserInfoManager.GetUserBasicInfos(conInfos.ToArray(ci => ci.UserId)).ToDictionary(v => v.UserId);
        }

        /*
        // 发送连接状态变化的消息给我的粉丝
        private void _SendOnlineStatusMessage(UserConnectionInfo[] connectionInfos, bool online, DateTime statusChangedTime = default(DateTime))
        {
            Dictionary<int, UserStatusInfo> statusDict = _service.UserStatusManager.GetUserStatus(
                connectionInfos.ToArray(ci => ci.UserId)).ToDictionary(st => st.UserId, true);

            Dictionary<int, UserBasicInfo> basicInfos = _GetUserBasicInfos(connectionInfos);
            foreach (UserConnectionInfo conInfo in connectionInfos)
            {
                try
                {
                    UserStatusInfo status = statusDict.GetOrDefault(conInfo.UserId);
                    UserBasicInfo basicInfo = basicInfos.GetOrDefault(conInfo.UserId);

                    var entity = User_StatusChanged_Message.Create(conInfo.UserId);
                    _service.MessageSender.SendToFollowers<User_StatusChanged_Message>(entity, conInfo.UserId, property: MsgProperty.Override);
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            }
        }*/

        // 提取出持久化的消息
        private void _SendMessagesFromCenter(UserConnectionInfo[] conInfos)
        {
            int[] userIds = conInfos.ToArray(cInfo => cInfo.UserId);
            foreach (UserConnectionInfo conInfo in conInfos)
            {
                try
                {
                    Msg[] msgs = _service.Invoker.MessageCenter.Read(conInfo.UserId);
                    if (!msgs.IsNullOrEmpty())
                    {
                        Array.Sort(msgs, (x, y) => x.Time.CompareTo(y.Time));
                        _service.MessageSender.SendArray(new ServiceFairy.Entities.Message.UserMsgArray {
                            ToUsers = Users.FromUserId(conInfo.UserId), Msgs = msgs,
                        });
                    }
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            }
        }

        private void _OnUserConnected(UserConnectionInfo[] connectionInfos)
        {
            int[] userIds = connectionInfos.ToArray(ci => ci.UserId);

            // 发送状态变化消息给粉丝
            //InvokeNoThrow(() => _SendOnlineStatusMessage(connectionInfos, true));

            // 从消息池中提取出持久化的消息
            InvokeNoThrow(() => _SendMessagesFromCenter(connectionInfos));

            // 用户上线的事件
            _service.ServiceEvent.Raise(new UserCenter_Online_Event { UserIds = userIds });
            _service.ServiceEvent.Raise(new User_StatusChanged_Event { UserIds = userIds });

            var eh = Connected;
            if (eh != null)
                eh(this, new UserConnectedEventArgs(connectionInfos));
        }

        private void _OnUserDisconnected(UserConnectionInfo[] connectionInfos)
        {
            // 发送消息
            int[] userIds = connectionInfos.ToArray(ci => ci.UserId);
            //InvokeNoThrow(() => _SendOnlineStatusMessage(connectionInfos, false));

            // 用户离线的事件
            _service.ServiceEvent.Raise(new UserCenter_Offline_Event { UserIds = userIds });
            _service.ServiceEvent.Raise(new User_StatusChanged_Event { UserIds = userIds });

            var eh = Disconnected;
            if (eh != null)
                eh(this, new UserDisconnectedEventArgs(connectionInfos));
        }

        /// <summary>
        /// 用户连接事件
        /// </summary>
        public event EventHandler<UserConnectedEventArgs> Connected;

        /// <summary>
        /// 用户断开连接事件
        /// </summary>
        public event EventHandler<UserDisconnectedEventArgs> Disconnected;
    }

    /// <summary>
    /// 用户连接事件
    /// </summary>
    class UserConnectedEventArgs : EventArgs
    {
        public UserConnectedEventArgs(UserConnectionInfo[] connectionInfos)
        {
            ConnectionInfos = connectionInfos;
        }

        /// <summary>
        /// 连接信息
        /// </summary>
        public UserConnectionInfo[] ConnectionInfos { get; private set; }
    }

    /// <summary>
    /// 用户断开连接事件
    /// </summary>
    class UserDisconnectedEventArgs : EventArgs
    {
        public UserDisconnectedEventArgs(UserConnectionInfo[] connectionInfos)
        {
            ConnectionInfos = connectionInfos;
        }

        /// <summary>
        /// 连接信息
        /// </summary>
        public UserConnectionInfo[] ConnectionInfos { get; private set; }
    }
}
