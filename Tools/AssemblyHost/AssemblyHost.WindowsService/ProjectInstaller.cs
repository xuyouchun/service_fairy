using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;

namespace AssemblyHost.WindowsService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            _serviceProcessInstaller.Account = Settings.ServiceAccount;
            _serviceInstaller.ServiceName = Settings.ServiceName;
            _serviceInstaller.DisplayName = Settings.DisplayName;
            _serviceInstaller.Description = Settings.Description;
            _serviceInstaller.DelayedAutoStart = Settings.DelayAutoStart;
            _serviceInstaller.StartType = Settings.ServiceStartMode;
            _serviceInstaller.ServicesDependedOn = Settings.ServicesDependedOn;
        }
    }
}
