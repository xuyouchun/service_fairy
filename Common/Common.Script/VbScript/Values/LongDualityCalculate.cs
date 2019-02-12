using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [DualityCalculate(typeof(long), typeof(long))]
    [DualityCalculate(typeof(long), typeof(int))]
    [DualityCalculate(typeof(int), typeof(long))]
    [DualityCalculate(typeof(long), typeof(short))]
    [DualityCalculate(typeof(short), typeof(long))]
    [DualityCalculate(typeof(long), typeof(byte))]
    [DualityCalculate(typeof(byte), typeof(long))]
    class LongDualityCalculate : DefaultDualityCalculate
    {
        public override Value Addition(Value value1, Value value2)
        {
            try
            {
                return checked((long)value1 + (long)value2);
            }
            catch (OverflowException)
            {
                return checked((double)value1 + (double)value2);
            }
        }

        public override Value Subtraction(Value value1, Value value2)
        {
            try
            {
                return checked((long)value1 - (long)value2);
            }
            catch (OverflowException)
            {
                return checked((double)value1 - (double)value2);
            }
        }

        public override Value Multiplication(Value value1, Value value2)
        {
            try
            {
                return checked((long)value1 * (long)value2);
            }
            catch (OverflowException)
            {
                return checked((double)value1 * (double)value2);
            }
        }

        public override Value Division(Value value1, Value value2)
        {
            long v2 = (long)value2;
            if (v2 == 0)
                throw new ScriptRuntimeException("除数为0");

            try
            {
                double result = checked((double)value1 / v2);
                long lnResult = (long)result;
                if (lnResult == result)
                    return lnResult;

                return result;
            }
            catch (OverflowException)
            {
                return checked((double)value1 / (double)value2);
            }
        }

        public override Value Modulus(Value value1, Value value2)
        {
            try
            {
                return checked((long)value1 % (long)value2);
            }
            catch (OverflowException)
            {
                return checked((double)value1 % (double)value2);
            }
        }

        public override Value Equality(Value value1, Value value2)
        {
            return (long)value1 == (long)value2;
        }

        public override Value InEnqulity(Value value1, Value value2)
        {
            return (long)value1 != (long)value2;
        }

        public override Value GreaterOrEqual(Value value1, Value value2)
        {
            return (long)value1 >= (long)value2;
        }

        public override Value GreaterThan(Value value1, Value value2)
        {
            return (long)value1 > (long)value2;
        }

        public override Value LessOrEqual(Value value1, Value value2)
        {
            return (long)value1 <= (long)value2;
        }

        public override Value LessThan(Value value1, Value value2)
        {
            return (long)value1 < (long)value2;
        }

        public override Value Pow(Value value1, Value value2)
        {
            double result = Math.Pow((long)value1, (long)value2);
            if (result <= long.MaxValue && result >= long.MinValue)
                return (long)result;

            return result;
        }

        public override Value And(Value value1, Value value2)
        {
            return (long)value1 & (long)value2;
        }

        public override Value Or(Value value1, Value value2)
        {
            return (long)value1 | (long)value2;
        }

        public override Value Xor(Value value1, Value value2)
        {
            return (long)value1 ^ (long)value2;
        }
    }
}
