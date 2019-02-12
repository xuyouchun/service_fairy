using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceFairy.Service.Master.Components
{
    partial class AppClientManager
    {
        /// <summary>
        /// 用于批量重启终端的执行器
        /// </summary>
        class RestartExecutor : IDisposable
        {
            public RestartExecutor(Guid[] clientIds)
            {
                _clientIds = clientIds;
            }

            private readonly Guid[] _clientIds;

            public void Execute(Action callback)
            {
                callback();
            }

            public void Dispose()
            {
                
            }
        }
    }
}
