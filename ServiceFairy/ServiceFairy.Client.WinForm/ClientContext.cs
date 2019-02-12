using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;

namespace ServiceFairy.Client.WinForm
{
    class ClientContext
    {
        public ClientContext(DockPanel panel, UserContextManager userCtxMgr)
        {
            Panel = panel;
            UserCtxMgr = userCtxMgr;
        }

        /// <summary>
        /// 停靠窗口
        /// </summary>
        public DockPanel Panel { get; private set; }

        /// <summary>
        /// 用户状态管理器
        /// </summary>
        public UserContextManager UserCtxMgr { get; private set; }
    }
}
