using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using BhFairy.Entities.ContactsBackup;
using ServiceFairy;
using BhFairy.Service.ContactsBackup.Components;
using Common.File.UnionFile;
using ServiceFairy.Entities.File;
using Common.Utility;
using Common.Contracts;

namespace BhFairy.Service.ContactsBackup
{
    /// <summary>
    /// 开始下载通信录备份
    /// </summary>
    [AppCommand("BeginDownload", title: "开始下载通信录备份数据", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class BeginDownloadAppCommand : ACS<Service>.Func<ContactsBackup_BeginDownload_Request, ContactsBackup_BeginDownload_Reply>
    {
        protected override ContactsBackup_BeginDownload_Reply OnExecute(AppCommandExecuteContext<Service> context, ContactsBackup_BeginDownload_Request req, ref ServiceResult sr)
        {
            Service srv = context.Service;

            UnionFileInfo fileInfo;
            StreamTableBasicInfo basicInfo;
            string token = srv.ContactsBackup.BeginDownload(context.GetSessionState(), req.Name, out basicInfo, out fileInfo);

            return new ContactsBackup_BeginDownload_Reply() {
                Token = token,
                ColumnHeaders = basicInfo.TableInfos.ToArray(ti => ti.Name),
                Info = new ContactBackupInfo() {
                    Name = fileInfo.Name, Title = fileInfo.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    Time = fileInfo.CreationTime, ContactCount = basicInfo.TableInfos.FirstOrDefault(ti => ti.Name == "contact_list").RowCount
                }
            };
        }

        const string Remarks = @"这是下载通信录备份数据的第一步，将会返回列头与事务标识，随后调用Download的时候需要携带该事务标识。";
    }
}
