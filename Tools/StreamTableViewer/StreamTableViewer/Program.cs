using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Common.Data;
using System.Data;
using Common.Package.Storage;
using System.IO;

namespace StreamTableViewer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
