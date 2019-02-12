using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.WinForm.Docking
{
    /// <summary>
    /// DockContent的一些操作
    /// </summary>
    public interface IDockContentOperation
    {
        /// <summary>
        /// 关闭通知（返回false则取消关闭）
        /// </summary>
        bool ClosingNotify();
    }
}
