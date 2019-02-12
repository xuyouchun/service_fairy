using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using Common.Package.Service;
using Common.Utility;
using ServiceFairy.Entities;
using ServiceFairy.Entities.MessageCenter;

namespace ServiceFairy.Service.Message.Components
{
    /// <summary>
    /// 消息存储器
    /// </summary>
    [AppComponent("消息存储器", "负责将未发送成功的消息存储到消息中心，并从消息中心提取消息")]
    class MessageStorageManagerAppComponent : TimerAppComponentBase
    {
        public MessageStorageManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(1))
        {
            _service = service;
        }

        private readonly Service _service;
        private readonly Queue<UserMsg> _msgQueue = new Queue<UserMsg>();

        protected override void OnExecuteTask(string taskName)
        {
            if (_msgQueue.Count == 0)
                return;

            UserMsg[] msgs = _msgQueue.SafeDequeueAll();
            _SaveToMessageCenter(msgs);
        }

        private UserMsgArray[] _ToUserMsgArrs(UserMsg[] msgs)
        {
            var dict = new Dictionary<Guid, KeyValuePair<UserMsg, HashSet<int>>>();
            foreach (UserMsg msg in msgs)
            {
                if (msg.To == 0)
                    continue;

                KeyValuePair<UserMsg, HashSet<int>> v;
                if (!dict.TryGetValue(msg.Msg.ID, out v))
                    dict.Add(msg.Msg.ID, v = new KeyValuePair<UserMsg, HashSet<int>>(msg, new HashSet<int>()));

                v.Value.Add(msg.To);
            }

            return dict.Values.ToArray(v => new UserMsgArray { Msgs = new[] { v.Key.Msg }, To = v.Value.ToArray() });
        }

        private void _SaveToMessageCenter(UserMsg[] msgs)
        {
            foreach (IEnumerable<UserMsg> part in msgs.Split(50))
            {
                AutoRetry(() => {
                    _service.Invoker.MessageCenter.Save(_ToUserMsgArrs(part.ToArray()));
                }, TimeSpan.FromSeconds(1), 5);
            }
        }

        /// <summary>
        /// 将消息持久化存储
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="to">接收者</param>
        public void Save(Msg msg, int to)
        {
            if (msg == null || to == 0)
                return;

            _msgQueue.SafeEnqueue(new UserMsg { Msg = msg, To = to });
        }

        /// <summary>
        /// 将消息批量持久化存储
        /// </summary>
        /// <param name="userMsgs"></param>
        public void Save(IEnumerable<UserMsg> userMsgs)
        {
            Contract.Requires(userMsgs != null);

            _msgQueue.SafeEnqueueRange(userMsgs);
        }
    }
}
