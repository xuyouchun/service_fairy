using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts.Service
{
    interface IExecutableAssembly
    {
        int Execute(string[] args);
    }
}
