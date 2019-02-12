using System;
using System.Collections.Generic;
using System.Text;
using VbValue = global::Common.Script.VbScript.Value;

namespace Common.Script.VbScript.Values
{
    [DualityCalculate(typeof(byte), typeof(string))]
    [DualityCalculate(typeof(int), typeof(string))]
    [DualityCalculate(typeof(long), typeof(string))]
    [DualityCalculate(typeof(float), typeof(string))]
    [DualityCalculate(typeof(double), typeof(string))]
    [DualityCalculate(typeof(decimal), typeof(string))]
    class NumberStringDualityCalculate : DefaultDualityCalculate
    {
        private readonly StringNumberDualityCalculate _Calculate = new StringNumberDualityCalculate();

        public override Value Addition(Value value1, Value value2)
        {
            return _Addition(value1, (string)value2);
        }

        private Value _Addition(Value v1, string s)
        {
            Value result;
            if (StringNumberDualityCalculate.TryParseToValue(s, v1, out result))
                return result;

            return v1.ToString() + s;
        }

        public override Value GreaterOrEqual(Value value1, Value value2)
        {
            return !_Calculate.GreaterOrEqual(value2, value1);
        }

        public override Value GreaterThan(Value value1, Value value2)
        {
            return !_Calculate.GreaterThan(value2, value1);
        }

        public override Value LessOrEqual(Value value1, Value value2)
        {
            return !_Calculate.LessOrEqual(value2, value1);
        }

        public override Value LessThan(Value value1, Value value2)
        {
            return !_Calculate.LessThan(value2, value1);
        }

        public override Value Equality(Value value1, Value value2)
        {
            return _Calculate.Equality(value2, value1);
        }

        public override VbValue InEnqulity(Value value1, Value value2)
        {
            return _Calculate.InEnqulity(value2, value1);
        }
    }
}
