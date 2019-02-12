using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoDeploy;
using System.Threading;

namespace Assembly1
{
    public class Executor : IExecutableAssembly
    {
        public void Start()
        {
            Thread thread = new Thread(_RunningFunc);
            _running = true;
            thread.Start();
        }

        private volatile bool _running = false;

        private void _RunningFunc()
        {
            int index = 1;
            while (_running)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Hello! - " + index++);
            }

            EventHandler eh = Exit;
            if (eh != null)
                eh(this, EventArgs.Empty);
        }

        public void Stop()
        {
            _running = false;
        }

        public event EventHandler Exit;
    }
}
