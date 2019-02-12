using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Common.Script.VbScript.Expressions;
using System.Threading;

namespace Common.Script.VbScript.Statements.StatementAnalysers
{
    /// <summary>
    /// 字符分析器分析出的一行
    /// </summary>
    unsafe class CharacterAnalyserLine
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="line">行文本</param>
        public CharacterAnalyserLine(char* ptr, int count, int lineNumber, IStatementCompileContext context)
        {
            _Ptr = ptr;
            _BeginPtr = _Ptr;
            _EndPtr = _Ptr + count;
            _LineNumber = lineNumber;
            _Context = context;
            Current = this;
        }

        private char* _Ptr, _BeginPtr, _EndPtr;
        private readonly int _LineNumber;
        private readonly IStatementCompileContext _Context;

        public int LineNumber { get { return _LineNumber; } }

        static readonly string _SetKeywordName = KeywordManager.GetKeywordInfo(Keywords.Set).Name;
        static readonly string _ForKeywordName = KeywordManager.GetKeywordInfo(Keywords.For).Name;
        static readonly string _IfKeywordName = KeywordManager.GetKeywordInfo(Keywords.If).Name;
        static readonly string _ThenKeywordName = KeywordManager.GetKeywordInfo(Keywords.Then).Name;
        static readonly string _ConstKeywordName = KeywordManager.GetKeywordInfo(Keywords.Const).Name;
        static readonly string _DimKeywordName = KeywordManager.GetKeywordInfo(Keywords.Dim).Name;

        /// <summary>
        /// 获取语句的各个部分
        /// </summary>
        /// <returns></returns>
        public IEnumerable GetParts()
        {
            string part;
            List<string> strParts = new List<string>();
            List<object> parts = new List<object>();
            Expression[] exps;

            while ((part = _GetNextPart()) != null)
            {
                KeywordInfo info;
                if (part == "=")  // 需要判断“=”是赋值操作还是等于判断
                {
                    string lastPart = parts.Count >= 1 ? parts[parts.Count - 1].ToString().ToUpper() : null;   // 最后一个部分
                    string firstPart = parts.Count >= 1 ? parts[0].ToString().ToUpper() : null;  // 第一个部分
                    object lastPart2 = parts.Count >= 2 ? parts[parts.Count - 2] : null;  // 倒数第二个部分
                    if (lastPart == _SetKeywordName || lastPart == _ForKeywordName || firstPart == _ConstKeywordName || firstPart == _DimKeywordName)  // 如果前面是SET、FOR、CONST、DIM，则其为赋值操作
                    {
                        foreach (Expression exp in _ParseToExceptions(strParts, parts))
                            yield return exp;

                        parts.Add(Keywords.Assign);
                        yield return Keywords.Assign;

                        continue;
                    }
                    else if ((lastPart == _ThenKeywordName || parts.Count == 0 || (lastPart2 is Keywords && (Keywords)lastPart2 == Keywords.Label))  // 跟在Then后面的等号为赋值，或在label后面的等号也为赋值
                        && (exps = _TryParseToExceptions(strParts)).Length == 1 && exps[0] is ILeftValueExpression)
                    {
                        foreach (object part0 in parts)
                        {
                            yield return part0;
                        }

                        parts.Clear();

                        parts.Add(Keywords.Set);
                        yield return Keywords.Set;

                        parts.Add(exps[0]);
                        yield return exps[0];

                        parts.Add(Keywords.Assign);
                        yield return Keywords.Assign;

                        strParts.Clear();

                        continue;
                    }

                    strParts.Add(part);
                }
                else if (part == ":")
                {
                    parts.Add(Keywords.Label);
                    yield return Keywords.Label;

                    foreach (Expression exp in _ParseToExceptions(strParts, parts))
                        yield return exp;

                    parts.Clear();
                }
                else if (strParts.Count > 0 && (info = KeywordManager.GetKeywordInfo(strParts[strParts.Count - 1] + " " + part)) != null)
                {
                    strParts.RemoveAt(strParts.Count - 1);
                    foreach (Expression exp in _ParseToExceptions(strParts, parts))
                        yield return exp;

                    parts.Add(info.Keyword);
                    yield return info.Keyword;
                }
                else if ((info = KeywordManager.GetKeywordInfo(part)) != null)
                {
                    foreach (Expression exp in _ParseToExceptions(strParts, parts))
                        yield return exp;

                    parts.Add(info.Keyword);
                    yield return info.Keyword;
                }
                else
                {
                    strParts.Add(part);
                }
            }

            foreach (Expression exp in _ParseToExceptions(strParts, parts))
                yield return exp;
        }

