using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Contracts;

namespace ServiceFairy.WinForm.AppCommandInvokeControls
{
    class ArgumentSampleInvokeControlContext : InvokeControlContextBase
    {
        public ArgumentSampleInvokeControlContext(InvokeControlContext ctx, string direct, DataFormat format)
        {
            _ctx = ctx;
            _direct = direct;
            _format = format;

            _richTextBox = new Lazy<Control>(_CreateControl);
        }

        private readonly InvokeControlContext _ctx;
        private readonly string _direct;
        private readonly DataFormat _format;

        private string _GetSample()
        {
            return _ctx.GetSample(_direct, _format);
        }

        private readonly Lazy<Control> _richTextBox;

        private Control _CreateControl()
        {
            ArgumentSampleInvokeControl control = new ArgumentSampleInvokeControl();
            control.Text = _GetSample();

            return control;
        }

        public override Control GetControl()
        {
            return _richTextBox.Value;
        }
    }
}
