using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf;
using Common.Contracts.Service;
using Common.Utility;
using Common.Package;
using System.Collections.Concurrent;

namespace Common.Framework.TrayPlatform
{
    partial class WcfTrayAppServiceApplication
    {
        /// <summary>
        /// 用户连接管理器
        /// </summary>
        class UserConnectionManager : IDisposable
        {
            public UserConnectionManager(Guid clientId)
            {
                _clientId = clientId;
                _timeHandle = GlobalTimer<ITask>.Default.Add(TimeSpan.FromSeconds(5), new TaskFuncAdapter(_CheckExpired), false);
            }

            class Wrapper
            {
                public IConnection Connection;
                public UserSessionState SessionState;
                public UserConnectionInfo ConnectionInfo;
            }

            private readonly Guid _clientId;
            private readonly Dictionary<int, Wrapper> _dictOfUserId = new Dictionary<int, Wrapper>();
            private readonly Dictionary<Sid, Wrapper> _dictOfSid = new Dictionary<Sid, Wrapper>();
            //private readonly Dictionary<string, Wrapper> _dictOfUserName = new Dictionary<string, Wrapper>();
            private readonly Dictionary<IConnection, Wrapper> _dictOfConnection = new Dictionary<IConnection, Wrapper>();
            private readonly Dictionary<int, UserDisconnectedInfo> _disconnectedUsers = new Dictionary<int, UserDisconnectedInfo>();
            private readonly IGlobalTimerTaskHandle _timeHandle;
            private readonly object _syncLocker = new object();
            private int _maxOnlineUserCount = 0;
            private bool _hasDowngrade = false;
            private DateTime _maxOnlineUserCountTime = DateTime.UtcNow;

            /// <summary>
            /// 添加一个用户连接
            /// </summary>
            /// <param name="sessionState"></param>
            /// <param name="connection"></param>
            public void Add(UserSessionState sessionState, IConnection connection)
            {
                lock (_syncLocker)
                {
                    Wrapper w;
                    int userId = sessionState.BasicInfo.UserId;
                    if (!_dictOfUserId.TryGetValue(userId, out w))
                    {
                        w = new Wrapper {
                            Connection = connection, SessionState = sessionState,
                            ConnectionInfo = new UserConnectionInfo { UserId = userId, ClientId = _clientId, LastAccessTime = QuickTime.UtcNow }
                        };

                        _dictOfSid[sessionState.Sid] = w;
                        _dictOfUserId[sessionState.BasicInfo.UserId] = w;
                        _dictOfConnection[connection] = w;

                        _OnUserConnected(userId);
                    }
                    else
                    {
                        w.Connection = connection;
                        w.ConnectionInfo.LastAccessTime = QuickTime.UtcNow;
                        w.SessionState = sessionState;
                        _dictOfConnection[connection] = w;
                    }

                    _disconnectedUsers.Remove(userId);

                    // 记录最大在线用户数
                    int count = _dictOfUserId.Count;
                    if (count > _maxOnlineUserCount || (count == _maxOnlineUserCount && _hasDowngrade))
                    {
                        _maxOnlineUserCount = count;
                        _maxOnlineUserCountTime = DateTime.UtcNow;
                        _hasDowngrade = false;
                    }
                    else if (count < _maxOnlineUserCount)
                    {
                        _hasDowngrade = true;
                    }
                }
            }

            private void _OnUserConnected(int userId)
            {
                
            }

            private void _Remove(Wrapper w)
            {
                UserSessionState uss = w.SessionState;
                int userId = uss.BasicInfo.UserId;

                _dictOfSid.Remove(uss.Sid);
                _dictOfUserId.Remove(userId);
                _dictOfConnection.Remove(w.Connection);
                _disconnectedUsers[userId] = new UserDisconnectedInfo() {
                    UserId = userId, ClientId = w.ConnectionInfo.ClientId, DisconnectedTime = DateTime.UtcNow
                };
            }

            /// <summary>
            /// 删除一个用户连接
            /// </summary>
            /// <param name="sid"></param>
            public void RemoveBySid(Sid sid)
            {
                lock (_syncLocker)
                {
                    Wrapper w;
                    if (_dictOfSid.TryGetValue(sid, out w))
                    {
                        _Remove(w);
                    }
                }
            }

