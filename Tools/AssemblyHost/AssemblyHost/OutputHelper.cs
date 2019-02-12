using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyHost
{
    static class OutputHelper
    {
        public static void WriteLine(this IOutput output, string s)
        {
            output.Write(s + "\r\n");
        }

        public static void WriteError(this IOutput output, Exception error)
        {
            output.Write(error.ToString() + "\r\n");
        }
    }
}
