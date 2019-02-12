using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm.Docking;
using Common.WinForm;
using ServiceFairy.Entities.User;

namespace ServiceFairy.Client.WinForm
{
    partial class UserMainWindow : DockContentEx
    {
        public UserMainWindow(ClientContext clientCtx, UserContext userCtx)
        {
            _clientCtx = clientCtx;
            _userCtx = userCtx;

            InitializeComponent();
        }

        private readonly ClientContext _clientCtx;
        private readonly UserContext _userCtx;

        private void _statusTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    _ChangeMyStatus(_statusTextBox.Text);
                }
                catch (Exception ex)
                {
                    this.ShowError(ex);
                }
            }
        }

        private void _ChangeMyStatus(string status)
        {
            _userCtx.UserCon.Invoker.User.SetStatus(status);
        }

        private void _statusTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void _statusChangedButton_Click(object sender, EventArgs e)
        {
            _ChangeMyStatus(_statusTextBox.Text);
        }

        private void _onlineCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                _userCtx.UserCon.Connected = _onlineCheckBox.Checked;
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        // 我的信息变化
        private void _changeInfoButton_Click(object sender, EventArgs e)
        {
            try
            {
                _userCtx.UserCon.Invoker.User.ModifyMyInfo(new Contact { FirstName = _firstNameTextBox.Text, LastName = _lastNameTextBox.Text });
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        public void ShowLog(string log, DateTime time = default(DateTime))
        {
            if (time == default(DateTime))
                time = DateTime.Now;

            _outputTextBox.AppendText(string.Format("[{0}] {1}\r\n", time, log));
        }

        private void _clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _outputTextBox.Clear();
        }
    }
}
