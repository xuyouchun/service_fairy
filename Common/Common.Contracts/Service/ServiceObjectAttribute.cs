using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Utility;
using System.Reflection;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 用于标识服务对象
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public abstract class ServiceObjectInfoAttributeBase : Attribute
    {
        /// <summary>
        /// 获取描述信息
        /// </summary>
        /// <param name="attrProvider"></param>
        /// <returns></returns>
        public abstract ServiceObjectInfo GetServiceObjectInfo(ICustomAttributeProvider attrProvider);

        public static ServiceObjectInfo GetInfo(ICustomAttributeProvider attrProvider)
        {
            Contract.Requires(attrProvider != null);

            ServiceObjectInfoAttributeBase attr = attrProvider.GetAttribute<ServiceObjectInfoAttributeBase>(true);
            if (attr != null)
            {
                return attr.GetServiceObjectInfo(attrProvider);
            }
            else
            {
                return new ServiceObjectInfo(GetNameSpaceFromType(attrProvider), attrProvider.GetName(), "");
            }
        }

        protected static string GetNameSpaceFromType(ICustomAttributeProvider attrProvider)
        {
            Type declaringType = attrProvider.GetDeclaringType();
            return (declaringType == null) ? "" : (declaringType.FullName + "," + declaringType.Assembly.FullName);
        }

        protected static string GetName(ICustomAttributeProvider attrProvider)
        {
            return ReflectionUtility.GetName(attrProvider);
        }
    }

    /// <summary>
    /// 用于标识服务对象
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class ServiceObjectInfoAttribute : ServiceObjectInfoAttributeBase
    {
        public ServiceObjectInfoAttribute(string @namespace, string name, string title, string desc = "", string help = "", int weight = 0)
        {
            Contract.Requires(name != null);

            _info = new ServiceObjectInfo(@namespace, name, title, desc, help, weight);
        }

        private readonly ServiceObjectInfo _info;

        public override ServiceObjectInfo GetServiceObjectInfo(ICustomAttributeProvider attrProvider)
        {
            return _info;
        }
    }

    /// <summary>
    /// 用于指定分组
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ServiceObjectGroupAttribute : Attribute
    {
        public ServiceObjectGroupAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Name { get; private set; }

        public static string GetGroup(MemberInfo memberInfo)
        {
            Contract.Requires(memberInfo != null);

            ServiceObjectGroupAttribute attr = memberInfo.GetAttribute<ServiceObjectGroupAttribute>(true);
            return (attr != null) ? attr.Name : string.Empty;
        }
    }

    /// <summary>
    /// 描述分组信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class ServiceObjectGroupDescAttribute : Attribute
    {

    }

    /// <summary>
    /// 标识指令
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ServiceObjectActionAttribute : Attribute
    {
        public ServiceObjectActionAttribute(ServiceObjectActionType actionType = ServiceObjectActionType.Normal)
        {
            ActionType = actionType;
        }

        /// <summary>
        /// 是否为默认指令
        /// </summary>
        public ServiceObjectActionType ActionType { get; private set; }
    }

    /// <summary>
    /// 标识指令
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ServiceObjectPropertyAttribute : Attribute
    {
        public ServiceObjectPropertyAttribute()
        {
            
        }
    }
}
