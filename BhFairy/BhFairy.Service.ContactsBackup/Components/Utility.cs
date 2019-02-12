using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using BhFairy.Entities.ContactsBackup;
using Common.Utility;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities.File;

namespace BhFairy.Service.ContactsBackup.Components
{
    static class Utility
    {
        /// <summary>
        /// 创建业务逻辑异常
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ServiceException CreateBusinessException(ContactsBackupStatusCode statusCode, string msg = null)
        {
            return new ServiceException(statusCode, msg);
        }

        public static string MakeUserPath(int userId)
        {
            return FileUtility.GetPathOfUser(userId);
        }

        public static string MakeBkPath(int userId, string name)
        {
            return MakeBkDirectory(userId) + "/" + name;
        }

        public static string MakeBkDirectory(int userId)
        {
            return MakeUserPath(userId) + ":contact_backup";
        }

        public const string FILE_EXT = ".contactbk.st";
    }
}
