using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package;
using Common.Package.GlobalTimer;

namespace Common.Package.Service
{
    /// <summary>
    /// 具有定时任务的组件
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public abstract class TimerAppComponentBase : AppComponentBaseEx
    {
        public TimerAppComponentBase(object owner, TimeSpan interval = default(TimeSpan), TimeSpan dueTime = default(TimeSpan), bool enableReenter = false)
            : base(owner)
        {
            TimerController = new TimerAppComponentController(null, interval, dueTime, enableReenter);
        }

        protected override IAppComponentController[] CreateControllers()
        {
            return new IAppComponentController[] { TimerController, new DefaultStatusAppComponentControler() };
        }

        /// <summary>
        /// 定时任务控制器
        /// </summary>
        public TimerAppComponentController TimerController { get; private set; }

        protected void ExecuteImmediately()
        {
            TimerController.ExecuteImmediately();
        }
    }
}
