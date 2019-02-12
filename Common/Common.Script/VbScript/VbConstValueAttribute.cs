using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    class VbConstValueAttribute : Attribute
    {
        public VbConstValueAttribute(string description)
        {
            Description = description;
        }

        public string Description { get; private set; }
    }
}
