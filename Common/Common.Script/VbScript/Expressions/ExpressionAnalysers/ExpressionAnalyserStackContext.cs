using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.ExpressionAnalysers
{
    class ExpressionAnalyserStackContext
    {
        public ExpressionAnalyserStackContext(IExpressionCompileContext context)
            : this(new ExpressionAnalyserStack(), context)
        {
            
        }

        public ExpressionAnalyserStackContext(ExpressionAnalyserStack stack, IExpressionCompileContext context)
        {
            Context = context;
            Stack = stack;
        }

        /// <summary>
        /// 编译上下文环境
        /// </summary>
        public IExpressionCompileContext Context { get; private set; }

        /// <summary>
        /// 堆栈
        /// </summary>
        public ExpressionAnalyserStack Stack { get; private set; }

        /// <summary>
        /// 之前保持的表达式
        /// </summary>
        public Expression PreviousExpression { get; private set; }

        /// <summary>
        /// 之前保持的操作符
        /// </summary>
        public OperatorInfo PreviousOperatorInfo { get; private set; }

        /// <summary>
        /// 将之前的表达式与操作符添加到所属栈中
        /// </summary>
        public void Flush()
        {
            if (PreviousOperatorInfo != null)
            {
                Stack.PushOperator(PreviousOperatorInfo);

                if (PreviousExpression != null)
                    Stack.PushExpression(PreviousExpression);
            }

            PreviousExpression = null;
            PreviousOperatorInfo = null;
        }

        /// <summary>
        /// 保持一个表达式
        /// </summary>
        /// <param name="exp"></param>
        public void KeepExpression(Expression exp)
        {
            if (PreviousExpression != null)
                throw AnalyserHelper.CreateException();

            if (PreviousOperatorInfo == null)  // 表示是第一个表达式，直接入栈
            {
                Stack.PushExpression(exp);
            }
            else
            {
                PreviousExpression = exp;
            }
        }

        /// <summary>
        /// 保持一个操作符
        /// </summary>
        /// <param name="opInfo"></param>
        /// <returns>返回非零表示不能在该队列中保持该操作符，因为它具有更高(大于零)或更低(小于零)的优先级</returns>
        public int KeepOperator(OperatorInfo opInfo)
        {
            if (PreviousOperatorInfo == null) // 之前没有操作符，表示这是第一个操作符，保持住留待与后续操作符做优先级比较
            {
                PreviousOperatorInfo = opInfo;
                return 0;
            }
            else
            {
                int result = opInfo.Priority - PreviousOperatorInfo.Priority;

                if (result == 0)
                {
                    Stack.PushOperator(PreviousOperatorInfo);
                    if (PreviousExpression != null)
                        Stack.PushExpression(PreviousExpression);

                    PreviousOperatorInfo = opInfo;
                    PreviousExpression = null;
                }
                else if (result < 0)
                {
                    Flush();
                }

                return result;
            }
        }

        public Expression ClearExpression()
        {
            Expression exp = PreviousExpression;
            PreviousExpression = null;
            return exp;
        }
    }
}
