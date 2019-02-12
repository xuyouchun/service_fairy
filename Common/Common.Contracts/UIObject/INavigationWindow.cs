using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace Common.Contracts.UIObject
{
    /// <summary>
    /// 导航项
    /// </summary>
    public interface INavigation
    {
        /// <summary>
        /// 显示导航
        /// </summary>
        /// <param name="executeContext"></param>
        /// <param name="provider"></param>
        void Show(IUIObjectExecuteContext executeContext, IServiceObjectProvider provider);

        /// <summary>
        /// 显示当前节点
        /// </summary>
        /// <param name="executeContext"></param>
        /// <param name="serviceObject"></param>
        void SetCurrent(IUIObjectExecuteContext executeContext, IServiceObject serviceObject);
    }

    /// <summary>
    /// 导航窗口
    /// </summary>
    public interface INavigationWindow : INavigation, IUIWindow
    {
        
    }
}
