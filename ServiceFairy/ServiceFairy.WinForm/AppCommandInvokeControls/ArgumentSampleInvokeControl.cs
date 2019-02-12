using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ServiceFairy.WinForm.AppCommandInvokeControls
{
    public partial class ArgumentSampleInvokeControl : UserControl
    {
        public ArgumentSampleInvokeControl()
        {
            InitializeComponent();
        }

        public override string Text
        {
            get
            {
                return _ctlText.Text;
            }
            set
            {
                _ctlText.Text = value;
            }
        }

        private void _ctlCopy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string s = _ctlText.Text;
            if (!string.IsNullOrEmpty(s))
                Clipboard.SetText(s);
        }
    }
}
