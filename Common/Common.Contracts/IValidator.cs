using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Contracts
{
    /// <summary>
    /// 验证器
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// 执行验证
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        ValidateResult Validate(object value);
    }

    /// <summary>
    /// 验证器的结果
    /// </summary>
    public class ValidateResult
    {
        public ValidateResult(bool succeed, string message)
        {
            Succeed = succeed;
            Message = message;
        }

        /// <summary>
        /// 是否验证成功
        /// </summary>
        public bool Succeed { get; private set; }

        /// <summary>
        /// 验证信息
        /// </summary>
        public string Message { get; private set; }

        public override string ToString()
        {
            return Message;
        }

        public static implicit operator bool(ValidateResult r)
        {
            return r == null ? false : r.Succeed;
        }

        public static readonly ValidateResult SucceedResult = new ValidateResult(true, "");

        public static readonly ValidateResult FailedResult = new ValidateResult(false, "");
    }

    /// <summary>
    /// 验证器
    /// </summary>
    public class ValidatorCollection : IValidator
    {
        public ValidatorCollection(IValidator[] validators)
        {
            Contract.Requires(validators != null);
            _validator = validators;
        }

        private readonly IValidator[] _validator;

        public ValidateResult Validate(object value)
        {
            for (int k = 0; k < _validator.Length; k++)
            {
                ValidateResult r = _validator[k].Validate(value);
                if (!r)
                    return r;
            }

            return ValidateResult.SucceedResult;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class Validators
    {
        /// <summary>
        /// 通过代理来创建验证器
        /// </summary>
        /// <param name="validateFunc"></param>
        /// <param name="failedMsg"></param>
        /// <param name="succeedMsg"></param>
        /// <returns></returns>
        public static IValidator Create(Func<object, bool> validateFunc, string failedMsg, string succeedMsg = "")
        {
            return new _Validator(validateFunc, failedMsg, succeedMsg);
        }

        /// <summary>
        /// 通过代理来创建验证器
        /// </summary>
        /// <param name="validateFunc"></param>
        /// <returns></returns>
        public static IValidator Create(Func<object, string> validateFunc, string succeedMsg = "")
        {
            return new _Validator(validateFunc, succeedMsg);
        }

        /// <summary>
        /// 字符串不为空的验证器
        /// </summary>
        /// <param name="failedMsg"></param>
        /// <param name="succeedMsg"></param>
        /// <returns></returns>
        public static IValidator NotEmpty(string failedMsg, string succeedMsg = "")
        {
            return Create(v => v != null && (!(v is string) || !string.IsNullOrEmpty((string)v)), failedMsg, succeedMsg);
        }

        #region Class _Validator ...

        class _Validator : IValidator
        {
            public _Validator(Func<object, bool> validateFunc, string failedMsg, string succeedMsg)
                : this((value => validateFunc(value) ? null : failedMsg), succeedMsg)
            {

            }

            public _Validator(Func<object, string> validateFunc, string succeedMsg)
            {
                _validateFunc = validateFunc;
                _succeedMsg = succeedMsg;
            }

            private readonly Func<object, string> _validateFunc;
            private readonly string _succeedMsg;

            public ValidateResult Validate(object value)
            {
                string failedMsg = _validateFunc(value);
                return (failedMsg == null) ? new ValidateResult(true, _succeedMsg) : new ValidateResult(true, failedMsg);
            }
        }

        #endregion
    }
}
