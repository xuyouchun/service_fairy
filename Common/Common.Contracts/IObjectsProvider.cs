using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts
{
    /// <summary>
    /// 用于提供实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectsProvider<T>
    {
        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();
    }
}
