using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [DualityCalculate(typeof(short), typeof(short))]
    [DualityCalculate(typeof(short), typeof(byte))]
    [DualityCalculate(typeof(byte), typeof(short))]
    class ShortDualityCaculate : DefaultDualityCalculate
    {
        private bool _CheckRange(int result)
        {
            return result >= int.MinValue && result <= int.MaxValue;
        }

        public override Value Addition(Value value1, Value value2)
        {
            int result = (int)value1 + (short)value2;
            if (_CheckRange(result))
                return (short)result;

            return result;
        }

        public override Value Subtraction(Value value1, Value value2)
        {
            int result = (int)value1 - (short)value2;
            if (_CheckRange(result))
                return (short)result;

            return result;
        }

        public override Value Multiplication(Value value1, Value value2)
        {
            int result = (int)value1 * (short)value2;
            if (_CheckRange(result))
                return (short)result;

            return result;
        }

        public override Value Division(Value value1, Value value2)
        {
            double result = (double)value1 / (short)value2;
            int intResult = (int)result;
            if (result == intResult)
            {
                if (_CheckRange(intResult))
                    return (short)intResult;

                return intResult;
            }

            return result;
        }

        public override Value Modulus(Value value1, Value value2)
        {
            return (short)value1 % (short)value2;
        }

        public override Value Equality(Value value1, Value value2)
        {
            return (short)value1 == (short)value2;
        }

        public override Value InEnqulity(Value value1, Value value2)
        {
            return (short)value1 != (short)value2;
        }

        public override Value GreaterOrEqual(Value value1, Value value2)
        {
            return (short)value1 >= (short)value2;
        }

        public override Value GreaterThan(Value value1, Value value2)
        {
            return (short)value1 > (short)value2;
        }

        public override Value LessOrEqual(Value value1, Value value2)
        {
            return (short)value1 <= (short)value2;
        }

        public override Value LessThan(Value value1, Value value2)
        {
            return (short)value1 < (short)value2;
        }

        public override Value Pow(Value value1, Value value2)
        {
            int result = (int)Math.Pow((short)value1, (short)value2);
            if (_CheckRange(result))
                return (short)result;

            return result;
        }

        public override Value And(Value value1, Value value2)
        {
            return (short)value1 & (short)value2;
        }

        public override Value Or(Value value1, Value value2)
        {
            return (short)value1 | (short)value2;
        }

        public override Value Xor(Value value1, Value value2)
        {
            return (short)value1 ^ (short)value2;
        }
    }
}
