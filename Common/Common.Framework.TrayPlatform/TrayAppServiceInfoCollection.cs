using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Collection;
using System.Diagnostics.Contracts;
using Common.Contracts;
using Common.Utility;
using Common.Contracts.Service;

namespace Common.Framework.TrayPlatform
{
    /// <summary>
    /// TrayAppServiceInfo的集合
    /// </summary>
    class TrayAppServiceInfoCollection : MarshalByRefObjectEx, IEnumerable<TrayAppServiceInfo>
    {
        private readonly Dictionary<SVersion, TrayAppServiceInfo> _dict = new Dictionary<SVersion, TrayAppServiceInfo>();
        private readonly RwLocker _locker = new RwLocker();

        /// <summary>
        /// 添加一项服务
        /// </summary>
        /// <param name="info"></param>
        /// <param name="setCurrent"></param>
        public void Add(TrayAppServiceInfo info, bool? setCurrent = null)
        {
            Contract.Requires(info != null);

            using (_locker.Write())
            {
                _dict.Add(info.AppServiceInfo.ServiceDesc.Version, info);
                if (setCurrent == true || (setCurrent == null && Current == null))
                    _current = info;
            }
        }

        /// <summary>
        /// 删除一项服务
        /// </summary>
        /// <param name="version"></param>
        public bool Remove(SVersion version)
        {
            Contract.Requires(version != null);

            using (_locker.Write())
            {
                TrayAppServiceInfo info;
                if (!_dict.TryGetValue(version, out info))
                    return false;

                if (Current == info)
                    _current = null;

                _dict.Remove(version);
                return true;
            }
        }

        /// <summary>
        /// 是否包含指定版本的服务
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public bool Contains(SVersion version)
        {
            return _dict.ContainsKey(version);
        }

        /// <summary>
        /// 获取指定版本的服务
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public TrayAppServiceInfo Get(SVersion version)
        {
            using (_locker.Read())
            {
                return _dict.GetOrDefault(version);
            }
        }

        private volatile TrayAppServiceInfo _current;

        /// <summary>
        /// 当前版本
        /// </summary>
        public TrayAppServiceInfo Current
        {
            get { return _current; }
        }

        /// <summary>
        /// 设置为当前的版本
        /// </summary>
        /// <param name="version"></param>
        public TrayAppServiceInfo SetCurrent(SVersion version)
        {
            Contract.Requires(version != null);

            using (_locker.Write())
            {
                TrayAppServiceInfo info = Get(version);
                if (info == null)
                    throw new InvalidOperationException("该版本不存在:" + version);

                _current = info;
                return info;
            }
        }

        /// <summary>
        /// 版本数量
        /// </summary>
        public int Count
        {
            get { return _dict.Count; }
        }

        public TrayAppServiceInfo[] GetAll()
        {
            using (_locker.Read())
            {
                return _dict.Values.ToArray();
            }
        }

        public IEnumerator<TrayAppServiceInfo> GetEnumerator()
        {
            return (IEnumerator<TrayAppServiceInfo>)GetAll().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
