using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using Common.Contracts.Service;
using ServiceFairy.Entities;
using System.Diagnostics.Contracts;
using Common.Contracts;
using ServiceFairy.Entities.Message;
using Common.Package;
using Common.Utility;

namespace ServiceFairy.Components
{
    /// <summary>
    /// 消息发送器
    /// </summary>
    [AppComponent("消息发送器", "发送消息给指定的用户", AppComponentCategory.System, "Sys_MessageSender")]
    public class MessageSenderAppComponent : AppComponent
    {
        public MessageSenderAppComponent(SystemAppServiceBase service)
            : base(service)
        {
            _service=service;
        }

        private readonly SystemAppServiceBase _service;

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="to">接收者</param>
        /// <returns>是否发送成功</returns>
        public bool Send(Msg msg, Users to)
        {
            Contract.Requires(msg != null && to != null);

            try
            {
                _service.Invoker.Message.SendMessage(msg,
                    _service.UserParser.ReviseMe(to, msg.From), CallingSettings.OneWay);

                return true;
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return false;
            }
        }

        /// <summary>
        /// 发送消息到群组
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="groupId">群组ID</param>
        /// <param name="includeMyself">是否包含自身</param>
        /// <returns>是否发送成功</returns>
        public bool SendToGroup(Msg msg, int groupId, bool includeMyself = false)
        {
            Users to = Users.FromGroupId(groupId);
            if (includeMyself)
                to = to - Users.FromMe();

            return Send(msg, to);
        }

        /// <summary>
        /// 发送消息到我的粉丝
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public bool SendToFollowers(Msg msg)
        {
            Contract.Requires(msg != null);
            Users to = Users.FromMyFollowers();
            return Send(msg, to);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="to">接收者</param>
        public bool Send(Msg msg, string to)
        {
            return Send(msg, new Users(to));
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="TMsg">消息实体类</typeparam>
        /// <param name="msgEntity">消息实体</param>
        /// <param name="format">消息格式</param>
        /// <param name="from">发送者</param>
        /// <param name="to">接收者</param>
        /// <param name="property">消息属性</param>
        public bool Send<TMsg>(TMsg msgEntity, int from, Users to, DataFormat format = DataFormat.Json,
            MsgProperty property = MsgProperty.Default) where TMsg : MessageEntity
        {
            Contract.Requires(msgEntity != null);

            Msg msg = Msg.Create(msgEntity, from, format: format, property: property);
            return Send(msg, to);
        }

        /// <summary>
        /// 发送消息到指定的组
        /// </summary>
        /// <typeparam name="TMsg">消息实体类</typeparam>
        /// <param name="msgEntity">消息实体</param>
        /// <param name="from">发送者</param>
        /// <param name="groupId">群组ID</param>
        /// <param name="format">消息格式</param>
        /// <param name="property">消息属性</param>
        /// <param name="includeMyself">是否包含自身</param>
        /// <returns></returns>
        public bool SendToGroup<TMsg>(TMsg msgEntity, int from, int groupId, DataFormat format = DataFormat.Json,
            MsgProperty property = MsgProperty.Default, bool includeMyself = false) where TMsg : MessageEntity
        {
            Users to = Users.FromGroupId(groupId);
            if (includeMyself)
                to = to - Users.FromMe();

            return Send<TMsg>(msgEntity, from, to, format, property);
        }

        /// <summary>
        /// 发送消息到我的粉丝
        /// </summary>
        /// <typeparam name="TMsg">消息实体类</typeparam>
        /// <param name="msgEntity">消息实体</param>
        /// <param name="from">发送者</param>
        /// <param name="format">消息格式</param>
        /// <param name="property">消息属性</param>
        /// <returns></returns>
        public bool SendToFollowers<TMsg>(TMsg msgEntity, int from, DataFormat format = DataFormat.Json,
            MsgProperty property = MsgProperty.Default) where TMsg : MessageEntity
        {
            Users to = Users.FromMyFollowers();

            return Send<TMsg>(msgEntity, from, to, format, property);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="TMsg">消息实体类</typeparam>
        /// <param name="msgEntity">消息</param>
        /// <param name="format">消息格式</param>
        /// <param name="from">发送者</param>
        /// <param name="to">接收者</param>
        /// <param name="property">消息属性</param>
        public bool Send<TMsg>(TMsg msgEntity, int from, string to, DataFormat format = DataFormat.Json,
            MsgProperty property = MsgProperty.Default) where TMsg : MessageEntity
        {
            return Send<TMsg>(msgEntity, from, new Users(to), format, property);
        }

        /// <summary>
        /// 批量发送
        /// </summary>
        /// <param name="msgArrs">消息</param>
        public bool SendArray(UserMsgArray[] msgArrs)
        {
            if (msgArrs.IsNullOrEmpty())
                return true;

            try
            {
                _service.Invoker.Message.SendMessages(msgArrs, CallingSettings.OneWay);
                return true;
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return false;
            }
        }

        /// <summary>
        /// 批量发送
        /// </summary>
        /// <param name="msgArr"></param>
        /// <returns></returns>
        public bool SendArray(UserMsgArray msgArr)
        {
            Contract.Requires(msgArr != null);

            return SendArray(new[] { msgArr });
        }
    }
}
