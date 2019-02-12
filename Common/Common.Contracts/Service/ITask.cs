using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 任务
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        void Execute();
    }
}
