using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using System.IO;
using Common.Contracts;
using System.Diagnostics;

namespace ServiceFairy.Setup.Builder
{
    class Program
    {
        const string BASE_PATH = @"d:\work\dev";
        const string SETUP_PATH = BASE_PATH + @"\Assembly\Setup\ServiceFairy";

        static void Main(string[] args)
        {
            try
            {
                _DoCopyFiles();
                Console.WriteLine("OK!");

                Process.Start(SETUP_PATH);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }

        private static void _DoCopyFiles()
        {
            // 拷贝Common程序集
            _CopyFiles(@"Assembly\Common\*.*", @"Assembly");

            // 拷贝Service目录
            foreach (string serviceDir in Directory.GetDirectories(Path.Combine(BASE_PATH, @"Assembly\Service")))
            {
                string serviceName = Path.GetFileName(serviceDir);
                if (!serviceName.StartsWith("Core.") && !serviceName.StartsWith("Sys."))
                    continue;

                foreach (string versionDir in Directory.GetDirectories(serviceDir))
                {
                    string version = Path.GetFileName(versionDir);
                    SVersion sversion;
                    if (SVersion.TryParse(version, out sversion))
                    {
                        foreach (string pattern in new[] { "Main.*"/*, "Main.*.*"*/ })
                        {
                            _CopyFiles(Path.Combine(versionDir, pattern),
                            Path.Combine(SETUP_PATH, @"Service\" + serviceName + "\\" + version));
                        }
                    }
                }
            }

            // 拷贝ServiceFairy文件
            _CopyFiles(@"Assembly\ServiceFairy\Common\ServiceFairy.*", @"Assembly");

            // 拷贝AssemblyHost
            _CopyFiles(@"Assembly\AssemblyHost\WindowsService\AssemblyHost.dll");
            _CopyFile(@"Assembly\AssemblyHost\WindowsService\AssemblyHost.WindowsService.exe", @"ServiceFairy.exe");
            _CopyFile(@"Assembly\AssemblyHost\Console\AssemblyHost.Console.exe", @"ServiceFairy.Console.exe");
            _CopyFile(@"Assembly\AssemblyHost\LiveUpdate\LiveUpdate.exe", @"LiveUpdate.exe");

            // 拷贝Product
            _CopyFiles(@"Assembly\ServiceFairy\Product\ServiceFairy.Product.*");

            // 拷贝TrayManagement
            _CopyFiles(@"Assembly\ServiceFairy\TrayMgr\TrayMgr.*");

            // 拷贝ServiceFaily.Install
            _CopyFiles(@"Assembly\ServiceFairy\Install\ServiceFairy.Install.*", @"Assembly");

            // 拷贝ServiceFaily.Cluster
            _CopyFiles(@"Assembly\ServiceFairy\Cluster\ServiceFairy.Cluster.*", @"Assembly");
            _CopyFiles(@"Assembly\ServiceFairy\Cluster\SfCluster.*");

            // 删除vshost文件
            _DeleteFiles(@"*.vshost.*");
        }

        private static void _CopyFile(string srcFile, string dstFile)
        {
            Console.WriteLine("{0} -> {1}", srcFile, dstFile);

            srcFile = Path.Combine(BASE_PATH, srcFile);
            dstFile = Path.Combine(SETUP_PATH, dstFile);

            File.Copy(srcFile, dstFile, true);
        }

        private static void _CopyFiles(string srcFile, string dstPath = "")
        {
            Console.WriteLine("{0} -> {1}", srcFile, dstPath);

            srcFile = Path.Combine(BASE_PATH, srcFile);
            dstPath = Path.Combine(SETUP_PATH, dstPath);

            string dir = Path.GetDirectoryName(srcFile);
            string pattern = Path.GetFileName(srcFile);

            if (!Directory.Exists(dir))
                return;

            PathUtility.CreateDirectoryIfNotExists(dstPath);

            foreach (string file in Directory.GetFiles(dir, pattern))
            {
                File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)), true);
            }
        }

        private static void _CopyDirectory(string srcDir, string dstDir)
        {
            Console.WriteLine("{0} -> {1}", srcDir, dstDir);

            srcDir = Path.Combine(BASE_PATH, srcDir);
            dstDir = Path.Combine(SETUP_PATH, dstDir);

            PathUtility.CopyDirectory(srcDir, dstDir, true);
        }

        private static void _DeleteFiles(string pattern)
        {
            foreach (string file in Directory.GetFiles(SETUP_PATH, pattern, SearchOption.AllDirectories))
            {
                File.Delete(file);
            }
        }
    }
}
