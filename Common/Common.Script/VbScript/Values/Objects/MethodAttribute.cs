using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values.Objects
{
    [global::System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    class MethodAttribute : Attribute
    {
        public MethodAttribute(string name)
        {
            Name = name;
        }

        public MethodAttribute()
        {

        }

        public string Name { get; private set; }
    }
}