        private Expression[] _TryParseToExceptions(List<string> strParts)
        {
            if (strParts.Count == 0)
                return new Expression[0];

            try
            {
                return _CreateExpressions(string.Join(" ", strParts.ToArray()));
            }
            catch
            {
                return new Expression[0];
            }
        }

        private Expression[] _ParseToExceptions(List<string> strParts, List<object> results)
        {
            if (strParts.Count == 0)
                return new Expression[0];

            Expression[] exps = _CreateExpressions(string.Join(" ", strParts.ToArray()));
            strParts.Clear();
            results.AddRange(exps);
            return exps;
        }

        private Expression[] _CreateExpressions(string expStr)
        {
            expStr = expStr.Trim();
            if (expStr.Length == 0)
                return new Expression[0];

            try
            {
                return Expression.ParseExpressions(expStr, _Context.ExpressionCompileContext);
            }
            catch (ScriptException ex)
            {
                throw new ScriptException(string.Format("表达式“{0}”语法错误", expStr), ex);
            }
        }

        private string _GetNextPart()
        {
            while (_Ptr < _EndPtr)
            {
                if (!char.IsWhiteSpace(*_Ptr))
                    break;

                _Ptr++;
            }

            char* _Ptr0 = _Ptr;
            char ch;

            if (_Ptr >= _EndPtr)
                return null;

            while (_Ptr < _EndPtr)
            {
                ch = *_Ptr;
                if (ch == '"')
                {
                    if (_Ptr > _Ptr0)
                        break;

                    _Ptr++;
                    if (!StringUtility.SkipDlbQuot(ref _Ptr, _EndPtr))
                        break;
                }
                else if (ch == '\'' || (ch == 'R' || ch == 'r') && StringUtility.StartWith(_Ptr, _EndPtr, "REM", true) && char.IsWhiteSpace(*(_Ptr + 3)))
                {
                    string result = new string(_Ptr0, 0, (int)(_Ptr - _Ptr0));
                    _Ptr = _EndPtr;
                    return result;
                }
                else if (char.IsWhiteSpace(ch))
                {
                    break;
                }
                else if (ch == '(' || ch == ')' || ch == ':' || ch == '=' && !_IsPartOfOperator(_Ptr))
                {
                    if (_Ptr > _Ptr0)
                        break;

                    _Ptr++;
                    return ch.ToString();
                }

                _Ptr++;
            }

            return new string(_Ptr0, 0, (int)(_Ptr - _Ptr0));
        }

        // 用于决定当前的等号是否为操作符>=、<=的一部分
        private bool _IsPartOfOperator(char* p)
        {
            if (p > _BeginPtr)
            {
                char c = *(p - 1);
                if (c == '<' || c == '>')
                    return true;
            }

            if (p < _EndPtr - 1)
            {
                char c = *(p + 1);
                if (c == '<' || c == '>')
                    return true;
            }

            return false;
        }

        private bool _SkipToChar(char c)
        {
            char ch;
            while (_Ptr < _EndPtr)
            {
                ch = *_Ptr;
                if (ch == '\\')
                {
                    _Ptr++;
                    continue;
                }
                else if (ch == c)
                {
                    _Ptr++;
                    return true;
                }

                _Ptr++;
            }

            return false;
        }

        /// <summary>
        /// 转换为字符串形式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return new string(_BeginPtr, 0, (int)(_EndPtr - _BeginPtr));
        }

        private static readonly LocalDataStoreSlot _Slot = Thread.AllocateDataSlot();

        public static CharacterAnalyserLine Current
        {
            get
            {
                return Thread.GetData(_Slot) as CharacterAnalyserLine;
            }
            internal set
            {
                Thread.SetData(_Slot, value);
            }
        }
    }
}
