using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements
{
    [global::System.AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class StatementTypeAttribute : Attribute
    {
        public StatementTypeAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
