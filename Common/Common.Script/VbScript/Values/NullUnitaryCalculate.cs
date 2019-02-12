using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [UnitaryCalculate(typeof(NullValue))]
    class NullUnitaryCalculate : DefaultUnitaryCalculate
    {
        public override Value Not(Value value)
        {
            return true;
        }
    }
}
