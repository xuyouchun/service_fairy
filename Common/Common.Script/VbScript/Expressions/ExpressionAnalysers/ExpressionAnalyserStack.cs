using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.ExpressionAnalysers
{
    /// <summary>
    /// 用于生成表达式的堆栈
    /// </summary>
    class ExpressionAnalyserStack
    {
        public ExpressionAnalyserStack()
        {
            Stack = new Stack<Expression>();
        }

        private Stack<OperatorInfoContext> _OperatorInfoStack = new Stack<OperatorInfoContext>();

        #region Class OperatorInfoContext ...

        /// <summary>
        /// 
        /// </summary>
        class OperatorInfoContext
        {
            public OperatorInfoContext(OperatorInfo info)
            {
                Info = info;
                ReadyCount = info.ParameterInfo.ParameterCountOnLeft;
            }

            public OperatorInfo Info { get; private set; }

            /// <summary>
            /// 记录已经准备好多少个参数
            /// </summary>
            public int ReadyCount { get; set; }
        }

        #endregion

        public void PushOperator(OperatorInfo info)
        {
            if (Stack.Count < info.ParameterInfo.ParameterCountOnLeft)
                throw new ScriptException("表达式语法错误");

            _OperatorInfoStack.Push(new OperatorInfoContext(info));
            _CheckStack();
        }

        public void PushExpression(Expression expression)
        {
            Stack.Push(expression);
            if (_OperatorInfoStack.Count > 0)
                _OperatorInfoStack.Peek().ReadyCount++;

            _CheckStack();
        }

        /// <summary>
        /// 结束当前的表达式
        /// </summary>
        public void EndExpression()
        {
            if (_OperatorInfoStack.Count > 0)
                throw AnalyserHelper.CreateException();
        }

        /// <summary>
        /// 检查栈中是否有可以生成的表达式
        /// </summary>
        private void _CheckStack()
        {
            if (_OperatorInfoStack.Count == 0)
                return;

            OperatorInfoContext ctx = _OperatorInfoStack.Peek();
            ParameterInfo pInfo = ctx.Info.ParameterInfo;
            if (ctx.ReadyCount >= pInfo.ParameterCount)
            {
                Expression exp = ctx.Info.CreateExpression(GetExpressions(pInfo.ParameterCount));
                _OperatorInfoStack.Pop();
                PushExpression(exp);
            }
        }

        protected Expression[] GetExpressions(int parameterCount)
        {
            if (parameterCount > Stack.Count)
                throw new ScriptException("表达式语法错误");

            Expression[] expressions = new Expression[parameterCount];
            for (int k = expressions.Length - 1; k >= 0; k--)
            {
                expressions[k] = Stack.Pop();
            }

            return expressions;
        }

        protected Expression[] GetAllExpressions()
        {
            return GetExpressions(Stack.Count);
        }

        public virtual Expression GetExpression()
        {
            Expression[] exps = GetExpressions();
            if (exps.Length != 1)
                throw new ScriptException("表达式语法错误");

            return exps[0];
        }

        public virtual Expression[] GetExpressions()
        {
            if (_OperatorInfoStack.Count != 0)
                throw new ScriptException("表达式语法错误");

            return GetAllExpressions();
        }

        protected Stack<Expression> Stack { get; private set; }
    }
}
