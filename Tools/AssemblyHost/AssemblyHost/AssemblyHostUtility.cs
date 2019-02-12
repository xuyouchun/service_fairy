using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace AssemblyHost
{
    public static class AssemblyHostUtility
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

        public static string GetCurrentProcessName()
        {
            return Process.GetCurrentProcess().ProcessName;
        }

        public static int GetCurrentProcessId()
        {
            return Process.GetCurrentProcess().Id;
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

        public static string GetWaitExitProcessIds()
        {
            return Process.GetCurrentProcess().Id.ToString();
        }

        public static string GetKillProcessIds()
        {
            return string.Join(",", _GetKillProcessIds().Distinct());
        }

        private static IEnumerable<int> _GetKillProcessIds()
        {
            int curProcessId = GetCurrentProcessId();

            Process curProcess = Process.GetCurrentProcess();
            string runningPath = Path.GetDirectoryName(curProcess.MainModule.FileName);
            foreach (string file in Directory.GetFiles(runningPath, "*.exe"))  // 需要结束该目录下所有运行的程序
            {
                foreach (Process p in _GetProcesses(file))
                {
                    if (p != null && p.Id != curProcessId)
                        yield return p.Id;
                }
            }
        }

        private static IEnumerable<Process> _GetProcesses(string file)
        {
            string processName = Path.GetFileNameWithoutExtension(file);
            Process[] ps = Process.GetProcessesByName(processName);

            if (ps == null || ps.Length == 0)
                yield break;

            foreach (Process p in ps)
            {
                if (string.Equals(p.MainModule.FileName, file, StringComparison.OrdinalIgnoreCase))
                    yield return p;
            }
        }

        public static void StartProcess(string proc, string args)
        {
            LogManager.Log("启动进程：{0} {1}", proc, args);

            Process.Start(proc, args);
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

        /// <summary>
        /// 执行剩余的LiveUpdate任务，将在LiveUpdate过程中不能更新的文件更新过来
        /// </summary>
        /// <param name="args"></param>
        public static void DoLiveUpdateRest(string[] args)
        {
            ParameterReader r = new ParameterReader(args);
            string procIds = r.GetArg("/weid");

            // 强制结束进程
            if (!string.IsNullOrWhiteSpace(procIds))
            {
                foreach (int procId in procIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)))
                {
                    try
                    {
                        Process p = _GetProcessById(procId);
                        if (p != null)
                        {
                            LogManager.Log("等待进程退出: ProcName:{0} ProcId:{1}", p.ProcessName, p.Id);
                            if (!p.WaitForExit(15 * 1000))
                            {
                                LogManager.Log("等待进程退出超时: ProcName:{0}, procId:{1}，强制结束进程", p.ProcessName, p.Id);
                                p.Kill();
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.Log(ex);
                    }
                }
            }

            // 将带有*.LiveUpdate的文件重命名
            foreach (string file in Directory.GetFiles(GetExecutePath(), "*.LiveUpdate", SearchOption.AllDirectories))
            {
                try
                {
                    LogManager.Log("重命名文件: " + file);
                    string newFile = file.Substring(0, file.Length - ".LiveUpdate".Length);
                    if (File.Exists(newFile))
                        File.Delete(newFile);

                    File.Move(file, newFile);
                }
                catch (Exception ex)
                {
                    LogManager.Log(ex);
                }
            }
        }
    }
}
