using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Script.VbScript.Values.Objects
{
    [Class]
    class RegExp
    {
        [Method]
        public bool IsMatch(string input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
        }
        
        [Method]
        public string Replace(string input, string pattern, string replacement)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return Regex.Replace(input, pattern, replacement);
        }

        [Method]
        public ArrayObject Match(string input, string pattern)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            Match m = Regex.Match(input, pattern);
            return !m.Success ? null : ArrayObject.Create(_SelectValues(m.Groups));
        }

        private static string[] _SelectValues(GroupCollection gs)
        {
            string[] ss = new string[gs.Count];
            for (int k = 0; k < gs.Count; k++)
            {
                ss[k] = gs[k].Value;
            }

            return ss;
        }
    }
}
