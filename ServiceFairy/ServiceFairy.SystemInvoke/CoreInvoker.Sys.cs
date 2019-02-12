using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using System.Diagnostics.Contracts;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Sys;
using Common.Contracts.Service;
using Common.Contracts;
using Common;

namespace ServiceFairy.SystemInvoke
{
	partial class CoreInvoker
	{
        private SysInvoker _sys;

        /// <summary>
        /// Sys Service
        /// </summary>
        public SysInvoker Sys
        {
            get { return _sys ?? (_sys = new SysInvoker(this)); }
        }

        /// <summary>
        /// 系统调用
        /// </summary>
        public class SysInvoker : Invoker
        {
            public SysInvoker(CoreInvoker owner)
                : base(owner)
            {
                
            }

            private static CallingSettings _CreateSettings(Guid clientId, ServiceDesc serviceDesc, Sid sid = default(Sid))
            {
                Contract.Requires(serviceDesc != null);

                return CallingSettings.FromTarget(clientId, serviceDesc, sid: sid);
            }

            /// <summary>
            /// 将事件通知订阅者
            /// </summary>
            /// <param name="source">事件源</param>
            /// <param name="eventName">事件名称</param>
            /// <param name="eventArgs">事件参数</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult OnEventSr(ServiceEndPoint source, string eventName, byte[] eventArgs, CallingSettings settings = null)
            {
                return SysServices.OnEvent(Sc, new Sys_OnEvent_Request() {
                    Source = source, EventName = eventName, EventArgs = eventArgs
                }, settings);
            }

            /// <summary>
            /// 将事件通知订阅者
            /// </summary>
            /// <param name="source">事件源</param>
            /// <param name="eventName">事件名称</param>
            /// <param name="eventArgs">事件参数</param>
            /// <param name="settings">调用设置</param>
            public void OnEvent(ServiceEndPoint source, string eventName, byte[] eventArgs, CallingSettings settings = null)
            {
                InvokeWithCheck(OnEventSr(source, eventName, eventArgs, settings));
            }

            /// <summary>
            /// 插件调用
            /// </summary>
            /// <param name="addinDesc">插件</param>
            /// <param name="method">接口</param>
            /// <param name="argument">参数</param>
            /// <param name="settings">调用设置</param>
            /// <returns>结果</returns>
            public ServiceResult<CommunicateData> AddinCallSr(AddinDesc addinDesc, string method, CommunicateData argument, CallingSettings settings = null)
            {
                var sr = SysServices.AddinCall(Sc,
                    new Sys_OnAddinCall_Request() { AddinDesc = addinDesc, Method = method, Argument = argument }, settings);

                return CreateSr(sr, r => r.ReturnValue);
            }

            /// <summary>
            /// 插件调用
            /// </summary>
            /// <param name="addinDesc">插件</param>
            /// <param name="method">接口</param>
            /// <param name="argument">参数</param>
            /// <param name="settings">调用设置</param>
            /// <returns>结果</returns>
            public CommunicateData AddinCall(AddinDesc addinDesc, string method, CommunicateData argument, CallingSettings settings = null)
            {
                return InvokeWithCheck(AddinCallSr(addinDesc, method, argument, settings));
            }

            /// <summary>
            /// 获取所有插件的信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<AppServiceAddinInfoItem[]> GetAddinInfosSr(CallingSettings settings = null)
            {
                var sr = SysServices.GetAddinsInfos(Sc, settings);
                return CreateSr(sr, r => r.AddinInfos);
            }

            /// <summary>
            /// 获取所有插件的信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public AppServiceAddinInfoItem[] GetAddinInfos(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetAddinInfosSr(settings));
            }

            /// <summary>
            /// 获取所有插件的信息
            /// </summary>
            /// <param name="clientId">服务终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="sid">安全码</param>
            /// <returns>插件信息</returns>
            public ServiceResult<AppServiceAddinInfoItem[]> GetAddinInfosSr(Guid clientId, ServiceDesc serviceDesc, Sid sid = default(Sid))
            {
                return GetAddinInfosSr(_CreateSettings(clientId, serviceDesc, sid));
            }

            /// <summary>
            /// 获取所有插件的信息
            /// </summary>
            /// <param name="clientId">服务终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="sid">安全码</param>
            /// <returns>插件信息</returns>
            public AppServiceAddinInfoItem[] GetAddinInfos(Guid clientId, ServiceDesc serviceDesc, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetAddinInfosSr(clientId, serviceDesc, sid));
            }

