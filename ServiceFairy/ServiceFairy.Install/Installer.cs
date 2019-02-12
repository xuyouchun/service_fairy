using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace ServiceFairy.Install
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        public Installer()
        {
            InitializeComponent();
        }

        private string _GetTargetDir()
        {
            string targetDir = Context.Parameters["targetdir"] ?? "";
            return targetDir.TrimEnd('\\');
        }

        /// <summary>
        /// 即将安装完成，执行初始化工作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Installer_Committing(object sender, InstallEventArgs e)
        {
            InstallerContext ctx = new InstallerContext(_GetTargetDir());

            ConfigurationInitializer cfgInitializer = new ConfigurationInitializer(ctx);
            cfgInitializer.Execute();

            InstallPathInitializer installPathInitializer = new InstallPathInitializer(ctx);
            installPathInitializer.Execute();
        }

        /// <summary>
        /// 已经提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Installer_Committed(object sender, InstallEventArgs e)
        {
            
        }

        /// <summary>
        /// 安装完毕，启动管理器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Installer_AfterInstall(object sender, InstallEventArgs e)
        {
            //string trayMgrPath = Path.Combine(_GetTargetDir(), "TrayMgr.exe");
            //Process.Start(trayMgrPath, "/first_run");
        }
    }
}
