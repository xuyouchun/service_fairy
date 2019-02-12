using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Expressions;
using Common.Script.VbScript.Values;

namespace Common.Script.VbScript.Statements.StatementAnalysers.StatementAnalyserStackContexts
{
    /// <summary>
    /// 控制类的创建过程
    /// </summary>
    class ClassStatementAnalyserStackContext : StatementAnalyserStackContext
    {
        public ClassStatementAnalyserStackContext(StatementAnalyserStack analyserStack, IStatementCompileContext context)
            : base(analyserStack, StatementType.Class, context)
        {

        }

        public string ClasName { get { return _ClassName; } }

        private string _ClassName;
        private bool _HasEnd = false;
        private ObjectMemberAccessRight? _AccessRight = null;
        public ObjectMemberAccessRight _DefaultAccessRight = ObjectMemberAccessRight.Private;
        private readonly List<VariableClassMember> _VariableMembers = new List<VariableClassMember>();
        private readonly List<FunctionClassMember> _FunctionMembers = new List<FunctionClassMember>();

        private ObjectMemberAccessRight _GetCurrentAccessRight()
        {
            return _AccessRight == null ? _DefaultAccessRight : (ObjectMemberAccessRight)_AccessRight;
        }

        public override void PushExpression(Expression expression)
        {
            VariableExpression varExpression;

            if (_ClassName == null)
            {
                VariableExpression varExp = expression as VariableExpression;
                if (varExp == null)
                    throw new ScriptException("类名定义错误: " + expression);

                _ClassName = varExp.Name;
            }
            else if ((varExpression = StatementAnalyserStackContextUtility.ConvertToVariableExpression(expression)) != null)
            {
                _VariableMembers.Add(new VariableClassMember(varExpression, _GetCurrentAccessRight()));
            }
            else
            {
                throw new ScriptException(string.Format("类{0}定义内部遇到了非法的表达式 {1}", _ClassName, expression));
            }
        }

        public override void PushStatement(Statement statement)
        {
            if (_ClassName == null)
                throw new ScriptException("类定义时未指定类名");

            FunctionStatement funcStatement;
            if ((funcStatement = statement as FunctionStatement) != null)
            {
                _FunctionMembers.Add(new FunctionClassMember(funcStatement, _GetCurrentAccessRight()));
            }
            else
            {
                throw new ScriptException(string.Format("类{0}定义内部遇到了非法的语句块 {1}", _ClassName, statement));
            }
        }

        public override bool PushKeyword(Keywords keyword)
        {
            if (keyword == Keywords.EndClass)
            {
                _HasEnd = true;
            }
            else if (keyword == Keywords.Private || keyword == Keywords.Public)
            {
                if (_AccessRight != null)
                    throw new ScriptException(string.Format("在类{0}定义中遇到重复的访问修饰符", _ClassName));

                _AccessRight = keyword == Keywords.Public ? ObjectMemberAccessRight.Public : ObjectMemberAccessRight.Private;
            }
            else
            {
                throw new ScriptException(string.Format("在类{0}定义中遇到不可识别的关键字{1}",
                    _ClassName, KeywordManager.GetKeywordInfo(keyword).Name));
            }

            return true;
        }

        public override void EndNewLine()
        {
            _AccessRight = null;
        }

        public override bool HasEnd()
        {
            return _HasEnd;
        }

        public override Statement GetStatement()
        {
            if (_ClassName == null)
                throw new ScriptException("类定义时未指定类名");

            return new ClassStatement(_ClassName, _VariableMembers.ToArray(), _FunctionMembers.ToArray());
        }
    }
}
