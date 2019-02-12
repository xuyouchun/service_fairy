using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoDeploy
{
    public interface IExecutableAssembly
    {
        void Start();
        
        void Stop();

        event EventHandler Exit;
    }
}
