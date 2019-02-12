using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ServiceFairy.WinForm.AppCommandInvokeControls
{
    class EmptyInvokeControlContext : InvokeControlContextBase
    {
        public override System.Windows.Forms.Control GetControl()
        {
            return new Panel();
        }

        public static readonly EmptyInvokeControlContext Instance = new EmptyInvokeControlContext();
    }
}
