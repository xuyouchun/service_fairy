using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values.Objects
{
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class ClassAttribute : Attribute
    {
        public ClassAttribute(string name)
        {
            Name = name;
        }

        public ClassAttribute()
        {

        }

        public string Name { get; private set; }
    }
}
