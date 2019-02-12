using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq.Expressions;
using System.Runtime.Remoting;
using CommonLibrary;
using System.Threading;
using System.IO;

namespace AppDomainTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string dllPath = @"D:\Work\Dev\Test\AppDomainTest\ClassLibrary1\bin\Debug\ClassLibrary1.dll";

            //AppDomainSetup appDomainSetup = new AppDomainSetup() { ApplicationBase = Path.GetDirectoryName(dllPath) };
            AppDomain domain = AppDomain.CreateDomain("aaa");
            Loader loader = domain.CreateInstanceFromAndUnwrap(typeof(Program).Assembly.FullName, typeof(Loader).FullName) as Loader;

            loader.Load(dllPath);

            AppDomain.Unload(domain);
            return;
        }

        class Loader : MarshalByRefObject
        {
            public void Load(string assemblyFile)
            {
                Assembly assembly = Assembly.Load(assemblyFile);
                Type type = assembly.GetType("ClassLibrary1.MyClass");
                object obj = Activator.CreateInstance(type);
            }
        }

        static void domain_DomainUnload(object sender, EventArgs e)
        {
            Console.WriteLine("Unloaded");
        }

        static void aaaa_Received(object sender, MyEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        private static void _CreateMyMethod(TypeBuilder tb)
        {
            ParameterExpression param_a = Expression.Parameter(typeof(int), "a"),
                param_b = Expression.Parameter(typeof(int), "b");
            ParameterExpression variable_r = Expression.Variable(typeof(int), "r");

            Expression body = Expression.Block(typeof(int), new[] { variable_r }, new Expression[]{
                Expression.Assign(variable_r, Expression.Add(param_a, param_b)),
            });

            Expression<Func<int, int, int>> exp = Expression.Lambda<Func<int, int, int>>(body, new[] { param_a, param_b });
            exp.CompileToMethod(tb.DefineMethod("MyMethod", MethodAttributes.Public | MethodAttributes.Static));
        }

        private static void _CreateMain(TypeBuilder tb)
        {
            Expression body = Expression.Block(new Expression[] {
                Expression.Call(typeof(Console).GetMethod("WriteLine", new[]{ typeof(string) }), Expression.Constant("Hello World!"))
            });

            Expression<Action> exp = Expression.Lambda<Action>(body);
            exp.CompileToMethod(tb.DefineMethod("Main", MethodAttributes.Public | MethodAttributes.Static));
        }
    }
}
