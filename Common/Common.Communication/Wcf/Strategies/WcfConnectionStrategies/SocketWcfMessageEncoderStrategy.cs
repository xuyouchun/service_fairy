using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf.Encoders;

namespace Common.Communication.Wcf.Strategies.WcfConnectionStrategies
{
    class SocketWcfMessageEncoderStrategy : IWcfMessageEncoderStrategy
    {
        public bool Require(Service.EntityMessageHeader header)
        {
            return true;
        }

        public bool OnewayWhenNoMessageId()
        {
            return true;
        }

        public static readonly SocketWcfMessageEncoderStrategy Instance = new SocketWcfMessageEncoderStrategy();
    }
}
