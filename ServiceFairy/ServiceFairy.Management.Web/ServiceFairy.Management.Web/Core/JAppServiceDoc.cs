using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceFairy.Management.Web.Core
{
    public class JAppServiceDoc : IComparable<JAppServiceDoc>
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string Title { get; set; }

        public int CompareTo(JAppServiceDoc other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}