using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Utility;
using System.Runtime.Serialization;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 通信数据
    /// </summary>
    [Serializable, DataContract]
    public class CommunicateData
    {
        public CommunicateData()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="format">数据格式</param>
        /// <param name="statusCode">状态码</param>
        /// <param name="detail">详细信息</param>
        /// <param name="statusDesc"></param>
        public CommunicateData(byte[] data, DataFormat format, int statusCode = (int)ServiceStatusCode.Ok,
            string statusDesc = null, string detail = null, Sid sid = default(Sid))
        {
            Data = data;
            DataFormat = format;
            StatusCode = statusCode;
            StatusDesc = statusDesc;
            Detail = detail;
            Sid = sid;
        }

        public CommunicateData(byte[] data, DataFormat format, Enum statusCode, string statusDesc = null, string detail = "", Sid sid = default(Sid))
            : this(data, format, (statusCode ?? ServiceStatusCode.Ok).ToType<int>((int)ServiceStatusCode.Ok),
                (statusDesc ?? (statusCode ?? ServiceStatusCode.Ok).GetDesc()), detail, sid)
        {
            
        }

        /// <summary>
        /// 数据（可为byte[]或Stream类型）
        /// </summary>
        [DataMember]
        public byte[] Data { get; set; }

        /// <summary>
        /// 数据格式
        /// </summary>
        [DataMember]
        public DataFormat DataFormat { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        [DataMember]
        public int StatusCode { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        [DataMember]
        public string StatusDesc { get; set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        [DataMember]
        public string Detail { get; set; }

        /// <summary>
        /// 安全码
        /// </summary>
        [DataMember]
        public Sid Sid { get; set; }

        /// <summary>
        /// 是否成功的请求
        /// </summary>
        public bool Succeed { get { return ContractUtility.IsSuccess(StatusCode); } }
    }
}
