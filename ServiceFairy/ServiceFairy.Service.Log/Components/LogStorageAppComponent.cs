using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.Log;
using Common.Utility;
using System.IO;
using Common.Contracts.Log;
using Key = System.Tuple<string, Common.Contracts.MessageType>;

namespace ServiceFairy.Service.Log.Components
{
    /// <summary>
    /// 日志存储器
    /// </summary>
    [AppComponent("日志存储器", "存储日志分析器的结果")]
    class LogStorageAppComponent : TimerAppComponentBase
    {
        public LogStorageAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
        }

        private readonly Service _service;

        public Dictionary<Key, Queue<LogItem>> _logDict = new Dictionary<Key, Queue<LogItem>>();

        /// <summary>
        /// 批量添加日志项
        /// </summary>
        /// <param name="items">日志项</param>
        /// <param name="clientId">服务终端唯一标识</param>
        public void AddRange(LogItem[] items, Guid clientId)
        {
            if (items.IsNullOrEmpty())
                return;

            lock (_logDict)
            {
                foreach (LogItem item in items)
                {
                    string category = item.Category;
                    if (!string.IsNullOrEmpty(category))
                    {
                        Queue<LogItem> list = _logDict.GetOrSet(new Key(category, item.Type));
                        list.Enqueue(item);

                        while (list.Count > 10000)
                        {
                            list.Dequeue();
                        }
                    }
                }
            }
        }

        private DateTime _curTime = _GetTime();

        private static DateTime _GetTime(DateTime t = default(DateTime))
        {
            if (t == default(DateTime))
                t = DateTime.UtcNow;

            return new DateTime(t.Year, t.Month, t.Day, t.Hour, 0, 0);
        }

        protected override void OnExecuteTask(string taskName)
        {
            lock (_logDict)
            {
                DateTime t = DateTime.UtcNow - TimeSpan.FromDays(3);  // 删除三天之前的日志
                foreach (Queue<LogItem> q in _logDict.Values)
                {
                    while (q.Count > 0 && q.Peek().Time < t)
                    {
                        q.Dequeue();
                    }
                }

                _logDict.RemoveWhere(v => v.Value.Count == 0);
            }
        }

        // 保存日志
        private void _SaveLogs(string file, PolyLog[] items)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(items.Length.ToString());
            for (int k = 0; k < items.Length; k++)
            {
                if (k > 0)
                    sb.AppendLine(_SPILTTER);

                _AppendLogItem(sb, items[k]);
            }

            lock (_GetLocker(file))
            {
                PathUtility.CreateDirectoryForFile(file);
                File.WriteAllText(file, sb.ToString(), Encoding.UTF8);
            }
        }

        private void _AppendLogItem(StringBuilder sb, PolyLog item)
        {
            object[] values = new object[] {
                item.Category, item.Source, item.ClientIds.JoinBy(","), item.Title,
                item.StartTime, item.EndTime, item.Times, item.Detail,
            };

            for (int k = 0; k < values.Length; k++)
            {
                if (k > 0)
                    sb.Append("\t");

                string s = values[k].ToStringIgnoreNull("").Replace('\t', ' ');
                sb.Append(s);
            }
        }

        private const string _SPILTTER = "\r\n==========\r\n";

        private string _GetFilePath(DateTime time)
        {
            return PathUtility.Combine(_service.ServiceDataPath,
                string.Format("{0}/{1}.txt", time.ToString("yyyy-MM-dd"), time.ToString("HH")));
        }

        // 获取指定文件的锁
        private object _GetLocker(string filePath)
        {
            return string.Intern(filePath.ToLower());
        }

        // 根据时间批量获取指定的日志
        private string[] _GetFilePaths(DateTime startTime, DateTime endTime)
        {
            DateTime t = _GetTime(startTime);
            List<string> paths = new List<string>();

            while (t < endTime + TimeSpan.FromHours(1))
            {
                string path = _GetFilePath(t);
                paths.Add(path);

                t += TimeSpan.FromHours(1);
            }

            return paths.ToArray();
        }
    }
}
