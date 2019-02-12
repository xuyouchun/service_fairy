using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication.Wcf.Service;

namespace Common.Communication.Wcf.Encoders
{
    interface IWcfMessageEncoderStrategy
    {
        bool Require(EntityMessageHeader header);

        /// <summary>
        /// 是否当MessageId为空时，便默认为Oneway
        /// </summary>
        /// <returns></returns>
        bool OnewayWhenNoMessageId();
    }
}
