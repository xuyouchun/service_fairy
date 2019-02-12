using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;

namespace BhFairy.Service.NameCardSharing.Components
{
    class MyComponent : AppComponent
    {
        public MyComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;

        public string GetStr()
        {
            return "Hello World!";
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }
    }
}
