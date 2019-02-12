using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StreamTableViewer
{
    public class ViewerContext
    {
        public ViewerContext(MainForm mainWindow, IServiceProvider serviceProvider)
        {
            MainWindow = mainWindow;
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// 主窗口
        /// </summary>
        public MainForm MainWindow { get; private set; }

        /// <summary>
        /// 服务集合
        /// </summary>
        public IServiceProvider ServiceProvider { get; private set; }
    }
}
