using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Package.Service;
using System.Diagnostics.Contracts;
using ServiceFairy.Entities.User;
using Common.Contracts.Service;
using ServiceFairy.Entities.UserCenter;
using ServiceFairy.Entities;

namespace ServiceFairy.SystemInvoke
{
    partial class SystemInvoker
    {
        private UserInvoker _user;

        /// <summary>
        /// User Service
        /// </summary>
        public UserInvoker User
        {
            get { return _user ?? (_user = new UserInvoker(this)); }
        }

        /// <summary>
        /// 用户服务
        /// </summary>
        public class UserInvoker : Invoker
        {
            public UserInvoker(SystemInvoker owner)
                : base(owner)
            {
                RegisterEventHandler<User_StatusChanged_Message>((sender, e) => RaiseEvent(StatusChanged, sender, e));
                RegisterEventHandler<User_NewUser_Message>((sender, e) => RaiseEvent(NewUser, sender, e));
                RegisterEventHandler<User_Message_Message>((sender, e) => RaiseEvent(Message, sender, e));
                RegisterEventHandler<User_InfoChanged_Message>((sender, e) => RaiseEvent(InfoChanged, sender, e));
            }

            /// <summary>
            /// 用户状态变化
            /// </summary>
            public event ServiceClientReceiveEventHandler<User_StatusChanged_Message> StatusChanged;

            /// <summary>
            /// 新用户注册
            /// </summary>
            public event ServiceClientReceiveEventHandler<User_NewUser_Message> NewUser;

            /// <summary>
            /// 接收到用户消息
            /// </summary>
            public event ServiceClientReceiveEventHandler<User_Message_Message> Message;

            /// <summary>
            /// 用户信息变化
            /// </summary>
            public event ServiceClientReceiveEventHandler<User_InfoChanged_Message> InfoChanged;

            /// <summary>
            /// 用户注册
            /// </summary>
            /// <param name="verifyCode">验证码</param>
            /// <param name="username">用户名</param>
            /// <param name="password">密码</param>
            /// <param name="autoLogin">是否自动登录</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<Sid> RegisterSr(string username, string password, string verifyCode, bool autoLogin = true, CallingSettings settings = null)
            {
                var sr = UserService.Register(Sc, new User_Register_Request() {
                        UserName = username, Password = password, VerifyCode = verifyCode, AutoLogin = autoLogin,
                    }, settings
                );

                return CreateSr(sr, r => r.Sid);
            }

            /// <summary>
            /// 用户注册
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="password">密码</param>
            /// <param name="verifyCode">验证码</param>
            /// <param name="autoLogin">是否自动登录</param>
            /// <param name="settings">调用设置</param>
            public void Register(string username, string password, string verifyCode, bool autoLogin = true, CallingSettings settings = null)
            {
                InvokeWithCheck(RegisterSr(username, password, verifyCode, autoLogin, settings));
            }

            /// <summary>
            /// 激活手机号
            /// </summary>
            /// <param name="phoneNumber">手机号码</param>
            /// <param name="verifyCode">验证码</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<int> MobileActiveSr(string phoneNumber, string verifyCode, CallingSettings settings = null)
            {
                var sr = UserService.MobileActive(Sc, new User_MobileActive_Request { PhoneNumber = phoneNumber, VerifyCode = verifyCode }, settings);
                return CreateSr(sr, r => r.UserId);
            }

            /// <summary>
            /// 激活手机号
            /// </summary>
            /// <param name="phoneNumber">手机号码</param>
            /// <param name="verifyCode">验证码</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void MobileActive(string phoneNumber, string verifyCode, CallingSettings settings = null)
            {
                InvokeWithCheck(MobileActiveSr(phoneNumber, verifyCode, settings));
            }

            /// <summary>
            /// 停用手机号
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult MobileDeactiveSr(CallingSettings settings = null)
            {
                return UserService.MobileDeactive(Sc, settings);
            }

            /// <summary>
            /// 停用手机号
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void MobileDeactive(CallingSettings settings = null)
            {
                InvokeWithCheck(MobileDeactiveSr(settings));
            }

