using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Client.Script.Expressions.ExpressionAnalysers
{
    class FunctionExpressonAnalyserStack : ExpressionAnalyserStack
    {
        public FunctionExpressonAnalyserStack(string functionName)
        {
            FunctionName = functionName;
        }

        public string FunctionName { get; private set; }

        public override Expression[] GetExpressions()
        {
            return new Expression[] { new FunctionExpression(FunctionName, GetAllExpressions()) };
        }
    }
}
