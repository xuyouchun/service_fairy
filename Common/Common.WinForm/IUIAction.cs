using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.WinForm
{
    /// <summary>
    /// 界面上的操作
    /// </summary>
    public interface IUIAction
    {
        /// <summary>
        /// 数据
        /// </summary>
        object Tag { get; set; }

        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="actionType"></param>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        void OnAction(UIActionType actionType, object sender, EventArgs e);
    }

    /// <summary>
    /// 界面操作基类
    /// </summary>
    public abstract class UIActionBase : IUIAction
    {
        public object Tag { get; set; }

        public abstract void OnAction(UIActionType actionType, object sender, EventArgs e);
    }

    /// <summary>
    /// 界面操作的类型
    /// </summary>
    public enum UIActionType
    {
        /// <summary>
        /// 单击
        /// </summary>
        TreeNodeMouseClick,

        /// <summary>
        /// 双击
        /// </summary>
        TreeNodeMouseDoubleClick,

        /// <summary>
        /// 鼠标移动
        /// </summary>
        MouseMove,

        /// <summary>
        /// 选择之前
        /// </summary>
        TreeNodeBeforeSelect,

        /// <summary>
        /// 选择之后
        /// </summary>
        TreeNodeAfterSelect,

        /// <summary>
        /// 树节点展开之前
        /// </summary>
        TreeNodeBeforeExpand,

        /// <summary>
        /// 树节点展开之后
        /// </summary>
        TreeNodeAfterExpand,

        /// <summary>
        /// 关闭
        /// </summary>
        Close,
    }
}
