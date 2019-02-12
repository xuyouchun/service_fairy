using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Package;

namespace ServiceFairy.Management.WinForm.Commands
{
    /// <summary>
    /// 断开连接
    /// </summary>
    [Command("Disconnect")]
    class DisconnectCommand : CommandBase
    {
        public override void Execute(object context)
        {

        }
    }
}
