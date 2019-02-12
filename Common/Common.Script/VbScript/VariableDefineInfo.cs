using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript
{
    public class VariableDefineInfo
    {
        internal VariableDefineInfo(VariableExpression variable, Expression value)
        {
            Variable = variable;
            Value = value;
        }

        internal VariableExpression Variable { get; private set; }

        public Expression Value { get; private set; }

        public string Name { get { return Variable.Name; } }

        public override string ToString()
        {
            if (Value == null)
                return Variable.ToString();

            return string.Format("{0} = {1}", Variable, Value);
        }

        public static VariableDefineInfo Create(string name)
        {
            return new VariableDefineInfo(new VariableExpression(name), null);
        }
    }
}
