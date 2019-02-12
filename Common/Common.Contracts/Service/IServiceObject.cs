using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics.Contracts;
using Common.Collection;

namespace Common.Contracts.Service
{
    /// <summary>
    /// ServiceObject实体
    /// </summary>
    public interface IServiceObjectEntity
    {
        /// <summary>
        /// 描述信息
        /// </summary>
        ServiceObjectInfo Info { get; }

        /// <summary>
        /// 键值对集合
        /// </summary>
        IDictionary<object, object> Items { get; }
    }

    /// <summary>
    /// 用于获取ServiceObjectInfo
    /// </summary>
    public interface IServiceObjectInfoProvider
    {
        ServiceObjectInfo GetServiceObjectInfo();
    }

    /// <summary>
    /// ServiceObject的描述信息
    /// </summary>
    [Serializable]
    public class ServiceObjectInfo
    {
        public ServiceObjectInfo(string @namespace, string name, string title = "", string desc = "", string help = "", int weight = 0)
        {
            Namespace = @namespace ?? string.Empty;
            Name = name ?? string.Empty;
            Title = title;
            Desc = desc;
            Help = help;
            Weight = weight;
        }

        /// <summary>
        /// 命名空间
        /// </summary>
        public string Namespace { get; private set; }

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

        /// <summary>
        /// 帮助
        /// </summary>
        public string Help { get; private set; }

        /// <summary>
        /// 权重
        /// </summary>
        public int Weight { get; private set; }

        public override string ToString()
        {
            return string.Join(", ", new string[] { Namespace, Name, Title, Desc, Help }.Where(s => !string.IsNullOrEmpty(s)));
        }

        public override int GetHashCode()
        {
            return Namespace.GetHashCode() ^ Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(ServiceObjectInfo))
                return false;

            ServiceObjectInfo info = (ServiceObjectInfo)obj;
            return info.Namespace == Namespace && info.Name == Name;
        }

        public static bool operator ==(ServiceObjectInfo info1, ServiceObjectInfo info2)
        {
            return object.Equals(info1, info2);
        }

        public static bool operator !=(ServiceObjectInfo info1, ServiceObjectInfo info2)
        {
            return !object.Equals(info1, info2);
        }

        public static readonly ServiceObjectInfo Empty = new ServiceObjectInfo("", "", "", "");

        public static ServiceObjectInfo Combine(ServiceObjectInfo info1, ServiceObjectInfo info2)
        {
            if (info1 == null || info2 == null)
                return info1 ?? info2;

            return new ServiceObjectInfo(
                _PickNotEmpty(info1.Namespace, info2.Namespace),
                _PickNotEmpty(info1.Name, info2.Name), _PickNotEmpty(info1.Title, info2.Title),
                _PickNotEmpty(info1.Desc, info2.Desc), _PickNotEmpty(info1.Help, info2.Help), info1.Weight == default(int) ? info2.Weight : info1.Weight
            );
        }

        public static ServiceObjectInfo Combine(params ServiceObjectInfo[] infos)
        {
            if (infos.Length == 0)
                return ServiceObjectInfo.Empty;

            ServiceObjectInfo info = infos[0];
            for (int k = 1; k < infos.Length; k++)
            {
                info = Combine(info, infos[k]);
            }

            return info;
        }

        private static string _PickNotEmpty(params string[] ss)
        {
            for (int k = 0, length = ss.Length; k < length; k++)
            {
                string s = ss[k];
                if (!string.IsNullOrEmpty(s))
                    return s;
            }

            return string.Empty;
        }

        public static ServiceObjectInfo OfTitle(string title)
        {
            return new ServiceObjectInfo(null, null, title);
        }

