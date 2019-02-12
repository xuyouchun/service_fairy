using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using Common.Package;
using ServiceFairy.Entities.Test;

namespace ServiceFairy.Service.Test.Components
{
    /// <summary>
    /// 测试任务的工厂
    /// </summary>
    static class TestTaskFactory
    {
        static TestTaskFactory()
        {
            var list = typeof(TestTaskFactory).Assembly.SearchTypes<TestTaskTypeInfo, TestTaskAttribute>((attrs, t) => attrs[0].Info);
            _taskWrappers = list.ToDictionary(item => item.Key.Name, item => new Wrapper { Type = item.Value, Info = item.Key });
        }

        /// <summary>
        /// 创建测试任务
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ITestTask CreateTestTask(string name)
        {
            ITestTask task;
            Wrapper wrapper;

            if (!_taskWrappers.TryGetValue(name, out wrapper) || (task = ObjectFactory.CreateObject(wrapper.Type) as ITestTask) == null)
                throw new NotSupportedException("不支持该测试任务");

            return task;
        }

        /// <summary>
        /// 获取所有测试任务的类型信息
        /// </summary>
        /// <returns></returns>
        public static TestTaskTypeInfo[] GetAllInfos()
        {
            return _taskWrappers.Values.ToArray(v => v.Info);
        }

        class Wrapper
        {
            public Type Type;
            public TestTaskTypeInfo Info;
        }

        private static readonly Dictionary<string, Wrapper> _taskWrappers;
    }
}
