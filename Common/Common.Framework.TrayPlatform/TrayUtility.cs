using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;
using System.Diagnostics.Contracts;
using Common.Package.Service;
using Common.Package;
using Common.Communication.Wcf;
using Common.Contracts;

namespace Common.Framework.TrayPlatform
{
    public static class TrayUtility
    {
        /// <summary>
        /// 从多个信道中获取其中一个，优先使用WTCP方式
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static CommunicationOption PickCommunicationOption(CommunicationOption[] options)
        {
            if (options.IsNullOrEmpty())
                return null;

            for (int k = 0; k < _communicationTypes.Length; k++)
            {
                CommunicationType t = _communicationTypes[k];

                for (int i = 0; i < options.Length; i++)
                {
                    CommunicationOption op = options[i];
                    if (op.Type == t && !op.Duplex && !op.IsLocalHost())  // 服务与服务之间的交互皆使用单向连接
                        return op;
                }
            }

            return null;
        }

        private static readonly CommunicationType[] _communicationTypes = { CommunicationType.WTcp, CommunicationType.Tcp, CommunicationType.Http };

        /// <summary>
        /// 从多个信道中获取其中一个，优先使用TCP方式
        /// </summary>
        /// <param name="invokeInfo"></param>
        /// <returns></returns>
        public static CommunicationOption PickCommunicationOption(this AppInvokeInfo invokeInfo)
        {
            return PickCommunicationOption(invokeInfo.CommunicateOptions);
        }

        /// <summary>
        /// 获取指定程序集的AppService信息
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <returns></returns>
        public static AppServiceInfo LoadAppServiceInfo(string assemblyFile)
        {
            Contract.Requires(assemblyFile != null);

            AppDomain domain = null;
            try
            {
                IAppService appService = AppDomainServiceLoader.Load<IAppService>(assemblyFile, out domain);
                return appService.Init(new ServiceProvider(), AppServiceInitModel.ReadInfo);
            }
            catch (Exception)
            {
                if (domain != null)
                    domain.SafeUnload();

                throw;
            }
        }

        /// <summary>
        /// 创建服务未发现的应答数据
        /// </summary>
        /// <param name="method"></param>
        /// <param name="format"></param>
        /// <param name="detail"></param>
        /// <returns></returns>
        internal static CommunicateData CreateServiceNotFoundCommunicateData(string method, DataFormat format, string detail = "")
        {
            MethodParser mp = new MethodParser(method);
            return new CommunicateData(null, format, ServerErrorCode.NotFound, _BuildServiceNotFoundErrorMessage(method), detail);
        }

        /// <summary>
        /// 创建服务未发现的异常
        /// </summary>
        /// <param name="method"></param>
        /// <param name="format"></param>
        /// <param name="detail"></param>
        /// <returns></returns>
        internal static ServiceException CreateServiceNotFoundException(string method, DataFormat format, string detail = "")
        {
            return new ServiceException(ServerErrorCode.NotFound, _BuildServiceNotFoundErrorMessage(method), detail);
        }

        private static string _BuildServiceNotFoundErrorMessage(string method)
        {
            MethodParser mp = new MethodParser(method);
            return string.Format("无法访问服务“{0}”（调用接口“{1}”）", mp.ServiceDesc, method);
        }

        /// <summary>
        /// 用于标识是服务自身不存在，并非该服务所调用的服务不存在，这个标识用于决定是否尝试远程调用
        /// </summary>
        internal const int ServiceNotFoundStatusCode_SelfNotFound = (int)ServerErrorCode.ServerError | (short.MaxValue << 16);
    }
}
