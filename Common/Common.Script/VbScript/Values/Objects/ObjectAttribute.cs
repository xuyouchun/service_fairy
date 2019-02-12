using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Client.Script.Values.Objects
{
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class ObjectAttribute : Attribute
    {
        public ObjectAttribute(string name)
        {
            Name = name;
        }

        public ObjectAttribute()
        {

        }

        public string Name { get; private set; }
    }
}