        public static ServiceObjectInfo OfName(string name)
        {
            return new ServiceObjectInfo(null, name);
        }
    }

    /// <summary>
    /// 具有交互功能的对象
    /// </summary>
    public interface IServiceObject : IServiceObjectEntity
    {
        /// <summary>
        /// 获取所有的组
        /// </summary>
        /// <returns></returns>
        ServiceObjectGroup[] GetGroups();

        /// <summary>
        /// 获取所有的指令
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        ServiceObjectAction[] GetActions(ServiceObjectActionType actionType);

        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="actionName"></param>
        object DoAction(string actionName, IDictionary<string, object> parameters);

        /// <summary>
        /// 获取所有的属性
        /// </summary>
        /// <returns></returns>
        ServiceObjectProperty[] GetProperties();

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        object GetPropertyValue(string propertyName);
    }

    /// <summary>
    /// 指令类型
    /// </summary>
    [Flags]
    public enum ServiceObjectActionType
    {
        /// <summary>
        /// 正常指令
        /// </summary>
        Normal = 0x01,

        /// <summary>
        /// 默认指令
        /// </summary>
        Default = 0x02,

        /// <summary>
        /// 特殊的默认指令
        /// </summary>
        AttachDefault = 0x04,

        /// <summary>
        /// 显示属性表
        /// </summary>
        ShowProperty = 0x08,

        /// <summary>
        /// 打开
        /// </summary>
        Open = 0x10,

        /// <summary>
        /// 在新窗口中打开
        /// </summary>
        OpenInNewWindow = 0x20,

        /// <summary>
        /// 刷新
        /// </summary>
        Refresh = 0x40,

        /// <summary>
        /// 关闭
        /// </summary>
        Close = 0x80,

        /// <summary>
        /// 新建
        /// </summary>
        New = 0x0100,

        /// <summary>
        /// 重新启动
        /// </summary>
        Restart = 0x0200,

        /// <summary>
        /// 保存
        /// </summary>
        Save = 0x0400,

        /// <summary>
        /// 另存为
        /// </summary>
        SaveAs = 0x0800,

        /// <summary>
        /// 取消
        /// </summary>
        Cancel = 0x1000,

        /// <summary>
        /// 所有指令
        /// </summary>
        All = -1,
    }

    /// <summary>
    /// 指令组
    /// </summary>
    public class ServiceObjectGroup : MarshalByRefObjectEx, IServiceObjectEntity
    {
        public ServiceObjectGroup(ServiceObjectInfo info)
        {
            Contract.Requires(info != null);

            Info = info;
            Items = new ThreadSafeDictionaryWrapper<object, object>();
        }

        /// <summary>
        /// 描述信息
        /// </summary>
        public ServiceObjectInfo Info { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(ServiceObjectGroup))
                return false;

            return object.Equals(Info, ((ServiceObjectGroup)obj).Info);
        }

        public override int GetHashCode()
        {
            return Info.GetHashCode();
        }

        public static bool operator ==(ServiceObjectGroup group1, ServiceObjectGroup group2)
        {
            return object.Equals(group1, group2);
        }

        public static bool operator !=(ServiceObjectGroup group1, ServiceObjectGroup group2)
        {
            return !object.Equals(group1, group2);
        }


        public IDictionary<object, object> Items { get; private set; }
    }

    /// <summary>
    /// 参数描述
    /// </summary>
    [Serializable]
    public class ServiceObjectParameter : IServiceObjectEntity
    {
        public ServiceObjectParameter(ServiceObjectInfo info, Type type, object defaultValue = null)
        {
            Contract.Requires(info != null);

            Info = info;
            DefaultValue = defaultValue;
            Items = new ThreadSafeDictionaryWrapper<object, object>();
        }

        /// <summary>
        /// 描述信息
        /// </summary>
        public ServiceObjectInfo Info { get; private set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(ServiceObjectParameter))
                return false;

            return object.Equals(Info, ((ServiceObjectParameter)obj).Info);
        }

        public override int GetHashCode()
        {
            return Info.GetHashCode();
        }

        public static bool operator ==(ServiceObjectParameter parameter1, ServiceObjectParameter parameter2)
        {
            return object.Equals(parameter1, parameter2);
        }

        public static bool operator !=(ServiceObjectParameter parameter1, ServiceObjectParameter parameter2)
        {
            return !object.Equals(parameter1, parameter2);
        }

        public static ServiceObjectParameter From(ParameterInfo pInfo)
        {
            Contract.Requires(pInfo != null);

            ServiceObjectInfo info = new ServiceObjectInfo(pInfo.Name, pInfo.Name, "", "");
            return new ServiceObjectParameter(info, pInfo.ParameterType, pInfo.DefaultValue);
        }

        public IDictionary<object, object> Items { get; private set; }
    }

    /// <summary>
    /// 指令
    /// </summary>
    [Serializable]
    public class ServiceObjectAction : IServiceObjectEntity
    {
        public ServiceObjectAction(ServiceObjectInfo info,
            ServiceObjectParameter[] inputParameters, ServiceObjectParameter returnParameter, string group, ServiceObjectActionType actionType)
        {
            Contract.Requires(info != null);

            Info = info;
            Group = group ?? string.Empty;
            InputParameters = inputParameters ?? new ServiceObjectParameter[0];
            ReturnParameter = returnParameter;
            Items = new ThreadSafeDictionaryWrapper<object, object>();
            ActionType = actionType;
        }

        /// <summary>
        /// 描述信息
        /// </summary>
        public ServiceObjectInfo Info { get; private set; }

        /// <summary>
        /// 组
        /// </summary>
        public string Group { get; private set; }

        /// <summary>
        /// 指令的类型
        /// </summary>
        public ServiceObjectActionType ActionType { get; private set; }

        /// <summary>
        /// 输入参数
        /// </summary>
        public ServiceObjectParameter[] InputParameters { get; private set; }

        /// <summary>
        /// 输出参数
        /// </summary>
        public ServiceObjectParameter ReturnParameter { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(ServiceObjectAction))
                return false;

            return object.Equals(Info, ((ServiceObjectAction)obj).Info);
        }

        public override int GetHashCode()
        {
            return Info.GetHashCode();
        }

        public static bool operator ==(ServiceObjectAction action1, ServiceObjectAction action2)
        {
            return object.Equals(action1, action2);
        }

        public static bool operator !=(ServiceObjectAction action1, ServiceObjectAction action2)
        {
            return !object.Equals(action1, action2);
        }

        public IDictionary<object, object> Items { get; private set; }
    }

    /// <summary>
    /// 属性
    /// </summary>
    [Serializable]
    public class ServiceObjectProperty : IServiceObjectEntity
    {
        public ServiceObjectProperty(ServiceObjectInfo info, ServiceObjectParameter parameter, string group)
        {
            Contract.Requires(info != null);

            Info = info;
            Parameter = parameter;
            Group = group ?? string.Empty;
            Items = new ThreadSafeDictionaryWrapper<object, object>();
        }

        /// <summary>
        /// 描述信息
        /// </summary>
        public ServiceObjectInfo Info { get; private set; }

        /// <summary>
        /// 组
        /// </summary>
        public string Group { get; private set; }

        /// <summary>
        /// 属性返回参数描述
        /// </summary>
        public ServiceObjectParameter Parameter { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(ServiceObjectProperty))
                return false;

            return object.Equals(Info, ((ServiceObjectProperty)obj).Info);
        }

        public override int GetHashCode()
        {
            return Info.GetHashCode();
        }

        public static bool operator ==(ServiceObjectProperty property1, ServiceObjectProperty property2)
        {
            return object.Equals(property1, property2);
        }

        public static bool operator !=(ServiceObjectProperty property1, ServiceObjectProperty property2)
        {
            return !object.Equals(property1, property2);
        }

        public IDictionary<object, object> Items { get; private set; }
    }
}
