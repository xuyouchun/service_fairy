using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Contracts;
using Common.Contracts.UIObject;

namespace Common.WinForm.Docking.DockingWindows
{
    public partial class PropertyDockingWindow : DockContentEx, IPropertyViewer, IPropertyWindow
    {
        public PropertyDockingWindow()
        {
            InitializeComponent();
        }

        public void ShowProperty(object obj)
        {
            _propertyGrid.SelectedObject = obj;
        }
    }
}
