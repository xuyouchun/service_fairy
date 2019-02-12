using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.IO;
using System.Threading;

namespace AssemblyHost
{
    public static class LogManager
    {
        public static void Log(string format, params object[] args)
        {
            Log(string.Format(format, args));
        }

        private static readonly string _LogFile = Path.Combine(AssemblyHostUtility.GetExecutePath(), "AssemblyHost.log");

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void Log(string msg)
        {
            try
            {
                File.AppendAllText(_LogFile,
                    string.Format("[{0}] <AssemblyHost> {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msg));
            }
            catch { }
        }

        public static void Log(Exception error)
        {
            if (error == null || error is ThreadAbortException)
                return;

            Log(error.ToString());
        }
    }
}
