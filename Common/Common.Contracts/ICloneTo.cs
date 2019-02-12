using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts
{
    /// <summary>
    /// 将对象复制到指定的对象
    /// </summary>
    public interface ICloneTo
    {
        /// <summary>
        /// 复制到指定的对象
        /// </summary>
        /// <param name="dst"></param>
        void CloneTo(object dst);
    }

    /// <summary>
    /// 将对象复制到指定的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneTo<T> : ICloneTo
    {
        /// <summary>
        /// 复制到指定的对象
        /// </summary>
        /// <param name="dst"></param>
        void CloneTo(T dst);
    }
}