            /// <summary>
            /// 用户登录
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="password">密码</param>
            /// <param name="settings">调用设置</param>
            /// <returns>安全码</returns>
            public ServiceResult<int> LoginSr(string username, string password, CallingSettings settings = null)
            {
                var sr = UserService.Login(Sc,
                    new User_Login_Request() { UserName = username, Password = password }, settings
                );

                return CreateSr(sr, r => r.UserId);
            }

            /// <summary>
            /// 用户登录
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="password">密码</param>
            /// <param name="settings">调用设置</param>
            /// <returns>安全码</returns>
            public int Login(string username, string password, CallingSettings settings = null)
            {
                return InvokeWithCheck(LoginSr(username, password, settings));
            }

            /// <summary>
            /// 用户退出登录
            /// </summary>
            /// <param name="sid">安全码</param>
            /// <param name="settings">调用设置</param>
            public ServiceResult LogoutSr(CallingSettings settings = null)
            {
                return UserService.Logout(Sc, settings);
            }

            /// <summary>
            /// 用户退出登录
            /// </summary>
            /// <param name="sid">安全码</param>
            /// <param name="settings">调用设置</param>
            public void Logout(CallingSettings settings = null)
            {
                InvokeWithCheck(LogoutSr(settings));
            }

            /// <summary>
            /// 密码重置
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="newPassword">新密码</param>
            /// <param name="verifyCode">验证码</param>
            /// <param name="settings">调用设置</param>
            /// <param name="autoLogin">是否自动登录</param>
            /// <returns></returns>
            public ServiceResult<Sid> ResetPasswordSr(string username, string newPassword, string verifyCode, bool autoLogin = true, CallingSettings settings = null)
            {
                var sr = UserService.ResetPassword(Sc, new User_ResetPassword_Request() {
                    UserName = username, VerifyCode = verifyCode,
                });

                return CreateSr(sr, r => r.Sid);
            }

            /// <summary>
            /// 密码重置
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="newPassword">新密码</param>
            /// <param name="settings">调用设置</param>
            /// <param name="verifyCode">验证码</param>
            /// <param name="autoLogin">是否自动登录</param>
            /// <returns></returns>
            public Sid ResetPassword(string username, string newPassword, string verifyCode, bool autoLogin = true, CallingSettings settings = null)
            {
                return InvokeWithCheck(ResetPasswordSr(username, newPassword, verifyCode, autoLogin, settings));
            }

            /// <summary>
            /// 修改密码
            /// </summary>
            /// <param name="oldPassword">旧密码</param>
            /// <param name="newPassword">新密码</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<Sid> ModifyPasswordSr(string oldPassword, string newPassword, CallingSettings settings = null)
            {
                var sr = UserService.ModifyPassword(Sc, new User_ModifyPassword_Request() {
                    NewPassword = newPassword, OldPassword = oldPassword
                });

                return CreateSr(sr, r => r.Sid);
            }

            /// <summary>
            /// 修改密码
            /// </summary>
            /// <param name="oldPassword">旧密码</param>
            /// <param name="newPassword">新密码</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public Sid ModifyPassword(string oldPassword, string newPassword, CallingSettings settings = null)
            {
                return InvokeWithCheck(ModifyPasswordSr(oldPassword, newPassword, settings));
            }

            /// <summary>
            /// 准备修改手机号，将会向新手机号发送验证码
            /// </summary>
            /// <param name="newPhoneNumber">新手机号</param>
            /// <param name="oldPassword">原手机密码</param>
            /// <param name="settings">调用设置</param>
            /// <returns>哈希加密之后的验证码</returns>
            public ServiceResult<string> PreModifyPhoneNumberSr(string oldPassword, string newPhoneNumber, CallingSettings settings = null)
            {
                var sr = UserService.PreModifyPhoneNumber(Sc, new User_PreModifyPhoneNumber_Request() {
                    NewPhoneNumber = newPhoneNumber, Password = oldPassword,
                }, settings);

                return CreateSr(sr, r => r.HashVerifyCode);
            }

