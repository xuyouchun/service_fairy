using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyHost
{
    public interface IOutput
    {
        void Write(string s);
    }

    public class EmptyOutput : IOutput
    {
        public void Write(string s)
        {
            
        }
    }
}
