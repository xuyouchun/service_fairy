using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Common;
using Common.Collection;
using Common.Contracts.Service;
using Common.Data;
using Common.Data.SqlExpressions;
using Common.Data.UnionTable;
using Common.Framework.TrayPlatform;
using Common.Package;
using Common.Package.Service;
using Common.Utility;
using ServiceFairy.DbEntities;
using ServiceFairy.DbEntities.User;
using ServiceFairy.Entities.User;
using ServiceFairy.SystemInvoke;

namespace ServiceFairy.Service.User.Components
{
    /// <summary>
    /// 通信录管理器
    /// </summary>
    [AppComponent("通信录管理器", "更新用户的通信录")]
    class ContactListManagerAppComponent : TimerAppComponentBase
    {
        public ContactListManagerAppComponent(Service service)
            : base(service, TimeSpan.FromSeconds(1))
        {
            _service = service;
            _utConProvider = new RemoteUtConnectionProvider(_service.Invoker, DbEntityUtility.LoadReviseInfo());
        }

        private readonly Service _service;
        private readonly IUtConnectionProvider _utConProvider;

        protected override void OnExecuteTask(string taskName)
        {
            
        }

        private readonly object _syncLocker = new object();

        /// <summary>
        /// 更新用户的通信录
        /// </summary>
        /// <param name="uss"></param>
        /// <param name="usernames"></param>
        public void UpdateContactList(UserSessionState uss, string[] usernames)
        {
            /*
            Contract.Requires(contacts != null);
            
            int userId = uss.BasicInfo.UserId;
            DbUserContact[] dbUserContacts = _CreateDbUserContacts(uss, contacts);
            DbUserContact.Merge(_utConProvider, userId, dbUserContacts, null, new[] { DbUserContact.F_CtUserName }, option: UtConnectionMergeOption.All);*/
        }

        /// <summary>
        /// 批量添加联系人
        /// </summary>
        /// <param name="uss">会话状态</param>
        /// <param name="usernames">用户名</param>
        /// <returns></returns>
        public int[] AddContacts(UserSessionState uss, string[] usernames)
        {
            // 去重
            string[] oldUserNames = usernames;
            if (usernames == null || (usernames = usernames.Where(un => !string.IsNullOrEmpty(un)).DistinctToArray()).Length == 0)
            {
                return usernames.IsNullOrEmpty() ? Array<int>.Empty : CollectionUtility.GenerateArray<int>(usernames.Length);
            }

            // 寻找已经成为关注的用户
            int userId = uss.BasicInfo.UserId;
            SqlExpression where = SqlExpression.Equals(DbUserRelation.F_FollowerId, userId);
            object[] existsUserNames = DbUserRelation.SelectOneColumn(_utConProvider, usernames, DbUserRelation.F_UserName, where.ToString());
            string[] newUserNames = usernames.Except(existsUserNames.CastAsString("")).ToArray();

            // 在关联表中插入新关联
            IDictionary<string, int> userIdDict = _service.UserParser.ConvertUserNameToIdsDict(oldUserNames);
            DbUserRelation[] relations = newUserNames.ToArray(username => new DbUserRelation {
                UserName = username, UserId = userIdDict.GetOrDefault(username, 0), FollowerId = userId,
            });
            DbUserRelation.Insert(_utConProvider, relations);

            // 新增关注
            _service.UserRelationManager.AddFollowings(userId, userIdDict.Values.Where(id => id != 0).ToArray(), async: true);

            return oldUserNames.ToArray(un => userIdDict.GetOrDefault(un, 0));
        }

        /// <summary>
        /// 添加联系人
        /// </summary>
        /// <param name="uss">会话状态</param>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public int AddContact(UserSessionState uss, string username)
        {
            Contract.Requires(uss != null && username != null);

            int[] userIds = AddContacts(uss, new[] { username });
            return userIds.FirstOrDefault();
        }

        /// <summary>
        /// 删除联系人
        /// </summary>
        /// <param name="uss"></param>
        /// <param name="usernames"></param>
        public void RemoveContacts(UserSessionState uss, string[] usernames)
        {
            Contract.Requires(usernames != null);

#warning 无法删除联系人 ...

            /*
            int userId = uss.BasicInfo.UserId;
            DbUserContact.DeleteIn(_utConProvider, DbUserContact.F_CtUserName, usernames, userId);

            int[] contactIds = _service.Invoker.UserCenter.ConvertUserNameToIds(usernames);
            _AddChangedList(userId, contactIds);*/
        }

        /// <summary>
        /// 删除联系人
        /// </summary>
        /// <param name="uss"></param>
        /// <param name="username"></param>
        public void RemoveContact(UserSessionState uss, string username)
        {
            Contract.Requires(username != null);

            RemoveContacts(uss, new[] { username });
        }

        /// <summary>
        /// 获取联系人列表
        /// </summary>
        /// <param name="uss"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        public string[] SafeGetContactList(UserSessionState uss)
        {
            Contract.Requires(uss != null);

#warning 无法查到联系人列表 ...
            /*
            string username = uss.BasicInfo.UserName;
            DbUserRelation[] relations = DbUserRelation.Select(_utConProvider, username,
                new[]{DbUserRelation.f*/

            return null;
        }
    }
}
