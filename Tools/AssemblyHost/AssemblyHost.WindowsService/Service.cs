using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.IO;

namespace AssemblyHost.WindowsService
{
    /// <summary>
    /// 服务
    /// </summary>
    public partial class Service : ServiceBase
    {
        public Service()
        {
            InitializeComponent();

            ServiceName = Settings.ServiceName;
            CanShutdown = Settings.CanShutDown;
            CanStop = Settings.CanStop;
            CanPauseAndContinue = Settings.CanPauseAndContinue;
            CanHandlePowerEvent = Settings.CanHandlePowerEvent;
            CanHandleSessionChangeEvent = Settings.CanHandleSessionChangeEvent;
            AutoLog = Settings.AutoLog;

            _thread = new Thread(_RunningFunc) { IsBackground = true };
        }

        private readonly ManualResetEvent _waitForExitEvent = new ManualResetEvent(false);
        private Thread _thread;
        private volatile int _exitCode;

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
                            string.Format("\"/cmd:{0}\" \"/ws:{1}\" \"/we:{2}\"",
                            action, Settings.ServiceName, AssemblyHostUtility.GetCurrentProcessName()));
                    }
                    else if (action == "LiveUpdate")
                    {
                        _e.Set();
                        AssemblyHostUtility.StartProcess(liveUpdateExe,
                            string.Format("\"/cmd:{0}\" \"/ws:{1}:{2}\" \"/kill:{3}\" \"/s:{1}\"",
                            action, Settings.ServiceName, AssemblyHostUtility.GetCurrentProcessId(),
                            AssemblyHostUtility.GetKillProcessIds()
                        ));
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Log(ex);
                }
            }
        }

        private void _RunningFunc()
        {
            try
            {
                LogManager.Log("AssemblyHost.WindowsService 开始启动 ...");
                AssemblyHostApplication app = new AssemblyHostApplication(null, _waitForExitEvent, new Callback(_waitForExitEvent).Execute, new string[0]);
                _exitCode = app.Run();
            }
            catch (Exception ex)
            {
                LogManager.Log(ex);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                AssemblyHostUtility.DoLiveUpdateRest(args);

                _waitForExitEvent.Reset();
                _thread.Start();
            }
            catch (Exception ex)
            {
                LogManager.Log(ex);
            }
        }

        protected override void OnStop()
        {
            _waitForExitEvent.Set();
        }
    }
}
