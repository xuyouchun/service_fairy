using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Configuration;
using System.Reflection;
using System.Management;
using System.IO;
using System.Collections;

namespace Common.Utility
{
    /// <summary>
    /// 公用的工具类
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public static class CommonUtility
    {
        /// <summary>
        /// 从app.config中加载值并转换为指定的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetFromAppConfig<T>(string key, T defaultValue = default(T))
        {
            string value = ConfigurationManager.AppSettings.Get(key);
            if (value == null)
                return defaultValue;

            return ConvertUtility.ToType<T>(value, defaultValue);
        }

        /// <summary>
        /// 从app.config中加载值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetFromAppConfig(string key, string defaultValue = null)
        {
            return ConfigurationManager.AppSettings.Get(key) ?? defaultValue;
        }

        /// <summary>
        /// 从指定的ServiceProvider中获取服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sp"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public static T GetService<T>(this IServiceProvider sp, bool throwError = false) where T : class
        {
            T svr = sp.GetService(typeof(T)) as T;
            if (throwError && svr == null)
                throw new InvalidOperationException("未找到服务" + typeof(T));

            return svr;
        }

        /// <summary>
        /// 自动重试的调用机制
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="func"></param>
        /// <param name="errorCallback"></param>
        /// <returns></returns>
        public static bool AutoTry<TOutput>(this Func<TOutput> func, out TOutput result, Func<int, Exception, bool> errorCallback)
        {
            Contract.Requires(func != null && errorCallback != null);
            result = default(TOutput);

            for (int tryTimes = 0; ; tryTimes++)
            {
                try
                {
                    result = func();
                    return true;
                }
                catch (Exception ex)
                {
                    if (!errorCallback(tryTimes, ex))
                        return false;
                }
            }
        }

        /// <summary>
        /// 自动重试的调用机制
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="func"></param>
        /// <param name="errorCallback"></param>
        /// <returns></returns>
        public static bool AutoTry(this Action func, Func<int, Exception, bool> errorCallback)
        {
            Contract.Requires(func != null && errorCallback != null);

            for (int tryTimes = 0; ; tryTimes++)
            {
                try
                {
                    func();
                    return true;
                }
                catch (Exception ex)
                {
                    if (!errorCallback(tryTimes, ex))
                        return false;
                }
            }
        }

        /// <summary>
        /// 自动重试的调用机制
        /// </summary>
        /// <param name="func"></param>
        /// <param name="errorCallback"></param>
        /// <returns></returns>
        public static bool AutoTry(this Func<bool> func, Func<int, Exception, bool> errorCallback)
        {
            Contract.Requires(func != null && errorCallback != null);

            for (int tryTimes = 0; ; tryTimes++)
            {
                try
                {
                    if (func())
                        return true;

                    if (!errorCallback(tryTimes, null))
                        return false;
                }
                catch (Exception ex)
                {
                    if (!errorCallback(tryTimes, ex))
                        return false;
                }
            }
        }


        /// <summary>
        /// 执行指定的方法并将其异常信息返回，如果返回null则表示执行正确
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Exception TryInvoke(this Action func)
        {
            Contract.Requires(func != null);

            try
            {
                func();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        /// <summary>
        /// 激发事件
        /// </summary>
        /// <param name="eh"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void RaiseEvent(this EventHandler eh, object sender, EventArgs e = null)
        {
            if (eh != null)
                eh(sender, e ?? EventArgs.Empty);
        }

        /// <summary>
        /// 激发错误事件
        /// </summary>
        /// <param name="eh"></param>
        /// <param name="sender"></param>
        /// <param name="error"></param>
        public static void RaiseEvent(this ErrorEventHandler eh, object sender, Exception error)
        {
            if (eh != null)
                eh(sender, new ErrorEventArgs(error));
        }

        /// <summary>
        /// 释放指定的对象
        /// </summary>
        /// <param name="obj"></param>
        public static void Dispose(this object obj)
        {
            IDisposable dis = obj as IDisposable;
            if (dis != null)
                dis.Dispose();
        }

        /// <summary>
        /// 强制System.Lazy执行加载操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lazy"></param>
        public static bool EnsureLoaded<T>(this Lazy<T> lazy)
        {
            Contract.Requires(lazy != null);

            if (!lazy.IsValueCreated)
            {
                _LazyLoad(lazy);
                return true;
            }

            return false;
        }

        private static T _LazyLoad<T>(Lazy<T> lazy)
        {
            return lazy.Value;
        }

        /// <summary>
        /// 判断异常是否为指定的异常，或由指定的异常所引起
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool IsCauseBy<TException>(this Exception error) where TException : Exception
        {
            while (error != null)
            {
                if (error is TException)
                    return true;

                error = error.InnerException;
            }

            return false;
        }

        public static int GetHashCode(IEnumerable objs)
        {
            return _GetHashCode(objs, new HashSet<object>());
        }

        public static int GetHashCode(object obj)
        {
            return _GetHashCode(obj, new HashSet<object>());
        }

        private static int _GetHashCode(IEnumerable objs, HashSet<object> hs)
        {
            int hc = 0;
            foreach (object obj in objs)
            {
                if (obj != null && !hs.Contains(obj))
                {
                    hc ^= _GetHashCode(obj, hs);
                    hs.Add(obj);

                }
            }

            return hc;
        }

        private static int _GetHashCode(object obj, HashSet<object> hs)
        {
            if (obj == null)
                return 0;

            if (obj is string)
            {
                return obj.GetHashCode();
            }

            if (obj is IEnumerable)
            {
                return _GetHashCode((IEnumerable)obj, hs);
            }

            return obj.GetHashCode();
        }

        public static bool CompareHashCode(object obj1, object obj2)
        {
            return GetHashCode(obj1) == GetHashCode(obj2);
        }
    }
}
