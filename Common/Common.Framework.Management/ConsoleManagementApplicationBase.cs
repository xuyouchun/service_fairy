using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package;
using Common.Contracts;
using Common.Contracts.Service;

namespace Common.Framework.Management
{
    /// <summary>
    /// 基于控制台的应用程序
    /// </summary>
    public abstract class ConsoleManagementApplicationBase : ManagementApplicationBase
    {
        protected override IOutput CreateOutput()
        {
            return new ConsoleInputOutput();
        }

        protected override void OnRun(ManagementContext context)
        {
            context.Output.WriteLine("Running ...");
        }
    }
}
