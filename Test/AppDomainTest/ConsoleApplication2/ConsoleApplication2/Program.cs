using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;

namespace ConsoleApplication2
{
    enum Enum1
    {
        A, B, C
    }

    class Class1
    {
        public Dictionary<string, string> Dict { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Class1 obj = new Class1 {
                Dict = new Dictionary<string, string> {
                    { "AAA", "aaa" }, { "BBB", "bbb" },
                },
            };

            string json = JsonUtility.SerializeToString(obj);

            //Class1 obj2 = JsonUtility.Deserialize<Class1>(json);

            return;
        }
    }
}
