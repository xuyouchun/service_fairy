using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.Group;
using Common.Contracts.Service;

namespace ServiceFairy.Service.Group
{
    static class Utility
    {
        /// <summary>
        /// 确保为群组成员
        /// </summary>
        /// <param name="service"></param>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <param name="refresh"></param>
        /// <param name="errorMsg"></param>
        public static void EnsureMemberOfGroup(this Service service, int userId, int groupId, bool refresh = false, string errorMsg = null)
        {
            if (!service.GroupManager.IsMemberOfGroup(userId, groupId, refresh))
                throw new ServiceException(GroupStatusCode.NotMemberOfGroup, message: errorMsg);
        }

        /// <summary>
        /// 确保为群组的创建者
        /// </summary>
        /// <param name="service"></param>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <param name="refresh"></param>
        /// <param name="errorMsg"></param>
        public static void EnsureCreatorOfGroup(this Service service, int userId, int groupId, bool refresh = false, string errorMsg = null)
        {
            if (!service.GroupManager.IsCreatorOfGroup(userId, groupId, refresh))
                throw new ServiceException(GroupStatusCode.NotCreatorOfGroup, message: errorMsg);
        }

        /// <summary>
        /// 确保不为群组的创建者
        /// </summary>
        /// <param name="service"></param>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <param name="refresh"></param>
        /// <param name="errorMsg"></param>
        public static void EnsureNotCreatorOfGroup(this Service service, int userId, int groupId, bool refresh = false, string errorMsg = null)
        {
            if (!service.GroupManager.IsCreatorOfGroup(userId, groupId, refresh))
                throw new ServiceException(GroupStatusCode.CreatorOfGroup, message: errorMsg);
        }

        /// <summary>
        /// 确保为群组的创建者
        /// </summary>
        /// <param name="service"></param>
        /// <param name="userId"></param>
        /// <param name="groupIds"></param>
        /// <param name="refresh"></param>
        /// <param name="errorMsg"></param>
        public static void EnsureCreatorOfGroups(this Service service, int userId, int[] groupIds, bool refresh = false, string errorMsg = null)
        {
            if (!service.GroupManager.IsCreatorOfGroups(userId, groupIds, refresh))
                throw new ServiceException(GroupStatusCode.NotCreatorOfGroup, message: errorMsg);
        }
    }
}
