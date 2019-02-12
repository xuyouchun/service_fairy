using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    sealed class UnitaryCalculateAttribute : Attribute
    {
        public UnitaryCalculateAttribute(Type type)
        {
            Type = type;
        }

        public Type Type { get; private set; }
    }
}
