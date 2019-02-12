using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.Addins.MyTest;

namespace ServiceFairy.Service.Test.Addins
{
    [AppServiceAddin("MyTest", "1.0", "测试插件")]
    class MyTestAddin : AppServiceAddinBaseEx
    {
        public MyTestAddin(Service service)
            : base(service)
        {

        }

        [AppCommand("Hello1")]
        class Hello1AppCommand : AppCommandBase<Hello1_Request, Hello1_Reply>
        {
            protected override Hello1_Reply OnExecute(AppCommandExecuteContext context, Hello1_Request req, ref ServiceResult sr)
            {
                return new Hello1_Reply() { ReturnValue = "Hello - " + req.Arg };
            }
        }
    }
}
