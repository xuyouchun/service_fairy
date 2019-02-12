using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.ExpressionAnalysers
{
    class DynamicExpressonAnalyserStack : ExpressionAnalyserStack
    {
        public DynamicExpressonAnalyserStack(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override Expression[] GetExpressions()
        {
            return new Expression[] { new DynamicExpression(Name, GetAllExpressions()) };
        }
    }
}
