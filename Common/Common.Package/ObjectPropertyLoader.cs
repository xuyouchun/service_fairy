using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Utility;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Diagnostics.Contracts;

namespace Common.Package
{
    /// <summary>
    /// 加载对象的属性
    /// </summary>
    public class ObjectPropertyLoader : IObjectPropertyProvider
    {
        public ObjectPropertyLoader(object obj)
        {
            Contract.Requires(obj != null);

            _obj = obj;
            _propertiesWrapper = new Lazy<PropertyWrapper>(_LoadWrapper);
        }

        private readonly object _obj;
        private readonly Lazy<PropertyWrapper> _propertiesWrapper;

        class PropertyWrapper
        {
            public ObjectProperty[] Properties;
            public Dictionary<string, ObjectProperty> Dict;
        }

        /// <summary>
        /// 获取所有属性
        /// </summary>
        /// <returns></returns>
        public virtual ObjectProperty[] GetAllProperties()
        {
            return _propertiesWrapper.Value.Properties;
        }

        private PropertyWrapper _LoadWrapper()
        {
            ObjectProperty[] properties = _LoadProperties();
            return new PropertyWrapper() {
                Properties = properties,
                Dict = properties.ToDictionary(p => p.Name, true)
            };
        }

        const BindingFlags _bindingFlags = (BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

        private ObjectProperty[] _LoadProperties()
        {
            Type t = _obj.GetType();

            List<ObjectProperty> list = new List<ObjectProperty>();

            // 所有字段及属性
            foreach (MemberInfo mInfo in t.GetMembers(_bindingFlags))
            {
                ObjectPropertyAttribute attr = mInfo.GetAttribute<ObjectPropertyAttribute>();
                if (attr != null && !attr.Browsable || mInfo.IsDefined(typeof(CompilerGeneratedAttribute), false))
                    continue;

                PropertyInfo pInfo = mInfo as PropertyInfo;
                FieldInfo fInfo;

                string name = (attr != null) ? (attr.Name ?? mInfo.Name) : mInfo.Name;
                string title = (attr != null) ? attr.Title : string.Empty;
                string desc = (attr != null) ? attr.Desc : string.Empty;
                if (pInfo != null)
                {
                    if (!pInfo.CanRead)
                        continue;

                    list.Add(new ObjectProperty(name, title, pInfo.GetGetMethod(true).IsStatic ?
                        ObjectPropertyType.StaticObjectProperty : ObjectPropertyType.ObjectProperty, desc)
                    );
                }
                else if ((fInfo = mInfo as FieldInfo) != null)
                {
                    list.Add(new ObjectProperty(name, title, fInfo.IsStatic ?
                        ObjectPropertyType.StaticObjectField : ObjectPropertyType.ObjectField, desc)
                    );
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// 获取指定名称的属性值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public virtual ObjectPropertyValue GetPropertyValue(string propertyName)
        {
            Contract.Requires(propertyName != null);

            ObjectProperty property = _propertiesWrapper.Value.Dict.GetOrDefault(propertyName);
            if (property == null)
                return null;

            Type t = _obj.GetType();
            object value;

            switch (property.Type)
            {
                case ObjectPropertyType.ObjectProperty:
                case ObjectPropertyType.StaticObjectProperty:
                    if (!_GetValue(t.GetProperty(propertyName, _bindingFlags), out value))
                        return null;

                    break;

                case ObjectPropertyType.ObjectField:
                case ObjectPropertyType.StaticObjectField:
                    if (!_GetValue(t.GetField(propertyName, _bindingFlags), out value))
                        return null;

                    break;

                default:
                    return null;
            }

            return property.CreateValue(value);
        }

        private bool _GetValue(FieldInfo fInfo, out object value)
        {
            if (fInfo == null)
                goto _error;

            if (fInfo.IsStatic)
                value = fInfo.GetValue(null);
            else
                value = fInfo.GetValue(_obj);

            return true;

        _error:
            value = null;
            return false;
        }

        private bool _GetValue(PropertyInfo pInfo, out object value)
        {
            if (pInfo == null || !pInfo.CanRead)
                goto _error;

            MethodInfo mInfo = pInfo.GetGetMethod(true);
            if (mInfo.IsStatic)
                value = mInfo.Invoke(null, null);
            else
                value = mInfo.Invoke(_obj, null);

            return true;

        _error:
            value = null;
            return false;
        }

        /// <summary>
        /// 设置属性的值
        /// </summary>
        /// <param name="value"></param>
        public void SetPropertyValue(ObjectPropertyValue value)
        {
            
        }
    }
}
