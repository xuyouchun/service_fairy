using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Statements
{
    public class DefaultStatementCompileContext : IStatementCompileContext
    {
        private readonly DefaultExpressionCompileContext _ExpressionCompileContext = new DefaultExpressionCompileContext();

        #region IStatementCompileContext 成员

        public IExpressionCompileContext ExpressionCompileContext
        {
            get { return _ExpressionCompileContext; }
        }

        #endregion
    }
}
