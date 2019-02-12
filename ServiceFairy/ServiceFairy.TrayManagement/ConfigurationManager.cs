using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Install;
using Common.WinForm;
using System.Windows.Forms;

namespace ServiceFairy.TrayManagement
{
    class ConfigurationManager
    {
        public ConfigurationManager(IWin32Window window, string configFile)
        {
            _configFile = configFile;
            _window = window;

            Configuration = RunningConfiguration.LoadFromFileOrDefault(configFile);

            if (Configuration.RunningModel == RunningModel.None)
                Configuration.RunningModel = RunningModel.Normal;
        }

        private readonly string _configFile;
        private readonly IWin32Window _window;

        /// <summary>
        /// 配置
        /// </summary>
        public RunningConfiguration Configuration { get; private set; }

        public void Save()
        {
            try
            {
                Configuration.SaveToFile(_configFile);
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(_window, ex);
            }
        }
    }
}
