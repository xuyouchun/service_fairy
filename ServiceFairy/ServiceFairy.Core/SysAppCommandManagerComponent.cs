using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Entities.Sys;
using Common.Utility;
using Common.Package;
using Common.Contracts;
using System.Runtime.Serialization;
using Common;
using Common.Collection;
using System.IO;
using System.Reflection;

namespace ServiceFairy
{
    /// <summary>
    /// 用于提供一组系统接口的组件
    /// </summary>
    [AppComponent("系统接口加载器", "提供一组系统接口，系统接口皆以“Sys_”开头", category: AppComponentCategory.System, name: "Sys_SysAppCommandManager")]
    public partial class SysAppCommandManagerComponent : AppComponent
    {
        public SysAppCommandManagerComponent(TrayAppServiceBaseEx service)
            : base(service)
        {
            _service = service;
            _commandCollection = AppCommandCollection.LoadFromType(GetType(), new[] { this });
        }

        private readonly AppCommandCollection _commandCollection;
        private readonly TrayAppServiceBaseEx _service;

        protected override void OnStart()
        {
            base.OnStart();

            _service.AddCommands(_commandCollection.GetAppCommands());
        }

        protected override void OnStop()
        {
            base.OnStop();

            _service.RemoveCommands(_commandCollection.GetAppCommands());
        }

        /// <summary>
        /// 用于测试服务是否可用
        /// </summary>
        [AppCommand("Sys_Hello", "测试服务是否可用", AppCommandCategory.System, SecurityLevel = SecurityLevel.Public)]
        class HelloAppCommand : ACS<TrayAppServiceBaseEx>.Action
        {
            public HelloAppCommand(SysAppCommandManagerComponent component)
            {
                _component = component;
            }

            private readonly SysAppCommandManagerComponent _component;

            protected override void OnExecute(AppCommandExecuteContext<TrayAppServiceBaseEx> context, ref ServiceResult sr)
            {

            }
        }

        /// <summary>
        /// 获取所有的接口
        /// </summary>
        [AppCommand("Sys_GetAppCommandInfos", "获取所有的接口", AppCommandCategory.System, SecurityLevel = SecurityLevel.Admin)]
        class GetAppCommandInfosAppCommand : ACS<TrayAppServiceBaseEx>.Func<Sys_GetAppCommandInfos_Reply>
        {
            public GetAppCommandInfosAppCommand(SysAppCommandManagerComponent component)
            {
                _component = component;
            }

            private readonly SysAppCommandManagerComponent _component;

            protected override Sys_GetAppCommandInfos_Reply OnExecute(AppCommandExecuteContext<TrayAppServiceBaseEx> context, ref ServiceResult sr)
            {
                IAppCommand[] commands = ((TrayAppServiceBaseEx)context.Service).GetAllCommands();
                AppCommandInfo[] infos = commands.ToArray(cmd => cmd.GetInfo());

                return new Sys_GetAppCommandInfos_Reply() {
                    AppCommandInfos = infos,
                };
            }
        }

        /// <summary>
        /// 获取接口的参数示例及文档
        /// </summary>
        [AppCommand("Sys_GetAppCommandDocs", "获取接口的参数示例及文档", AppCommandCategory.System, SecurityLevel = SecurityLevel.Admin)]
        class GetAppCommandDocsAppCommand : ACS<TrayAppServiceBaseEx>.Func<Sys_GetAppCommandDocs_Request, Sys_GetAppCommandDocs_Reply>
        {
            public GetAppCommandDocsAppCommand(SysAppCommandManagerComponent component)
            {
                _component = component;
            }

            private readonly SysAppCommandManagerComponent _component;

