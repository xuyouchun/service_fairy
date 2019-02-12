using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [UnitaryCalculate(typeof(long))]
    class LongUnitaryCalculate : DefaultUnitaryCalculate
    {
        public override Value Minus(Value value)
        {
            return -(long)value;
        }

        public override Value Not(Value value)
        {
            return ~(long)value;
        }
    }
}
