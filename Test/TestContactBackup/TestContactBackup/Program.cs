using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;
using BhFairy.ApplicationInvoke;
using Common.Contracts.Service;
using BhFairy.Entities.ContactsBackup;
using System.Diagnostics;
using Common.Package.TaskDispatcher;
using Common.Utility;
using Common.Package;
using System.Threading;

namespace TestContactBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            //LogManager.RegisterFileLogWriter("d:\\temp\\log\\TestContractBackup");
            //LogManager.RegisterConsoleWindowWriter();
            string navigation = "net.tcp://127.0.0.1:8090";
            //string navigation = "xuyc-pc:8090";
            //string navigation = "net.tcp://117.79.130.229:8090";

            using (ApplicationInvoker appInvoker = ApplicationInvoker.FromNavigation(navigation))
            using (SystemInvoker sysInvoker = new SystemInvoker(appInvoker))
            {
                string sid = sysInvoker.User.Login("+86 13717674043", "93939393");
                //UserSessionState uss = UserManager.TestUser.Create();
                CommunicateCallingSettings settings = CommunicateCallingSettings.RequestReplyWithSid(sid);

                for (int k = 0; ; k++)
                {
                    LogManager.LogMessage(string.Format("第{0}次尝试 ...", k));
                    try
                    {
                        _BackupAndDownload(appInvoker, settings);
                        LogManager.LogMessage("OK!");
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogError(ex);
                    }

                    Thread.Sleep(500);
                }
            }
        }

        private static void _DeleteAll(ApplicationInvoker appInvoker, CommunicateCallingSettings settings)
        {
             ContactBackupInfo[] infos = appInvoker.ContactsBackup.GetList(settings);
             appInvoker.ContactsBackup.Delete(infos.SelectFromList(info => info.Name), settings);
        }

        private static void _BackupAndDownload(ApplicationInvoker appInvoker, CommunicateCallingSettings settings)
        {
            string token = appInvoker.ContactsBackup.BeginUpload(new[] { "name", "phone_number", "sex", "address", "work" }, settings);

            for (int index = 0; index < 100; index++)
            {
                appInvoker.ContactsBackup.Upload(token, new Contact {
                    Columns = new[] { "name_" + index, "phone_number_" + index, "1", "address_" + index, "work_" + index },
                }, settings);
            }

            appInvoker.ContactsBackup.EndUpload(token, settings);

            ContactBackupInfo[] infos = appInvoker.ContactsBackup.GetList(settings);
            Console.WriteLine("现有备份数量：{0}", infos.Length);
            if (infos.Length > 0)
            {
                ContactBackupInfo info = infos.Last();

                string[] columnHeaders;
                ContactBackupInfo bkInfo;
                token = appInvoker.ContactsBackup.BeginDownload(info.Name, out columnHeaders, out bkInfo, settings);

                List<Contact> contacts = new List<Contact>();
                for (int k = 0; k < bkInfo.ContactCount; k++)
                {
                    Contact contact = appInvoker.ContactsBackup.Download(token, k, settings);
                    if (contact != null)
                        contacts.Add(contact);
                }

                appInvoker.ContactsBackup.EndDownload(token, settings);
            }
        }
    }
}
