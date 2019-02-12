using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Package.UIObject;

namespace ServiceFairy.Management.FaultDiagnosis
{
    [SoInfo("故障诊断"), UIObjectImage(ResourceNames.FaultDiagnosis)]
    class FaultDiagnosisNode : ServiceObjectKernelTreeNodeBase
    {
        public FaultDiagnosisNode(SfManagementContext mgrCtx)
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
