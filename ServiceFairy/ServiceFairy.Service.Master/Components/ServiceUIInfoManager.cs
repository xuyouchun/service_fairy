using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using Common.Package;
using System.Diagnostics.Contracts;
using ServiceFairy.Service.UI;
using Common.Utility;
using System.IO;
using ServiceFairy.Entities.Master;


namespace ServiceFairy.Service.Master.Components
{
    /// <summary>
    /// 服务管理器信息管理
    /// </summary>
    [AppComponent("服务界面信息管理器", "提供服务的界面相关信息，例如图标、管理器插件等")]
    class ServiceUIInfoManager : AppComponent
    {
        public ServiceUIInfoManager(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;

        private readonly Cache<ServiceDesc, ServiceUIInfo> _serviceUIInfos = new Cache<ServiceDesc, ServiceUIInfo>();

        /// <summary>
        /// 获取指定服务的管理信息
        /// </summary>
        /// <param name="serviceDesc"></param>
        /// <returns></returns>
        public ServiceUIInfo Get(ServiceDesc serviceDesc)
        {
            Contract.Requires(serviceDesc != null);

            ServiceUIInfo info = _serviceUIInfos.GetOrAddOfFileExpire(serviceDesc, _LoadServiceUIInfo, _GetRelationFiles(serviceDesc));
            return object.ReferenceEquals(_errorServiceUIInfo, info) ? null : info;
        }

        private readonly ServiceUIInfo _errorServiceUIInfo = new ServiceUIInfo();

        private readonly Dictionary<ServiceDesc, string[]> _relationFilesDict = new Dictionary<ServiceDesc, string[]>();

        // d
        private string[] _GetRelationFiles(ServiceDesc serviceDesc)
        {
            return _relationFilesDict.GetOrSet(serviceDesc, (sd) => {
                string path = SFSettings.GetServiceRunningBasePath(serviceDesc, _service.RunningBasePath, _service.Context.ClientID);
                return SFSettings.GetAllCoreFiles().ToArray(file => Path.Combine(path, file));
            });
        }

        private ServiceUIInfo _LoadServiceUIInfo(ServiceDesc serviceDesc)
        {
            try
            {
                return SFUtility.LoadServiceUIInfo(serviceDesc, _service.ServiceBasePath);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return _errorServiceUIInfo;
            }
        }
    }
}
