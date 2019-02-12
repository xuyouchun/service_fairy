using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Test;

namespace ServiceFairy.Service.Test.Components.TestTasks
{
    [TestTask("CacheTest", "分布式缓存测试", "/thread_count:线程数 /key_length:键最大长度 /value_length:值最大长度")]
    class CacheTestTask : TestTaskBase
    {
        protected override void OnStart(TaskArgsParser args)
        {
            
        }

        protected override void OnStop()
        {
            throw new NotImplementedException();
        }
    }
}
