using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.File;
using Common.Utility;

namespace ServiceFairy.Service.File.Components
{
    static class Utility
    {
        public static ServiceException CreateBusinessException(FileStatusCode statusCode, string statusDesc = null)
        {
            return new ServiceException(statusCode, statusDesc);
        }
    }
}
