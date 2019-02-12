using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf.Service;
using System.IO;
using System.Xml;
using Common.Contracts;
using Common.Utility;
using Common.Contracts.Service;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Remoting;
using Common.Package.Serializer;
using System.ServiceModel.Channels;

namespace Common.Communication.Wcf.Encoders
{
    /// <summary>
    /// 二进制的编码方式
    /// </summary>
    class BinaryWcfMessageEncoder : WcfMessageEncoderBase
    {
        public BinaryWcfMessageEncoder()
            : base(DataFormat.Binary)
        {

        }

        private const int MAX_BODY_LENGTH = WcfSettings.MaxMessageSize;
        private const int MAX_STRING_LENGHT = byte.MaxValue;

        public override string GetContentType()
        {
            return "application/octet-stream";
        }

        public override ArraySegment<byte> Serialize(EntityMessage message, int maxMessageSize, BufferManager bufferManager, int messageOffset, IWcfMessageEncoderStrategy strategy)
        {
            MemoryStream ms = new MemoryStream(1024);
            ms.WriteByte((byte)1);  // 第一个用1来表示是二进制编码

            string action = message.Headers.Action, method = message.Method;
            UniqueId messageId = message.Headers.MessageId, relatesTo = message.Headers.RelatesTo;
            if (strategy.Require(EntityMessageHeader.Action) && !string.IsNullOrEmpty(action) && action != WcfRequestActions.RequestReply)
                _WriteAction(ms, MessageHeader.Action, action);

            if (strategy.Require(EntityMessageHeader.MessageID) && messageId != null)
                _WriteUniqueId(ms, MessageHeader.MessageId, messageId);

            if (strategy.Require(EntityMessageHeader.RelatesTo) && relatesTo != null)
                _WriteUniqueId(ms, MessageHeader.Relatesto, relatesTo);

            if (strategy.Require(EntityMessageHeader.Method) && !string.IsNullOrEmpty(method))
                _WriteString(ms, MessageHeader.Method, method);

            if (strategy.Require(EntityMessageHeader.ID))
                _WriteInt(ms, MessageHeader.ID, message.ID);

            CommunicateData d = message.Data;
            if (strategy.Require(EntityMessageHeader.StatusCode) && d.StatusCode != EncoderSettings.DefaultStatusCode)
            {
                _WriteInt(ms, MessageHeader.StatusCode, d.StatusCode);
            }

            if (strategy.Require(EntityMessageHeader.StatusDesc) && !string.IsNullOrEmpty(d.StatusDesc) && d.StatusDesc != EncoderSettings.DefaultStatusDesc)
            {
                _WriteString(ms, MessageHeader.StatusDesc, d.StatusDesc);
            }

            if (strategy.Require(EntityMessageHeader.Settings) && !CallingSettings.IsDefault(message.Settings))
            {
                ms.WriteByte((byte)MessageHeader.Settings);
                message.Settings.WriteToStream(ms);
            }

            if (strategy.Require(EntityMessageHeader.SessionId) && message.SessionId != Guid.Empty)
            {
                ms.WriteByte((byte)MessageHeader.SessionId);
                ms.Write(message.SessionId);
            }

            if (strategy.Require(EntityMessageHeader.Sid) && !d.Sid.IsEmpty())
            {
                _WriteString(ms, MessageHeader.Sid, d.Sid.ToString());
            }

            ServiceEndPoint caller;
            if (strategy.Require(EntityMessageHeader.Caller) && (caller = message.Caller) != null)
            {
                ms.WriteByte((byte)MessageHeader.Caller);
                ms.Write(caller.ClientId);

                if (caller.ServiceDesc != null)
                {
                    _WriteString(ms, MessageHeader.Caller, caller.ServiceDesc.Name ?? "", false);
                    ms.Write((uint)caller.ServiceDesc.Version);
                }
                else
                {
                    _WriteString(ms, MessageHeader.Caller, "", false);
                }
            }

            byte[] bodyBuffer = (message.Data.Data as byte[]) ?? new byte[0];
            if (bodyBuffer.Length > 0)
            {
                if (bodyBuffer.Length > MAX_BODY_LENGTH)
                    throw _CreateArgumentException("BODY长度" + bodyBuffer.Length + "超出范围");

                _WriteBytes(ms, MessageHeader.Body, bodyBuffer);
            }

            byte[] buffer = bufferManager.TakeBuffer((int)ms.Length + messageOffset);
            byte[] msBuffer = ms.GetBuffer();
            Buffer.BlockCopy(msBuffer, 0, buffer, messageOffset, (int)ms.Length);
            return new ArraySegment<byte>(buffer, messageOffset, (int)ms.Length);
        }

