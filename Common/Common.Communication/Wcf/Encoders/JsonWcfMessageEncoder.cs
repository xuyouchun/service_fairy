using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf.Service;
using System.IO;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Utility;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml;
using Common.Package.Serializer;
using System.ServiceModel.Channels;

namespace Common.Communication.Wcf.Encoders
{
    class JsonWcfMessageEncoder : WcfMessageEncoderBase
    {
        public JsonWcfMessageEncoder()
            : base(DataFormat.Json)
        {

        }

        public override string GetContentType()
        {
            return "text/json";
        }

        public override ArraySegment<byte> Serialize(EntityMessage message, int maxMessageSize, BufferManager bufferManager, int messageOffset, IWcfMessageEncoderStrategy strategy)
        {
            Writter w = new Writter();
            string action = message.Headers.Action;
            if (strategy.Require(EntityMessageHeader.Action) && !string.IsNullOrEmpty(action) && action != WcfRequestActions.RequestReply)
                w.WriteNode(Names.ActionBytes, message.Headers.Action);

            if (strategy.Require(EntityMessageHeader.Method) && !string.IsNullOrEmpty(message.Method))
                w.WriteNode(Names.MethodBytes, message.Method);

            CommunicateData d = message.Data;
            if (strategy.Require(EntityMessageHeader.StatusCode) && d.StatusCode != EncoderSettings.DefaultStatusCode)
                w.WriteNode(Names.StatusCodeBytes, (int)d.StatusCode);

            if (strategy.Require(EntityMessageHeader.StatusDesc) && !string.IsNullOrEmpty(d.StatusDesc) && d.StatusDesc != EncoderSettings.DefaultStatusDesc)
                w.WriteNode(Names.StatusDescBytes, d.StatusDesc);

            if (strategy.Require(EntityMessageHeader.Sid) && !d.Sid.IsEmpty())
                w.WriteNode(Names.SIDBytes, d.Sid.ToString());

            UniqueId messageId = message.Headers.MessageId, relatesTo = message.Headers.RelatesTo;
            if (strategy.Require(EntityMessageHeader.MessageID) && messageId != null)
                w.WriteNode(Names.MessageIDBytes, messageId);

            if (strategy.Require(EntityMessageHeader.RelatesTo) && relatesTo != null)
                w.WriteNode(Names.RelatesToBytes, relatesTo);

            if (strategy.Require(EntityMessageHeader.Settings) && !CallingSettings.IsDefault(message.Settings))
                w.WriteNode(Names.SettingsBytes, message.Settings);

            if (strategy.Require(EntityMessageHeader.ID) && message.ID != 0)
                w.WriteNode(Names.IDBytes, message.ID);

            if (strategy.Require(EntityMessageHeader.SessionId) && message.SessionId != Guid.Empty)
                w.WriteNode(Names.SessionIdBytes, message.SessionId.ToString());

            if (strategy.Require(EntityMessageHeader.Caller) && message.Caller != null)
                w.WriteNode(Names.CallerBytes, message.Caller.ToString());

            byte[] dataBuffer = _GetBufferBytes(d.Data);
            if (!dataBuffer.IsNullOrEmpty())
                w.WriteNode(Names.BodyBytes, dataBuffer);

            ArraySegment<byte> msgBuffer = w.GetBytes();
            byte[] buffer = bufferManager.TakeBuffer(msgBuffer.Count + messageOffset);
            Buffer.BlockCopy(msgBuffer.Array, 0, buffer, messageOffset, msgBuffer.Count);
            return new ArraySegment<byte>(buffer, messageOffset, msgBuffer.Count);
        }

        private byte[] _GetBufferBytes(object buffer)
        {
            if (buffer == null)
                return null;

            byte[] bytes = buffer as byte[];
            if (bytes != null)
                return bytes;

            Stream stream = buffer as Stream;
            if (stream != null)
                return stream.ToBytes();

            throw new NotSupportedException("不支持的格式:" + buffer.GetType());
        }

        #region Class Writter ...

        class Writter
        {
            public Writter()
            {
                _ms.Write(_StartBytes);
            }

            private static readonly byte[] _nodeHeaderBytes = _GetBytes(":");
            private static readonly byte[] _nodeSplitterBytes = _GetBytes(",");
            private static readonly byte[] _nullBytes = _GetBytes("null");
            private static readonly byte[] _StartBytes = _GetBytes("{"), _EndBytes = _GetBytes("}");
            private int _nodeCount = 0;

