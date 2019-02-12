using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Data;
using Common.Data.UnionTable;
using Common.Contracts;
using ServiceFairy.Entities.Database;

namespace ServiceFairy.Service.Database.Components
{
    /// <summary>
    /// 数据库查询执行器
    /// </summary>
    [AppComponent("数据库查询执行器", "执行数据库的查询操作")]
    class DbQuerierAppComponent : AppComponent
    {
        public DbQuerierAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;
        
        /// <summary>
        /// 创建表的连接
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private IUtConnection _CreateUtConnection(string tableName)
        {
            return _service.DbManager.CreateUtConnection(tableName);
        }

        /// <summary>
        /// 选取数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="param">查询参数</param>
        /// <param name="columns">列</param>
        /// <param name="settings">调用设置</param>
        /// <returns></returns>
        public DataList Select(string tableName, object[] routeKeys, DbSearchParam param, string[] columns, UtInvokeSettings settings = null)
        {
            return _CreateUtConnection(tableName).Select(routeKeys, columns, param, settings);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="data">数据</param>
        /// <param name="autoUpdate">当数据已经存在时，是否自动更新</param>
        /// <param name="settings">数据库调用设置</param>
        /// <returns></returns>
        public int Insert(string tableName, DataList data, bool autoUpdate, UtInvokeSettings settings = null)
        {
            return _CreateUtConnection(tableName).Insert(data, autoUpdate, settings);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="data">数据</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="where">条件</param>
        /// <param name="settings">数据库调用设置</param>
        /// <returns></returns>
        public int Update(string tableName, DataList data, object[] routeKeys, string where, UtInvokeSettings settings = null)
        {
            return _CreateUtConnection(tableName).Update(data, routeKeys, where, settings);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="data">数据</param>
        /// <param name="where">条件</param>
        /// <param name="settings">数据库调用设置</param>
        /// <returns></returns>
        public int Delete(string tableName, DataList data, string where, UtInvokeSettings settings = null)
        {
            return _CreateUtConnection(tableName).Delete(data, where, settings);
        }

        /// <summary>
        /// 合并数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="data">需要合并的数据</param>
        /// <param name="compareColumns">用于比较的列</param>
        /// <param name="where">过滤条件</param>
        /// <param name="option">合并选项</param>
        /// <param name="settings">数据库调用设置</param>
        /// <returns></returns>
        public int Merge(string tableName, object routeKey, DataList data, string[] compareColumns,
            string where, UtConnectionMergeOption option, UtInvokeSettings settings = null)
        {
            return _CreateUtConnection(tableName).Merge(routeKey, data, compareColumns, where, option, settings);
        }
    }
}
