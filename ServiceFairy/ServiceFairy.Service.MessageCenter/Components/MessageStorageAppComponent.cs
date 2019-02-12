using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Contracts.Service;
using Common.Package.Service;
using ServiceFairy.Entities.MessageCenter;
using Common.Utility;
using Common.Data.UnionTable;
using Common.Data.SqlExpressions;
using ServiceFairy.Entities;
using Common;
using ServiceFairy.DbEntities.User;
using Common.Contracts;
using Common.Package;

namespace ServiceFairy.Service.MessageCenter.Components
{
    /// <summary>
    /// 消息存储器
    /// </summary>
    [AppComponent("消息存储器", "存储消息并维护消息目录树")]
    class MessageStorageAppComponent : TimerAppComponentBase
    {
        public MessageStorageAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(1))
        {
            _service = service;
            _utProvider = service.DbConnectionManager.Provider;
        }

        private readonly Service _service;
        private readonly IUtConnectionProvider _utProvider;
        private readonly List<UserMsgArray> _msgList = new List<UserMsgArray>();

        protected override void OnExecuteTask(string taskName)
        {
            if (_msgList.Count == 0)
                return;

            UserMsgArray[] arrs;
            lock (_msgList)
            {
                arrs = _msgList.ToArray();
                _msgList.Clear();
                _msgList.TrimExcess();
            }

            DbMessage[] msgs = _ToDbMesssages(arrs);
            DbMessage.Insert(_utProvider, msgs);
        }

        // 转换为字符串
        private string _MsgDataToString(Msg msg, bool throwError = false)
        {
            if (msg.Data.IsNullOrEmpty())
                return null;

            try
            {
                if (msg.Format == DataFormat.Json)
                    return Encoding.UTF8.GetString(msg.Data);
            }
            catch (Exception ex)
            {
                if (throwError)
                    throw;

                LogManager.LogError(ex);
            }

            return null;
        }

        // 转换为字节流
        private byte[] _MsgDataToBytes(string data, DataFormat format, bool throwError = false)
        {
            try
            {
            if(format == DataFormat.Json)
                return Encoding.UTF8.GetBytes(data);

            }
            catch(Exception ex)
            {
                if(throwError)
                    throw;

                LogManager.LogError(ex);
            }

            return null;
        }

        private DbMessage[] _ToDbMesssages(UserMsgArray[] arrs)
        {
            List<DbMessage> msgs = new List<DbMessage>();
            foreach (UserMsgArray arr in arrs)
            {
                if (arr == null || arr.Msgs.IsNullOrEmpty() || arr.To.IsNullOrEmpty())
                    continue;

                for (int j = 0, jLen = arr.Msgs.Length; j < jLen; j++)
                {
                    Msg msg = arr.Msgs[j];
                    string data;

                    if (msg != null && (data = _MsgDataToString(msg)) != null)
                    {
                        for (int k = 0, kLen = arr.To.Length; k < kLen; k++)
                        {
                            int userId = arr.To[k];
                            if (userId != 0)
                            {
                                msgs.Add(new DbMessage {
                                    UserId = userId, Data = data, From = msg.From,
                                    Method = msg.Method, Property = (int)msg.Property, Time = msg.Time,
                                });
                            }
                        }
                    }
                }
            }

            return msgs.ToArray();
        }

        /// <summary>
        /// 批量存储消息
        /// </summary>
        /// <param name="userMsgArrs"></param>
        public void Save(UserMsgArray[] userMsgArrs)
        {
            if (userMsgArrs.IsNullOrEmpty())
                return;

            _msgList.SafeAddRange(userMsgArrs);
        }

        /// <summary>
        /// 读取指定用户的消息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public Msg[] Read(int userId)
        {
            Msg[] msgs = _DequeueMsgs(userId);
            if (msgs.IsNullOrEmpty())
                return Array<Msg>.Empty;

            List<Msg> list = new List<Msg>();
            var dict = new Dictionary<Tuple<string, int>, Msg>();

            // 对具有Override属性的消息去重
            foreach (Msg msg in msgs)
            {
                if (string.IsNullOrEmpty(msg.Method))
                    continue;

                if (!msg.Property.HasFlag(MsgProperty.Override))
                    list.Add(msg);

                Msg oldMsg;
                var key = new Tuple<string, int>(msg.Method, msg.From);
                if (!dict.TryGetValue(key, out oldMsg) || oldMsg.Time < msg.Time)
                    dict[key] = msg;
            }

            list.AddRange(dict.Values);
            list.Sort((x, y) => x.Time.CompareTo(y.Time));
            return list.ToArray();
        }

        private Msg[] _DequeueMsgs(int userId)
        {
            DbMessage[] dbMsgs = DbMessage.SelectAllColumns(_utProvider, userId);
            if (dbMsgs.IsNullOrEmpty())
                return Array<Msg>.Empty;

            DateTime maxTime = dbMsgs.Max(msg => msg.Time);
            DbMessage.DeleteByRouteKey(_utProvider, userId, (string)SqlExpression.LittleEquals(DbMessage.F_Time, maxTime));

            return dbMsgs.ToArray(dbMsg => new Msg {
                Format = DataFormat.Json, ID = Guid.NewGuid(), From = dbMsg.From,
                Method = dbMsg.Method, Property = (MsgProperty)dbMsg.Property, Time = dbMsg.Time, Data = _MsgDataToBytes(dbMsg.Data, DataFormat.Json)
            });
        }

        /// <summary>
        /// 清空指定用户的消息池
        /// </summary>
        /// <param name="userId">用户ID</param>
        public void Clear(int userId)
        {
            Clear(new[] { userId });
        }

        /// <summary>
        /// 清空指定用户的消息池
        /// </summary>
        /// <param name="userIds">用户ID</param>
        public void Clear(int[] userIds)
        {
            if (userIds.IsNullOrEmpty())
                return;

            DbMessage.DeleteByRouteKey(_utProvider, userIds);
        }
    }
}
