using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace Common.Contracts.UIObject
{
    /// <summary>
    /// 树形目录结构窗口
    /// </summary>
    public interface ITreeViewWindow : IUIWindow
    {
        /// <summary>
        /// 显示树形结构
        /// </summary>
        /// <param name="executeContext"></param>
        /// <param name="root"></param>
        /// <param name="expandDeep"></param>
        void Show(IUIObjectExecuteContext executeContext, IServiceObjectTreeNode root, int expandDeep);

        /// <summary>
        /// 更新子节点
        /// </summary>
        /// <param name="serviceObject"></param>
        /// <param name="executeContext"></param>
        void UpdateChildren(IUIObjectExecuteContext executeContext, IServiceObject serviceObject);

        /// <summary>
        /// 设置当前节点
        /// </summary>
        /// <param name="serviceObject"></param>
        void SetCurrent(IServiceObject serviceObject);
    }
}
