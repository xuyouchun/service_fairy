using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Serializer;
using Common.Contracts;

namespace TestJsonEncode
{
    class Program
    {
        static void Main(string[] args)
        {
            //IObjectSerializer ser = SerializerFactory.CreateSerializer(DataFormat.Json);

            Type t = typeof(Class1);

            object obj = Activator.CreateInstance(t, true);

            

            return;

        }




    }
}
