using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(string.Join(", ", args));
            File.WriteAllLines(@"d:\temp\install.txt", args);

            Console.ReadLine();
        }
    }
}
