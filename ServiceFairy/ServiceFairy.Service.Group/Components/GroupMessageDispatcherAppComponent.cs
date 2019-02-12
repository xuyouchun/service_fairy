using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using System.Collections.Concurrent;
using Common.Utility;
using System.Threading;
using ServiceFairy.Entities;
using ServiceFairy.Entities.Group;
using ServiceFairy.Entities.Message;

namespace ServiceFairy.Service.Group.Components
{
    /// <summary>
    /// 群组消息分发器
    /// </summary>
    [AppComponent("群组消息分发器", "将消息分发给群组的其它在线者")]
    class GroupMessageDispatcherAppComponent : TimerAppComponentBase
    {
        public GroupMessageDispatcherAppComponent(Service service)
            : base(service)
        {
            _service = service;
        }

        private readonly Service _service;

        protected override void OnExecuteTask(string taskName)
        {
            
        }

        protected override void OnStart()
        {
            base.OnStart();

            ThreadUtility.StartNew(_DispatchFunc, ThreadPriority.AboveNormal);
        }

        // 消息分发线程
        private void _DispatchFunc()
        {
            while (Wait(_waitForMsg))
            {
                GroupMsg[] gMsgs;
                while (!(gMsgs = _PopAllMessages()).IsNullOrEmpty())
                {
                    InvokeNoThrow(() => _SendMessages(gMsgs));
                }
            }
        }

        // 批量发送消息
        private void _SendMessages(GroupMsg[] gMsgs)
        {
            UserMsgArray[] items = gMsgs.ToArray(_CreateMsgItems);
            _service.MessageSender.SendArray(items);
        }

        private UserMsgArray _CreateMsgItems(GroupMsg gMsg)
        {
            Group_Message_Message msg = gMsg.Message;
            int from = gMsg.From;
            return UserMsgArray.Create(Msg.Create(msg, from), Users.FromGroupId(msg.GroupId) - Users.FromUserId(from));
        }

        private readonly ManualResetEvent _waitForMsg = new ManualResetEvent(false);
        private readonly Queue<GroupMsg> _groupMsgs = new Queue<GroupMsg>();

        private GroupMsg[] _PopAllMessages()
        {
            return _groupMsgs.SafeDequeueAll();
        }

        private void _PushMessage(GroupMsg groupMsg)
        {
            _groupMsgs.SafeEnqueue(groupMsg);
            _waitForMsg.Set();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="groupId">组ID</param>
        /// <param name="content">消息</param>
        public void SendMessage(int sender, int groupId, string content)
        {
            Group_Message_Message msg = new Group_Message_Message() {
                GroupId = groupId, Content = content, Sender = sender, Time = DateTime.UtcNow
            };

            _PushMessage(new GroupMsg(msg, sender));
        }

        #region Class GroupMsg ...

        class GroupMsg
        {
            public GroupMsg(Group_Message_Message msg, int from)
            {
                Message = msg;
                From = from;
            }

            public Group_Message_Message Message { get; private set; }

            public int From { get; private set; }
        }

        #endregion

    }
}
