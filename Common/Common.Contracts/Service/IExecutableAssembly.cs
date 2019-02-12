using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 可执行的程序集
    /// </summary>
    public interface IExecutableAssembly
    {
        /// <summary>
        /// 执行该程序集
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        /// <param name="waitHandle"></param>
        /// <returns></returns>
        object Execute(object context, Action<string, string[]> callback, WaitHandle waitHandle);
    }
}
