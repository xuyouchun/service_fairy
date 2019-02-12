using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Common.Package
{
    /// <summary>
    /// 对象的自动加载逻辑的封装
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AutoLoad<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loadFunc"></param>
        /// <param name="interval"></param>
        public AutoLoad(Func<T> loadFunc, TimeSpan interval = default(TimeSpan))
        {
            Contract.Requires(loadFunc != null);

            _loadFunc = loadFunc;
            _interval = interval <= TimeSpan.Zero ? TimeSpan.FromSeconds(5) : interval;
            _forceLoadInterval = TimeSpan.FromTicks(interval.Ticks * 10);
        }

        private readonly Func<T> _loadFunc;
        private TimeSpan _interval, _forceLoadInterval;

        /// <summary>
        /// 值
        /// </summary>
        public T Value
        {
            get { return _GetValue(); }
        }

        private T _value;

        private DateTime _lastLoad = default(DateTime);
        private volatile bool _loading, _loaded;
        private readonly object _thisLock = new object();

        private T _GetValue()
        {
            TimeSpan ts = QuickTime.UtcNow - _lastLoad;
            if (!_loaded || (ts > _interval && !_loading))
            {
                lock (_thisLock)
                {
                    if (!_loading)
                    {
                        _loading = true;

                        if (!_loaded || ts > _forceLoadInterval)
                        {
                            _Load(true);
                        }
                        else
                        {
                            ThreadPool.QueueUserWorkItem(_Load);
                        }
                    }
                }
            }
            else
            {
                return _value;
            }

            return _value;
        }

        private void _Load(object state)
        {
            try
            {
                _value = _loadFunc();
                _loaded = true;
            }
            catch (Exception ex)
            {
                if (state != null)
                    throw;

                LogManager.LogError(ex);
            }
            finally
            {
                _lastLoad = QuickTime.UtcNow;
                _loading = false;
            }
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void ClearCache()
        {
            lock (_thisLock)
            {
                _loaded = false;
                _lastLoad = DateTime.MinValue;
                _value = default(T);
            }
        }
    }
}
