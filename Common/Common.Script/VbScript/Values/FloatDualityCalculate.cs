using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [DualityCalculate(typeof(float), typeof(float))]
    [DualityCalculate(typeof(float), typeof(long))]
    [DualityCalculate(typeof(long), typeof(float))]
    [DualityCalculate(typeof(float), typeof(int))]
    [DualityCalculate(typeof(int), typeof(float))]
    [DualityCalculate(typeof(float), typeof(short))]
    [DualityCalculate(typeof(short), typeof(float))]
    [DualityCalculate(typeof(float), typeof(byte))]
    [DualityCalculate(typeof(byte), typeof(float))]
    class FloatDualityCalculate : DoubleDualityCalculate
    {
        private bool _CheckRange(double result)
        {
            return result >= float.MinValue && result <= float.MaxValue;
        }

        public override Value Addition(Value value1, Value value2)
        {
            double r = (float)value1 + (float)value2;
            if (_CheckRange(r))
                return (float)r;

            return r;
        }

        public override Value Subtraction(Value value1, Value value2)
        {
            double r = (float)value1 - (float)value2;
            if (_CheckRange(r))
                return (float)r;

            return r;
        }

        public override Value Multiplication(Value value1, Value value2)
        {
            double r = (float)value1 * (float)value2;
            if (_CheckRange(r))
                return (float)r;

            return r;
        }

        public override Value Division(Value value1, Value value2)
        {
            float v2 = (float)value2;
            if (v2 == 0)
                throw new ScriptRuntimeException("除数为0");

            double r = (float)value1 / v2;
            if (_CheckRange(r))
                return (float)r;

            return r;
        }

        public override Value Modulus(Value value1, Value value2)
        {
            return (float)value1 % (float)value2;
        }

        public override Value Equality(Value value1, Value value2)
        {
            return (float)value1 == (float)value2;
        }

        public override Value InEnqulity(Value value1, Value value2)
        {
            return (float)value1 != (float)value2;
        }

        public override Value GreaterOrEqual(Value value1, Value value2)
        {
            return (float)value1 >= (float)value2;
        }

        public override Value GreaterThan(Value value1, Value value2)
        {
            return (float)value1 > (float)value2;
        }

        public override Value LessOrEqual(Value value1, Value value2)
        {
            return (float)value1 <= (float)value2;
        }

        public override Value LessThan(Value value1, Value value2)
        {
            return (float)value1 < (float)value2;
        }

        public override Value Pow(Value value1, Value value2)
        {
            double result = Math.Pow((float)value1, (float)value2);
            if (_CheckRange(result))
                return (float)result;

            return result;
        }

        public override Value And(Value value1, Value value2)
        {
            float v1 = (float)value1, v2 = (float)value2;
            int lV1 = (int)v1, lV2 = (int)v2;
            if (v1 == lV1 && v2 == lV2)
                return lV1 & lV2;

            return base.And(value1, value2);
        }

        public override Value Or(Value value1, Value value2)
        {
            float v1 = (float)value1, v2 = (float)value2;
            int lV1 = (int)v1, lV2 = (int)v2;
            if (v1 == lV1 && v2 == lV2)
                return lV1 | lV2;

            return base.Or(value1, value2);
        }

        public override Value Xor(Value value1, Value value2)
        {
            float v1 = (float)value1, v2 = (float)value2;
            int lV1 = (int)v1, lV2 = (int)v2;
            if (v1 == lV1 && v2 == lV2)
                return lV1 ^ lV2;

            return base.Xor(value1, value2);
        }
    }
}
