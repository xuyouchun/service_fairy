using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Package;

namespace ServiceFairy.Management.WinForm.Commands
{
    [Command("Exit")]
    class ExitCommand : CommandBase
    {
        public override void Execute(object context)
        {
            SmContext ctx = (SmContext)context;
            ctx.MainForm.Close();
        }
    }
}
