using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using Common.Utility;

namespace Common.Contracts.Service
{
    /// <summary>
    /// AppService的方法信息
    /// </summary>
    [Serializable, DataContract]
    public class AppCommandInfo
    {
        public AppCommandInfo(CommandDesc commandDesc, AppParameter inputParameter = null, AppParameter outputParameter = null, string title = "", string desc = "",
            AppCommandCategory category = AppCommandCategory.Application, SecurityLevel securityLevel = SecurityLevel.Undefined, 
            UsableType usable = UsableType.Normal, string usableDesc = "")
        {
            Contract.Requires(commandDesc != null);

            CommandDesc = commandDesc;
            Desc = desc ?? string.Empty;
            Title = title ?? string.Empty;
            InputParameter = inputParameter ?? AppParameter.Empty;
            OutputParameter = outputParameter ?? AppParameter.Empty;
            Category = category;
            SecurityLevel = securityLevel;
            Usable = usable;
            UsableDesc = usableDesc;
        }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public CommandDesc CommandDesc { get; private set; }

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
        /// 输入参数
        /// </summary>
        [DataMember]
        public AppParameter InputParameter { get; private set; }

        /// <summary>
        /// 输出参数
        /// </summary>
        [DataMember]
        public AppParameter OutputParameter { get; private set; }

        /// <summary>
        /// 接口类型
        /// </summary>
        [DataMember]
        public AppCommandCategory Category { get; private set; }

        /// <summary>
        /// 权限级别
        /// </summary>
        [DataMember]
        public SecurityLevel SecurityLevel { get; private set; }

        /// <summary>
        /// 可用状态
        /// </summary>
        [DataMember]
        public UsableType Usable { get; private set; }
        
        /// <summary>
        /// 可用状态描述
        /// </summary>
        [DataMember]
        public string UsableDesc { get; private set; }

        public override string ToString()
        {
            return CommandDesc.ToString();
        }

        /// <summary>
        /// 从原型创建
        /// </summary>
        /// <param name="info"></param>
        /// <param name="inputParameter"></param>
        /// <param name="outputParameter"></param>
        /// <returns></returns>
        public static AppCommandInfo FromPrototype(AppCommandInfo info, AppParameter inputParameter, AppParameter outputParameter)
        {
            Contract.Requires(info != null);

            return new AppCommandInfo(info.CommandDesc,
                title: info.Title, desc: info.Desc, category: info.Category,
                inputParameter: inputParameter == null ? inputParameter : inputParameter,
                outputParameter: outputParameter == null ? outputParameter : outputParameter,
                securityLevel: info.SecurityLevel, usable: info.Usable, usableDesc: info.UsableDesc
            );
        }

        /// <summary>
        /// 从原型创建
        /// </summary>
        /// <param name="info"></param>
        /// <param name="inputType"></param>
        /// <param name="outputType"></param>
        /// <returns></returns>
        public static AppCommandInfo FromPrototype(AppCommandInfo info, Type inputType = null, Type outputType = null)
        {
            Contract.Requires(info != null);

            return FromPrototype(info,
                inputType != null ? new AppParameter(inputType) : info.InputParameter,
                outputType != null ? new AppParameter(outputType) : info.OutputParameter
            );
        }
    }

    /// <summary>
    /// 参数信息
    /// </summary>
    [Serializable, DataContract]
    public class AppParameter
    {
        public AppParameter(Type type = null, string name = null)
        {
            _parameterType = type;
            _typeString = (type == null) ? null : type.Name;

            Name = name ?? (type == null ? null : type.Name);
        }

        private readonly string _typeString;

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; private set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        [IgnoreDataMember]
        public Type ParameterType { get { return _parameterType; } }

        [IgnoreDataMember, NonSerialized]
        private readonly Type _parameterType;

        /// <summary>
        /// 空的参数信息
        /// </summary>
        public static readonly AppParameter Empty = new AppParameter(null);

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsEmpty { get { return string.IsNullOrEmpty(_typeString); } }

        public override string ToString()
        {
            if (IsEmpty)
                return "(空)";

            return _typeString;
        }
    }
}
