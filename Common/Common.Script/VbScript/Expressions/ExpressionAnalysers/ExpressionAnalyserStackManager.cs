using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Common.Script.VbScript.Expressions.ExpressionAnalysers
{
    class ExpressionAnalyserStackManager
    {
        public ExpressionAnalyserStackManager(IExpressionCompileContext context)
        {
            _Context = context;
            _BeginExpressionGroup();
        }

        private readonly IExpressionCompileContext _Context;
        private readonly Stack<ExpressionAnalyserStackContext> _ExpressionStacks = new Stack<ExpressionAnalyserStackContext>();

        /// <summary>
        /// 添加一个表达式
        /// </summary>
        /// <param name="exp"></param>
        public void PushExpression(Expression exp)
        {
            exp = _FixExpression(exp);
            _ExpressionStacks.Peek().KeepExpression(exp);
        }

        /// <summary>
        /// 添加一个操作符
        /// </summary>
        /// <param name="opInfo"></param>
        public void PushOperator(OperatorInfo opInfo)
        {
            ExpressionAnalyserStackContext ctx = _ExpressionStacks.Peek();

            int result;
            if ((result = ctx.KeepOperator(opInfo)) == 0)
                return;

            if (result > 0) // 新操作符具有更高的优先级
            {
                _BeginExpressionGroup();
                ExpressionAnalyserStackContext curCtx = _ExpressionStacks.Peek();
                Expression exp = ctx.ClearExpression();
                if (exp != null)
                    curCtx.KeepExpression(exp);

                PushOperator(opInfo);
            }
            else            // 新操作符具有更低的优先级
            {
                if (_ExpressionStacks.Count >= 2 && !(_ExpressionStacks.Peek().Stack is DynamicExpressonAnalyserStack))
                    _EndExpressionGroup();
                else
                    _ExpressionStacks.Peek().Flush();

                PushOperator(opInfo);
            }
        }

        public void PushDynamicExpression(string name)
        {
            _BracketCount++;
            _ExpressionStacks.Push(new ExpressionAnalyserStackContext(new DynamicExpressonAnalyserStack(name), _Context));
        }

        public void _BeginExpressionGroup()
        {
            _ExpressionStacks.Push(new ExpressionAnalyserStackContext(_Context));
        }

        public void BeginExpressionGroup()
        {
            _BracketCount++;
            _BeginExpressionGroup();
        }

        public void _EndExpressionGroup()
        {
            if (_ExpressionStacks.Count < 2)
                return;

            _ExpressionStacks.Peek().Flush();
            Expression exp = _ExpressionStacks.Pop().Stack.GetExpression();
            _ExpressionStacks.Peek().KeepExpression(exp);
        }

        int _BracketCount = 0;

        public void EndExpressionGroup()
        {
            _BracketCount--;
            _EndExpressionGroup();
        }

        public void EndExpression()
        {
        begin:

            _ExpressionStacks.Peek().Flush();
            _ExpressionStacks.Peek().Stack.EndExpression();

            if (_ExpressionStacks.Count > 1 && !(_ExpressionStacks.Peek().Stack is DynamicExpressonAnalyserStack))
            {
                _EndExpressionGroup();
                goto begin;
            }
        }

        public Expression[] GetExpressions()
        {
            if (_BracketCount != 0)
                throw new ScriptException(string.Format("表达式错误，遗漏了{0}括号", _BracketCount < 0 ? "左" : "右"));

            while (_ExpressionStacks.Count > 1)
            {
                EndExpressionGroup();
            }

            _ExpressionStacks.Peek().Flush();
            
            return _ExpressionStacks.Peek().Stack.GetExpressions();
        }

        /// <summary>
        /// 判断哪些是常量表达式，并作相应的转换
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private Expression _FixExpression(Expression exp)
        {
            VariableExpression varExp = exp as VariableExpression;
            if (varExp != null)
            {
                Value value = _Context.GetConstValue(varExp.Name);
                if ((object)value != null)
                    return new ConstValueExpression(varExp.Name, value);

                value = VbConstValues.GetConstValue(varExp.Name);
                if ((object)value != null)
                    return new ConstValueExpression(varExp.Name, value);
            }

            return exp;
        }
    }
}
