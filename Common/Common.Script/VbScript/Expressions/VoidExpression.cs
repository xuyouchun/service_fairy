using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Client.Script.Expressions
{
    /// <summary>
    /// 空的表达式
    /// </summary>
    class VoidExpression : Expression
    {
        protected override Value OnExecute(IExpressionContext context)
        {
            return Value.Void;
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
