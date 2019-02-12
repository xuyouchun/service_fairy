using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;
using System.Threading;
using Common.Package;
using System.IO;
using Common.Communication.Wcf;
using System.Diagnostics.Contracts;

namespace Common.Framework.TrayPlatform
{
    partial class TrayAppServiceManager
    {
        class TrayPlatform : MarshalByRefObjectEx, ITrayPlatform, IDisposable
        {
            public TrayPlatform(TrayAppServiceManager owner, TrayAppServiceInfo info)
            {
                _owner = owner;
                _info = info;
            }

            private readonly TrayAppServiceManager _owner;
            private TrayAppServiceInfo _info;

            public void SetData(string key, object value)
            {
                _owner.SetData(key, value);
            }

            public object GetData(string key)
            {
                return _owner.GetData(key);
            }

            public void StatusChangedNotify(AppServiceStatus status)
            {
                _info.Status = status;
            }

            public ServiceInfo LoadService(string assemblyFile, string configuration, bool autoStart)
            {
                TrayAppServiceInfo sInfo = _owner.LoadService(assemblyFile, configuration);
                if (autoStart)
                {
                    ThreadPool.QueueUserWorkItem(delegate {
                        try
                        {
                            sInfo.Service.Start();
                        }
                        catch (Exception ex)
                        {
                            LogManager.LogError(ex);
                        }
                    });
                }

                return new ServiceInfo() { ServiceDesc = sInfo.AppServiceInfo.ServiceDesc, Status = sInfo.Status, Title = sInfo.AppServiceInfo.Title };
            }

            public bool UnloadService(ServiceDesc serviceDesc)
            {
                return _owner.UnloadService(serviceDesc);
            }

            private IAppService _GetAppService(ServiceDesc serviceDesc, bool throwError = true)
            {
                IAppService service = _owner.GetService(serviceDesc.Name, serviceDesc.Version);
                if (service == null && throwError)
                    throw new InvalidOperationException("服务不存在：" + serviceDesc);

                return service;
            }

            public void SetServiceStatus(ServiceDesc serviceDesc, AppServiceStatus status)
            {
                IAppService service = _GetAppService(serviceDesc);

                if (status == AppServiceStatus.Running)
                {
                    if (service.Status != AppServiceStatus.Running)
                        service.Start();
                }
                else if (status == AppServiceStatus.Init)
                {
                    if (service.Status == AppServiceStatus.Running)
                        service.Stop();
                }
                else
                {
                    throw new InvalidOperationException("服务不允许设置为该状态：" + status.GetDesc());
                }
            }

            public void SetServiceAvaliable(ServiceDesc serviceDesc, bool avaliable)
            {
                TrayAppServiceInfo info = _owner.GetServiceInfo(serviceDesc);
                if (info == null)
                    throw new InvalidOperationException("服务不存在：" + serviceDesc);

                info.Avaliable = avaliable;
            }

            public void StartListener(CommunicationOption option)
            {
                _owner._callback.StartListener(option);
            }

            public void UpdateInvokeInfos(AppInvokeInfo[] invokeInfos)
            {
                _owner._callback.UpdateInvokeInfos(invokeInfos);
            }

            public AppInvokeInfo[] GetInvokeInfos()
            {
                return _owner._callback.GetInvokeInfos();
            }

            public ServiceInfo[] GetAllServiceInfos()
            {
                return _owner.GetAllServiceInfos();
            }

            public CommunicationOption[] GetAllCommunicationOptions()
            {
                return _owner._callback.GetAllCommunicateOptions();
            }

            public Guid GetClientID()
            {
                return _owner.ClientID;
            }

            public void StopListener(ServiceAddress address)
            {
                _owner._callback.StopListener(address);
            }

            /// <summary>
            /// 重新启动平台
            /// </summary>
            /// <param name="commandName"></param>
            /// <param name="args"></param>
            public void DoCommand(string commandName, string[] args)
            {
                _owner._callback.DoCommand(commandName, args);
            }

            /// <summary>
            /// 获取运行目录
            /// </summary>
            /// <returns></returns>
            public string GetRunningPath()
            {
                return Path.GetDirectoryName(_info.AssemblyFile);
            }

            private Dictionary<Type, object[]> _handlerDict = new Dictionary<Type, object[]>();
            private readonly object _handlerDictLocker = new object();

            /// <summary>
            /// 注册一个句柄
            /// </summary>
            /// <param name="type"></param>
            /// <param name="handler"></param>
            public void RegisterHandler(Type type, object handler)
            {
                _owner._handlerManager.Register(this, type, handler);
            }

            /// <summary>
            /// 取消注册一个句柄
            /// </summary>
            /// <param name="type"></param>
            /// <param name="handler"></param>
            public void UnregisterHandler(Type type, object handler)
            {
                _owner._handlerManager.Unregister(this, type, handler);
            }

            public void Dispose()
            {
                _owner._handlerManager.Unregister(this);
            }
        }
    }
}
