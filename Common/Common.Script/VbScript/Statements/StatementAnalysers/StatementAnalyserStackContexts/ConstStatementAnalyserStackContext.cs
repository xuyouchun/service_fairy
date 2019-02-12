using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;

namespace Common.Script.VbScript.Statements.StatementAnalysers.StatementAnalyserStackContexts
{
    class ConstStatementAnalyserStackContext : StatementAnalyserStackContext
    {
        public ConstStatementAnalyserStackContext(StatementAnalyserStack analyserStack, IStatementCompileContext context)
            : base(analyserStack, StatementType.Const, context)
        {

        }

        private List<VariableDefineInfo> _Items = new List<VariableDefineInfo>();
        private bool _HasEnd = false;
        private bool _MetAssignOp = false;

        public override void PushExpression(Expression expression)
        {
            if (!_MetAssignOp)  // 等号之前
            {
                VariableExpression varExp = StatementAnalyserStackContextUtility.ConvertToVariableExpression(expression);
                if (varExp != null)
                {
                    if (_ContainsName(varExp.Name))
                        throw new ScriptException(string.Format("常量“{0}”被重复定义", varExp.Name));

                    _Items.Add(new VariableDefineInfo(varExp, null));
                }
                else
                {
                    throw new ScriptException("定义常量时语法错误：" + expression);
                }
            }
            else // 等号之后
            {
                if (_Items.Count == 0 || _Items[_Items.Count - 1].Value != null)
                    throw new ScriptException("CONST语句中等号语法错误");

                VariableDefineInfo item = _Items[_Items.Count - 1];
                _Items.RemoveAt(_Items.Count - 1);
                _Items.Add(new VariableDefineInfo(item.Variable, expression));

                _MetAssignOp = false;
            }
        }

        private bool _ContainsName(string name)
        {
            foreach (var item in _Items)
            {
                if (string.Equals(name, item.Variable.Name, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        public override bool PushKeyword(Keywords keyword)
        {
            if (keyword == Keywords.Assign)
            {
                if (_Items.Count == 0 || _Items[_Items.Count - 1].Value != null)
                {
                    throw new ScriptException("CONST语句中等号语法错误");
                }
                else
                {
                    if (_MetAssignOp)
                        throw new ScriptException("CONST语句中遇到多余的等号");

                    _MetAssignOp = true;
                    return true;
                }
            }
            else
            {
                throw new ScriptException("CONST语句中遇到不能识别的关键字“" + KeywordManager.GetKeywordInfo(keyword).Name + "”");
            }
        }

        public override void PushStatement(Statement statement)
        {
            throw new ScriptException("CONST语句声明语法错误");
        }

        public override bool HasEnd()
        {
            return _HasEnd;
        }

        public override void EndNewLine()
        {
            _HasEnd = true;
        }

        public override Statement GetStatement()
        {
            if (_Items.Count == 0)
                throw new ScriptException("CONST语句未指定要声明的变量");

            return new ConstStatement(_Items.ToArray());
        }
    }
}
