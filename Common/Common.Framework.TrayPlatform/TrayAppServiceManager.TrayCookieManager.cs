using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Contracts.Service;
using Common.Utility;
using Common.Package;
using Common.Package.GlobalTimer;
using Common.Contracts;

namespace Common.Framework.TrayPlatform
{
    partial class TrayAppServiceManager
    {
        private readonly CookieManager _cookieManager = new CookieManager();

        class CookieManager : Dictionary<ServiceDesc, CookieCollection>, IDisposable
        {
            public CookieManager()
            {
                _handle = GlobalTimer<ITask>.Default.Add(TimeSpan.FromSeconds(5), new TaskFuncAdapter(_CheckExpired), false);
            }

            private readonly IGlobalTimerTaskHandle _handle;

            public ReplyCookieCollection ReplyCookies { get; set; }

            private void _CheckExpired()
            {
                DateTime time = DateTime.UtcNow - TimeSpan.FromSeconds(20);

                lock (this)
                {
                    foreach (CookieCollection cookieCollection in this.Values)
                    {
                        cookieCollection.RemoveWhere(item => item.Value.CreationTime < time);
                    }
                }

                ReplyCookieCollection cookies = ReplyCookies;
                if (cookies != null && cookies.CreationTime < time)
                    ReplyCookies = null;
            }

            /// <summary>
            /// 删除指定服务的Cookie
            /// </summary>
            /// <param name="serviceDesc"></param>
            public new bool Remove(ServiceDesc serviceDesc)
            {
                Contract.Requires(serviceDesc != null);

                lock (this)
                {
                    return Remove(serviceDesc);
                }
            }

            public void Dispose()
            {
                _handle.Dispose();
            }
        }

        /// <summary>
        /// Cookie的集合
        /// </summary>
        class CookieCollection : Dictionary<string, CookieItem>
        {

        }

        class CookieItem
        {
            public DateTime CreationTime { get; set; }

            public AppServiceCookie Cookie { get; set; }
        }

        class ReplyCookieCollection
        {
            public DateTime CreationTime { get; set; }

            public Dictionary<ServiceDesc, AppServiceCookie[]> Cookies { get; set; }
        }

        class TrayCookieManager : MarshalByRefObjectEx, ITrayCookieManager
        {
            public TrayCookieManager(TrayAppServiceManager owner, TrayAppServiceInfo info)
            {
                Contract.Requires(owner != null && info != null);
                _owner = owner;
                _info = info;
            }

            private readonly TrayAppServiceInfo _info;
            private ServiceEndPoint _serviceEndPoint;
            private volatile CookieCollection _cookieCollection;
            private readonly TrayAppServiceManager _owner;

            private CookieCollection _GetCookieCollection()
            {
                if (_cookieCollection == null)
                {
                    lock (_owner._cookieManager)
                    {
                        _cookieCollection = _cookieCollection ?? _owner._cookieManager.GetOrSet(_info.AppServiceInfo.ServiceDesc);
                    }
                }

                return _cookieCollection;
            }

            private ServiceEndPoint _GetServiceEndPoint()
            {
                return _serviceEndPoint ?? (_serviceEndPoint = new ServiceEndPoint(_owner.ClientID, _info.AppServiceInfo.ServiceDesc));
            }

            /// <summary>
            /// 添加Cookie
            /// </summary>
            /// <param name="type"></param>
            /// <param name="data"></param>
            public void AddCookie(string type, byte[] data)
            {
                Contract.Requires(type != null && data != null);
                CookieCollection cookieCollection = _GetCookieCollection();

                lock (_owner._cookieManager)
                {
                    cookieCollection[type] = new CookieItem() {
                        CreationTime = DateTime.UtcNow,
                        Cookie = new AppServiceCookie() {
                            Data = data, DataType = type, EndPoint = _GetServiceEndPoint(),
                        }
                    };
                }
            }

            /// <summary>
            /// 删除Cookie
            /// </summary>
            /// <param name="type"></param>
            public void RemoveCookie(string type)
            {
                Contract.Requires(type != null);

                CookieCollection cookieCollection = _GetCookieCollection();
                lock (_owner._cookieManager)
                {
                    cookieCollection.Remove(type);
                }
            }

            /// <summary>
            /// 清除Cookie
            /// </summary>
            public void ClearCookie()
            {
                CookieCollection cookieCollection = _GetCookieCollection();
                lock (_owner._cookieManager)
                {
                    cookieCollection.Clear();
                }
            }

            private AppServiceCookie[] _GetCookies(string type, ServiceDesc serviceDesc)
            {
                if (serviceDesc != null)
                {
                    CookieCollection cookieCollection;
                    if (!_owner._cookieManager.TryGetValue(serviceDesc, out cookieCollection))
                        return Array<AppServiceCookie>.Empty;

                    if (type == null)
                        return cookieCollection.Values.ToArray(v => v.Cookie);

                    CookieItem cookieItem;
                    if (cookieCollection.TryGetValue(type, out cookieItem))
                        return new[] { cookieItem.Cookie };

                    return Array<AppServiceCookie>.Empty;
                }
                else
                {
                    if (serviceDesc == null)
                        return _owner._cookieManager.Values.SelectMany().ToArray(v => v.Value.Cookie);

                    return _owner._cookieManager.WhereNotNull(v => v.Value.GetOrDefault(type)).ToArray(v => v.Cookie);
                }
            }

            /// <summary>
            /// 获取Cookie集合
            /// </summary>
            /// <param name="type"></param>
            /// <param name="serviceDesc"></param>
            /// <returns></returns>
            public AppServiceCookieCollection GetCookies(string type, ServiceDesc serviceDesc)
            {
                lock (_owner._cookieManager)
                {
                    return new AppServiceCookieCollection() {
                        Cookies = _GetCookies(type, serviceDesc)
                    };
                }
            }

            /// <summary>
            /// 设置应答的Cookie
            /// </summary>
            /// <param name="cookies"></param>
            public void SetReplyCookies(AppServiceCookieCollection cookies)
            {
                if (cookies == null)
                {
                    _owner._cookieManager.ReplyCookies = null;
                }
                else
                {
                    _owner._cookieManager.ReplyCookies = new ReplyCookieCollection() {
                        CreationTime = DateTime.UtcNow,
                        Cookies = cookies.Cookies.GroupBy(ck => ck.EndPoint.ServiceDesc).ToDictionary(g => g.Key, g => g.ToArray())
                    };
                }
            }

            /// <summary>
            /// 获取应答Cookie
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public AppServiceCookie[] GetReplyCookies(string type)
            {
                ReplyCookieCollection replyCookies = _owner._cookieManager.ReplyCookies;
                if (replyCookies == null)
                    return Array<AppServiceCookie>.Empty;

                ServiceEndPoint endpoint = _GetServiceEndPoint();
                AppServiceCookie[] cookies1, cookies2;
                replyCookies.Cookies.TryGetValue(endpoint.ServiceDesc, out cookies1);
                replyCookies.Cookies.TryGetValue(new ServiceDesc(endpoint.ServiceDesc.Name, SVersion.Empty), out cookies2);

                AppServiceCookie[] cookies = cookies1.Concat(cookies2);

                if (type == null)
                    return cookies;

                return cookies.WhereToArray(ck => ck.DataType == type);
            }
        }
    }
}
