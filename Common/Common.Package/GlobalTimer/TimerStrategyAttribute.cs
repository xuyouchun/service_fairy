using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Package.GlobalTimer
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    class TimerStrategyAttribute : Attribute
    {
        public TimerStrategyAttribute(string name, Type creatorType)
        {
            Name = name;
            CreatorType = creatorType;
        }

        public string Name { get; private set; }

        public Type CreatorType { get; private set; }
    }
}
