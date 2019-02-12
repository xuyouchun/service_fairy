using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf.Service;

namespace Common.Communication.Wcf.Encoders
{
    class HttpWcfMessageEncoderStrategy : IWcfMessageEncoderStrategy
    {
        private static readonly HashSet<EntityMessageHeader> _requireHeaders = new HashSet<EntityMessageHeader>(
            new EntityMessageHeader[]{
                EntityMessageHeader.Action, EntityMessageHeader.Method,
                EntityMessageHeader.StatusCode, EntityMessageHeader.StatusDesc
        });

        public bool Require(EntityMessageHeader header)
        {
            return _requireHeaders.Contains(header);
        }

        public bool OnewayWhenNoMessageId()
        {
            return false;
        }

        public static readonly HttpWcfMessageEncoderStrategy Instance = new HttpWcfMessageEncoderStrategy();
    }
}
