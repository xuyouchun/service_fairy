using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceFairy.Management.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConsoleManagementApplication app = new ConsoleManagementApplication();
            app.Run(null, null);
        }
    }
}
