using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace ServiceFairy.Service.UI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ServiceUIAttribute : Attribute
    {
        public ServiceUIAttribute(string name, string version = "1.0")
        {
            Contract.Requires(name != null);
            ServiceDesc = new ServiceDesc(name, version);
        }

        public ServiceDesc ServiceDesc { get; private set; }

        public static ServiceDesc GetFromType(Type type)
        {
            Contract.Requires(type != null);

            ServiceUIAttribute attr = type.GetAttribute<ServiceUIAttribute>();
            return attr == null ? null : attr.ServiceDesc;
        }
    }
}
