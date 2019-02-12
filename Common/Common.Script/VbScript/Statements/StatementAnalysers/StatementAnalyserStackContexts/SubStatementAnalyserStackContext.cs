using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Statements.StatementAnalysers.StatementAnalyserStackContexts
{
    class SubStatementAnalyserStackContext : StatementAnalyserStackContext
    {
        public SubStatementAnalyserStackContext(StatementAnalyserStack stack, string ownerClassName, IStatementCompileContext context)
            : base(stack, StatementType.Sub, context)
        {
            _OwnerClassName = ownerClassName;
        }

        private string _SubName, _OwnerClassName;
        private List<VariableExpression> _Parameters = new List<VariableExpression>();
        private readonly ComplexStatementBuilder _BodyBuilder = new ComplexStatementBuilder();
        private bool _IsMetNewLine = false, _HasEnd = false, _HasEndDeclare = false;

        public override void PushExpression(Expression expression)
        {
            if (_SubName == null)
            {
                if (_SubName != null || _HasEndDeclare)
                    throw new ScriptException("定义过程时语法错误");

                DynamicExpression dynamicExp = expression as DynamicExpression;
                if (dynamicExp != null)
                {
                    _SubName = dynamicExp.Name;
                    foreach (Expression exp in dynamicExp.Parameters)
                    {
                        VariableExpression varExp = exp as VariableExpression;
                        if (varExp == null)
                            throw new ScriptException(string.Format("定义过程{0}时参数{1}不能被识别", _SubName, varExp.ToString()));

                        _Parameters.Add(varExp);
                    }

                    _HasEndDeclare = true;
                }
                else
                {
                    VariableExpression varExp = expression as VariableExpression;
                    if (varExp != null)
                        _SubName = varExp.Name;
                    else
                        throw new ScriptException("定义过程的名字格式错误：" + expression.ToString());

                    _HasEndDeclare = true;
                }
            }
            else
            {
                if (!_IsMetNewLine)
                    throw new ScriptException(string.Format("定义过程{0}时参数中遗漏了括号", _SubName));
                else
                    PushStatement(CreateSimpleStatement(expression));
            }
        }

        public override void PushStatement(Statement statement)
        {
            _BodyBuilder.AddStatement(statement);
        }

        public override void EndNewLine()
        {
            _IsMetNewLine = true;
            _HasEndDeclare = true;
        }

        public override bool HasEnd()
        {
            return _HasEnd;
        }

        public override bool PushKeyword(Keywords keyword)
        {
            if (!_IsMetNewLine)
                throw new ScriptException("过程定义时遇到不能识别的关键字：" + KeywordManager.GetKeywordInfo(keyword).Name);

            if (keyword == Keywords.EndSub)
            {
                _HasEnd = true;
                return true;
            }

            return base.PushKeyword(keyword);
        }

        public override Statement GetStatement()
        {
            if (_SubName == null)
                throw new ScriptException("过程定义时语法错误");

            return new SubStatement(_SubName, _Parameters.ToArray(), _BodyBuilder.GetStatement(), _OwnerClassName);
        }
    }
}
