using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts.UIObject
{
    /// <summary>
    /// 列表视图的管理器
    /// </summary>
    public interface IListViewWindowManager
    {
        /// <summary>
        /// 获取当前的列表视图
        /// </summary>
        /// <param name="autoCreate"></param>
        /// <returns></returns>
        IListViewWindow GetCurrent(bool autoCreate = false);

        /// <summary>
        /// 创建新的列表视图管理器
        /// </summary>
        /// <param name="setCurrent"></param>
        /// <returns></returns>
        IListViewWindow CreateNew(bool setCurrent = true);

        /// <summary>
        /// 设置当前的列表视图
        /// </summary>
        /// <param name="window"></param>
        void SetCurrent(IListViewWindow window);

        /// <summary>
        /// 关闭指定的列表视图
        /// </summary>
        /// <param name="window"></param>
        void Close(IListViewWindow window);

        /// <summary>
        /// 获取全部的列表视图
        /// </summary>
        /// <returns></returns>
        IListViewWindow[] GetAll();

        /// <summary>
        /// 当前列表发生变化
        /// </summary>
        event EventHandler CurrentChanged;
    }
}
