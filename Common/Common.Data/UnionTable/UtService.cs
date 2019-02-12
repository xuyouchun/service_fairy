using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Data.UnionTable
{
    /// <summary>
    /// UnionTable的数据服务
    /// </summary>
    public class UtService
    {
        public UtService()
        {

        }

        /// <summary>
        /// 连接到数据库
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="conType"></param>
        /// <returns></returns>
        public UtDatabase Connect(string conStr, string conType = null)
        {
            Contract.Requires(conStr != null);

            IUtProvider provider = TableMetaDataProviderFactory.Create(conStr, conType);
            return Connect(provider);
        }

        /// <summary>
        /// 连接到数据库
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public UtDatabase Connect(IUtProvider provider)
        {
            Contract.Requires(provider != null);

            return new UtDatabase(provider);
        }
    }
}
