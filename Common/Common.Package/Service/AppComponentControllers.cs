using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Utility;
using Common.Contracts;
using Common.Package.GlobalTimer;

namespace Common.Package.Service
{
    /// <summary>
    /// 具有定时任务功能的组件控制器
    /// </summary>
    public class TimerAppComponentController : IAppComponentController
    {
        public TimerAppComponentController(string taskName, ITimerStrategy timerStrategy, bool enableReenter = false)
        {
            _taskName = taskName;
            _handle = GlobalTimer<ITask>.Default.Add(timerStrategy, new TaskFuncAdapter(_Execute), null, enableReenter, false);
        }

        public TimerAppComponentController(string taskName, TimeSpan interval, TimeSpan dueTime, bool enableReenter = false)
        {
            _taskName = taskName;

            if (interval > TimeSpan.Zero)
                _handle = GlobalTimer<ITask>.Default.Add(interval, dueTime, new TaskFuncAdapter(_Execute), false, false);
        }

        private readonly IGlobalTimerTaskHandle _handle;
        private readonly string _taskName;
        private IAppComponentTaskFunction _taskService;
        private IAppComponentControllerContext _context;

        private void _Execute()
        {
            if (_taskService != null)
                _taskService.Execute(_taskName);
        }

        public void ExecuteImmediately()
        {
            if (_handle != null)
                _handle.ExecuteImmediately();
        }

        public void Init(IAppComponentControllerContext context)
        {
            _context = context;
            _taskService = context.ServiceProvider.GetService<IAppComponentTaskFunction>();
        }

        public void Apply()
        {
            if (_handle != null)
                _handle.Start();
        }

        public void Abolish()
        {
            if (_handle != null)
                _handle.Stop();
        }

        public void Dispose()
        {
            if (_handle != null)
                _handle.Dispose();
        }
    }

    /// <summary>
    /// 默认的状态控制器
    /// </summary>
    public class DefaultStatusAppComponentControler : IAppComponentController
    {
        public DefaultStatusAppComponentControler()
        {

        }

        public void Init(IAppComponentControllerContext context)
        {
            _service = context.ServiceProvider.GetService<IAppComponentStatusFunction>();
        }

        private IAppComponentStatusFunction _service;

        private void _SetStatus(AppComponentStatus status)
        {
            if (_service != null)
                _service.Status = status;
        }

        public void Apply()
        {
            _SetStatus(AppComponentStatus.Enable);
        }

        public void Abolish()
        {
            _SetStatus(AppComponentStatus.Disable);
        }

        public void Dispose()
        {
            _SetStatus(AppComponentStatus.Disable);
        }
    }
}
