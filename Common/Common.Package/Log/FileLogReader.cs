using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Log;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using System.IO;
using Common.Utility;
using System.Text.RegularExpressions;

namespace Common.Package.Log
{
    /// <summary>
    /// 日志读取器
    /// </summary>
    /// <typeparam name="TLogItem"></typeparam>
    public class FileLogReader<TLogItem> : MarshalByRefObjectEx, ILogReader<TLogItem>, IDisposable
        where TLogItem : class, ILogItem
    {
        public FileLogReader(string baseDirectory)
        {
            Contract.Requires(baseDirectory != null);
            _baseDirectory = baseDirectory.Trim();
        }

        private readonly string _baseDirectory;

        [Flags]
        enum ReadFromPathOption
        {
            FromDirectory = 0x01,

            FromFile = 0x02,

            All = FromDirectory | FromFile,
        }

        private LogItemGroup[] _ReadFromPath(string path, ReadFromPathOption option)
        {
            if (Directory.Exists(path))
            {
                List<LogItemGroup> groups = new List<LogItemGroup>();

                if (option.HasFlag(ReadFromPathOption.FromDirectory))
                {
                    groups.AddRange(from dir in Directory.GetDirectories(path)
                                    let dInfo = new DirectoryInfo(dir)
                                    select new LogItemGroup(_GetName(dir), dInfo.CreationTimeUtc, dInfo.LastWriteTimeUtc, -1));
                }

                if (option.HasFlag(ReadFromPathOption.FromFile))
                {
                    groups.AddRange(from file in Directory.GetFiles(path)
                                    let fInfo = new FileInfo(file)
                                    select new LogItemGroup(_GetName(file), fInfo.CreationTimeUtc, fInfo.LastWriteTimeUtc, fInfo.Length));
                }

                return groups.ToArray();
            }

            return Array<LogItemGroup>.Empty;
        }

        private string _GetName(string path)
        {
            return path.Substring(_baseDirectory.Length).TrimStart('/', '\\');
        }

        private LogItemGroup[] _ReadGroups(string parentGroup, ReadFromPathOption option)
        {
            if (string.IsNullOrEmpty(parentGroup))
                return _ReadFromPath(_baseDirectory, option);

            return _ReadFromPath(Path.Combine(_baseDirectory, parentGroup), option);
        }

        public LogItemGroup[] ReadGroups(string parentGroup)
        {
            return _ReadGroups(parentGroup, ReadFromPathOption.All);
        }

        public LogItemGroup[] ReadGroupsByTime(DateTime start, DateTime end)
        {
            return _ReadGroupsByTime(null, start, end);
        }

        private LogItemGroup[] _ReadGroupsByTime(string parentGroup, DateTime start, DateTime end)
        {
            List<LogItemGroup> list = new List<LogItemGroup>();

            foreach (LogItemGroup g in _ReadGroups(parentGroup, ReadFromPathOption.FromFile))
            {
                if ((start == default(DateTime) || start >= g.CreationTime) &&
                    (end == default(DateTime) || end <= g.LastModifyTime))
                {
                    list.Add(g);
                }
            }

            foreach (LogItemGroup g in _ReadGroups(parentGroup, ReadFromPathOption.FromDirectory))
            {
                list.AddRange(_ReadGroupsByTime(g.Name, start, end));
            }

            return list.ToArray();
        }

        public TLogItem[] ReadItems(string parentGroup)
        {
            string file = Path.Combine(_baseDirectory, parentGroup);
            if (!File.Exists(file))
                return Array<TLogItem>.Empty;

            lock (string.Intern("LOGFILE_" + file))
            {
                string text = File.ReadAllText(file);
                IEnumerable<string> logItems = _regex.Split(text).Where(s => !string.IsNullOrEmpty(s));

                return logItems.ToArray(item => ObjectFactory.CreateObject<TLogItem>(item));
            }
        }

        private static readonly Regex _regex = new Regex("\r\n" + LogSettings.FileWriteSpliter + "\r\n", RegexOptions.Singleline | RegexOptions.Compiled);

        /// <summary>
        /// 删除日志组
        /// </summary>
        /// <param name="group"></param>
        public void DeleteGroup(string group)
        {
            string path;
            if (string.IsNullOrEmpty(group))  // 删除全部日志
            {
                path = _baseDirectory;
                PathUtility.ClearPath(path);
            }
            else
            {
                path = Path.Combine(_baseDirectory, group);
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
                else if (File.Exists(path))
                    File.Delete(path);
            }
        }

        public void Dispose()
        {
            
        }
    }
}
