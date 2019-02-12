using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;

namespace ServiceFairy.Entities.Station
{
    /// <summary>
    /// 注册插件－请求
    /// </summary>
    [Serializable, DataContract]
    public class Station_RegisterAddins_Request : RequestEntity
    {
        /// <summary>
        /// 关联关系
        /// </summary>
        [DataMember]
        public ServiceAddinRelations[] Relations { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(Relations, "Relations");
        }
    }

    /// <summary>
    /// 服务插件的关联关系
    /// </summary>
    [Serializable, DataContract]
    public class ServiceAddinRelation
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        [DataMember]
        public AppServiceAddinInfo AddinInfo { get; set; }

        /// <summary>
        /// 所在服务终端
        /// </summary>
        [DataMember]
        public ServiceEndPoint Source { get; set; }

        /// <summary>
        /// 注册目标
        /// </summary>
        [DataMember]
        public ServiceDesc Target { get; set; }
    }

    /// <summary>
    /// ServiceAddinRelation与ServiceEndPoint的关联
    /// </summary>
    [Serializable, DataContract]
    public class ServiceAddinRelations
    {
        /// <summary>
        /// 终端
        /// </summary>
        [DataMember]
        public ServiceEndPoint EndPoint { get; set; }

        /// <summary>
        /// 关联关系
        /// </summary>
        [DataMember]
        public ServiceAddinRelation[] Relations { get; set; }
    }
}
