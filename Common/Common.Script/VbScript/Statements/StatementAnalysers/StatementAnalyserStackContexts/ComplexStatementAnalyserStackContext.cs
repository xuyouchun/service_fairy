using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements.StatementAnalysers.StatementAnalyserStackContexts
{
    class ComplexStatementAnalyserStackContext : StatementAnalyserStackContext
    {
        public ComplexStatementAnalyserStackContext(StatementAnalyserStack analyserStack, IStatementCompileContext context)
            : base(analyserStack, StatementType.Complex, context)
        {

        }

        public override void PushExpression(Expression expression)
        {
            PushStatement(CreateSimpleStatement(expression));
        }

        public override void PushStatement(Statement statement)
        {
            base.PushStatement(statement);
        }

        public override Statement GetStatement()
        {
            ComplexStatement complexStatement = (ComplexStatement)base.GetStatement();

            return ComplexStatementBuilder.Optimize(complexStatement);
        }
    }
}