            protected override Sys_GetAppCommandDocs_Reply OnExecute(AppCommandExecuteContext<TrayAppServiceBaseEx> context, Sys_GetAppCommandDocs_Request req, ref ServiceResult sr)
            {
                ServiceDesc serviceDesc = ((TrayAppServiceBaseEx)context.Service).AppServiceInfo.ServiceDesc;
                IAppCommand[] commands = ((TrayAppServiceBaseEx)context.Service).GetAllCommands();
                var list = from cmd in commands
                           let info = cmd.GetInfo()
                           where req.CommandDescs == null || req.CommandDescs.Contains(info.CommandDesc)
                           let input = info.InputParameter
                           let output = info.OutputParameter
                           let method = info.CommandDesc.ToString(false)
                           select new AppCommandDoc() {
                               CommandDesc = info.CommandDesc,
                               Summary = info.Title, Remarks = info.Desc,
                               SecurityDoc = new SecurityDoc{ Level = (int)info.SecurityLevel, Desc = info.SecurityLevel.GetDesc() },
                               UsableDoc = new UsableDoc { Usable = info.Usable, Desc = info.UsableDesc },
                               Input = _GetArgumentData(serviceDesc, method, input, req.Formats, typeof(RequestWrapper<>)),
                               Output = _GetArgumentData(serviceDesc, method, output, req.Formats, typeof(ReplyWrapper<>)),
                           };

                AppCommandDoc[] docs = list.ToArray();
                return new Sys_GetAppCommandDocs_Reply() { Docs = docs };
            }
        }

        #region Class RequestWrapper<T> ...

        [DataContract]
        class RequestWrapper
        {
            [DataMember(Name = "method")]
            [Summary("接口的全名"), Remarks("格式为“服务名称/接口名称”，例如：“System.User/Login”，也可以指定版本号：“System.User 1.0/Login 1.0”，如果不指定版本号，将寻找一个最新的版本")]
            public string Method { get; set; }

            [DataMember(Name = "sid")]
            [Summary("安全码"), Remarks("在登录后会获得安全码，该安全码用于判断是否具有对某个接口访问的权限")]
            public Sid sid { get; set; }
        }

        [DataContract]
        class RequestWrapper<T> : RequestWrapper
        {
            [DataMember(Name = "body")]
            [Summary("请求参数"), Remarks("每个接口都具有自己的请求参数格式，也有可能为空")]
            public T Body { get; set; }
        }

        #endregion

        #region Class ReplyWrapper<T> ...

        [DataContract]
        class ReplyWrapper
        {
            [DataMember(Name = "statusCode")]
            [Summary("状态码"), Remarks("用于标识应答的状态，详情请参见详细的状态码文档，如果未返回，则默认为请求成功，此时状态码为0")]
            public int StatusCode { get; set; }

            [DataMember(Name = "statusDesc")]
            [Summary("状态描述"), Remarks("对错误原因进行描述，如果未返回，则默认为空串")]
            public string StatusDesc { get; set; }

            [DataMember(Name = "sid")]
            [Summary("安全码"), Remarks("用户登录或安全码需要更新时返回，系统根据安全码来确定用户的权限")]
            public Sid sid { get; set; }
        }

        [DataContract]
        class ReplyWrapper<T> : ReplyWrapper
        {
            [DataMember(Name = "body")]
            [Summary("应答参数"), Remarks("每个接口都具有自己的应答参数格式，也有可能为空")]
            public T Body { get; set; }
        }

        #endregion

        #region Class MessageWrapper<T> ...

        [DataContract]
        class MessageWrapper
        {
            [DataMember(Name = "method")]
            [Summary("消息的全名"), Remarks("格式为“服务名称/消息名称”，例如：“System.User/StatusChanged”，也可以指定版本号：“System.User 1.0/StatusChanged 1.0”，如果不指定版本号，则默认为1.0.0.0")]
            public string Method { get; set; }

            [DataMember(Name = "sid")]
            [Summary("安全码"), Remarks("安全码需要更新时返回，系统根据安全码来确定用户的权限")]
            public Sid sid { get; set; }
        }

        [DataContract]
        class MessageWrapper<T> : MessageWrapper
        {
            [Summary("消息体"), Remarks("每个消息都有自己的消息格式，也有可能为空")]
            public T Body { get; set; }
        }

        #endregion

        private static AppArgumentData _GetArgumentData(ServiceDesc serviceDesc, string method, AppParameter parameter, DataFormat[] formats, Type template)
        {
            Type t = parameter.ParameterType;
            bool empty = (t == null || t == typeof(void));
            Type wrapperType = empty ? template.BaseType : template.MakeGenericType(t);

            return new AppArgumentData() {
                IsEmpty = empty,
                Samples = _GetArgumentSamples(serviceDesc, method, wrapperType, formats).ToArray(),
                DocTree = DocUtility.GetDocTree(wrapperType)
            };
        }

