using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;

namespace Common.Package.Storage
{
    static class StreamTableSettings
    {
        public static readonly uint SIGN = BufferUtility.ToUInt32(Encoding.ASCII.GetBytes("BHJL"));
    }
}
