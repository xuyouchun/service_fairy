using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;
using System.Threading;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 脚本执行器
    /// </summary>
    public class ScriptExecutor : IDisposable
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ScriptExecutor()
        {

        }

        /// <summary>
        /// 异步执行
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <param name="statement">语句块</param>
        /// <param name="context">上下文执行环境</param>
        /// <param name="runAtOnce">是否立即执行</param>
        public virtual IScriptExecutorAsyncResult BeginExecute(AsyncCallback callback, Statement statement, IExpressionContext context, bool runAtOnce)
        {
            if (callback == null || statement == null || context == null)
                throw new ArgumentNullException(callback == null ? "callback" : statement == null ? "statement" : "context");

            Thread thread = new Thread(_RunningFunc);
            thread.IsBackground = true;

            StatementContext staCtx = new StatementContext(this, statement, runAtOnce, thread);
            RunningContext runCtx = new RunningContext(staCtx, context);
            staCtx.RunningContext = runCtx;

            thread.Start(new object[] { runCtx, callback });

            return staCtx;
        }

        /// <summary>
        /// 异步执行
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <param name="statement">语句块</param>
        /// <param name="context">上下文执行环境</param>
        /// <returns></returns>
        public IScriptExecutorAsyncResult BeginExecute(AsyncCallback callback, Statement statement, IExpressionContext context)
        {
            return BeginExecute(callback, statement, context, true);
        }

        /// <summary>
        /// 异步执行
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <param name="statement">语句块</param>
        /// <returns></returns>
        public IScriptExecutorAsyncResult BeginExecute(AsyncCallback callback, Statement statement)
        {
            return BeginExecute(callback, statement, new DefaultExpressionContext(), true);
        }

        /// <summary>
        /// 异步执行
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <param name="statement">语句块</param>
        /// <param name="pause">是否立即执行</param>
        /// <returns></returns>
        public IScriptExecutorAsyncResult BeginExecute(AsyncCallback callback, Statement statement, bool runAtOnce)
        {
            return BeginExecute(callback, statement, new DefaultExpressionContext(), runAtOnce);
        }

        /// <summary>
        /// 异步执行
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <param name="script">脚本</param>
        /// <param name="context">上下文执行环境</param>
        /// <param name="runAtOnce">是否立即执行</param>
        /// <returns></returns>
        public IScriptExecutorAsyncResult BeginExecute(AsyncCallback callback, string script, IExpressionContext context, bool runAtOnce)
        {
            if (callback == null || script == null || context == null)
                throw new ArgumentNullException(callback == null ? "callback" : script == null ? "script" : "context");

            return BeginExecute(callback, Statement.Parse(script), context, runAtOnce);
        }

        /// <summary>
        /// 异步执行
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <param name="script">脚本</param>
        /// <param name="context">上下文执行环境</param>
        /// <returns></returns>
        public IScriptExecutorAsyncResult BeginExecute(AsyncCallback callback, string script, IExpressionContext context)
        {
            return BeginExecute(callback, script, context, true);
        }

        /// <summary>
        /// 异步执行
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <param name="script">脚本</param>
        /// <returns></returns>
        public IScriptExecutorAsyncResult BeginExecute(AsyncCallback callback, string script)
        {
            return BeginExecute(callback, script, new DefaultExpressionContext(), true);
        }

        /// <summary>
        /// 异步执行
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <param name="script">脚本</param>
        /// <param name="runAtOnce">是否立即执行</param>
        /// <returns></returns>
        public IScriptExecutorAsyncResult BeginExecute(AsyncCallback callback, string script, bool runAtOnce)
        {
            return BeginExecute(callback, script, new DefaultExpressionContext(), runAtOnce);
        }

        private void _RunningFunc(object state)
        {
            object[] args = (object[])state;
            RunningContext runCtx = (RunningContext)args[0];
            AsyncCallback callback = (AsyncCallback)args[1];
            StatementContext stCtx = (StatementContext)runCtx.StatementContext;

            stCtx.AsyncState = this;

            try
            {
                runCtx.Initialize(stCtx.Statement);

                // 执行
                runCtx.MainBody.Execute(runCtx);

                stCtx.IsCompleted = true;
                stCtx.CompletedSynchronously = true;
                stCtx.AsyncWaitEvent.Set();

                callback(stCtx);
            }
            catch (Exception ex)
            {
                stCtx.Error = ex;
                stCtx.IsCompleted = true;
                stCtx.CompletedSynchronously = false;
                stCtx.AsyncWaitEvent.Set();

                callback(stCtx);
            }
        }

        /// <summary>
        /// 异步执行结束
        /// </summary>
        /// <param name="result"></param>
        public IScriptExecutorAsyncResult EndExecute(IAsyncResult result)
        {
            StatementContext ctx = (StatementContext)result;
            if (ctx.Error != null && !(ctx.Error is ThreadAbortException))
                throw ctx.Error;

            return (IScriptExecutorAsyncResult)result;
        }

        /// <summary>
        /// 同步执行
        /// </summary>
        /// <param name="statement">语句块</param>
        /// <param name="context">上下文执行环境</param>
        public virtual void Execute(Statement statement, IExpressionContext context)
        {
            if (statement == null || context == null)
                throw new ArgumentNullException(statement == null ? "statement" : "context");

            RunningContext runCtx = new RunningContext(new StatementContext(this, statement, true, null), context);
            runCtx.Initialize(statement);

            runCtx.MainBody.Execute(runCtx);
        }

        /// <summary>
        /// 同步执行
        /// </summary>
        /// <param name="statement"></param>
        public void Execute(Statement statement)
        {
            Execute(statement, new DefaultExpressionContext());
        }

        /// <summary>
        /// 同步执行
        /// </summary>
        /// <param name="script"></param>
        /// <param name="context"></param>
        public void Execute(string script, IExpressionContext context)
        {
            if (script == null || context == null)
                throw new ArgumentNullException(script == null ? "script" : "context");

            Execute(Statement.Parse(script), context);
        }

        /// <summary>
        /// 同步执行
        /// </summary>
        /// <param name="script"></param>
        public void Execute(string script)
        {
            Execute(script, new DefaultExpressionContext());
        }

        /// <summary>
        /// 开始执行某行代码
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        internal protected virtual bool OnBeforeExecuteStatement(Statement statement)
        {
            var eh = BeforeExecuteStatement;
            if (eh != null)
            {
                ScriptExecutorBeforeExecuteStatementEventArgs e = new ScriptExecutorBeforeExecuteStatementEventArgs(statement);
                eh(this, e);
                return !e.Cancel;
            }

            return true;
        }

        /// <summary>
        /// 结束执行某行代码
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="result"></param>
        internal protected virtual void OnEndExecuteStatement(Statement statement, StatementExecuteResult result)
        {
            var eh = EndExecuteStatement;
            if (eh != null)
                eh(this, new ScriptExecutorEndExecuteStatementEventArgs(statement, result.RaiseByCurrentLine ? result.Error : null));
        }

        /// <summary>
        /// 开始执行某条语句
        /// </summary>
        public event ScriptExecutorBeforeExecuteStatementEventHandler BeforeExecuteStatement;

        /// <summary>
        /// 结束执行某条语句
        /// </summary>
        public event ScriptExecutorEndExecuteStatementEventHandler EndExecuteStatement;

        #region IDisposable 成员

        public virtual void Dispose()
        {
            
        }

        #endregion
    }
}
