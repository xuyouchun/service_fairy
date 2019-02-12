using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.SystemInvoke;
using ServiceFairy.Entities;
using Common.Contracts.Service;
using Common.Package;
using System.Threading;
using Common.Utility;
using System.Threading.Tasks;
using ServiceFairy.Entities.MessageCenter;
using ServiceFairy.Entities.Addins.MesssageSubscript;

namespace ServiceFairy.Service.Message.Components
{
    partial class MessageDispatcherAppComponent
    {
        private readonly AutoResetEvent _waitForMessageEvent = new AutoResetEvent(false);
        private readonly HashSet<int> _runningTaskUserIds = new HashSet<int>();

        private void _DealMessageFunc()
        {
            int index;
            while ((index = Wait(_waitForMessageEvent, 5 * 1000)) == 0 || index == WaitHandle.WaitTimeout)
            {
                try
                {
                    while (_DoRemoveMessages() || _DoSendMessages()) ;
                }
                catch (Exception ex)
                {
                    LogManager.LogError(ex);
                }
            }
        }

        private bool _DoRemoveMessages()
        {
            // 删除消息
            long[] removedMsgIndexes;
            lock (_removedMsgIndexes)
            {
                if (_removedMsgIndexes.Count == 0)
                    return false;

                removedMsgIndexes = _removedMsgIndexes.ToArray();
                _removedMsgIndexes.Clear();
                _removedMsgIndexes.TrimExcess();
            }

            if (removedMsgIndexes.Length > 0)
            {
                lock (_syncLocker)
                {
                    for (int k = 0, length = removedMsgIndexes.Length; k < length; k++)
                    {
                        _RemoveMessage(removedMsgIndexes[k]);
                    }

                    _dictOfMsgIds.TrimExcess();
                }
            }

            return true;
        }

        private bool _DoSendMessages()
        {
            int[] userIds;
            lock (_runningTaskUserIds)
            {
                if (_runningTaskUserIds.Count == 0)
                    return false;

                userIds = _runningTaskUserIds.ToArray();
                _runningTaskUserIds.Clear();
            }

            // 收集需要发送的消息
            Dictionary<int, UserMsgWrapper[]> dict = new Dictionary<int, UserMsgWrapper[]>();
            lock (_syncLocker)
            {
                for (int k = 0; k < userIds.Length; k++)
                {
                    int userId = userIds[k];
                    MessageGroup g;
                    if (_dictOfUser.TryGetValue(userId, out g) && g.Queue.Count > 0)
                    {
                        dict.Add(userId, g.Queue.DequeueAll());
                        g.Queue.TrimExcess();
                    }
                }
            }

            // 对消息按终端进行组合
            var tasks = from item in dict
                        let userId = item.Key
                        let msgs = item.Value
                        let clientId = _service.UserOnlineInfoCollector.GetClientID(userId)
                        group msgs by clientId into g
                        select new { ClientId = g.Key, Msgs = g.SelectMany().ToArray() };

            // 启动多个线程进行消息分发
            Parallel.ForEach(tasks, task => _ExecuteSendMessageTask(task.ClientId, task.Msgs));
            return true;
        }

        private void _ExecuteSendMessageTask(Guid clientId, UserMsgWrapper[] ws)
        {
            if (clientId == Guid.Empty)  // 不在线的用户
            {
                _AddToStorage(ws);
                ws.ForEach(msg => msg.State = MsgItemState.Removed);
                RemoveMessages(ws.ToArray(w => w.Index));
            }
            else  // 在线用户
            {
                _ExecuteSendMessageTaskToOnlineUsers(clientId, ws);
            }
        }

        // 存储消息
        private void _AddToStorage(UserMsgWrapper[] ws)
        {
            _service.MessageStorageManager.Save(ws.ToArray(w => w.UserMsg));
        }

        private void _ExecuteSendMessageTaskToOnlineUsers(Guid clientId, UserMsgWrapper[] msgWrappers)
        {
            // 设定发送时间，并选出不可靠消息（不需要重复发送的消息）
            List<UserMsgWrapper> notReliableMsgs = new List<UserMsgWrapper>();
            DateTime now = DateTime.UtcNow;
            for (int k = 0; k < msgWrappers.Length; k++)
            {
                UserMsgWrapper w = msgWrappers[k];
                w.LastSendTime = now;
                w.State = MsgItemState.Sending;
                w.TryTimes++;
                if (w.UserMsg.Msg.Property.HasFlag(MsgProperty.NotReliable))
                {
                    notReliableMsgs.Add(w);
                }
            }

            // 分组并发送消息
            SystemInvoker invoker = _TryCreateSystemInvoker(clientId);
            if (invoker != null)
            {
                foreach (UserMsgWrapper[] ws in _SplitBySize(msgWrappers))
                {
                    try
                    {
                        UserMsgItem[] userMsgs = ws.ToArray(w => new UserMsgItem {
                            Msg = w.UserMsg.Msg,
                            To = new[] { w.UserMsg.To },
                            MsgIndex = w.Index,
                        });

                        invoker.MesasgeSubscript.ApplyMessage(userMsgs, CallingSettings.OneWay);
                        _sendCountStopwatch.Increment(ws.Count(item => item.TryTimes <= 1));
                    }
                    catch (Exception ex)
                    {
                        LogManager.LogError(ex);
                    }
                }
            }

            // 删除不可靠传输的消息
            if (notReliableMsgs.Count > 0)
            {
                RemoveMessages(notReliableMsgs.ToArray(w => w.Index));
            }
        }

        private SystemInvoker _TryCreateSystemInvoker(Guid clientId)
        {
            try
            {
                IAppServiceAddin addin;
                if (clientId == Guid.Empty || (addin = _service.MessageSubscriptAddinManager.GetAddin(clientId)) == null)
                    return null;

                return _service.CreateInvoker(addin);
            }
            catch (Exception ex)
            {
                LogManager.LogError(ex);
                return null;
            }
        }

        private IEnumerable<UserMsgWrapper[]> _SplitBySize(UserMsgWrapper[] ws, int maxSize = 1024 * 1024 * 1, int maxCount = 200)
        {
            List<UserMsgWrapper> items = new List<UserMsgWrapper>();
            int curSize = 0;
            foreach (UserMsgWrapper w in ws)
            {
                UserMsg m = w.UserMsg;
                int size = _GetMemorySize(m.Msg);
                if (curSize + size > maxSize && items.Count > 0 || items.Count + 1 > maxCount)
                {
                    yield return items.ToArray();
                    items.Clear();
                    curSize = 0;
                }

                items.Add(w);
                curSize += size;
            }

            if (items.Count > 0)
            {
                yield return items.ToArray();
            }
        }
    }
}