        private static void _WriteBytes(MemoryStream ms, MessageHeader header, byte[] buffer)
        {
            ms.WriteByte((byte)header);
            ms.Write(_IntToBytes((int)buffer.Length));
            ms.Write(buffer);
        }

        private static void _WriteShort(MemoryStream ms, MessageHeader header, short data)
        {
            ms.WriteByte((byte)header);
            ms.WriteByte((byte)data);
            ms.WriteByte((byte)(data >> 8));
        }

        private static byte[] _IntToBytes(int number)
        {
            return BufferUtility.ToBytes(number);
        }

        private static int _BytesToInt(byte[] bytes)
        {
            return BufferUtility.ToInt32(bytes);
        }

        private static void _WriteInt(MemoryStream ms, MessageHeader header, int number)
        {
            ms.WriteByte((byte)header);
            ms.Write(_IntToBytes(number));
        }

        private static void _WriteAction(MemoryStream ms, MessageHeader header, string s)
        {
            ms.WriteByte((byte)header);
            ms.WriteByte((byte)s[0]);
        }

        private static void _WriteUniqueId(MemoryStream ms, MessageHeader header, UniqueId id)
        {
            ms.WriteByte((byte)header);
            Guid g;

            if (id.TryGetGuid(out g))
            {
                ms.WriteByte((byte)0);
                ms.Write(g.ToByteArray());
            }
            else
            {
                byte[] buffer = Encoding.UTF8.GetBytes(id.ToString());
                ms.WriteByte((byte)buffer.Length);
                ms.Write(buffer);
            }
        }

        private static void _WriteString(MemoryStream ms, MessageHeader header, string s, bool withHeader = true)
        {
            if (withHeader)
                ms.WriteByte((byte)header);

            byte[] buffer = Encoding.UTF8.GetBytes(s);
            if (buffer.Length > MAX_STRING_LENGHT)
                throw _CreateArgumentException(header.ToString() + "长度" + buffer.Length + "超出范围");

            ms.WriteByte((byte)buffer.Length);
            ms.Write(buffer);
        }

        private static ServiceException _CreateArgumentException(string errorMsg = null)
        {
            return new ServiceException(ServerErrorCode.ArgumentError, errorMsg ?? "消息格式错误");
        }

        public override void Deserialize(ref EntityMessage message, byte[] buffer, int offset, int count, IWcfMessageEncoderStrategy strategy)
        {
            MemoryStream ms = new MemoryStream(buffer, offset, count);
            if (ms.ReadByte() != 1)  // 最开头的1，用于标明这是二进制
                throw _CreateArgumentException("二进制编码第一个字节必须为1");

            int statusCode = EncoderSettings.DefaultStatusCode;
            CallingSettings settings = null;
            string statusDesc = null, sid = null;
            byte[] bodyBuffer = null;
            ServiceEndPoint caller = null;
            Guid sessionId = Guid.Empty;

            while (ms.Position < count)
            {
                MessageHeader h = (MessageHeader)ms.ReadByte();
                switch (h)
                {
                    case MessageHeader.Action:
                        message.Headers.Action = ((char)ms.ReadByte()).ToString();
                        break;

                    case MessageHeader.MessageId:
                        message.Headers.MessageId = _ReadUniqueId(ms);
                        break;

                    case MessageHeader.Relatesto:
                        message.Headers.RelatesTo = _ReadUniqueId(ms);
                        break;

                    case MessageHeader.Method:
                        message.Method = _ReadString(ms);
                        break;

                    case MessageHeader.ID:
                        message.ID = _ReadInt(ms);
                        break;

                    case MessageHeader.StatusCode:
                        statusCode = _ReadInt(ms);
                        break;

                    case MessageHeader.StatusDesc:
                        statusDesc = _ReadString(ms);
                        break;

                    case MessageHeader.Sid:
                        sid = _ReadString(ms);
                        break;

                    case MessageHeader.Settings:
                        settings = EncoderUtility.ReadFromStream(ms);
                        break;

                    case MessageHeader.Caller:
                        string serviceName;
                        caller = new ServiceEndPoint(_ReadGuid(ms),
                            new ServiceDesc(serviceName = _ReadString(ms), 
                                string.IsNullOrEmpty(serviceName) ? SVersion.Empty : new SVersion((uint)_ReadInt(ms))
                            )
                        );
                        break;

                    case MessageHeader.SessionId:
                        sessionId = _ReadGuid(ms);
                        break;

                    case MessageHeader.Body:
                        bodyBuffer = _ReadBytes(ms);
                        break;

                    default:
                        throw _CreateArgumentException("不被识别的信息头: " + h);
                }
            }

            if (statusDesc == null && statusCode == EncoderSettings.DefaultStatusCode)
                statusDesc = EncoderSettings.DefaultStatusDesc;

            message.Data = new CommunicateData(bodyBuffer, Format, statusCode: statusCode, statusDesc: statusDesc, sid: Sid.Parse(sid));
            message.Settings = settings ?? CallingSettings.RequestReply;
            message.Caller = caller;

            if (string.IsNullOrEmpty(message.Headers.Action))
                message.Headers.Action = WcfRequestActions.RequestReply;
        }

