using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data.UnionTable
{
    /// <summary>
    /// 与数据库结构相关的操作
    /// </summary>
    public interface IUtSchema
    {
        /// <summary>
        /// 批量生成新的主键
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        object[] GenerateNewKeys(int count);
    }
}
