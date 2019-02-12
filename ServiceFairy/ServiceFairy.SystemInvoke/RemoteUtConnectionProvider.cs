//#define NODB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data.UnionTable;
using Common.Data;
using System.Diagnostics.Contracts;
using Common.Package;
using Common.Utility;
using Common.Contracts.Service;

namespace ServiceFairy.SystemInvoke
{
    /// <summary>
    /// 远程的数据库连接
    /// </summary>
    public class RemoteUtConnectionProvider : IUtConnectionProvider, IDisposable
    {
        public RemoteUtConnectionProvider(SystemInvoker invoker, UtTableGroupReviseInfo[] reviseInfos = null)
        {
            Contract.Requires(invoker != null);

            _invoker = invoker;
            _tableInfos = new AutoLoad<IDictionary<string, TableInfo>>(_LoadAllTableInfos, TimeSpan.FromSeconds(60));
            _reviseInfos = reviseInfos;

            if (_reviseInfos != null)
                _taskHandle = GlobalTimer<ITask>.Default.Add(TimeSpan.FromMinutes(30), TimeSpan.FromSeconds(60), _InitMetaData, false);
        }

        private readonly SystemInvoker _invoker;
        private readonly AutoLoad<IDictionary<string, TableInfo>> _tableInfos;
        private readonly IGlobalTimerTaskHandle _taskHandle;
        private readonly UtTableGroupReviseInfo[] _reviseInfos;

        private IDictionary<string, TableInfo> _LoadAllTableInfos()
        {
            TableInfo[] infos = _invoker.Database.GetAllTableInfos();
            return infos.ToIgnoreCaseDictionary(info => info.Group + "." + info.Name);
        }

        private TableInfo _GetTableInfo(string tableName)
        {
            return _tableInfos.Value.GetOrDefault(tableName);
        }

        /// <summary>
        /// 创建表连接
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public IUtConnection CreateUtConnection(string tableName)
        {
            Contract.Requires(tableName != null);

            TableInfo info = _GetTableInfo(tableName);
            if (info == null)
                return null;

            return new RemoteUtConnection(_invoker, info);
        }

        private void _InitMetaData()
        {
#if !NODB

            if (_reviseInfos == null)
                return;

            try
            {
                OnInitMetaData(_reviseInfos);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }
#endif
        }

        protected virtual void OnInitMetaData(UtTableGroupReviseInfo[] reviseInfos)
        {
            _invoker.Database.InitMetadata(reviseInfos, CallingSettings.OneWay);
        }

        #region Class RemoteUtConnection ...

        class RemoteUtConnection : IUtConnection
        {
            public RemoteUtConnection(SystemInvoker invoker, TableInfo tableInfo)
            {
                _invoker = invoker;
                _tableInfo = tableInfo;
                _fullTableName = _tableInfo.Group + "." + _tableInfo.Name;
            }

            private readonly SystemInvoker _invoker;
            private readonly TableInfo _tableInfo;
            private readonly string _fullTableName;

            public DataList Select(object[] routeKeys, string[] columns, DbSearchParam param, UtInvokeSettings settings)
            {
                return _invoker.Database.Select(_fullTableName, routeKeys, param, columns, settings);
            }

            public int Insert(DataList data, bool autoUpdate, UtInvokeSettings settings)
            {
                return _invoker.Database.Insert(_fullTableName, data, autoUpdate, settings);
            }

            public int Update(DataList data, object[] routeKeys, string where, UtInvokeSettings settings)
            {
                return _invoker.Database.Update(_fullTableName, data, routeKeys, where, settings);
            }

            public int Delete(DataList data, string where, UtInvokeSettings settings)
            {
                return _invoker.Database.Delete(_fullTableName, data, where, settings);
            }

            public int Merge(object routeKey, DataList data, string[] compareColumns, string where, UtConnectionMergeOption option, UtInvokeSettings settings)
            {
                return _invoker.Database.Merge(_fullTableName, routeKey, data, compareColumns, where, option, settings);
            }
        }

        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            if (_taskHandle != null)
                _taskHandle.Dispose();
        }

        ~RemoteUtConnectionProvider()
        {
            Dispose();
        }
    }
}
