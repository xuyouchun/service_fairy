using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Data.UnionTable;

namespace ServiceFairy.Entities.Database
{
    /// <summary>
    /// 初始化表的元数据－请求
    /// </summary>
    [Serializable, DataContract]
    public class Database_InitMetadata_Request : RequestEntity
    {
        /// <summary>
        /// 修正信息
        /// </summary>
        [DataMember]
        public UtTableGroupReviseInfo[] ReviseInfos { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(ReviseInfos, "ReviseInfos");
        }
    }
}
