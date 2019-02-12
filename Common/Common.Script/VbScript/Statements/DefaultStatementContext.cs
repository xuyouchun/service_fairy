using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Statements
{
    class DefaultStatementContext : IStatementContext
    {
        private readonly IExpressionContext _ExpressionContext = new DefaultExpressionContext();

        public IExpressionContext ExpressionContext
        {
            get { return _ExpressionContext; }
        }


        public bool BeforeExecuteStatement(RunningContext context, Statement statement)
        {
            return true;
        }

        public void EndExecuteStatement(RunningContext context, Statement stateme, StatementExecuteResult result)
        {
            
        }
    }
}
