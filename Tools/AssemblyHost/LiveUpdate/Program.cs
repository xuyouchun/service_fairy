using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace LiveUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            string executePath = Utility.GetExecutePath();
            ParameterReader pr = new ParameterReader(args);

            Utility.Log("command line:{0}", Environment.CommandLine);

            // 停止服务
            string waitForExitService = pr.GetArg("/ws");
            if (!string.IsNullOrWhiteSpace(waitForExitService))
                Utility.StopService(waitForExitService);

            // kill进程
            string killProcIds = pr.GetArg("/kill");
            if (!string.IsNullOrWhiteSpace(killProcIds))
                Utility.KillProc(killProcIds);

            // 等待程序退出
            string waitForExitProc = pr.GetArg("/we");
            if (!string.IsNullOrWhiteSpace(waitForExitProc))
                Utility.WaitForExit(waitForExitProc);

            string waitForExitProcId = pr.GetArg("/weid");
            if (!string.IsNullOrWhiteSpace(waitForExitProcId))
                Utility.WaitForExitByIds(waitForExitProcId);

            // 更新程序集
            if (pr.GetArg("/cmd") == "LiveUpdate")
                _TryUpdate(executePath);

            // 启动普通程序
            string exec = pr.GetArg("/e");
            if (!string.IsNullOrWhiteSpace(exec))
            {
                Utility.Log("正在启动:" + exec);
                Utility.StartProcess(Path.GetFullPath(exec));
            }

            // 启动服务
            string service = pr.GetArg("/s");
            if (!string.IsNullOrWhiteSpace(service))
            {
                Utility.Log("启动服务:" + service);
                Utility.StartService(service);
            }
        }

        private static bool _TryUpdate(string executePath)
        {
            try
            {
                _Backup(executePath);
                _Update(executePath);

                return true;
            }
            catch(Exception ex)
            {
                Utility.Log(ex);
                return false;
            }
        }

        /// <summary>
        /// 将全部文件备份
        /// </summary>
        /// <param name="executePath"></param>
        private static void _Backup(string executePath)
        {
            Utility.Log("正在备份当前程序集 ...");

            string backupPath = Path.Combine(executePath, "LiveUpdate\\Backup");
            if (!Directory.Exists(backupPath))
                Directory.CreateDirectory(backupPath);

            string[] dirs;
            while ((dirs = Directory.GetDirectories(backupPath)).Length > 10)  // 最多保留十个备份
            {
                string dir = dirs.Min();
                Directory.Delete(dir);
            }

            string backupDir = Path.Combine(backupPath, DateTime.UtcNow.ToString("yyyyMMdd_HHmmss"));
            Utility.CopyDirectory(executePath, backupDir, (src, dst) =>
                !string.Equals(src, executePath + "\\LiveUpdate", StringComparison.OrdinalIgnoreCase), null
            );
        }

        private static string _EnsurePostfix(string s, string postfix)
        {
            if (s == null || !s.EndsWith(postfix))
                s += postfix;

            return s;
        }

        private static void _Update(string executePath)
        {
            Utility.Log("正在更新程序集 ...");

            string liveUpdatePath = Path.Combine(executePath, "LiveUpdate\\Update");
            string[] dirs;
            if (!Directory.Exists(liveUpdatePath) || (dirs = Directory.GetDirectories(liveUpdatePath)).Length == 0)
            {
                Utility.Log(string.Format("不存在路径：{0}，或该路径下没有任何文件", liveUpdatePath));
                return;
            }

            string dir = dirs.Max();
            Utility.Log(string.Format("开始从“{0}”拷贝文件", dir));
            Utility.CopyDirectory(dir, executePath, _CanCopy, _ErrorCallback);
        }

        private static readonly string _executeFile = Utility.GetExecuteFile();

        private static bool _CanCopy(string src, string dst)
        {
            if (!string.Equals(_executeFile, dst, StringComparison.OrdinalIgnoreCase))
            {
                int len = Path.GetDirectoryName(_executeFile).Length + 1;
                if (dst.Length > len)
                {
                    Utility.Log(string.Format("正在更新文件：{0} ...", dst.Substring(len)));
                    return true;
                }
            }

            File.Copy(src, dst + ".LiveUpdate");
            return false;
        }

        private static void _ErrorCallback(string src, string dst, Exception error)
        {
            Utility.Log("更新文件“{0}”时出错：{1}", dst, error.ToString());
        }
    }
}
