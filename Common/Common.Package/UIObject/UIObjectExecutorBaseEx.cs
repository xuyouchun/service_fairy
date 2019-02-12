using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using Common.Contracts.Service;
using System.Reflection;

namespace Common.Package.UIObject
{
    /// <summary>
    /// UIObject指令执行器的基类，可以得到其所属对象
    /// </summary>
    public abstract class UIObjectExecutorBaseEx : UIObjectExecutorBase, IUIObjectExecutorStatusProvider
    {
        void IUIObjectExecutorStatusProvider.SetExecutorInfo(object kernel, MethodInfo methodInfo)
        {
            Kernel = kernel;
            MethodInfo = methodInfo;
        }

        /// <summary>
        /// 所属对象
        /// </summary>
        public virtual object Kernel { get; private set; }

        /// <summary>
        /// 所属方法
        /// </summary>
        public MethodInfo MethodInfo { get; private set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKernel"></typeparam>
    public abstract class UIObjectExecutorBaseEx<TKernel> : UIObjectExecutorBaseEx where TKernel : class
    {
        public new virtual TKernel Kernel
        {
            get { return base.Kernel as TKernel; }
        }
    }

    public interface IUIObjectExecutorStatusProvider
    {
        void SetExecutorInfo(object kernel, MethodInfo methodInfo);
    }
}
