using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Contracts.Service;
using Common.Utility;
using Common.WinForm;
using Common.WinForm.Docking;
using DevComponents.DotNetBar;
using ServiceFairy.Client.WinForm.Properties;
using ServiceFairy.Entities.Group;
using ServiceFairy.Entities.User;
using Common;

namespace ServiceFairy.Client.WinForm
{
    partial class UserMainFrame : DockContentEx
    {
        public UserMainFrame(ClientContext clientContext, UserContext userCtx)
        {
            Contract.Requires(userCtx != null && clientContext != null);

            _clientCtx = clientContext;
            UserCtx = userCtx;

            InitializeComponent();

            Text = userCtx.UserName ?? "";
            _mainWindow = new UserMainWindow(_clientCtx, UserCtx);
        }

        private readonly ClientContext _clientCtx;
        private readonly UserMainWindow _mainWindow;

        /// <summary>
        /// 用户状态
        /// </summary>
        public UserContext UserCtx { get; private set; }

        private void _ShowLog(string log, DateTime time = default(DateTime))
        {
            this.BeginInvoke(delegate {
                _mainWindow.ShowLog(log, time);
            });
        }

        private void UserMainWindow_Load(object sender, EventArgs e)
        {
            try
            {
                _mainWindow.Show(_dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);

                _LoadContacts();
                _RefreshStatus();
                _LoadFollowers();
                _LoadGroups();

                var invoker = UserCtx.UserCon.Invoker;
                invoker.User.StatusChanged += User_StatusChanged;
                invoker.User.NewUser += User_NewUser;
                invoker.User.InfoChanged += User_InfoChanged;
                invoker.Group.NewGroup += Group_NewGroup;
                invoker.Group.ExitGroup += Group_ExitGroup;
                invoker.Group.MemberAdded += Group_MemberAdded;
                invoker.Group.MemberRemoved += Group_MemberRemoved;
                invoker.Group.Message += Group_Message;
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(this, ex);
            }
        }

        // 用户信息变化
        private void User_InfoChanged(object sender, ServiceClientReceiveEventArgs<User_InfoChanged_Message> e)
        {
            int[] userIds;
            if (e.Entity == null || (userIds = e.Entity.UserIds).IsNullOrEmpty())
                return;

            _ShowLog(string.Format("{0}信息变化", _clientCtx.UserCtxMgr.GetUserDesc(userIds).JoinBy(", ")));

            Contact[] contacts = UserCtx.UserCon.Invoker.User.GetUserInfos(Users.FromUserIds(userIds));
            this.BeginInvoke(delegate {
                try
                {
                    foreach (Contact contact in contacts)
                    {
                        ButtonItem buttonItem = _FindContactButtonItem(contact.UserId);
                        buttonItem.Tag = contact;

                        UserDetailWindow detailWindow = _GetDetailWindow(contact, false);
                        if (detailWindow != null)
                            detailWindow.Update(contact);
                    }
                }
                catch (Exception ex)
                {
                    this.ShowError(ex);
                }
            });
        }

        // 新用户注册
        private void User_NewUser(object sender, ServiceClientReceiveEventArgs<User_NewUser_Message> e)
        {
            int[] userIds;
            if (e.Entity != null && !(userIds = e.Entity.UserIds).IsNullOrEmpty())
                _ShowLog(string.Format("用户{0}已注册", _clientCtx.UserCtxMgr.GetUserDesc(userIds).JoinBy(", ")));
        }

        // 新群组
        private void Group_NewGroup(object sender, ServiceClientReceiveEventArgs<Group_NewGroup_Message> e)
        {
            this.BeginInvoke(() => _NewGroup(e.Entity));
            _ShowLog(string.Format("您被加入了群组：{0}", e.Entity.GroupId));
        }

