using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    public class Class1 : MarshalByRefObject, IClass1
    {
        public Output Request(Input input)
        {
            return new Output() { OutputString = input.InputString };
        }
    }

    public interface IClass1
    {
        
    }

    [Serializable]
    public class Input
    {
        public string InputString = "InputString";
        public string InputString2 = "InputString2";
        public string InputString3 = "InputString3";
        public string InputString4 = "InputString4";
        public string InputString5 = "InputString5";
    }

    [Serializable]
    public class Output
    {
        public string OutputString;
    }
}
