using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts
{
    /// <summary>
    /// 配置接口
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <returns></returns>
        string Get(string name);

        /// <summary>
        /// 获取全部的键名
        /// </summary>
        /// <returns></returns>
        string[] GetAllKeys();
    }
}
