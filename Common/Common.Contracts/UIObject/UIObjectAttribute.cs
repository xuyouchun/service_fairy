using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Common.Utility;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;

namespace Common.Contracts.UIObject
{
    /// <summary>
    /// 标识UI对象
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class UIObjectImageAttributeBase : Attribute
    {
        /// <summary>
        /// 创建图片读取器
        /// </summary>
        /// <param name="memberOrTypeOrAssembly"></param>
        /// <returns></returns>
        public abstract IUIObjectImageLoader CreateUIObjectImageLoader(ICustomAttributeProvider attrProvider);

        public static IUIObjectImageLoader CreateImageLoader(ICustomAttributeProvider attrProvider)
        {
            UIObjectImageAttributeBase attr = attrProvider.GetAttribute<UIObjectImageAttributeBase>(true);
            if (attr == null)
                return null;

            return attr.CreateUIObjectImageLoader(attrProvider);
        }
    }

    public class EmptyUIObjectExecutor : MarshalByRefObjectEx, IUIObjectExecutor
    {
        public void Execute(IUIObjectExecuteContext context, IServiceObject serviceObject)
        {
            
        }

        public UIObjectExecutorState GetState(IUIObjectExecuteContext context, IServiceObject serviceObject)
        {
            return UIObjectExecutorState.Default;
        }

        public static readonly EmptyUIObjectExecutor Instance = new EmptyUIObjectExecutor();
    }

    /// <summary>
    /// 标识UI指令
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class UIObjectActionAttributeBase : Attribute
    {
        public abstract IUIObjectExecutor CreateActionUIObject(object kernel, MethodInfo methodInfo);

        public static IUIObjectExecutor CreateUIObject(object kernel, MethodInfo methodInfo)
        {
            Contract.Requires(methodInfo != null);

            UIObjectActionAttributeBase attr = methodInfo.GetAttribute<UIObjectActionAttributeBase>(true);
            if (attr == null)
                return null;

            return attr.CreateActionUIObject(kernel, methodInfo);
        }
    }

    /// <summary>
    /// 标识UI属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class UIObjectPropertyAttributeBase : Attribute
    {
        public abstract IUIObjectExecutor CreatePropertyUIObject(object kernel, PropertyInfo propertyInfo);

        public static IUIObjectExecutor CreateUIObject(object kernel, PropertyInfo propertyInfo)
        {
            Contract.Requires(propertyInfo != null);

            UIObjectPropertyAttributeBase attr = propertyInfo.GetAttribute<UIObjectPropertyAttributeBase>(true);
            if (attr == null)
                return null;

            return attr.CreatePropertyUIObject(kernel, propertyInfo);
        }
    }
}