            /// <summary>
            /// 准备修改手机号，将会向新手机号发送验证码
            /// </summary>
            /// <param name="newPhoneNumber">新手机号</param>
            /// <param name="oldPassword">验证码</param>
            /// <param name="settings">调用设置</param>
            /// <returns>哈希加密之后的验证码</returns>
            public string PreModifyPhoneNumber(string oldPassword, string newPhoneNumber, CallingSettings settings = null)
            {
                return InvokeWithCheck(PreModifyPhoneNumberSr(oldPassword, newPhoneNumber, settings));
            }

            /// <summary>
            /// 修改手机号
            /// </summary>
            /// <param name="newPhoneNumber">新手机号</param>
            /// <param name="newPassword">新密码</param>
            /// <param name="verifyCode">验证码</param>
            /// <param name="settings">调用设置</param>
            public ServiceResult<Sid> ModifyPhoneNumberSr(string newPhoneNumber, string newPassword, string verifyCode, CallingSettings settings = null)
            {
                var sr = UserService.ModifyPhoneNumber(Sc, new User_ModifyPhoneNumber_Request() {
                    NewPhoneNumber = newPhoneNumber, NewPassword = newPassword, VerifyCode = verifyCode
                }, settings);

                return CreateSr(sr, r => r.Sid);
            }

            /// <summary>
            /// 修改手机号
            /// </summary>
            /// <param name="sid">安全码</param>
            /// <param name="newPhoneNumber">新手机号</param>
            /// <param name="newPassword">新密码</param>
            /// <param name="verifyCode">验证码</param>
            /// <param name="settings">调用设置</param>
            public Sid ModifyPhoneNumber(string newPhoneNumber, string newPassword, string verifyCode, CallingSettings settings = null)
            {
                return InvokeWithCheck(ModifyPhoneNumberSr(newPhoneNumber, newPassword, verifyCode, settings));
            }

            /// <summary>
            /// 修改用户信息
            /// </summary>
            /// <param name="name">姓名</param>
            /// <param name="vCard">需要修改的用户信息</param>
            /// <param name="settings">调用设置</param>
            public ServiceResult ModifyMyInfoSr(string name, string vCard, CallingSettings settings = null)
            {
                return UserService.ModifyMyInfo(Sc, new User_ModifyMyInfo_Request {
                    Name = name, VCard = vCard,
                }, settings);
            }

            /// <summary>
            /// 修改用户信息
            /// </summary>
            /// <param name="name">姓名</param>
            /// <param name="vCard">需要修改的用户信息</param>
            /// <param name="settings">调用设置</param>
            public void ModifyMyInfo(string name, string vCard, CallingSettings settings = null)
            {
                InvokeWithCheck(ModifyMyInfoSr(name, vCard, settings));
            }

            /// <summary>
            /// 获取用户的信息
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<UserInfo> GetMyInfoSr(CallingSettings settings = null)
            {
                var sr = UserService.GetMyInfo(Sc, settings);
                return CreateSr(sr, r => r.Info);
            }

            /// <summary>
            /// 获取用户信息
            /// </summary>
            /// <param name="settings"></param>
            /// <returns></returns>
            public UserInfo GetMyInfo(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetMyInfoSr(settings));
            }

            /// <summary>
            /// 获取验证码
            /// </summary>
            /// <param name="phoneNumber"></param>
            /// <param name="for"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult SendVerifyCodeSr(string phoneNumber, string @for, CallingSettings settings = null)
            {
                return UserService.SendVerifyCode(Sc, new User_SendVerifyCode_Request() {
                    PhoneNumber = phoneNumber, For = @for
                }, settings);
            }

            /// <summary>
            /// 获取验证码
            /// </summary>
            /// <param name="phoneNumber"></param>
            /// <param name="for"></param>
            /// <param name="setting"></param>
            /// <returns></returns>
            public void SendVerifyCode(string phoneNumber, string @for, CallingSettings setting = null)
            {
                InvokeWithCheck(SendVerifyCodeSr(phoneNumber, @for, setting));
            }

