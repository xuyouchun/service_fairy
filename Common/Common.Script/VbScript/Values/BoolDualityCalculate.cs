using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [DualityCalculate(typeof(bool), typeof(bool))]
    class BoolDualityCalculate : DefaultDualityCalculate
    {
        public override Value And(Value value1, Value value2)
        {
            return (bool)value1 && (bool)value2;
        }

        public override Value Or(Value value1, Value value2)
        {
            return (bool)value1 || (bool)value2;
        }
    }
}
