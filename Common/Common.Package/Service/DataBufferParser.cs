using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Package.Serializer;
using Common.Contracts;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;

namespace Common.Package.Service
{
    /// <summary>
    /// 数据序列化
    /// </summary>
    public static class DataBufferParser
    {
        private static Stream _CreateStream(object buffer)
        {
            if (buffer == null)
                return null;

            byte[] bytes = buffer as byte[];
            if (bytes != null)
            {
                if (bytes.Length == 0)
                    return null;

                return new MemoryStream(bytes);
            }

            Stream stream = buffer as Stream;
            if (stream == null)
                throw new NotSupportedException("不支持该数据类型作为序列化的源: " + buffer.GetType());

            return stream;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="buffer">数据</param>
        /// <param name="type">类型</param>
        /// <param name="format">数据格式</param>
        /// <returns></returns>
        public static object Deserialize(object buffer, Type type, DataFormat format)
        {
            Stream stream = _CreateStream(buffer);
            if (stream == null)
                return null;

            return SerializerFactory.CreateSerializer(format).Deserialize(type, stream);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static T Deserialize<T>(object buffer, DataFormat format)
        {
            return (T)Deserialize(buffer, typeof(T), format);
        }

        /// <summary>
        /// 反序列化指定的通信数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        public static object Deserialize(CommunicateData data, Type type)
        {
            Contract.Requires(type != null);

            if (data == null)
                return null;

            return Deserialize(data.Data, type, data.DataFormat);
        }

        /// <summary>
        /// 反序列化指定的通信数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Deserialize<T>(CommunicateData data)
        {
            object r = Deserialize(data, typeof(T));
            return !(r is T) ? default(T) : (T)Deserialize(data, typeof(T));
        }

        /// <summary>
        /// 序列化为字节数组
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static byte[] SerializeToBytes(object entity, DataFormat format)
        {
            if (entity == null)
                return new byte[0];

            MemoryStream ms = new MemoryStream();
            SerializeToStream(entity, format, ms);
            return ms.ToArray();
        }

        /// <summary>
        /// 序列化到流中
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="format"></param>
        /// <param name="outputStream"></param>
        public static void SerializeToStream(object entity, DataFormat format, Stream outputStream)
        {
            Contract.Requires(outputStream != null);

            if (entity == null)
                return;

            SerializerFactory.CreateSerializer(format).Serialize(entity, outputStream);
        }

        /// <summary>
        /// 序列化为通信实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="format"></param>
        /// <param name="bufferType"></param>
        /// <param name="statusCode"></param>
        /// <param name="errorMsg"></param>
        /// <param name="detail"></param>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static CommunicateData Serialize(object entity, DataFormat format,
            int statusCode = (int)ServiceStatusCode.Ok, string errorMsg = "", string detail = "", Sid sid = default(Sid))
        {
            format = format.Revise();
            if (entity == null)
                return new CommunicateData(null, format, statusCode, errorMsg, detail, sid);

            byte[] data = SerializeToBytes(entity, format);
            return new CommunicateData(data, format, statusCode, errorMsg, detail, sid);
        }

        /// <summary>
        /// 序列化为通信实体
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static CommunicateData Serialize(OutputAppCommandArg arg, DataFormat format)
        {
            if (arg == null)
                return new CommunicateData(null, format.Revise());

            return Serialize(arg.Value, format, arg.StatusCode, arg.StatusDesc, "", arg.Sid);
        }
    }
}
