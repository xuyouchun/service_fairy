using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace Common.Contracts
{
    /// <summary>
    /// 支持属性获取的对象
    /// </summary>
    public interface IObjectPropertyProvider
    {
        /// <summary>
        /// 获取全部的属性
        /// </summary>
        /// <returns></returns>
        ObjectProperty[] GetAllProperties();

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        ObjectPropertyValue GetPropertyValue(string propertyName);

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="value"></param>
        void SetPropertyValue(ObjectPropertyValue value);
    }

    /// <summary>
    /// 属性
    /// </summary>
    [Serializable, DataContract]
    public class ObjectProperty
    {
        public ObjectProperty(string name, string title, ObjectPropertyType type, string desc = "")
        {
            Contract.Requires(name != null);

            Name = name;
            Title = title ?? string.Empty;
            Type = type;
            Desc = desc ?? string.Empty;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; private set; }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataMember]
        public string Desc { get; private set; }

        /// <summary>
        /// 类型
        /// </summary>
        [DataMember]
        public ObjectPropertyType Type { get; private set; }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj==null || obj.GetType() != typeof(ObjectProperty))
                return false;

            ObjectProperty op = (ObjectProperty)obj;
            return Name == op.Name;
        }

        public static bool operator ==(ObjectProperty obj1, ObjectProperty obj2)
        {
            return object.Equals(obj1, obj2);
        }

        public static bool operator !=(ObjectProperty obj1, ObjectProperty obj2)
        {
            return !object.Equals(obj1, obj2);
        }

        public ObjectPropertyValue CreateValue(object value)
        {
            return new ObjectPropertyValue(Name, value);
        }
    }

    /// <summary>
    /// 用于标注对象的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ObjectPropertyAttribute : Attribute
    {
        public ObjectPropertyAttribute(string name, string title, string desc)
        {
            Name = name;
            Title = title;
            Desc = desc;
            Browsable = true;
        }

        public ObjectPropertyAttribute(string title)
            : this(null, title, null)
        {

        }

        public ObjectPropertyAttribute(string title, string desc)
            : this(null, title, desc)
        {

        }

        public ObjectPropertyAttribute(bool browsable)
        {
            Browsable = browsable;
        }

        /// <summary>
        /// 是否可被浏览访问
        /// </summary>
        public bool Browsable { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; private set; }
    }

    /// <summary>
    /// 属性值
    /// </summary>
    [Serializable, DataContract]
    public class ObjectPropertyValue
    {
        public ObjectPropertyValue(string name, object value)
        {
            Contract.Requires(name != null);

            Name = name;
            StringValue = value.ToStringIgnoreNull(null);
            _rawValue = value;
        }

        /// <summary>
        /// 属性名称
        /// </summary>
        [DataMember]
        public string Name { get; private set; }

        /// <summary>
        /// 值
        /// </summary>
        [DataMember]
        public string StringValue { get; private set; }

        /// <summary>
        /// 真实的值（带有类型）
        /// </summary>
        [IgnoreDataMember]
        public object RawValue { get { return _rawValue; } }

        [IgnoreDataMember, NonSerialized]
        private object _rawValue;

        public override int GetHashCode()
        {
            return StringValue == null ? 0 : StringValue.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(ObjectPropertyValue))
                return false;

            ObjectPropertyValue v = (ObjectPropertyValue)obj;
            return object.Equals(StringValue, v.StringValue);
        }

        public static bool operator ==(ObjectPropertyValue obj1, ObjectPropertyValue obj2)
        {
            return object.Equals(obj1, obj2);
        }

        public static bool operator !=(ObjectPropertyValue obj1, ObjectPropertyValue obj2)
        {
            return !object.Equals(obj1, obj2);
        }

        public override string ToString()
        {
            return ReflectionUtility.GetObjectString(RawValue);
        }
    }

    /// <summary>
    /// 对象属性的类型
    /// </summary>
    public enum ObjectPropertyType
    {
        /// <summary>
        /// 字段
        /// </summary>
        [Desc("字段")]
        ObjectField,

        /// <summary>
        /// 静态字段
        /// </summary>
        [Desc("静态字段")]
        StaticObjectField,

        /// <summary>
        /// 属性
        /// </summary>
        [Desc("属性")]
        ObjectProperty,

        /// <summary>
        /// 静态属性
        /// </summary>
        [Desc("静态属性")]
        StaticObjectProperty,

        /// <summary>
        /// 自定义
        /// </summary>
        [Desc("自定义")]
        UserDefined,
    }

}
