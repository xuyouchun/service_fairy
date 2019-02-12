using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Statements.StatementAnalysers
{
    /// <summary>
    /// 用于字符分析的执行器
    /// </summary>
    unsafe class CharacterAnalyserExecutor
    {
        public CharacterAnalyserExecutor(char* ptr, IStatementCompileContext context)
        {
            _Ptr = ptr;
            _Context = context;
        }

        private readonly char* _Ptr;
        private readonly IStatementCompileContext _Context;

        /// <summary>
        /// 执行
        /// </summary>
        public void Analyse()
        {
            char* ptr = _Ptr;
            char* ptrStart;
            int count, lineNum = 0;

            try
            {
                while ((_GetNextLine(ref ptr, out ptrStart, out count)))
                {
                    lineNum++;
                    if (count == 0 || _IsRemarkLine(ptrStart, count))
                        continue;

                    _RaiseBeginNewLineEvent();

                    CharacterAnalyserLine lineInfo = new CharacterAnalyserLine(ptrStart, count, lineNum, _Context);
                    foreach (object part in lineInfo.GetParts())
                    {
                        Expression exp = part as Expression;
                        if (exp != null)
                            _RaiseNewExpressionEvent(exp);
                        else
                            _RaiseNewKeywordEvent((Keywords)part);
                    }

                    _RaiseEndNewLineEvent();
                }
            }
            finally
            {
                CharacterAnalyserLine.Current = null;
            }
        }

        private bool _IsRemarkLine(char* p, int count)
        {
            char* ptrEnd = p + count;
            while (p < ptrEnd)  // 越过之前的空格
            {
                if (!char.IsWhiteSpace(*p))
                    break;

                p++;
            }

            if (*p == '\'')
                return true;

            return StringUtility.StartWith(p, ptrEnd, "REM", true) && char.IsWhiteSpace(*(p + 3));
        }

        private bool _GetNextLine(ref char* ptr, out char* ptrStart, out int count)
        {
            if (*ptr == '\0')
            {
                ptrStart = null;
                count = 0;
                return false;
            }

            StringUtility.SkipWhiteSpace(ref ptr);
            ptrStart = ptr;
            char ch;
            bool _IsInQuot = false;

            while ((ch = *ptr) != '\0')
            {
                if (ch == '"')
                {
                    _IsInQuot = !_IsInQuot;
                }
                else if (ch == '\r' || ch == '\n')// || ch == ':' && !_IsInQuot)
                {
                    count = (int)(ptr - ptrStart);
                    _SkipLineChar(ref ptr);
                    return true;
                }

                //else if (ch == '_' && !_IsInQuot && _IsJoinLineChar(ptr) && !_IsRemarkLine(ptrStart, (int)(ptr - ptrStart + 1)))
                //{
                //    ptr++;
                //    StringUtility.SkipWhiteSpace(ref ptr);
                //    _SkipLineChar(ref ptr);
                //    continue;
                //}

                ptr++;
            }

            count = (int)(ptr - ptrStart);
            return true;
        }

        //private bool _IsJoinLineChar(char* ptr)
        //{
        //    if (ptr > _Ptr && StringUtility.IsVariableChar(*(ptr - 1)) && *(ptr - 1) != '_')
        //        return false;

        //    if (*ptr++ != '_')
        //        return false;

        //    char ch;
        //    while ((ch = *ptr) != '\0')
        //    {
        //        if (ch == '\r' || ch == '\n')
        //            return true;

        //        if (!char.IsWhiteSpace(ch))
        //            return false;

        //        ptr++;
        //    }

        //    return true;
        //}


        /// <summary>
        /// 跳过回车换行键
        /// </summary>
        /// <param name="ptr"></param>
        public static void _SkipLineChar(ref char* ptr)
        {
            char ch;

            while ((ch = *ptr) != '\0')
            {
                if (ch == '\r' || ch == '\n')// || ch == ':')
                {
                    ptr++;
                    continue;
                }

                return;
            }
        }

        #region Events ...

        /// <summary>
        /// 生成新的语句块
        /// </summary>
        public event CharacterAnalyserExecutorNewKeywordEventHandler NewKeyword;

        private void _RaiseNewKeywordEvent(Keywords keyword)
        {
            var eh = NewKeyword;
            if (eh != null)
                eh(this, new CharacterAnalyserExecutorNewKeywordEventArgs(keyword));
        }

        /// <summary>
        /// 生成新的表达式
        /// </summary>
        public event CharacterAnalyserStackNewExpressionEventHandler NewExpression;

        private void _RaiseNewExpressionEvent(Expression exp)
        {
            var eh = NewExpression;
            if (eh != null)
                eh(this, new CharacterAnalyserStackNewExpressionEventArgs(exp));
        }

        /// <summary>
        /// 开始新的行
        /// </summary>
        public event EventHandler BeginNewLine;

        private void _RaiseBeginNewLineEvent()
        {
            var eh = BeginNewLine;
            if (eh != null)
                eh(this, EventArgs.Empty);
        }

        /// <summary>
        /// 结束新的行
        /// </summary>
        public event EventHandler EndNewLine;

        private void _RaiseEndNewLineEvent()
        {
            var eh = EndNewLine;
            if (eh != null)
                eh(this, EventArgs.Empty);
        }

        #endregion

    }
}
