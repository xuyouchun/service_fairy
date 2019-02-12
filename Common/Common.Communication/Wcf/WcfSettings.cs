using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;

namespace Common.Communication.Wcf
{
    static class WcfSettings
    {
        public static readonly MessageVersion MessageVersion = MessageVersion.Default;

        public const int MaxBufferPoolSize = 64 * MaxMessageSize;

        public const int MaxMessageSize = 5 * 1024 * 1024;

        public const int MaxBufferSize = MaxMessageSize;

        public const string ContentType = "application/octet-stream";

        public const string MediaType = "application/octet-stream";
    }
}
