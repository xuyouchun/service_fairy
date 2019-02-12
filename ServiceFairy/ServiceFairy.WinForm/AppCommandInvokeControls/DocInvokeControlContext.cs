using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ServiceFairy.WinForm.AppCommandInvokeControls
{
    /// <summary>
    /// 文档
    /// </summary>
    class DocInvokeControlContext : InvokeControlContextBase
    {
        public DocInvokeControlContext(InvokeControlContext context, string direct)
        {
            _context = context;
            _direct = direct;
            _control = new Lazy<Control>(_CreateControl);
        }

        private readonly InvokeControlContext _context;
        private readonly string _direct;

        private Control _CreateControl()
        {
            DocInvokeControl control = new DocInvokeControl();
            control.SetDoc(_context.GetDocTree(_direct));
            return control;
        }

        private readonly Lazy<Control> _control;

        public override Control GetControl()
        {
            return _control.Value;
        }
    }
}
