using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace ServiceFairy.Cluster
{
    class ParameterReader
    {
        public ParameterReader(string[] args)
        {
            foreach (string arg in args ?? Array<string>.Empty)
            {
                _Parse(arg);
            }
        }

        private readonly Dictionary<string, string> _dict = new Dictionary<string, string>();

        private void _Parse(string arg)
        {
            int k = arg.IndexOf(':');
            if (k > 0)
            {
                _dict[arg.Substring(0, k)] = arg.Substring(k + 1);
            }
        }

        public string GetArg(string name)
        {
            string s;
            _dict.TryGetValue(name, out s);
            return s;
        }
    }
}
