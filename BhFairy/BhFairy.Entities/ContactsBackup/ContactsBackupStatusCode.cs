using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using ServiceFairy;

namespace BhFairy.Entities.ContactsBackup
{
    /// <summary>
    /// 通信录备份操作的状态码
    /// </summary>
    public enum ContactsBackupStatusCode
    {
        Error = BhStatusCode.ContactsBackup,

        /// <summary>
        /// 无效的Token
        /// </summary>
        [Desc("当前操作无效或已被取消")]
        InvalidToken = Error | 1,

        /// <summary>
        /// 当前的Token不支持上传
        /// </summary>
        [Desc("上传失败")]
        UploadNotSupported = Error | 2,

        /// <summary>
        /// 当前的Token不支持下载
        /// </summary>
        [Desc("下载失败")]
        DownloadNotSupported = Error | 3,
    }
}
