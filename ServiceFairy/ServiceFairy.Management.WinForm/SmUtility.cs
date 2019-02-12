using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Windows.Forms;
using Common.WinForm;
using ServiceFairy.WinForm;

namespace ServiceFairy.Management.WinForm
{
    static class SmUtility
    {
        public static LoginResult Login(IWin32Window owner)
        {
            LoginDialog dlg = new LoginDialog();
            dlg.SetDefaultNavigationAddresses(Settings.GetNavigations());
            if (dlg.ShowDialog(owner) != DialogResult.OK)
                return null;

            return dlg.GetLoginResult();
        }
    }
}
