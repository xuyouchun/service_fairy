using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Utility;
using System.Reflection;
using Common.Contracts;
using Common.Contracts.UIObject;
using System.Drawing;

namespace Common.Package.Service
{
    /// <summary>
    /// Service Object 执行器
    /// </summary>
    public class ServiceObjectExecutor : MarshalByRefObjectEx, IServiceObjectExecutor
    {
        public ServiceObjectExecutor(ServiceObjectActionItem[] actionItems, ServiceObjectPropertyItem[] propertyItems)
        {
            _actionItemsDict = (actionItems ?? new ServiceObjectActionItem[0]).ToDictionary(v => v.Action.Info.Name);
            _propertyItemsDict = (propertyItems ?? new ServiceObjectPropertyItem[0]).ToDictionary(v => v.Property.Info.Name);
        }

        private readonly Dictionary<string, ServiceObjectActionItem> _actionItemsDict;
        private readonly Dictionary<string, ServiceObjectPropertyItem> _propertyItemsDict;

        #region Class ServiceObjectActionItem ...

        public class ServiceObjectActionItem : MarshalByRefObjectEx
        {
            public ServiceObjectActionItem(ServiceObjectAction action, Func<IDictionary<string, object>, object> func, ServiceObjectActionType actionType)
            {
                Contract.Requires(action != null && func != null);

                Action = action;
                Func = func;
                ActionType = actionType;
            }

            public ServiceObjectAction Action { get; private set; }

            public Func<IDictionary<string, object>, object> Func { get; private set; }

            public ServiceObjectActionType ActionType { get; private set; }
        }

        #endregion

        #region Class ServiceObjectPropertyItem ...

        public class ServiceObjectPropertyItem : MarshalByRefObjectEx
        {
            public ServiceObjectPropertyItem(ServiceObjectProperty property, Func<object> func)
            {
                Contract.Requires(property != null && func != null);

                Property = property;
                Func = func;
            }

            public ServiceObjectProperty Property { get; private set; }

            public Func<object> Func { get; private set; }
        }

        #endregion

        public object DoAction(string action, IDictionary<string, object> parameters)
        {
            Contract.Requires(action != null);

            ServiceObjectActionItem actionItem;
            if (!_actionItemsDict.TryGetValue(action, out actionItem))
                return null;

            return actionItem.Func(parameters);
        }

        public object GetProperty(string property)
        {
            Contract.Requires(property != null);

            ServiceObjectPropertyItem propertyItem;
            if (!_propertyItemsDict.TryGetValue(property, out propertyItem))
                return null;

            return propertyItem.Func();
        }

        /// <summary>
        /// 获取全部指令
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public ServiceObjectAction[] GetActions(ServiceObjectActionType actionType)
        {
            return _actionItemsDict.Values.Where(item => (item.ActionType & actionType) != 0).ToArray(item => item.Action);
        }

        /// <summary>
        /// 获取全部属性
        /// </summary>
        /// <returns></returns>
        public ServiceObjectProperty[] GetProperties()
        {
            return _propertyItemsDict.Values.ToArray(v => v.Property);
        }

        public static ServiceObjectExecutor LoadFromObject(object obj)
        {
            Contract.Requires(obj != null);

            Type t = obj.GetType();
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            ServiceObjectActionItem[] actionItems = t.SearchMembers<ServiceObjectActionItem, ServiceObjectActionAttribute>(
                (attrs, mInfo) => _CreateServiceObjectActionItem(obj, attrs[0], (MethodInfo)mInfo), flags).ToArray();
            ServiceObjectPropertyItem[] propertyItems = t.SearchMembers<ServiceObjectPropertyItem, ServiceObjectPropertyAttribute>(
                (attrs, mInfo) => _CreateServiceObjectPropertyItem(obj, attrs[0], (PropertyInfo)mInfo), flags).ToArray();

            return new ServiceObjectExecutor(actionItems, propertyItems);
        }

