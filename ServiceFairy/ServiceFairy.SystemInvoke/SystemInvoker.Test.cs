using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Entities.Test;

namespace ServiceFairy.SystemInvoke
{
	partial class SystemInvoker
	{
        private TestInvoker _test;

        /// <summary>
        /// Test Service
        /// </summary>
        public TestInvoker Test
        {
            get { return _test ?? (_test = new TestInvoker(this)); }
        }

        public class TestInvoker : Invoker
        {
            public TestInvoker(SystemInvoker owner)
                : base(owner)
            {
                
            }

            /// <summary>
            /// 启动测试
            /// </summary>
            /// <param name="name"></param>
            /// <param name="args"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<Guid> StartTestSr(string name, string args, CallingSettings settings = null)
            {
                var sr = TestService.StartTest(Sc, new Test_StartTest_Request() {
                    Name = name, Args = args
                }, settings);

                return CreateSr(sr, r => r.TaskId);
            }

            /// <summary>
            /// 启动测试
            /// </summary>
            /// <param name="name"></param>
            /// <param name="args"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public Guid StartTest(string name, string args, CallingSettings settings = null)
            {
                return InvokeWithCheck(StartTestSr(name, args, settings));
            }
        }
	}
}