        private static IEnumerable<AppArgumentDataSample> _GetArgumentSamples(ServiceDesc serviceDesc, string method, Type wrapperType, DataFormat[] formats)
        {
            if (formats == null || formats.Contains(DataFormat.Json))
                yield return new AppArgumentDataSample() {
                    Format = DataFormat.Json, Sample = JsonUtility.GetJsonSampleForType(wrapperType, (t, p) => (p == "/method") ? ("\"" + serviceDesc.ToString(false) + "/" + method + "\"") : null)
                };

            if (formats == null || formats.Contains(DataFormat.Xml))
                yield return new AppArgumentDataSample() { Format = DataFormat.Xml, Sample = XmlUtility.GetXmlSampleForType(wrapperType) };
        }

        /// <summary>
        /// 获取所有消息
        /// </summary>
        [AppCommand("Sys_GetAppMessageInfos", "获取所有消息", AppCommandCategory.System, SecurityLevel = SecurityLevel.Admin)]
        public class GetAppMessageInfosAppCommand : ACS<TrayAppServiceBaseEx>.Func<Sys_GetAppMessageInfos_Reply>
        {
            public GetAppMessageInfosAppCommand(SysAppCommandManagerComponent component)
            {
                _component = component;
            }

            private readonly SysAppCommandManagerComponent _component;

            protected override Sys_GetAppMessageInfos_Reply OnExecute(AppCommandExecuteContext<TrayAppServiceBaseEx> context, ref ServiceResult sr)
            {
                AppMessageInfo[] msgInfos = _component._service.AppMessageManager.GetAll();

                return new Sys_GetAppMessageInfos_Reply() { Infos = msgInfos };
            }
        }

        /// <summary>
        /// 获取消息参数示例及文档
        /// </summary>
        [AppCommand("Sys_GetAppMessageDocs", "获取消息参数示例及文档", AppCommandCategory.System, SecurityLevel = SecurityLevel.Admin)]
        public class GetAppMessageDocsAppCommand : ACS<TrayAppServiceBaseEx>.Func<Sys_GetAppMessageDocs_Request, Sys_GetAppMessageDocs_Reply>
        {
            public GetAppMessageDocsAppCommand(SysAppCommandManagerComponent component)
            {
                _component = component;
            }

            private readonly SysAppCommandManagerComponent _component;

            protected override Sys_GetAppMessageDocs_Reply OnExecute(AppCommandExecuteContext<TrayAppServiceBaseEx> context, Sys_GetAppMessageDocs_Request request, ref ServiceResult sr)
            {
                ServiceDesc serviceDesc = ((TrayAppServiceBaseEx)context.Service).AppServiceInfo.ServiceDesc;
                AppMessageInfo[] msgInfos = _component._service.AppMessageManager.GetAll();
                var list = from msgInfo in msgInfos
                           where request.MessageDescs == null || request.MessageDescs.Contains(msgInfo.MessageDesc)
                           let data = msgInfo.Data
                           let method =  msgInfo.MessageDesc.ToString(false)
                           select new AppMessageDoc {
                                Summary = msgInfo.Title, Remarks = msgInfo.Desc, MessageDesc = msgInfo.MessageDesc,
                                Data = _GetArgumentData(serviceDesc, method, data, request.Formats, typeof(MessageWrapper<>))
                           };

                return new Sys_GetAppMessageDocs_Reply { Docs = list.ToArray()};
            }
        }

        /// <summary>
        /// 获取状态码描述信息
        /// </summary>
        [AppCommand("Sys_GetAppStatusCodeInfos", "获取状态码描述信息", AppCommandCategory.System, SecurityLevel = SecurityLevel.Admin)]
        public class GetAppStatusCodeInfosAppCommand : ACS<TrayAppServiceBaseEx>.Func<Sys_GetAppStatusCodeInfos_Reply>
        {
            public GetAppStatusCodeInfosAppCommand(SysAppCommandManagerComponent component)
            {
                _component = component;
            }

            private readonly SysAppCommandManagerComponent _component;

