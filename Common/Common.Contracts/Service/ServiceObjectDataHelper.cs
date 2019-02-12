using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace Common.Contracts.Service
{
    public class ServiceObjectDataHelper
    {
        public ServiceObjectDataHelper(IServiceObjectEntity entity)
        {
            Contract.Requires(entity != null);

            _entity = entity;
        }

        private readonly IServiceObjectEntity _entity;

        /// <summary>
        /// 获取指定键的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T Get<T>(object key, T defaultValue = default(T))
            where T : class
        {
            Contract.Requires(key != null);

            return _entity.Items.GetOrDefault(key, defaultValue) as T;
        }

        /// <summary>
        /// 获取指定类型的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>() where T : class
        {
            return _entity.Items.GetOrDefault(typeof(T)) as T;
        }

        /// <summary>
        /// 设置指定键的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(object key, object value)
        {
            Contract.Requires(key != null);

            _entity.Items[key] = value;
        }

        /// <summary>
        /// 设置指定类型的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void Set<T>(T value) where T : class
        {
            _entity.Items[typeof(T)] = value;
        }

        /// <summary>
        /// 获取或设置指定键的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public object GetOrSet(object key, Func<object, object> creator)
        {
            Contract.Requires(key != null);
            return _entity.Items.GetOrSet(key, creator);
        }

        /// <summary>
        /// 获取或设置指定类型的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="creator"></param>
        /// <returns></returns>
        public T GetOrSet<T>(Func<Type, T> creator) where T : class
        {
            return _entity.Items.GetOrSet(typeof(T), key => creator((Type)key)) as T;
        }

        /// <summary>
        /// 获取或设置指定类型的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetOrSet<T>() where T : class, new()
        {
            return GetOrSet<T>(t => new T());
        }
    }
}
