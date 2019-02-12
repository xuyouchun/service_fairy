using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Data.UnionTable;
using Common.Contracts;
using ServiceFairy.Entities.Database;
using Common.Data;
using System.Diagnostics.Contracts;
using Common.Utility;
using Common.Package;

namespace ServiceFairy.Service.Database.Components
{
    /// <summary>
    /// 数据库连接管理器
    /// </summary>
    [AppComponent("数据库连接管理器", "维护数据库的连接")]
    class DbManagerAppComponent : TimerAppComponentBase
    {
        public DbManagerAppComponent(Service service)
            : base(service, TimeSpan.FromMinutes(5))
        {
            _service = service;

            _utService = new UtService();
            _connectionString = new AutoLoad<string>(_LoadConnectionString, TimeSpan.FromSeconds(10));
        }

        private readonly Service _service;

        private readonly UtService _utService;
        private UtDatabase _utDatabase;
        private string _lastConnectionString;
        private readonly object _syncLocker = new object();

        private UtDatabase _LoadUtDatabase()
        {
            string conStr = ConnectionString;
            if (string.IsNullOrEmpty(conStr))
                throw new ServiceException(ServerErrorCode.ConfigurationError, "未配置元数据的数据库连接");

            if (_utDatabase == null || _lastConnectionString != conStr)  // 如果是初次加载，或者是数据库连接字符串发生变化
            {
                lock (_syncLocker)
                {
                    if (_utDatabase == null || _lastConnectionString != conStr)
                    {
                        UtDatabase utDatabase = _utService.Connect(conStr);
                        utDatabase.InitMetaDataSchame();

                        _utDatabase = utDatabase;
                        _lastConnectionString = conStr;
                    }
                }
            }

            return _utDatabase;
        }

        /// <summary>
        /// UtDatabase
        /// </summary>
        public UtDatabase UtDatabase
        {
            get { return _LoadUtDatabase(); }
        }

        private readonly AutoLoad<string> _connectionString;

        private string _LoadConnectionString()
        {
            return _service.Invoker.DatabaseCenter.GetConStr();
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        [ObjectProperty("数据库连接字符串")]
        private string ConnectionString
        {
            get
            {
                return _connectionString.Value;
            }
        }

        /// <summary>
        /// 创建表的连接
        /// </summary>
        /// <param name="table">表名</param>
        /// <returns>表连接</returns>
        public IUtConnection CreateUtConnection(string table)
        {
            _EnsureInit();

            UtDatabase utDb = _service.DbManager.UtDatabase;
            if (utDb == null)
                return EmptyUtConnection.Instance;

            IUtConnection utCon = utDb.CreateUtConnection(table);
            if (utCon == null)
                throw Utility.CreateBusinessException(DatabaseStatusCode.TableNotExist, string.Format("表“{0}”不存在", table));

            return utCon;
        }

        private void _EnsureInit()
        {
            ValidateAvaliableState();
        }

        /// <summary>
        /// 获取所有表信息
        /// </summary>
        /// <returns>表信息</returns>
        public TableInfo[] GetTableInfos()
        {
            return UtDatabase.GetTableInfos();
        }

        /// <summary>
        /// 获取指定名称的表信息
        /// </summary>
        /// <param name="table">表名</param>
        /// <returns>表信息</returns>
        public TableInfo GetTableInfo(string table)
        {
            return UtDatabase.GetTableInfo(table);
        }

        private readonly object _metadataLocker = new object();
        private readonly HashSet<int> _reviseHistoryHashCodes = new HashSet<int>();

        /// <summary>
        /// 初始化表的元数据
        /// </summary>
        /// <param name="reviseInfos"></param>
        public void InitMetadata(UtTableGroupReviseInfo[] reviseInfos)
        {
            Contract.Requires(reviseInfos != null);

            int hashCode = CommonUtility.GetHashCode(reviseInfos);
            if (_reviseHistoryHashCodes.Contains(hashCode))
                return;

            lock (_metadataLocker)
            {
                if (!_reviseHistoryHashCodes.Contains(hashCode))
                {
                    UtDatabase.InitMetaData(reviseInfos);
                    _reviseHistoryHashCodes.Add(hashCode);
                }
            }
        }

        protected override void OnExecuteTask(string taskName)
        {
            lock (_metadataLocker)
            {
                _reviseHistoryHashCodes.Clear();
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
        }
    }
}
