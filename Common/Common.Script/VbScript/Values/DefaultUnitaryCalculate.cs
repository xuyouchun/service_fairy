using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    class DefaultUnitaryCalculate : UnitaryCalculate
    {
        protected static ScriptException CreateException(string opName, Value value)
        {
            return new ScriptException(string.Format("无法对类型“{0}”做“{1}”计算",
                value.GetValueTypeString(), opName));
        }

        public override Value Not(Value value)
        {
            throw CreateException("取反", value);
        }

        public override Value Minus(Value value)
        {
            throw CreateException("取负", value);
        }
    }
}