        private static ServiceObjectActionItem _CreateServiceObjectActionItem(object obj, ServiceObjectActionAttribute attr, MethodInfo mInfo)
        {
            ServiceObjectParameter[] parameters = _GetInputServiceObjectParameters(mInfo);
            ServiceObjectInfo info = ServiceObjectInfoAttribute.GetInfo(mInfo);
            string group = ServiceObjectGroupAttribute.GetGroup(mInfo);

            ServiceObjectAction action = new ServiceObjectAction(info, parameters, _GetOutputServiceObjectParameter(mInfo), group, attr.ActionType);
            IUIObjectImageLoader imageLoader = UIObjectImageAttributeBase.CreateImageLoader(mInfo);
            IUIObjectExecutor executor = UIObjectActionAttributeBase.CreateUIObject(obj, mInfo);

            ActionUIObject uiObject = new ActionUIObject(info, imageLoader ?? EmptyUIObjectImageLoader.Instance, executor ?? EmptyUIObjectExecutor.Instance);
            action.SetUIObject(uiObject);
            return new ServiceObjectActionItem(action, _GetActionFunc(obj, mInfo, parameters), attr.ActionType);
        }

        private static Func<IDictionary<string, object>, object> _GetActionFunc(object obj, MethodInfo mInfo, ServiceObjectParameter[] parameters)
        {
            ParameterInfo[] pInfos = mInfo.GetParameters();
            return new Func<IDictionary<string, object>, object>(delegate(IDictionary<string, object> dict) {
                object[] values = new object[parameters.Length];

                for (int k = 0, length = parameters.Length; k < length; k++)
                {
                    ServiceObjectParameter p = parameters[k];
                    object value;
                    if (!dict.TryGetValue(p.Info.Name, out value))
                        value = ServiceUtility.GetParameterDefaultValue(pInfos[k]);

                    values[k] = value;
                }

                return mInfo.Invoke(obj, values);
            });
        }

        private static ServiceObjectParameter _GetOutputServiceObjectParameter(MethodInfo mInfo)
        {
            return ServiceUtility.GetServiceObjectParameter(mInfo.ReturnParameter);
        }

        private static ServiceObjectParameter[] _GetInputServiceObjectParameters(MethodInfo mInfo)
        {
            return mInfo.GetParameters().ToArray(paramInfo => ServiceUtility.GetServiceObjectParameter(paramInfo));
        }

        private static ServiceObjectPropertyItem _CreateServiceObjectPropertyItem(object obj, ServiceObjectPropertyAttribute attr, PropertyInfo propertyInfo)
        {
            MethodInfo getMethodInfo = propertyInfo.GetGetMethod();
            if (getMethodInfo == null)
                throw new FormatException("属性必须是可读的");

            ServiceObjectInfo info = ServiceObjectInfoAttribute.GetInfo(propertyInfo);
            string group = ServiceObjectGroupAttribute.GetGroup(propertyInfo);
            IUIObjectExecutor executor = UIObjectPropertyAttributeBase.CreateUIObject(obj, propertyInfo);
            IUIObjectImageLoader imageLoader = UIObjectImageAttributeBase.CreateImageLoader(propertyInfo);
            PropertyUIObject uiObject = new PropertyUIObject(info, imageLoader ?? EmptyUIObjectImageLoader.Instance, executor ?? EmptyUIObjectExecutor.Instance);
            ServiceObjectProperty property = new ServiceObjectProperty(info, ServiceUtility.GetServiceObjectParameter(getMethodInfo.ReturnParameter), group);
            property.SetUIObject(uiObject);
            return new ServiceObjectPropertyItem(property, _CreateGetPropertyFunc(obj, getMethodInfo));
        }

        private static Func<object> _CreateGetPropertyFunc(object obj, MethodInfo mInfo)
        {
            return new Func<object>(delegate() {
                return mInfo.Invoke(obj, null);
            });
        }
    }
}
