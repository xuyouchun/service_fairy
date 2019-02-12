using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 组件
    /// </summary>
    public interface IAppComponent : IObjectPropertyProvider, IDisposable
    {
        /// <summary>
        /// 所有者
        /// </summary>
        object Owner { get; }

        /// <summary>
        /// 设置/获取运行状态
        /// </summary>
        bool Running { get; set; }

        /// <summary>
        /// 获取组件详细信息
        /// </summary>
        /// <returns></returns>
        AppComponentInfo GetInfo();
    }

    /// <summary>
    /// 组件控制器
    /// </summary>
    public interface IAppComponentController : IDisposable
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context"></param>
        void Init(IAppComponentControllerContext context);

        /// <summary>
        /// 应用
        /// </summary>
        void Apply();

        /// <summary>
        /// 取消
        /// </summary>
        void Abolish();
    }

    /// <summary>
    /// 组件控制器的上下文执行环境
    /// </summary>
    public interface IAppComponentControllerContext
    {
        /// <summary>
        /// 可供使用的服务集合
        /// </summary>
        IServiceProvider ServiceProvider { get; }
    }

    /// <summary>
    /// 组件状态控制
    /// </summary>
    public interface IAppComponentStatusFunction
    {
        /// <summary>
        /// 状态
        /// </summary>
        AppComponentStatus Status { get; set; }

        /// <summary>
        /// 状态变化事件
        /// </summary>
        event EventHandler<AppComponentStatusChangedEventArgs> StatusChanged;
    }

    /// <summary>
    /// 组件状态
    /// </summary>
    public enum AppComponentStatus
    {
        /// <summary>
        /// 禁用
        /// </summary>
        [Desc("禁用")]
        Disable,

        /// <summary>
        /// 启用
        /// </summary>
        [Desc("启用")]
        Enable,
    }

    /// <summary>
    /// 组件状态变化
    /// </summary>
    public class AppComponentStatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="status"></param>
        public AppComponentStatusChangedEventArgs(AppComponentStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// 组件状态
        /// </summary>
        public AppComponentStatus Status { get; private set; }
    }

    /// <summary>
    /// 组件任务控制服务
    /// </summary>
    public interface IAppComponentTaskFunction
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="taskName"></param>
        void Execute(string taskName);
    }

    /// <summary>
    /// 用于标识组件
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AppComponentAttribute : Attribute
    {
        public AppComponentAttribute(string title = "", string desc = "", AppComponentCategory category = AppComponentCategory.Application, string name = "")
        {
            Title = title ?? string.Empty;
            Desc = desc ?? string.Empty;
            Category = category;
            Name = name;
        }

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
        /// 组件类别
        /// </summary>
        public AppComponentCategory Category { get; private set; }
    }

    /// <summary>
    /// 组件类型
    /// </summary>
    public enum AppComponentCategory
    {
        /// <summary>
        /// 应用组件
        /// </summary>
        [Desc("应用组件")]
        Application,

        /// <summary>
        /// 系统组件
        /// </summary>
        [Desc("系统组件")]
        System,
    }
}
