using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Utility;

namespace Common.Communication.Wcf.Encoders
{
    static class EncoderUtility
    {
        public static UniqueId ParseUniqueId(string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            return new UniqueId(s);
        }

        /// <summary>
        /// 根据序列化格式获取ContentType
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetContentType(DataFormat format)
        {
            IWcfMessageEncoder encoder = EncoderFactory.CreateEncoder(format);
            return encoder == null ? null : encoder.GetContentType();
        }

        /// <summary>
        /// 转换为字节流
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this CallingSettings settings)
        {
            Contract.Requires(settings != null);
            MemoryStream ms = new MemoryStream();
            WriteToStream(settings, ms);
            return ms.ToArray();
        }

        /// <summary>
        /// 写入到流中
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="stream"></param>
        public static void WriteToStream(this CallingSettings settings, Stream stream)
        {
            Contract.Requires(settings != null && stream != null);

            stream.Write((int)settings.InvokeType);
            stream.WriteByte((byte)(settings.Target == null ? ServiceTargetModel.Auto : settings.Target.Model));
            stream.Write(settings.Sid);
            int endpointCount = (settings.Target == null || settings.Target.EndPoints.IsNullOrEmpty()) ? 0 : settings.Target.EndPoints.Length;
            stream.Write((ushort)endpointCount);

            if (endpointCount > 0)
            {
                for (int k = 0; k < endpointCount; k++)
                {
                    ServiceEndPoint sep = settings.Target.EndPoints[k];
                    stream.Write(sep.ClientId);
                    ServiceDesc sd = sep.ServiceDesc;
                    if (sd != null && !string.IsNullOrEmpty(sd.Name))
                    {
                        stream.WriteByte((byte)1);
                        stream.Write(sd.Name);
                        stream.Write((uint)sd.Version);
                    }
                    else
                    {
                        stream.WriteByte((byte)0);
                    }
                }
            }
        }

        /// <summary>
        /// 从流中读取
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static CallingSettings ReadFromStream(Stream stream)
        {
            Contract.Requires(stream != null);

            CommunicateInvokeType invokeType = (CommunicateInvokeType)stream.ReadInt32();
            ServiceTargetModel targetModel = (ServiceTargetModel)stream.ReadByte();
            Sid sid = stream.ReadSid();
            uint endpointCount = stream.ReadUInt16();
            List<ServiceEndPoint> endpoints = new List<ServiceEndPoint>();
            for (int k = 0; k < endpointCount; k++)
            {
                Guid clientId = stream.ReadGuid();
                ServiceDesc sd = (stream.ReadByte() == (byte)1) ?
                    new ServiceDesc(stream.ReadString(), new SVersion(stream.ReadUInt32())) : null;

                endpoints.Add(new ServiceEndPoint(clientId, sd));
            }

            return new CallingSettings(invokeType, sid, new ServiceTarget(targetModel, endpoints.ToArray()));
        }

        /// <summary>
        /// 从字节流中读取
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static CallingSettings ReadFromBytes(byte[] buffer)
        {
            Contract.Requires(buffer != null);
            return ReadFromStream(new MemoryStream(buffer));
        }
    }
}
