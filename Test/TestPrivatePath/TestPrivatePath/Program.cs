using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassLibrary1;

namespace TestPrivatePath
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = Class1.Hello();
            Console.WriteLine(s);

            Console.ReadLine();
        }
    }
}