            /// <summary>
            /// 测试接口是否可用
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult HelloSr(CallingSettings settings)
            {
                return SysServices.Hello(Sc, settings);
            }

            /// <summary>
            /// 测试接口是否可用
            /// </summary>
            /// <param name="settings">调用设置</param>
            public void Hello(CallingSettings settings)
            {
                InvokeWithCheck(HelloSr(settings));
            }

            /// <summary>
            /// 获取所有接口的信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<AppCommandInfo[]> GetAppCommandInfosSr(CallingSettings settings)
            {
                var sr = SysServices.GetAppCommandInfos(Sc, settings);
                return CreateSr(sr, r => r.AppCommandInfos);
            }

            /// <summary>
            /// 获取所有接口的信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public AppCommandInfo[] GetAppCommandInfos(CallingSettings settings)
            {
                return InvokeWithCheck(GetAppCommandInfosSr(settings));
            }

            /// <summary>
            /// 获取指定终端所有接口的信息
            /// </summary>
            /// <param name="clientId">服务终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="sid">安全码</param>
            /// <returns>接口信息</returns>
            public ServiceResult<AppCommandInfo[]> GetAppCommandInfosSr(Guid clientId, ServiceDesc serviceDesc, Sid sid = default(Sid))
            {
                return GetAppCommandInfosSr(_CreateSettings(clientId, serviceDesc, sid));
            }

