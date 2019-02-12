using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    /// <summary>
    /// 一元运算符
    /// </summary>
    abstract class UnitaryCalculate
    {
        /// <summary>
        /// 逻辑非
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract Value Not(Value value);

        /// <summary>
        /// 取负
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract Value Minus(Value value);
    }
}
