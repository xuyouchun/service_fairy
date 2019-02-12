using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts.Service;
using System.Collections;

namespace ServiceFairy.Entities
{
    /// <summary>
    /// 验证实体类参数的有效性
    /// </summary>
    public static class EntityValidate
    {
        /// <summary>
        /// 创建参数异常
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ServiceException CreateArgumentException(string errorMsg, string name = "")
        {
            if (!string.IsNullOrEmpty(name))
                errorMsg = name + ": " + errorMsg;

            return new ServiceException(ServerErrorCode.ArgumentError, errorMsg);
        }

        /// <summary>
        /// 创建参数为空的异常
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ServiceException CreateArgumentNullException(string name = "")
        {
            return new ServiceException(ServerErrorCode.ArgumentError, name + ": 参数为空");
        }

        /// <summary>
        /// 验证参数是否为空
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="name"></param>
        public static void ValidateNull(object argument, string name = "")
        {
            if (argument == null)
                throw CreateArgumentNullException(name);
        }

        /// <summary>
        /// 验证参数是否为空引用或空串
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="name"></param>
        public static void ValidateNullOrEmpty(object argument, string name = "")
        {
            if (argument == null || (argument is string && string.IsNullOrEmpty((string)argument)))
                throw CreateArgumentNullException(name);

            if (argument is IList && ((IList)argument).Count == 0)
                throw CreateArgumentNullException(name);
        }

        /// <summary>
        /// 验证参数是否为空引用或空白串
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="name"></param>
        public static void ValidateNullOrWhiteSpace(object argument, string name = "")
        {
            if (argument == null || (argument is string && string.IsNullOrWhiteSpace((string)argument)))
                throw CreateArgumentNullException(name);
        }

        /// <summary>
        /// 验证手机号码格式是否正确
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="name"></param>
        /// <param name="length">长度</param>
        public static void ValidatePhoneNumber(string phoneNumber, string name, int length = 11)
        {
            ValidateNullOrWhiteSpace(phoneNumber, name);

            if (phoneNumber.Length != length || phoneNumber[0] != '1' || !phoneNumber.All(c => char.IsNumber(c)))
                throw CreateArgumentException("手机号码格式不正确", name);
        }

        /// <summary>
        /// 验证验证码是否正确
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="length"></param>
        public static void ValidateVerifyCode(string code, string name, int length = 4)
        {
            ValidateNullOrWhiteSpace(code, name);

            if (code.Length != length || !code.All(c => char.IsNumber(c)))
                throw CreateArgumentException("验证码不正确", name);
        }
    }
}
