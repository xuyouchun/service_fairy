using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Configuration;
using System.Threading;

namespace AssemblyHost
{
    /// <summary>
    /// 用于启动程序集的App
    /// </summary>
    public class AssemblyHostApplication
    {
        public AssemblyHostApplication(IOutput output, WaitHandle waitForExit, Action<string, string[]> callback, string[] args)
        {
            _output = output ?? new EmptyOutput();
            _args = args;
            _waitForExit = waitForExit;
            _callback = callback;
        }

        private readonly IOutput _output;
        private readonly string[] _args;
        private bool _useConfigSetting;
        private readonly WaitHandle _waitForExit;
        private readonly Action<string, string[]> _callback;

        public int Run()
        {
            string[] args = _GetArgs();
            string path = _GetAssemblyPath();
            if (!string.IsNullOrEmpty(path))
            {
                AssemblyLoader loader = new AssemblyLoader();
                loader.RunAssembly(path, args, _callback, _waitForExit);
            }
            else
            {
                InstanceConfigurationElement[] instances = GetInstances();

                foreach (InstanceConfigurationElement instance in instances)
                {
                    try
                    {
                        path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, instance.Path);
                        AppDomainSetup setup = new AppDomainSetup();
                        setup.ApplicationName = instance.Name;
                        setup.ConfigurationFile = (instance.AppConfig == null) ? AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE") as string :
                            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, instance.AppConfig);
                        setup.ApplicationBase = Path.GetDirectoryName(path);

                        AppDomain domain = AppDomain.CreateDomain(instance.Name, AppDomain.CurrentDomain.Evidence, setup);
                        IAssemblyLoader loader = domain.CreateInstanceAndUnwrap(this.GetType().Assembly.FullName, typeof(AssemblyLoader).FullName) as IAssemblyLoader;
                        Thread thread = new Thread(_ThreadFunc);
                        thread.IsBackground = true;
                        thread.Start(new object[] { loader, path, args, instance });
                    }
                    catch (Exception ex)
                    {
                        _output.WriteError(ex);
                    }
                }

                _waitForExit.WaitOne();
            }

            return 0;
        }

        private void _ThreadFunc(object state)
        {
            try
            {
                object[] ts = (object[])state;
                IAssemblyLoader loader = ts[0] as IAssemblyLoader;
                string path = ts[1] as string;
                string[] args = ts[2] as string[];
                InstanceConfigurationElement cfg = ts[3] as InstanceConfigurationElement;

                _output.WriteLine("Start " + cfg.Name + " ...");
                loader.RunAssembly(path, args, _callback, _waitForExit);
            }
            catch (Exception ex)
            {
                _output.WriteError(ex);
            }
        }

        private InstanceConfigurationElement[] GetInstances()
        {
            InstanceConfigurationSection section = ConfigurationManager.GetSection("instances") as InstanceConfigurationSection;
            if (section == null)
                return new InstanceConfigurationElement[0];

            List<InstanceConfigurationElement> instances = new List<InstanceConfigurationElement>();
            foreach (InstanceConfigurationElement instance in section.Instances)
            {
                if (instance.AppConfig == null)
                    instance.AppConfig = section.Instances.DefaultAppConfig;

                if (instance.Path == null)
                    instance.Path = section.Instances.DefaultPath;

                instances.Add(instance);
            }

            return instances.ToArray();
        }

        private string _GetAssemblyPath()
        {
            string path = ConfigurationManager.AppSettings.Get("assemblyPath");
            if (path != null)
            {
                _useConfigSetting = true;
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }

            if (_args.Length > 0)
            {
                _useConfigSetting = false;
                return _args[0];
            }

            return null;
        }

        private string[] _GetArgs()
        {
            if (_args.Length == 0)
                return new string[0];

            string[] args = new string[_args.Length - (_useConfigSetting ? 0 : 1)];
            Array.Copy(_args, 1, args, 0, args.Length);
            return args;
        }
    }
}
