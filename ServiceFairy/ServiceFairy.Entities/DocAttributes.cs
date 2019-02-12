using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using Common.Package;
using System.Reflection;
using Common.Utility;
using System.Diagnostics.Contracts;
using ServiceFairy.Entities.User;
using Common;

namespace ServiceFairy.Entities
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class CommonDocAttributeBase : DocAttribute, IDocSummaryProvider, IDocRemarksProvider,
        IDocPossibleValueProvider, IDocExampleProvider, IDocCustomFlagProvider
    {
        public CommonDocAttributeBase(Enum field)
        {
            FieldInfo fi = ReflectionUtility.GetEnumFieldInfo(field);
            if (fi != null)
            {
                _summary = DocUtility.GetSummary(fi);
                _remarks = DocUtility.GetRemarks(fi);
                _example = DocUtility.GetExample(fi);
                _possibleValues = DocUtility.GetPossiableValues(fi, false);
                _customFlags = DocUtility.GetCustomFlags(fi);
            }
        }

        private readonly string _summary, _remarks, _example;
        private readonly IDictionary<string, string> _customFlags;
        private readonly PossibleValue[] _possibleValues;

        public string GetRemarks()
        {
            return _remarks;
        }

        public string GetSummary()
        {
            return _summary;
        }

        public PossibleValue[] GetPossibleValues()
        {
            return _possibleValues;
        }

        public string GetExample()
        {
            return _example;
        }

        public override int GetRank()
        {
            return -1;
        }

        public string[] GetAllKeys()
        {
            return _customFlags == null ? Array<string>.Empty : _customFlags.Keys.ToArray();
        }

        public string GetFlag(string key)
        {
            return _customFlags == null || key == null ? null : _customFlags.GetOrDefault(key);
        }
    }

    /// <summary>
    /// 用于标注公共字段的文档
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class SysFieldDocAttribute : CommonDocAttributeBase
    {
        public SysFieldDocAttribute(SysField field)
            : base(field)
        {

        }
    }

    /// <summary>
    /// 字段
    /// </summary>
    public enum SysField
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Summary("用户名"), Remarks("可以为手机号、邮件、微博帐号等所有可能的形式，如果为手机号，格式为“国际代号 手机号”，例如：“+86 13717674043”")]
        UserName,

        /// <summary>
        /// 用户名的集合
        /// </summary>
        [Summary("用户名的集合"), Remarks("在JSON格式中为[username1, username2, ...]的形式")]
        UserNames,

        /// <summary>
        /// 用户ID与用户名的组合
        /// </summary>
        [Summary("用户ID与用户名的组合")]
        UserIdName,

        /// <summary>
        /// 密码
        /// </summary>
        [Summary("密码")]
        Password,

        /// <summary>
        /// 联系人
        /// </summary>
        [Summary("联系人"), Remarks("手机通讯录中的所有成员均为联系人，无论该成员是否已注册")]
        Contact,

        /// <summary>
        /// 用户ID
        /// </summary>
        [Summary("用户ID，为一个整型数字")]
        UserId,

        /// <summary>
        /// 用户ID的集合
        /// </summary>
        [Summary("用户ID的集合")]
        UserIds,

        /// <summary>
        /// 用户集合
        /// </summary>
        [Summary("用户集合"), Remarks(DocTexts.UsersRemarks)]
        Users,

        /// <summary>
        /// 起始时间
        /// </summary>
        [Summary("起始时间"), Remarks(DocTexts.DateTimeRemarks)]
        Since,

        /// <summary>
        /// 昵称
        /// </summary>
        [Summary("昵称")]
        NickName,

        /// <summary>
        /// 手机号
        /// </summary>
        [Summary("手机号"), Remarks("格式为“国际区号 手机号”，例如：“+86 13717674043”")]
        PhoneNumber,

        /// <summary>
        /// 用户的详细信息
        /// </summary>
        [Summary("用户的详细信息"), Remarks("用键值对的形式定义的详细信息"), PossibleValues("Name:用户姓名")]
        UserDetailInfo,

        /// <summary>
        /// 用户的详细信息
        /// </summary>
        [Summary("用户的详细信息"), Remarks("用户详细信息的项可由客户端来定义")]
        UserInfo,

        /// <summary>
        /// 验证码
        /// </summary>
        [Summary("验证码"), Remarks("通过短信发送到手机，为随机生成的四位整型")]
        VerifyCode,

        /// <summary>
        /// 是否自动登录
        /// </summary>
        [Summary("是否自动登录"), Remarks("用于指定在注册、修改密码、重置密码等接口中，是否在完成上述任务之后自动登录，返回安全码")]
        AutoLogin,

        /// <summary>
        /// 安全码
        /// </summary>
        [Summary("安全码"), Remarks("服务器端随机生成的一串字符串，在登录之后下发到客户端，客户端携带该Sid进行下一步的请求，即为登录状态")]
        Sid,

        /// <summary>
        /// 消息内容
        /// </summary>
        [Summary("消息内容"), Remarks("为自定义的文本格式")]
        UserMessageContent,

        [Summary("消息属性"), Remarks(@"可选的值有：
Default：默认，当消息发送失败时重试，用户不在线时将其持久，待用户上线时再发送给用户
NotReliable：不可靠消息，消息发送失败时不重试，用户不在线时不发送给用户，也不做持久化")]
        MessageProperty,

        /// <summary>
        /// 发送验证码的用途
        /// </summary>
        [Summary("验证码的用途")]
        [PossibleValues(typeof(VerifyCodeFor))]
        SendVerifyCodeFor,

        /// <summary>
        /// 用户状态文本
        /// </summary>
        [Summary("联系人状态"), Remarks("用于标明用户当前状态的一串自定义格式的文本，例如：开会中")]
        ContactStatusText,

        /// <summary>
        /// 联系人状态所对应的Url
        /// </summary>
        [Summary("联系人状态所对应的Url"), Remarks("通常用于在查看联系人状态文本时，点击可以进入一个链接地址")]
        ContactStatusUrl,

        /// <summary>
        /// 用户状态
        /// </summary>
        [Summary("联系人状态"), Remarks("用户表示当前用户的个性签名、是否在线等信息")]
        ContactStatus,

        /// <summary>
        /// 变化时间
        /// </summary>
        [Summary("变化时间"), Remarks(DocTexts.DateTimeRemarks)]
        ChangedTime,

        /// <summary>
        /// 时间
        /// </summary>
        [Summary("时间"), Remarks(DocTexts.DateTimeRemarks)]
        Time,

        /// <summary>
        /// 用户是否在线
        /// </summary>
        [Summary("用户是否在线")]
        UserOnline,

        /// <summary>
        /// 群组唯一标识，为一个32位的整数
        /// </summary>
        [Summary("群组唯一标识，为一个32位的整数"), Remarks(DocTexts.GroupIdRemarks)]
        GroupId,

        /// <summary>
        /// 群组ID的集合
        /// </summary>
        [Summary("群组ID的数组"), Remarks(DocTexts.GroupIdRemarks)]
        GroupIds,

        /// <summary>
        /// 群组详细信息
        /// </summary>
        [Summary("群组详细信息"), Remarks("以键值对定义的详细信息"), PossibleValues("Name:群组名称")]
        GroupDetailInfo,

        /// <summary>
        /// 群组成员
        /// </summary>
        [Summary("群组成员"), Remarks(DocTexts.UsersRemarks)]
        GroupMembers,

        /// <summary>
        /// 群组成员ID
        /// </summary>
        [Summary("群组成员ID"), Remarks("群组成员的用户ID，为一个32位的整数")]
        GroupMemberId,

        /// <summary>
        /// 群组创建者
        /// </summary>
        [Summary("群组创建者")]
        GroupCreator,

        /// <summary>
        /// 群组名称
        /// </summary>
        [Summary("群组名称"), Remarks("可以在创建群组的时候可以指定群组的名称，在调用GetGroups等接口时也可以返回群组的名称。")]
        GroupName,

        /// <summary>
        /// 群组信息
        /// </summary>
        [Summary("群组信息")]
        GroupInfo,

        /// <summary>
        /// 群组信息数组
        /// </summary>
        [Summary("群组信息数组")]
        GroupInfos,

        /// <summary>
        /// 群组成员信息
        /// </summary>
        [Summary("群组成员信息")]
        FcGroupMemberInfo,

        /// <summary>
        /// 群组ID与群组成员的组合
        /// </summary>
        [Summary("群组ID与群组成员的组合")]
        GroupMemberItem,

        /// <summary>
        /// 群组消息内容
        /// </summary>
        [Summary("消息内容")]
        GroupMessageContent,

        /// <summary>
        /// 群组信息版本号
        /// </summary>
        [Summary("群组信息版本号"), Remarks("为一个64位整型数字，它实际上是当前时间的Ticks数（万分之一毫秒）")]
        GroupVersion,

        /// <summary>
        /// 群组ID与版本号的键值对
        /// </summary>
        [Summary("群组ID与版本号的键值对"), Remarks("键为群组ID（int32），值为版本号(int64)")]
        GroupVersionDict,
    }

    public static class DocTexts
    {
        public const string DateTimeRemarks = @"JSON有自己的标准时间格式，为ISO8601标准，jQuery中时间也是这种格式：
例：/Date(1234234123413453+0800)/，
括号中的数字，前半部分为1970-01-01到现在的毫秒数，后面为时区，中间可以为“+”或“-”，表示时区偏移的方向。
例如：+0800为东八区";

        public const string GroupIdRemarks = @"大部分群组服务的接口都需要群组ID。
用户创建群组时会获得该ID，也可以通过调用GetGroups接口来获取当前用户所拥有的群组。";

        public const string UsersRemarks = @"以多种方式指定的用户集合，格式为：“前缀:内容”，前缀表示内容的类型，例如“un:+86 13717674043”表示用户名为+86 13717674043的用户。
多个项之间用逗号隔开，例如：“un:+86 13717674043,+86 13717674044”，前缀包括：
un:用户名
u:用户ID（用户ID为一个整型数字）
fr:所有粉丝
fg:所有关注
g:群组成员
online:所有在线用户
offline:所有不在线用户
如果当前为登录状态，用“me”可以表示当前登录用户，例如：“fg:me”表示我的所有关注
可以嵌套使用，例如：“fg:un:+86 13717674043”，表示用户名为“+86 13717674043”的所有关注
多个部分之间可用分号隔开，例如：“fg:me;fr:me”，表示我的所有关注与粉丝
前面加“-”表示不包括：例如：“fg:me;-offline”表示我的所有关注成员中去掉不在线的一部分，即我的所有在线的关注成员
";
    }

    /// <summary>
    /// 用于标识用户名
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class NewUserNameAttribute : CustomFlagAttribute
    {
        public NewUserNameAttribute()
            : base("field:new_username")
        {

        }
    }

    /// <summary>
    /// 用于标识密码
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class NewPasswordAttribute : CustomFlagAttribute
    {
        public NewPasswordAttribute()
            : base("field:new_password")
        {

        }
    }
}
