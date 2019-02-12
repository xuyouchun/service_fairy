using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace ServiceFairy.Entities.DatabaseCenter
{
    public enum DatabaseCenterStatusCode
    {
        [Desc("Error")]
        Error = SFStatusCodes.DatabaseCenter,
    }
}
