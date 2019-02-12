using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Log;
using System.Threading;

namespace Common.Contracts.Service
{
    /// <summary>
    /// Application的基类
    /// </summary>
    public abstract class ApplicationBase : IDisposable
    {
        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="callback">回调</param>
        /// <param name="waitHandle">等待信号</param>
        public abstract void Run(Action<string, string[]> callback, WaitHandle waitHandle);

        /// <summary>
        /// 停止
        /// </summary>
        public virtual void Dispose()
        {

        }

        protected virtual ILogWriter<LogItem> CreateLogWriter()
        {
            return EmptyLogWriter<LogItem>.Instance;
        }

        protected virtual ILogReader<LogItem> CreateLogReader()
        {
            return EmptyLogReader<LogItem>.Instance;
        }
    }
}
