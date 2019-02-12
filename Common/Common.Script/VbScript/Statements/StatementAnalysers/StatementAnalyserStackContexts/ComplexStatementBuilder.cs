using System;
using System.Collections.Generic;
using System.Text;

namespace Ats.Client.Script.Statements.StatementAnalysers.StatementAnalyserStackContexts
{
    class ComplexStatementBuilder
    {
        public ComplexStatementBuilder()
        {

        }

        private readonly List<Statement> _Statements = new List<Statement>();

        public void AddStatement(Statement statement)
        {
            _Statements.Add(statement);
        }

        public void AddStatements(IEnumerable<Statement> statements)
        {
            foreach (Statement statement in statements)
            {
                AddStatement(statement);
            }
        }

        public bool IsEmpty { get { return _Statements.Count == 0; } }

        public Statement GetStatement()
        {
            switch (_Statements.Count)
            {
                case 0: return Statement.Void;

                case 1: return _Statements[0];

                default: return new ComplexStatement(_Statements.ToArray());
            }
        }

        /// <summary>
        /// 优化指定的复合表达式，如果只有一条语句，则直接将该语句返回，如果没有语句，则返回Void
        /// </summary>
        /// <param name="complexStatement"></param>
        /// <returns></returns>
        public static Statement Optimize(ComplexStatement complexStatement)
        {
            ComplexStatementBuilder builder = new ComplexStatementBuilder();
            builder.AddStatements(complexStatement.Statements);
            return builder.GetStatement();
        }
    }
}
