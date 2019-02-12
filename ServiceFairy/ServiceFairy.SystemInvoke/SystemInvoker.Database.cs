using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Common.Package.Service;
using ServiceFairy.Entities.Database;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Data.UnionTable;

namespace ServiceFairy.SystemInvoke
{
    partial class SystemInvoker
    {
        private DatabaseInvoker _database;

        /// <summary>
        /// Database Service
        /// </summary>
        public DatabaseInvoker Database
        {
            get { return _database ?? (_database = new DatabaseInvoker(this)); }
        }

        /// <summary>
        /// Database Service
        /// </summary>
        public class DatabaseInvoker : Invoker
        {
            public DatabaseInvoker(SystemInvoker owner)
                : base(owner)
            {
                
            }

            /// <summary>
            /// 选取数据
            /// </summary>
            /// <param name="tableName">表名</param>
            /// <param name="routeKeys">路由键</param>
            /// <param name="param">查询参数</param>
            /// <param name="columns">列名</param>
            /// <param name="utSettings">数据库调用设置</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<DataList> SelectSr(string tableName, object[] routeKeys, DbSearchParam param,
                string[] columns, UtInvokeSettings utSettings = null, CallingSettings settings = null)
            {
                var sr = DatabaseService.Select(Sc, new Database_Select_Request() {
                    TableName = tableName, Param = param, RouteKeys = routeKeys, Columns = columns, Settings = utSettings
                }, settings);

                return CreateSr(sr, r => r.Data);
            }

            /// <summary>
            /// 选取数据
            /// </summary>
            /// <param name="tableName">表名</param>
            /// <param name="routeKeys">路由键</param>
            /// <param name="param">查询参数</param>
            /// <param name="columns">列名</param>
            /// <param name="utSettings">数据库调用设置</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public DataList Select(string tableName, object[] routeKeys, DbSearchParam param, string[] columns,
                UtInvokeSettings utSettings = null, CallingSettings settings = null)
            {
                return InvokeWithCheck(SelectSr(tableName, routeKeys, param, columns, utSettings, settings));
            }

