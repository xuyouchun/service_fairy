using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements.StatementAnalysers.StatementAnalyserStackContexts
{
    class SelectStatementAnalyserStackContext : StatementAnalyserStackContext
    {
        public SelectStatementAnalyserStackContext(StatementAnalyserStack analyserStack, IStatementCompileContext context)
            : base(analyserStack, StatementType.Select, context)
        {

        }

        #region Class CaseStatementPair ...

        class CaseStatementPair
        {
            public CaseStatementPair()
                : this(false)
            {

            }

            public CaseStatementPair(bool isCaseElse)
            {
                IsCaseElse = isCaseElse;
                StatementBuilder = new ComplexStatementBuilder();
            }

            // 是否为Case Else
            public bool IsCaseElse { get; private set; }

            /// <summary>
            /// 表达式
            /// </summary>
            public Expression Expression { get; set; }

            /// <summary>
            /// 语句块
            /// </summary>
            public ComplexStatementBuilder StatementBuilder { get; private set; }
        }

        #endregion

        private Expression _SelectExpression;
        private readonly List<CaseStatementPair> _CaseStatementPairs = new List<CaseStatementPair>();
        private bool _HasEnd = false;
        private bool _IsMetFirstCase = false, _IsMetCaseElse = false;

        private CaseStatementPair _LastPair
        {
            get
            {
                if (_CaseStatementPairs.Count == 0)
                    return null;

                return _CaseStatementPairs[_CaseStatementPairs.Count - 1];
            }
        }

        public override void PushExpression(Expression expression)
        {
            if (_SelectExpression == null)
            {
                _SelectExpression = expression;
                _IsMetFirstCase = true;  // SELECT之后的CASE允许省略
            }
            else
            {
                if (_CaseStatementPairs.Count == 0)
                    throw new ScriptException("SELECT语句语法错误，遗漏了CASE关键字");

                CaseStatementPair pair = _LastPair;
                if (pair.Expression == null)
                    pair.Expression = expression;
                else
                    pair.StatementBuilder.AddStatement(CreateSimpleStatement(expression));
            }
        }

        public override void PushStatement(Statement statement)
        {
            
        }

        public override bool PushKeyword(Keywords keyword)
        {
            if (keyword == Keywords.Case)
            {
                if (_SelectExpression == null)
                {
                    if (_IsMetFirstCase)
                        throw new ScriptException("SELECT语句语法错误，遇到多余的CASE");

                    _IsMetFirstCase = true;
                }
                else
                {
                    if (_CaseStatementPairs.Count > 0 && _LastPair.Expression == null)
                        throw new ScriptException("SELECT语句语法错误，CASE语句遗漏了表达式");

                    _CaseStatementPairs.Add(new CaseStatementPair());
                }
            }
            else if (keyword == Keywords.Else)
            {
                if (_CaseStatementPairs.Count > 0)
                {
                    CaseStatementPair pair = _LastPair;
                    if (pair.Expression != null)
                        throw new ScriptException("SELECT语句中CASE ELSE语法错误");

                    _CaseStatementPairs.RemoveAt(_CaseStatementPairs.Count - 1);
                }

                if (_IsMetCaseElse)
                    throw new ScriptException("SELECT语句中遇到多余的CASE ELSE");

                _IsMetCaseElse = true;
                _CaseStatementPairs.Add(new CaseStatementPair(true) { Expression = Expression.Empty });
            }
            else if (keyword == Keywords.EndSelect)
            {
                _HasEnd = true;
            }
            else
            {
                if (!base.PushKeyword(keyword))
                    throw new ScriptException("SELECT语句中遇到了不能识别的关键字" + KeywordManager.GetKeywordInfo(keyword));
            }

            return true;
        }

        public override bool HasEnd()
        {
            return _HasEnd;
        }

        public override Statement GetStatement()
        {
            if (_SelectExpression == null)
                throw new ScriptException("SELECT语句中未指定表达式");

            Expressions.Add(_SelectExpression);

            int _CaseElsePairIndex = -1;
            CaseStatementPair[] pairs = _CaseStatementPairs.ToArray();
            for (int k = 0; k < pairs.Length; k++)
            {
                CaseStatementPair pair = pairs[k];
                if (pair.IsCaseElse)
                {
                    _CaseElsePairIndex = k;
                }
                else
                {
                    if (pair.Expression == null)
                        throw new ScriptException("SELECT语句中CASE未指定表达式");

                    Expressions.Add(pair.Expression);
                    Statements.Add(_GetCaseStatement(pairs, k));
                }
            }

            Statements.Add(_CaseElsePairIndex < 0 ? Statement.Empty : _GetCaseStatement(pairs, _CaseElsePairIndex));

            return base.GetStatement();
        }

        // 可能遇到空的Case语句块，将向后查找，直到找到一个非空语句块为止
        private Statement _GetCaseStatement(CaseStatementPair[] pairs, int k)
        {
            CaseStatementPair pair = pairs[k];
            if (!pair.StatementBuilder.IsEmpty)
                return pair.StatementBuilder.GetStatement();

            for (int i = k + 1; i < pairs.Length; i++)
            {
                if (!pairs[i].StatementBuilder.IsEmpty)
                    return pairs[i].StatementBuilder.GetStatement();
            }

            return Statement.Empty;
        }
    }
}
