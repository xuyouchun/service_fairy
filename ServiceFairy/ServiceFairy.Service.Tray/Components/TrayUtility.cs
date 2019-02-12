using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.Tray;
using Common.Utility;

namespace ServiceFairy.Service.Tray.Components
{
    static class TrayUtility
    {
        /// <summary>
        /// 创建业务逻辑异常
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ServiceException CreateException(TrayStatusCode statusCode, string message = null)
        {
            return new ServiceException(statusCode, message);
        }
    }
}
