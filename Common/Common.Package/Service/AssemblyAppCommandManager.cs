using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace Common.Package.Service
{
    [AppComponent("接口加载器", "利用反射机制从所属程序集中加载一组接口", category: AppComponentCategory.System, name: "Sys_AssemblyAppCommandManager")]
    public class AssemblyAppCommandManager : AppComponent
    {
        public AssemblyAppCommandManager(AssemblyAppServiceBase service)
            : base(service)
        {
            _service = service;
            _commandCollection = AppCommandCollection.LoadFromAssemblies(service.GetAssemblies());
        }

        private readonly AssemblyAppServiceBase _service;
        private readonly AppCommandCollection _commandCollection;

        protected override void OnStart()
        {
            base.OnStart();

            _service.AddCommands(_commandCollection.GetAppCommands());
        }

        protected override void OnStop()
        {
            base.OnStop();

            _service.RemoveCommands(_commandCollection.GetAppCommands());
        }
    }
}