            /// <summary>
            /// 获取指定终端的所有接口的信息
            /// </summary>
            /// <param name="clientId">服务终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="sid">安全码</param>
            /// <returns>接口信息</returns>
            public AppCommandInfo[] GetAppCommandInfos(Guid clientId, ServiceDesc serviceDesc, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetAppCommandInfosSr(clientId, serviceDesc, sid));
            }

            /// <summary>
            /// 获取指定终端指定服务的消息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns>消息描述</returns>
            public ServiceResult<AppMessageInfo[]> GetAppMessageInfosSr(CallingSettings settings)
            {
                var sr = SysServices.GetAppMessageInfos(Sc, settings);
                return CreateSr(sr, r => r.Infos);
            }

            /// <summary>
            /// 获取指定终端指定服务的消息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns>消息描述</returns>
            public AppMessageInfo[] GetAppMessageInfos(CallingSettings settings)
            {
                return InvokeWithCheck(GetAppMessageInfosSr(settings));
            }

            /// <summary>
            /// 获取指定终端指定服务的消息
            /// </summary>
            /// <param name="clientId">服务终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="sid">安全码</param>
            /// <returns>消息描述</returns>
            public ServiceResult<AppMessageInfo[]> GetAppMessageInfosSr(Guid clientId, ServiceDesc serviceDesc, Sid sid = default(Sid))
            {
                return GetAppMessageInfosSr(_CreateSettings(clientId, serviceDesc, sid));
            }

            /// <summary>
            /// 获取指定终端指定服务的消息
            /// </summary>
            /// <param name="clientId">服务终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="sid">安全码</param>
            /// <returns>消息描述</returns>
            public AppMessageInfo[] GetAppMessageInfos(Guid clientId, ServiceDesc serviceDesc, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetAppMessageInfosSr(clientId, serviceDesc, sid));
            }

            /// <summary>
            /// 获取消息参数示例及文档
            /// </summary>
            /// <param name="msgs">消息</param>
            /// <param name="formats">示例文档格式</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<AppMessageDoc[]> GetAppMessageDocsSr(MessageDesc[] msgs, DataFormat[] formats, CallingSettings settings)
            {
                var sr = SysServices.GetAppMessageDocs(Sc, new Sys_GetAppMessageDocs_Request { MessageDescs = msgs, Formats = formats }, settings);
                return CreateSr(sr, r => r.Docs);
            }

            /// <summary>
            /// 获取消息参数示例及文档
            /// </summary>
            /// <param name="msgs">消息</param>
            /// <param name="formats">示例文档格式</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public AppMessageDoc[] GetAppMessageDocs(MessageDesc[] msgs, DataFormat[] formats, CallingSettings settings)
            {
                return InvokeWithCheck(GetAppMessageDocsSr(msgs, formats, settings));
            }

            /// <summary>
            /// 获取消息参数示例及文档
            /// </summary>
            /// <param name="msgs">消息</param>
            /// <param name="formats">示例文档格式</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<AppMessageDoc[]> GetAppMessageDocsSr(MessageDesc[] msgs, DataFormat[] formats,
                Guid clientId, ServiceDesc serviceDesc, Sid sid = default(Sid))
            {
                return GetAppMessageDocsSr(msgs, formats, _CreateSettings(clientId, serviceDesc, sid));
            }

            /// <summary>
            /// 获取消息参数示例及文档
            /// </summary>
            /// <param name="msgs">消息</param>
            /// <param name="formats">示例文档格式</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public AppMessageDoc[] GetAppMessageDocs(MessageDesc[] msgs, DataFormat[] formats,
                Guid clientId, ServiceDesc serviceDesc, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetAppMessageDocsSr(msgs, formats, clientId, serviceDesc, sid));
            }

            /// <summary>
            /// 获取消息参数示例及文档
            /// </summary>
            /// <param name="msg">消息</param>
            /// <param name="formats">示例文档格式</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<AppMessageDoc> GetAppMessageDocSr(MessageDesc msg, DataFormat[] formats,
                CallingSettings settings)
            {
                var sr = GetAppMessageDocsSr(new[] { msg }, formats, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取消息参数示例及文档
            /// </summary>
            /// <param name="msg">消息</param>
            /// <param name="formats">示例文档格式</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public AppMessageDoc GetAppMessageDoc(MessageDesc msg, DataFormat[] formats, CallingSettings settings)
            {
                return InvokeWithCheck(GetAppMessageDocSr(msg, formats, settings));
            }

            /// <summary>
            /// 获取消息参数示例及文档
            /// </summary>
            /// <param name="msg">消息</param>
            /// <param name="formats">示例文档格式</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult<AppMessageDoc> GetAppMessageDocSr(MessageDesc msg, DataFormat[] formats,
                Guid clientId, ServiceDesc serviceDesc, Sid sid = default(Sid))
            {
                return GetAppMessageDocSr(msg, formats, _CreateSettings(clientId, serviceDesc, sid));
            }

            /// <summary>
            /// 获取消息参数示例及文档
            /// </summary>
            /// <param name="msg">消息</param>
            /// <param name="formats">示例文档格式</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public AppMessageDoc GetAppMessageDoc(MessageDesc msg, DataFormat[] formats, Guid clientId,
                ServiceDesc serviceDesc, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetAppMessageDocSr(msg, formats, clientId, serviceDesc, sid));
            }

            /// <summary>
            /// 获取状态码信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns>状态码描述</returns>
            public ServiceResult<AppStatusCodeInfo[]> GetAppStatusCodeInfosSr(CallingSettings settings)
            {
                var sr = SysServices.GetAppStatusCodeInfos(Sc, settings);
                return CreateSr(sr, r => r.Infos);
            }

            /// <summary>
            /// 获取状态码信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns>状态码描述</returns>
            public AppStatusCodeInfo[] GetAppStatusCodeInfos(CallingSettings settings)
            {
                return InvokeWithCheck(GetAppStatusCodeInfosSr(settings));
            }

            /// <summary>
            /// 获取状态码信息
            /// </summary>
            /// <param name="clientId">服务终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="sid">安全码</param>
            /// <returns>状态码描述</returns>
            public ServiceResult<AppStatusCodeInfo[]> GetAppStatusCodeInfosSr(Guid clientId, 
                ServiceDesc serviceDesc, Sid sid = default(Sid))
            {
                return GetAppStatusCodeInfosSr(_CreateSettings(clientId, serviceDesc, sid));
            }

            /// <summary>
            /// 获取状态码信息
            /// </summary>
            /// <param name="clientId">服务终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="sid">安全码</param>
            /// <returns>状态码描述</returns>
            public AppStatusCodeInfo[] GetAppStatusCodeInfos(Guid clientId, ServiceDesc serviceDesc, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetAppStatusCodeInfosSr(clientId, serviceDesc, sid));
            }

            /// <summary>
            /// 获取所有组件的信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns>组件描述信息</returns>
            public ServiceResult<AppComponentInfo[]> GetAppComponentInfosSr(CallingSettings settings)
            {
                var sr = SysServices.GetAppComponentInfos(Sc, settings);
                return CreateSr(sr, r => r.AppComponentInfos);
            }

            /// <summary>
            /// 获取所有组件信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns>组件描述信息</returns>
            public AppComponentInfo[] GetAppComponentInfos(CallingSettings settings)
            {
                return InvokeWithCheck(GetAppComponentInfosSr(settings));
            }

            /// <summary>
            /// 获取所有组件的信息
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="sid">安全码</param>
            /// <returns>组件描述信息</returns>
            public ServiceResult<AppComponentInfo[]> GetAppComponentInfosSr(Guid clientId, ServiceDesc serviceDesc, Sid sid = default(Sid))
            {
                Contract.Requires(clientId != default(Guid) && serviceDesc != null);
                return GetAppComponentInfosSr(_CreateSettings(clientId, serviceDesc, sid: sid));
            }

            /// <summary>
            /// 获取所有组件的信息
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="sid">安全码</param>
            /// <returns>组件描述信息</returns>
            public AppComponentInfo[] GetAppComponentInfos(Guid clientId, ServiceDesc serviceDesc, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetAppComponentInfosSr(clientId, serviceDesc, sid));
            }

            /// <summary>
            /// 获取接口参数信息
            /// </summary>
            /// <param name="commandDescs">接口</param>
            /// <param name="formats">数据格式</param>
            /// <param name="settings">调用设置</param>
            /// <returns>接口文档</returns>
            public ServiceResult<AppCommandDoc[]> GetAppCommandDocsSr(CommandDesc[] commandDescs,
                DataFormat[] formats = null, CallingSettings settings = null)
            {
                var sr = SysServices.GetAppCommandDocs(Sc, new Sys_GetAppCommandDocs_Request() { CommandDescs = commandDescs, Formats = formats }, settings);
                return CreateSr(sr, r => r.Docs);
            }

            /// <summary>
            /// 获取接口参数信息
            /// </summary>
            /// <param name="commandDescs">接口</param>
            /// <param name="formats">数据格式</param>
            /// <param name="settings">调用设置</param>
            /// <returns>接口文档</returns>
            public AppCommandDoc[] GetAppCommandDocs(CommandDesc[] commandDescs, DataFormat[] formats = null,
                CallingSettings settings = null)
            {
                return InvokeWithCheck(GetAppCommandDocsSr(commandDescs, formats, settings));
            }

            /// <summary>
            /// 获取接口参数信息
            /// </summary>
            /// <param name="commandDesc">接口</param>
            /// <param name="formats">数据格式</param>
            /// <param name="settings">调用设置</param>
            /// <returns>接口文档</returns>
            public ServiceResult<AppCommandDoc> GetAppCommandDocSr(CommandDesc commandDesc, DataFormat[] formats = null, CallingSettings settings = null)
            {
                var sr = GetAppCommandDocsSr(new[] { commandDesc }, formats, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取接口参数信息
            /// </summary>
            /// <param name="commandDesc">接口</param>
            /// <param name="formats">数据格式</param>
            /// <param name="settings">调用设置</param>
            /// <returns>接口文档</returns>
            public AppCommandDoc GetAppCommandDoc(CommandDesc commandDesc, DataFormat[] formats = null, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetAppCommandDocSr(commandDesc, formats, settings));
            }

            /// <summary>
            /// 获取接口参数信息
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="formats">数据格式</param>
            /// <param name="commandDescs">接口</param>
            /// <param name="sid">安全码</param>
            /// <returns>接口文档</returns>
            public ServiceResult<AppCommandDoc[]> GetAppCommandDocsSr(Guid clientId, ServiceDesc serviceDesc,
                CommandDesc[] commandDescs, DataFormat[] formats = null, Sid sid = default(Sid))
            {
                Contract.Requires(clientId != Guid.Empty && serviceDesc != null);

                return GetAppCommandDocsSr(commandDescs, formats, _CreateSettings(clientId, serviceDesc, sid: sid));
            }

            /// <summary>
            /// 获取接口参数信息
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="commandDescs">接口</param>
            /// <param name="formats">数据格式</param>
            /// <param name="sid">安全码</param>
            /// <returns>接口文档</returns>
            public AppCommandDoc[] GetAppCommandDocs(Guid clientId, ServiceDesc serviceDesc, CommandDesc[] commandDescs,
                DataFormat[] formats = null, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetAppCommandDocsSr(clientId, serviceDesc, commandDescs, formats, sid));
            }

            /// <summary>
            /// 获取接口参数信息
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="commandDesc">接口</param>
            /// <param name="formats">数据格式</param>
            /// <param name="sid">安全码</param>
            /// <returns>接口文档</returns>
            public ServiceResult<AppCommandDoc> GetAppCommandDocSr(Guid clientId, ServiceDesc serviceDesc,
                CommandDesc commandDesc, DataFormat[] formats = null, Sid sid = default(Sid))
            {
                var sr = GetAppCommandDocsSr(clientId, serviceDesc, new[] { commandDesc }, formats, sid);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取接口参数信息
            /// </summary>
            /// <param name="clientId">服务终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="commandDesc">接口</param>
            /// <param name="formats">数据格式</param>
            /// <returns>接口文档</returns>
            public AppCommandDoc GetAppCommandDoc(Guid clientId, ServiceDesc serviceDesc, CommandDesc commandDesc,
                DataFormat[] formats, Sid sid = default(Sid))
            {
                return InvokeWithCheck(GetAppCommandDocSr(clientId, serviceDesc, commandDesc, formats, sid));
            }

            /// <summary>
            /// 获取组件的属性
            /// </summary>
            /// <param name="componentNames">组件名称</param>
            /// <param name="settings">调用设置</param>
            /// <returns>属性值</returns>
            public ServiceResult<ObjectPropertyGroup[]> GetAppComponentPropertiesSr(string[] componentNames, CallingSettings settings)
            {
                var sr = SysServices.GetAppComponentProperties(Sc,
                    new Sys_GetAppComponentProperties_Request() { ComponentNames = componentNames }, settings);

                return CreateSr(sr, r => r.PropertyGroups);
            }

            /// <summary>
            /// 获取组件的属性
            /// </summary>
            /// <param name="componentNames">组件名称</param>
            /// <param name="settings">调用设置</param>
            /// <returns>属性</returns>
            public ObjectPropertyGroup[] GetAppComponentProperties(string[] componentNames, CallingSettings settings)
            {
                return InvokeWithCheck(GetAppComponentPropertiesSr(componentNames, settings));
            }

            /// <summary>
            /// 获取组件的属性
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="componentNames">组件名称</param>
            /// <returns></returns>
            public ServiceResult<ObjectPropertyGroup[]> GetAppComponentPropertiesSr(Guid clientId, ServiceDesc serviceDesc,
                string[] componentNames)
            {
                return GetAppComponentPropertiesSr(componentNames, _CreateSettings(clientId, serviceDesc));
            }

            /// <summary>
            /// 获取组件的属性
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param> 
            /// <param name="componentNames">组件名称</param>
            /// <returns></returns>
            public ObjectPropertyGroup[] GetAppComponentProperties(Guid clientId, ServiceDesc serviceDesc, string[] componentNames)
            {
                return InvokeWithCheck(GetAppComponentPropertiesSr(clientId, serviceDesc, componentNames));
            }

            /// <summary>
            /// 获取组件的属性
            /// </summary>
            /// <param name="clientId">终端Id</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="componentName">组件名称</param>
            /// <returns></returns>
            public ServiceResult<ObjectProperty[]> GetAppComponentPropertiesSr(Guid clientId, ServiceDesc serviceDesc,
                string componentName)
            {
                var sr = GetAppComponentPropertiesSr(clientId, serviceDesc, new[] { componentName });
                return CreateSr(sr, r => r.Length == 0 ? Array<ObjectProperty>.Empty : r.FirstOrDefault().Properties, Array<ObjectProperty>.Empty);
            }

            /// <summary>
            /// 获取组件的属性
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="componentName">组件名称</param>
            /// <returns></returns>
            public ObjectProperty[] GetAppComponentProperties(Guid clientId, ServiceDesc serviceDesc, string componentName)
            {
                return InvokeWithCheck(GetAppComponentPropertiesSr(clientId, serviceDesc, componentName));
            }

            /// <summary>
            /// 获取组件属性值          
            /// </summary>
            /// <param name="componentNames">组件名称</param>
            /// <param name="settings">调用设置</param>
            /// <returns>属性值</returns>
            public ServiceResult<AppComponentPropertyValueGroup[]> GetAppComponentPropertyValuesSr(string[] componentNames,
                CallingSettings settings)
            {
                var sr = SysServices.GetAppComponentPropertyValues(Sc,
                    new Sys_GetAppComponentPropertyValues_Request() { ComponentNames = componentNames }, settings);
                return CreateSr(sr, r => r.ValueGroups);
            }

            /// <summary>
            /// 获取组件属性值
            /// </summary>
            /// <param name="componentNames">组件名称</param>
            /// <param name="settings">调用设置</param>
            /// <returns>属性值</returns>
            public AppComponentPropertyValueGroup[] GetAppComponentPropertyValues(string[] componentNames,
                CallingSettings settings)
            {
                return InvokeWithCheck(GetAppComponentPropertyValuesSr(componentNames, settings));
            }

            /// <summary>
            /// 获取组件属性值
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="componentNames">组件名称</param>
            /// <returns>属性值</returns>
            public ServiceResult<AppComponentPropertyValueGroup[]> GetAppComponentPropertyValuesSr(Guid clientId,
                ServiceDesc serviceDesc, string[] componentNames)
            {
                return GetAppComponentPropertyValuesSr(componentNames, CallingSettings.FromTarget(clientId, serviceDesc));
            }

            /// <summary>
            /// 获取组件属性值
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="componentNames">组件名称</param>
            /// <returns>属性值</returns>
            public AppComponentPropertyValueGroup[] GetAppComponentPropertyValues(Guid clientId, ServiceDesc serviceDesc,
                string[] componentNames)
            {
                return InvokeWithCheck(GetAppComponentPropertyValuesSr(clientId, serviceDesc, componentNames));
            }

            /// <summary>
            /// 获取组件属性值
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="componentName">组件名称</param>
            /// <returns>属性值</returns>
            public ServiceResult<AppComponentPropertyValue[]> GetAppComponentPropertyValuesSr(Guid clientId,
                ServiceDesc serviceDesc, string componentName)
            {
                var sr = GetAppComponentPropertyValuesSr(clientId, serviceDesc, new[] { componentName });
                var empty = Array<AppComponentPropertyValue>.Empty;
                return CreateSr(sr, r => r.Length == 0 ? empty : r[0].Values, empty);
            }

            /// <summary>
            /// 获取组件属性值
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="componentName">组件名称</param>
            /// <returns>属性值</returns>
            public AppComponentPropertyValue[] GetAppComponentPropertyValues(Guid clientId, ServiceDesc serviceDesc,
                string componentName)
            {
                return InvokeWithCheck(GetAppComponentPropertyValuesSr(clientId, serviceDesc, componentName));
            }

            /// <summary>
            /// 设置指定组件指定属性的值
            /// </summary>
            /// <param name="groups">属性组</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SetAppComponentPropertyValuesSr(AppComponentPropertyValueGroup[] groups,
                CallingSettings settings = null)
            {
                return SysServices.SetAppComponentPropertyValues(Sc,
                    new Sys_SetAppComponentPropertyValues_Request() { ValueGroups = groups }, settings);
            }

            /// <summary>
            /// 设置指定组件指定属性的值
            /// </summary>
            /// <param name="groups">属性组</param>
            /// <param name="settings">调用设置</param>
            public void SetAppComponentPropertyValues(AppComponentPropertyValueGroup[] groups,
                CallingSettings settings = null)
            {
                InvokeWithCheck(SetAppComponentPropertyValuesSr(groups, settings));
            }

            /// <summary>
            /// 设置指定组件指定属性的值
            /// </summary>
            /// <param name="group">属性组</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SetAppComponentPropertyValuesSr(AppComponentPropertyValueGroup group,
                CallingSettings settings = null)
            {
                return SetAppComponentPropertyValuesSr(new[] { group }, settings);
            }

            /// <summary>
            /// 设置指定组件指定属性的值
            /// </summary>
            /// <param name="group">属性组</param>
            /// <param name="settings">调用设置</param>
            public void SetAppComponentPropertyValues(AppComponentPropertyValueGroup group,
                CallingSettings settings = null)
            {
                InvokeWithCheck(SetAppComponentPropertyValuesSr(group, settings));
            }

            /// <summary>
            /// 设置指定组件指定属性的值
            /// </summary>
            /// <param name="group">属性组</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            /// <returns></returns>
            public ServiceResult SetAppComponentPropertyValuesSr(AppComponentPropertyValueGroup group,
                Guid clientId, Sid sid = default(Sid))
            {
                return SetAppComponentPropertyValuesSr(group, CallingSettings.FromTarget(clientId, sid: sid));
            }

            /// <summary>
            /// 设置指定组件指定属性值
            /// </summary>
            /// <param name="group">属性组</param>
            /// <param name="clientId">终端ID</param>
            /// <param name="sid">安全码</param>
            public void SetAppComponentPropertyValues(AppComponentPropertyValueGroup group,
                Guid clientId, Sid sid = default(Sid))
            {
                InvokeWithCheck(SetAppComponentPropertyValuesSr(group, clientId, sid));
            }

            /// <summary>
            /// 获取服务基路径中的所有文件信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns>文件信息</returns>
            public ServiceResult<AppFileInfo[]> GetAppServiceFileInfosSr(CallingSettings settings)
            {
                var sr = SysServices.GetAppServiceFileInfos(Sc, settings);
                return CreateSr(sr, r => r.FileInfos);
            }

            /// <summary>
            /// 获取服务基路径中的所有文件信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns>文件信息</returns>
            public AppFileInfo[] GetAppServiceFileInfos(CallingSettings settings)
            {
                return InvokeWithCheck(GetAppServiceFileInfosSr(settings));
            }

            /// <summary>
            /// 获取服务基路径中的所有文件信息
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <returns>文件信息</returns>
            public ServiceResult<AppFileInfo[]> GetAppServiceFileInfosSr(Guid clientId, ServiceDesc serviceDesc)
            {
                return GetAppServiceFileInfosSr(CallingSettings.FromTarget(clientId, serviceDesc));
            }

            /// <summary>
            /// 获取服务基路径中的所有文件信息
            /// </summary>
            /// <param name="clientId">终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <returns>文件信息</returns>
            public AppFileInfo[] GetAppServiceFileInfos(Guid clientId, ServiceDesc serviceDesc)
            {
                return InvokeWithCheck(GetAppServiceFileInfosSr(clientId, serviceDesc));
            }

            /// <summary>
            /// 下载服务的文件
            /// </summary>
            /// <param name="fileNames">文件路径</param>
            /// <param name="settings">调用设置</param>
            /// <returns>文件数据</returns>
            public ServiceResult<AppServiceFileData[]> DownloadAppServiceFilesSr(string[] fileNames, CallingSettings settings)
            {
                var sr = SysServices.DownloadAppServiceFiles(Sc,
                    new Sys_DownloadAppServiceFiles_Request() { FileNames = fileNames }, settings);

                return CreateSr(sr, r => r.Files);
            }

            /// <summary>
            /// 下载服务的文件
            /// </summary>
            /// <param name="fileNames">文件路径</param>
            /// <param name="settings">调用设置</param>
            /// <returns>文件数据</returns>
            public AppServiceFileData[] DownloadAppServiceFiles(string[] fileNames, CallingSettings settings)
            {
                return InvokeWithCheck(DownloadAppServiceFilesSr(fileNames, settings));
            }

            /// <summary>
            /// 下载服务的文件
            /// </summary>
            /// <param name="fileName">文件路径</param>
            /// <param name="settings">调用设置</param>
            /// <returns>文件数据</returns>
            public ServiceResult<AppServiceFileData> DownloadAppServiceFileSr(string fileName, CallingSettings settings)
            {
                var sr = DownloadAppServiceFilesSr(new[] { fileName }, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 下载服务的文件
            /// </summary>
            /// <param name="fileName">文件路径</param>
            /// <param name="settings">调用设置</param>
            /// <returns>文件数据</returns>
            public AppServiceFileData DownloadAppServiceFile(string fileName, CallingSettings settings)
            {
                return InvokeWithCheck(DownloadAppServiceFileSr(fileName, settings));
            }

            /// <summary>
            /// 下载服务的文件
            /// </summary>
            /// <param name="fileNames">文件路径</param>
            /// <param name="clientId">服务终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <returns>文件数据</returns>
            public ServiceResult<AppServiceFileData[]> DownloadAppServiceFilesSr(string[] fileNames,
                Guid clientId, ServiceDesc serviceDesc)
            {
                return DownloadAppServiceFilesSr(fileNames, _CreateSettings(clientId, serviceDesc));
            }

            /// <summary>
            /// 下载服务的文件
            /// </summary>
            /// <param name="fileNames">文件路径</param>
            /// <param name="clientId">服务终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <returns>文件数据</returns>
            public AppServiceFileData[] DownloadAppServiceFiles(string[] fileNames, Guid clientId, ServiceDesc serviceDesc)
            {
                return InvokeWithCheck(DownloadAppServiceFilesSr(fileNames, clientId, serviceDesc));
            }

            /// <summary>
            /// 下载服务的文件
            /// </summary>
            /// <param name="fileName">文件路径</param>
            /// <param name="clientId">服务终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <returns>文件数据</returns>
            public ServiceResult<AppServiceFileData> DownloadAppServiceFileSr(string fileName, Guid clientId, ServiceDesc serviceDesc)
            {
                return DownloadAppServiceFileSr(fileName, _CreateSettings(clientId, serviceDesc));
            }

            /// <summary>
            /// 下载服务的文件
            /// </summary>
            /// <param name="fileName">文件路径</param>
            /// <param name="clientId">服务终端ID</param>
            /// <param name="serviceDesc">服务</param>
            /// <returns>文件数据</returns>
            public AppServiceFileData DownloadAppServiceFile(string fileName, Guid clientId, ServiceDesc serviceDesc)
            {
                return InvokeWithCheck(DownloadAppServiceFileSr(fileName, clientId, serviceDesc));
            }

            /// <summary>
            /// 判断是否包含指定的服务
            /// </summary>
            /// <param name="commandDescs">接口</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="sid">安全码</param>
            /// <returns>文件数据</returns>
            public ServiceResult<CommandDesc[]> ExistsCommandSr(CommandDesc[] commandDescs, ServiceDesc serviceDesc,
                Sid sid = default(Sid))
            {
                var sr = SysServices.ExistsCommand(Sc, new Sys_ExistsCommand_Request() { CommandDescs = commandDescs },
                    CallingSettings.FromTarget(serviceDesc, sid: sid, model: ServiceTargetModel.Auto));

                return CreateSr(sr, r => r.CommandDescs);
            }

            /// <summary>
            /// 判断是否包含指定的服务
            /// </summary>
            /// <param name="commandDescs">接口</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="sid">安全码</param>
            /// <returns>已包含的接口</returns>
            public CommandDesc[] ExistsCommand(CommandDesc[] commandDescs, ServiceDesc serviceDesc, Sid sid = default(Sid))
            {
                return InvokeWithCheck(ExistsCommandSr(commandDescs, serviceDesc, sid));
            }

            /// <summary>
            /// 判断是否包含指定的服务
            /// </summary>
            /// <param name="commandDesc">接口</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="sid">安全码</param>
            /// <returns>是否包含该接口</returns>
            public ServiceResult<bool> ExistsCommandSr(CommandDesc commandDesc, ServiceDesc serviceDesc, Sid sid = default(Sid))
            {
                var sr = ExistsCommandSr(new[] { commandDesc }, serviceDesc, sid);
                return CreateSr(sr, r => r.Length > 0);
            }

            /// <summary>
            /// 判断是否包含指定的服务
            /// </summary>
            /// <param name="commandDesc">接口</param>
            /// <param name="serviceDesc">服务</param>
            /// <param name="sid">安全码</param>
            /// <returns>是否包含该接口</returns>
            public bool ExistsCommand(CommandDesc commandDesc, ServiceDesc serviceDesc, Sid sid = default(Sid))
            {
                return InvokeWithCheck(ExistsCommandSr(commandDesc, serviceDesc, sid));
            }

            /// <summary>
            /// 判断是否包含指定的服务
            /// </summary>
            /// <param name="fullMethodName">全接口名</param>
            /// <param name="sid">安全码</param>
            /// <returns>是否包含该接口</returns>
            public ServiceResult<bool> ExistsCommandSr(string fullMethodName, Sid sid = default(Sid))
            {
                Contract.Requires(fullMethodName != null);

                MethodParser mp = new MethodParser(fullMethodName);
                return ExistsCommandSr(mp.CommandDesc, mp.ServiceDesc, sid);
            }

            /// <summary>
            /// 判断是否包含指定的服务
            /// </summary>
            /// <param name="fullMethodName">全接口名</param>
            /// <param name="sid">安全码</param>
            /// <returns>是否包含该接口</returns>
            public bool ExistsCommand(string fullMethodName, Sid sid = default(Sid))
            {
                return InvokeWithCheck(ExistsCommandSr(fullMethodName, sid));
            }
        }
	}
}