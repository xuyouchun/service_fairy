using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities;
using System.Threading;
using Common.Package;

namespace ServiceFairy.Service.Message.Components
{
    /// <summary>
    /// 消息与接收者的组合
    /// </summary>
    struct UserMsg
    {
        public UserMsg(Msg msg, int to)
        {
            Msg = msg;
            To = to;
        }

        /// <summary>
        /// 消息
        /// </summary>
        public Msg Msg;

        /// <summary>
        /// 接收者
        /// </summary>
        public int To;
    }

    /// <summary>
    /// 消息包装
    /// </summary>
    class UserMsgWrapper
    {
        public UserMsgWrapper()
        {
            Index = Interlocked.Increment(ref _index);
            LastSendTime = QuickTime.UtcNow;
        }

        private static long _index;

        /// <summary>
        /// 消息
        /// </summary>
        public UserMsg UserMsg { get; set; }

        /// <summary>
        /// 上次发送时间
        /// </summary>
        public DateTime LastSendTime { get; set; }

        /// <summary>
        /// 消息状态
        /// </summary>
        public MsgItemState State { get; set; }

        /// <summary>
        /// 已发送次数
        /// </summary>
        public int TryTimes { get; set; }

        /// <summary>
        /// 自增索引号
        /// </summary>
        public long Index { get; set; }
    }

    /// <summary>
    /// 消息的状态
    /// </summary>
    enum MsgItemState
    {
        /// <summary>
        /// 正在等待
        /// </summary>
        Wait,

        /// <summary>
        /// 正在发送
        /// </summary>
        Sending,

        /// <summary>
        /// 已删除
        /// </summary>
        Removed,
    }
}
