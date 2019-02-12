using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Contracts
{
    /// <summary>
    /// 用于记录操作的结果
    /// </summary>
    public class OperateResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">结果码</param>
        /// <param name="message">结果描述信息</param>
        public OperateResult(int code, string message = "")
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// 操作码
        /// </summary>
        public int Code { get; private set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; private set; }

        #region Equals ...

        public override int GetHashCode()
        {
            return Code;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(OperateResult))
                return false;

            return ((OperateResult)obj).Code == Code;
        }

        public static bool operator ==(OperateResult obj1, OperateResult obj2)
        {
            if ((object)obj1 == null || (object)obj2 == null)
                return (object)obj1 == null && (object)obj2 == null;

            return obj1.Code == obj2.Code;
        }

        public static bool operator !=(OperateResult obj1, OperateResult obj2)
        {
            return !(obj1 == obj2);
        }

        public static implicit operator bool(OperateResult op)
        {
            if ((object)op == null)
                return false;

            return op.Code >= 0;
        }

        #endregion

        /// <summary>
        /// 成功
        /// </summary>
        public static readonly OperateResult Success = new OperateResult(OperateCodes.Success);

        /// <summary>
        /// 失败
        /// </summary>
        public static readonly OperateResult Fail = new OperateResult(OperateCodes.Fail);
    }

    /// <summary>
    /// 带有附加数据的操作结果
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class OperateResult<TData> : OperateResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">结果码</param>
        /// <param name="message">信息</param>
        /// <param name="data">数据</param>
        public OperateResult(int code, string message = "", TData data = default(TData))
            : base(code, message)
        {
            Data = data;
        }

        /// <summary>
        /// 附加数据
        /// </summary>
        public TData Data { get; private set; }

        #region Equals ...

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(OperateResult<TData> obj1, OperateResult<TData> obj2)
        {
            if ((object)obj1 == null || (object)obj2 == null)
                return (object)obj1 == null && (object)obj2 == null;

            return obj1.Code == obj2.Code;
        }

        public static bool operator !=(OperateResult<TData> obj1, OperateResult<TData> obj2)
        {
            return !(obj1 == obj2);
        }

        public static implicit operator bool(OperateResult<TData> op)
        {
            if ((object)op == null)
                return false;

            return op.Code >= 0;
        }

        #endregion

        /// <summary>
        /// 成功
        /// </summary>
        public new static readonly OperateResult<TData> Success = new OperateResult<TData>(OperateCodes.Success);

        /// <summary>
        /// 失败
        /// </summary>
        public new static readonly OperateResult<TData> Fail = new OperateResult<TData>(OperateCodes.Fail);
    }
}
