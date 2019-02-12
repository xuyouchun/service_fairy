using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Collection;
using System.Reflection;
using Common.Contracts;
using Common.Contracts.UIObject;

namespace Common.Package.Service
{
    public class ServiceObject : MarshalByRefObjectEx, IServiceObject
    {
        public ServiceObject(ServiceObjectInfo info, ServiceObjectGroup[] groups, ServiceObjectAction[] actions,
            ServiceObjectProperty[] properties, IServiceObjectExecutor executor)
        {
            Contract.Requires(info != null);

            Info = info;
            _groups = groups ?? new ServiceObjectGroup[0];
            _actions = actions ?? new ServiceObjectAction[0];
            _properties = properties ?? new ServiceObjectProperty[0];
            _executor = executor ?? new EmptyServiceObjectExecutor();
            Items = new ThreadSafeDictionaryWrapper<object, object>();
        }

        private readonly ServiceObjectGroup[] _groups;
        private readonly ServiceObjectProperty[] _properties;
        private readonly ServiceObjectAction[] _actions;
        private readonly IServiceObjectExecutor _executor;

        /// <summary>
        /// 描述信息
        /// </summary>
        public ServiceObjectInfo Info { get; private set; }

        /// <summary>
        /// 获取所有的组
        /// </summary>
        /// <returns></returns>
        public ServiceObjectGroup[] GetGroups()
        {
            return _groups;
        }

        /// <summary>
        /// 获取所有指令
        /// </summary>
        /// <returns></returns>
        public ServiceObjectAction[] GetActions(ServiceObjectActionType actionType)
        {
            return _actions.Where(action => (action.ActionType & actionType) != 0).ToArray();
        }

        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object DoAction(string actionName, IDictionary<string, object> parameters)
        {
            return _executor.DoAction(actionName, parameters);
        }

        /// <summary>
        /// 获取所有属性
        /// </summary>
        /// <returns></returns>
        public ServiceObjectProperty[] GetProperties()
        {
            return _properties;
        }

        /// <summary>
        /// 获取指定的属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public object GetPropertyValue(string propertyName)
        {
            return _executor.GetProperty(propertyName);
        }

        public IDictionary<object, object> Items { get; private set; }

        /// <summary>
        /// 从指定的对象加载
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static ServiceObject FromObject(object obj, ServiceObjectInfo info = null)
        {
            Contract.Requires(obj != null);

            Type t = obj.GetType();
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            IServiceObjectInfoProvider soInfoProvider = obj as IServiceObjectInfoProvider;
            info = ServiceObjectInfo.Combine(soInfoProvider == null ? null : soInfoProvider.GetServiceObjectInfo(), info, ServiceObjectInfoAttribute.GetInfo(t));
            ServiceObjectExecutor executor = ServiceObjectExecutor.LoadFromObject(obj);
            
            ServiceObjectAction[] actions = executor.GetActions(ServiceObjectActionType.All);
            ServiceObjectProperty[] properties = executor.GetProperties();

            ServiceObjectGroup[] groups = t.SearchMembers<ServiceObjectGroup, ServiceObjectGroupDescAttribute>(
                (attrs, mInfo) => _CreateServiceObjectGroup(obj, attrs[0], mInfo), flags).ToArray();

            IUIObjectImageLoader imageLoader = UIObjectImageAttributeBase.CreateImageLoader(t);
            ServiceObject so = new ServiceObject(info, groups, actions, properties, executor);
            ServiceUIObject uiObject = new ServiceUIObject(info, imageLoader);
            so.SetUIObject(uiObject);
            return so;
        }

        private static ServiceObjectGroup _CreateServiceObjectGroup(object obj, ServiceObjectGroupDescAttribute attr, MemberInfo mInfo)
        {
            ServiceObjectInfo info = ServiceObjectInfoAttribute.GetInfo(mInfo);
            IUIObjectImageLoader imageLoader = UIObjectImageAttributeBase.CreateImageLoader(mInfo) ?? EmptyUIObjectImageLoader.Instance;
            ServiceUIObject uiObject = new ServiceUIObject(info, imageLoader);
            ServiceObjectGroup group = new ServiceObjectGroup(info);
            group.SetUIObject(uiObject);
            return group;
        }

        #region Class EmptyServiceObject ...

