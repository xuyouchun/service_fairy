using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Statements;
using Common.Script.VbScript.Values;
using Common.Script.VbScript.Common;
using System.Threading;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 运行时的上下文执行环境
    /// </summary>
    public class RunningContext
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="body">语句块</param>
        /// <param name="statementContext">语句的上下文执行环境</param>
        /// <param name="expressionContext">表达式的上下文执行环境</param>
        internal RunningContext(IStatementContext statementContext, IExpressionContext expressionContext)
        {
            if (statementContext == null || expressionContext == null)
                throw new ArgumentNullException(statementContext == null ? "statementContext" : "expressionContext");

            StatementContext = statementContext;
            ExpressionContext = expressionContext;
            GlobalExpressonContext = expressionContext;

            CallStack = new FunctionCallStack(this);
            StatementStack = new StatementStack();
            LastValueCacheManager = new LastValueCacheManager();
        }

        /// <summary>
        /// 语句的上下文执行环境
        /// </summary>
        public IStatementContext StatementContext { get; internal set; }

        /// <summary>
        /// 表达式的上下文执行环境
        /// </summary>
        public IExpressionContext ExpressionContext { get; internal set; }

        /// <summary>
        /// 全局命名空间中的ExpressionContext
        /// </summary>
        public IExpressionContext GlobalExpressonContext { get; private set; }

        /// <summary>
        /// 是否声明了Option Explicit，表明变量必须显式声明
        /// </summary>
        internal bool IsOptionExplicit { get; set; }

        /// <summary>
        /// 堆栈调用深度
        /// </summary>
        public int CallStackDepth { get; set; }

        /// <summary>
        /// Exit状态，例如：Exit For、Exit While、Exit Loop等
        /// </summary>
        internal ExitState ExitState { get; set; }

        /// <summary>
        /// 函数调用堆栈
        /// </summary>
        internal FunctionCallStack CallStack { get; private set; }

        /// <summary>
        /// 用于控制goto语句的行为
        /// </summary>
        internal GoToLabelState GoToLabelState { get; set; }

        /// <summary>
        /// 用于存储局部变量的值，便于后续读取时，能够读取到一致的值
        /// </summary>
        internal LastValueCacheManager LastValueCacheManager { get; private set; }

        /// <summary>
        /// 语句执行的堆栈
        /// </summary>
        internal StatementStack StatementStack { get; private set; }

        /// <summary>
        /// 遇到错误时是否继续之后的语句
        /// </summary>
        internal bool OnErrorResumeNext { get; set; }

        /// <summary>
        /// 开始执行语句
        /// </summary>
        /// <param name="statement"></param>
        /// <returns>返回false表示要取消该语句的执行</returns>
        internal bool BeforeExecuteStatement(Statement statement)
        {
            if (GoToLabelState != null && !GoToLabelState.HasExitToRoot)
                return false;

            if (ExitState != null && ExitState.StatementType != statement.GetStatementType())
                return false;

            if (StatementContext.BeforeExecuteStatement(this, statement))
            {
                StatementStack.Push(statement);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 判断是否在EXIT语句的有效范围内
        /// </summary>
        /// <returns></returns>
        internal bool CheckExit()
        {
            return ExitState != null;
        }

        /// <summary>
        /// 结束语句的执行
        /// </summary>
        /// <param name="result"></param>
        internal void EndExecuteStatement(Statement statement, StatementExecuteResult result)
        {
            if (ExitState != null && ExitState.StatementType == statement.GetStatementType())
                ExitState = null;

            try
            {
                if (result.Success)
                {
                    _CurrentError = null;
                }
                else
                {
                    result.RaiseByCurrentLine = (_CurrentError == null);
                    if (_CurrentError == null)
                        _CurrentError = result.Error;
                }

                StatementContext.EndExecuteStatement(this, statement, result);

                if (result.Error is OverflowException || !result.Success && !GetCurrentField().OnErrorResumeNext)
                    throw result.Error;

                _CurrentError = null;
            }
            finally
            {
                StatementStack.Pop();
            }
        }

        private Exception _CurrentError;

        /// <summary>
        /// 获取自定义的函数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal FunctionStatement GetFunctionStatement(string name)
        {
            FunctionStatement statement;
            _FuncDict.TryGetValue(name.ToUpper(), out statement);

            return statement;
        }

        private Dictionary<string, FunctionStatement> _FuncDict = new Dictionary<string, FunctionStatement>();

        /// <summary>
        /// 脚本执地主体
        /// </summary>
        internal MainStatement MainBody { get; private set; }

        /// <summary>
        /// 添加一个自定义函数
        /// </summary>
        /// <param name="statement"></param>
        private void _AddFunctionStatement(FunctionStatement statement)
        {
            string name = statement.Name.ToUpper();
            if (_FuncDict.ContainsKey(name))
                throw new ScriptException(string.Format("函数{0}重复定义", name));

            _FuncDict.Add(name, statement);
        }

        /// <summary>
        /// 添加自定义函数
        /// </summary>
        /// <param name="statements"></param>
        private void _AddFunctionStatements(IEnumerable<FunctionStatement> statements)
        {
            foreach (FunctionStatement statement in statements)
            {
                _AddFunctionStatement(statement);
            }
        }

        private static LocalDataStoreSlot _LocalDataStoreSlot = Thread.AllocateDataSlot();

        internal void Initialize(Statement statement)
        {
            Thread.SetData(_LocalDataStoreSlot, this);
            RunningThread = Thread.CurrentThread;

            _FuncDict.Clear();
            StatementReader sr = new StatementReader(statement);
            MainBody = sr.MainBody;
            _AddFunctionStatements(sr.FunctionStatements);
            _AddClassStatements(sr.ClassStatements);
        }

        /// <summary>
        /// 执行当前脚本的线程
        /// </summary>
        internal Thread RunningThread { get; private set; }

        public static RunningContext Current
        {
            get
            {
                return (RunningContext)Thread.GetData(_LocalDataStoreSlot);
            }
        }

        /// <summary>
        /// 是否在上下文运行环境中
        /// </summary>
        public static bool IsInStatementContext
        {
            get
            {
                return Current != null;
            }
        }

        /// <summary>
        /// 当前的语句块
        /// </summary>
        public Statement CurrentStatement
        {
            get
            {
                return StatementStack.GetCurrent();
            }
        }

        /// <summary>
        /// 获取当前所缓存的局部变量的值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Value GetLocalValueFromCache(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            return LastValueCacheManager.GetValue(name);
        }

        private void _AddClassStatements(IEnumerable<ClassStatement> statements)
        {
            foreach (ClassStatement statement in statements)
            {
                _AddClassStatement(statement);
            }
        }

        private void _AddClassStatement(ClassStatement statement)
        {
            string name = statement.ClassName.ToUpper();
            if (_ClassDict.ContainsKey(name))
                throw new ScriptException(string.Format("类{0}重复定义", name));

            _ClassDict.Add(name, Class.BuildFromClassStatemet(statement));
        }

        private readonly Dictionary<string, Class> _ClassDict = new Dictionary<string, Class>();

        internal Class GetClass(string className)
        {
            Class classInfo;
            _ClassDict.TryGetValue(className.ToUpper(), out classInfo);
            return classInfo;
        }

        /// <summary>
        /// 获取当前的代码域
        /// </summary>
        /// <returns></returns>
        internal IStatementField GetCurrentField()
        {
            FunctionCallStackItem current = CallStack.Current;
            if (current != null)
                return current.Function;

            return MainBody;
        }

        /// <summary>
        /// 获取表达式的值
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        internal bool GetValue(string exp, out Value result, out ExpressionType expType)
        {
            return Expression.GetValue(ExpressionContext, exp, out result, out expType);
        }
    }
}
