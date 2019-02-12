using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Data.SqlExpressions
{
    /// <summary>
    /// 函数表达式
    /// </summary>
    public class FunctionSqlExpression : SqlExpressionBase
    {
        internal FunctionSqlExpression(string funcName, ArraySqlExpression parameters)
        {
            Contract.Requires(funcName != null);

            FunctionName = funcName;
            Parameters = parameters ?? ArraySqlExpression.Empty;
        }

        /// <summary>
        /// 函数名　
        /// </summary>
        public string FunctionName { get; private set; }

        /// <summary>
        /// 参数
        /// </summary>
        public ArraySqlExpression Parameters { get; private set; }

        protected override string OnToString(ISqlExpressionContext context)
        {
            return FunctionName + Parameters.ToString(context);
        }

        public override IEnumerator<SqlExpression> GetEnumerator()
        {
            yield return Parameters;
        }
    }
}
