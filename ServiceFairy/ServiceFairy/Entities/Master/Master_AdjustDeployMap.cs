using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Runtime.Serialization;
using Common.Contracts;

namespace ServiceFairy.Entities.Master
{
    /// <summary>
    /// 修改部署地图－请求
    /// </summary>
    [Serializable, DataContract]
    public class Master_AdjustDeployMap_Request : RequestEntity
    {
        /// <summary>
        /// 需要做出的修改
        /// </summary>
        [DataMember]
        public AppClientAdjustInfo[] AdjustInfos { get; set; }

        public override void Validate()
        {
            base.Validate();

            if (AdjustInfos == null)
                throw new ServiceException(ServerErrorCode.ArgumentError, "AdjustInfos为空");
        }
    }

    /// <summary>
    /// 修改部署地图－应答
    /// </summary>
    [Serializable, DataContract]
    public class Master_AdjustDeployMap_Reply : ReplyEntity
    {

    }
}
