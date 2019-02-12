using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.GlobalTimer.TimerStrategies;
using Common.Contracts.Service;
using System.Reflection;
using System.Diagnostics.Contracts;
using Common.Utility;
using System.Threading;
using Common.Contracts.UIObject;
using System.Drawing;
using Common.Package.UIObject;
using Common.Contracts;

namespace Common.Package
{
    [System.Diagnostics.DebuggerStepThrough]
    public static class PackageUtility
    {
        /// <summary>
        /// 加载指定的可执行程序集，并寻找入口点
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <returns></returns>
        public static Type GetAssemblyEntryPointType(string assemblyFile)
        {
            Contract.Requires(assemblyFile != null);

            Assembly assembly = Assembly.UnsafeLoadFrom(assemblyFile);
            return GetAssemblyEntryPointType(assembly);
        }

        /// <summary>
        /// 从指定的程序集中寻找入口点
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Type GetAssemblyEntryPointType(Assembly assembly)
        {
            Contract.Requires(assembly != null);

            AssemblyEntryPointAttribute attr = assembly.GetAttribute<AssemblyEntryPointAttribute>();
            if (attr != null)
                return attr.EntryPointType;

            return assembly.GetTypes().FirstOrDefault(t => t.IsDefined(typeof(AppEntryPointAttribute), true));
        }

        /// <summary>
        /// 加载指定的可执行程序集，并寻找入口点
        /// </summary>
        /// <param name="assemblyBytes"></param>
        /// <returns></returns>
        public static Type GetAssemblyEntryPointType(byte[] assemblyBytes)
        {
            Contract.Requires(assemblyBytes != null);

            Assembly assembly = Assembly.Load(assemblyBytes);
            return GetAssemblyEntryPointType(assembly);
        }

        /// <summary>
        /// 加载指定的可执行程序集
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <returns></returns>
        public static object GetAssemblyEntryPoint(string assemblyFile)
        {
            Contract.Requires(assemblyFile != null);

            Type type = GetAssemblyEntryPointType(assemblyFile);
            if (type == null)
                throw new NotSupportedException("未从该程序集中找到可执行的类型");

            return ObjectFactory.CreateObject(type);
        }

        /// <summary>
        /// 加载指定的可执行程序集
        /// </summary>
        /// <param name="assemblyBytes"></param>
        /// <returns></returns>
        public static object GetAssemblyEntryPoint(byte[] assemblyBytes)
        {
            Contract.Requires(assemblyBytes != null);
            Type type = GetAssemblyEntryPointType(assemblyBytes);
            if (type == null)
                throw new NotSupportedException("未从该程序集中找到可执行的类型");

            return ObjectFactory.CreateObject(type);
        }

        /// <summary>
        /// 自动重试
        /// </summary>
        /// <param name="func"></param>
        /// <param name="interval"></param>
        /// <param name="maxTryTimes"></param>
        /// <returns></returns>
        public static bool AutoTry(Action func, TimeSpan interval, int maxTryTimes = int.MaxValue)
        {
            return CommonUtility.AutoTry(func, delegate(int tryTimes, Exception error) {

                LogManager.LogError(error);
                if (tryTimes >= maxTryTimes)
                    return false;

                Thread.Sleep(interval);
                return true;
            });
        }

        /// <summary>
        /// 自动重试
        /// </summary>
        /// <param name="func">调用的方法</param>
        /// <param name="initInterval">初始频率</param>
        /// <param name="stepInterval">递增频率</param>
        /// <param name="maxInterval">最大频率</param>
        /// <param name="maxTryTimes">最大重试次数</param>
        /// <returns></returns>
        public static bool AutoTry(Action func, TimeSpan initInterval, TimeSpan stepInterval, TimeSpan maxInterval, int maxTryTimes = int.MaxValue)
        {
            TimeSpan interval = initInterval;
            return CommonUtility.AutoTry(func, delegate(int tryTimes, Exception error)
            {
                LogManager.LogError(error);
                if (tryTimes >= maxTryTimes)
                    return false;

                Thread.Sleep(interval);
                if (interval < maxInterval)
                    interval = MathUtility.Min(maxInterval, interval + stepInterval);

                return true;
            });
        }

