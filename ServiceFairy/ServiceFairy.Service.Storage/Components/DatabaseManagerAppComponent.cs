using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Data.UnionTable;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;
using Common.Data.UnionTable.MsSql;
using ServiceFairy.Entities.Storage;
using Common;
using Common.Utility;
using Common.Contracts;
using Common.Data;
using Common.Data.SqlExpressions;

namespace ServiceFairy.Service.Storage.Components
{
    /// <summary>
    /// 数据库访问管理器
    /// </summary>
    [AppComponent("数据库访问管理器", "执行对数据库的增删改查操作")]
    class DatabaseManagerAppComponent : AppComponent
    {
        public DatabaseManagerAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;
        private DbService _dbService;
        private UnionTableDbAccess _dbAccess;

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        /// <param name="transcation"></param>
        public void Insert(string tableName, DataCollection data, IUnionTableTranscation transcation = null)
        {
            ValidateAvaliableState();
            Contract.Requires(tableName != null && data != null);

            _dbAccess.Insert(tableName, data, transcation);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="routeKeys"></param>
        /// <param name="fields"></param>
        /// <param name="parameter"></param>
        /// <param name="transcation"></param>
        public DataCollection Select(string tableName, object[] routeKeys, string[] fields, 
            UnionTableSearchParameter parameter, IUnionTableTranscation transcation = null)
        {
            ValidateAvaliableState();
            Contract.Requires(tableName != null && routeKeys != null);

            return _dbAccess.Select(tableName, routeKeys, fields, parameter, transcation);
        }

        /// <summary>
        /// 按组查询数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="groups">组名</param>
        /// <param name="where">过滤条件</param>
        /// <param name="transcation">事务</param>
        /// <returns></returns>
        public DataCollection SelectGroup(string tableName, object[] routeKeys, string[] groups, SqlExpression where, IUnionTableTranscation transcation = null)
        {
            ValidateAvaliableState();
            Contract.Requires(tableName != null && routeKeys != null && groups != null);

            return _dbAccess.SelectGroup(tableName, routeKeys, groups, where, transcation);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        /// <param name="transcation"></param>
        public void Update(string tableName, DataCollection data, IUnionTableTranscation transcation = null)
        {
            ValidateAvaliableState();
            Contract.Requires(tableName != null && data != null);

            _dbAccess.Update(tableName, data, transcation);
        }

        /// <summary>
        /// 按条件更新数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        /// <param name="where"></param>
        /// <param name="transcation"></param>
        public void UpdateWhere(string tableName, DataCollection data, SqlExpression where, IUnionTableTranscation transcation = null)
        {
            ValidateAvaliableState();
            Contract.Requires(tableName != null && data != null);

            _dbAccess.Update(tableName, data, where, transcation);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="routeKeys"></param>
        /// <param name="where"></param>
        /// <param name="transcation"></param>
        public void Delete(string tableName, object[] routeKeys, SqlExpression where, IUnionTableTranscation transcation = null)
        {
            ValidateAvaliableState();
            Contract.Requires(tableName != null && routeKeys != null);

            _dbAccess.Delete(tableName, routeKeys, where, transcation);
        }

        /// <summary>
        /// 搜索主键值
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="parameter">参数</param>
        /// <param name="fields">字段</param>
        /// <param name="transcation">事务</param>
        public UnionTableSearchResult SearchPrimaryKeys(string tableName, string[] fields, UnionTableSearchParameter parameter, IUnionTableTranscation transcation = null)
        {
            ValidateAvaliableState();
            Contract.Requires(tableName != null && parameter != null);

            return _dbAccess.SearchImage(tableName, fields, parameter, transcation);
        }

        /// <summary>
        /// 搜索数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="parameter">搜索参数</param>
        /// <param name="totalCount">总数</param>
        /// <param name="transcation">事务</param>
        /// <param name="fields">字段</param>
        /// <returns></returns>
        public DataCollection Search(string tableName, UnionTableSearchParameter parameter, string[] fields, out int totalCount, IUnionTableTranscation transcation = null)
        {
            ValidateAvaliableState();
            Contract.Requires(tableName != null && parameter != null);

            return _dbAccess.Search(tableName, fields, out totalCount, parameter, transcation);
        }

        /// <summary>
        /// 按组搜索数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="parameter">搜索参数</param>
        /// <param name="groups">组</param>
        /// <param name="totalCount">总数</param>
        /// <param name="transcation">事务</param>
        /// <returns></returns>
        public DataCollection SearchGroup(string tableName, UnionTableSearchParameter parameter, string[] groups, out int totalCount, IUnionTableTranscation transcation = null)
        {
            ValidateAvaliableState();
            Contract.Requires(tableName != null && parameter != null);

            return _dbAccess.SearchGroup(tableName, groups, out totalCount, parameter, transcation);
        }

        /// <summary>
        /// 搜索全部字段的数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="parameter">搜索参数</param>
        /// <param name="totalCount">总数</param>
        /// <param name="transcation">事务</param>
        /// <returns></returns>
        public DataCollection SearchAllFields(string tableName, UnionTableSearchParameter parameter, out int totalCount, IUnionTableTranscation transcation = null)
        {
            ValidateAvaliableState();
            Contract.Requires(tableName != null && parameter != null);

            return _dbAccess.SearchAllFields(tableName, out totalCount, parameter, transcation);
        }

        [ObjectProperty("数据库连接字符串")]
        private string ConnectionString
        {
            get
            {
                return _service.Context.ConfigReader.Get(CONNECTION_STRING_KEY);
            }
        }

        const string CONNECTION_STRING_KEY = "connectionString";

        protected override void OnStart()
        {
            base.OnStart();

            string conStr = ConnectionString;
            if (string.IsNullOrEmpty(conStr))
            {
                SetAvaliableState(new ServiceResult(ServiceStatusCode.ConfigurationError, 0, "未配置元数据的数据库连接：" + CONNECTION_STRING_KEY));
            }
            else
            {
                _dbService = new DbService(new SqlMetaDataProvider(conStr));
                _dbAccess = new UnionTableDbAccess(_dbService);
            }
        }

        /// <summary>
        /// 获取表的信息
        /// </summary>
        /// <param name="tableNames"></param>
        /// <param name="includeDetail"></param>
        /// <returns></returns>
        public DbTableInfo[] GetTableInfos(string[] tableNames, bool includeDetail = true)
        {
            Contract.Requires(tableNames != null);

            if (tableNames.Length == 0)
                return Array<DbTableInfo>.Empty;

            return tableNames.Distinct().Select(tableName => GetTableInfo(tableName)).WhereNotNull().ToArray();
        }

        /// <summary>
        /// 获取表信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="includeDetail"></param>
        /// <returns></returns>
        public DbTableInfo GetTableInfo(string tableName, bool includeDetail = true)
        {
            TableInfo info = _dbService.GetTableInfo(tableName);

            return new DbTableInfo() {
                TableName = tableName,
                TableInfo = includeDetail ? info : null,
                FieldGroupCount = info.AllGroups.Length,
                FieldCount = info.FieldInfos.Length,
            };
        }

        /// <summary>
        /// 获取所有表的信息
        /// </summary>
        /// <param name="includeDetail"></param>
        /// <returns></returns>
        public DbTableInfo[] GetAllTableInfos(bool includeDetail = true)
        {
            ValidateAvaliableState();

            return GetTableInfos(_dbAccess.GetTableNames());
        }

        /// <summary>
        /// 按列组查询数据库
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="routeKeys">路由键</param>
        /// <param name="groups">组名</param>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public DataCollection SelectGroup(string tableName, object[] routeKeys, string[] groups, SqlExpression where)
        {
            Contract.Requires(tableName != null && routeKeys != null && groups != null);

            return _dbAccess.SelectGroup(tableName, routeKeys, groups, where);
        }

        /// <summary>
        /// 查询数据库表的所有字段
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="keys">主键</param>
        /// <returns></returns>
        public DataCollection SelectAllFields(string tableName, object routeKey, SqlExpression where)
        {
            Contract.Requires(tableName != null && routeKey != null);

            return _dbAccess.SelectAllFields(tableName, routeKey, where);
        }

        /// <summary>
        /// 数据库合并
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="routeKey">路由键</param>
        /// <param name="data">数据</param>
        /// <param name="compareFields">用作比较的字段</param>
        /// <param name="mergeType">合并方式</param>
        /// <param name="fields">合并字段</param>
        public void Merge(string tableName, object routeKey, DataCollection data, string[] compareFields, string[] fields, TableConnectionMergeType mergeType)
        {
            Contract.Requires(routeKey != null && data != null && fields != null);

            _dbAccess.Merge(tableName, routeKey, data, compareFields, mergeType);
        }

        protected override void OnDispose()
        {
            base.OnDispose();

        }
    }
}
