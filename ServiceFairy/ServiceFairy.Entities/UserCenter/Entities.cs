using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common;
using Common.Utility;

namespace ServiceFairy.Entities.UserCenter
{
    /// <summary>
    /// 用户中心的请求实体
    /// </summary>
    [Serializable, DataContract]
    public class UserCenterRequestEntity : RequestEntity
    {
        /// <summary>
        /// 是否允许路由
        /// </summary>
        [DataMember]
        public bool EnableRoute { get; set; }
    }

    /// <summary>
    /// 用户中心的应答实体
    /// </summary>
    [Serializable, DataContract]
    public class UserCenterReplyEntity : ReplyEntity
    {

    }

    #region Class UserCacheKey ...

    /// <summary>
    /// 用户缓存键
    /// </summary>
    [DataContract, Serializable]
    public class UserCacheKey
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public UserInfoMask Mask { get; set; }

        public override string ToString()
        {
            return UserId + ":" + (int)Mask;
        }

        public override int GetHashCode()
        {
            return UserId ^ (int)Mask;
        }

        public override bool Equals(object obj)
        {
            UserCacheKey obj2 = (UserCacheKey)obj;
            return UserId == obj2.UserId && Mask == obj2.Mask;
        }

        public static UserCacheKey[] Join(UserCacheKey[] keys)
        {
            if (keys.Length == 0)
                return Array<UserCacheKey>.Empty;

            if (keys.Length == 1)
                return keys;

            Dictionary<int, UserCacheKey> dict = new Dictionary<int, UserCacheKey>();
            for (int k = 0; k < keys.Length; k++)
            {
                UserCacheKey key = keys[k];
                dict.GetOrSet(key.UserId, (uid) => new UserCacheKey { UserId = uid }).Mask |= key.Mask;
            }

            return dict.Values.ToArray();
        }

        public static UserCacheKeyGroup[] JoinAndGroup(UserCacheKey[] keys)
        {
            UserCacheKey[] newKeys = Join(keys);
            return newKeys.GroupBy(key => key.Mask).ToArray(g => new UserCacheKeyGroup { Mask = g.Key, UserIds = g.ToArray(v => v.UserId) });
        }

        /// <summary>
        /// 创建缓存键
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public static UserCacheKey[] CreateCacheKeys(int[] userIds)
        {
            return userIds.SelectMany(userId => new UserCacheKey[] {
                new UserCacheKey{ UserId = userId, Mask = UserInfoMask.Basic },
                new UserCacheKey { UserId = userId, Mask = UserInfoMask.Detail },
                new UserCacheKey { UserId = userId, Mask = UserInfoMask.Status }
            }).ToArray();
        }
    }

    public class UserCacheKeyGroup
    {
        public int[] UserIds { get; set; }
        public UserInfoMask Mask { get; set; }
    }

    #endregion

    #region Class GroupCacheKey ...

    /// <summary>
    /// 群组缓存键
    /// </summary>
    [DataContract, Serializable]
    public class GroupCacheKey
    {
        [DataMember]
        public int GroupId { get; set; }

        [DataMember]
        public GroupInfoMask Mask { get; set; }

        public override string ToString()
        {
            return GroupId + ":" + (int)Mask;
        }

        public override int GetHashCode()
        {
            return GroupId ^ (int)Mask;
        }

        public override bool Equals(object obj)
        {
            GroupCacheKey obj2 = (GroupCacheKey)obj;
            return GroupId == obj2.GroupId && Mask == obj2.Mask;
        }

        public static GroupCacheKey[] Join(GroupCacheKey[] keys)
        {
            if (keys.Length == 0)
                return Array<GroupCacheKey>.Empty;

            if (keys.Length == 1)
                return keys;

            Dictionary<int, GroupCacheKey> dict = new Dictionary<int, GroupCacheKey>();
            for (int k = 0; k < keys.Length; k++)
            {
                GroupCacheKey key = keys[k];
                dict.GetOrSet(key.GroupId, (gid) => new GroupCacheKey { GroupId = gid }).Mask |= key.Mask;
            }

            return dict.Values.ToArray();
        }

        public static GroupCacheKeyGroup[] JoinAndGroup(GroupCacheKey[] keys)
        {
            GroupCacheKey[] newKeys = Join(keys);
            return newKeys.GroupBy(key => key.Mask).ToArray(g => new GroupCacheKeyGroup { Mask = g.Key, GroupIds = g.ToArray(v => v.GroupId) });
        }

        /// <summary>
        /// 创建缓存键
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        public static GroupCacheKey[] CreateCacheKeys(int[] groupIds)
        {
            return groupIds.SelectMany(userId => new GroupCacheKey[] {
                new GroupCacheKey { GroupId = userId, Mask = GroupInfoMask.Basic },
                new GroupCacheKey { GroupId = userId, Mask = GroupInfoMask.Detail },
                new GroupCacheKey { GroupId = userId, Mask = GroupInfoMask.Member }
            }).ToArray();
        }
    }

    public class GroupCacheKeyGroup
    {
        public int[] GroupIds { get; set; }
        public GroupInfoMask Mask { get; set; }
    }

    #endregion
}
