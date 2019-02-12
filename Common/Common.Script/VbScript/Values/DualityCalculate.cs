using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    /// <summary>
    /// 二元计算器
    /// </summary>
    abstract class DualityCalculate
    {
        /// <summary>
        /// 大于比较
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public abstract Value GreaterThan(Value value1, Value value2);

        /// <summary>
        /// 小于比较
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public abstract Value LessThan(Value value1, Value value2);

        /// <summary>
        /// 相等比较
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public abstract Value Equality(Value value1, Value value2);

        /// <summary>
        /// 不相等比较
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public abstract Value InEnqulity(Value value1, Value value2);

        /// <summary>
        /// 小于或等于
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public abstract Value LessOrEqual(Value value1, Value value2);

        /// <summary>
        /// 大于或等于
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public abstract Value GreaterOrEqual(Value value1, Value value2);

        /// <summary>
        /// 加法
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public abstract Value Addition(Value value1, Value value2);

        /// <summary>
        /// 减法
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public abstract Value Subtraction(Value value1, Value value2);

        /// <summary>
        /// 乘法
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public abstract Value Multiplication(Value value1, Value value2);

        /// <summary>
        /// 除法
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public abstract Value Division(Value value1, Value value2);

        /// <summary>
        /// 取余
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public abstract Value Modulus(Value value1, Value value2);

        /// <summary>
        /// 逻辑与
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public abstract Value And(Value value1, Value value2);

        /// <summary>
        /// 逻辑或
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public abstract Value Or(Value value1, Value value2);

        /// <summary>
        /// 求幂
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public abstract Value Pow(Value value1, Value value2);

        /// <summary>
        /// 异或
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public abstract Value Xor(Value value1, Value value2);
    }
}
