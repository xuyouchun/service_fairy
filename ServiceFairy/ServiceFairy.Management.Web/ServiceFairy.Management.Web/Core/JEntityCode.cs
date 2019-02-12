using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceFairy.Management.Web.Core
{
    public class JEntityCode
    {
        public JEntityCodeRange[] Codes { get; set; }
    }

    public class JEntityCodeRange
    {
        public string Name { get; set; }

        public string Code { get; set; }
    }
}