using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.UserCenter;
using Common.Utility;

namespace ServiceFairy.Service.UserCenter
{
    /// <summary>
    /// 将用户ID转换为用户名
    /// </summary>
    [AppCommand("ConvertUserIdToNames", "将用户ID转换为用户名", SecurityLevel = SecurityLevel.SysRunningLevel)]
    class ConvertUserIdToNamesAppCommand : ACS<Service>.Func<UserCenter_ConvertUserIdToNames_Request, UserCenter_ConvertUserIdToNames_Reply>
    {
        protected override UserCenter_ConvertUserIdToNames_Reply OnExecute(AppCommandExecuteContext<Service> context, UserCenter_ConvertUserIdToNames_Request request, ref ServiceResult sr)
        {
            int[] userIds = request.UserIds;
            UserBasicInfo[] basicInfos = context.Service.UserInfoManager.GetUserBasicInfos(userIds, request.Refresh);
            Dictionary<int, UserBasicInfo> infoDict = basicInfos.ToDictionary(bi => bi.UserId);

            string[] userNames = userIds.ToArray(userId => _GetUserName(infoDict, userId));
            return new UserCenter_ConvertUserIdToNames_Reply { UserNames = userNames };
        }

        private string _GetUserName(Dictionary<int, UserBasicInfo> infoDict, int userId)
        {
            UserBasicInfo bi;
            if (infoDict.TryGetValue(userId, out bi))
                return bi.UserName;

            return null;
        }
    }
}
