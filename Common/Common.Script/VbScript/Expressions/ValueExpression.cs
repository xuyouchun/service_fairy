using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions
{
    /// <summary>
    /// 常量表达式
    /// </summary>
    class ValueExpression : Expression
    {
        public ValueExpression(Value value)
        {
            if (object.ReferenceEquals(value, null))
                throw new ArgumentNullException("value");

            Value = value;
        }

        public Value Value { get; private set; }

        protected override Value OnExecute(IExpressionContext context)
        {
            return Value;
        }

        public override string ToString()
        {
            object v = Value.InnerValue;
            if (v == null || v.GetType() != typeof(string))
                return Value.ToString();

            return "\"" + v + "\"";
        }
    }
}
