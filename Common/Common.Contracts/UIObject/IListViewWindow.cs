using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Drawing;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace Common.Contracts.UIObject
{
    /// <summary>
    /// 列表显示
    /// </summary>
    public interface IListViewWindow : IUIWindow
    {
        /// <summary>
        /// 显示列表
        /// </summary>
        /// <param name="executeContext"></param>
        /// <param name="data"></param>
        /// <param name="isRefresh"></param>
        void ShowList(IUIObjectExecuteContext executeContext, IListViewData data, bool isRefresh);

        /// <summary>
        /// 获取当前的列表
        /// </summary>
        /// <returns></returns>
        IListViewData GetCurrent();

        /// <summary>
        /// 清空列表
        /// </summary>
        void Clear();

        /// <summary>
        /// 向上
        /// </summary>
        event EventHandler<ListViewWindowActionEventArgs> OnAction;

        /// <summary>
        /// 当前列表发生变化
        /// </summary>
        event EventHandler CurrentChanged;
    }

    /// <summary>
    /// 列表框的事件
    /// </summary>
    public enum ListViewWindowActionType
    {
        
    }

    /// <summary>
    /// 列表框事件参数
    /// </summary>
    [Serializable]
    public class ListViewWindowActionEventArgs : EventArgs
    {
        public ListViewWindowActionEventArgs(IUIObjectExecuteContext executeContext, IListViewData currentListViewData, ListViewWindowActionType eventType)
        {
            ExecuteContext = executeContext;
            CurrentListViewData = currentListViewData;
            EventType = eventType;
        }

        /// <summary>
        /// 上下文执行环境
        /// </summary>
        public IUIObjectExecuteContext ExecuteContext { get; private set; }

        /// <summary>
        /// 当前的列表数据
        /// </summary>
        public IListViewData CurrentListViewData { get; private set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        public ListViewWindowActionType EventType { get; private set; }
    }

    /// <summary>
    /// 列表数据
    /// </summary>
    public interface IListViewData
    {
        /// <summary>
        /// 所属节点的ServiceObject
        /// </summary>
        IServiceObject Owner { get; }

        /// <summary>
        /// 子节点数据
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 获取指定范围的子节点
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IServiceObject[] GetRange(int start, int count);

        /// <summary>
        /// 刷新列表数据
        /// </summary>
        void Refresh();

        /// <summary>
        /// 是否可以向上一层
        /// </summary>
        bool HasParent { get; }

        /// <summary>
        /// 获取上一层的数据
        /// </summary>
        /// <returns></returns>
        IListViewData GetParentData();
    }

    /// <summary>
    /// 列表数据的实现
    /// </summary>
    public class ListViewData : MarshalByRefObjectEx, IListViewData
    {
        public ListViewData(IServiceObjectTreeNode treeNode, Action<IServiceObjectTreeNode> refreshAction = null)
        {
            Contract.Requires(treeNode != null);

            Owner = treeNode.ServiceObject;
            _treeNode = treeNode;

            if ((_refreshAction = refreshAction) == null)
            {
                _refreshAction = delegate(IServiceObjectTreeNode node) {
                    IRefreshableServiceObjectTreeNode refreshable = node as IRefreshableServiceObjectTreeNode;
                    if (refreshable != null)
                        refreshable.Refresh(false);
                };
            }
        }

        private readonly IServiceObjectTreeNode _treeNode;
        private readonly Action<IServiceObjectTreeNode> _refreshAction;

        /// <summary>
        /// 节点数量
        /// </summary>
        public int Count
        {
            get { return _treeNode.Count; }
        }

        /// <summary>
        /// 获取指定范围内的ServiceObject
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IServiceObject[] GetRange(int start, int count)
        {
            return _treeNode.Range(start, count).ToArray(v => v.ServiceObject);
        }

        /// <summary>
        /// 父节点的ServiceObject
        /// </summary>
        public IServiceObject Owner { get; private set; }

        /// <summary>
        /// 刷新列表数据
        /// </summary>
        public void Refresh()
        {
            _refreshAction(_treeNode);
        }

        /// <summary>
        /// 是否有上一层的数据
        /// </summary>
        public bool HasParent
        {
            get { return _treeNode.Parent != null; }
        }

        /// <summary>
        /// 获取上一层的数据
        /// </summary>
        /// <returns></returns>
        public IListViewData GetParentData()
        {
            IServiceObjectTreeNode parent = _treeNode.Parent;
            if (parent == null)
                return null;

            return new ListViewData(parent, _refreshAction);
        }

        public override string ToString()
        {
            return Owner == null ? "" : StringUtility.GetFirstNotNullOrWhiteSpaceString(Owner.Info.Title, Owner.Info.Name);
        }
    }
}
