using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Database;

namespace ServiceFairy.Service.Database
{
    static class Utility
    {
        public static ServiceException CreateBusinessException(DatabaseStatusCode statusCode, string errorMsg = null)
        {
            return new ServiceException(statusCode, errorMsg);
        }
    }
}
