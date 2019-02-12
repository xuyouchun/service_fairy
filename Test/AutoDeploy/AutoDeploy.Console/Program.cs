using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace AutoDeploy.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string assemblyFile in ExecutableAssemblyManager.SearchExecutableAssemblyFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assemblies")))
            {
                try
                {
                    IExecutableAssembly executableAssembly = _LoadExecutableAssembly(assemblyFile);
                    if (executableAssembly != null)
                    {
                        Executor executor = new Executor(executableAssembly);
                        executor.Start();
                    }
                }
                catch (Exception)
                {
                    
                }
            }
        }

        private static IExecutableAssembly _LoadExecutableAssembly(string assemblyFile)
        {
            Assembly assembly = Assembly.LoadFile(assemblyFile);
            if (!assembly.IsDefined(typeof(ExecutableAssemblyAttribute), true))
                return null;

            ExecutableAssemblyAttribute attr = (ExecutableAssemblyAttribute)assembly.GetCustomAttributes(typeof(ExecutableAssemblyAttribute), true)[0];
            return Activator.CreateInstance(attr.ExecutableType) as IExecutableAssembly;
        }
    }
}
