using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;

namespace ServiceFairy.Management.Web.Core
{
    public class JAppCommand : IComparable<JAppCommand>
    {
        public JAppCommand(string name, string version, string title, string desc,
            bool isSys, string requestTypeName, string replyTypeName, int securityLevel, string securityDesc,
            UsableType usable, string usableDesc)
        {
            Name = name;
            Version = version;
            Title = title;
            Desc = desc;
            IsSys = isSys;

            RequestTypeName = requestTypeName;
            ReplyTypeName = replyTypeName;

            SecurityLevel = securityLevel;
            SecurityDesc = securityDesc;

            Usable = usable;
            UsableDesc = usableDesc;
        }

        public string Name { get; private set; }

        public string Version { get; private set; }

        public string Title { get; private set; }

        public string Desc { get; private set; }

        public int SecurityLevel { get; private set; }

        public string SecurityDesc { get; private set; }

        public bool IsSys { get; private set; }

        public string RequestTypeName { get; private set; }

        public string ReplyTypeName { get; private set; }

        public UsableType Usable { get; private set; }

        public string UsableDesc { get; private set; }

        public int CompareTo(JAppCommand other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}