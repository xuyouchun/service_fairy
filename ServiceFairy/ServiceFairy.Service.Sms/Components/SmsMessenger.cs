using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.DbEntities.Sms;

namespace ServiceFairy.Service.Sms.Components
{
    class SmsMessenger
    {
        public SmsMessenger(Service service)
        {
            Service = service;
        }

        public Service Service { get; private set; }

        public void AddSmsLog(string phoneNumbers, string content, string result)
        {
            DbSmsLog myDbSmsLog = new DbSmsLog {
                Mobiles = phoneNumbers,
                Content = content,
                Result = result,
                CreateTime = DateTime.Now
            };

            myDbSmsLog.Insert(Service.DbConnectionManager.Provider);
        }
    }
}
