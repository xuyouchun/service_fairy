using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Utility;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 服务调用的返回值
    /// </summary>
    [Serializable, DataContract]
    public class ServiceResult
    {
        public ServiceResult(Sid sid)
            : this(null, null, sid)
        {

        }

        public ServiceResult(int statusCode, string statusDesc = null, Sid sid = default(Sid))
        {
            StatusCode = statusCode;
            StatusDesc = statusDesc;
            Sid = sid;
        }

        public ServiceResult(Enum statusCode, string statusDesc = null, Sid sid = default(Sid))
            : this((statusCode ?? ServiceStatusCode.Ok).ToType<int>((int)ServiceStatusCode.Ok),
                statusDesc ?? (statusCode ?? ServiceStatusCode.Ok).GetDesc(), sid)
        {

        }

        public ServiceResult(ServiceResult sr)
        {
            sr = sr ?? Success;
            StatusCode = sr.StatusCode;
            StatusDesc = sr.StatusDesc;
            Sid = sr.Sid;
        }

        /// <summary>
        /// 状态码
        /// </summary>
        [DataMember]
        public int StatusCode { get; private set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        [DataMember]
        public string StatusDesc { get; private set; }

        /// <summary>
        /// 安全码
        /// </summary>
        [DataMember]
        public Sid Sid { get; private set; }

        /// <summary>
        /// 成功的结果
        /// </summary>
        public static readonly ServiceResult Success = new ServiceResult(ServiceStatusCode.Ok);

        /// <summary>
        /// 根据返回结果创建异常信息
        /// </summary>
        public ServiceException CreateException()
        {
            return new ServiceException(StatusCode, StatusDesc);
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Succeed
        {
            get { return ContractUtility.IsSuccess(StatusCode); }
        }

        /// <summary>
        /// 如果是失败的结果，则抛出异常
        /// </summary>
        public ServiceResult Validate()
        {
            if (!Succeed)
                throw CreateException();

            return this;
        }
    }

    /// <summary>
    /// 对返回值的包装
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceResult<T> : ServiceResult
    {
        public ServiceResult(T result, int statusCode, string statusDesc, Sid sid = default(Sid))
            : base(statusCode, statusDesc, sid)
        {
            Result = result;
        }

        public ServiceResult(T result, Enum statusCode, string statusDesc = null, Sid sid = default(Sid))
            : base(statusCode, statusDesc, sid)
        {
            Result = result;
        }

        /// <summary>
        /// 返回值
        /// </summary>
        public T Result { get; private set; }

        /// <summary>
        /// 如果是失败的结果，则抛出异常
        /// </summary>
        public ServiceResult<T> Validate()
        {
            base.Validate();
            return this;
        }
    }
}
