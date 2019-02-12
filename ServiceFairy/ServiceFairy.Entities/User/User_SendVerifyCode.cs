using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Common.Contracts.Service;
using Common.Contracts;
using Common;

namespace ServiceFairy.Entities.User
{
    /// <summary>
    /// 获取验证码－请求
    /// </summary>
    [Serializable, DataContract, Summary("获取验证码－请求")]
    public class User_SendVerifyCode_Request : RequestEntity
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [DataMember, SysFieldDoc(SysField.PhoneNumber)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 用途
        /// </summary>
        [DataMember, SysFieldDoc(SysField.SendVerifyCodeFor)]
        public string For { get; set; }

        public override void Validate()
        {
            base.Validate();

            EntityValidate.ValidateNullOrWhiteSpace(PhoneNumber, "PhoneNumber");
            EntityValidate.ValidateNullOrWhiteSpace(For, "For");

            if (!VerifyCodeFor.IsCorrect(For))
                throw EntityValidate.CreateArgumentException("验证码的用途无效", "For");
        }
    }

    /// <summary>
    /// 验证码的用途
    /// </summary>
    public static class VerifyCodeFor
    {
        /// <summary>
        /// 注册
        /// </summary>
        [Desc("注册")]
        public const string Register = "reg";

        /// <summary>
        /// 激活手机号
        /// </summary>
        [Desc("激活手机号")]
        public const string ActiviteMobile = "act";

        /// <summary>
        /// 修改手机号码
        /// </summary>
        [Desc("修改手机号")]
        public const string ModifyPhoneNumber = "mpn";

        /// <summary>
        /// 重置密码
        /// </summary>
        [Desc("重置密码")]
        public const string ResetPassword = "rpw";

        public static bool IsCorrect(string @for)
        {
            return @for == Register || @for == ActiviteMobile || @for == ModifyPhoneNumber || @for == ResetPassword;
        }

        /// <summary>
        /// 获取短信模板
        /// </summary>
        /// <param name="for"></param>
        /// <returns></returns>
        public static string GetTemplate(string @for)
        {
            switch (@for)
            {
                case Register:
                    return "拨号精灵 验证码: {0}";

                case ActiviteMobile:
                    return "拨号精灵 验证码: {0}";

                case ResetPassword:
                    return "拨号精灵 验证码: {0}";

                case ModifyPhoneNumber:
                    return "拨号精灵 验证码: {0}";
            }

            throw new ServiceException(ServerErrorCode.ArgumentError, "错误的短信模板");
        }
    }

    /// <summary>
    /// 获取验证码－应答
    /// </summary>
    [Serializable, DataContract, Summary("获取验证码－应答")]
    public class User_SendVerifyCode_Reply : UserReplyEntity
    {
        /// <summary>
        /// VerifyCode的哈希码
        /// </summary>
        [DataMember]
        public string HashVerifyCode { get; set; }
    }
}
