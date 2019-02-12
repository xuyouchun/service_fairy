using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.IO;
using Common.Utility;
using Common.Package.GlobalTimer;
using Common.Contracts.Log;
using Common.Contracts.Service;

namespace Common.Package.Log
{
    /// <summary>
    /// 用于提供FileLogWriter所使用的名称
    /// </summary>
    public interface IFileLogWriterPathStrategy
    {
        /// <summary>
        /// 获取一个新的日志文件路径
        /// </summary>
        /// <param name="createNew"></param>
        /// <returns></returns>
        string BuildPath(bool createNew = false);
    }

    #region Class TimeFileLogWriterPathStrategy ...

    /// <summary>
    /// 以当前时间为名称提供日志文件路径的策略
    /// </summary>
    class TimeFileLogWriterPathStrategy : IFileLogWriterPathStrategy
    {
        public TimeFileLogWriterPathStrategy(string directory)
        {
            Contract.Requires(directory != null);

            _directory = directory;
        }

        private readonly string _directory;
        private DateTime _lastCreateTime;
        private string _lastPath;

        #region IFileLogWriterPathStrategy Members

        public string BuildPath(bool createNew)
        {
            DateTime now = DateTime.Now;
            if (createNew || _lastPath == null || now.Date != _lastCreateTime.Date)
            {
                for (int k = 0; ; k++)
                {
                    string path = Path.Combine(_directory, string.Format("{0}{1}.txt",
                        DateTime.Now.ToString(@"yyyy-MM-dd\\HHmmss"), k == 0 ? "" : ("_" + k)));

                    if (!File.Exists(path))
                    {
                        _lastPath = path;
                        break;
                    }
                }

                _lastCreateTime = now;
            }

            return _lastPath;
        }

        #endregion
    }

    #endregion

    /// <summary>
    /// 基于文件的日志记录策略
    /// </summary>
    /// <typeparam name="TLogItem">日志类型</typeparam>
    [System.Diagnostics.DebuggerStepThrough]
    public class FileLogWriter<TLogItem> : MarshalByRefObjectEx, ILogWriter<TLogItem>, IDisposable
        where TLogItem : class, ILogItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pathStrategy">用于提供所使用的路径</param>
        /// <param name="maxFileSize">每个日志文件的最大尺寸</param>
        public FileLogWriter(IFileLogWriterPathStrategy pathStrategy, int maxFileSize = DEFAULT_MAX_FILE_SIZE)
        {
            Contract.Requires(pathStrategy != null && maxFileSize > 0);
            _PathStrategy = pathStrategy;
            _maxFileSize = maxFileSize;

            _TaskHandle = Global.GlobalTimer.Add(TimeSpan.FromSeconds(2), new TaskFuncAdapter(_WriteLogItems), false);
        }

        internal const int DEFAULT_MAX_FILE_SIZE = 1 * 1024 * 1024;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="directory">日志路径</param>
        public FileLogWriter(string directory, int maxFileSize = DEFAULT_MAX_FILE_SIZE)
            : this(new TimeFileLogWriterPathStrategy(directory))
        {

        }

        private readonly IFileLogWriterPathStrategy _PathStrategy;
        private readonly IGlobalTimerTaskHandle _TaskHandle;
        private readonly int _maxFileSize;

        #region ILogWriter<TLogItem> Members

        private readonly List<TLogItem> _logItems = new List<TLogItem>();
        private string _path;

        /// <summary>
        /// 批量记录日志
        /// </summary>
        /// <param name="items">日志</param>
        public void Write(IEnumerable<TLogItem> items)
        {
            if (items == null)
                return;

            lock (_logItems)
            {
                foreach (TLogItem item in items)
                {
                    _logItems.Add(item);
                }
            }
        }

        #endregion

        private void _WriteLogItems()
        {
            if (_logItems.Count == 0)
                return;

            TLogItem[] items;
            lock (_logItems)
            {
                items = _logItems.ToArray();
                _logItems.Clear();
            }

            using (IEnumerator<TLogItem> enumerator = ((IEnumerable<TLogItem>)items).GetEnumerator())
            {
            _restart:
                _path = _PathStrategy.BuildPath(_path == null || _GetFileSize(_path) >= _maxFileSize);

                lock (string.Intern(GetType() + "_" + _path))
                {
                    PathUtility.CreateDirectoryForFile(_path);
                    using (FileStream fs = new FileStream(_path, FileMode.Append))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8, 2048))
                        {
                            while (enumerator.MoveNext())
                            {
                                TLogItem logItem = enumerator.Current;
                                sw.Write(logItem.ToText());
                                sw.Write(SPLITER);

                                if (fs.Length >= _maxFileSize)
                                    goto _restart;
                            }
                        }
                    }
                }
            }
        }

        const string SPLITER = "\r\n" + LogSettings.FileWriteSpliter + "\r\n";

        private bool _TryWriteLogItems(TLogItem[] items)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (TLogItem item in items)
                {
                    sb.Append(item.ToText());
                    sb.Append(SPLITER);
                }

                _path = _PathStrategy.BuildPath(_path == null || _GetFileSize(_path) >= _maxFileSize);
                string directory = Path.GetDirectoryName(_path);
                if (!File.Exists(directory))
                    Directory.CreateDirectory(directory);

                File.AppendAllText(_path, sb.ToString(), Encoding.UTF8);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static long _GetFileSize(string path)
        {
            if (!File.Exists(path))
                return 0;

            return new FileInfo(path).Length;
        }

        #region IDisposable Members

        ~FileLogWriter()
        {
            Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _TaskHandle.Dispose();
            _WriteLogItems();
        }

        #endregion
    }
}
