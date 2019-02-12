using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using Common.Contracts.Service;

namespace Common.Package.UIObject
{
    /// <summary>
    /// UIObject指令执行器的基类
    /// </summary>
    public abstract class UIObjectExecutorBase : MarshalByRefObjectEx, IUIObjectExecutor
    {
        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="context"></param>
        /// <param name="serviceObject"></param>
        public abstract void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject);

        /// <summary>
        /// 是否为有效状态
        /// </summary>
        /// <param name="context"></param>
        /// <param name="serviceObject"></param>
        /// <returns></returns>
        public virtual UIObjectExecutorState GetState(IUIObjectExecuteContext context, IServiceObject serviceObject)
        {
            return UIObjectExecutorState.Default;
        }
    }
}
