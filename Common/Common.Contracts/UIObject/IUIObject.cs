using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Common.Contracts.Service;

namespace Common.Contracts.UIObject
{
    /// <summary>
    /// 具有界面显示与交互功能的对象
    /// </summary>
    public interface IUIObject : IUIObjectImageLoader
    {
        /// <summary>
        /// 信息
        /// </summary>
        ServiceObjectInfo Info { get; }
    }

    #region Class UIObject ...

    /// <summary>
    /// 空的UIObject对象
    /// </summary>
    public abstract class UIObjectBase : MarshalByRefObjectEx, IUIObject
    {
        public UIObjectBase(ServiceObjectInfo info, IUIObjectImageLoader imageLoader)
        {
            _info = info ?? ServiceObjectInfo.Empty;
            _imageLoader = imageLoader ?? EmptyUIObjectImageLoader.Instance;
        }

        private readonly ServiceObjectInfo _info;
        private readonly IUIObjectImageLoader _imageLoader;

        public virtual ServiceObjectInfo Info
        {
            get { return _info; }
        }

        public virtual UIObjectImage GetImage(Size size)
        {
            return _imageLoader.GetImage(size);
        }

        public static readonly UIObjectBase Empty = new ServiceUIObject(null, null);
    }

    #endregion

    #region Class ServiceUIObject ...

    /// <summary>
    /// 空的ServiceUIObject对象
    /// </summary>
    public class ServiceUIObject : UIObjectBase, IUIObject
    {
        public ServiceUIObject(ServiceObjectInfo info, IUIObjectImageLoader imageLoader)
            : base(info, imageLoader)
        {
            
        }
    }

    #endregion

    #region Class ActionUIObject ...

    /// <summary>
    /// 指令的UI对象
    /// </summary>
    public class ActionUIObject : UIObjectBase, IActionUIObject
    {
        public ActionUIObject(ServiceObjectInfo info, IUIObjectImageLoader imageLoader, IUIObjectExecutor executor)
            : base(info, imageLoader)
        {
            _executor = executor;
        }

        private readonly IUIObjectExecutor _executor;

        public void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
        {
            _executor.Execute(context, serviceObject);
        }

        public UIObjectExecutorState GetState(IUIObjectExecuteContext context, IServiceObject serviceObject)
        {
            return _executor.GetState(context, serviceObject);
        }
    }

    #endregion

    #region Class PropertyUIObject ...

    /// <summary>
    /// 属性的UI对象
    /// </summary>
    public class PropertyUIObject : UIObjectBase, IPropertyUIObject
    {
        public PropertyUIObject(ServiceObjectInfo info, IUIObjectImageLoader imageLoader, IUIObjectExecutor executor)
            : base(info, imageLoader)
        {
            _executor = executor;
        }

        private readonly IUIObjectExecutor _executor;

        public void ShowProperty(IUIObjectExecuteContext context, IServiceObject serviceObject)
        {
            _executor.Execute(context, serviceObject);
        }
    }

    #endregion

    /// <summary>
    /// UIObject的上下文执行环境
    /// </summary>
    public interface IUIObjectExecuteContext
    {
        IServiceProvider ServiceProvider { get; }
    }

    /// <summary>
    /// 指令的UI对象
    /// </summary>
    public interface IActionUIObject : IUIObject, IUIObjectExecutor
    {
        
    }

    /// <summary>
    /// 空的指令UI对象
    /// </summary>
    public class EmptyActionUIObject : MarshalByRefObjectEx, IActionUIObject
    {
        public EmptyActionUIObject(ServiceObjectInfo info)
        {
            Info = info ?? ServiceObjectInfo.Empty;
        }

        public void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
        {
            
        }

        public UIObjectExecutorState GetState(IUIObjectExecuteContext context, IServiceObject serviceObject)
        {
            return UIObjectExecutorState.Default;
        }

        public ServiceObjectInfo Info { get; private set; }

        public UIObjectImage GetImage(Size size)
        {
            return null;
        }
    }

    /// <summary>
    /// 属性的UI对象
    /// </summary>
    public interface IPropertyUIObject : IUIObject
    {
        /// <summary>
        /// 显示属性表
        /// </summary>
        /// <param name="context"></param>
        void ShowProperty(IUIObjectExecuteContext context, IServiceObject serviceObject);
    }
}
