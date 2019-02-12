using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Statements.StatementAnalysers
{
    abstract class StatementAnalyserStackContext
    {
        public StatementAnalyserStackContext(StatementAnalyserStack analyserStack, StatementType statementType, IStatementCompileContext context)
        {
            Context = context;

            AnalyserStack = analyserStack;
            StatementType = statementType;

            Expressions = new List<Expression>();
            Statements = new List<Statement>();
        }

        /// <summary>
        /// 上下文环境
        /// </summary>
        protected IStatementCompileContext Context { get; private set; }

        /// <summary>
        /// 表达式的集合
        /// </summary>
        protected List<Expression> Expressions { get; private set; }

        /// <summary>
        /// 语句块的集合
        /// </summary>
        protected List<Statement> Statements { get; private set; }

        /// <summary>
        /// 语句块的类型
        /// </summary>
        public StatementType StatementType { get; protected set; }

        /// <summary>
        /// 分析栈
        /// </summary>
        public StatementAnalyserStack AnalyserStack { get; private set; }

        /// <summary>
        /// 将一个表达式入栈
        /// </summary>
        /// <param name="expression"></param>
        public virtual void PushExpression(Expression expression)
        {
            Expressions.Add(expression);
        }

        /// <summary>
        /// 将一个语句块入栈
        /// </summary>
        /// <param name="statement"></param>
        public virtual void PushStatement(Statement statement)
        {
            Statements.Add(statement);
        }

        /// <summary>
        /// 将一个关键字入栈
        /// </summary>
        /// <param name="keyword"></param>
        public virtual bool PushKeyword(Keywords keyword)
        {
            switch (keyword)
            {
                case Keywords.ExitFor:
                    if (!AnalyserStack.ContainsInStatement(StatementType.For))
                        throw new ScriptException("EXIT FOR语句不包含在FOR语句中");

                    PushStatement(new ExitStatement(StatementType.For));
                    break;

                case Keywords.ExitLoop:
                    if (!AnalyserStack.ContainsInStatement(StatementType.Loop))
                        throw new ScriptException("EXIT LOOP语句不包含在LOOP语句中");

                    PushStatement(new ExitStatement(StatementType.Loop));
                    break;

                case Keywords.ExitWhile:
                    if (!AnalyserStack.ContainsInStatement(StatementType.While))
                        throw new ScriptException("EXIT WHILE语句不包含在WHILE语句中");

                    PushStatement(new ExitStatement(StatementType.While));
                    break;

                case Keywords.ExitFunction:
                    if (!AnalyserStack.ContainsInStatement(StatementType.Function))
                        throw new ScriptException("EXIT FUNCTION语句不包含在FUNCTION定义中");

                    PushStatement(new ExitStatement(StatementType.Function));
                    break;
                    
                case Keywords.ExitSub:
                    if (!AnalyserStack.ContainsInStatement(StatementType.Sub))
                        throw new ScriptException("EXIT SUB语句不包含在SUB定义中");

                    PushStatement(new ExitStatement(StatementType.Sub));
                    break;

                case Keywords.OptionExplicit:
                    PushStatement(new OptionExplicitStatement());
                    break;

                default:
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 开始一行
        /// </summary>
        public virtual void BeginNewLine()
        {

        }

        /// <summary>
        /// 结束一行
        /// </summary>
        public virtual void EndNewLine()
        {

        }

        public virtual Statement GetStatement()
        {
            return StatementInfoManager.GetInfo(StatementType)
                .CreateStatement(Expressions.ToArray(), Statements.ToArray());
        }

        protected SimpleStatement CreateSimpleStatement(Expression exp)
        {
            if (exp is FunctionExpression || exp is MemberExpression || exp is DynamicExpression)
                return new SimpleStatement(exp);                

            throw new ScriptException(string.Format("单个表达式{0}不可以作为一条语句", exp));
        }

        public virtual bool HasEnd()
        {
            return false;
        }

        private readonly Dictionary<string, Value> _ConstDict = new Dictionary<string, Value>();

        /// <summary>
        /// 常量表达式
        /// </summary>
        /// <param name="statement"></param>
        public void PushConstStatement(ConstStatement statement)
        {
            foreach (var item in statement.Items)
            {
                string name = item.Name.ToUpper();
                if (_ConstDict.ContainsKey(name))
                    throw new ScriptException(string.Format("常量{0}重复定义", name));

                _ConstDict.Add(name, item.Value.Execute());
            }
        }

        public Value GetConstValue(string name)
        {
            foreach (StatementAnalyserStackContext ctx in AnalyserStack.GetStackContexts())
            {
                Value value;
                if (ctx._ConstDict.TryGetValue(name.ToUpper(), out value))
                    return value;
            }

            Value v = Context.ExpressionCompileContext.GetConstValue(name);
            if ((object)v == null)
                return null;

            return v;
        }

        //public Expression FixExpression(Expression exp)
        //{
        //    if (exp.GetType() == typeof(VariableExpression))
        //    {
        //        VariableExpression varExp = (VariableExpression)exp;
        //        Value value = GetConstValue(varExp.Name);
        //        if (value != null)
        //            return new ConstValueExpression(varExp.Name, value);
        //    }

        //    return exp;
        //}
    }
}
