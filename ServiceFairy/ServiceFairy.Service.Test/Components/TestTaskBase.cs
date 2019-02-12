using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Test;

namespace ServiceFairy.Service.Test.Components
{
    abstract class TestTaskBase : ITestTask
    {
        public virtual void Validate(string args)
        {
            
        }

        private readonly object _thisLock = new object();
        private volatile bool _running = false;

        void ITestTask.Start(string args)
        {
            lock (_thisLock)
            {
                if (!_running)
                {
                    _running = true;
                    OnStart(new TaskArgsParser(args));
                }
            }
        }

        /// <summary>
        /// 开始运行
        /// </summary>
        /// <param name="args"></param>
        protected abstract void OnStart(TaskArgsParser args);

        void ITestTask.Stop()
        {
            lock (_thisLock)
            {
                if (_running)
                {
                    _running = false;
                    OnStop();
                }
            }
        }

        /// <summary>
        /// 结束运行
        /// </summary>
        protected abstract void OnStop();

        /// <summary>
        /// 是否正在运行
        /// </summary>
        bool ITestTask.Running
        {
            get { return _running; }
        }
    }
}
