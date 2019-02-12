using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package;
using Common.Package.Service;
using ServiceFairy.Entities.Security;
using Common.Utility;
using Common;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 安全信息管理器
    /// </summary>
    [AppComponent("安全信息管理器", "获取并维护安全码的访问权限", AppComponentCategory.System, "Sys_SecurityManager")]
    public class SecurityManagerAppComponent : AppComponent
    {
        public SecurityManagerAppComponent(CoreAppServiceBase service)
            : base(service)
        {
            _service = service;

            _cacheChain = new CacheChain<Sid, SecurityInfo>();
            _cacheChain.AddWeakReferenceCache();
            _cacheChain.AddMemoryCache(TimeSpan.FromSeconds(60));
            _cacheChain.AddTrayCache(_service.Context, "SID", TimeSpan.FromMinutes(5), true);
            _cacheChain.AddPoly();
            _cacheChain.AddLoader(_LoadSecurityInfos);
        }

        private readonly CoreAppServiceBase _service;
        private readonly CacheChain<Sid, SecurityInfo> _cacheChain;

        private KeyValuePair<Sid, SecurityInfo>[] _LoadSecurityInfos(Sid[] sids, bool refresh)
        {
            sids = sids.WhereToArray(sid => !sid.IsEmpty());
            if (sids.Length == 0)
                return Array<KeyValuePair<Sid, SecurityInfo>>.Empty;

            CallingSettings settings = new CallingSettings { InvokeType = CommunicateInvokeType.UseOriginalSid | CommunicateInvokeType.RequestReply };
            SidInfo[] sidInfos = _service.Invoker.Security.GetSidInfos(sids, settings);
            var list = new List<KeyValuePair<Sid, SecurityInfo>>();

            for (int k = 0; k < sids.Length; k++)
            {
                Sid sid = sids[k];
                SidInfo si = sidInfos[k];
                if (si != null)
                    list.Add(new KeyValuePair<Sid, SecurityInfo>(sid, new SecurityInfo(sid, si.UserId, si.SecurityLevel)));
            }

            return list.ToArray();
        }

        /// <summary>
        /// 获取安全码信息
        /// </summary>
        /// <param name="sid">安全码</param>
        /// <returns>安全信息</returns>
        public SecurityInfo GetSecurityInfo(Sid sid)
        {
            if (sid.IsEmpty())
                return null;

            return _cacheChain.Get(sid);
        }

        /// <summary>
        /// 批量获取安全码信息
        /// </summary>
        /// <param name="sids">安全码</param>
        /// <returns>安全信息</returns>
        public IDictionary<Sid, SecurityInfo> GetSecurityInfoDict(Sid[] sids)
        {
            if (sids.IsNullOrEmpty())
                return new Dictionary<Sid, SecurityInfo>();

            return _cacheChain.GetRange(sids).ToDictionary(true);
        }

        /// <summary>
        /// 批量获取安全码信息
        /// </summary>
        /// <param name="sids">安全码</param>
        /// <returns>安全信息</returns>
        public SecurityInfo[] GetSecurityInfos(Sid[] sids)
        {
            if (sids.IsNullOrEmpty())
                return Array<SecurityInfo>.Empty;

            IDictionary<Sid, SecurityInfo> dict = GetSecurityInfoDict(sids);
            SecurityInfo[] sis = new SecurityInfo[sids.Length];
            for (int k = 0; k < sis.Length; k++)
            {
                sis[k] = dict.GetOrDefault(sids[k]);
            }

            return sis;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public SecurityInfo Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
                return null;

            SecurityInfo si = _sidCache.GetOrAddOfRelative(username, TimeSpan.FromSeconds(30), delegate(string key) {
                CallingSettings settings = new CallingSettings { InvokeType = CommunicateInvokeType.UseOriginalSid | CommunicateInvokeType.RequestReply };
                ServiceResult<SidInfo> sr = _service.Invoker.Security.LoginSr(username, password, settings);
                if (sr == null || !sr.Succeed)
                {
                    return _emptySi;
                }

                return new SecurityInfo(sr.Sid, sr.Result.UserId, sr.Result.SecurityLevel);
            });

            return (si == _emptySi) ? null : si;
        }

        private readonly SecurityInfo _emptySi = new SecurityInfo(default(Sid), default(int), default(SecurityLevel));
        private readonly Cache<string, SecurityInfo> _sidCache = new Cache<string, SecurityInfo>();
    }
}