            private readonly MemoryStream _ms = new MemoryStream();

            private void _WriteHeader(byte[] nodeNameBytes)
            {
                if ((_nodeCount++) > 0)
                    _ms.Write(_nodeSplitterBytes);

                _ms.Write(nodeNameBytes);
                _ms.Write(_nodeHeaderBytes);
            }

            private void _WriteFootter()
            {
                
            }

            public void WriteNode(byte[] nodeNameBytes, object value)
            {
                WriteNode(nodeNameBytes, _GetNodeBytes(value == null ? "" : value.ToString()));
            }

            public void WriteNode(byte[] nodeNameBytes, CallingSettings settings)
            {
                WriteNode(nodeNameBytes, Convert.ToBase64String(settings.ToBytes()));
            }

            public void WriteNode(byte[] nodeNameBytes, byte[] buffer)
            {
                _WriteHeader(nodeNameBytes);
                _ms.Write(buffer ?? _nullBytes);
                _WriteFootter();
            }

            public ArraySegment<byte> GetBytes()
            {
                _ms.Write(_EndBytes);
                return new ArraySegment<byte>(_ms.GetBuffer(), 0, (int)_ms.Length);
            }
        }

        #endregion

        public override unsafe void Deserialize(ref EntityMessage message, byte[] buffer, int offset, int count, IWcfMessageEncoderStrategy strategy)
        {
            byte[] body;
            EntityMessageHeaderData d;
            fixed (byte* pBuffer = buffer)
            {
                //d = JsonUtility.Deserialize<EntityMessageHeaderData>(buffer, offset, count);
                byte* p0 = pBuffer + offset, p = p0, pBodyStart, pBodyEnd, pNameStart, pNameEnd;
                if (_FindBody(ref p, count, out pNameStart, out pNameEnd, out pBodyStart, out pBodyEnd))
                {
                    body = BufferUtility.ToBytes(pBodyStart, (int)(pBodyEnd - pBodyStart));

                    byte* p1 = pNameStart - 1;
                    bool commaAtFront = false;
                    if (BufferUtility.BackSkipWhiteSpace(ref p1, (int)(p1 - p0)) && *p1 == (byte)',')
                    {
                        p1--;
                        commaAtFront = true;
                    }

                    if (BufferUtility.SkipWhiteSpace(ref pBodyEnd, (int)((p0 + count) - pBodyEnd)) && *pBodyEnd == (byte)',' && !commaAtFront)
                        pBodyEnd++;

                    int len1 = (int)(p1 - p0 + 1), len2 = (count - (int)(pBodyEnd - p0));
                    BufferUtility.Copy(p0 + len1, pBodyEnd, len2);
                    d = JsonUtility.Deserialize<EntityMessageHeaderData>(buffer, offset, len1 + len2);
                }
                else
                {
                    d = JsonUtility.Deserialize<EntityMessageHeaderData>(buffer, offset, count);
                    body = null;
                }
            }

            message.Headers.Action = d.Action ?? WcfRequestActions.RequestReply;
            message.Method = d.Method;
            message.Headers.MessageId = EncoderUtility.ParseUniqueId(d.MessageId);
            message.Headers.RelatesTo = EncoderUtility.ParseUniqueId(d.RelatesTo);
            message.SessionId = d.SessionId;
            message.Settings = string.IsNullOrEmpty(d.Settings) ?
                CallingSettings.RequestReply : EncoderUtility.ReadFromBytes(Convert.FromBase64String(d.Settings));

            if (!d.SID.IsEmpty())
                message.Settings = CallingSettings.FromPrototype(message.Settings, d.SID);

            message.Data = new CommunicateData(body, DataFormat.Json,
                d.StatusCode == (int)ServiceStatusCode.Ok ? EncoderSettings.DefaultStatusCode : (ServiceStatusCode)d.StatusCode,
                statusDesc: d.StatusDesc ?? EncoderSettings.DefaultStatusDesc, sid:d.SID
            );

            if (!string.IsNullOrEmpty(d.Caller))
                message.Caller = ServiceEndPoint.Parse(d.Caller);
             
            // 不带有messageId的请求都认为为单向请求
            if (strategy.OnewayWhenNoMessageId() && message.Headers.MessageId == null)
            {
                message.Headers.MessageId = new UniqueId(Guid.NewGuid());
                message.Headers.Action = WcfRequestActions.OneWay;
            }
        }

