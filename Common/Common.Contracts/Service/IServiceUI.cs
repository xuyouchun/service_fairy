using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 与可管理服务对象的交互
    /// </summary>
    public interface IServiceUI
    {
        /// <summary>
        /// 获取ServiceObjectProvider
        /// </summary>
        /// <returns></returns>
        IServiceObjectTree GetTree();
    }
}