            /// <summary>
            /// 设置用户状态
            /// </summary>
            /// <param name="status">状态文本</param>
            /// <param name="statusUrl">状态链接</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SetStatusSr(string status, string statusUrl = "", CallingSettings settings = null)
            {
                return UserService.SetStatus(Sc,
                    new User_SetStatus_Request() { Status = status, StatusUrl = statusUrl }, settings);
            }

            /// <summary>
            /// 设置用户状态
            /// </summary>
            /// <param name="status">状态文本</param>
            /// <param name="statusUrl">状态链接</param>
            /// <param name="settings">调用设置</param>
            public void SetStatus(string status, string statusUrl = "", CallingSettings settings = null)
            {
                InvokeWithCheck(SetStatusSr(status, statusUrl, settings));
            }

            /// <summary>
            /// 发送消息
            /// </summary>
            /// <param name="method">消息方法</param>
            /// <param name="to">接收者</param>
            /// <param name="property">消息属性</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SendMessageSr(string content, string to,
                MsgProperty property = MsgProperty.Default, CallingSettings settings = null)
            {
                return UserService.SendMessage(Sc, new User_SendMessage_Request() {
                    Content = content, To = to, Property = property
                }, settings);
            }

            /// <summary>
            /// 发送消息
            /// </summary>
            /// <param name="content">消息内容</param>
            /// <param name="to">接收者</param>
            /// <param name="messageId">消息标识</param>
            /// <param name="property">消息属性</param>
            /// <param name="settings">调用设置</param>
            public void SendMessage(string content, string to,
                MsgProperty property = MsgProperty.Default, CallingSettings settings = null)
            {
                InvokeWithCheck(SendMessageSr(content, to, property, settings));
            }

