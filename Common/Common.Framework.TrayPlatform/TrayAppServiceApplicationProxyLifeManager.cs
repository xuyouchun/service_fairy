using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package;
using Common.Package.GlobalTimer;
using Common.Utility;
using System.Diagnostics.Contracts;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// 代理生命周期管理器
    /// </summary>
    class TrayAppServiceApplicationProxyLifeManager : IDisposable
    {
        public TrayAppServiceApplicationProxyLifeManager()
        {
            _handle = GlobalTimer<ITask>.Default.Add(TimeSpan.FromSeconds(5), new TaskFuncAdapter(_CheckExpired), false);
        }

        private readonly IGlobalTimerTaskHandle _handle;

        private readonly Dictionary<object, Tag> _tags = new Dictionary<object, Tag>();

        class Tag
        {
            public Tag(object owner)
            {
                Owner = owner;
                LastAccessTime = DateTime.UtcNow;
            }

            public object Owner { get; set; }

            public DateTime LastAccessTime { get; set; }
        }

        public void KeepEnable(object owner)
        {
            Contract.Requires(owner != null);

            lock (_tags)
            {
                _tags.GetOrSet(owner, (key) => new Tag(owner)).LastAccessTime = DateTime.UtcNow;
            }
        }

        public void Disable(object owner)
        {
            Contract.Requires(owner != null);

            lock (_tags)
            {
                _tags.Remove(owner);
            }
        }

        public bool Enabled
        {
            get { return _tags.Count > 0; }
        }

        private void _CheckExpired()
        {
            if (_tags.Count == 0)
                return;

            lock (_tags)
            {
                DateTime now = DateTime.UtcNow;
                _tags.RemoveWhere(tag => tag.Value.LastAccessTime + TimeSpan.FromSeconds(30) < now);
            }
        }

        public void Dispose()
        {
            _handle.Dispose();

            lock (_tags)
            {
                _tags.Clear();
            }
        }
    }
}
