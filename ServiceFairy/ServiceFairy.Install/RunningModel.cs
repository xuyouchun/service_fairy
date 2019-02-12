using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Common.Contracts.Service;
using SfName = ServiceFairy.SFNames.ServiceNames;
using Common.Utility;
using System.Runtime.Serialization;

namespace ServiceFairy.Install
{
    /// <summary>
    /// 运行模式
    /// </summary>
    [Flags]
    public enum RunningModel
    {
        None = 0x00,

        /// <summary>
        /// 普通终端
        /// </summary>
        [Desc("普通终端")]
        Normal = 0x01,

        /// <summary>
        /// 中心服务
        /// </summary>
        [Desc("中心服务")]
        Master = 0x02,

        /// <summary>
        /// 导航服务
        /// </summary>
        [Desc("导航服务")]
        Navigation = 0x04,

        /// <summary>
        /// 代理服务
        /// </summary>
        [Desc("代理服务")]
        Proxy = 0x08,
    }

    public static class RunningModelUtility
    {
        private static readonly Dictionary<RunningModel, ServiceDesc[]> _dict = new Dictionary<RunningModel,string[]> {
            { RunningModel.Normal, new [] { SfName.Tray } },
            { RunningModel.Master, new [] { SfName.Tray, SfName.Master, SfName.Deploy, SfName.Configuration, SfName.Station, SfName.Security } },
            { RunningModel.Navigation, new [] { SfName.Tray, SfName.Navigation } },
            { RunningModel.Proxy, new [] { SfName.Tray, SfName.Proxy } },
        }.ToDictionary(item=>item.Key, item=>item.Value.ToArray(v=>ServiceDesc.Parse(v)));

        public static ServiceDesc[] GetServiceDescs(this RunningModel model)
        {
            HashSet<ServiceDesc> hs = new HashSet<ServiceDesc> { ServiceDesc.Parse(SfName.Tray) };
            foreach (KeyValuePair<RunningModel, ServiceDesc[]> item in _dict)
            {
                if (model.HasFlag(item.Key))
                {
                    hs.AddRange(item.Value);
                }
            }

            return hs.ToArray();
        }
    }
}
