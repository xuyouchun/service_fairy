using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm;

namespace ServiceFairy.WinForm
{
    public partial class EditTitleAndDescDialog : XDialog
    {
        public EditTitleAndDescDialog()
        {
            InitializeComponent();
        }

        public static bool Show(IWin32Window window, ref string title, ref string desc)
        {
            EditTitleAndDescDialog dlg = new EditTitleAndDescDialog();
            dlg._ctlClientTitle.Text = title;
            dlg._ctlClientDesc.Text = desc;

            if (dlg.ShowDialog(window) == DialogResult.OK)
            {
                title = dlg._ctlClientTitle.Text;
                desc = dlg._ctlClientDesc.Text;

                return true;
            }

            return false;
        }
    }
}
