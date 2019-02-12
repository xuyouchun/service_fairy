using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [UnitaryCalculate(typeof(bool))]
    class BoolUnitaryCalculate : DefaultUnitaryCalculate
    {
        public override Value Not(Value value)
        {
            return !(bool)value;
        }
    }
}