        private unsafe bool _FindBody(ref byte* p, int count,
            out byte *pNameStart, out byte *pNameEnd, out byte* pBodyStart, out byte* pBodyEnd)
        {
            pNameStart = pNameEnd = pBodyStart = pBodyEnd = null;
            byte* p2 = p + count;

            if (!BufferUtility.SkipWhiteSpace(ref p, count))
                return false;

            if (*p != (byte)'{')
                return false;

            p++;
            return JsonWcfMessageEncoderHelper.SearchNode(ref p, (int)(p2 - p), Names.Body, out pNameStart, out pNameEnd, out pBodyStart, out pBodyEnd);
        }

        #region Class EntityMessageHeaderData ...

        [DataContract]
        class EntityMessageHeaderData
        {
            [DataMember(Name = Names.Action)]
            public string Action;

            [DataMember(Name = Names.StatusCode)]
            public int StatusCode;

            [DataMember(Name = Names.StatusDesc)]
            public string StatusDesc;

            [DataMember(Name = Names.Method)]
            public string Method;

            [DataMember(Name = Names.MessageID)]
            public string MessageId;

            [DataMember(Name = Names.RelatesTo)]
            public string RelatesTo;

            [DataMember(Name = Names.ID)]
            public int ID;

            [DataMember(Name = Names.SID)]
            public Sid SID;

            [DataMember(Name = Names.Settings)]
            public string Settings;

            [DataMember(Name = Names.SessionId)]
            public Guid SessionId;

            [DataMember(Name = Names.Caller)]
            public string Caller;
        }

        #endregion

        #region Class Reader ...

        class Reader
        {
            public Reader(EntityMessage message)
            {
                _message = message;
            }

            private readonly EntityMessage _message;
        }

        #endregion

        private static readonly byte[] _emptyBytes = _GetBytes("\"\"");

        private static byte[] _GetNodeBytes(string s)
        {
            if (string.IsNullOrEmpty(s))
                return _emptyBytes;

            return _GetBytes("\"" + s.Replace("\"", "\\\"").Replace(@"\", @"\\") + "\"");
        }

        private static byte[] _GetBytes(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }

        #region Class Names ...

        static class Names
        {
            /// <summary>
            /// Action
            /// </summary>
            public const string Action = "action";

            public static readonly byte[] ActionBytes = _GetNodeBytes(Action);

             /// <summary>
            /// 信息
            /// </summary>
            public const string StatusDesc = "statusDesc";

            public static readonly byte[] StatusDescBytes = _GetNodeBytes(StatusDesc);

            /// <summary>
            /// 状态码
            /// </summary>
            public const string StatusCode = "statusCode";

            public static readonly byte[] StatusCodeBytes = _GetNodeBytes(StatusCode);

            /// <summary>
            /// 方法
            /// </summary>
            public const string Method = "method";

            public static readonly byte[] MethodBytes = _GetNodeBytes(Method);

            /// <summary>
            /// 消息体
            /// </summary>
            public const string Body = "body";

            public static readonly byte[] BodyBytes = _GetNodeBytes(Body);

            /// <summary>
            /// 调用设置
            /// </summary>
            public const string Settings = "settings";

            public static readonly byte[] SettingsBytes = _GetNodeBytes(Settings);

            /// <summary>
            /// 消息ID
            /// </summary>
            public const string MessageID = "messageId";

            public static readonly byte[] MessageIDBytes = _GetNodeBytes(MessageID);

            /// <summary>
            /// 关联消息ID
            /// </summary>
            public const string RelatesTo = "relatesTo";

            public static readonly byte[] RelatesToBytes = _GetNodeBytes(RelatesTo);

            /// <summary>
            /// ID
            /// </summary>
            public const string ID = "id";

            public static readonly byte[] IDBytes = _GetNodeBytes(ID);

            /// <summary>
            /// SID
            /// </summary>
            public const string SID = "sid";

            public static readonly byte[] SIDBytes = _GetNodeBytes(SID);

            /// <summary>
            /// SessionId
            /// </summary>
            public const string SessionId = "sessionId";

            public static readonly byte[] SessionIdBytes = _GetNodeBytes(SessionId);

            /// <summary>
            /// Caller
            /// </summary>
            public const string Caller = "caller";

            public static readonly byte[] CallerBytes = _GetNodeBytes(Caller);
        }

        #endregion
    }
}
