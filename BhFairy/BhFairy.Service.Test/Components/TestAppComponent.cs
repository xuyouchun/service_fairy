using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Package.Service;

namespace BhFairy.Service.Test.Components
{
    class TestAppComponent : AppComponent
    {
        public TestAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;

        public void WriteLog(string log)
        {
            // TODO
            /*
            _service.SystemInvoker.Storage.DbInsert("Log", null,
                new Dictionary<string, object> { { "Main.Time", DateTime.Now }, { "Main.Log", log } }
            );*/
        }
    }
}
