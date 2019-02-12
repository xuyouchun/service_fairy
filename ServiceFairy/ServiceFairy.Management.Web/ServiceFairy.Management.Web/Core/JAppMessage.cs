using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceFairy.Management.Web.Core
{
    public class JAppMessage : IComparable<JAppMessage>
    {
        public JAppMessage(string name, string version, string title, string desc, string typeName)
        {
            Name = name;
            Version = version;
            Title = title;
            Desc = desc;
            TypeName = typeName;
        }

        public string Name { get; private set; }

        public string Version { get; private set; }

        public string Title { get; private set; }

        public string Desc { get; private set; }

        public string TypeName { get; private set; }

        public int CompareTo(JAppMessage other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}