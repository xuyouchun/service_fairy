using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Common.Script.VbScript.Statements.StatementAnalysers.StatementAnalyserStackContexts;

namespace Common.Script.VbScript.Statements.StatementAnalysers
{
    /// <summary>
    /// 语句块的分析栈
    /// </summary>
    class StatementAnalyserStack
    {
        public StatementAnalyserStack(IStatementCompileContext context)
        {
            _Context = context;
            _Stack.Push(new ComplexStatementAnalyserStackContext(this, context));
        }

        private readonly IStatementCompileContext _Context;

        /// <summary>
        /// 将一个表达式入栈
        /// </summary>
        /// <param name="expression"></param>
        public void PushExpression(Expression expression)
        {
            StatementAnalyserStackContext ctx = _Stack.Peek();
            ctx.PushExpression(expression);

            _CheckEnd();
        }

        /// <summary>
        /// 将一个语句块入栈
        /// </summary>
        /// <param name="keyword"></param>
        public void PushKeyword(Keywords keyword)
        {
            switch (keyword)
            {
                case Keywords.Dim:
                    _BeginNewPart(new DimStatementAnalyserStackContext(this, _Context));
                    break;

                case Keywords.Const:
                    _BeginNewPart(new ConstStatementAnalyserStackContext(this, _Context));
                    break;

                case Keywords.If:
                    _BeginNewPart(new IfStatementAnalyserStackContext(this, _Context));
                    break;

                case Keywords.ElseIf:
                    _Stack.Peek().PushKeyword(Keywords.Else);
                    _BeginNewPart(new IfStatementAnalyserStackContext(this, true, _Context));
                    break;

                case Keywords.While:
                    _BeginNewPart(new WhileStatementAnalyserStackContext(this, _Context));
                    break;

                case Keywords.Set:
                    _BeginNewPart(new AssignStatementAnalyserStackContext(this, _Context));
                    break;

                case Keywords.Loop:
                    _BeginNewPart(new LoopStatementAnalyserStackContext(this, _Context));
                    break;

                case Keywords.For:
                    _BeginNewPart(new ForStatementAnalyserStackContext(this, _Context));
                    break;

                case Keywords.Select:
                    _BeginNewPart(new SelectStatementAnalyserStackContext(this, _Context));
                    break;

                case Keywords.Function:
                    if (_Stack.Count != 1 && _Stack.Peek().StatementType != StatementType.Class)
                        throw new ScriptException("函数不允许定义在其它语句块内部");

                    _BeginNewPart(new FunctionStatementAnalyserStackContext(this, _GetOwnerClassName(), _Context));
                    break;

                case Keywords.Sub:
                    if (_Stack.Count != 1 && _Stack.Peek().StatementType != StatementType.Class)
                        throw new ScriptException("过程不允许定义在其它语句块内部");

                    _BeginNewPart(new SubStatementAnalyserStackContext(this, _GetOwnerClassName(), _Context));
                    break;

                case Keywords.Class:
                    if (_Stack.Count != 1)
                        throw new ScriptException("类不允许定义在其它语句块内部");
                    _BeginNewPart(new ClassStatementAnalyserStackContext(this, _Context));
                    break;

                case Keywords.Call:
                    _BeginNewPart(new CallStatementAnalyserStackContext(this, _Context));
                    break;

                case Keywords.GoTo:
                    _BeginNewPart(new GotoStatementAnalyserStackContext(this, _Context));
                    break;

                case Keywords.Label:
                    _BeginNewPart(new LabelStatementAnalyserStackContext(this, _Context));
                    break;

                case Keywords.OnError:
                    _BeginNewPart(new OnErrorStatementAnalyserStackContext(this, _Context));
                    break;
                    
                default:
                    _Stack.Peek().PushKeyword(keyword);
                    _CheckEnd();
                    break;
            }
        }

        private string _GetOwnerClassName()
        {
            ClassStatementAnalyserStackContext ctx = _Stack.Peek() as ClassStatementAnalyserStackContext;
            return ctx == null ? null : ctx.ClasName;
        }

        private void _CheckEnd()
        {
            while (_Stack.Count > 1 && _Stack.Peek().HasEnd())
            {
                _EndNewPart(_Stack.Peek().GetType());
            }
        }

        private readonly Stack<StatementAnalyserStackContext> _Stack = new Stack<StatementAnalyserStackContext>();

        private void _BeginNewPart(StatementAnalyserStackContext context)
        {
            _Stack.Push(context);
        }

        private void _EndNewPart(Type type)
        {
            if (_Stack.Count <= 1 || _Stack.Peek().GetType() != type)
                throw new ScriptException("语法错误");

            Statement statement = _Stack.Pop().GetStatement();
            if (statement is ConstStatement)
                _Stack.Peek().PushConstStatement((ConstStatement)statement);
            else
                _Stack.Peek().PushStatement(statement);
        }

        /// <summary>
        /// 判断当前语句块是否包含在指定类型的代码块中
        /// </summary>
        /// <param name="statementType"></param>
        /// <returns></returns>
        public bool ContainsInStatement(StatementType statementType)
        {
            foreach (StatementAnalyserStackContext ctx in _Stack.ToArray())
            {
                if (ctx.StatementType == statementType)
                    return true;
            }

            return false;
        }


        public StatementAnalyserStackContext[] GetStackContexts()
        {
            return _Stack.ToArray();
        }

        /// <summary>
        /// 获取最终的语句块
        /// </summary>
        /// <returns></returns>
        public Statement GetStatement()
        {
            if (_Stack.Count != 1)
                throw new ScriptException(_Stack.Peek().StatementType + "语句未结束");

            return _Stack.Peek().GetStatement();
        }

        public void BeginNewLine()
        {
            _Stack.Peek().BeginNewLine();
        }

        public void EndNewLine()
        {
            _Stack.Peek().EndNewLine();

            _CheckEnd();
        }

        /// <summary>
        /// 获取常量值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Value GetConstValue(string name)
        {
            return _Stack.Peek().GetConstValue(name);
        }
    }
}
