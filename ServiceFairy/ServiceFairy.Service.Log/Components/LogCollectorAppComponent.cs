using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts.Log;
using Common.Contracts.Service;
using Common.Package;
using Common.Package.Service;
using Common.Utility;

namespace ServiceFairy.Service.Log.Components
{
    /// <summary>
    /// 日志收集器
    /// </summary>
    [AppComponent("日志收集器", "收集各服务终端上的日志")]
    class LogCollectorAppComponent : TimerAppComponentBase
    {
        public LogCollectorAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
        }

        private readonly Service _service;
        private readonly ConcurrentDictionary<Guid, DateTime> _startTimeDict = new ConcurrentDictionary<Guid, DateTime>();

        protected override void OnExecuteTask(string taskName)
        {
            Guid[] clientIds = _service.CloudManager.GetAllClientIds();

            Parallel.ForEach(clientIds, (clientId) => _TryCollectLogs(clientId));
        }

        // 收集指定终端的日志
        private void _TryCollectLogs(Guid clientId)
        {
            try
            {
                DateTime startTime;
                if (!_startTimeDict.TryGetValue(clientId, out startTime))
                    startTime = DateTime.UtcNow;

                _startTimeDict[clientId] = _CollectLogs(clientId, startTime.AddTicks(1));
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
            }
        }

        // 收集指定终端的日志
        private DateTime _CollectLogs(Guid clientId, DateTime startTime)
        {
            LogItem[] items = _GetLogItems(clientId, ref startTime);
            _service.LogStorage.AddRange(items, clientId);

            return startTime;
        }

        private LogItem[] _GetLogItems(Guid clientId, ref DateTime startTime)
        {
            List<LogItem> list = new List<LogItem>();
            const int MAX_COUNT = 500;

            LogItem[] items;
            do
            {
                items = _service.Invoker.Tray.GetLocalLogItemsByTime(startTime, default(DateTime), MAX_COUNT, clientId);
                if (items.IsNullOrEmpty())
                    break;

                list.AddRange(items);
                startTime = items.Max(item => item.Time).AddTicks(1);

            } while (items.Length >= MAX_COUNT);

            return list.ToArray();
        }
    }
}
