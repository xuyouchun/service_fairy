using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.IO;
using System.Text;

namespace ConsoleApplication1
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        public Installer1()
        {
            InitializeComponent();


            this.BeforeInstall += new InstallEventHandler(Installer1_BeforeInstall);
            this.AfterInstall += new InstallEventHandler(Installer1_AfterInstall);
        }

        public override void Commit(IDictionary savedState)
        {
            string path = Context.Parameters["path"];
            _AppendText(path);

            base.Commit(savedState);
        }

        void Installer1_AfterInstall(object sender, InstallEventArgs e)
        {

        }

        void Installer1_BeforeInstall(object sender, InstallEventArgs e)
        {
            
        }

        private void _AppendText(string text)
        {
            File.AppendAllText(@"d:\temp\install.txt", text);
        }
    }
}
