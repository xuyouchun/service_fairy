using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using ServiceFairy.Entities.User;
using ServiceFairy.Service.User.Components;
using System.Runtime.Serialization;
using ServiceFairy.DbEntities.User;
using Common.Contracts;

namespace ServiceFairy.Service.User
{
    /// <summary>
    /// 修改用户信息
    /// </summary>
    [AppCommand("ModifyMyInfo", title: "修改当前用户信息", SecurityLevel = SecurityLevel.User), Remarks(Remarks)]
    class ModifyMyInfoAppCommand : ACS<Service>.Action<User_ModifyMyInfo_Request>
    {
        protected override void OnExecute(AppCommandExecuteContext<Service> context, User_ModifyMyInfo_Request req, ref ServiceResult sr)
        {
            int userId = context.GetSessionState().BasicInfo.UserId;
            context.Service.UserAccountManager.ModifyUserInfo(userId, req.Name, req.VCard);
        }

        const string Remarks = @"修改当前登录用户的信息。修改成功之后，我的所有粉丝将收到InfoChanged消息。";
    }
}
