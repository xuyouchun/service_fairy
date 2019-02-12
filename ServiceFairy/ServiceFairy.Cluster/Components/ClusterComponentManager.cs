using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using System.Diagnostics.Contracts;

namespace ServiceFairy.Cluster.Components
{
    /// <summary>
    /// 组件管理器
    /// </summary>
    public class ClusterComponentManager : AppComponentManager
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public ClusterComponentManager(ClusterContext context)
        {
            Contract.Requires(context != null);
            _context = context;

            this.AddComponents(new AppComponent[]{
                AppClientManager = new AppClientManager(context)
            });
        }

        private readonly ClusterContext _context;

        /// <summary>
        /// 服务终端管理器
        /// </summary>
        public AppClientManager AppClientManager { get; private set; }
    }
}
