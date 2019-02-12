using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using System.Diagnostics.Contracts;
using Common;
using Common.Utility;
using ServiceFairy.Components;
using Common.Contracts;
using System.Diagnostics;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities.Station;

namespace ServiceFairy.Service.Station.Components
{
    /// <summary>
    /// 服务插件管理器
    /// </summary>
    [AppComponent("插件管理器", "通过插件实现服务功能扩展", AppComponentCategory.Application, name: "ServiceAddInManager")]
    class ServiceAddinManagerAppComponent : TimerAppComponentBase
    {
        public ServiceAddinManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
        }

        private readonly Service _service;
        private readonly Dictionary<ServiceEndPoint, Group> _dictOfServiceEndpoint = new Dictionary<ServiceEndPoint, Group>();

        private readonly RwLocker _locker = new RwLocker();

        class Group
        {
            public DateTime UpdateTime { get; set; }

            public ServiceEndPoint Source { get; set; }

            public ServiceAddinRelation[] Relations { get; set; }
        }

        protected override void OnExecuteTask(string taskName)
        {
            ServiceAddinRelations[] relations;
            using (_locker.Write())
            {
                DateTime now = DateTime.UtcNow;
                _dictOfServiceEndpoint.RemoveWhere(item => item.Value.UpdateTime + TimeSpan.FromMinutes(1) < now);

                relations = _dictOfServiceEndpoint.Values.ToArray(v => new ServiceAddinRelations { EndPoint = v.Source, Relations = v.Relations });
            }

            IServiceClient serviceClient = _service.Context.CreateBoradcastServiceClientOfMyself();
            CoreInvoker invoker = CoreInvoker.FromServiceClient(serviceClient);
            invoker.Station.RegisterAddins(relations);
        }

        /// <summary>
        /// 注册插件
        /// </summary>
        /// <param name="sourceEndpoint">终端</param>
        /// <param name="relations">关联关系</param>
        public void Register(ServiceEndPoint sourceEndpoint, ServiceAddinRelation[] relations)
        {
            Contract.Requires(sourceEndpoint != null && relations != null);

            using (_locker.Write())
            {
                _Register(sourceEndpoint, relations);
            }
        }

        private void _Register(ServiceEndPoint sourceEndpoint, ServiceAddinRelation[] relations)
        {
            Group g = _dictOfServiceEndpoint.GetOrSet(sourceEndpoint,
                    (key) => new Group { Source = sourceEndpoint, UpdateTime = DateTime.UtcNow, Relations = relations });

            g.UpdateTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 注册插件
        /// </summary>
        /// <param name="relations">关联关系</param>
        public void Register(ServiceAddinRelations relations)
        {
            Contract.Requires(relations != null);

            using (_locker.Write())
            {
                _Register(relations.EndPoint, relations.Relations);
            }
        }

        /// <summary>
        /// 注册插件
        /// </summary>
        /// <param name="relations">关联关系</param>
        public void Register(ServiceAddinRelations[] relations)
        {
            Contract.Requires(relations != null);

            using (_locker.Write())
            {
                foreach (ServiceAddinRelations r in relations)
                {
                    _Register(r.EndPoint, r.Relations);
                }
            }
        }

        /// <summary>
        /// 获取指定服务所拥有的插件
        /// </summary>
        /// <param name="target"></param>
        public ServiceAddinRelation[] GetByTarget(ServiceDesc target)
        {
            Contract.Requires(target != null);

            using (_locker.Read())
            {
                return _dictOfServiceEndpoint.Values.SelectMany(v => v.Relations)
                    .Where(r => ServiceDesc.Compatible(target, r.Target)
                ).ToArray();
            }
        }

        /// <summary>
        /// 获取指定服务所拥有的插件
        /// </summary>
        /// <param name="targets"></param>
        /// <returns></returns>
        public ServiceAddinRelation[] GetByTargets(ServiceDesc[] targets)
        {
            Contract.Requires(targets != null);

            using (_locker.Read())
            {
                return _dictOfServiceEndpoint.Values.SelectMany(v => v.Relations)
                    .Where(r => targets.Any(target => ServiceDesc.Compatible(target, r.Target))
                ).ToArray();
            }
        }

        public override ObjectProperty[] GetAllProperties()
        {
            List<ObjectProperty> properties = new List<ObjectProperty>(base.GetAllProperties());

            using (_locker.Read())
            {
                var list = from g in _dictOfServiceEndpoint.Values.SelectMany(v => v.Relations).GroupBy(r => r.AddinInfo.AddinDesc)
                           let addinDesc = g.Key
                           let addinInfo = g.First().AddinInfo
                           select new ObjectProperty(addinDesc.ToString(), addinInfo.Title, ObjectPropertyType.UserDefined, addinInfo.Desc);

                properties.AddRange(list);
            }

            return properties.ToArray();
        }

        public override ObjectPropertyValue GetPropertyValue(string propertyName)
        {
            ObjectPropertyValue value = base.GetPropertyValue(propertyName);
            if (value != null)
                return value;

            AddinDesc addinDesc;
            if (AddinDesc.TryParse(propertyName, out addinDesc))
            {
                StringBuilder sb = new StringBuilder();
                var list = _dictOfServiceEndpoint.Values.SelectMany(v => v.Relations).Where(r => r.AddinInfo.AddinDesc == addinDesc);
                foreach (ServiceAddinRelation r in list.OrderBy(r => r.Target))
                {
                    if (sb.Length > 0)
                        sb.AppendLine();

                    sb.AppendFormat("来源:{0}　接入:{1}", r.Source, r.Target);
                }

                return new ObjectPropertyValue(propertyName, sb.ToString());
            }

            return null;
        }
    }
}
