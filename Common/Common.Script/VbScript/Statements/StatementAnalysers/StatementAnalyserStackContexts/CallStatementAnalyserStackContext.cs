using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Statements.StatementAnalysers.StatementAnalyserStackContexts
{
    class CallStatementAnalyserStackContext : StatementAnalyserStackContext
    {
        public CallStatementAnalyserStackContext(StatementAnalyserStack analyserStack, IStatementCompileContext context)
            : base(analyserStack, StatementType.Simple, context)
        {

        }

        private bool _HasEnd = false;
        private string _SubName;
        private MemberExpression _Result;

        public override void PushExpression(Expression expression)
        {
            if (_SubName == null)
            {
                DynamicExpression dynamicExp;
                VariableExpression varExp;
                MemberExpression memberExp;

                if ((dynamicExp = expression as DynamicExpression) != null)
                {
                    _SubName = dynamicExp.Name;
                    Expressions.AddRange(dynamicExp.Parameters);
                    _HasEnd = true;
                }
                else if ((varExp = expression as VariableExpression) != null)
                {
                    _SubName = varExp.Name;
                }
                else if ((memberExp = expression as MemberExpression) != null)
                {
                    _Result = memberExp;
                    _HasEnd = true;
                }
                else
                {
                    throw new ScriptException("CALL语句语法错误");
                }
            }
            else
            {
                Expressions.Add(expression);
            }
        }

        public override bool PushKeyword(Keywords keyword)
        {
            throw new ScriptException("在CALL语句中遇到不能识别的关键字" + KeywordManager.GetKeywordInfo(keyword).Name);
        }

        public override void EndNewLine()
        {
            _HasEnd = true;
        }

        public override bool HasEnd()
        {
            return _HasEnd;
        }

        public override void PushStatement(Statement statement)
        {
            throw new ScriptException("CALL语句语法错误");
        }

        public override Statement GetStatement()
        {
            if (_Result != null)
                return CreateSimpleStatement(_Result);

            if (_SubName == null)
                throw new ScriptException("CALL语句语法错误");

            return new SimpleStatement(new FunctionExpression(_SubName, Expressions.ToArray()));
        }
        
    }
}
