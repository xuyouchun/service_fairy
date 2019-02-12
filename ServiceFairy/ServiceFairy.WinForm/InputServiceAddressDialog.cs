using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm;
using Common.Contracts;
using Common.Contracts.Service;

namespace ServiceFairy.WinForm
{
    /// <summary>
    /// 输入导航服务器的地址
    /// </summary>
    public partial class InputServiceAddressDialog : XDialog
    {
        public InputServiceAddressDialog()
        {
            InitializeComponent();

            AddValidator(_ipAddress, (value) => ServiceAddress.IsCorrect((string)value), "地址格式错误");
        }

        /// <summary>
        /// 导航服务器地址
        /// </summary>
        public ServiceAddress Navigation
        {
            get { return ServiceAddress.Parse(_ipAddress.Text); }
            set { _ipAddress.Text = value.ToString(); }
        }

        /// <summary>
        /// 显示输入地址对话框
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="defaultAddress"></param>
        /// <param name="promptText"></param>
        /// <returns></returns>
        public static ServiceAddress Show(IWin32Window owner, ServiceAddress defaultAddress, string promptText = "请输入地址")
        {
            InputServiceAddressDialog dlg = new InputServiceAddressDialog();
            dlg._ipAddress.Text = (defaultAddress == null) ? string.Empty : defaultAddress.ToString();
            dlg._lbCaption.Text = promptText;

            if (dlg.ShowDialog(owner) == DialogResult.OK)
                return dlg.Navigation;

            return null;
        }
    }
}
