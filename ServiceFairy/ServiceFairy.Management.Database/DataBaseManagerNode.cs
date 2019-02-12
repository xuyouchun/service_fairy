using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;

namespace ServiceFairy.Management.Database
{
    /// <summary>
    /// 数据库管理
    /// </summary>
    [SoInfo("数据库管理"), UIObjectImage(ResourceNames.DatabaseManager)]
    class DataBaseManagerNode : ServiceObjectKernelTreeNodeBase
    {
        public DataBaseManagerNode(SfManagementContext mgrCtx)
        {
            _mgrCtx = mgrCtx;
        }

        private readonly SfManagementContext _mgrCtx;

        protected override object GetKey()
        {
            return GetType();
        }
    }
}
