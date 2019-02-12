using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts.Service
{
    /// <summary>
    /// ServiceObject的指令执行器
    /// </summary>
    public interface IServiceObjectExecutor
    {
        /// <summary>
        /// 执行指定的指令
        /// </summary>
        /// <param name="action"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        object DoAction(string action, IDictionary<string, object> parameters);

        /// <summary>
        /// 获取指定的属性
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        object GetProperty(string property);
    }

    /// <summary>
    /// 空的指令执行器
    /// </summary>
    public class EmptyServiceObjectExecutor : IServiceObjectExecutor
    {
        public object DoAction(string action, IDictionary<string, object> parameters)
        {
            throw new NotSupportedException("不支持指令" + action);
        }

        public object GetProperty(string property)
        {
            throw new NotSupportedException("不支持属性" + property);
        }
    }
}
