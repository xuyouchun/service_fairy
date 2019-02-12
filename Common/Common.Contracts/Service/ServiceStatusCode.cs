using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 状态码
    /// </summary>
    public enum ServiceStatusCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Desc("OK")]
        Ok = (0 << 30),

        /// <summary>
        /// 客户端错误
        /// </summary>
        [Desc("客户端错误")]
        ClientError = (1 << 30),

        /// <summary>
        /// 服务器端错误
        /// </summary>
        [Desc("服务器端错误")]
        ServerError = (2 << 30),

        /// <summary>
        /// 业务逻辑错误
        /// </summary>
        [Desc("业务逻辑错误")]
        BusinessError = (3 << 30),
    }

    /// <summary>
    /// 成功的状态码
    /// </summary>
    public enum SuccessCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Desc("OK")]
        Ok = ServiceStatusCode.Ok,

        /// <summary>
        /// 操作尚未完成
        /// </summary>
        [Desc("操作尚未完成")]
        Continue = ServiceStatusCode.Ok | 1,

        /// <summary>
        /// 接口已不推荐使用
        /// </summary>
        [Desc("接口已不推荐使用")]
        Obsolete = ServiceStatusCode.Ok | 2,
    }
}