            /// <summary>
            /// 删除一个用户连接
            /// </summary>
            /// <param name="userId"></param>
            public void RemoveByUserId(int userId)
            {
                lock (_syncLocker)
                {
                    Wrapper w;
                    if (_dictOfUserId.TryGetValue(userId, out w))
                    {
                        _Remove(w);
                    }
                }
            }

            /// <summary>
            /// 通过WCF连接删除用户连接
            /// </summary>
            /// <param name="connection"></param>
            public void RemoveByConnection(IConnection connection)
            {
                lock (_syncLocker)
                {
                    Wrapper w;
                    if (_dictOfConnection.TryGetValue(connection, out w) && connection == w.Connection)
                    {
                        _Remove(w);
                    }
                }
            }

            /// <summary>
            /// 获取全部
            /// </summary>
            /// <returns></returns>
            public UserSessionState[] GetAllUserSessionStates()
            {
                lock (_syncLocker)
                {
                    return _dictOfSid.Values.ToArray(v => v.SessionState);
                }
            }

            /// <summary>
            /// 获取用户的连接信息
            /// </summary>
            /// <returns></returns>
            public UserConnectionInfo[] GetAllUserConnectionInfos()
            {
                lock (_syncLocker)
                {
                    return _dictOfSid.Values.ToArray(v => v.ConnectionInfo);
                }
            }

            /// <summary>
            /// 根据上次访问时间获取用户的连接信息
            /// </summary>
            /// <param name="from"></param>
            /// <param name="to"></param>
            /// <returns></returns>
            public UserConnectionInfo[] GetConnectionUsersByLastAccessTime(DateTime from, DateTime to)
            {
                lock (_syncLocker)
                {
                    return _dictOfSid.Values.Where(
                        v => v.ConnectionInfo.LastAccessTime >= from && v.ConnectionInfo.LastAccessTime <= to)
                        .ToArray(v => v.ConnectionInfo
                    );
                }
            }

            /// <summary>
            /// 获取最近断开连接的用户
            /// </summary>
            /// <returns></returns>
            public UserDisconnectedInfo[] GetDisconnectedInfos()
            {
                lock (_syncLocker)
                {
                    UserDisconnectedInfo[] infos = _disconnectedUsers.Values.ToArray();
                    _disconnectedUsers.Clear();

                    return infos;
                }
            }

            /// <summary>
            /// 已连接的用户数量
            /// </summary>
            public int Count
            {
                get
                {
                    return _dictOfUserId.Count;
                }
            }

            /// <summary>
            /// 获取指定ID的用户连接
            /// </summary>
            /// <param name="userId"></param>
            /// <returns></returns>
            internal IConnection GetUserConnection(int userId)
            {
                lock (_syncLocker)
                {
                    Wrapper w;
                    if (_dictOfUserId.TryGetValue(userId, out w))
                        return w.Connection;

                    return null;
                }
            }

            /// <summary>
            /// 获取在线用户统计信息
            /// </summary>
            /// <returns></returns>
            internal OnlineUserStatInfo GetOnlineUserStatInfo()
            {
                return new OnlineUserStatInfo() {
                    CurrentOnlineUserCount = Count,
                    MaxOnlineUserCount = _maxOnlineUserCount,
                    MaxOnlineUserCountTime = _maxOnlineUserCountTime,
                };
            }

            private void _CheckExpired()
            {
                lock (_syncLocker)
                {
                    var dict = _dictOfSid.RemoveWhereWithReturn(item => item.Value.Connection.State == ConnectionState.Closed);
                    if (dict.Count > 0)
                    {
                        _dictOfUserId.RemoveRange(dict.Values.Select(v => v.ConnectionInfo.UserId));
                        _dictOfConnection.RemoveRange(dict.Values.Select(v => v.Connection));
                        _dictOfSid.RemoveRange(dict.Values.Select(v => v.SessionState.Sid));
                    }
                }
            }

            public void Dispose()
            {
                _timeHandle.Dispose();
            }
        }
    }
}
