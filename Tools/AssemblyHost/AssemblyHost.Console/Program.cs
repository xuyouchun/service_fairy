using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Diagnostics;
using AssemblyHost;

namespace AssemblyHost.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ManualResetEvent e = new ManualResetEvent(false);
            try
            {
                AssemblyHostUtility.DoLiveUpdateRest(args);

                LogManager.Log("AssemblyHost.Console 开始启动 ...");
                AssemblyHostApplication app = new AssemblyHostApplication(new Output(), e, new Callback(e).Execute, args);
                Environment.ExitCode = app.Run();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                System.Console.ReadKey();
            }
        }

        class Callback : MarshalByRefObject
        {
            public Callback(ManualResetEvent e)
            {
                _e = e;
            }

            private readonly ManualResetEvent _e;

            public override object InitializeLifetimeService()
            {
                return null;
            }

            public void Execute(string action, string[] args)
            {
                LogManager.Log("Command " + action);

                try
                {
                    string liveUpdateExe = Path.Combine(AssemblyHostUtility.GetExecutePath(), "LiveUpdate.exe");

                    if (action == "Exit")
                    {
                        _e.Set();
                    }
                    else if (action == "Restart")
                    {
                        _e.Set();
                        AssemblyHostUtility.StartProcess(liveUpdateExe,
                            string.Format("\"/cmd:{0}\" \"/weid:{1}\" ", action, AssemblyHostUtility.GetWaitExitProcessIds()));
                    }
                    else if (action == "LiveUpdate")
                    {
                        _e.Set();
                        AssemblyHostUtility.StartProcess(liveUpdateExe,
                            string.Format("\"/cmd:{0}\" \"/weid:{1}\" \"/kill:{2}\" \"/e:{3}\"",
                            action, AssemblyHostUtility.GetWaitExitProcessIds(), AssemblyHostUtility.GetKillProcessIds(), AssemblyHostUtility.GetExecuteFile()));
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Log(ex);
                }
            }
        }

        class Output : IOutput
        {
            public void Write(string s)
            {
                System.Console.Write(s);
            }
        }
    }
}
