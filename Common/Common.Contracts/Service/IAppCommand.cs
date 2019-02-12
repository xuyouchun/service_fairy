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
    /// 服务的方法
    /// </summary>
    public interface IAppCommand : IObjectPropertyProvider, IDisposable
    {
        /// <summary>
        /// 获取该方法的信息
        /// </summary>
        /// <returns></returns>
        AppCommandInfo GetInfo();

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        OutputAppCommandArg Execute(AppCommandExecuteContext context, InputAppCommandArg arg);
    }

    /// <summary>
    /// 用于标注AppCommand
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AppCommandAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="version">版本号</param>
        /// <param name="inputType">输入参数类型</param>
        /// <param name="outputType">输出参数类型</param>
        /// <param name="title">标题</param>
        /// <param name="desc">描述</param>
        /// <param name="category">接口类型</param>
        /// <param name="securityLevel">安全级别</param>
        public AppCommandAttribute(string name, string version, Type inputType, Type outputType,
            string title = "", string desc = "",
            AppCommandCategory category = AppCommandCategory.Application,
            SecurityLevel securityLevel = SecurityLevel.Undefined)
        {
            Name = name;
            Version = SVersion.Parse(version ?? "1.0");
            InputType = inputType;
            OutputType = outputType;
            Title = title ?? "";
            Desc = desc ?? "";
            Category = category;
            SecurityLevel = securityLevel;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">名称</param>
        public AppCommandAttribute(string name)
            : this(name, "1.0", null, null, "", "", AppCommandCategory.Application)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="title">标题</param>
        public AppCommandAttribute(string name, string title)
            : this(name, "1.0", null, null, title, "", AppCommandCategory.Application)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="title">标题</param>
        /// <param name="category">类别</param>
        public AppCommandAttribute(string name, string title, AppCommandCategory category)
            : this(name, "1.0", null, null, title, "", category)
        {

        }

        /// <summary>
        /// Contractor
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="title">标题</param>
        /// <param name="desc">描述</param>
        /// <param name="commandType">接口类型</param>
        public AppCommandAttribute(string name, string title, string desc, AppCommandCategory commandType = AppCommandCategory.Application)
            : this(name, "1.0", null, null, title, desc, commandType)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="inputType">输入类型</param>
        /// <param name="outputType">输出类型</param>
        /// <param name="title">标题</param>
        /// <param name="desc">描述</param>
        /// <param name="commandType">接口类型</param>
        public AppCommandAttribute(string name, Type inputType, Type outputType, string title = "", string desc = "",
            AppCommandCategory commandType = AppCommandCategory.Application)
            : this(name, "1.0", inputType, outputType, title, desc, commandType)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="inputType">输入参数</param>
        /// <param name="title">标题</param>
        /// <param name="desc">描述</param>
        /// <param name="commandType">接口类型</param>
        public AppCommandAttribute(string name, Type inputType, string title = "", string desc = "",
            AppCommandCategory commandType = AppCommandCategory.Application)
            : this(name, "1.0", inputType, null, title, desc, commandType)
        {

        }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public SVersion Version { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 输入参数类型
        /// </summary>
        public Type InputType { get; set; }

        /// <summary>
        /// 输出参数类型
        /// </summary>
        public Type OutputType { get; set; }

        /// <summary>
        /// 接口类型
        /// </summary>
        public AppCommandCategory Category { get; set; }

        /// <summary>
        /// 安全级别
        /// </summary>
        public SecurityLevel SecurityLevel { get; set; }

        /// <summary>
        /// 获取指定类型的AppCommandInfo
        /// </summary>
        /// <param name="mInfo"></param>
        /// <returns></returns>
        public static AppCommandInfo GetAppCommandInfo(MemberInfo mInfo)
        {
            Contract.Requires(mInfo != null);

            AppCommandAttribute attr = mInfo.GetAttribute<AppCommandAttribute>(true);
            UsableAttribute usableAttr = mInfo.GetAttribute<UsableAttribute>(true);

            return attr == null ? null : new AppCommandInfo(
                new CommandDesc(attr.Name, attr.Version),
                inputParameter: new AppParameter(attr.InputType),
                outputParameter: new AppParameter(attr.OutputType),
                title: attr.Title, desc: attr.Desc, category: attr.Category, securityLevel: attr.SecurityLevel,
                usable: (usableAttr == null) ? UsableType.Normal : usableAttr.Usable,
                usableDesc: (usableAttr == null) ? "" : usableAttr.Message);
        }

        public override string ToString()
        {
            return Version.IsEmpty ? Name : (Name + " " + Version);
        }
    }

    /// <summary>
    /// AppCommand的类型
    /// </summary>
    public enum AppCommandCategory
    {
        /// <summary>
        /// 应用接口
        /// </summary>
        [Desc("应用接口")]
        Application,

        /// <summary>
        /// 系统接口
        /// </summary>
        [Desc("系统接口")]
        System,
    }
}
