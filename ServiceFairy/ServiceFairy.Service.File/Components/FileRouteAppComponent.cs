using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using Common.Algorithms;
using ServiceFairy.Components;
using System.Diagnostics.Contracts;
using ServiceFairy.Entities.File;

namespace ServiceFairy.Service.File.Components
{
    /// <summary>
    /// 路由管理器
    /// </summary>
    [AppComponent("文件路由器")]
    class FileRouteAppComponent : ServiceConsistentNodeManagerAppComponent
    {
        public FileRouteAppComponent(Service service)
            : base(service, service.Context.ServiceDesc)
        {

        }

        /// <summary>
        /// 根据路径获取终端ID
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Guid GetClientIdForPath(string path)
        {
            Contract.Requires(path != null);

            FilePathParser parser = new FilePathParser(path);
            ServiceConsistentNode node = GetNode(parser.RouteHashCode);

            if (node == null)
                throw Utility.CreateBusinessException(FileStatusCode.RouteError);

            return node.ClientID;
        }
    }
}