            /// <summary>
            /// 插入数据
            /// </summary>
            /// <param name="tableName">表名</param>
            /// <param name="data">数据</param>
            /// <param name="utSettings">数据库调用设置</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<int> InsertSr(string tableName, DataList data, bool autoUpdate = false,
                UtInvokeSettings utSettings = null, CallingSettings settings = null)
            {
                var sr = DatabaseService.Insert(Sc, new Database_Insert_Request() {
                    TableName = tableName, Data = data, AutoUpdate = autoUpdate, Settings = utSettings
                }, settings);

                // 填入新生成的主键
                int pkIndex;
                if (sr != null && sr.Result != null && !sr.Result.NewPrimaryKeys.IsNullOrEmpty()
                    && (pkIndex = data.GetPrimaryKeyColumnIndex()) >= 0)
                {
                    object[] newPrimaryKeys = sr.Result.NewPrimaryKeys;
                    int index = 0;

                    for (int k = 0; k < data.Rows.Length; k++)
                    {
                        DataListRow row = data.Rows[k];
                        if (row.Cells[pkIndex] == null)
                        {
                            if (index >= newPrimaryKeys.Length)
                                throw new ServiceException(ServerErrorCode.ReturnValueError, "新生成的主键数量与预期不符");

                            row.Cells[pkIndex] = newPrimaryKeys[index++];
                        }
                    }

                    if(index!=newPrimaryKeys.Length)
                        throw new ServiceException(ServerErrorCode.ReturnValueError, "新生成的主键数量与预期不符");
                }

                return CreateSr(sr, r => r.EffectRowCount);
            }

            /// <summary>
            /// 插入数据
            /// </summary>
            /// <param name="tableName">表名</param>
            /// <param name="data">数据</param>
            /// <param name="autoUpdate">当数据已经存在时，是否自动更新</param>
            /// <param name="utSettings">数据库调用设置</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public int Insert(string tableName, DataList data, bool autoUpdate = false, UtInvokeSettings utSettings = null, 
                CallingSettings settings = null)
            {
                return InvokeWithCheck(InsertSr(tableName, data, autoUpdate, utSettings, settings));
            }

            /// <summary>
            /// 更新数据
            /// </summary>
            /// <param name="tableName">表名</param>
            /// <param name="data">数据</param>
            /// <param name="routeKeys">路由键</param>
            /// <param name="where">更新条件</param>
            /// <param name="utSettings">数据库调用设置</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<int> UpdateSr(string tableName, DataList data, object[] routeKeys, string where,
                UtInvokeSettings utSettings = null, CallingSettings settings = null)
            {
                var sr = DatabaseService.Update(Sc, new Database_Update_Request() {
                    TableName = tableName, Data = data, RouteKeys = routeKeys, Where = where, Settings = utSettings,
                }, settings);

                return CreateSr(sr, r => r.EffectRowCount);
            }

            /// <summary>
            /// 更新数据
            /// </summary>
            /// <param name="tableName">表名</param>
            /// <param name="data">数据</param>
            /// <param name="routeKeys">路由键</param>
            /// <param name="where">更新条件</param>
            /// <param name="utSettings">数据库调用设置</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public int Update(string tableName, DataList data, object[] routeKeys, string where,
                UtInvokeSettings utSettings = null, CallingSettings settings = null)
            {
                return InvokeWithCheck(UpdateSr(tableName, data, routeKeys, where, utSettings, settings));
            }

            /// <summary>
            /// 删除数据
            /// </summary>
            /// <param name="tableName">表名</param>
            /// <param name="data">数据</param>
            /// <param name="where">删除条件</param>
            /// <param name="utSettings">数据库调用设置</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<int> DeleteSr(string tableName, DataList data, string where,
                UtInvokeSettings utSettings = null, CallingSettings settings = null)
            {
                var sr = DatabaseService.Delete(Sc,
                    new Database_Delete_Request { TableName = tableName, Data = data, Where = where, Settings = utSettings },
                    settings
                );

                return CreateSr(sr, r => r.EffectRowCount);
            }

            /// <summary>
            /// 删除数据
            /// </summary>
            /// <param name="tableName">表名</param>
            /// <param name="data">数据</param>
            /// <param name="where">删除条件</param>
            /// <param name="utSettings">数据库调用设置</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public int Delete(string tableName, DataList data, string where, UtInvokeSettings utSettings = null, CallingSettings settings = null)
            {
                return InvokeWithCheck(DeleteSr(tableName, data, where, utSettings, settings));
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
            /// <param name="utSettings">数据库调用设置</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<int> MergeSr(string tableName, object routeKey, DataList data, string[] compareColumns, string where,
                UtConnectionMergeOption option = UtConnectionMergeOption.InsertUpdate, UtInvokeSettings utSettings = null, CallingSettings settings = null)
            {
                var sr = DatabaseService.Merge(Sc, new Database_Merge_Request {
                    TableName = tableName, RouteKey = routeKey, Data = data,
                    Option = option, CompareColumns = compareColumns, Where = where, Settings = utSettings,
                }, settings);

                return CreateSr(sr, r => r.EffectRowCount);
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
            /// <param name="utSettings">数据库调用设置</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public int Merge(string tableName, object routeKey, DataList data, string[] compareColumns, string where,
                UtConnectionMergeOption option = UtConnectionMergeOption.InsertUpdate, UtInvokeSettings utSettings = null, CallingSettings settings = null)
            {
                return InvokeWithCheck(MergeSr(tableName, routeKey, data, compareColumns, where, option, utSettings, settings));
            }

            /// <summary>
            /// 获取表信息
            /// </summary>
            /// <param name="tableNames">表名，若为空引用则获取全部的表信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<TableInfo[]> GetTableInfosSr(string[] tableNames, CallingSettings settings = null)
            {
                var sr = DatabaseService.GetTableInfos(Sc,
                    new Database_GetTableInfos_Request() { TableNames = tableNames }, settings);

                return CreateSr(sr, r => r.TableInfos);
            }

            /// <summary>
            /// 获取表信息
            /// </summary>
            /// <param name="tableNames">表名，若为空引用则获取全部的表信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public TableInfo[] GetTableInfos(string[] tableNames, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetTableInfosSr(tableNames, settings));
            }

            /// <summary>
            /// 获取表信息
            /// </summary>
            /// <param name="tableName">表名</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<TableInfo> GetTableInfoSr(string tableName, CallingSettings settings = null)
            {
                Contract.Requires(tableName != null);

                var sr = GetTableInfosSr(new[] { tableName }, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取表信息
            /// </summary>
            /// <param name="tableName">表名</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public TableInfo GetTableInfo(string tableName, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetTableInfoSr(tableName, settings));
            }

            /// <summary>
            /// 获取所有表信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<TableInfo[]> GetAllTableInfosSr(CallingSettings settings = null)
            {
                return GetTableInfosSr(null, settings);
            }

            /// <summary>
            /// 获取所有表信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public TableInfo[] GetAllTableInfos(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetAllTableInfosSr(settings));
            }

            /// <summary>
            /// 初始化表的元数据
            /// </summary>
            /// <param name="reviseInfos">修正信息</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult InitMetadataSr(UtTableGroupReviseInfo[] reviseInfos, CallingSettings settings = null)
            {
                return DatabaseService.InitMetadata(Sc,
                    new Database_InitMetadata_Request() { ReviseInfos = reviseInfos }, settings);
            }

            /// <summary>
            /// 初始化表的元数据
            /// </summary>
            /// <param name="reviseInfos">修正信息</param>
            /// <param name="settings">调用设置</param>
            public void InitMetadata(UtTableGroupReviseInfo[] reviseInfos, CallingSettings settings = null)
            {
                InvokeWithCheck(InitMetadataSr(reviseInfos, settings));
            }

            /// <summary>
            /// 执行SQL语句
            /// </summary>
            /// <param name="sql">SQL语句</param>
            /// <param name="parameters">参数</param>
            /// <param name="utSettings">数据库调用设置</param>
            /// <param name="settings">调用设置</param>
            /// <returns>数据</returns>
            public ServiceResult<DataList> ExecuteSqlSr(string sql, object[] parameters, UtInvokeSettings utSettings = null, CallingSettings settings = null)
            {
                var request = new Database_ExecuteSql_Request { Sql = sql, Parameters = parameters, Settings = utSettings };
                var sr = DatabaseService.ExecuteSql(Sc, request, settings);
                return CreateSr(sr, r => r.Data);
            }

            /// <summary>
            /// 执行SQL语句
            /// </summary>
            /// <param name="sql">SQL语句</param>
            /// <param name="parameters">参数</param>
            /// <param name="utSettings">数据库调用设置</param>
            /// <param name="settings">调用设置</param>
            /// <returns>数据</returns>
            public DataList ExecuteSql(string sql, object[] parameters, UtInvokeSettings utSettings = null, CallingSettings settings = null)
            {
                return InvokeWithCheck(ExecuteSqlSr(sql, parameters, utSettings, settings));
            }

            /// <summary>
            /// 执行SQL语句
            /// </summary>
            /// <param name="sql">SQL语句</param>
            /// <param name="utSettings">数据库调用设置</param>
            /// <param name="settings">调用设置</param>
            /// <returns>数据</returns>
            public ServiceResult<DataList> ExecuteSqlSr(string sql, UtInvokeSettings utSettings = null, CallingSettings settings = null)
            {
                return ExecuteSqlSr(sql, (object[])null, utSettings, settings);
            }

            /// <summary>
            /// 执行SQL语句
            /// </summary>
            /// <param name="sql">SQL语句</param>
            /// <param name="utSettings">数据库调用设置</param>
            /// <param name="settings">调用设置</param>
            /// <returns>数据</returns>
            public DataList ExecuteSql(string sql, UtInvokeSettings utSettings = null, CallingSettings settings = null)
            {
                return InvokeWithCheck(ExecuteSqlSr(sql, utSettings, settings));
            }
        }
    }
}
