using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoDeploy.Console
{
    class Executor
    {
        public Executor(IExecutableAssembly executableAssembly)
        {
            _executableAssembly = executableAssembly;
        }

        private readonly IExecutableAssembly _executableAssembly;

        public void Start()
        {
            _executableAssembly.Start();
        }
    }
}
