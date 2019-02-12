using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace ServiceFairy.Entities.File
{
    /// <summary>
    /// 文件服务的状态码
    /// </summary>
    public enum FileStatusCode
    {
        [Desc("Error")]
        Error = SFStatusCodes.File,

        /// <summary>
        /// 文件不存在
        /// </summary>
        [Desc("文件不存在")]
        FileNotExists = Error | 1,

        /// <summary>
        /// 目录不存在
        /// </summary>
        [Desc("目录不存在")]
        DirectoryNotExists = Error | 2,

        /// <summary>
        /// 文件已经存在
        /// </summary>
        [Desc("文件已经存在")]
        FileAlreadyExists = Error | 3,

        /// <summary>
        /// 目录已经存在
        /// </summary>
        [Desc("目录已经存在")]
        DirectoryAlreadyExists = Error | 4,

        /// <summary>
        /// 无效的Token
        /// </summary>
        [Desc("无效的Token")]
        InvalidToken = Error | 5,

        /// <summary>
        /// 不支持写入
        /// </summary>
        [Desc("不支持写入")]
        WriteNotSupported = Error | 6,

        /// <summary>
        /// 不支持读取
        /// </summary>
        [Desc("不支持读取")]
        ReadNotSupported = Error | 7,

        /// <summary>
        /// 路由错误
        /// </summary>
        [Desc("路由错误")]
        RouteError = Error | 8,

        /// <summary>
        /// 尚未创建StreamTable
        /// </summary>
        [Desc("尚未创建StreamTable")]
        StreamTableNoTable = Error | 20,

        /// <summary>
        /// StreamTable不存在
        /// </summary>
        [Desc("StreamTable不存在")]
        StreamTableNotFound = Error | 21,
    }
}
