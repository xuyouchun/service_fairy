using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;
using Common.Communication.Wcf;
using System.ServiceModel;
using System.Net.Sockets;

namespace Common.Framework.TrayPlatform
{
    partial class WcfTrayAppServiceApplication
    {
        class TraySessionStateManager : MarshalByRefObjectEx, ITraySessionStateManager
        {
            public TraySessionStateManager(WcfTrayAppServiceApplication owner, ISecurityProvider securityProvider)
            {
                _owner = owner;
                _securityProvider = securityProvider;
            }

            private readonly WcfTrayAppServiceApplication _owner;
            private readonly ISecurityProvider _securityProvider;

            /// <summary>
            /// 根据安全码获取用户的会话状态
            /// </summary>
            /// <param name="sid"></param>
            /// <returns></returns>
            public UserSessionState GetSessionState(Sid sid)
            {
                return _securityProvider.GetUserSessionState(sid);
            }

            /// <summary>
            /// 根据用户名获取用户的基本信息
            /// </summary>
            /// <param name="username"></param>
            /// <returns></returns>
            public UserBasicInfo GetBasicInfo(string username)
            {
                return _securityProvider.GetUserBasicInfo(username);
            }

            /// <summary>
            /// 获取所有连接在该终端的用户
            /// </summary>
            /// <returns></returns>
            public UserConnectionInfo[] GetAllConnectedUsers()
            {
                return _owner._userConnectionManager.Value.GetAllUserConnectionInfos();
            }

            /// <summary>
            /// 获取在线用户统计信息
            /// </summary>
            /// <returns></returns>
            public OnlineUserStatInfo GetOnlineUserStatInfo()
            {
                UserConnectionManager mgr = _owner._userConnectionManager.Value;
                return mgr.GetOnlineUserStatInfo();
            }

            /// <summary>
            /// 根据上次访问时间获取该终端的用户
            /// </summary>
            /// <param name="from"></param>
            /// <param name="to"></param>
            /// <returns></returns>
            public UserConnectionInfo[] GetConnectionUsersByLastAccessTime(DateTime from, DateTime to)
            {
                return _owner._userConnectionManager.Value.GetConnectionUsersByLastAccessTime(from, to);
            }

            /// <summary>
            /// 获取最近断开连接的用户
            /// </summary>
            /// <returns></returns>
            public UserDisconnectedInfo[] GetDisconnectedInfos()
            {
                return _owner._userConnectionManager.Value.GetDisconnectedInfos();
            }

            /// <summary>
            /// 创建指定用户的通道
            /// </summary>
            /// <param name="userId"></param>
            /// <returns></returns>
            public ICommunicate CreateCommunicate(int userId)
            {
                UserConnectionManager conMgr = _owner._userConnectionManager.Value;
                IConnection connection = conMgr.GetUserConnection(userId);
                if (connection == null)
                    return null;

                return new UserCommunicateProxy(this, conMgr, userId, connection);
            }

            #region Class UserCommunicateProxy ...

            class UserCommunicateProxy : MarshalByRefObjectEx, ICommunicate
            {
                public UserCommunicateProxy(TraySessionStateManager ssMgr, UserConnectionManager conMgr, int userId, IConnection connection)
                {
                    _ssMgr = ssMgr;
                    _conMgr = conMgr;
                    _userId = userId;
                    _connection = connection;
                }

                private readonly TraySessionStateManager _ssMgr;
                private readonly UserConnectionManager _conMgr;
                private readonly int _userId;
                private IConnection _connection;

                public CommunicateData Call(CommunicateContext context, string method, CommunicateData data, CallingSettings settings = null)
                {
                    try
                    {
                        IConnection con = _conMgr.GetUserConnection(_userId);
                        if (con != null)
                            _connection = con;

                        return _connection.Call(context, method, data, settings);
                    }
                    catch (Exception ex)
                    {
                        if (ex is ObjectDisposedException || ex is CommunicationException || ex.IsCauseBy<SocketException>())
                        {
                            _conMgr.RemoveByConnection(_connection);
                        }

                        throw;
                    }
                }

                public void Dispose()
                {
                    
                }
            }

            #endregion
        }
    }
}
