using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace Common.Contracts
{
    /// <summary>
    /// 提供一些类型的默认值
    /// </summary>
    public static class DefaultValues
    {
        /// <summary>
        /// 默认的数据序列化格式
        /// </summary>
        public const DataFormat DefaultDataFormat = DataFormat.Binary;

        /// <summary>
        /// 将DataFormat.Unknown修正为默认值DataFormat.Binary
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DataFormat Revise(this DataFormat format)
        {
            return (format == DataFormat.Unknown) ? DataFormat.Binary : format;
        }

        /// <summary>
        /// 考虑到默认值的情况，比较两个DataFormat是否相等
        /// </summary>
        /// <param name="format1"></param>
        /// <param name="format2"></param>
        /// <returns></returns>
        public static bool EqualsTo(this DataFormat format1, DataFormat format2)
        {
            return format1 == format2 || (format1.Revise() == format2.Revise());
        }

        /// <summary>
        /// 默认的数据缓冲区类型
        /// </summary>
        public const BufferType DefaultBufferType = BufferType.Bytes;

        /// <summary>
        /// 将BufferType.Unknown修正为默认值BufferType.Bytes
        /// </summary>
        /// <param name="bufferType"></param>
        /// <returns></returns>
        public static BufferType Revise(this BufferType bufferType)
        {
            return (bufferType == BufferType.Unknown) ? BufferType.Bytes : bufferType;
        }

        /// <summary>
        /// 默认的通信协议
        /// </summary>
        public const CommunicationType DefaultCommunicationType = CommunicationType.WTcp;

        /// <summary>
        /// CommunicationType.Unknown修正为CommunicationType.Tcp
        /// </summary>
        /// <param name="communicationType"></param>
        /// <returns></returns>
        public static CommunicationType Revise(this CommunicationType communicationType)
        {
            return (communicationType == CommunicationType.Unknown) ? CommunicationType.WTcp : communicationType;
        }

        /// <summary>
        /// 考虑到默认值的情况，比较两个BufferType是否相等
        /// </summary>
        /// <param name="bufferType1"></param>
        /// <param name="bufferType2"></param>
        /// <returns></returns>
        public static bool EqualsTo(this BufferType bufferType1, BufferType bufferType2)
        {
            return bufferType1 == bufferType2 || (bufferType1.Revise() == bufferType2.Revise());
        }
    }
}
