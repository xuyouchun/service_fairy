using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    class DefaultDualityCalculate : DualityCalculate
    {
        protected static ScriptException CreateException(string opName, Value value1, Value value2)
        {
            return new ScriptException(string.Format("无法在类型“{0}”与“{1}”之间做“{2}”运算",
                value1.GetValueTypeString(), value2.GetValueTypeString(), opName));
        }

        public override Value GreaterThan(Value value1, Value value2)
        {
            throw CreateException("大于", value1, value2);
        }

        public override Value LessThan(Value value1, Value value2)
        {
            throw CreateException("小于", value1, value2);
        }

        public override Value Equality(Value value1, Value value2)
        {
            if (value1.IsEmpty() || value2.IsEmpty())
                return value1.IsEmpty() && value2.IsEmpty();

            throw CreateException("等于", value1, value2);
        }

        public override Value InEnqulity(Value value1, Value value2)
        {
            if (value1.IsEmpty() || value2.IsEmpty())
                return !(value1.IsEmpty() && value2.IsEmpty());

            throw CreateException("不等于", value1, value2);
        }

        public override Value LessOrEqual(Value value1, Value value2)
        {
            throw CreateException("小于等于", value1, value2);
        }

        public override Value GreaterOrEqual(Value value1, Value value2)
        {
            throw CreateException("大于等于", value1, value2);
        }

        public override Value Addition(Value value1, Value value2)
        {
            throw CreateException("加法", value1, value2);
        }

        public override Value Subtraction(Value value1, Value value2)
        {
            throw CreateException("减法", value1, value2);
        }

        public override Value Multiplication(Value value1, Value value2)
        {
            throw CreateException("乘法", value1, value2);
        }

        public override Value Division(Value value1, Value value2)
        {
            throw CreateException("除法", value1, value2);
        }

        public override Value Modulus(Value value1, Value value2)
        {
            throw CreateException("取余", value1, value2);
        }

        public override Value And(Value value1, Value value2)
        {
            throw CreateException("与", value1, value2);
        }

        public override Value Or(Value value1, Value value2)
        {
            throw CreateException("或", value1, value2);
        }

        public override Value Pow(Value value1, Value value2)
        {
            throw CreateException("求幂", value1, value2);
        }

        public override Value Xor(Value value1, Value value2)
        {
            throw CreateException("异或", value1, value2);
        }
    }
}
