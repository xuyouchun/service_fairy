using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions
{
    /// <summary>
    /// 常量表达式
    /// </summary>
    class ConstValueExpression : Expression
    {
        public ConstValueExpression(string name, Value value)
        {
            if (object.ReferenceEquals(value, null))
                throw new ArgumentNullException("value");

            Name = name;
            Value = value;
        }

        /// <summary>
        /// 常量名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 常量值
        /// </summary>
        public Value Value { get; private set; }

        protected override Value OnExecute(IExpressionContext context)
        {
            return Value;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
