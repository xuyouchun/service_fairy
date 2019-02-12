using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Package;
using Common.Data.UnionTable;
using Common.Data;

namespace ServiceFairy.Service.DatabaseCenter.Components
{
    /// <summary>
    /// 数据完整性检查器
    /// </summary>
    [AppComponent("数据完整性检查器", "使其它分组中的数据与主分组一致")]
    class DbIntegrityCheckerAppComponent : TimerAppComponentBase
    {
        public DbIntegrityCheckerAppComponent(Service service)
            : base(service, TimeSpan.FromHours(1), TimeSpan.FromSeconds(10))
        {
            _service = service;
        }

        private readonly Service _service;

        protected override void OnExecuteTask(string taskName)
        {
            _Check();
        }

        private void _Check()
        {
            foreach (UtDatabase db in _GetAllUtDbs())
            {
                try
                {
                    _Check(db);
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            }
        }

        private void _Check(UtDatabase db)
        {
            foreach (TableInfo tInfo in db.GetTableInfos())
            {
                for (int index = 0; index < tInfo.PartialTableCount; index++)
                {

                }
            }
        }

        private IEnumerable<UtDatabase> _GetAllUtDbs()
        {
            foreach (string conStr in _service.DbManager.GetAllConStrs())
            {
                yield return UtDatabase.Create(_service.DbManager.GetConStr());
            }
        }
    }
}
