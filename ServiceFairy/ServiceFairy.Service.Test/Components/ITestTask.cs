using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Test;

namespace ServiceFairy.Service.Test.Components
{
    /// <summary>
    /// 测试任务
    /// </summary>
    interface ITestTask
    {
        /// <summary>
        /// 验证参数的有效性
        /// </summary>
        /// <param name="args"></param>
        void Validate(string args);

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="args"></param>
        void Start(string args);

        /// <summary>
        /// 停止
        /// </summary>
        void Stop();

        /// <summary>
        /// 是否正在运行
        /// </summary>
        bool Running { get; }
    }

    /// <summary>
    /// 测试任务信息
    /// </summary>
    class TestTaskInfo
    {
        /// <summary>
        /// 测试任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 测试任务标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 测试任务参数
        /// </summary>
        public string Args { get; set; }
    }

    /// <summary>
    /// 用于标注测试任务
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TestTaskAttribute : Attribute
    {
        public TestTaskAttribute(string name, string desc, string argsFormat)
        {
            Info = new TestTaskTypeInfo() { Name = name, Desc = desc, ArgsFormat = argsFormat };
        }

        public TestTaskTypeInfo Info { get; private set; }
    }
}
