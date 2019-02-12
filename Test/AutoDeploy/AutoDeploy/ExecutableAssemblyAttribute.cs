using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoDeploy
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = true)]
    public class ExecutableAssemblyAttribute : Attribute
    {
        public ExecutableAssemblyAttribute(Type executableType)
        {
            ExecutableType = executableType;
        }

        public Type ExecutableType { get; private set; }
    }
}
