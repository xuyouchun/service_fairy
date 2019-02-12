using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm.Docking;
using ServiceFairy.Entities.Group;
using Common.WinForm;
using DevComponents.DotNetBar;
using ServiceFairy.Client.WinForm.Properties;
using ServiceFairy.Entities.User;
using Common.Utility;
using Common.Contracts.Service;
using System.Diagnostics;

namespace ServiceFairy.Client.WinForm
{
    partial class GroupMessageForm : DockContentEx
    {
        public GroupMessageForm(ClientContext clientCtx, UserContext UserCtx, FcGroupInfo fcGroupInfo)
        {
            _clientCtx = clientCtx;
            _userCtx = UserCtx;
            GroupInfo = fcGroupInfo;

            InitializeComponent();
            Text = fcGroupInfo.Name ?? fcGroupInfo.GroupId.ToString();
        }

        private ClientContext _clientCtx;
        private UserContext _userCtx;

        public FcGroupInfo GroupInfo { get; private set; }

        private void GroupMessageForm_Load(object sender, EventArgs e)
        {
            try
            {
                _RefreshMembers();
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(this, ex);
            }
        }

        private void _RefreshMembers()
        {
            FcGroupMemberInfo[] memberInfos = _userCtx.UserCon.Invoker.Group.GetGroupMembers(GroupInfo.GroupId);

            _memberItemPanel.BeginUpdate();
            _memberItemPanel.Items.Clear();
            foreach (FcGroupMemberInfo memberInfo in memberInfos)
            {
                ButtonItem buttonItem = new ButtonItem {
                    Text = memberInfo.UserName, Tag = memberInfo, Name = memberInfo.UserName ?? Guid.NewGuid().ToString(), AutoCheckOnClick = true,
                    ButtonStyle = eButtonStyle.ImageAndText, ItemAlignment = eItemAlignment.Far, Image = Resources.User.ToBitmap(),
                };

                _memberItemPanel.Items.Add(buttonItem);
            }
            _memberItemPanel.EndUpdate();
        }

        // 添加成员
        private void _addMemberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Contact[] contacts = SelectDialog.Show(this, _userCtx.UserCon.Invoker.User.GetContactList(), SelectionMode.MultiExtended);
                if (contacts.IsNullOrEmpty())
                    return;

                Users users = Users.FromUserNames(contacts.Select(c => c.UserName).ToArray());
                _userCtx.UserCon.Invoker.Group.AddGroupMembers(GroupInfo.GroupId, users.ToString());

                _RefreshMembers();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private FcGroupMemberInfo[] _GetSelectedMembers()
        {
            return _memberItemPanel.Items.Cast<ButtonItem>().Where(bi => bi.Checked).ToArray(bi => (FcGroupMemberInfo)bi.Tag);
        }

        // 删除成员
        private void _removeMemberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FcGroupMemberInfo[] members = _GetSelectedMembers();
                if (members.IsNullOrEmpty())
                {
                    this.ShowMessage("请首先选择要删除的群组成员");
                    return;
                }

                Users users = Users.FromUserNames(members.Select(c => c.UserName).ToArray());
                _userCtx.UserCon.Invoker.Group.RemoveGroupMembers(GroupInfo.GroupId, users.ToString());

                _RefreshMembers();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        // 刷新
        private void _refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                _RefreshMembers();
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void _sendMsgTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _TrySendMessage();
                e.SuppressKeyPress = true;
            }
        }

        private void _sendButton_Click(object sender, EventArgs e)
        {
            _TrySendMessage();
        }

        private void _TrySendMessage()
        {
            string text = _sendMsgTextBox.Text;
            if (text.Length > 0)
            {
                _TrySendMessage(text);
                AppendMessage(_userCtx.UserName, text, DateTime.UtcNow);
            }

            _sendMsgTextBox.Clear();
        }

        public void AppendMessage(string username, string content, DateTime time)
        {
            _msgTextBox.AppendText(string.Format("\r\n{0} [{1}]\r\n{2}",
                username, time.ToLocalTime().ToShortTimeString(), content));
        }

        private bool _TrySendMessage(string msg)
        {
            try
            {
                _SendMessage(msg);
                return true;
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
                return false;
            }
        }

        private void _SendMessage(string msg)
        {
            _userCtx.UserCon.Invoker.Group.SendMessage(GroupInfo.GroupId, msg ?? string.Empty);
        }

        private void _clearTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _msgTextBox.Clear();
        }
    }
}
