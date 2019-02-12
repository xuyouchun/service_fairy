using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    public class Class1
    {
        public Class1(string msg)
        {
            _msg = msg;
        }

        private readonly string _msg;

        public string GetString()
        {
            return _msg;
        }
    }
}