            protected override Sys_GetAppStatusCodeInfos_Reply OnExecute(AppCommandExecuteContext<TrayAppServiceBaseEx> context, ref ServiceResult sr)
            {
                AppStatusCodeInfo[] statusCodeInfos = _component._service.AppStatusCodeManager.GetAll();
                return new Sys_GetAppStatusCodeInfos_Reply() { Infos = statusCodeInfos };
            }
        }

        /// <summary>
        /// 获取服务的文件列表
        /// </summary>
        [AppCommand("Sys_GetAppServiceFileInfos", "获取服务的文件列表", AppCommandCategory.System, SecurityLevel = SecurityLevel.Admin)]
        class DownloadAppServiceFileInfosAppCommand : ACS<TrayAppServiceBaseEx>.Func<Sys_GetAppServiceFileInfos_Reply>
        {
            public DownloadAppServiceFileInfosAppCommand(SysAppCommandManagerComponent component)
            {
                _component = component;
            }

            private readonly SysAppCommandManagerComponent _component;

            protected override Sys_GetAppServiceFileInfos_Reply OnExecute(AppCommandExecuteContext<TrayAppServiceBaseEx> context, ref ServiceResult sr)
            {
                var service = _component._service;
                AppFileInfo[] fileInfos = AppFileInfo.LoadFromDirectory(service.Context.RunningPath);

                return new Sys_GetAppServiceFileInfos_Reply() { FileInfos = fileInfos };
            }
        }

        /// <summary>
        /// 下载服务的文件
        /// </summary>
        [AppCommand("Sys_DownloadAppServiceFiles", "下载服务的文件", AppCommandCategory.System, SecurityLevel = SecurityLevel.Admin)]
        class DownloadAppServiceFilesAppCommand : ACS<TrayAppServiceBaseEx>.Func<Sys_DownloadAppServiceFiles_Request, Sys_DownloadAppServiceFiles_Reply>
        {
            public DownloadAppServiceFilesAppCommand(SysAppCommandManagerComponent component)
            {
                _component = component;
            }

            private readonly SysAppCommandManagerComponent _component;

            protected override Sys_DownloadAppServiceFiles_Reply OnExecute(AppCommandExecuteContext<TrayAppServiceBaseEx> context, Sys_DownloadAppServiceFiles_Request req, ref ServiceResult sr)
            {
                var list = from filename in (req.FileNames ?? Array<string>.Empty).Distinct(IgnoreCaseEqualityComparer.Instance)
                           let content = _ReadBytes(filename)
                           select new AppServiceFileData() { FileName = filename, Content = content };

                return new Sys_DownloadAppServiceFiles_Reply() { Files = list.ToArray() };
            }

            private byte[] _ReadBytes(string filename)
            {
                string fullFileName = Path.Combine(_component._service.Context.RunningPath, filename);
                if (!File.Exists(fullFileName))
                    throw new ServiceException(ServerErrorCode.DataError, string.Format("文件{0}不存在", filename));

                FileInfo fInfo = new FileInfo(fullFileName);
                if (fInfo.Length > 5 * 1024 * 1024)
                    throw new ServiceException(ServerErrorCode.DataError, string.Format("文件{0}太大，无法下载", filename));

                return File.ReadAllBytes(fullFileName);
            }
        }

        /// <summary>
        /// 判断指定的接口是否存在
        /// </summary>
        [AppCommand("Sys_ExistsCommand", "判断指定的接口是否存在", AppCommandCategory.System, SecurityLevel = SecurityLevel.Admin)]
        class ExistsCommandAppCommand : ACS<TrayAppServiceBaseEx>.Func<Sys_ExistsCommand_Request, Sys_ExistsCommand_Reply>
        {
            public ExistsCommandAppCommand(SysAppCommandManagerComponent component)
            {
                _component = component;
            }

            private readonly SysAppCommandManagerComponent _component;

            protected override Sys_ExistsCommand_Reply OnExecute(AppCommandExecuteContext<TrayAppServiceBaseEx> context, Sys_ExistsCommand_Request req, ref ServiceResult sr)
            {
                var list = from sd in req.CommandDescs
                           let cmd = _component._service.GetCommand(sd)
                           where cmd != null
                           select cmd.GetInfo().CommandDesc;

                return new Sys_ExistsCommand_Reply() { CommandDescs = list.ToArray() };
            }
        }
    }
}
