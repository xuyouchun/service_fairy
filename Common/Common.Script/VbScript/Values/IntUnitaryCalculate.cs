using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [UnitaryCalculate(typeof(int))]
    class IntUnitaryCalculate : DefaultUnitaryCalculate
    {
        public override Value Minus(Value value)
        {
            return -(int)value;
        }

        public override Value Not(Value value)
        {
            return ~(int)value;
        }
    }
}
