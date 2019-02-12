using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceFairy.Entities.User;
using Common.Contracts.Service;
using Common.Contracts;

namespace ServiceFairy.Service.User
{
    /*
    /// <summary>
    /// 获取指定联系人的信息
    /// </summary>
    [AppCommand("GetUserInfo", "获取指定联系人的信息", SecurityLevel = SecurityLevel.User), Remarks(Remarks), DisabledCommand]
    class GetUserInfoAppCommand : ACS<Service>.Func<User_GetUserInfo_Request, User_GetUserInfo_Reply>
    {
        protected override User_GetUserInfo_Reply OnExecute(AppCommandExecuteContext<Service> context, User_GetUserInfo_Request request, ref Common.Contracts.Service.ServiceResult sr)
        {
            UserInfo info = context.Service.UserAccountManager.GetUserInfo(request.UserId);
            return new User_GetUserInfo_Reply { Info = info };
        }

        const string Remarks = @"获取指定联系人的详细信息，会首先根据用户所具有的权限进行过滤，并非能看到所有用户的详细信息。
也可能会过滤掉某些字段，例如看不到对方的真实姓名等。
进行群组对话时可能会用到该接口获取一些本地不存在的联系人信息，例如由我的联系人所添加进来的其它联系人等。";
    }*/
}
