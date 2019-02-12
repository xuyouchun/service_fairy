using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts.Service
{
    /// <summary>
    /// Service Object提供策略
    /// </summary>
    public interface IServiceObjectProvider
    {
        /// <summary>
        /// 获取Service Object树
        /// </summary>
        /// <returns></returns>
        IServiceObjectTree GetTree();
    }
}
