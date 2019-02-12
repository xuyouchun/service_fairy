using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.ServiceProcess;
using System.Threading;
using System.Runtime.CompilerServices;

namespace LiveUpdate
{
    static class Utility
    {
        private static string _executePath;

        /// <summary>
        /// 当前可执行程序文件所在的目录
        /// </summary>
        /// <returns></returns>
        public static string GetExecutePath()
        {
            return _executePath ?? (_executePath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
        }

        private static string _executeFile;

        /// <summary>
        /// 获取可执行程序文件
        /// </summary>
        /// <returns></returns>
        public static string GetExecuteFile()
        {
            return _executeFile ?? (_executeFile = Process.GetCurrentProcess().MainModule.FileName);
        }

        /// <summary>
        /// 将一个路径下的所有文件拷贝到另一个路径
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="destDirectory"></param>
        /// <param name="overwrite"></param>
        public static void CopyDirectory(string sourceDirectory, string destDirectory, bool overwrite = true, Action<string, string, Exception> errorCallback = null)
        {
            Contract.Requires(sourceDirectory != null && destDirectory != null);

            CopyDirectory(sourceDirectory, destDirectory, delegate(string src, string dst) {
                return overwrite || !File.Exists(dst);
            }, errorCallback);
        }

        /// <summary>
        /// 将一个路径下较新的文件拷贝到另一个路径
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="destDirectory"></param>
        /// <param name="errorCallback"></param>
        public static void CopyDirectoryIfNewer(string sourceDirectory, string destDirectory, Action<string, string, Exception> errorCallback)
        {
            Contract.Requires(sourceDirectory != null && destDirectory != null);

            CopyDirectory(sourceDirectory, destDirectory, delegate(string src, string dst) {
                return !File.Exists(dst) || File.GetLastWriteTimeUtc(src) > File.GetLastWriteTimeUtc(dst);
            }, errorCallback);
        }

        /// <summary>
        /// 将一个路径下的符合条件的文件拷贝到另一个路径
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="destDirectory"></param>
        /// <param name="condition"></param>
        public static void CopyDirectory(string sourceDirectory, string destDirectory, Func<string, string, bool> condition, Action<string, string, Exception> errorCallback)
        {
            Contract.Requires(sourceDirectory != null && destDirectory != null);

            if (condition != null && !condition(sourceDirectory, destDirectory))
                return;

            CreateDirectoryIfNotExists(destDirectory);
            foreach (string sourceFile in Directory.GetFiles(sourceDirectory))
            {
                string fileName = Path.GetFileName(sourceFile);
                string destFile = Path.Combine(destDirectory, fileName);

                if (condition == null || condition(sourceFile, destFile))
                {
                    try
                    {
                        File.Copy(sourceFile, destFile, true);
                    }
                    catch (Exception ex)
                    {
                        if (errorCallback != null)
                            errorCallback(sourceFile, destFile, ex);
                        else
                            throw;
                    }
                }
            }

            foreach (string sourcePath in Directory.GetDirectories(sourceDirectory))
            {
                string dir = Path.GetFileName(sourcePath);
                string destPath = Path.Combine(destDirectory, dir);

                CopyDirectory(sourcePath, destPath, condition, errorCallback);
            }
        }

        /// <summary>
        /// 如果文件存在则拷贝
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destFile"></param>
        /// <param name="overwrite"></param>
        public static void CopyFileIfExist(string sourceFile, string destFile, bool overwrite = false)
        {
            Contract.Requires(sourceFile != null && destFile != null);

            if (File.Exists(sourceFile))
                File.Copy(sourceFile, destFile, overwrite);
        }

        /// <summary>
        /// 如果不存在该目录，则创建
        /// </summary>
        /// <param name="directory"></param>
        public static void CreateDirectoryIfNotExists(string directory)
        {
            Contract.Requires(directory != null);

            if (!Directory.Exists(directory))
            {
                lock (string.Intern(directory))
                {
                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);
                }
            }
        }

        private static Process[] _GetProcess(string proc)
        {
            string[] procs = proc.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return procs.SelectMany(p => Process.GetProcessesByName(p.Trim())).ToArray();
        }

