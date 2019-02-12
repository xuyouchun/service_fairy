using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ServiceFairy.Entities.UserCenter;
using Common.Contracts;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 修改用户信息－请求
    /// </summary>
    [Serializable, DataContract, Summary("修改用户信息－请求")]
    public class User_ModifyMyInfo_Request : UserRequestEntity
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 信息项
        /// </summary>
        [DataMember, SysFieldDoc(SysField.UserDetailInfo)]
        public string VCard { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNull(VCard, "VCard");
        }
    }
}
