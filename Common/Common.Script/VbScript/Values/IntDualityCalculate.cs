using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [DualityCalculate(typeof(int), typeof(int))]
    [DualityCalculate(typeof(int), typeof(byte))]
    [DualityCalculate(typeof(byte), typeof(int))]
    [DualityCalculate(typeof(int), typeof(short))]
    [DualityCalculate(typeof(short), typeof(int))]
    class IntDualityCalculate : DefaultDualityCalculate
    {
        private bool _CheckRange(long result)
        {
            return result >= int.MinValue && result <= int.MaxValue;
        }

        public override Value Addition(Value value1, Value value2)
        {
            long result = (long)value1 + (int)value2;
            if(_CheckRange(result))
                return (int)result;

            return result;
        }

        public override Value Subtraction(Value value1, Value value2)
        {
            long result = (long)value1 - (int)value2;
            if (_CheckRange(result))
                return (int)result;

            return result;
        }

        public override Value Multiplication(Value value1, Value value2)
        {
            long result = (long)value1 * (int)value2;
            if (_CheckRange(result))
                return (int)result;

            return result;
        }

        public override Value Division(Value value1, Value value2)
        {
            int v2 = (int)value2;
            if (v2 == 0)
                throw new ScriptRuntimeException("除数为0");

            double result = (double)value1 / v2;
            long lnResult = (long)result;
            if (result == lnResult)
            {
                if (_CheckRange(lnResult))
                    return (int)lnResult;

                return lnResult;
            }

            return result;
        }

        public override Value Modulus(Value value1, Value value2)
        {
            return (int)value1 % (int)value2;
        }

        public override Value Equality(Value value1, Value value2)
        {
            return (int)value1 == (int)value2;
        }

        public override Value InEnqulity(Value value1, Value value2)
        {
            return (int)value1 != (int)value2;
        }

        public override Value GreaterOrEqual(Value value1, Value value2)
        {
            return (int)value1 >= (int)value2;
        }

        public override Value GreaterThan(Value value1, Value value2)
        {
            return (int)value1 > (int)value2;
        }

        public override Value LessOrEqual(Value value1, Value value2)
        {
            return (int)value1 <= (int)value2;
        }

        public override Value LessThan(Value value1, Value value2)
        {
            return (int)value1 < (int)value2;
        }

        public override Value Pow(Value value1, Value value2)
        {
            long result = (long)Math.Pow((int)value1, (int)value2);
            if (_CheckRange(result))
                return (int)result;

            return result;
        }

        public override Value And(Value value1, Value value2)
        {
            return (int)value1 & (int)value2;
        }

        public override Value Or(Value value1, Value value2)
        {
            return (int)value1 | (int)value2;
        }

        public override Value Xor(Value value1, Value value2)
        {
            return (int)value1 ^ (int)value2;
        }
    }
}
