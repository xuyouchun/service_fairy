using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Statements;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 语句块
    /// </summary>
    public abstract class Statement
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Statement()
        {

        }

        /// <summary>
        /// 执行语句，留待派生类重写
        /// </summary>
        /// <param name="context"></param>
        protected abstract void OnExecute(RunningContext context);

        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="context">脚本的上下文执行环境</param>
        public bool Execute(RunningContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (context.CallStackDepth > 2000)
                throw new StackOverflowException("堆栈溢出");

            try
            {
                context.CallStackDepth++;

                do
                {
                    if (context.GoToLabelState != null)  // 从指定的标签开始执行，用于支持GOTO语句
                    {
                        GoToLabelState state = context.GoToLabelState;
                        Statement curSt = state.CurrentStatement;

                        if (curSt is IGotoSupported)
                        {
                            if (!context.BeforeExecuteStatement(this))
                                return false;

                            try
                            {
                                _ExecuteOnGotoSupported(context, curSt);
                                context.EndExecuteStatement(this, new StatementExecuteResult(null));
                            }
                            catch (Exception ex)
                            {
                                context.EndExecuteStatement(this, new StatementExecuteResult(ex));
                            }
                        }
                        else
                        {
                            if (!state.MoveNext())
                                context.GoToLabelState = null;

                            curSt.Execute(context);
                        }
                    }
                    else
                    {
                        if (!context.BeforeExecuteStatement(this))
                            return false;

                        try
                        {
                            if (this is IStatementField)
                                context.LastValueCacheManager.BeginLayer();

                            OnExecute(context);
                            context.EndExecuteStatement(this, new StatementExecuteResult(null));
                        }
                        catch (Exception ex)
                        {
                            context.EndExecuteStatement(this, new StatementExecuteResult(ex));
                        }
                        finally
                        {
                            if (this is IStatementField)
                                context.LastValueCacheManager.EndLayer();
                        }
                    }

                } while (_IsMetLabel(context));

                return !context.CheckExit();
            }
            finally
            {
                context.CallStackDepth--;
            }
        }

        private void _ExecuteOnGotoSupported(RunningContext context, Statement curSt)
        {
            GoToLabelState state = context.GoToLabelState;
            IGotoSupported gotoSta = (IGotoSupported)curSt;

            if (curSt != this && !context.BeforeExecuteStatement(curSt))
                return;

            try
            {
                if (this is IStatementField)
                    context.LastValueCacheManager.BeginLayer();

                state.MoveNext();
                Statement nextSt = state.CurrentStatement;

                gotoSta.Execute(context, nextSt);

                if (curSt != this)
                    context.EndExecuteStatement(curSt, new StatementExecuteResult(null));
            }
            catch (Exception ex)
            {
                if (curSt != this)
                    context.EndExecuteStatement(curSt, new StatementExecuteResult(ex));
            }
            finally
            {
                if (this is IStatementField)
                    context.LastValueCacheManager.EndLayer();
            }
        }

        private bool _IsMetLabel(RunningContext context)
        {
            if (context.GoToLabelState == null)
                return false;

            if (context.GoToLabelState.RootStatement == this)
            {
                context.GoToLabelState.HasExitToRoot = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取语句块的类型
        /// </summary>
        /// <returns></returns>
        internal abstract StatementType GetStatementType();

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns></returns>
        public abstract override string ToString();

        /// <summary>
        /// 用于保存一些与该语句块相关的信息
        /// </summary>
        public object Tag { get; private set; }

        /// <summary>
        /// 获取子语句
        /// </summary>
        /// <returns></returns>
        internal abstract IEnumerable<Statement> GetChildStatements();

        /// <summary>
        /// 获取所有的表达式
        /// </summary>
        /// <returns></returns>
        internal abstract IEnumerable<Expression> GetAllExpressions();

        /// <summary>
        /// 加载代码块
        /// </summary>
        /// <param name="script"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Statement Parse(string script, IStatementCompileContext context)
        {
            if (script == null)
                throw new ArgumentNullException("script");

            return new StatementAnalyser(script, context ?? new DefaultStatementCompileContext()).Parse();
        }

        /// <summary>
        /// 加载代码块
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public static Statement Parse(string script)
        {
            return Parse(script, new DefaultStatementCompileContext());
        }

        /// <summary>
        /// 空语句块
        /// </summary>
        public static readonly Statement Empty = new EmptyStatement();

        /// <summary>
        /// 获取所有引用到的函数名称
        /// </summary>
        /// <returns></returns>
        public FunctionInvokeInfo[] GetAllFunctionInvokeInfo()
        {
            List<FunctionInvokeInfo> infos = new List<FunctionInvokeInfo>();
            _FindFunctionInvokeInfo(this, infos);

            return infos.ToArray();
        }

        private static void _FindFunctionInvokeInfo(Statement statement, List<FunctionInvokeInfo> infos)
        {
            foreach (Expression exp in statement.GetAllExpressions())
            {
                _FindFunctionInvokeInfo(exp, infos);
            }

            foreach (Statement subStatement in statement.GetChildStatements())
            {
                _FindFunctionInvokeInfo(subStatement, infos);
            }
        }

        private static void _FindFunctionInvokeInfo(Expression exp, List<FunctionInvokeInfo> infos)
        {
            DynamicExpression dynaExp;
            FunctionExpression funcExp;
            if ((dynaExp = exp as DynamicExpression) != null)
                infos.Add(new FunctionInvokeInfo(dynaExp.Name, dynaExp.Parameters));
            else if ((funcExp = exp as FunctionExpression) != null)
                infos.Add(new FunctionInvokeInfo(funcExp.FunctionName, dynaExp.Parameters));

            foreach (Expression subExp in exp.GetAllSubExpressions())
            {
                _FindFunctionInvokeInfo(subExp, infos);
            }
        }

        /// <summary>
        /// 获取函数的定义信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FunctionDefineInfo GetFuncInfo(string name)
        {
            if (name == null)
                return null;

            FunctionStatement info;
            if (_GetFuncDict().TryGetValue(name.ToUpper(), out info))
                return new FunctionDefineInfo(info.Name, FunctionType.Private, string.Empty, string.Empty);

            return null;
        }

        /// <summary>
        /// 获取所有的函数定义信息
        /// </summary>
        /// <returns></returns>
        public FunctionDefineInfo[] GetAllFuncInfos()
        {
            List<FunctionDefineInfo> infos = new List<FunctionDefineInfo>();

            foreach (FunctionStatement statement in _GetFuncDict().Values)
            {
                infos.Add(new FunctionDefineInfo(statement.Name));
            }

            return infos.ToArray();
        }

        private Dictionary<string, FunctionStatement> _FuncDict;

        private Dictionary<string, FunctionStatement> _GetFuncDict()
        {
            var dict = _FuncDict;
            if (dict != null)
                return dict;

            dict = new Dictionary<string, FunctionStatement>();
            foreach (Statement statement in GetChildStatements())
            {
                FunctionStatement funcStatement = statement as FunctionStatement;
                if (funcStatement != null)
                    dict[funcStatement.Name.ToUpper()] = funcStatement; 
            }

            return _FuncDict = dict;
        }

        public VariableDefineInfo[] GetAllVariables()
        {
            List<VariableDefineInfo> infos = new List<VariableDefineInfo>();

            _AddDimInfo(infos, this);
            return infos.ToArray();
        }

        private void _AddDimInfo(List<VariableDefineInfo> infos, Statement statement)
        {
            DimStatement dim = statement as DimStatement;
            if (dim != null)
            {
                infos.AddRange(dim.Items);
            }
            else
            {
                foreach (Statement item in statement.GetChildStatements())
                {
                    _AddDimInfo(infos, item);
                }
            }
        }
    }
}
