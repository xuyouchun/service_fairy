﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf.Service;
using Common.Contracts.Service;

namespace Common.Communication.Wcf.Strategies.WcfConnectionStrategies
{
    /// <summary>
    /// Socket连接的基类
    /// </summary>
    class SocketWcfConnection : WcfConnectionBase
    {
        public SocketWcfConnection(WcfCommunicationStrategyBase owner, WcfListener listener, CommunicationOption option)
            : base(owner, listener, option)
        {

        }

        protected internal override EntityMessage Reply(EntityMessage reqMsg, CommunicateData rspData)
        {
            if (reqMsg.Headers.Action == WcfRequestActions.RequestReply)
            {
                EntityMessage msg = Owner.CreateMessage(Option);
                msg.Data = rspData;
                msg.Headers.RelatesTo = reqMsg.Headers.MessageId;
                msg.Headers.ReplyTo = reqMsg.Headers.ReplyTo;
                msg.Headers.MessageId = null;

                return msg;
            }

            return null;
        }
    }
}