using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [DualityCalculate(typeof(NullValue), typeof(NullValue))]
    class NullDualityCalculate : DefaultDualityCalculate
    {
        public override Value Equality(Value value1, Value value2)
        {
            return true;
        }

        public override Value InEnqulity(Value value1, Value value2)
        {
            return false;
        }

        public override Value GreaterOrEqual(Value value1, Value value2)
        {
            return true;
        }

        public override Value GreaterThan(Value value1, Value value2)
        {
            return false;
        }

        public override Value LessOrEqual(Value value1, Value value2)
        {
            return true;
        }

        public override Value LessThan(Value value1, Value value2)
        {
            return false;
        }
    }
}