        /// <summary>
        /// 执行可取消的操作
        /// </summary>
        /// <param name="task"></param>
        /// <param name="cancelControl"></param>
        /// <returns></returns>
        public static bool ExecuteCancelableOperation(ICancelableTask task, ICancelableController cancelControl)
        {
            return new CancelableOperation().Execute(task, cancelControl);
        }

        /// <summary>
        /// 执行可取消的操作
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancelControl"></param>
        /// <returns></returns>
        public static bool ExecuteCancelableOperation(Action action, ICancelableController cancelControl)
        {
            Contract.Requires(action != null);

            return ExecuteCancelableOperation(new CancelableTask(action), cancelControl);
        }

        #region Class CancelableTask ...

        class CancelableTask : ICancelableTask
        {
            public CancelableTask(Action action)
            {
                _action = action;
            }

            private readonly Action _action;

            public void Cancel()
            {
                if (_runningThread != null)
                    _runningThread.Abort();
            }

            public bool SupportProgress()
            {
                return false;
            }

            public float GetProgress()
            {
                return 0.0f;
            }

            public event EventHandler ProgressChanged
            {
                add { }
                remove { }
            }

            private Thread _runningThread;

            public void Execute()
            {
                _runningThread = Thread.CurrentThread;
                _action();
            }
        }

        #endregion

        public static IUIObjectExecuteContext SetCurrent<T>(this IUIObjectExecuteContext executeContext, T obj) where T : class
        {
            Contract.Requires(executeContext != null);

            return UIObjectExecuteContextHelper.SetCurrent<T>(executeContext, obj);
        }

        /// <summary>
        /// 给指定的定时任务附加一个时间策略
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="timeStrategy"></param>
        public static void Attach(this IGlobalTimerTaskHandle handle, ITimerStrategy timeStrategy)
        {
            Contract.Requires(handle != null && timeStrategy != null);


        }

        /// <summary>
        /// 从类型中加载状态码信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static AppStatusCodeInfo[] LoadStatusCodeInfosFromType(Type type)
        {
            Contract.Requires(type != null);

            List<AppStatusCodeInfo> infos = new List<AppStatusCodeInfo>();
            foreach (FieldInfo fInfo in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                AppStatusCodeInfo codeInfo = LoadStatusCodeInfoFromFieldInfo(fInfo);
                if (codeInfo != null)
                    infos.Add(codeInfo);
            }

            return infos.ToArray();
        }

        /// <summary>
        /// 从字段信息中加载状态码信息
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public static AppStatusCodeInfo LoadStatusCodeInfoFromFieldInfo(FieldInfo fieldInfo)
        {
            object value;
            if (ConvertUtility.TryConvert(fieldInfo.GetValue(null), typeof(int), out value))
            {
                int statusCode = (int)value;
                string title = StringUtility.GetFirstNotNullOrEmptyString(fieldInfo.GetDesc(), DocUtility.GetSummary(fieldInfo));
                string desc = DocUtility.GetRemarks(fieldInfo);

                return new AppStatusCodeInfo(statusCode, fieldInfo.Name, title, desc);
            }

            return null;
        }

        /// <summary>
        /// 从枚举成员中加载状态码信息
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static AppStatusCodeInfo LoadStatusCodeInfoFromEnum(Enum enumValue)
        {
            Contract.Requires(enumValue != null);

            FieldInfo fInfo = ReflectionUtility.GetEnumFieldInfo(enumValue);
            return (fInfo == null) ? null : LoadStatusCodeInfoFromFieldInfo(fInfo);
        }
    }
}
