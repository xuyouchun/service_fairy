using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Common;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Message;
using ServiceFairy.Service.Message.Components;

namespace ServiceFairy.Service.Message
{
    static class Utility
    {
        /// <summary>
        /// 组合消息与接收者
        /// </summary>
        /// <param name="service">服务</param>
        /// <param name="msgs">消息</param>
        /// <param name="toUsers">接收者</param>
        /// <returns>消息与接收者的组合</returns>
        public static UserMsg[] CombineMsgs(this Service service, Msg[] msgs, Users toUsers)
        {
            Contract.Requires(msgs != null);

            if (toUsers == null)
            {
                return Array<UserMsg>.Empty;
            }
            else
            {
                int[] userIds = service.UserParser.Parse(toUsers);
                return msgs.SelectMany(
                    msg => userIds.Select(userId => new UserMsg(msg, userId))).ToArray();
            }
        }

        /// <summary>
        /// 组合消息与接收者
        /// </summary>
        /// <param name="service">服务</param>
        /// <param name="msg">消息</param>
        /// <param name="toUsers">接收者</param>
        /// <returns>消息与接收者的组合</returns>
        public static UserMsg[] CombineMsgs(this Service service, Msg msg, Users toUsers)
        {
            Contract.Requires(msg != null);

            return CombineMsgs(service, new[] { msg }, toUsers);
        }
    }
}