        /// <summary>
        /// 等待指定的进程退出
        /// </summary>
        /// <param name="proc"></param>
        public static void WaitForExit(string proc)
        {
            Process[] ps = _GetProcess(proc);
            _WaitForExit(ps);
        }

        private static void _WaitForExit(Process[] ps)
        {
            if (ps == null || ps.Length == 0)
                return;

            CountdownEvent e = new CountdownEvent(ps.Length);
            foreach (Process p in ps)
            {
                Process p0 = p;
                ThreadPool.QueueUserWorkItem(delegate {
                    _WaitForExit(p0, e);
                });
            }

            e.Wait();
        }

        private static void _WaitForExit(Process p, CountdownEvent e = null)
        {
            try
            {
                Log("等待进程结束:ProcId:{0} ProcName:{1}", p.Id, p.ProcessName);
                if (p != null && !p.WaitForExit(1000 * 15))
                {
                    Log("等待超时，强制结束进程:ProcId{0} ProcName:{1}", p.Id, p.ProcessName);
                    _Kill(p);
                }
            }
            catch(Exception ex)
            {
                Log(ex);
            }
            finally
            {
                if (e != null)
                    e.Signal();
            }
        }

        /// <summary>
        /// 等待指定的进程退出
        /// </summary>
        /// <param name="procIds"></param>
        public static void WaitForExitByIds(int[] procIds)
        {
            Process[] ps = procIds.Select(pId => _GetProcessById(pId)).ToArray();
            _WaitForExit(ps);
        }

        public static void WaitForExitByIds(string procIds)
        {
            int[] ids = procIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.Parse(s)).ToArray();

            WaitForExitByIds(ids);
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="service"></param>
        public static void StopService(string service)
        {
            try
            {
                int index = service.IndexOf(':');
                string serviceName = index < 0 ? service : service.Substring(0, index);
                int procId = index < 0 ? -1 : int.Parse(service.Substring(index + 1));

                using (ServiceController sc = new ServiceController(serviceName))
                {
                    Log("正在停止服务:{0}", service);

                    if (sc.CanStop)
                        sc.Stop();

                    sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(15));
                    sc.Refresh();
                    if (sc.Status != ServiceControllerStatus.Stopped)
                    {
                        Log("服务{0}超时未结束", serviceName);
                        if (procId >= 0)
                        {
                            Log("强制结束服务{0}的进程", serviceName);
                            _WaitForExit(_GetProcessById(procId), null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        public static int GetCurrentProcessId()
        {
            return Process.GetCurrentProcess().Id;
        }

        public static string GetFullPath(string path)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        }

        public static void StartService(string service)
        {
            using (ServiceController sc = new ServiceController(service))
            {
                sc.Start(new[] { "/weid:" + GetCurrentProcessId() });
                sc.WaitForStatus(ServiceControllerStatus.Running);
            }
        }

        public static void StartProcess(string procFile)
        {
            Process.Start(procFile, string.Format("\"/weid:{0}\"", GetCurrentProcessId()));
        }

        public static void KillProc(string killProcIds)
        {
            int[] procIds = killProcIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.Parse(s)).ToArray();

            KillProc(procIds);
        }

        private static void KillProc(int[] procIds)
        {
            foreach (int procId in procIds)
            {
                _Kill(procId);
            }
        }

        private static void _Kill(Process proc)
        {
            if (proc == null)
                return;

            try
            {
                Log("正在结束进程:ProcID={0}, ProcName={1}", proc.Id, proc.ProcessName);
                proc.Kill();
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        private static void _Kill(int procId)
        {
            _Kill(_GetProcessById(procId));
        }

        private static Process _GetProcessById(int procId)
        {
            try
            {
                return Process.GetProcessById(procId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void Log(string format, params object[] args)
        {
            Log(string.Format(format, args));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Log(string msg)
        {
            try
            {
                File.AppendAllText(_logPath,
                    string.Format("[{0}] <LiveUpdate> {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msg));
            }
            catch { }
        }

        private static readonly string _logPath = Path.Combine(GetExecutePath(), "AssemblyHost.log");

        public static void Log(Exception error)
        {
            if (error == null || error is ThreadAbortException)
                return;

            Log(error.ToString());
        }
    }
}
