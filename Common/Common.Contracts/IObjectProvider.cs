using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Contracts
{
    /// <summary>
    /// 用于获取指定类型的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectProvider<T>
    {
        /// <summary>
        /// 获取指定类型的对象
        /// </summary>
        /// <returns></returns>
        T Get();
    }

    /// <summary>
    /// 单个对象的服务提供策略
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectProvider<T> : IObjectProvider<T>
    {
        public ObjectProvider(T obj)
        {
            _obj = obj;
        }

        private readonly T _obj;

        public T Get()
        {
            return _obj;
        }
    }
}