        private static short _ReadShort(MemoryStream ms)
        {
            return (short)(ms.ReadByte() | (ms.ReadByte() << 8));
        }

        private static string _ReadString(MemoryStream ms)
        {
            byte[] buffer = new byte[ms.ReadByte()];
            ms.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }

        private UniqueId _ReadUniqueId(MemoryStream ms)
        {
            int length = ms.ReadByte();
            if (length == 0)
            {
                byte[] buffer = new byte[16];
                ms.Read(buffer, 0, buffer.Length);
                return new UniqueId(buffer);
            }
            else
            {
                byte[] buffer = new byte[length];
                ms.Read(buffer, 0, buffer.Length);
                return new UniqueId(Encoding.UTF8.GetString(buffer));
            }
        }

        public Guid _ReadGuid(MemoryStream ms)
        {
            return ms.ReadGuid();
        }

        private static int _ReadInt(MemoryStream ms)
        {
            byte[] bytes = new byte[4];
            ms.Read(bytes, 0, 4);
            return _BytesToInt(bytes);
        }

        private static byte[] _ReadBytes(MemoryStream ms)
        {
            int bodyLength = _ReadInt(ms);
            if (bodyLength > 0)
            {
                if (bodyLength > MAX_BODY_LENGTH)
                    throw _CreateArgumentException("长度" + bodyLength + "超出范围");

                byte[] bodyBuffer = new byte[bodyLength];
                ms.Read(bodyBuffer, 0, bodyLength);
                return bodyBuffer;
            }

            throw _CreateArgumentException();
        }

        private static UniqueId _ParseUniqueId(string s)
        {
            return EncoderUtility.ParseUniqueId(s);
        }

        #region Enum MessageHeader ...

        enum MessageHeader : byte
        {
            /// <summary>
            /// Action
            /// </summary>
            Action = 1,

            /// <summary>
            /// MessageId
            /// </summary>
            MessageId = 2,

            /// <summary>
            /// Relatesto
            /// </summary>
            Relatesto = 3,

            /// <summary>
            /// 安全码
            /// </summary>
            Sid = 246,

            /// <summary>
            /// 会话标识
            /// </summary>
            SessionId = 247,

            /// <summary>
            /// 调用者
            /// </summary>
            Caller = 248,

            /// <summary>
            /// 调用设置
            /// </summary>
            Settings = 249,

            /// <summary>
            /// Id，可用于路由
            /// </summary>
            ID = 250,

            /// <summary>
            /// 信息
            /// </summary>
            StatusDesc = 251,

            /// <summary>
            /// 状态码
            /// </summary>
            StatusCode = 252,

            /// <summary>
            /// 方法
            /// </summary>
            Method = 253,

            /// <summary>
            /// 消息体
            /// </summary>
            Body = 254,
        }

        #endregion

    }
}
