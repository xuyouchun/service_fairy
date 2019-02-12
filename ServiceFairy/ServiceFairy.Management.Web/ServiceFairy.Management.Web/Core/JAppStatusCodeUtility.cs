using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using System.Text;
using System.Collections.Concurrent;
using Common.Utility;
using System.Diagnostics.Contracts;
using Common.Contracts;
using Common.Package;
using Common.Contracts.Service;

namespace ServiceFairy.Management.Web.Core
{
    public static class JAppStatusCodeUtility
    {
        public static string ToDefName(string name, string serviceName)
        {
            if (string.IsNullOrEmpty(name))
                return "";

            int k = serviceName.IndexOf('.');
            serviceName = k < 0 ? serviceName : serviceName.Substring(k + 1);

            return "SC_" + _ToUpperName(serviceName + "_" + name);
        }

        private static string _ToUpperName(string name)
        {
            StringBuilder sb = new StringBuilder();
            bool f = false;
            foreach (char c in name)
            {
                if (char.IsUpper(c))
                {
                    if (sb.Length > 0 && !f)
                        sb.Append("_");

                    sb.Append(c);
                    f = true;
                }
                else
                {
                    char c0 = (c == '.') ? '_' : char.ToUpper(c);
                    sb.Append(c0);
                    f = (c0 == '_');
                }
            }

            return sb.ToString();
        }
    }
}