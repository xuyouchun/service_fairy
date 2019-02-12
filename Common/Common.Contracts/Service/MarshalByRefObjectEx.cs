using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts.Service
{
    public class MarshalByRefObjectEx : MarshalByRefObject
    {
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
