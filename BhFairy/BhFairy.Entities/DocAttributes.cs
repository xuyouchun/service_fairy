using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using Common.Contracts;

namespace BhFairy.Entities
{
    /// <summary>
    /// 用于标注特殊字段的文档
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    class AppFieldDocAttribute : CommonDocAttributeBase
    {
        public AppFieldDocAttribute(AppField field)
            : base(field)
        {

        }
    }

    /// <summary>
    /// 特殊的字段
    /// </summary>
    public enum AppField
    {
        /// <summary>
        /// 通信录备份名称
        /// </summary>
        [Summary("通信录备份名称")]
        ContactBackup_Name,

        /// <summary>
        /// 通信录备份的列头
        /// </summary>
        [Summary("通信录备份的列头"), Remarks("有一部分列名需要服务器与客户端约定，以便将来用于检索；客户端也可以自定义列名，在下载的时候按原样下载，其具体的含义由客户端来解释")]
        ContactBackup_ColumnHeaders,

        /// <summary>
        /// 通信录备份信息
        /// </summary>
        [Summary("通信录备份信息")]
        ContactBackup_Info,

        /// <summary>
        /// 通信录备份的事务标识
        /// </summary>
        [Summary("通信录备份的事务标识"), Remarks("该事务标识用于在本次备份操作中的唯一标识")]
        ContactBackup_Token,

        /// <summary>
        /// 通信录备份的条目
        /// </summary>
        [Summary("联系人备份数据"), Remarks("存储联系人的详细数据")]
        ContactBackup_Contacts,

        /// <summary>
        /// 通信录备份的数据
        /// </summary>
        [Summary("通信录备份的数据"), Remarks("按BeginUpload或BeginDownload接口中指定的的列名顺序排列的各项详细信息")]
        ContactBackup_ContactData,

        /// <summary>
        /// 通信录备份名称
        /// </summary>
        [Summary("通信录备份名称")]
        ContactBackupInfo_Name,

        /// <summary>
        /// 通信录备份标题
        /// </summary>
        [Summary("通信录备份标题")]
        ContactBackupInfo_Title,

        /// <summary>
        /// 通信录备份时间
        /// </summary>
        [Summary("通信录备份时间(UTC时间)")]
        ContactBackupInfo_Time,

        /// <summary>
        /// 通信录备份中联系人的数量
        /// </summary>
        [Summary("通信录备份中联系人的数量")]
        ContactBackupInfo_ContactCount,
    }
}
