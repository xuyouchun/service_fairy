using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Statements.StatementAnalysers.StatementAnalyserStackContexts
{
    class ForStatementAnalyserStackContext : StatementAnalyserStackContext
    {
        public ForStatementAnalyserStackContext(StatementAnalyserStack analyserStack, IStatementCompileContext context)
            : base(analyserStack, StatementType.For, context)
        {

        }

        private VariableExpression _VarExpression, _NextExpression;
        private Expression _InitExpression, _ToExpression, _StepExpression;
        private bool _IsMetAssign = false, _IsMetTo = false, _IsMetStep = false, _IsMetNext = false, _HasEnd = false;
        private readonly ComplexStatementBuilder _BodyBuilder = new ComplexStatementBuilder();

        public override void PushExpression(Expression expression)
        {
            if (_VarExpression == null)
            {
                _VarExpression = expression as VariableExpression;
                if (_VarExpression == null)
                    throw new ScriptException("FOR语句循环变量类型错误");
            }
            else if (_InitExpression == null)
            {
                if (!_IsMetAssign)
                    throw new ScriptException("FOR语句给循环变量赋初始值的语法错误");

                _InitExpression = expression;
            }
            else if (_ToExpression == null)
            {
                if (!_IsMetTo)
                    throw new ScriptException("FOR语句给循环变量指定终点的语法错误");

                _ToExpression = expression;
            }
            else if (_IsMetStep && !_IsMetNext && _StepExpression == null)
            {
                if (_StepExpression != null)
                    throw new ScriptException("FOR语句中STEP语法错误");

                _StepExpression = expression;
            }
            else if (!_IsMetNext)
            {
                PushStatement(CreateSimpleStatement(expression));
            }
            else
            {
                if (_NextExpression != null)
                    throw new ScriptException("FOR语句中NEXT语法错误");

                _NextExpression = expression as VariableExpression;
                if (_NextExpression == null)
                    throw new ScriptException("FOR语句中NEXT语法错误");

                if (_NextExpression.Name != _VarExpression.Name)
                    throw new ScriptException("FOR语句语法错误，NEXT语句所指定的变量与循环变量不同");

                _HasEnd = true;
            }
        }

        public override void EndNewLine()
        {
            if (_IsMetNext)
            {
                if (_NextExpression == null)
                    _NextExpression = _VarExpression;

                _HasEnd = true;
            }
        }

        public override void PushStatement(Statement statement)
        {
            _BodyBuilder.AddStatement(statement);
        }

        public override bool PushKeyword(Keywords keyword)
        {
            switch (keyword)
            {
                case Keywords.Assign:  // 等号运算符
                    if (_VarExpression == null)
                        throw new ScriptException("FOR语句未指定循环变量");
                    if (_IsMetAssign)
                        throw new ScriptException("FOR语句遇到多余的赋值操作符");
                    _IsMetAssign = true;
                    break;

                case Keywords.To:  // To
                    if (!_IsMetAssign || _IsMetTo)
                        throw new ScriptException("FOR语句TO语法错误");

                    _IsMetTo = true;
                    break;

                case Keywords.Step:  // Step
                    if (!_IsMetTo || _IsMetStep || _IsMetNext)
                        throw new ScriptException("FOR语句STEP语法错误");

                    _IsMetStep = true;
                    break;

                case Keywords.Next:  // Next
                    if (!_IsMetTo || _IsMetNext)
                        throw new ScriptException("FOR语句NEXT语法错误");

                    _IsMetNext = true;
                    break;

                default:
                    if (!base.PushKeyword(keyword))
                        throw new ScriptException("FOR语句中遇到错误的关键字" + keyword);

                    break;
            }

            return true;
        }

        public override bool HasEnd()
        {
            return _HasEnd;
        }

        public override Statement GetStatement()
        {
            if (_StepExpression == null)
                _StepExpression = Expression.Empty;

            if (_VarExpression == null)
                throw new ScriptException("FOR语句未指定循环变量");

            if (_InitExpression == null)
                throw new ScriptException("FOR语句中循环变量未指定初始值");

            if (_ToExpression == null)
                throw new ScriptException("FOR语句中循环变量未指定终点");

            if (_NextExpression == null)
                _NextExpression = _VarExpression;

            Expressions.AddRange(new Expression[] { _VarExpression, _InitExpression, _ToExpression, _StepExpression });
            Statements.Add(_BodyBuilder.GetStatement());

            return base.GetStatement();
        }
    }
}
