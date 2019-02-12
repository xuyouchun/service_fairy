using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [UnitaryCalculate(typeof(byte))]
    class ByteUnitaryCalculate : DefaultUnitaryCalculate
    {
        public override Value Minus(Value value)
        {
            return -(int)value;
        }

        public override Value Not(Value value)
        {
            return ~(byte)value;
        }
    }
}
