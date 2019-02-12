using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace Common.Contracts.Service
{
    /// <summary>
    /// 消息信息
    /// </summary>
    [Serializable, DataContract]
    public class AppMessageInfo
    {
        public AppMessageInfo(ServiceDesc serviceDesc, MessageDesc messageDesc, AppParameter data, string title, string desc)
        {
            Contract.Requires(serviceDesc != null && messageDesc != null);

            ServiceDesc = serviceDesc;
            MessageDesc = messageDesc;
            Data = data;
            Title = title;
            Desc = desc;
        }

        /// <summary>
        /// 服务
        /// </summary>
        [DataMember]
        public ServiceDesc ServiceDesc { get; private set; }

        /// <summary>
        /// 消息
        /// </summary>
        [DataMember]
        public MessageDesc MessageDesc { get; private set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        [DataMember]
        public string Title { get; private set; }

        /// <summary>
        /// 消息描述
        /// </summary>
        [DataMember]
        public string Desc { get; private set; }

        /// <summary>
        /// 数据
        /// </summary>
        [DataMember]
        public AppParameter Data { get; private set; }

        public static AppMessageInfo FromPrototy(AppMessageInfo info, string title, string desc)
        {
            return new AppMessageInfo(info.ServiceDesc, info.MessageDesc, info.Data,
                StringUtility.GetFirstNotNullOrWhiteSpaceString(title, info.Title),
                StringUtility.GetFirstNotNullOrWhiteSpaceString(desc, info.Desc)
            );
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool full)
        {
            return string.IsNullOrEmpty(ServiceDesc.Name) ? MessageDesc.ToString(full)
                : ServiceDesc.ToString(full) + "/" + MessageDesc.ToString(full);
        }
    }
}
