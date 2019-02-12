using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm;
using Common.WinForm.Docking;
using Common.WinForm.Docking.DockingWindows;
using Common.Package;
using Common.Contracts;
using WeifenLuo.WinFormsUI.Docking;

namespace StreamTableViewer
{
    public partial class MainForm : XForm
    {
        public MainForm()
        {
            InitializeComponent();

            _dockingWindowManager = new DockingWindowManager(_dockPanel, Settings.CreateDefaultDockingWindowLayout());
            _outputDockingWindow = new OutputDockingWindow();
            _propertyDockingWindow = new PropertyDockingWindow();
            _viewerContext = new ViewerContext(this, _CreateServiceProvider());
            _navigationDockingWindow = new NavigationDockingWindow(_viewerContext);
        }

        private IServiceProvider _CreateServiceProvider()
        {
            ServiceProvider sp = new ServiceProvider();
            sp.AddService(typeof(IOutput), _outputDockingWindow);
            sp.AddService(typeof(IPropertyViewer), _propertyDockingWindow);

            return sp;
        }

        private readonly ViewerContext _viewerContext;
        private readonly DockingWindowManager _dockingWindowManager;
        private readonly NavigationDockingWindow _navigationDockingWindow;
        private readonly OutputDockingWindow _outputDockingWindow;
        private readonly PropertyDockingWindow _propertyDockingWindow;

        public DockPanel DockPanel
        {
            get { return _dockPanel; }
        }

        private void _tsOpen_Click(object sender, EventArgs e)
        {
            if (_dlgOpenFile.ShowDialog(this) == DialogResult.OK)
            {
                NavigationDockingWindow navigation = _dockingWindowManager.GetDockContent<NavigationDockingWindow>();

                navigation.ShowFiles(_dlgOpenFile.FileNames);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _dockingWindowManager.ShowDefaultLayout(new DockContentEx[] {
                _navigationDockingWindow, _outputDockingWindow, _propertyDockingWindow
            });
        }

        private void _tsClose_Click(object sender, EventArgs e)
        {
            _navigationDockingWindow.CloseCurrent();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];

            if (files != null)
            {
                NavigationDockingWindow navigation = _dockingWindowManager.GetDockContent<NavigationDockingWindow>();
                navigation.ShowFiles(files);
            }
        }
    }
}
