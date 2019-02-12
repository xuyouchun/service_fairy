using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;

namespace ServiceFairy.Management
{
    [SoInfo("场景模拟"), UIObjectImage("SceneTest")]
    class SceneTestNode : ServiceObjectKernelTreeNodeBase
    {
        public SceneTestNode(SfManagementContext mgrCtx)
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
