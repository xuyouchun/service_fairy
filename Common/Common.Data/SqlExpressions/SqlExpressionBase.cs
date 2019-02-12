using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Data.SqlExpressions
{
    public abstract class SqlExpressionBase : SqlExpression
    {
        public sealed override string ToString(ISqlExpressionContext context)
        {
            string s;
            if (context != null && (s = context.ToStringCallback(this)) != null)
                return s;

            return OnToString(context);
        }

        protected abstract string OnToString(ISqlExpressionContext context);

        public override IEnumerator<SqlExpression> GetEnumerator()
        {
            yield break;
        }
    }
}