        private void _NewGroup(Group_NewGroup_Message msg)
        {
            try
            {
                FcGroupInfo gInfo = UserCtx.UserCon.Invoker.Group.GetGroup(msg.GroupId);
                if (gInfo != null)
                {
                    ButtonItem buttonItem = _GetGroupButtonItem(gInfo);
                    _OpenMessageWindow(buttonItem);
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        // 退出群组
        private void Group_ExitGroup(object sender, ServiceClientReceiveEventArgs<Group_ExitGroup_Message> e)
        {
            this.BeginInvoke(() => _ExitGroup(e.Entity));
        }

        private void _ExitGroup(Group_ExitGroup_Message msg)
        {
            try
            {
                int groupId = msg.GroupId;
                ButtonItem buttonItem = _GetGroupButtonItem(groupId, false);
                if (buttonItem != null)
                {
                    _CloseMessageWindow(buttonItem);
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        // 新成员加入群组
        private void Group_MemberAdded(object sender, ServiceClientReceiveEventArgs<Group_MemberAdded_Message> e)
        {
            
        }

        // 成员退出群组
        private void Group_MemberRemoved(object sender, ServiceClientReceiveEventArgs<Group_MemberRemoved_Message> e)
        {
            
        }

        // 群组消息
        private void Group_Message(object sender, ServiceClientReceiveEventArgs<Group_Message_Message> e)
        {
            this.BeginInvoke(() => _MessageReceived(e.Entity));
        }

        private void _MessageReceived(Group_Message_Message msg)
        {
            try
            {
                ButtonItem buttonItem = _GetGroupButtonItem(msg.GroupId);
                GroupMessageForm form = _OpenMessageWindow(buttonItem);
                form.AppendMessage(msg.Sender, msg.Content, msg.Time);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private void User_StatusChanged(object sender, ServiceClientReceiveEventArgs<User_StatusChanged_Message> e)
        {
            this.BeginInvoke(delegate {
                if (e.Entity != null && !e.Entity.Status.IsNullOrEmpty())
                {
                    _SetStatuses(e.Entity.Status);
                }
            });

            _ShowLog(e.Entity.Status.Select(s => string.Format("{0}状态变为：{1} {2}",
                _clientCtx.UserCtxMgr.GetUserDesc(s.UserId), s.Status, s.Online ? "在线" : "离线")).JoinBy("\r\n"));
        }

        private void _SetStatuses(ContactStatus[] uses)
        {
            foreach (ContactStatus us in uses ?? Array<ContactStatus>.Empty)
            {
                _SetStatus(us);
            }
        }

        private void _SetStatus(ContactStatus us)
        {
            ButtonItem bi = _FindContactButtonItem(us.UserId);

            if (bi != null)
            {
                _contactsItemPanel.BeginUpdate();
                string s = ((Contact)bi.Tag).UserName;

                if (!string.IsNullOrEmpty(us.Status))
                    s += " (" + us.Status + ")";

                bi.FontBold = us.Online;
                bi.ForeColor = us.Online ? Color.Blue : Color.Gray;

                bi.Text = s;
                _contactsItemPanel.EndUpdate();
            }
        }

        // 加载联系人
        private void _LoadContacts()
        {
            _contactsItemPanel.Items.Clear();

            var invoker = UserCtx.UserCon.Invoker;
            Contact[] contacts = invoker.User.GetContactList();
            HashSet<int> followings = (invoker.User.GetFollowings() ?? Array<int>.Empty).ToHashSet();
            ButtonItem[] buttonItems = _AddContacts(contacts, _contactsItemPanel);

            buttonItems.ForEach(bi => {
                Contact contact = (Contact)bi.Tag;
                bi.DoubleClick += new EventHandler(following_DoubleClick);
                bi.ForeColor = followings.Contains(contact.UserId) ? Color.Black : Color.LightGray;
                bi.MouseUp += delegate(object sender, MouseEventArgs e) {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        ContextMenuStrip ms = new ContextMenuStrip();
                        ms.Items.Add("查看详情 ...").Click += delegate {
                            UserDetailWindow window = _GetDetailWindow((Contact)bi.Tag, true);
                            window.Show(_dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
                        };

                        ms.Show(_contactsItemPanel, e.Location);
                    }
                };
            });
        }

        private readonly Dictionary<string, UserDetailWindow> _detailWindows = new Dictionary<string, UserDetailWindow>();

        private UserDetailWindow _GetDetailWindow(Contact contact, bool autoCreate)
        {
            if (autoCreate)
            {
                return _detailWindows.GetOrSet(contact.UserName, (key) => {
                    var window = new UserDetailWindow(contact, UserCtx);
                    window.FormClosed += delegate { _detailWindows.Remove(key); };
                    return window;
                });
            }
            else
            {
                return _detailWindows.GetOrDefault(contact.UserName);
            }
        }

        // 刷新状态
        private void _RefreshStatus()
        {
            // 获取联系人的状态
            ContactStatus[] statuses = UserCtx.UserCon.Invoker.User.GetFollowingsStatus(DateTime.MinValue);
            _SetStatuses(statuses);
        }

        private void following_DoubleClick(object sender, EventArgs e)
        {
            ButtonItem bi = (ButtonItem)sender;
            Contact contact = (Contact)bi.Tag;

            try
            {
                _StartMessage(new[] { contact });
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        private ButtonItem[] _AddContacts(Contact[] contacts, ItemPanel panel)
        {
            panel.BeginUpdate();
            panel.Items.Clear();

            List<ButtonItem> buttonItems = new List<ButtonItem>();
            foreach (Contact contact in contacts)
            {
                ButtonItem buttonItem = new ButtonItem() {
                    Text = contact.UserName, Tag = contact, Name = contact.UserName ?? Guid.NewGuid().ToString(), AutoCheckOnClick = true,
                    ButtonStyle = eButtonStyle.ImageAndText, ItemAlignment = eItemAlignment.Far, Image = Resources.User.ToBitmap(),
                };

                buttonItems.Add(buttonItem);
                panel.Items.Add(buttonItem);
            }
            panel.EndUpdate();

            return buttonItems.ToArray();
        }

        // 加载粉丝
        private void _LoadFollowers()
        {
            _followersItemPanel.Items.Clear();
            Contact[] contacts = UserCtx.UserCon.Invoker.User.GetUserInfos(Users.FromMyFollowers());
            _AddContacts(contacts, _followersItemPanel);
        }

        // 加载群组
        private void _LoadGroups()
        {
            _groupItemPanel.Items.Clear();
            FcGroupInfo[] groupInfos = UserCtx.UserCon.Invoker.Group.GetAllGroups();

            _groupItemPanel.BeginUpdate();
            _groupItemPanel.Items.Clear();
            foreach (FcGroupInfo groupInfo in groupInfos)
            {
                _GetGroupButtonItem(groupInfo);
            }
            _groupItemPanel.EndUpdate();
        }

        private ButtonItem _FindContactButtonItem(int userId)
        {
            return _FindButtonItem(_contactsItemPanel, userId);
        }

        private ButtonItem _FindButtonItem(ItemPanel panel, int userId)
        {
            foreach (ButtonItem buttonItem in panel.Items)
            {
                if (((Contact)buttonItem.Tag).UserId == userId)
                    return buttonItem;
            }

            return null;
        }

        private ButtonItem _GetGroupButtonItem(FcGroupInfo groupInfo, bool autoAdd = true)
        {
            ButtonItem buttonItem = _GetGroupButtonItem(groupInfo.GroupId);
            if (buttonItem != null || !autoAdd)
                return buttonItem;

            buttonItem = new ButtonItem() {
                Text = groupInfo.Name + "(" + groupInfo.GroupId + ")", Tag = groupInfo, Name = groupInfo.GroupId.ToString(), AutoCheckOnClick = true,
                ButtonStyle = eButtonStyle.ImageAndText, ItemAlignment = eItemAlignment.Far, Image = Resources.Group.ToBitmap(),
            };

            buttonItem.DoubleClick += delegate { _OpenMessageWindow(buttonItem); };
            _groupItemPanel.Items.Add(buttonItem);
            return buttonItem;
        }

        private ButtonItem _GetGroupButtonItem(int groupId)
        {
            return _groupItemPanel.Items.Cast<ButtonItem>().FirstOrDefault(bi => _GetGroupInfo(bi).GroupId == groupId);
        }

        private ButtonItem _GetGroupButtonItem(int groupId, bool autoAdd)
        {
            ButtonItem buttonItem = _GetGroupButtonItem(groupId);
            if (buttonItem != null || !autoAdd)
                return buttonItem;

            FcGroupInfo groupInfo = UserCtx.UserCon.Invoker.Group.GetGroup(groupId);
            if (groupInfo == null)
                throw new Exception("群组不存在");

            return _GetGroupButtonItem(groupInfo, true);
        }

        private FcGroupInfo _GetGroupInfo(ButtonItem buttonItem)
        {
            FcGroupInfo fgInfo = buttonItem.Tag as FcGroupInfo;
            if (fgInfo != null)
                return fgInfo;

            return ((GroupMessageForm)buttonItem.Tag).GroupInfo;
        }

        // 添加关注
        private void _addFollowingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserInfo[] userInfos = SelectDialog.Show(this, Settings.GetUserInfos(), SelectionMode.MultiExtended);
            if (userInfos.IsNullOrEmpty())
                return;

            try
            {
                UserCtx.UserCon.Invoker.User.AddContacts(userInfos.ToArray(ui => ui.ToContact()));
                _LoadContacts();
                _RefreshStatus();
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(this, ex);
            }
        }

        private Contact[] _GetSelectedContacts()
        {
            List<Contact> items = new List<Contact>();
            foreach (ButtonItem buttonItem in _contactsItemPanel.Items)
            {
                if (buttonItem.Checked)
                    items.Add((Contact)buttonItem.Tag);
            }

            return items.ToArray();
        }

        // 发起会话
        private void _startMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Contact[] contacts = _GetSelectedContacts();
            if (contacts.Length == 0)
            {
                this.ShowError("请首先选择要发起会话的联系人");
                return;
            }

            try
            {
                _StartMessage(contacts);
            }
            catch (Exception ex)
            {
                this.ShowError(ex);
            }
        }

        // 发起会话
        private void _StartMessage(Contact[] contacts)
        {
            string[] usernames = contacts.Select(c => c.UserName).Distinct().ToArray();
            Users users = Users.FromUserNames(usernames);

            int groupId = UserCtx.UserCon.Invoker.Group.CreateGroup("My Group", users.ToString());
            FcGroupInfo groupInfo = new FcGroupInfo() { GroupId = groupId, Name = "My Group" };
            ButtonItem buttonItem = _GetGroupButtonItem(groupInfo);

            _OpenMessageWindow(buttonItem);
        }

        private GroupMessageForm _OpenMessageWindow(ButtonItem buttonItem)
        {
            GroupMessageForm msgForm;
            if ((msgForm = buttonItem.Tag as GroupMessageForm) == null)
            {
                msgForm = new GroupMessageForm(_clientCtx, UserCtx, (FcGroupInfo)buttonItem.Tag);
                buttonItem.Tag = msgForm;
            }

            msgForm.Show(_dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            return msgForm;
        }

        private void _CloseMessageWindow(ButtonItem buttonItem)
        {
            GroupMessageForm msgForm = buttonItem.Tag as GroupMessageForm;
            if (msgForm != null)
            {
                msgForm.HideOnClose = false;
                msgForm.Close();
            }
        }

        private void _refreshContactsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _LoadContacts();
        }

        private void _refreshFollowersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _LoadFollowers();
        }

        private void _refreshGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _LoadGroups();
        }
    }
}
