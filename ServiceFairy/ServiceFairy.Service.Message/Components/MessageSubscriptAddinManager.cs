using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Components;
using Common.Utility;
using Common;
using ServiceFairy.Entities;

namespace ServiceFairy.Service.Message.Components
{
    /// <summary>
    /// 消息订阅插件管理器
    /// </summary>
    [AppComponent("消息订阅插件管理器", "管理所有的消息订阅插件")]
    class MessageSubscriptAddinManager : TimerAppComponentBase
    {
        public MessageSubscriptAddinManager(Service service)
            : base(service, TimeSpan.FromSeconds(1))
        {
            _service = service;
        }

        private readonly Service _service;
        private Wrapper _wrapper = Wrapper.Empty;

        class Wrapper
        {
            public Dictionary<Guid, AppServiceAddinItem> Dict;
            public AppServiceAddinItem[] AddinItems;

            public static readonly Wrapper Empty = new Wrapper {
                Dict = new Dictionary<Guid, AppServiceAddinItem>(),
                AddinItems = Array<AppServiceAddinItem>.Empty
            };
        }

        /// <summary>
        /// 获取指定终端的插件
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public AppServiceAddinItem GetAddinItem(Guid clientId)
        {
            return _wrapper.Dict.GetOrDefault(clientId);
        }

        /// <summary>
        /// 获取指定终端的插件
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public IAppServiceAddin GetAddin(Guid clientId)
        {
            AppServiceAddinItem item = GetAddinItem(clientId);
            return item == null ? null : item.Addin;
        }

        /// <summary>
        /// 获取所有的插件
        /// </summary>
        /// <returns></returns>
        public AppServiceAddinItem[] GetAllAddinItems()
        {
            return _wrapper.AddinItems;
        }

        protected override void OnExecuteTask(string taskName)
        {
            AppServiceAddinItem[] addinItems = _service.ServiceAddIn.GetAddins(ServiceAddinNames.ADDIN_MESSAGE_SUBSCRIPT);
            if (addinItems.IsNullOrEmpty())
                _wrapper = Wrapper.Empty;
            else
                _wrapper = new Wrapper {
                    Dict = addinItems.ToDictionary(addinItem => addinItem.Source.ClientId, true),
                    AddinItems = addinItems
                };
        }
    }
}
