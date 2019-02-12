using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Common.Contracts.Service;
using System.Net.Security;

namespace Common.Communication.Wcf.Service
{
    /// <summary>
    /// 双向通信
    /// </summary>
    [ServiceContract(Name = "srv", CallbackContract = typeof(IWcfServiceCallback))]
    interface IDuplexWcfServiceInterface : IWcfServiceInterface
    {

    }

    /// <summary>
    /// 回调
    /// </summary>
    interface IWcfServiceCallback
    {
        /// <summary>
        /// 单向通道
        /// </summary>
        /// <param name="input"></param>
        [OperationContract(Action = WcfRequestActions.OneWay, IsOneWay = true)]
        void OneWay(Message input);

        /// <summary>
        /// 请求应答
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract(Action = WcfRequestActions.RequestReply, ReplyAction = "*")]
        Message Request(Message input);
    }

    /// <summary>
    /// 回调的实现
    /// </summary>
    class WcfServiceCallback : IWcfServiceCallback
    {
        public WcfServiceCallback(WcfConnection owner)
        {
            _owner = owner;
        }

        private readonly WcfConnection _owner;

        public void OneWay(Message input)
        {
            _owner.RaiseDataReceivedEvent((EntityMessage)input);
        }

        public Message Request(Message input)
        {
            EntityMessage inputMsg = (EntityMessage)input;
            CommunicateData replyData = _owner.RaiseDataReceivedEvent(inputMsg);

            EntityMessage outputMsg = new EntityMessage(replyData);
            outputMsg.Headers.RelatesTo = input.Headers.MessageId;
            outputMsg.Headers.ReplyTo = input.Headers.ReplyTo;
            outputMsg.Headers.MessageId = null;

            return outputMsg;
        }
    }
}
