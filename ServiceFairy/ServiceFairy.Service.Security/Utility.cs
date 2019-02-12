using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;
using ServiceFairy.Entities.Security;
using ServiceFairy.Service.Security.Components;

namespace ServiceFairy.Service.Security
{
    static unsafe class Utility
    {
        public static ServiceException CreateException(SecurityStatusCode statusCode, string message = null)
        {
            return new ServiceException(statusCode, message);
        }

        /// <summary>
        /// 获取指定安全码的用户ID
        /// </summary>
        /// <param name="service"></param>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static int GetUserId(this Service service, Sid sid)
        {
            SidX sidX = service.SidGenerator.Decrypt(sid, false);
            if (sidX.IsEmpty())
                return 0;

            return sidX.UserId;
        }
    }
}
