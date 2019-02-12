using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.Tray;
using System.Diagnostics;
using Common.Utility;
using Common;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace ServiceFairy.Service.Tray.Components
{
    /// <summary>
    /// 进程管理器
    /// </summary>
    [AppComponent("进程管理器", "获取进程信息，及对进程进行控制")]
    class ProcessManagerAppComponent : AppComponent
    {
        public ProcessManagerAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;

        /// <summary>
        /// 获取所有进程的信息
        /// </summary>
        /// <returns></returns>
        public ProcessInfo[] GetAllProcessInfos()
        {
            Process[] process = Process.GetProcesses();
            return process.ToArray(p => new ProcessInfo() {
                ID = _TryGet(() => p.Id, -1),
                Name = _TryGet(() => p.ProcessName),
                FileName = _TryGet(() => p.MainModule.FileName),
                StartTime = _TryGet(() => p.StartTime.ToUniversalTime()),
                ThreadCount = _TryGet(() => p.Threads.Count, -1),
                User = _TryGet(() => p.StartInfo.UserName),
                VirtualMemorySize = _TryGet(() => p.VirtualMemorySize64, -1),
                PrivateMemorySize = _TryGet(() => p.PrivateMemorySize64, -1),
                PagedMemorySize = _TryGet(() => p.PagedMemorySize64, -1),
                PagedSystemMemorySize = _TryGet(() => p.PagedSystemMemorySize64, -1),
                PeakPagedMemorySize = _TryGet(() => p.PeakPagedMemorySize64, -1),
                PeakVirtualMemorySize = _TryGet(() => p.PeakVirtualMemorySize64, -1),
                WorkingSet = _TryGet(() => p.WorkingSet64, -1),
                NonpagedSystemMemorySize = _TryGet(() => p.NonpagedSystemMemorySize64, -1),
                MainWindowTitle = _TryGet(() => p.MainWindowTitle),
                PeakWorkingSet = _TryGet(() => p.PeakWorkingSet64, -1),
                TotalProcessorTime = _TryGet(() => p.TotalProcessorTime),
                UserProcessorTime = _TryGet(() => p.UserProcessorTime),
                ModuleCount = _TryGet(() => p.Modules.Count, -1),
                PriorityClass = _TryGet(() => p.PriorityClass, (ProcessPriorityClass)0),
            });
        }



        private static T _TryGet<T>(Func<T> func, T defaultValue = default(T))
        {
            try
            {
                return func();
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取线程信息
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public ThreadInfo[] GetThreadInfos(int processId)
        {
            Process p = Process.GetProcessById(processId);
            if (p == null)
                return Array<ThreadInfo>.Empty;

            return p.Threads.OfType<ProcessThread>().Select(t => new ThreadInfo() {
                ID = _TryGet(() => t.Id),
                StartTime = _TryGet(() => t.StartTime.ToUniversalTime()),
                ThreadState = _TryGet(() => t.ThreadState),
                ThreadPriorityLevel = _TryGet(() => t.PriorityLevel),
                ThreadWaitReason = _TryGet(() => t.WaitReason),
            }).ToArray();
        }

        /// <summary>
        /// 结束进程
        /// </summary>
        /// <param name="processIds">进程ID</param>
        public void KillProcesses(int[] processIds)
        {
            if (processIds.IsNullOrEmpty())
                return;

            int curProcessId = Process.GetCurrentProcess().Id;
            if (processIds.Contains(curProcessId))
                throw TrayUtility.CreateException(TrayStatusCode.CannotKillCurrentProcess);

            Parallel.ForEach<int>(processIds, (pId) => {
                Process p = Process.GetProcessById(pId);
                if (p != null)
                    p.Kill();
            });
        }

        /// <summary>
        /// 启动新进程
        /// </summary>
        /// <param name="startInfos">进程启动信息</param>
        public void StartProcesses(ServiceFairy.Entities.Tray.ProcessStartInfo[] startInfos)
        {
            if (startInfos.IsNullOrEmpty())
                return;

            Parallel.ForEach(startInfos, (startInfo) => StartProcess(startInfo));
        }

        /// <summary>
        /// 启动新进程
        /// </summary>
        /// <param name="startInfo">进程启动信息</param>
        public void StartProcess(ServiceFairy.Entities.Tray.ProcessStartInfo startInfo)
        {
            Contract.Requires(startInfo != null);

            Process.Start(startInfo.Path, startInfo.Args);
        }
    }
}
