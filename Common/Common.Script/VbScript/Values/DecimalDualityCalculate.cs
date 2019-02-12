using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [DualityCalculate(typeof(decimal), typeof(decimal))]
    [DualityCalculate(typeof(decimal), typeof(double))]
    [DualityCalculate(typeof(double), typeof(decimal))]
    [DualityCalculate(typeof(decimal), typeof(float))]
    [DualityCalculate(typeof(float), typeof(decimal))]
    [DualityCalculate(typeof(decimal), typeof(long))]
    [DualityCalculate(typeof(long), typeof(decimal))]
    [DualityCalculate(typeof(decimal), typeof(int))]
    [DualityCalculate(typeof(int), typeof(decimal))]
    [DualityCalculate(typeof(decimal), typeof(short))]
    [DualityCalculate(typeof(short), typeof(decimal))]
    [DualityCalculate(typeof(decimal), typeof(byte))]
    [DualityCalculate(typeof(byte), typeof(decimal))]
    class DecimalDualityCalculate : DefaultDualityCalculate
    {
        public override Value Addition(Value value1, Value value2)
        {
            return checked((decimal)value1 + (decimal)value2);
        }

        public override Value Subtraction(Value value1, Value value2)
        {
            return checked((decimal)value1 - (decimal)value2);
        }

        public override Value Multiplication(Value value1, Value value2)
        {
            return checked((decimal)value1 * (decimal)value2);
        }

        public override Value Division(Value value1, Value value2)
        {
            decimal v2 = (decimal)value2;
            if (v2 == 0)
                throw new ScriptRuntimeException("除数为0");

            return checked((decimal)value1 / v2);
        }

        public override Value Modulus(Value value1, Value value2)
        {
            return checked((decimal)value1 % (decimal)value2);
        }

        public override Value GreaterOrEqual(Value value1, Value value2)
        {
            return (decimal)value1 >= (decimal)value2;
        }

        public override Value GreaterThan(Value value1, Value value2)
        {
            return (decimal)value1 > (decimal)value2;
        }

        public override Value LessOrEqual(Value value1, Value value2)
        {
            return (decimal)value1 <= (decimal)value2;
        }

        public override Value LessThan(Value value1, Value value2)
        {
            return (decimal)value1 < (decimal)value2;
        }

        public override Value Equality(Value value1, Value value2)
        {
            return (decimal)value1 == (decimal)value2;
        }

        public override Value InEnqulity(Value value1, Value value2)
        {
            return (decimal)value1 != (decimal)value2;
        }

        public override Value Pow(Value value1, Value value2)
        {
            return checked((decimal)Math.Pow((double)value1, (double)value2));
        }

        public override Value And(Value value1, Value value2)
        {
            decimal v1 = (decimal)value1, v2 = (decimal)value2;
            if((long)v1==v1 && (long)v2==v2)
            {
                return (long)v1 & (long)v2;
            }

            return base.And(value1, value2);
        }

        public override Value Or(Value value1, Value value2)
        {
            decimal v1 = (decimal)value1, v2 = (decimal)value2;
            if ((long)v1 == v1 && (long)v2 == v2)
            {
                return (long)v1 | (long)v2;
            }

            return base.Or(value1, value2);
        }

        public override Value Xor(Value value1, Value value2)
        {
            decimal v1 = (decimal)value1, v2 = (decimal)value2;
            if ((long)v1 == v1 && (long)v2 == v2)
            {
                return (long)v1 ^ (long)v2;
            }

            return base.Xor(value1, value2);
        }
    }
}
