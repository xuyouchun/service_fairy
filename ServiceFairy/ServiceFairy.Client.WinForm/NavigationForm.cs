using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.WinForm.Docking;
using System.Diagnostics.Contracts;
using DevComponents.DotNetBar;
using Common.WinForm;
using ServiceFairy.Client.WinForm.Properties;
using Common.Contracts.Service;
using ServiceFairy.Entities.User;

namespace ServiceFairy.Client.WinForm
{
    partial class NavigationForm : DockContentEx
    {
        public NavigationForm(ClientContext clientCtx)
        {
            InitializeComponent();

            _clientCtx = clientCtx;
        }

        private readonly ClientContext _clientCtx;

        private void NavigationForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 批量添加用户
        /// </summary>
        /// <param name="userInfos"></param>
        public void AddRange(UserInfo[] userInfos)
        {
            Contract.Requires(userInfos != null);

            foreach (UserInfo userInfo in userInfos)
            {
                _Add(userInfo);
            }
        }

        private void _Add(UserInfo userInfo)
        {
            ButtonItem buttonItem = new ButtonItem {
                Text = userInfo.UserName ?? "", Name = userInfo.UserName ?? Guid.NewGuid().ToString(),
                ButtonStyle = eButtonStyle.ImageAndText, ItemAlignment = eItemAlignment.Far, //AutoCheckOnClick = true,
                Image = Resources.User.ToBitmap(),
            };

            buttonItem.DoubleClick += new EventHandler(buttonItem_DoubleClick);
            buttonItem.Tag = userInfo;
            _usersPanel.Items.Add(buttonItem);
        }

        // 双击出现用户窗口
        private void buttonItem_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                ButtonItem buttonItem = (ButtonItem)sender;
                UserMainFrame userWin = buttonItem.Tag as UserMainFrame;
                if (userWin == null)
                {
                    userWin = _CreateUserMainWindow((UserInfo)buttonItem.Tag);
                    buttonItem.Tag = userWin;
                }

                //userWin.Show(_clientCtx.Panel, new Rectangle { X = 100, Y = 100, Width = 800, Height = 500 });
                userWin.Show(_clientCtx.Panel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(this, ex);
            }
        }

        private UserMainFrame _CreateUserMainWindow(UserInfo userInfo)
        {
            UserContext userCtx = _clientCtx.UserCtxMgr.Add(userInfo.UserName, userInfo.Password);
            
            return new UserMainFrame(_clientCtx, userCtx);
        }

        /// <summary>
        /// 添加一个用户
        /// </summary>
        /// <param name="userInfo"></param>
        public void Add(UserInfo userInfo)
        {
            Contract.Requires(userInfo != null);
            AddRange(new[] { userInfo });
        }
    }
}
