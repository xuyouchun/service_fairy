using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;

namespace Common.Communication.Wcf.Encoders
{
    static class EncoderSettings
    {
        public const int DefaultStatusCode = (int)ServiceStatusCode.Ok;

        public static readonly string DefaultStatusDesc = ServiceStatusCode.Ok.GetDesc();
    }
}
