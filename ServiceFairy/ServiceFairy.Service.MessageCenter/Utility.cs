using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceFairy.Entities;
using ServiceFairy.Entities.MessageCenter;
using Common.Utility;
using Common;
using System.Diagnostics.Contracts;
using Common.Contracts;
using ServiceFairy.DbEntities.User;

namespace ServiceFairy.Service.MessageCenter
{
    static class Utility
    {
        /// <summary>
        /// 转换为数据库消息实体
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static DbMessage ToDbMessage(this Msg msg)
        {
            Contract.Requires(msg != null);

            return new DbMessage {
                Data = _ToBase64String(msg.Data),
                From = msg.From,
                Method = msg.Method,
                Time = msg.Time,
                Property = (int)msg.Property
            };
        }

        private static string _ToBase64String(byte[] data)
        {
            if (data.IsNullOrEmpty())
                return string.Empty;

            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// 从数据库实体转换为消息实体
        /// </summary>
        /// <param name="dbMsg">数据库实体</param>
        /// <returns>消息实体</returns>
        public static Msg ToMsg(this DbMessage dbMsg)
        {
            Contract.Requires(dbMsg != null);

            return new Msg {
                Data = _FromBase64String(dbMsg.Data),
                From = dbMsg.From,
                Method = dbMsg.Method,
                Property = (MsgProperty)dbMsg.Property,
                Time = dbMsg.Time,
            };
        }

        private static byte[] _FromBase64String(string data)
        {
            if (string.IsNullOrEmpty(data))
                return Array<byte>.Empty;

            return Convert.FromBase64String(data);
        }

        /// <summary>
        /// 验证当前操作序号的合法性
        /// </summary>
        /// <param name="opIndex"></param>
        public static void ValidateOpIndex(int opIndex)
        {
            if (opIndex < 0 || opIndex > 1)
                throw new ArgumentException("当前操作序号只能为0或1");
        }
    }
}
