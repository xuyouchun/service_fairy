using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using System.Runtime.Serialization;
using Common.Contracts;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// 会话状态管理器
    /// </summary>
    public interface ITraySessionStateManager
    {
        /// <summary>
        /// 获取指定安全码的会话状态，如果安全码不存在则返回空引用
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        UserSessionState GetSessionState(Sid sid);

        /// <summary>
        /// 获取用户基础信息
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        UserBasicInfo GetBasicInfo(string username);

        /// <summary>
        /// 获取所有已连接的用户
        /// </summary>
        /// <returns></returns>
        UserConnectionInfo[] GetAllConnectedUsers();

        /// <summary>
        /// 按上次访问时间获取已连接的用户
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        UserConnectionInfo[] GetConnectionUsersByLastAccessTime(DateTime from, DateTime to);

        /// <summary>
        /// 获取最近断线的用户
        /// </summary>
        /// <returns></returns>
        UserDisconnectedInfo[] GetDisconnectedInfos();

        /// <summary>
        /// 创建指定用途的通道
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ICommunicate CreateCommunicate(int userId);

        /// <summary>
        /// 获取已连接的用户数量
        /// </summary>
        /// <returns></returns>
        OnlineUserStatInfo GetOnlineUserStatInfo();
    }
    

    /// <summary>
    /// 安全策略
    /// </summary>
    public interface ISecurityProvider
    {
        /// <summary>
        /// 获取指定安全码的会话状态
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        UserSessionState GetUserSessionState(Sid sid);

        /// <summary>
        /// 获取指定用户名的基础信息
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        UserBasicInfo GetUserBasicInfo(string username);
    }
}
