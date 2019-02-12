using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf;
using System.Collections.Concurrent;
using Common.Contracts;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Contracts.Service;
using ServiceFairy.Entities.Navigation;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 用于管理多个用户连接
    /// </summary>
    public class UserConnectionManager : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="proxy">代理地址，类似net.http://127.0.0.1:80的形式</param>
        /// <param name="defaultDataFormat">默认的编码方式</param>
        public UserConnectionManager(string proxy, DataFormat defaultDataFormat = DataFormat.Unknown)
            : this(CommunicationOption.Parse(proxy), defaultDataFormat)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="proxy">代理地址</param>
        /// <param name="defaultDataFormat">默认的编码方式</param>
        public UserConnectionManager(CommunicationOption proxy, DataFormat defaultDataFormat = DataFormat.Unknown)
            : this(() => proxy, defaultDataFormat)
        {

        }

        internal UserConnectionManager(Func<CommunicationOption> proxyGetter,
            DataFormat defaultDataFormat = DataFormat.Unknown)
        {
            _proxyGetter = proxyGetter;
            _defaultDataFormat = defaultDataFormat;
        }

        private readonly Func<CommunicationOption> _proxyGetter;
        private readonly DataFormat _defaultDataFormat;

        private readonly ConcurrentDictionary<Sid, UserConnection> _connectionsOfSid = new ConcurrentDictionary<Sid, UserConnection>();
        private readonly ConcurrentDictionary<string, UserConnection> _connectionsOfUserName = new ConcurrentDictionary<string, UserConnection>();

        /// <summary>
        /// 创建一个用户连接
        /// </summary>
        /// <param name="sid">安全码</param>
        /// <param name="proxy">代理</param>
        /// <param name="format">编码</param>
        /// <returns></returns>
        public UserConnection Create(Sid sid, CommunicationOption proxy = null, DataFormat format = DataFormat.Unknown)
        {
            Contract.Requires(sid != null);

            Func<Sid, UserConnection> creator = (sid0) => _CreateConnection(sid0, proxy, format);
            return _Wrap(_connectionsOfSid.AddOrUpdate(sid, creator, (sid0, oldCon) => { oldCon.Dispose(); return creator(sid0); }));
        }

        /// <summary>
        /// 创建一个用户连接
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="proxy">代理</param>
        /// <param name="format">编码</param>
        /// <returns></returns>
        public UserConnection Create(string username, string password, CommunicationOption proxy = null, DataFormat format = DataFormat.Unknown)
        {
            _ReviseParameters(ref proxy, ref format);
            UserConnection userCon = new UserConnection(username, password, proxy, format) { Container = this };

            return _Wrap(_connectionsOfSid.AddOrUpdate(userCon.Sid, (sid0) => userCon, (sid0, oldCon) => { oldCon.Dispose(); return userCon; }));
        }

        /// <summary>
        /// 删除指定安全码的连接
        /// </summary>
        /// <param name="sid"></param>
        public UserConnection Remove(Sid sid)
        {
            Contract.Requires(sid != null);

            UserConnection uc, uc2;
            if (_connectionsOfSid.TryRemove(sid, out uc))
                _connectionsOfUserName.TryRemove(uc.UserInfo.UserName, out uc2);

            return uc;
        }

        private UserConnection _Wrap(UserConnection con)
        {
            con.Disposed += (sender, e) => {

                _connectionsOfSid.Remove(con.Sid);
                string username = _GetUserName(con);

                if (!string.IsNullOrEmpty(username))
                    _connectionsOfUserName.Remove(username);
            };

            return con;
        }

        private static string _GetUserName(UserConnection con)
        {
            return con.UserInfo == null ? null : con.UserInfo.UserName;
        }

        private void _ReviseParameters(ref CommunicationOption proxy, ref DataFormat format)
        {
            if (proxy == null)
                proxy = _proxyGetter();

            if (format == DataFormat.Unknown)
                format = _defaultDataFormat;

            if (proxy == null)
                throw new ArgumentException("无法获取代理服务器");
        }

        private UserConnection _CreateConnection(Sid sid, CommunicationOption proxy, DataFormat format)
        {
            _ReviseParameters(ref proxy, ref format);
            return new UserConnection(sid, proxy, format) { Container = this };
        }

        /// <summary>
        /// 获取全部用户连接
        /// </summary>
        /// <returns></returns>
        public UserConnection[] GetAll()
        {
            return _connectionsOfSid.Values.ToArray();
        }

        /// <summary>
        /// 获取指定sid的用户连接
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public UserConnection GetBySid(Sid sid)
        {
            return _connectionsOfSid.GetOrDefault(sid);
        }

        /// <summary>
        /// 通过用户名寻找
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserConnection GetByUserName(string username)
        {
            Contract.Requires(username != null);

            UserConnection userCon = _connectionsOfUserName.GetOrDefault(username);
            if (userCon == null)
            {
                userCon = _connectionsOfSid.Values.FirstOrDefault(con => _GetUserName(con) == username);
                if (userCon != null)
                    _connectionsOfUserName[username] = userCon;
            }

            return userCon;
        }

        /// <summary>
        /// 释放所有用户连接
        /// </summary>
        public void Dispose()
        {
            GetAll().ForEach(con => con.Dispose());

            _connectionsOfUserName.Clear();
            _connectionsOfSid.Clear();
        }

        /// <summary>
        /// 通过导航创建
        /// </summary>
        /// <param name="navigation">导航服务器地址</param>
        /// <param name="navDataFormat">导航数据格式</param>
        /// <param name="proxyCommunicationType">代理通信类型</param>
        /// <param name="proxyDataFormat">代理数据格式</param>
        /// <returns></returns>
        public static UserConnectionManager FromNavigation(CommunicationOption navigation, DataFormat navDataFormat = DataFormat.Unknown,
            CommunicationType proxyCommunicationType = CommunicationType.Unknown, DataFormat proxyDataFormat = DataFormat.Unknown)
        {
            Contract.Requires(navigation != null);

            return new UserConnectionManager(
                () => InvokerBase.GetProxyList(navigation, proxyCommunicationType, CommunicationDirection.Bidirectional, proxyDataFormat, 1, throwErrorWhenEmpty: true).FirstOrDefault(),
                proxyDataFormat
            );
        }

        /// <summary>
        /// 通过导航创建
        /// </summary>
        /// <param name="navigation">导航服务器地址</param>
        /// <param name="navDataFormat">导航数据格式</param>
        /// <param name="proxyCommunicationType">代理通信类型</param>
        /// <param name="proxyDataFormat">代理数据格式</param>
        /// <returns></returns>
        public static UserConnectionManager FromNavigation(string navigation, DataFormat navDataFormat = DataFormat.Unknown,
            CommunicationType proxyCommunicationType = CommunicationType.Unknown, DataFormat proxyDataFormat = DataFormat.Unknown)
        {
            return FromNavigation(CommunicationOption.Parse(navigation), navDataFormat, proxyCommunicationType, proxyDataFormat);
        }
    }
}
