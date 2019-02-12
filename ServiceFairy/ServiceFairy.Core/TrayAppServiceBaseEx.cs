﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using Common.Package.Service;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Utility;
using System.IO;
using System.Reflection;
using ServiceFairy.Entities;
using Common.Contracts;
using Common.Framework.TrayPlatform;

namespace ServiceFairy
{
    /// <summary>
    /// TrayAppService的基类
    /// </summary>
    public abstract class TrayAppServiceBaseEx : TrayAppServiceBase
    {
        public TrayAppServiceBaseEx()
        {
            
        }

        protected override void OnInit(AppServiceInfo info)
        {
            base.OnInit(info);

            AppComponentManager.AddRange(new IAppComponent[] {
                SysAppComponent = new SysAppCommandManagerComponent(this),
                AppMessageManager = new AppMessageManagerComponent(this),
                AppStatusCodeManager = new AppStatusCodeManagerComponent(this),
            });

            ITrayPlatform pf = Context.Platform;
            RunningBasePath = pf.GetData(SFNames.DataKeys.RUNNING_BASE_PATH) as string ?? SFSettings.DefaultRunningPath;
            ServiceBasePath = pf.GetData(SFNames.DataKeys.SERVICE_BASE_PATH) as string ?? SFSettings.DefaultServicePath;
            DeployPackagePath = pf.GetData(SFNames.DataKeys.DEPLOY_PACKAGE_PATH) as string ?? SFSettings.DefaultDeployPackagePath;
            InstallPath = pf.GetData(SFNames.DataKeys.INSTALL_PATH) as string ?? PathUtility.GetExecutePath();
            DataPath = pf.GetData(SFNames.DataKeys.DATA_PATH) as string ?? SFSettings.DefaultDataPath;
            LogPath = pf.GetData(SFNames.DataKeys.LOG_PATH) as string ?? SFSettings.DefaultLogPath;
            ServiceDataPath = Path.Combine(DataPath, Context.ServiceDesc.Name);
        }

        /// <summary>
        /// 从程序集中加载消息
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="filter"></param>
        public void LoadAppMessagesFromAssembly(Assembly assembly, Func<AppMessageInfo, bool> filter)
        {
            Contract.Requires(assembly != null);

            var dict = assembly.SearchTypes<AppMessageInfo, MessageAttribute>((attrs, type) =>
                AppMessageInfo.FromPrototy(attrs[0].ToAppMessageInfo(type), DocUtility.GetSummary(type), DocUtility.GetRemarks(type))
            );
            AppMessageManager.Add(dict.Keys.Where(filter ?? ((info) => true)));
        }

        /// <summary>
        /// 从程序集中加载当前服务的消息
        /// </summary>
        /// <param name="assembly"></param>
        public void LoadAppMessagesFromAssembly(Assembly assembly)
        {
            ServiceDesc sd = Context.ServiceDesc;
            LoadAppMessagesFromAssembly(assembly, (info) => ServiceDesc.Compatible(sd, info.ServiceDesc));
        }

        /// <summary>
        /// 从程序集中加载状态码
        /// </summary>
        /// <param name="type"></param>
        public void LoadStatusCodeInfosFromType(Type type)
        {
            Contract.Requires(type != null);

            AppStatusCodeManager.Add(PackageUtility.LoadStatusCodeInfosFromType(type));
        }

        /// <summary>
        /// 用于提供一组系统接口的组件
        /// </summary>
        public SysAppCommandManagerComponent SysAppComponent { get; private set; }

        /// <summary>
        /// 消息信息管理器
        /// </summary>
        public AppMessageManagerComponent AppMessageManager { get; private set; }

        /// <summary>
        /// 状态码管理器
        /// </summary>
        public AppStatusCodeManagerComponent AppStatusCodeManager { get; private set; }

        /// <summary>
        /// 运行基路径
        /// </summary>
        public string RunningBasePath { get; private set; }

        /// <summary>
        /// 服务基路径
        /// </summary>
        public string ServiceBasePath { get; private set; }

        /// <summary>
        /// 安装包路径
        /// </summary>
        public string DeployPackagePath { get; private set; }

        /// <summary>
        /// 数据的路径
        /// </summary>
        public string DataPath { get; private set; }

        /// <summary>
        /// 日志路径
        /// </summary>
        public string LogPath { get; private set; }

        /// <summary>
        /// 服务的数据路径
        /// </summary>
        public string ServiceDataPath { get; private set; }

        /// <summary>
        /// 安装路径
        /// </summary>
        public string InstallPath { get; private set; }
    }
}