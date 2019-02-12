using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.UserCenter;
using Common.Contracts.Service;

namespace ServiceFairy.Service.UserCenter
{
    /// <summary>
    /// 获取用户关系
    /// </summary>
    [AppCommand("GetRelation", "获取用户关系", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class GetRelationAppCommand : ACS<Service>.Func<UserCenter_GetRelation_Request, UserCenter_GetRelation_Reply>
    {
        protected override UserCenter_GetRelation_Reply OnExecute(AppCommandExecuteContext<Service> context, UserCenter_GetRelation_Request req, ref ServiceResult sr)
        {
            var urMgr = context.Service.UserRelationManager;

            List<UserRelation> urs = new List<UserRelation>();

            foreach (int userId in req.UserIds)
            {
                UserRelation ur = new UserRelation() { UserId = userId };
                if (req.Mask.HasFlag(GetRelationMask.Follower))
                {
                    ur.Followers = urMgr.GetFollowers(userId);
                }

                if (req.Mask.HasFlag(GetRelationMask.Following))
                {
                    ur.Followings = urMgr.GetFollowings(userId);
                }

                urs.Add(ur);
            }

            return new UserCenter_GetRelation_Reply() { Relations = urs.ToArray() };
        }
    }
}
