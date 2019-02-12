using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts.Service
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    class AssemblyEntityPointAttribute : Attribute
    {
        public AssemblyEntityPointAttribute(Type entityPointType)
        {
            EntityPointType = entityPointType;
        }

        public Type EntityPointType { get; private set; }
    }
}
