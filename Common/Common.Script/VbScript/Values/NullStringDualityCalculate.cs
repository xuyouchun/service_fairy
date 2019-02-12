using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [DualityCalculate(typeof(NullValue), typeof(string))]
    [DualityCalculate(typeof(string), typeof(NullValue))]
    class NullStringDualityCalculate : DefaultDualityCalculate
    {
        public override Value Addition(Value value1, Value value2)
        {
            return value1.IsEmpty() ? value2 : value1;
        }

        public override Value Equality(Value value1, Value value2)
        {
            return false;
        }

        public override Value GreaterOrEqual(Value value1, Value value2)
        {
            return value2.IsEmpty();
        }

        public override Value GreaterThan(Value value1, Value value2)
        {
            return value2.IsEmpty();
        }

        public override Value LessOrEqual(Value value1, Value value2)
        {
            return value1.IsEmpty();
        }

        public override Value LessThan(Value value1, Value value2)
        {
            return value1.IsEmpty();
        }

        public override Value InEnqulity(Value value1, Value value2)
        {
            return true;
        }
    }
}
