using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts
{
    /// <summary>
    /// 获取一个对象的键
    /// </summary>
    public interface IObjectKeyProvider
    {
        /// <summary>
        /// 获取对象的键
        /// </summary>
        /// <returns></returns>
        object GetKey();
    }

    /// <summary>
    /// 获取一个对象的键
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IObjectKeyProvider<TKey> : IObjectKeyProvider
    {
        /// <summary>
        /// 获取对象的键
        /// </summary>
        /// <returns></returns>
        new TKey GetKey();
    }
}
