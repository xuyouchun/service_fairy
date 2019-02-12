using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Dictionary = System.Collections.Generic.Dictionary<string, object>;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Common.Utility
{
    /// <summary>
    /// 线程工具类
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public static class ThreadUtility
    {
        private static readonly LocalDataStoreSlot _LocalDataStoreSlot = Thread.AllocateDataSlot();

        private static Dictionary _GetCurrentDictionary()
        {
            Dictionary dict = Thread.GetData(_LocalDataStoreSlot) as Dictionary;
            if (dict == null)
                Thread.SetData(_LocalDataStoreSlot, dict = new Dictionary());

            return dict;
        }

        /// <summary>
        /// 设置线程局部变量
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetTlsData(string key, object value)
        {
            Contract.Requires(key != null);
            _GetCurrentDictionary()[key] = value;
        }

        /// <summary>
        /// 获取线程局部变量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetTlsData<T>(string key, T defaultValue = default(T))
            where T : class
        {
            Contract.Requires(key != null);
            return _GetCurrentDictionary().GetOrDefault(key, defaultValue) as T;
        }

        /// <summary>
        /// 获取或设置线程局部变量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="loader"></param>
        /// <returns></returns>
        public static T GetOrSetTlsData<T>(string key, Func<T> loader)
            where T : class
        {
            Contract.Requires(key != null);
            return _GetCurrentDictionary().GetOrSet(key, (item) => loader()) as T;
        }

        /// <summary>
        /// 获取线程局部变量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            Contract.Requires(key != null);
            return _GetCurrentDictionary().GetOrDefault(key);
        }

        /// <summary>
        /// 获取线程局部变量
        /// </summary>
        /// <param name="key"></param>
        /// <param name="loader"></param>
        /// <returns></returns>
        public static object GetOrSetTlsData(string key, Func<object> loader)
        {
            Contract.Requires(key != null);
            return _GetCurrentDictionary().GetOrSet(key, (item) => loader());
        }

        /// <summary>
        /// 如果对象为空则调用指定的方法来创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static T Create<T>(ref T obj, Func<T> creator) where T : class
        {
            Contract.Requires(creator != null);

            if (obj != null)
                return obj;

            lock (creator.Method)
            {
                if (obj == null)
                    obj = creator();
            }

            return obj;
        }

        public static T Create<T>(ref T obj) where T : class, new()
        {
            return Create<T>(ref obj, () => new T());
        }

        /// <summary>
        /// 启动线程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="threadProc"></param>
        /// <param name="arg"></param>
        /// <param name="threadPriority"></param>
        public static Thread StartNew<T>(Action<T> threadProc, T arg, ThreadPriority threadPriority = ThreadPriority.Normal)
        {
            Contract.Requires(threadProc != null);

            Thread thread = new Thread(new ParameterizedThreadStart(delegate {
                threadProc(arg);
            })) {
                IsBackground = true,
                Priority = threadPriority,
            };

            thread.Start();
            return thread;
        }

        /// <summary>
        /// 启动线程
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="threadProc"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="threadPriority"></param>
        /// <returns></returns>
        public static Thread StartNew<T1, T2>(Action<T1, T2> threadProc, T1 arg1, T2 arg2, ThreadPriority threadPriority = ThreadPriority.Normal)
        {
            Contract.Requires(threadProc != null);
            Thread thread = new Thread(new ParameterizedThreadStart(delegate {
                threadProc(arg1, arg2);
            })) {
                IsBackground = true,
                Priority = threadPriority,
            };

            thread.Start();
            return thread;
        }

        /// <summary>
        /// 启动线程
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="threadProc"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="threadPriority"></param>
        /// <returns></returns>
        public static Thread StartNew<T1, T2, T3>(Action<T1, T2, T3> threadProc, T1 arg1, T2 arg2, T3 arg3, ThreadPriority threadPriority = ThreadPriority.Normal)
        {
            Contract.Requires(threadProc != null);
            Thread thread = new Thread(new ParameterizedThreadStart(delegate {
                threadProc(arg1, arg2, arg3);
            })) {
                IsBackground = true,
                Priority = threadPriority,
            };

            thread.Start();
            return thread;
        }

        /// <summary>
        /// 启动线程
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="threadProc"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <param name="threadPriority"></param>
        /// <returns></returns>
        public static Thread StartNew<T1, T2, T3, T4>(Action<T1, T2, T3, T4> threadProc, T1 arg1, T2 arg2, T3 arg3, T4 arg4, ThreadPriority threadPriority = ThreadPriority.Normal)
        {
            Contract.Requires(threadProc != null);
            Thread thread = new Thread(new ParameterizedThreadStart(delegate {
                threadProc(arg1, arg2, arg3, arg4);
            })) {
                IsBackground = true,
                Priority = threadPriority,
            };

            thread.Start();
            return thread;
        }

        /// <summary>
        /// 启动线程
        /// </summary>
        /// <param name="threadPriority"></param>
        /// <param name="threadProc"></param>
        /// <returns></returns>
        public static Thread StartNew(ThreadStart threadProc, ThreadPriority threadPriority = ThreadPriority.Normal)
        {
            Contract.Requires(threadProc != null);

            Thread thread = new Thread(threadProc) { IsBackground = true, Priority = threadPriority };
            thread.Start();
            return thread;
        }
    }
}
