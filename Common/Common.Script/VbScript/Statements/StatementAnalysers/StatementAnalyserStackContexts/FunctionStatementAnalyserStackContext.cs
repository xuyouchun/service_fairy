using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Statements.StatementAnalysers.StatementAnalyserStackContexts
{
    class FunctionStatementAnalyserStackContext : StatementAnalyserStackContext
    {
        public FunctionStatementAnalyserStackContext(StatementAnalyserStack analyserStack, string ownerClassName, IStatementCompileContext context)
            : base(analyserStack, StatementType.Function, context)
        {
            _OwnerClassName = ownerClassName;
        }

        private string _FunctionName, _OwnerClassName;
        private readonly ComplexStatementBuilder _BodyBuilder = new ComplexStatementBuilder();
        private readonly List<VariableExpression> _Parameters = new List<VariableExpression>();
        private bool _HasEnd = false, _IsMetNewLine = false, _HasEndDeclare = false;

        public override void PushExpression(Expression expression)
        {
            if (_FunctionName == null)
            {
                if (_FunctionName != null || _HasEndDeclare)
                    throw new ScriptException("定义函数时语法错误");

                DynamicExpression dynamicExp = expression as DynamicExpression;
                if (dynamicExp != null)
                {
                    _FunctionName = dynamicExp.Name;
                    foreach (Expression exp in dynamicExp.Parameters)
                    {
                        VariableExpression varExp = exp as VariableExpression;
                        if (varExp == null)
                            throw new ScriptException(string.Format("定义函数{0}时参数{1}不能被识别", _FunctionName, varExp.ToString()));

                        _Parameters.Add(varExp);
                    }

                    _HasEndDeclare = true;
                }
                else
                {
                    VariableExpression varExp = expression as VariableExpression;
                    if (varExp != null)
                        _FunctionName = varExp.Name;
                    else
                        throw new ScriptException("定义过程的名字格式错误：" + expression.ToString());

                    _HasEndDeclare = true;
                }
            }
            else
            {
                if (!_IsMetNewLine)
                    throw new ScriptException(string.Format("定义过程{0}时参数中遗漏了括号", _FunctionName));
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
                throw new ScriptException("定义函数时在参数处遇到不能识别的关键字" + KeywordManager.GetKeywordInfo(keyword).Name);

            if (keyword == Keywords.EndFunction)
            {
                _HasEnd = true;
                return true;
            }
            else
            {
                return base.PushKeyword(keyword);
            }
        }

        public override Statement GetStatement()
        {
            if (_FunctionName == null)
                throw new ScriptException("定义函数时格式错误");

            return new FunctionStatement(_FunctionName, _Parameters.ToArray(), _BodyBuilder.GetStatement(), _OwnerClassName);
        }
    }
}
