using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;

namespace AssemblyHost
{
    interface IAssemblyLoader
    {
        int RunAssembly(string path, string[] args, Action<string, string[]> callback, WaitHandle waitHandle);
    }

    class AssemblyLoader : MarshalByRefObject, IAssemblyLoader
    {
        public int RunAssembly(string path, string[] args, Action<string, string[]> callback, WaitHandle waitHandle)
        {
            dynamic obj = _LoadExecutableAssembly(path);
            object result = obj.Execute(args, callback, waitHandle);
            return (result is int) ? (int)result : 0;
        }

        private object _LoadExecutableAssembly(string path)
        {
            Type type = _GetExecutableType(path);
            if (type == null)
                throw new NotSupportedException("未指定可执行的类型");

            if (type.GetInterfaces().FirstOrDefault(t => t.FullName == "Common.Contracts.Service.IExecutableAssembly") == null)
                throw new NotSupportedException("类型" + type + "未实现接口IExecutableAssembly");

            return Activator.CreateInstance(type);
        }

        private Type _GetExecutableType(string path)
        {
            Assembly assembly = Assembly.UnsafeLoadFrom(path);
            foreach (Attribute attr in assembly.GetCustomAttributes(true))
            {
                if (attr.GetType().FullName == "Common.Contracts.Service.AssemblyEntryPointAttribute")
                {
                    return attr.GetType().GetProperty("EntryPointType").GetValue(attr, null) as Type;
                }
            }

            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(true).FirstOrDefault(attr => attr.GetType().FullName == "Common.Contracts.Service.AppEntryPointAttribute") != null)
                    return type;
            }

            return null;
        }
    }
}
