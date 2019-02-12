using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Statements;

namespace Common.Script.VbScript
{
    class StatementReader
    {
        public StatementReader(Statement statement)
        {
            Statement = statement;

            _Init();
        }

        private void _Init()
        {
            ComplexStatement cpxSt;

            if ((cpxSt = Statement as ComplexStatement) != null)
            {
                List<FunctionStatement> funcs = new List<FunctionStatement>();
                List<ClassStatement> classes = new List<ClassStatement>();
                List<Statement> statements = new List<Statement>();

                foreach (Statement st in cpxSt.Statements)
                {
                    FunctionStatement funcSt;
                    ClassStatement classSt;

                    if ((funcSt = st as FunctionStatement) != null)
                        funcs.Add(funcSt);
                    else if ((classSt = st as ClassStatement) != null)
                        classes.Add(classSt);
                    else
                        statements.Add(st);
                }

                MainBody = new MainStatement(statements.ToArray());
                FunctionStatements = funcs.ToArray();
                ClassStatements = classes.ToArray();
            }
            else
            {
                MainBody = new MainStatement(new Statement[] { MainStatement.Empty });
                FunctionStatements = new FunctionStatement[0];
                ClassStatements = new ClassStatement[0];

                FunctionStatement funcSt;
                ClassStatement classSt;

                if ((funcSt = Statement as FunctionStatement) != null)
                    FunctionStatements = new FunctionStatement[] { funcSt };
                else if ((classSt = Statement as ClassStatement) != null)
                    ClassStatements = new ClassStatement[] { classSt };
                else
                    MainBody = new MainStatement(new Statement[] { Statement });
            }
        }

        public Statement Statement { get; private set; }

        /// <summary>
        /// 主语句块
        /// </summary>
        public MainStatement MainBody { get; private set; }

        /// <summary>
        /// 自定义函数集合
        /// </summary>
        public FunctionStatement[] FunctionStatements { get; private set; }

        /// <summary>
        /// 自定义类集合
        /// </summary>
        public ClassStatement[] ClassStatements { get; private set; }
    }
}
