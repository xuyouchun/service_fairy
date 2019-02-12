using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.UIObject;
using System.Reflection;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace Common.Package.UIObject
{
    /// <summary>
    /// 标识UI对象
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class UIObjectImageAttribute : UIObjectImageAttributeBase
    {
        public UIObjectImageAttribute(string resourceName)
        {
            _resourceName = resourceName;
        }

        private readonly string _resourceName;

        public override IUIObjectImageLoader CreateUIObjectImageLoader(ICustomAttributeProvider attrProvider)
        {
            return ResourceUIObjectImageLoader.Load(attrProvider, _resourceName);
        }
    }

    /// <summary>
    /// 标识UI指令
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UIObjectActionAttribute : UIObjectActionAttributeBase
    {
        public UIObjectActionAttribute(Type executorType)
        {
            Contract.Requires(executorType != null);
            _executorType = executorType;
        }

        private Type _executorType;
        private IUIObjectExecutor _executor;

        private IUIObjectExecutor _CreateUIObjectExecutor(object kernel, MethodInfo methodInfo)
        {
            IUIObjectExecutor executor = ObjectFactory.CreateObject(_executorType) as IUIObjectExecutor;
            IUIObjectExecutorStatusProvider statusProvider = executor as IUIObjectExecutorStatusProvider;
            if (statusProvider != null)
                statusProvider.SetExecutorInfo(kernel, methodInfo);

            return executor;
        }

        public override IUIObjectExecutor CreateActionUIObject(object kernel, MethodInfo methodInfo)
        {
            return ThreadUtility.Create(ref _executor, () => _CreateUIObjectExecutor(kernel, methodInfo));
        }
    }

    /// <summary>
    /// 标识UI属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UIObjectPropertyAttribute : UIObjectPropertyAttributeBase
    {
        public UIObjectPropertyAttribute(Type executorType)
        {
            Contract.Requires(executorType != null);

            _executorType = executorType;
        }

        private readonly Type _executorType;
        private IUIObjectExecutor _executor;

        private IUIObjectExecutor _CreateUIObjectExecutor(object kernel, PropertyInfo propertyInfo)
        {
            if (ReflectionUtility.CanCreateDirect(_executorType))
                return Activator.CreateInstance(_executorType) as IUIObjectExecutor;

            return Activator.CreateInstance(_executorType, new object[] { kernel }) as IUIObjectExecutor;
        }

        public override IUIObjectExecutor CreatePropertyUIObject(object kernel, PropertyInfo propertyInfo)
        {
            return ThreadUtility.Create(ref _executor, () => _CreateUIObjectExecutor(kernel, propertyInfo));
        }
    }
}
