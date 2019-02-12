using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TestRegexPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            string @pattern = @"426[^,\x12\x13\x14\x15\x16\x17\x18\x19]*?";
            string input = "\x000F4268\x0013";

            Match m = Regex.Match(input, @pattern);

            return;
        }
    }
}
