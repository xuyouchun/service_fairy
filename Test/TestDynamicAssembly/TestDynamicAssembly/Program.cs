using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq.Expressions;

namespace TestDynamicAssembly
{
    class Program
    {
        public static void MyMethod()
        {
            Console.WriteLine("Write");
        }

        static void Main(string[] args)
        {
            Type interfaceType = typeof(MyCoreClass).Assembly.GetType("Core.IMyInterface");

            AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(
                new AssemblyName("MyInterface_AppDomain"), AssemblyBuilderAccess.Run);
            ModuleBuilder mb = ab.DefineDynamicModule("TempModel");
            TypeBuilder tb = mb.DefineType("MyClass", TypeAttributes.AutoClass, typeof(object), new Type[] { interfaceType });
            MethodBuilder methodBuilder = tb.DefineMethod("MyMethod", MethodAttributes.CheckAccessOnOverride | MethodAttributes.Virtual, typeof(void), new Type[0]);
            ILGenerator il = methodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ret);

            tb.DefineMethodOverride(methodBuilder, interfaceType.GetMethod("MyMethod"));

            /*
            MethodInfo methodInfo = typeof(Program).GetMethod("MyMethod", BindingFlags.Static | BindingFlags.Public);
            Expression body = Expression.Block(
                Expression.Call(methodInfo)
            );
            Expression<Action> a = Expression.Lambda<Action>(body);
            a.CompileToMethod(methodBuilder);*/

            Type type = tb.CreateType();

            object obj = Activator.CreateInstance(type);
            //MemberInfo[] mInfos = type.GetMembers();
            MethodInfo mInfo = type.GetMethod("MyMethod", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            mInfo.Invoke(obj, null);

            MyCoreClass coreClass = new MyCoreClass();

            FieldInfo fInfo = typeof(MyCoreClass).GetField("_interface", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            fInfo.SetValue(coreClass, obj);


            coreClass.Call();
        }




    }
}
