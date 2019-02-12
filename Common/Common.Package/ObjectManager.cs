using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;
using Common.Utility;

namespace Common.Package
{
    /// <summary>
    /// 对象管理器，提供对对象轮询的管理功能
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectManager<T> : IDisposable where T : class
    {
        public ObjectManager(TimeSpan interval, Action<T> action)
        {
            _action = action;
            _taskHandle = GlobalTimer<ITask>.Default.Add(interval, _Refresh, false);
        }

        private readonly HashSet<WeakReference<T>> _hs = new HashSet<WeakReference<T>>();
        private readonly IGlobalTimerTaskHandle _taskHandle;
        private readonly Action<T> _action;

        /// <summary>
        /// 注册一个对象
        /// </summary>
        /// <param name="obj"></param>
        public void Register(T obj)
        {
            Contract.Requires(obj != null);

            lock (_hs)
            {
                _hs.Add(new WeakReference<T>(obj));
            }
        }

        /// <summary>
        /// 取消注册一个对象
        /// </summary>
        /// <param name="obj"></param>
        public void Unregister(T obj)
        {
            Contract.Requires(obj != null);

            lock (_hs)
            {
                _hs.Remove(new WeakReference<T>(obj));
            }
        }

        /// <summary>
        /// 获取全部对象
        /// </summary>
        /// <returns></returns>
        public T[] GetAll()
        {
            return _GetAllObjects().ToArray();
        }

        private IList<T> _GetAllObjects()
        {
            List<T> objs = new List<T>();
            lock (_hs)
            {
                List<WeakReference<T>> removedObjs = null;
                foreach (WeakReference<T> item in _hs)
                {
                    T obj = item.Target;
                    if (obj != null)
                        objs.Add(obj);
                    else
                        (removedObjs ?? (removedObjs = new List<WeakReference<T>>())).Add(item);
                }

                if (removedObjs != null)
                {
                    _hs.RemoveRange(removedObjs);
                }

                return objs.ToArray();
            }
        }

        private void _Refresh()
        {
            foreach (T obj in _GetAllObjects())
            {
                try
                {
                    if (_action != null)
                        _action(obj);
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            _taskHandle.Dispose();
        }

        ~ObjectManager()
        {
            Dispose();
        }
    }
}
