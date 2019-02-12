using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [UnitaryCalculate(typeof(short))]
    class ShortUnitaryCalculate : DefaultUnitaryCalculate
    {
        public override Value Minus(Value value)
        {
            return -(short)value;
        }

        public override Value Not(Value value)
        {
            return ~(short)value;
        }
    }
}
