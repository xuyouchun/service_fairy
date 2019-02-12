using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Statements.StatementAnalysers.StatementAnalyserStackContexts
{
    static class StatementAnalyserStackContextUtility
    {
        public static ArrayExpression ConvertToArrayExpression(DynamicExpression dynamicExp)
        {
            string arrayName = dynamicExp.Name;
            foreach (Expression exp in dynamicExp.Parameters)
            {
                ValueExpression constExp = exp as ValueExpression;
                if (constExp == null)
                    throw new ScriptException("定义数组时指定的参数错误:" + exp);

                if (!constExp.Value.IsInteger())
                    throw new ScriptException("定义数组时指定的参数并非整型:" + constExp.Value);

                if (constExp.Value.GetValue<int>() < 0)
                    throw new ScriptException("定义数组时指定的参数为负数:" + constExp.Value);
            }

            return new ArrayExpression(dynamicExp.Name, dynamicExp.Parameters);
        }

        public static VariableExpression ConvertToVariableExpression(Expression expression)
        {
            VariableExpression varExp;
            DynamicExpression dynamicExp;

            if ((varExp = expression as VariableExpression) != null)
            {
                return varExp;
            }
            else if ((dynamicExp = expression as DynamicExpression) != null)
            {
                return ConvertToArrayExpression(dynamicExp);
            }

            return null;
        }
    }
}
