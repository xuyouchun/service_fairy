using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [DualityCalculate(typeof(string), typeof(string))]
    class StringDualityCalculate : DefaultDualityCalculate
    {
        public override Value Addition(Value value1, Value value2)
        {
            return value1.ToString() + value2.ToString();
        }

        public override Value GreaterThan(Value value1, Value value2)
        {
            return string.Compare((string)value1, (string)value2) > 0;
        }

        public override Value GreaterOrEqual(Value value1, Value value2)
        {
            return string.Compare((string)value1, (string)value2) >= 0;
        }

        public override Value LessThan(Value value1, Value value2)
        {
            return string.Compare((string)value1, (string)value2) < 0;
        }

        public override Value LessOrEqual(Value value1, Value value2)
        {
            return string.Compare((string)value1, (string)value2) <= 0;
        }

        public override Value Equality(Value value1, Value value2)
        {
            return (string)value1 == (string)value2;
        }

        public override Value InEnqulity(Value value1, Value value2)
        {
            return (string)value1 != (string)value2;
        }
    }
}
