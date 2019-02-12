using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;

namespace Common.Contracts.Service
{

    /// <summary>
    /// 输入参数
    /// </summary>
    public abstract class AppCommandArg
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        public AppCommandArg(object value)
        {
            Value = value;
        }

        /// <summary>
        /// 获取参数类型
        /// </summary>
        /// <returns></returns>
        public object Value { get; private set; }

        public override string ToString()
        {
            return Value == null ? string.Empty : Value.ToString();
        }
    }

    /// <summary>
    /// 输入参数
    /// </summary>
    public class InputAppCommandArg : AppCommandArg
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        public InputAppCommandArg(object value)
            : base(value)
        {

        }
    }

    /// <summary>
    /// 输出参数
    /// </summary>
    public class OutputAppCommandArg : AppCommandArg
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="statusCode">状态码</param>
        /// <param name="statusDesc">状态描述</param>
        /// <param name="sid">安全码</param>
        public OutputAppCommandArg(object value, int statusCode = (int)ServiceStatusCode.Ok, string statusDesc = null, Sid sid = default(Sid))
            : base(value)
        {
            StatusCode = statusCode;
            StatusDesc = statusDesc;
            Sid = sid;
        }

        public OutputAppCommandArg(object value, Enum statusCode, string statusDesc = null, Sid sid = default(Sid))
            : this(value, (statusCode??ServiceStatusCode.Ok).ToType<int>((int)ServiceStatusCode.Ok),
                (statusDesc ?? (statusCode??ServiceStatusCode.Ok).GetDesc()), sid)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        /// <param name="statusDesc"></param>
        public OutputAppCommandArg(object value, string statusDesc = null)
            : this(value, ServiceStatusCode.Ok, statusDesc)
        {

        }

        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; private set; }

        /// <summary>
        /// 状态描述
        /// </summary>
        public string StatusDesc { get; private set; }

        /// <summary>
        /// 安全码
        /// </summary>
        public Sid Sid { get; private set; }

        /// <summary>
        /// 创建只包含状态码的应答
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="statusDesc"></param>
        /// <returns></returns>
        public static OutputAppCommandArg Create(ServiceStatusCode statusCode = ServiceStatusCode.Ok, string statusDesc = null)
        {
            return new OutputAppCommandArg(null, statusCode, statusDesc);
        }

        public static implicit operator OutputAppCommandArg(ServiceStatusCode statusCode)
        {
            return new OutputAppCommandArg(null, 0);
        }
    }
}
