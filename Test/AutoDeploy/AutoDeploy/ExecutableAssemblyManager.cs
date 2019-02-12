using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace AutoDeploy
{
    public static class ExecutableAssemblyManager
    {
        public static IEnumerable<string> SearchExecutableAssemblyFiles(string path)
        {
            if (!Directory.Exists(path))
                yield break;

            foreach (string subPath in Directory.GetDirectories(path).OrderByDescending(v => v))
            {
                string assemblyFile = Path.Combine(subPath, "Main.dll");
                if (File.Exists(assemblyFile))
                    yield return assemblyFile;
            }
        }
    }
}