        class EmptyServiceObject : IServiceObject
        {
            public ServiceObjectGroup[] GetGroups()
            {
                return new ServiceObjectGroup[0];
            }

            public ServiceObjectAction[] GetActions(ServiceObjectActionType actionType)
            {
                return new ServiceObjectAction[0];
            }

            public object DoAction(string actionName, IDictionary<string, object> parameters)
            {
                return null;
            }

            public ServiceObjectProperty[] GetProperties()
            {
                return new ServiceObjectProperty[0];
            }

            public object GetPropertyValue(string propertyName)
            {
                return null;
            }

            public ServiceObjectInfo Info
            {
                get { return ServiceObjectInfo.Empty; }
            }

            private IDictionary<object, object> _items;

            public IDictionary<object, object> Items
            {
                get { return _items ?? (_items = new Dictionary<object, object>()); }
            }

            public static readonly EmptyServiceObject Empty = new EmptyServiceObject();
        }

        #endregion

        #region Class ServiceObjectCollection ...

        class ServiceObjectCollection : MarshalByRefObjectEx, IServiceObject
        {
            public ServiceObjectCollection(IServiceObject[] serviceObjects, ServiceObjectInfo info)
            {
                _serviceObjects = serviceObjects;
                _info = info ?? ServiceObjectInfo.Empty;
                _wrapper = new Lazy<Wrapper>(_Load);
            }

            private readonly IServiceObject[] _serviceObjects;
            private readonly ServiceObjectInfo _info;

            public ServiceObjectGroup[] GetGroups()
            {
                return _serviceObjects.SelectMany(so => so.GetGroups()).ToArray();
            }

            public ServiceObjectAction[] GetActions(ServiceObjectActionType actionType)
            {
                return _serviceObjects.SelectMany(so => so.GetActions(actionType)).ToArray();
            }

            class Wrapper
            {
                public Dictionary<string, IServiceObject[]> ActionDict;
                public Dictionary<string, IServiceObject[]> PropertyDict;
            }

            private Wrapper _Load()
            {
                return new Wrapper() {
                    ActionDict = _serviceObjects.SelectMany(
                        so => so.GetActions(ServiceObjectActionType.All).Select(action => new { So = so, ActionName = action.Info.Name })
                    ).GroupBy(item => item.ActionName).ToDictionary(g => g.Key, g => g.ToArray(item => item.So)),
                    PropertyDict = _serviceObjects.SelectMany(
                        so => so.GetProperties().Select(property => new { So = so, PropertyName = property.Info.Name })
                    ).GroupBy(item => item.PropertyName).ToDictionary(g => g.Key, g => g.ToArray(item => item.So))
                };
            }

            private readonly Lazy<Wrapper> _wrapper;

            public object DoAction(string actionName, IDictionary<string, object> parameters)
            {
                Contract.Requires(actionName != null);
                IServiceObject[] sos = _wrapper.Value.ActionDict.GetOrDefault(actionName);
                if (sos == null)
                    return null;

                return sos[0].DoAction(actionName, parameters);
            }

            public ServiceObjectProperty[] GetProperties()
            {
                return _serviceObjects.SelectMany(so => so.GetProperties()).ToArray();
            }

            public object GetPropertyValue(string propertyName)
            {
                Contract.Requires(propertyName != null);
                IServiceObject[] sos = _wrapper.Value.ActionDict.GetOrDefault(propertyName);
                if (sos == null)
                    return null;

                for (int k = 0; k < sos.Length; k++)
                {
                    object value = sos[k].GetPropertyValue(propertyName);
                    if (value != null)
                        return value;
                }

                return null;
            }

            public ServiceObjectInfo Info
            {
                get { return _info; }
            }

            public IDictionary<object, object> Items
            {
                get
                {
                    return _dict ?? (_dict = new ThreadSafeDictionaryWrapper<object, object>());
                }
            }

            private ThreadSafeDictionaryWrapper<object, object> _dict;
        }

        #endregion

        public static IServiceObject Combine(IServiceObject[] serviceObjects, ServiceObjectInfo info)
        {
            Contract.Requires(serviceObjects != null);
            if (serviceObjects.Length == 0)
                return EmptyServiceObject.Empty;

            return new ServiceObjectCollection(serviceObjects, info);
        }

        public static readonly ServiceObject Empty = ServiceObject.FromObject(new object());
    }
}
