using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Utility;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 服务的异常
    /// </summary>
    [Serializable, DataContract]
    public class ServiceException : ApplicationException
    {
        public ServiceException()
            : this(ServiceStatusCode.ServerError, null, null)
        {

        }

        public ServiceException(Enum statusCode)
            : this(statusCode, null, null, "")
        {
        
        }

        public ServiceException(int statusCode)
            : this(statusCode, null, null, "")
        {

        }

        public ServiceException(Enum statusCode, string message, string detail = "")
            : this(statusCode, message, null, detail)
        {
            
        }

        public ServiceException(int statusCode, string message, string detail = "")
            : this(statusCode, message, null, detail)
        {

        }

        public ServiceException(Enum statusCode, string message, Exception inner, string detail = "")
            : this(statusCode.ToType<int>((int)ServerErrorCode.ServerError), message ?? ((statusCode ?? ServerErrorCode.ServerError).GetDesc()), inner, detail)
        {

        }

        public ServiceException(int statusCode, string message, Exception inner, string detail = "")
            : base(message, inner)
        {
            StatusCode = statusCode;
            Detail = detail;
        }

        protected ServiceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            StatusCode = info.GetInt32("SC");
            Detail = info.GetString("D");
        }

        /// <summary>
        /// 状态码
        /// </summary>
        [DataMember]
        public int StatusCode { get; private set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        [DataMember]
        public string Detail { get; private set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("SC", StatusCode);
            info.AddValue("D", Detail);
        }

        public override string ToString()
        {
            return string.Format("{0} StatusCode:{1} \r\n{2}", Message, StatusCode, StackTrace);
        }
    }
}
