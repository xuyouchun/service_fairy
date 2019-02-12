using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 服务器端错误码
    /// </summary>
    public enum ServerErrorCode
    {
        /// <summary>
        /// 服务器端错误
        /// </summary>
        [Desc("服务器端错误")]
        ServerError = ServiceStatusCode.ServerError,

        /// <summary>
        /// 服务或接口不存在
        /// </summary>
        [Desc("服务或接口不存在")]
        NotFound = ServerError | 1,

        /// <summary>
        /// 服务不可用
        /// </summary>
        [Desc("服务不可用")]
        ServiceInvalid = ServerError | 2,

        /// <summary>
        /// 当前操作无法继续
        /// </summary>
        [Desc("当前操作无效")]
        InvalidOperation = ServerError | 3,

        /// <summary>
        /// 参数错误
        /// </summary>
        [Desc("参数错误")]
        ArgumentError = ServerError | 4,

        /// <summary>
        /// 返回值错误
        /// </summary>
        [Desc("返回值错误")]
        ReturnValueError = ServerError | 5,

        /// <summary>
        /// 数据校验错误
        /// </summary>
        [Desc("数据校验错误")]
        DataValidateError = ServerError | 6,

        /// <summary>
        /// 不支持
        /// </summary>
        [Desc("不支持")]
        NotSupported = ServerError | 7,

        /// <summary>
        /// 无效的终端
        /// </summary>
        [Desc("无效的终端")]
        InvalidClient = ServerError | 8,

        /// <summary>
        /// 数据尚未初始化
        /// </summary>
        [Desc("数据尚未初始化")]
        DataNotReady = ServerError | 9,

        /// <summary>
        /// 数据错误
        /// </summary>
        [Desc("数据错误")]
        DataError = ServerError | 10,

        /// <summary>
        /// 数据不存在
        /// </summary>
        [Desc("数据不存在")]
        NoData = ServerError | 11,

        /// <summary>
        /// 配置文件错误
        /// </summary>
        [Desc("配置文件错误")]
        ConfigurationError = ServerError | 12,

        /// <summary>
        /// 服务忙
        /// </summary>
        [Desc("服务忙")]
        ServiceBusy = ServerError | 13,

        /// <summary>
        /// 无权限
        /// </summary>
        [Desc("权限错误")]
        SecurityError = ServerError | 14,

        /// <summary>
        /// 代理错误
        /// </summary>
        [Desc("代理错误")]
        ProxyError = ServerError | 15,

        /// <summary>
        /// 已禁用
        /// </summary>
        [Desc("已禁用")]
        Disabled = ServerError | 16,

        /// <summary>
        /// 无效用户
        /// </summary>
        [Desc("无效用户")]
        InvalidUser = ServerError | 1024,
    }

    /// <summary>
    /// 客户端错误
    /// </summary>
    public enum ClientErrorCode
    {
        /// <summary>
        /// 客户端错误
        /// </summary>
        [Desc("客户端错误")]
        ClientError = ServiceStatusCode.ClientError,

        /// <summary>
        /// 被取消
        /// </summary>
        [Desc("操作被取消")]
        Canceled = ClientError | 1,

        /// <summary>
        /// 网络通信错误
        /// </summary>
        [Desc("网络通信错误")]
        NetworkError = ClientError | 2,

        /// <summary>
        /// 超时
        /// </summary>
        [Desc("超时")]
        Timeout = ClientError | 3,
    }
}
