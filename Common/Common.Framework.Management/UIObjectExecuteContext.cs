using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using Common.Contracts.Service;

namespace Common.Framework.Management
{
    /// <summary>
    /// UI对象的上下文执行环境
    /// </summary>
    class UIObjectExecuteContext : MarshalByRefObjectEx, IUIObjectExecuteContext
    {
        public UIObjectExecuteContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; private set; }
    }
}
