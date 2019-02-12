using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Contracts;
using Common.Utility;
using Common.Package.Service;
using Common.Collection;
using System.Diagnostics.Contracts;
using Common.Package.GlobalTimer;
using System.Threading;
using Common.Package.UIObject;
using Common.Contracts.UIObject;
using System.Reflection;

namespace Common.Package.Service
{
    /// <summary>
    /// 同时具有树节点与ServiceObject功能的对象基类
    /// </summary>
    public abstract class ServiceObjectKernelTreeNodeBase : MarshalByRefObjectEx, IServiceObjectTreeNode, IRefreshableServiceObjectTreeNode, IServiceObjectInfoProvider, IObjectKeyProvider
    {
        public ServiceObjectKernelTreeNodeBase()
        {
            _kernel = this;
            CurrentStatus = ServiceStatus.Default;
            _node = new Lazy<IServiceObjectTreeNode>(_CreateTreeNode, true);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ar"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        internal IAsyncResult BeginInit(AsyncCallback callback, object state)
        {
            AsyncResult ar = new AsyncResult(this);
            StartRefresh(new InitRefresher(this));
            ThreadPool.QueueUserWorkItem(_WaitForCompleted, new object[] { ar, callback, state });
            return ar;
        }

        private readonly AutoResetEvent _waitForCompletedEvent = new AutoResetEvent(false);

        private void _WaitForCompleted(object f)
        {
            object[] objs = (object[])f;
            AsyncResult ar = (AsyncResult)objs[0];
            AsyncCallback callback = (AsyncCallback)objs[1];
            object state = objs[2];

            _waitForCompletedEvent.WaitOne();
            callback(ar);
        }

        protected virtual ServiceObjectRefreshResult OnInitRefresh(InitContext initCtx)
        {
            return ServiceObjectRefreshResult.Completed;
        }

        #region Class AsyncResult ...

        class AsyncResult : IAsyncResult
        {
            public AsyncResult(object state)
            {
                _state = state;
            }

            private readonly object _state;
            private volatile bool _isCompleted;

            public object AsyncState
            {
                get { return _state; }
            }

            private readonly ManualResetEvent _wh = new ManualResetEvent(false);

            public WaitHandle AsyncWaitHandle
            {
                get { return _wh; }
            }

            public bool CompletedSynchronously
            {
                get { return _isCompleted; }
            }

            public bool IsCompleted
            {
                get { return _isCompleted; }
            }

            public Exception Error { get; private set; }

            public void Complete(Exception error)
            {
                _isCompleted = true;
                Error = error;

                _wh.Set();
            }
        }

        #endregion

        #region Class InitContext ...

        protected class InitContext
        {
            public int TryTimes { get; protected set; }
        }

        #endregion

        #region Class InitRefresher ...

        class InitRefresher : IServiceObjectRefresher
        {
            public InitRefresher(ServiceObjectKernelTreeNodeBase owner)
            {
                _owner = owner;
                _initContext = new InitContextEx();
            }

            private readonly ServiceObjectKernelTreeNodeBase _owner;
            private readonly InitContextEx _initContext;

            public int TryTimes
            {
                get { return 0; }
            }

            public ServiceObjectRefreshResult Refresh()
            {
                _initContext.IncreaseTryTimes();
                ServiceObjectRefreshResult r = _owner.OnInitRefresh(_initContext);
                if ((r & ServiceObjectRefreshResult.Completed) != 0)
                {
                    _owner._waitForCompletedEvent.Set();
                }

                return r;
            }

            class InitContextEx : InitContext
            {
                public void IncreaseTryTimes()
                {
                    TryTimes++;
                }
            }
        }

        #endregion

        internal void EndInit(IAsyncResult ar)
        {
            Contract.Requires(ar != null);

            AsyncResult ar0 = (AsyncResult)ar;
            if (ar0.Error != null)
                throw ar0.Error;
        }

        public virtual void OnRefreshSubNodes(bool force)
        {
            if (_node == null || _node.IsValueCreated)
                _node = new Lazy<IServiceObjectTreeNode>(_CreateTreeNode, true);
        }

        private Lazy<IServiceObjectTreeNode> _node;
        private object _kernel;

        private IServiceObjectTreeNode _CreateTreeNode()
        {
            IServiceObjectTreeNode treeNode = new ServiceObjectTreeNode(
                RefreshableServiceObject.FromObject(_kernel), new SubTreeNodeProxy(this), HasChildren()
            );

            treeNode.ServiceObject.SetTreeNode(this);

            IUIObject uiObject = OnCreateUIObject();
            if (uiObject != null)
                treeNode.ServiceObject.SetUIObject(uiObject);

            return treeNode;
        }

        /// <summary>
        /// 创建UIObject
        /// </summary>
        /// <returns></returns>
        protected virtual IUIObject OnCreateUIObject()
        {
            return null;
        }

        #region Class SubTreeNodeProxy ...

        class SubTreeNodeProxy : IEnumerable<IServiceObjectTreeNode>
        {
            public SubTreeNodeProxy(ServiceObjectKernelTreeNodeBase owner)
            {
                _owner = owner;

                _nodes = new Lazy<IEnumerable<IServiceObjectTreeNode>>(owner.LoadServiceObjectSubTreeNodes);
            }

            private readonly ServiceObjectKernelTreeNodeBase _owner;

            private readonly Lazy<IEnumerable<IServiceObjectTreeNode>> _nodes;

            public IEnumerator<IServiceObjectTreeNode> GetEnumerator()
            {
                return _nodes.Value.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        #endregion

        protected virtual bool? HasChildren()
        {
            return null;
        }

        /// <summary>
        /// 加载子节点
        /// </summary>
        /// <returns></returns>
        internal protected virtual IEnumerable<IServiceObjectTreeNode> LoadServiceObjectSubTreeNodes()
        {
            return (OnCreateServiceObjectSubTreeNodes() ?? new IServiceObjectTreeNode[0]).Select(SetParent);
        }

        protected IServiceObjectTreeNode SetParent(IServiceObjectTreeNode node)
        {
            ServiceObjectKernelTreeNodeBase node0 = node as ServiceObjectKernelTreeNodeBase;
            if (node0 != null)
                node0.Parent = this;

            return node;
        }

        /// <summary>
        /// 父节点
        /// </summary>
        public IServiceObjectTreeNode Parent { get; private set; }

        protected virtual IEnumerable<IServiceObjectTreeNode> OnCreateServiceObjectSubTreeNodes()
        {
            return null;
        }

        protected virtual ServiceObjectInfo OnGetServiceObjectInfo()
        {
            return null;
        }

        /// <summary>
        /// 开始刷新
        /// </summary>
        /// <param name="refresher"></param>
        public void StartRefresh(IServiceObjectRefresher refresher)
        {
            Contract.Requires(refresher != null);
            ((RefreshableServiceObject)_node.Value.ServiceObject).StartRefresh(refresher);
        }

        /// <summary>
        /// 开始刷新
        /// </summary>
        /// <param name="func"></param>
        public void StartRefresh(ServiceObjectRefreshFunc func)
        {
            Contract.Requires(func != null);
            ((RefreshableServiceObject)_node.Value.ServiceObject).StartRefresh(func);
        }

        /// <summary>
        /// 停止刷新
        /// </summary>
        /// <param name="r"></param>
        public void StopAllRefresh(ServiceObjectRefreshResult r = ServiceObjectRefreshResult.Completed)
        {
            ((RefreshableServiceObject)_node.Value.ServiceObject).StopAllRefresh(r);
            _RemoveNode();
        }

        /// <summary>
        /// 停止刷新
        /// </summary>
        /// <param name="refresher"></param>
        /// <param name="r"></param>
        public void StopRefresh(IServiceObjectRefresher refresher, ServiceObjectRefreshResult r = ServiceObjectRefreshResult.Completed)
        {
            Contract.Requires(refresher != null);
            ((RefreshableServiceObject)_node.Value.ServiceObject).StopRefresh(refresher, r);
            _RemoveNode();
        }

        private void _RemoveNode()
        {
            ListServiceObjectKernelTreeNodeBase parent = Parent as ListServiceObjectKernelTreeNodeBase;
            if (parent != null)
                parent.Remove(this.GetKey());
        }

        /// <summary>
        /// 获取键
        /// </summary>
        /// <returns></returns>
        protected abstract object GetKey();

        object IObjectKeyProvider.GetKey()
        {
            return GetKey();
        }

        /// <summary>
        /// 获取当前状态的描述
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        protected virtual string GetServiceStatusDesc(ServiceStatus status)
        {
            return status.GetDesc();
        }

        /// <summary>
        /// 获取当前服务状态的描述
        /// </summary>
        /// <returns></returns>
        public string GetCurrentServiceStatusDesc()
        {
            ServiceStatus status = CurrentStatus;
            string desc = GetServiceStatusDesc(CurrentStatus);

            if (ProcessAttribute.IsProcess(status))
                desc = desc.PadDots(DateTime.Now.Second % 4);

            return desc;
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        protected ServiceStatus CurrentStatus { get; set; }

        #region IServiceObjectTreeNode Members ...

        IServiceObject IServiceObjectTreeNode.ServiceObject
        {
            get { return _node.Value.ServiceObject; }
        }

        int IReadOnlyList<IServiceObjectTreeNode>.Count
        {
            get { return _node.Value.Count; }
        }

        IServiceObjectTreeNode IReadOnlyList<IServiceObjectTreeNode>.this[int index]
        {
            get { return _node.Value[index]; }
        }

        bool? IServiceObjectTreeNode.HasChildren
        {
            get { return HasChildren(); }
        }

        public IEnumerator<IServiceObjectTreeNode> GetEnumerator()
        {
            return _node.Value.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)_node.Value).GetEnumerator();
        }

        #endregion

        #region IRefreshableServiceObjectTreeNode Members ...

        void IRefreshableServiceObjectTreeNode.Refresh(bool force)
        {
            OnRefreshSubNodes(force);
        }

        #endregion

        #region IServiceObjectInfoProvider Members ...

        ServiceObjectInfo IServiceObjectInfoProvider.GetServiceObjectInfo()
        {
            return OnGetServiceObjectInfo();
        }

        #endregion

        #region Dispose ...

        private volatile bool _disposed;

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            if (!_disposed)
            {
                _disposed = true;

                
            }
        }

        ~ServiceObjectKernelTreeNodeBase()
        {
            Dispose();
        }

        #endregion

        #region Enum ServiceStatus ...

        [Flags]
        protected enum ServiceStatus
        {
            [Desc("正在初始化"), Process]
            Init = 0x01,

            [Desc("正在启动"), Process]
            Starting = 0x02,

            [Desc("正在运行")]
            Default = 0x04,

            [Desc("正在停止"), Process]
            Stopping = 0x08,

            [Desc("正在重新启动"), Process]
            Restarting = 0x10,

            [Desc("正在取消"), Process]
            Canceling = 0x20,

            [Desc("已经停止")]
            Stopped = 0x40,

            [Desc("出现错误")]
            Error = 0x80,

            [Desc("正在删除"), Process]
            Deleting = 0x100,

            [Desc("正在上传"), Process]
            Uploading = 0x200,

            [Desc("正在部署"), Process]
            Deploying = 0x400,

            [Desc("部署完成")]
            Deployed = 0x800,

            [Desc("信号中断")]
            SignalBreak = 0x1000,

            [Desc("置为当前"), Process]
            SetCurrent = 0x2000,

            All = -1,
        }

        #endregion

        #region Class ProcessAttribute ...

        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
        class ProcessAttribute : Attribute
        {
            public ProcessAttribute(bool process = true)
            {
                Process = process;
            }

            public bool Process { get; private set; }

            static readonly Dictionary<ServiceStatus, ProcessAttribute> _dict
                = typeof(ServiceStatus).SearchMembers<ServiceStatus, ProcessAttribute, ProcessAttribute>(
                    (attrs, fInfo) => (ServiceStatus)((FieldInfo)fInfo).GetValue(null), (attrs, fInfo) => attrs[0], BindingFlags.Static | BindingFlags.Public);

            public static bool IsProcess(ServiceStatus status)
            {
                ProcessAttribute attr = _dict.GetOrDefault(status);
                return attr == null ? false : attr.Process;
            }
        }

        #endregion

        #region Class VisiableAttribute ...

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
        protected class VisiableAttribute : Attribute
        {
            public VisiableAttribute(ServiceStatus status)
            {
                Status = status;
            }

            public ServiceStatus Status { get; private set; }
        }

        #endregion

        #region Class EnableAttribute ...

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
        protected class EnableAttribute : Attribute
        {
            public EnableAttribute(ServiceStatus status)
            {
                Status = status;
            }

            public ServiceStatus Status { get; private set; }
        }

        #endregion

        #region Class ServiceStatusActionBase ...

        protected abstract class ActionBase<T> : UIObjectExecutorBaseEx<T> where T : ServiceObjectKernelTreeNodeBase
        {
            public ActionBase()
            {
                _visiableAttr = new Lazy<VisiableAttribute>(_LoadAttr<VisiableAttribute>);
                _enableAttr = new Lazy<EnableAttribute>(_LoadAttr<EnableAttribute>);
            }

            public override UIObjectExecutorState GetState(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                ServiceStatus enableStatus = _enableAttr.Value == null ? ServiceStatus.All : _enableAttr.Value.Status;
                ServiceStatus visiableStatus = _visiableAttr.Value == null ? ServiceStatus.All : _visiableAttr.Value.Status;

                ServiceStatus curStatus = Kernel.CurrentStatus;
                return new UIObjectExecutorState((enableStatus & curStatus) != 0, (visiableStatus & curStatus) != 0, false);
            }

            private Lazy<VisiableAttribute> _visiableAttr;
            private Lazy<EnableAttribute> _enableAttr;

            private TAttr _LoadAttr<TAttr>() where TAttr : Attribute
            {
                return this.MethodInfo.GetAttribute<TAttr>(true);
            }

            protected void Refresh(IUIObjectExecuteContext context, IServiceObject serviceObject)
            {
                ServiceUtility.RefreshCurrentListView(context, serviceObject);
            }

            /// <summary>
            /// 当前状态
            /// </summary>
            protected ServiceStatus CurrentStatus
            {
                get { return Kernel.CurrentStatus; }
                set { Kernel.CurrentStatus = value; }
            }

            /// <summary>
            /// 开始刷新
            /// </summary>
            /// <param name="refresher"></param>
            protected void StartRefresh(IServiceObjectRefresher refresher)
            {
                Kernel.StartRefresh(refresher);
            }

            /// <summary>
            /// 开始刷新
            /// </summary>
            /// <param name="refreshFunc"></param>
            protected void StartRefresh(ServiceObjectRefreshFunc refreshFunc)
            {
                Kernel.StartRefresh(refreshFunc);
            }

            /// <summary>
            /// 停止刷新
            /// </summary>
            /// <param name="refresher"></param>
            /// <param name="r"></param>
            protected void StopRefresh(IServiceObjectRefresher refresher, ServiceObjectRefreshResult r = ServiceObjectRefreshResult.Completed)
            {
                Kernel.StopRefresh(refresher, r);
            }

            /// <summary>
            /// 停止刷新
            /// </summary>
            /// <param name="r"></param>
            protected void StopAllRefresh(ServiceObjectRefreshResult r = ServiceObjectRefreshResult.Completed)
            {
                Kernel.StopAllRefresh(r);
            }
        }

        #endregion
    }
}
