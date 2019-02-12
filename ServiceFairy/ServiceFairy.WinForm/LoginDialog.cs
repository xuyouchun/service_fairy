using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm;
using Common.Contracts.Service;
using Common.Utility;
using Common.Contracts;
using Common.Communication.Wcf;

namespace ServiceFairy.WinForm
{
    public partial class LoginDialog : XDialog
    {
        public LoginDialog()
        {
            InitializeComponent();

            _tsCommunicationTypeGroup = new ToolStripMenuItemGroup(_tsCommunicationType);
            _ctlDataFormatGroup = new ToolStripMenuItemGroup(_tsDataFormat);
        }

        private void InputNavigationAddressDialog_Load(object sender, EventArgs e)
        {
            
        }

        private readonly ToolStripMenuItemGroup _tsCommunicationTypeGroup, _ctlDataFormatGroup;

        public void SetDefaultNavigationAddresses(CommunicationOption[] ops)
        {
            _ctlNavigation.Items.Clear();
            _ctlNavigation.Text = "";

            if (!ops.IsNullOrEmpty())
            {
                _ctlNavigation.Items.AddRange(ops);
                _ctlNavigation.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 获取登录的结果
        /// </summary>
        /// <returns></returns>
        public LoginResult GetLoginResult()
        {
            return new LoginResult(
                _ctlUserName.Text, _ctlPassword.Text, _navigation, null,
                (CommunicationType)Enum.Parse(typeof(CommunicationType), _tsCommunicationTypeGroup.Current.Tag.ToString().TrimStart("PROTOCAL:"), true),
                (DataFormat)Enum.Parse(typeof(DataFormat), _ctlDataFormatGroup.Current.Tag.ToString().TrimStart("DATA_FORMAT:"), true)
            );
        }

        private CommunicationOption _navigation;

        protected override void OnClosing(CancelEventArgs e)
        {
            if (DialogResult != System.Windows.Forms.DialogResult.OK)
                return;

            if (!CommunicationOption.TryParse(_ctlNavigation.Text, out _navigation))
            {
                this.ShowMessage("导航服务器格式错误");
                e.Cancel = true;
            }
        }

        private void _ctOption_Click(object sender, EventArgs e)
        {
            _msContextMenu.Show(_ctOption, new Point(0, _ctOption.Height), ToolStripDropDownDirection.BelowRight);
        }
    }

    /// <summary>
    /// 登录结果
    /// </summary>
    public class LoginResult
    {
        public LoginResult(string username, string password, CommunicationOption navigation,
            ServiceAddress proxyAddress, CommunicationType communicationType, DataFormat dataFormat)
        {
            UserName = username;
            Password = password;
            Navigation = navigation;
            ProxyAddress = proxyAddress;
            CommunicationType = communicationType;
            DataFormat = dataFormat;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// 导航地址
        /// </summary>
        public CommunicationOption Navigation { get; private set; }

        /// <summary>
        /// 代理地址
        /// </summary>
        public ServiceAddress ProxyAddress { get; private set; }

        /// <summary>
        /// 通信方式
        /// </summary>
        public CommunicationType CommunicationType { get; private set; }

        /// <summary>
        /// 数据格式
        /// </summary>
        public DataFormat DataFormat { get; private set; }
    }
}
