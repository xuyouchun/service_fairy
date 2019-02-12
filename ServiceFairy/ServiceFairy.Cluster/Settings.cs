using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;

namespace ServiceFairy.Cluster
{
    static class Settings
    {
        public static readonly ServiceDesc TrayServiceDesc = new ServiceDesc(SFNames.ServiceNames.Tray, "1.0");

    }
}