            /// <summary>
            /// 向粉丝发送消息
            /// </summary>
            /// <param name="content">消息内容</param>
            /// <param name="property">消息属性</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SendMessageToFollowersSr(string content,
                MsgProperty property = MsgProperty.Default, CallingSettings settings = null)
            {
                return SendMessageSr(content, Users.FromMyFollowers().ToString(), property, settings);
            }

            /// <summary>
            /// 向粉丝发送消息
            /// </summary>
            /// <param name="content">消息内容</param>
            /// <param name="property">消息属性</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void SendMessageToFollowers(string content,
                MsgProperty property = MsgProperty.Default, CallingSettings settings = null)
            {
                InvokeWithCheck(SendMessageToFollowersSr(content, property, settings));
            }

            /// <summary>
            /// 向粉丝发送消息
            /// </summary>
            /// <param name="content">消息内容</param>
            /// <param name="property">消息属性</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult SendMessageToFollowingsSr(string content,
                MsgProperty property = MsgProperty.Default, CallingSettings settings = null)
            {
                return SendMessageSr(content, Users.FromMyFollowings().ToString(), property, settings);
            }

            /// <summary>
            /// 向粉丝发送消息
            /// </summary>
            /// <param name="content">内容</param>
            /// <param name="property">属性</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public void SendMessageToFollowings(string content,
                MsgProperty property = MsgProperty.Default, CallingSettings settings = null)
            {
                InvokeWithCheck(SendMessageToFollowingsSr(content, property, settings));
            }

            /*
            /// <summary>
            /// 添加联系人
            /// </summary>
            /// <param name="usernames">联系人</param>
            /// <param name="settings">调用设置</param>
            /// <returns>已经注册的联系人的UserId</returns>
            public ServiceResult<int[]> AddContactsSr(string[] usernames, CallingSettings settings = null)
            {
                var sr = UserService.AddContacts(Sc,
                    new User_AddContacts_Request() { UserNames = usernames }, settings);

                return CreateSr(sr, r => r.UserIds);
            }

            /// <summary>
            /// 添加联系人
            /// </summary>
            /// <param name="usernames">联系人</param>
            /// <param name="settings">调用设置</param>
            /// <returns>已经注册的联系人的UserId</returns>
            public int[] AddContacts(string[] usernames, CallingSettings settings = null)
            {
                return InvokeWithCheck(AddContactsSr(usernames, settings));
            }

            /// <summary>
            /// 添加联系人
            /// </summary>
            /// <param name="username">联系人</param>
            /// <param name="settings">调用设置</param>
            /// <returns>该联系人的UserId，若该联系人尚未注册，则返回0</returns>
            public ServiceResult<int> AddContactSr(string username, CallingSettings settings = null)
            {
                var sr = UserService.AddContact(Sc, new User_AddContact_Request { UserName = username }, settings);
                return CreateSr(sr, r => r.UserId);
            }

            /// <summary>
            /// 添加联系人
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="settings">调用设置</param>
            /// <returns>该联系人的UserId，若该联系人尚未注册，则返回0</returns>
            public int AddContact(string username, CallingSettings settings = null)
            {
                return InvokeWithCheck(AddContactSr(username, settings));
            }*/

            /// <summary>
            /// 以组合方式批量添加联系人
            /// </summary>
            /// <param name="usernames">联系人</param>
            /// <param name="settings">调用设置</param>
            /// <returns>联系人的UserId</returns>
            public ServiceResult<string[]> AddContactsSr(string[] usernames, CallingSettings settings = null)
            {
                var sr = UserService.AddContacts(Sc, new User_AddContacts_Request { UserNames = usernames }, settings);
                return CreateSr(sr, r => r.UserIds);
            }

            /// <summary>
            /// 以组合方式批量添加联系人
            /// </summary>
            /// <param name="usernames">联系人</param>
            /// <param name="settings">调用设置</param>
            /// <returns>联系人的UserId</returns>
            public string[] AddContacts(string[] usernames, CallingSettings settings = null)
            {
                return InvokeWithCheck(AddContactsSr(usernames, settings));
            }

            /// <summary>
            /// 删除联系人
            /// </summary>
            /// <param name="usernames">用户名</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult RemoveContactsSr(string[] usernames, CallingSettings settings = null)
            {
                return UserService.RemoveContacts(Sc,
                    new User_RemoveContacts_Request() { UserNames = usernames }, settings);
            }

            /// <summary>
            /// 删除联系人
            /// </summary>
            /// <param name="usernames">用户名</param>
            /// <param name="settings">调用设置</param>
            public void RemoveContacts(string[] usernames, CallingSettings settings = null)
            {
                InvokeWithCheck(RemoveContactsSr(usernames, settings));
            }

            /// <summary>
            /// 删除联系人
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult RemoveContactSr(string username, CallingSettings settings = null)
            {
                return UserService.RemoveContact(Sc, new User_RemoveContact_Request { UserName = username }, settings);
            }

            /// <summary>
            /// 删除联系人
            /// </summary>
            /// <param name="username"></param>
            /// <param name="settings"></param>
            public void RemoveContact(string username, CallingSettings settings = null)
            {
                InvokeWithCheck(RemoveContactSr(username, settings));
            }

            /// <summary>
            /// 更新联系人
            /// </summary>
            /// <param name="usernames">用户名</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult UpdateContactSr(string[] usernames, CallingSettings settings = null)
            {
                return UserService.UpdateContactList(Sc, 
                    new User_UpdateContactList_Request() { UserNames = usernames }, settings);
            }

            /// <summary>
            /// 更新联系人
            /// </summary>
            /// <param name="usernames">用户名</param>
            /// <param name="settings">调用设置</param>
            public void UpdateContact(string[] usernames, CallingSettings settings = null)
            {
                InvokeWithCheck(UpdateContactSr(usernames, settings));
            }

            /// <summary>
            /// 获取联系人列表
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<UserIdName[]> GetContactListSr(CallingSettings settings = null)
            {
                var sr = UserService.GetContactList(Sc, settings);
                return CreateSr(sr, r => r.Users);
            }

            /// <summary>
            /// 获取联系人列表
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public UserIdName[] GetContactList(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetContactListSr(settings));
            }

            /// <summary>
            /// 获取关注列表
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<int[]> GetFollowingsSr(CallingSettings settings = null)
            {
                var sr = UserService.GetFollowings(Sc, new User_GetFollowings_Request(), settings);
                return CreateSr(sr, r => r.UserIds);
            }

            /// <summary>
            /// 获取关注列表
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public int[] GetFollowings(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetFollowingsSr(settings));
            }

            /// <summary>
            /// 获取用户状态
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="since">起始变化时间</param>
            /// <param name="settings">调用设置</param>
            /// <returns>用户状态</returns>
            public ServiceResult<UserStatus[]> GetStatusSr(int[] userIds, CallingSettings settings = null)
            {
                var sr = UserService.GetStatus(Sc, new User_GetStatus_Request { UserIds = userIds });
                return CreateSr(sr, r => r.Status);
            }

            /// <summary>
            /// 获取用户的状态
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="since">起始变化时间</param>
            /// <param name="settings">调用设置</param>
            /// <returns>用户状态</returns>
            public UserStatus[] GetStatus(int[] userIds, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetStatusSr(userIds, settings));
            }

            /// <summary>
            /// 获取用户的状态
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns>用户状态</returns>
            public ServiceResult<UserStatus> GetStatusSr(int userId, CallingSettings settings = null)
            {
                var sr = GetStatusSr(new[] { userId }, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取用户的状态
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns>用户状态</returns>
            public UserStatus GetStatus(int userId, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetStatusSr(userId, settings));
            }

            /*
            /// <summary>
            /// 获取用户的状态
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="since">起始变化时间</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<UserStatus[]> GetStatusSr(int[] userIds, DateTime since = default(DateTime), CallingSettings settings = null)
            {
                return GetStatusSr(userIds), since, settings);
            }

            /// <summary>
            /// 获取用户的状态
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="since">起始变化时间</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public UserStatus[] GetStatus(int[] userIds, DateTime since = default(DateTime), CallingSettings settings = null)
            {
                return InvokeWithCheck(GetStatusSr(userIds, since, settings));
            }

            /// <summary>
            /// 获取用户的状态
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="since">起始变化时间</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<UserStatus> GetStatusSr(int userId, DateTime since = default(DateTime), CallingSettings settings = null)
            {
                var sr = GetStatusSr(new[] { userId }, since, settings);
                return CreateSr(sr, r => r.FirstOrDefault());
            }

            /// <summary>
            /// 获取用户的状态
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="since">起始变化时间</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public UserStatus GetStatus(int userId, DateTime since = default(DateTime), CallingSettings settings = null)
            {
                return InvokeWithCheck(GetStatusSr(userId, since, settings));
            }

            /// <summary>
            /// 获取所关注的用户的状态
            /// </summary>
            /// <param name="since"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public ServiceResult<UserStatus[]> GetFollowingsStatusSr(DateTime since = default(DateTime), CallingSettings settings = null)
            {
                return GetStatusSr(Users.FromMyFollowings().ToString(), since, settings);
            }

            /// <summary>
            /// 获取所关注用户的状态
            /// </summary>
            /// <param name="since"></param>
            /// <param name="settings"></param>
            /// <returns></returns>
            public UserStatus[] GetFollowingsStatus(DateTime since = default(DateTime), CallingSettings settings = null)
            {
                return InvokeWithCheck(GetFollowingsStatusSr(since, settings));
            }*/

            /// <summary>
            /// 获取粉丝列表
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<int[]> GetFollowersSr(CallingSettings settings = null)
            {
                var sr = UserService.GetFollowers(Sc, settings);
                return CreateSr(sr, r => r.UserIds);
            }

            /// <summary>
            /// 获取粉丝列表
            /// </summary>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public int[] GetFollowers(CallingSettings settings = null)
            {
                return InvokeWithCheck(GetFollowersSr(settings));
            }

            /// <summary>
            /// 获取指定的联系人信息
            /// </summary>
            /// <param name="users">联系人</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<UserInfo[]> GetUserInfosSr(string users, CallingSettings settings = null)
            {
                var sr = UserService.GetUserInfos(Sc, new User_GetUserInfos_Request { Users = users }, settings);
                return CreateSr(sr, r => r.Infos);
            }

            /// <summary>
            /// 获取指定的联系人信息
            /// </summary>
            /// <param name="users">联系人</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public UserInfo[] GetUserInfos(string users, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserInfosSr(users, settings));
            }

            /// <summary>
            /// 获取指定的联系人信息
            /// </summary>
            /// <param name="users">联系人</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<UserInfo[]> GetUserInfosSr(Users users, CallingSettings settings = null)
            {
                return GetUserInfosSr(users == null ? "" : users.ToString(), settings);
            }

            /// <summary>
            /// 获取指定的联系人信息
            /// </summary>
            /// <param name="users">联系人</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public UserInfo[] GetUserInfos(Users users, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserInfosSr(users, settings));
            }

            /// <summary>
            /// 获取用户信息
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns>用户信息</returns>
            public ServiceResult<UserInfo> GetUserInfoSr(int userId, CallingSettings settings = null)
            {
                var sr = UserService.GetUserInfo(Sc, new User_GetUserInfo_Request { UserId = userId }, settings);
                return CreateSr(sr, r => r.Info);
            }

            /// <summary>
            /// 获取用户信息
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns>用户信息</returns>
            public UserInfo GetUserInfo(int userId, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserInfoSr(userId, settings));
            }

            /// <summary>
            /// 将UserName转换为UserId
            /// </summary>
            /// <param name="usernames">用户名</param>
            /// <param name="settings">调用设置</param>
            /// <returns>用户ID</returns>
            public ServiceResult<int[]> GetUserIdsSr(string[] usernames, CallingSettings settings = null)
            {
                var sr = UserService.GetUserIds(Sc, new User_GetUserIds_Request { UserNames = usernames }, settings);
                return CreateSr(sr, r => r.UserIds);
            }

            /// <summary>
            /// 将UserName转换为UserId
            /// </summary>
            /// <param name="usernames">用户名</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public int[] GetUserIds(string[] usernames, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserIdsSr(usernames, settings));
            }

            /// <summary>
            /// 将UserName转换为UserId
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public ServiceResult<int> GetUserIdSr(string username, CallingSettings settings = null)
            {
                var sr = UserService.GetUserId(Sc, new User_GetUserId_Request { UserName = username }, settings);
                return CreateSr(sr, r => r.UserId);
            }

            /// <summary>
            /// 将UserName转换为UserId
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="settings">调用设置</param>
            /// <returns></returns>
            public int GetUserId(string username, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserIdSr(username, settings));
            }

            /// <summary>
            /// 根据用户ID批量获取用户名
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns>用户名</returns>
            public ServiceResult<string[]> GetUserNamesSr(int[] userIds, CallingSettings settings = null)
            {
                var sr = UserService.GetUserNames(Sc, new User_GetUserNames_Request { UserIds = userIds }, settings);
                return CreateSr(sr, r => r.UserNames);
            }

            /// <summary>
            /// 根据用户ID批量获取用户名
            /// </summary>
            /// <param name="userIds">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns>用户名</returns>
            public string[] GetUserNames(int[] userIds, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserNamesSr(userIds, settings));
            }

            /// <summary>
            /// 根据用户ID获取用户名
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns>用户名</returns>
            public ServiceResult<string> GetUserNameSr(int userId, CallingSettings settings = null)
            {
                var sr = UserService.GetUserName(Sc, new User_GetUserName_Request { UserId = userId }, settings);
                return CreateSr(sr, r => r.UserName);
            }

            /// <summary>
            /// 根据用户ID获取用户名
            /// </summary>
            /// <param name="userId">用户ID</param>
            /// <param name="settings">调用设置</param>
            /// <returns>用户名</returns>
            public string GetUserName(int userId, CallingSettings settings = null)
            {
                return InvokeWithCheck(GetUserNameSr(userId, settings));
            }
        }
    }
}
