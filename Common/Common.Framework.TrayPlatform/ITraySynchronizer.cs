using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// 线程同步器
    /// </summary>
    public interface ITraySynchronizer
    {
        /// <summary>
        /// 进入临界区
        /// </summary>
        /// <param name="name"></param>
        /// <param name="timeout"></param>
        bool Enter(string name, TimeSpan timeout);

        /// <summary>
        /// 尝试进入临界区
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool TryEnter(string name);

        /// <summary>
        /// 退出临界区
        /// </summary>
        /// <param name="name"></param>
        void Exit(string name);
    }
}
