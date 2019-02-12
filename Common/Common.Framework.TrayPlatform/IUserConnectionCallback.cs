using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// 用户连接状态
    /// </summary>
    public interface IUserConnectionCallback
    {
        /// <summary>
        /// 用户连接
        /// </summary>
        /// <param name="userIds"></param>
        void OnConnected(int[] userIds);

        /// <summary>
        /// 用户断开连接
        /// </summary>
        /// <param name="userIds"></param>
        void OnDisconnected(int[] userIds);
    }
}
