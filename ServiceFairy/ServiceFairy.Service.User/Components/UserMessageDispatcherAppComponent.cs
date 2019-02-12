using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.User;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities;
using System.Diagnostics.Contracts;
using Common.Contracts;

namespace ServiceFairy.Service.User.Components
{
    /// <summary>
    /// 用户消息管理器
    /// </summary>
    [AppComponent("用户消息管理器", "负责转发用户的消息")]
    class UserMessageDispatcherAppComponent : TimerAppComponentBase
    {
        public UserMessageDispatcherAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(5))
        {
            _service = service;
        }

        private readonly Service _service;

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="uss">会话状态</param>
        /// <param name="to">接收者</param>
        /// <param name="content">消息内容</param>
        /// <param name="property">消息属性</param>
        public void SendUserMessage(UserSessionState uss, Users to, string content, MsgProperty property = MsgProperty.Default)
        {
            Contract.Requires(uss != null);
            SendUserMessage(uss.BasicInfo.UserId, to, content, property);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="uss">会话状态</param>
        /// <param name="to">接收者</param>
        /// <param name="content">消息内容</param>
        /// <param name="property">消息属性</param>
        public void SendUserMessage(UserSessionState uss, string to, string content, MsgProperty property = MsgProperty.Default)
        {
            SendUserMessage(uss, new Users(to), content, property);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="from">发送者</param>
        /// <param name="to">接收者</param>
        /// <param name="content">消息内容</param>
        /// <param name="property">消息属性</param>
        public void SendUserMessage(int from, Users to, string content, MsgProperty property = MsgProperty.Default)
        {
            var msg = new User_Message_Message() { From = from, Content = content };
            _service.MessageSender.Send<User_Message_Message>(msg, from, to, property: property);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="from">用户ID</param>
        /// <param name="to">接收者</param>
        /// <param name="content">消息内容</param>
        /// <param name="property">消息属性</param>
        public void SendUserMessage(int from, string to, string content, MsgProperty property = MsgProperty.Default)
        {
            SendUserMessage(from, new Users(to), content, property);
        }

        protected override void OnExecuteTask(string taskName)
        {
            
        }
    }
}
