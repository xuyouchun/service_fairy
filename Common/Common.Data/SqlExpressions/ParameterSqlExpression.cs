using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Data.SqlExpressions
{
    /// <summary>
    /// 参数表达式
    /// </summary>
    public class ParameterSqlExpression : SqlExpressionBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parameterName"></param>
        internal ParameterSqlExpression(string parameterName)
        {
            Contract.Requires(parameterName != null);

            ParameterName = parameterName;
        }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParameterName { get; private set; }

        protected override string OnToString(ISqlExpressionContext context)
        {
            return null;
            //object value = (context == null) ? null : context.GetValue(ParameterName);
            //return UnionTableUtility.ReviseToMsSqlValue(value);
        }
    }
}
