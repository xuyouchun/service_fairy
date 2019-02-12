using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;

namespace Common.Contracts.UIObject
{
    /// <summary>
    /// 窗口
    /// </summary>
    public interface IUIWindow
    {
        /// <summary>
        /// 设置窗口的信息
        /// </summary>
        /// <param name="info"></param>
        void SetWindowInfo(UIWindowInfo info);

        /// <summary>
        /// 将该窗口设置为活动窗口
        /// </summary>
        void Activate();

        /// <summary>
        /// 显示状态
        /// </summary>
        bool Visible { get; set; }
    }

    /// <summary>
    /// 窗口的信息
    /// </summary>
    public class UIWindowInfo : MarshalByRefObjectEx
    {
        public UIWindowInfo(string title, IUIObjectImageLoader imageLoader)
        {
            Title = title;
            ImageLoader = imageLoader;
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 图标
        /// </summary>
        public IUIObjectImageLoader ImageLoader { get; private set; }

        /// <summary>
        /// 从UIObject创建
        /// </summary>
        /// <param name="uiObject"></param>
        /// <returns></returns>
        public static UIWindowInfo FromUIObject(IUIObject uiObject)
        {
            Contract.Requires(uiObject != null);

            ServiceObjectInfo info = uiObject.Info;
            return new UIWindowInfo(info.GetTitle(), uiObject);
        }

        /// <summary>
        /// 从ServiceObject创建
        /// </summary>
        /// <param name="serviceObject"></param>
        /// <returns></returns>
        public static UIWindowInfo FromServiceObject(IServiceObject serviceObject)
        {
            Contract.Requires(serviceObject != null);

            IUIObject uiObject = serviceObject.GetUIObject();
            if (uiObject != null)
                return FromUIObject(uiObject);

            return new UIWindowInfo(serviceObject.Info.GetTitle(), EmptyUIObjectImageLoader.Instance);
        }

        /// <summary>
        /// 从ServiceObjectTreeNode创建
        /// </summary>
        /// <param name="treeNode"></param>
        /// <returns></returns>
        public static UIWindowInfo FromServiceObjectTreeNode(IServiceObjectTreeNode treeNode)
        {
            Contract.Requires(treeNode != null);

            return FromServiceObject(treeNode.ServiceObject);
        }
    }
}
