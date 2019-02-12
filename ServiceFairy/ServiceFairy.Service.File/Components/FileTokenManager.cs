using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using ServiceFairy.Entities.File;
using Common.Contracts.Service;
using Common.Package;
using Common.Package.GlobalTimer;

namespace ServiceFairy.Service.File.Components
{
    /// <summary>
    /// 事件读写事务的管理器
    /// </summary>
    class FileTokenManager : IDisposable
    {
        public FileTokenManager()
        {
            _taskHandle = GlobalTimer<ITask>.Default.Add(TimeSpan.FromSeconds(30), new TaskFuncAdapter(_CheckExpired), false);
        }

        private readonly Dictionary<string, FileToken> _tokenDict = new Dictionary<string, FileToken>();
        private readonly IGlobalTimerTaskHandle _taskHandle;

        /// <summary>
        /// 添加一个事务
        /// </summary>
        /// <param name="token"></param>
        public void Add(FileToken token)
        {
            Contract.Requires(token != null);

            _tokenDict.Add(token.Token, token);
        }

        /// <summary>
        /// 获取一个事务
        /// </summary>
        /// <param name="token"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public FileToken Get(string token, bool throwError = true)
        {
            Contract.Requires(token != null);

            lock (_tokenDict)
            {
                FileToken ft;
                if (!_tokenDict.TryGetValue(token, out ft) && throwError)
                    throw Utility.CreateBusinessException(FileStatusCode.InvalidToken);

                ft.LastAccessTime = DateTime.UtcNow;
                return ft;
            }
        }

        /// <summary>
        /// 删除一个事务
        /// </summary>
        /// <param name="token"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public FileToken Remove(string token, bool throwError = true)
        {
            Contract.Requires(token != null);

            lock (_tokenDict)
            {
                FileToken ft;
                if (!_tokenDict.TryGetValue(token, out ft) && throwError)
                    throw Utility.CreateBusinessException(FileStatusCode.InvalidToken);

                _tokenDict.Remove(token);
                return ft;
            }
        }

        private void _CheckExpired()
        {
            DateTime now = DateTime.UtcNow;
            TimeSpan ts = TimeSpan.FromMinutes(5);

            lock (_tokenDict)
            {
                foreach (FileToken ft in _tokenDict.Values)
                {
                    if (now - ft.LastAccessTime > ts)
                    {
                        if (ft.IsStreamCreated())
                            ft.Stream.Cancel();
                    }
                }
            }
        }

        public void Dispose()
        {
            _taskHandle.Dispose();
        }
    }
}
