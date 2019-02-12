using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm;
using WeifenLuo.WinFormsUI.Docking;

namespace ServiceFairy.Client.WinForm
{
    partial class MainForm : XForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 可停靠窗体的容器
        /// </summary>
        public DockPanel DockPanel
        {
            get { return dockPanel1; }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Text += " - " + Settings.NavigationUrl;
        }
    }
}
