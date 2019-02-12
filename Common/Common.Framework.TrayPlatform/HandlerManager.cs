using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using System.Diagnostics.Contracts;

namespace Common.Framework.TrayPlatform
{
    class HandlerManager
    {
        private Dictionary<Type, object[]> _handlerDict = new Dictionary<Type, object[]>();
        private Dictionary<object, Dictionary<Type, HashSet<object>>> _dict = new Dictionary<object, Dictionary<Type, HashSet<object>>>();
        private readonly object _locker = new object();

        private void _Rebuilder()
        {
            var list = from item in _dict.Values.SelectMany()
                       group item by item.Key into g
                       select new { Type = g.Key, Handlers = g.SelectMany(v => v.Value).ToArray() };

            _handlerDict = list.ToDictionary(v => v.Type, v => v.Handlers);
        }

        /// <summary>
        /// 注册一个句柄
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="type"></param>
        /// <param name="handler"></param>
        public void Register(object owner, Type type, object handler)
        {
            Contract.Requires(owner != null && type != null && handler != null);

            lock (_locker)
            {
                _dict.GetOrSet(owner).GetOrSet(type).Add(handler);
                _Rebuilder();
            }
        }

        /// <summary>
        /// 取消注册一个句柄
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="type"></param>
        /// <param name="handler"></param>
        public void Unregister(object owner, Type type, object handler)
        {
            Contract.Requires(owner != null && type != null && handler != null);

            lock (_locker)
            {
                Dictionary<Type, HashSet<object>> dict;
                if (_dict.TryGetValue(owner, out dict))
                {
                    HashSet<object> hs;
                    if (dict.TryGetValue(type, out hs))
                    {
                        if (hs.Remove(handler))
                        {
                            _Rebuilder();
                            if (hs.Count == 0)
                                dict.Remove(type);
                        }
                    }

                    if (dict.Count == 0)
                        _dict.Remove(owner);
                }
            }
        }

        /// <summary>
        /// 取消注册
        /// </summary>
        /// <param name="owner"></param>
        public void Unregister(object owner)
        {
            Contract.Requires(owner != null);

            lock (_locker)
            {
                if (_dict.Remove(owner))
                {
                    _Rebuilder();
                }
            }
        }

        /// <summary>
        /// 获取指定类型的句柄
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object[] GetHandlers(Type type)
        {
            Contract.Requires(type != null);

            object[] arr;
            if (_handlerDict.TryGetValue(type, out arr))
                return arr;

            return Array<object>.Empty;
        }

        /// <summary>
        /// 获取指定类型的句柄
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] GetHandlers<T>() where T : class
        {
            return (T[])GetHandlers(typeof(T));
        }

        /// <summary>
        /// 获取指定类型的句柄
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object GetHandler(Type type)
        {
            Contract.Requires(type != null);
            return GetHandlers(type).FirstOrDefault();
        }

        /// <summary>
        /// 获取指定类型的句柄
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetHandler<T>() where T : class
        {
            return GetHandler(typeof(T)) as T;
        }
    }
}
