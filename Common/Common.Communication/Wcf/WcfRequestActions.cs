using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace Common.Communication.Wcf
{
    static class WcfRequestActions
    {
        /// <summary>
        /// 请求应答
        /// </summary>
        public const string RequestReply = "r";

        /// <summary>
        /// 单向无应答
        /// </summary>
        public const string OneWay = "w";

        /// <summary>
        /// 打开流连接
        /// </summary>
        public const string OpenStream = "t";

        internal static string GetAction(CommunicateInvokeType invokeType)
        {
            switch (invokeType & (CommunicateInvokeType)0xFF)
            {
                case CommunicateInvokeType.OneWay:
                    return OneWay;

                case CommunicateInvokeType.RequestReply:
                    return RequestReply;

                case CommunicateInvokeType.OpenStream:
                    return OpenStream;

                default:
                    throw new NotSupportedException("不支持的调用方式");
            }
        }
    }
}
