using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [DualityCalculate(typeof(byte), typeof(byte))]
    class ByteDualityCalculate : DefaultDualityCalculate
    {
        public override Value Addition(Value value1, Value value2)
        {
            int result = (byte)value1 + (byte)value2;
            if (result <= byte.MaxValue && result >= byte.MinValue)
                return (byte)result;

            return result;
        }

        public override Value Subtraction(Value value1, Value value2)
        {
            int result = (byte)value1 - (byte)value2;
            if (result <= byte.MaxValue && result >= byte.MinValue)
                return (byte)result;

            return result;
        }

        public override Value Multiplication(Value value1, Value value2)
        {
            int result = (byte)value1 * (byte)value2;
            if (result <= byte.MaxValue && result >= byte.MinValue)
                return (byte)result;

            return result;
        }

        public override Value Division(Value value1, Value value2)
        {
            byte v2 = (byte)value2;
            if (v2 == 0)
                throw new ScriptRuntimeException("除数为0");

            double result = (double)(byte)value1 / v2;
            if (result == (long)result)
                return (byte)result;

            return result;
        }

        public override Value Modulus(Value value1, Value value2)
        {
            return (byte)((byte)value1 % (byte)value2);
        }

        public override Value Equality(Value value1, Value value2)
        {
            return (byte)value1 == (byte)value2;
        }

        public override Value InEnqulity(Value value1, Value value2)
        {
            return (byte)value1 != (byte)value2;
        }

        public override Value GreaterThan(Value value1, Value value2)
        {
            return (byte)value1 > (byte)value2;
        }

        public override Value GreaterOrEqual(Value value1, Value value2)
        {
            return (byte)value1 >= (byte)value2;
        }

        public override Value LessThan(Value value1, Value value2)
        {
            return (byte)value1 < (byte)value2;
        }

        public override Value LessOrEqual(Value value1, Value value2)
        {
            return (byte)value1 <= (byte)value2;
        }

        public override Value And(Value value1, Value value2)
        {
            return (byte)value1 & (byte)value2;
        }

        public override Value Or(Value value1, Value value2)
        {
            return (byte)value1 | (byte)value2;
        }

        public override Value Pow(Value value1, Value value2)
        {
            int result = (int)Math.Pow((byte)value1, (byte)value2);
            if (result <= byte.MaxValue && result >= byte.MinValue)
                return (byte)result;

            return result;
        }

        public override Value Xor(Value value1, Value value2)
        {
            return (byte)value1 ^ (byte)value2;
        }
    }
}
