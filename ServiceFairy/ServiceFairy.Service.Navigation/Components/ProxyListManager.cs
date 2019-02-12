using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Common.Collection;
using Common.Communication.Wcf;
using Common.Contracts;
using Common.Contracts.Service;
using Common.Framework.TrayPlatform;
using Common.Package;
using Common.Package.Service;
using Common.Utility;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Deploy;
using ServiceFairy.Entities.Navigation;
using ServiceFairy.SystemInvoke;
using Key = System.Tuple<bool, Common.Contracts.Service.CommunicationType, ServiceFairy.Entities.Navigation.CommunicationDirection>;

namespace ServiceFairy.Service.Navigation.Components
{
    /// <summary>
    /// 代理列表
    /// </summary>
    [AppComponent("代理列表管理器", "收集并缓存代理服务的列表")]
    class ProxyListManagerAppComponent : AppComponent
    {
        public ProxyListManagerAppComponent(Service service)
            : base(service)
        {
            _service = service;
            _cache = new SingleMemoryCache<Wrapper>(TimeSpan.FromSeconds(5), _LoadCommunicationOptions);
        }

        class Wrapper
        {
            public CommunicationOption[] CommunicationOptions { get; set; }

            public Dictionary<CommunicationType, CommunicationOption[]> CommunicationTypeDict { get; set; }

            public Dictionary<IPAddress, CommunicationOption[]> AddressDict { get; set; }

            public Dictionary<Guid, CommunicationOption[]> ClientDict { get; set; }

            public Guid[] ClientIds { get; set; }
        }

        private readonly SingleMemoryCache<Wrapper> _cache;
        private readonly Service _service;
        private IPAddress _localhost = IPAddress.Parse("127.0.0.1");
        private int _index = 0;

        private Wrapper _LoadCommunicationOptions()
        {
            try
            {
                ServiceResult<AppInvokeInfo[]> sr = _service.Invoker.Deploy.SearchServicesSr(new ServiceDesc(SFNames.ServiceNames.Proxy));
                if (!sr.Succeed || sr.Result == null)
                    return null;

                // 对地址排序，将locahost地址排在前面
                sr.Result.ForEach(r => Array.Sort(r.CommunicateOptions, (x, y) => x.Address.IsLocalHost().CompareTo(y.Address.IsLocalHost())));

                CommunicationOption[] options = sr.Result.SelectMany(invokeInfo => invokeInfo.CommunicateOptions).ToArray();
                options = options.Where(op => !op.IsLocalHost()).OrderBy(x => Guid.NewGuid()).ToArray();  // 所有信道，乱序排列
                Guid[] clientIds = sr.Result.Select(v => v.ClientID).OrderBy(x => Guid.NewGuid()).ToArray();  // 终端ID，乱序排列

                return new Wrapper() {
                    CommunicationOptions = options,
                    CommunicationTypeDict = options.GroupBy(op => op.Type).ToDictionary(v => v.Key, v => v.ToArray()),
                    AddressDict = options.GroupBy(op => {
                        IPAddress address; return IPAddress.TryParse(op.Address.Address, out address) ? address : null;
                    }).Where(v => v.Key != null).ToDictionary(v => v.Key, v => v.ToArray()),
                    ClientDict = sr.Result.ToDictionary(v => v.ClientID, v => v.CommunicateOptions, true),
                    ClientIds = clientIds,
                };
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取代理列表
        /// </summary>
        /// <param name="from">调用者</param>
        /// <param name="type"></param>
        /// <param name="direction">通信方向</param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public CommunicationOption[] GetProxyList(ServiceAddress from, CommunicationType type, CommunicationDirection direction, int maxCount)
        {
            Wrapper w = _cache.Get();
            if (w == null)
                throw new ServiceException(ServerErrorCode.DataNotReady);

            if (direction == CommunicationDirection.None)
                direction = CommunicationDirection.Unidirectional;

            if (maxCount <= 0)
                maxCount = 1;

            bool isLocalHost = (from == null) || from.IsLocalHost();

            HashList<CommunicationOption> hsList = new HashList<CommunicationOption>();
            Func<CommunicationOption, bool> add = delegate(CommunicationOption op) {
                hsList.Add(op);
                return hsList.Count >= maxCount;
            };

            // 如果是本地调用，优先返回localhost地址
            if (isLocalHost)
            {
                CommunicationOption[] ops;
                if (w.ClientDict.TryGetValue(_service.Context.ClientID, out ops))
                {
                    foreach (CommunicationOption op in ops)  // localhost地址
                    {
                        if (_IsFix(op, type, direction) && add(op))
                            return hsList.ToArray();
                    }
                }
            }

            // 优先返回与调用者地址相同的代理
            if (!isLocalHost)
            {
                IPAddress address;
                CommunicationOption[] ops;
                if (IPAddress.TryParse(from.Address, out address) && w.AddressDict.TryGetValue(address, out ops))
                {
                    foreach (CommunicationOption op in ops)
                    {
                        if (_IsFix(op, type, direction) && add(op))
                            return hsList.ToArray();
                    }
                }

                CommunicationOption[] ops2;
                if (w.ClientDict.TryGetValue(_service.Context.ClientID, out ops2))  // 判断是否为本机IP
                {
                    if (ops2.Any(op => { IPAddress address2; return IPAddress.TryParse(op.Address.Address, out address2) && object.Equals(address, address2); }))
                    {
                        foreach (CommunicationOption op in ops2)
                        {
                            if (!op.IsLocalHost() && _IsFix(op, type, direction) && add(op))
                                return hsList.ToArray();
                        }
                    }
                }
            }

            // 遍历全部符合条件的地址
            int length = w.ClientIds.Length;
            if (length > 0)
            {
                int count;
                do
                {
                    count = hsList.Count;
                    Guid[] clienIds = w.ClientIds;

                    // 循环各个终端，按压力分散开
                    for (int k = _index++, kEnd = k + length; k < kEnd; k++)
                    {
                        Guid clientId = clienIds[k % length];
                        CommunicationOption[] ops = w.ClientDict[clientId];
                        for (int j = 0, jEnd = j + ops.Length; j < jEnd; j++)
                        {
                            CommunicationOption op = ops[j % ops.Length];
                            if (!hsList.Contains(op) && !op.IsLocalHost() && _IsFix(op, type, direction))
                            {
                                if (add(op))
                                    return hsList.ToArray();

                                break;
                            }
                        }
                    }
                } while (hsList.Count > count);
            }

            return hsList.ToArray();
        }

        private bool _IsFix(CommunicationOption op, CommunicationType type, CommunicationDirection direction)
        {
            return (type == CommunicationType.Unknown || op.Type == type)
                && ((op.Duplex ? CommunicationDirection.Bidirectional : CommunicationDirection.Unidirectional) & direction) != 0;
        }
    }
}
