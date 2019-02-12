using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [DualityCalculate(typeof(double), typeof(double))]
    [DualityCalculate(typeof(double), typeof(float))]
    [DualityCalculate(typeof(float), typeof(double))]
    [DualityCalculate(typeof(double), typeof(long))]
    [DualityCalculate(typeof(long), typeof(double))]
    [DualityCalculate(typeof(double), typeof(int))]
    [DualityCalculate(typeof(int), typeof(double))]
    [DualityCalculate(typeof(double), typeof(short))]
    [DualityCalculate(typeof(short), typeof(double))]
    [DualityCalculate(typeof(double), typeof(byte))]
    [DualityCalculate(typeof(byte), typeof(double))]
    class DoubleDualityCalculate : DefaultDualityCalculate
    {
        public override Value Addition(Value value1, Value value2)
        {
            return (double)value1 + (double)value2;
        }

        public override Value Subtraction(Value value1, Value value2)
        {
            return (double)value1 - (double)value2;
        }

        public override Value Multiplication(Value value1, Value value2)
        {
            return (double)value1 * (double)value2;
        }

        public override Value Division(Value value1, Value value2)
        {
            double v2 = (double)value2;
            if (v2 == 0)
                throw new ScriptRuntimeException("除数为0");

            return (double)value1 / v2;
        }

        public override Value Modulus(Value value1, Value value2)
        {
            return (double)value1 % (double)value2;
        }

        public override Value Equality(Value value1, Value value2)
        {
            return (double)value1 == (double)value2;
        }

        public override Value InEnqulity(Value value1, Value value2)
        {
            return (double)value1 != (double)value2;
        }

        public override Value GreaterOrEqual(Value value1, Value value2)
        {
            return (double)value1 >= (double)value2;
        }

        public override Value GreaterThan(Value value1, Value value2)
        {
            return (double)value1 > (double)value2;
        }

        public override Value LessOrEqual(Value value1, Value value2)
        {
            return (double)value1 <= (double)value2;
        }

        public override Value LessThan(Value value1, Value value2)
        {
            return (double)value1 < (double)value2;
        }

        public override Value Pow(Value value1, Value value2)
        {
            return Math.Pow((double)value1, (double)value2);
        }

        public override Value And(Value value1, Value value2)
        {
            double v1 = (double)value1, v2 = (double)value2;
            long lV1 = (long)v1, lV2 = (long)v2;
            if (v1 == lV1 && v2 == lV2)
                return lV1 & lV2;

            return base.And(value1, value2);
        }

        public override Value Or(Value value1, Value value2)
        {
            double v1 = (double)value1, v2 = (double)value2;
            long lV1 = (long)v1, lV2 = (long)v2;
            if (v1 == lV1 && v2 == lV2)
                return lV1 | lV2;

            return base.Or(value1, value2);
        }

        public override Value Xor(Value value1, Value value2)
        {
            double v1 = (double)value1, v2 = (double)value2;
            long lV1 = (long)v1, lV2 = (long)v2;
            if (v1 == lV1 && v2 == lV2)
                return lV1 ^ lV2;

            return base.Xor(value1, value2);
        }
    }
}
