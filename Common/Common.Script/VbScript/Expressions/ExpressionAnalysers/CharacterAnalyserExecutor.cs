using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.ExpressionAnalysers
{
    /// <summary>
    /// 用于分析字符串表达式的堆栈
    /// </summary>
    unsafe class CharacterAnalyserExecutor
    {
        public CharacterAnalyserExecutor(char* ptr)
        {
            _Ptr = ptr;
        }

        private char* _Ptr;

        /// <summary>
        /// 指针的当前位置
        /// </summary>
        public char* Ptr { get { return _Ptr; } }

        /// <summary>
        /// 记录之前所分析的是否为一个表达式，用于在分析"-"是一元运算符还是二元运算符
        /// </summary>
        private bool _PreviousIsExpression = false;

        /// <summary>
        /// 加载表达式
        /// </summary>
        /// <returns></returns>
        public void Analyse()
        {
            char ch;

            do
            {
            startLabel:
                StringUtility.SkipWhiteSpace(ref _Ptr);
                if ((ch = *_Ptr) == '\0')
                    break;

                switch (ch)
                {
                    case '(':
                        _RaiseBeginNewExpressionGroupEvent();
                        _Ptr++;
                        goto startLabel;

                    case ')':
                        _RaiseEndNewExpressionGroupEvent();
                        _Ptr++;
                        goto startLabel;

                    case ',':
                        _PreviousIsExpression = false;
                        _RaiseEndExpressionEvent();
                        _Ptr++;
                        goto startLabel;
                }

            } while (_Analyse());
        }

        /// <summary>
        /// 分析指定的表达式字符串
        /// </summary>
        /// <param name="_Ptr"></param>
        private bool _Analyse()
        {
            char ch;
            if (!StringUtility.SkipWhiteSpace(ref _Ptr))
                return false;

            if ((ch = *_Ptr) != '\0')
            {
                if (char.IsWhiteSpace(ch))
                {
                    _Ptr++;
                    return true;
                }

                OperatorInfo opInfo = _PickOperator();
                if (opInfo != null)  // 是操作符
                {
                    _RaiseNewOperatorEvent(opInfo);
                    return true;
                }
                else  // 不是操作符
                {
                    if (char.IsNumber(ch) || ch == '-')  // 数字
                    {
                        _PickNumber(ref _Ptr);
                    }
                    else if (ch == '"')
                    {
                        _PickString(ref _Ptr);
                    }
                    else
                    {
                        _PickVariableOrDynamic(ref _Ptr);
                    }
                }

                return *_Ptr != '\0';
            }

            return false;
        }

        /// <summary>
        /// 提取数字
        /// </summary>
        /// <param name="_Ptr"></param>
        private void _PickNumber(ref char* _Ptr)
        {
            StringBuilder sb = new StringBuilder();
            char ch;
            bool metDot = false, metE = false, metPlus = false, metNumAfterPlus = false;

            if (*_Ptr == '0' && *(_Ptr + 1) == 'x' || *(_Ptr + 1) == 'X') // 十六进制数字
            {
                #region 转换十六进制数据  ...

                _Ptr += 2;
                while ((ch = *_Ptr) != '\0')
                {
                    if (char.IsNumber(ch) || ch <= 'f' && ch >= 'a' || ch <= 'F' && ch >= 'A')
                        sb.Append(ch);
                    else if (sb.Length == 0)
                        throw new ScriptException("十六进制数据格式错误");
                    else break;

                    _Ptr++;
                }

                long value = Convert.ToInt64(sb.ToString(), 16);
                Value v = value <= int.MaxValue && value >= int.MinValue ? (Value)(int)value : (Value)value;
                _RaiseNewExpressionEvent(new ValueExpression(v));

                #endregion
            }
            else  // 普通的数字
            {
                #region 读取普通的数字 ...

                while ((ch = *_Ptr) != '\0')
                {
                    if (char.IsNumber(ch))
                    {
                        sb.Append(ch);

                        if (metPlus)
                            metNumAfterPlus = true;

                        _Ptr++;
                    }
                    else if (ch == '.')
                    {
                        if (metDot)
                            throw new ScriptException("数字格式错误");

                        metDot = true;
                        sb.Append(".");
                        _Ptr++;
                    }
                    else if (ch == 'e' || ch == 'E')
                    {
                        if (metE)
                            throw new ScriptException("数字格式错误");

                        metE = true;
                        sb.Append(ch);
                        _Ptr++;
                    }
                    else if (ch == '+' || ch == '-')
                    {
                        if (!(ch == '-' && sb.Length == 0))  // 第一个‘-’是合法的
                        {
                            if (!metE && sb.Length > 0)
                                break;

                            if (!metE || metPlus)
                                throw new ScriptException("数字格式错误");
                        }

                        metPlus = true;
                        sb.Append(ch);
                        _Ptr++;
                        StringUtility.SkipWhiteSpace(ref _Ptr);  // 允许负号与之后的部分有空格，此处将空格跳过
                    }
                    else
                    {
                        break;
                    }
                }

                if (metPlus && !metNumAfterPlus)
                    throw new ScriptException("数字格式错误");

                if (sb.Length == 0)
                    return;

                Value value = _ConvertToNumber(sb.ToString());
                _RaiseNewExpressionEvent(new ValueExpression(value));

                #endregion
            }
        }

        /// <summary>
        /// 提取字符串
        /// </summary>
        /// <param name="_Ptr"></param>
        /// <param name="ch"></param>
        private void _PickString(ref char* _Ptr)
        {
            StringBuilder sb = new StringBuilder();
            char ch;
            _Ptr++;

            while ((ch = *_Ptr) != '\0')
            {
                if (ch == '"')
                {
                    _Ptr++;
                    break;
                }

                sb.Append(ch);
                _Ptr++;
            }

            _RaiseNewExpressionEvent(new ValueExpression(new Value(sb.ToString())));
        }

        private Value _ConvertToNumber(string str)
        {
            int index = str.IndexOfAny(new char[] { 'E', 'e' });
            if (index < 0)
            {
                if (str.IndexOf('.') < 0)
                {
                    long result = long.Parse(str);
                    if (result <= int.MaxValue && result >= int.MinValue)
                        return (int)result;

                    return result;
                }

                return double.Parse(str);
            }

            string part1 = str.Substring(0, index), part2 = str.Substring(index + 1);
            return Math.Pow(double.Parse(part1), double.Parse(part2));
        }

        /// <summary>
        /// 识别变量或动态类型（函数、数组）
        /// </summary>
        /// <param name="_Ptr"></param>
        private void _PickVariableOrDynamic(ref char* _Ptr)
        {
            char ch = *_Ptr;

            if (ch == '[')  // 中括号引起来的为变量名
            {
                char* ptr = _Ptr++;
                if (!StringUtility.SkipTo(ref _Ptr, ']'))
                    throw new ScriptException("用中括号括起来的变量遗漏了右中括号“]”");

                string varName = new string(ptr + 1, 0, (int)(_Ptr - ptr - 1));
                _DealNewVariable(varName);
                _Ptr++;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                bool metLetter = false;

                while ((ch = *_Ptr) != '\0')
                {
                    if (_IsVarName(ch) || char.IsNumber(ch) && metLetter)  // 变量名称为字母、下划线开头，之后可以是数字
                    {
                        metLetter = true;
                        sb.Append(ch);
                        _Ptr++;
                    }
                    else
                    {
                        break;
                    }
                }

                string s = sb.ToString().Trim();
                if (s.Length == 0)
                    throw new ScriptException("遇到不能解析的字符“" + *_Ptr + "”");

                StringUtility.SkipWhiteSpace(ref _Ptr);
                if (*_Ptr == '(') // 说明是函数
                {
                    _Ptr++;
                    _RaiseNewDynamicExpressionEvent(s);
                    return;
                }

                _DealNewVariable(s);
            }
        }

        private void _DealNewVariable(string s)
        {
            Expression exp;

            switch (s.ToUpper())
            {
                case "TRUE":
                    exp = new ValueExpression(new Value(true));
                    break;

                case "FALSE":
                    exp = new ValueExpression(new Value(false));
                    break;

                case "NOTHING":
                case "EMPTY":
                case "NULL":
                    exp = new ValueExpression(Value.Void);
                    break;

                default:
                    exp = new VariableExpression(s);
                    break;
            }

            _RaiseNewExpressionEvent(exp);
        }

        private static bool _IsVarName(char ch)
        {
            return StringUtility.IsVariableChar(ch);
        }

        private OperatorInfo _PickOperator()
        {
            if (char.IsNumber(*_Ptr))
                return null;

            char ch = *_Ptr;
            if (ch == '-')  // 对于减号做特殊处理，判断其为减号或为负号
            {
                if (_PreviousIsExpression)
                {
                    _Ptr++;
                    return OperatorInfoManager.GetInfo(Operators.Substract);
                }

                if (char.IsNumber(StringUtility.GetNextCharSkipWhiteSpace(_Ptr + 1)))  // 如果后面是数字，则该负号与数字为一个整体
                    return null;

                _Ptr++;
                return OperatorInfoManager.GetInfo(Operators.Minus);
            }

            string str = _GetWord(_Ptr);
            OperatorInfo opInfo = OperatorInfoManager.GetInfo(str.ToUpper());
            if (opInfo != null)
            {
                _Ptr += str.Length;
                return opInfo;
            }

            if (char.IsLetter(ch))
                return null;

            char ch2 = *(_Ptr + 1);
            if (ch2 != '\0')
            {
                opInfo = OperatorInfoManager.GetInfo(ch.ToString() + ch2);
                if (opInfo != null)
                {
                    _Ptr += 2;
                    return opInfo;
                }
            }

            opInfo = OperatorInfoManager.GetInfo(ch.ToString());
            if (opInfo != null)
            {
                _Ptr++;
                return opInfo;
            }

            //if (_InvalidOperatorChars.Contains(ch))
            //    throw new ScriptException("遇到不能识别的字符“" + ch + "”");

            return null;
        }

        //static readonly ContainsList<char> _InvalidOperatorChars = new ContainsList<char>(new char[] {
        //    '#', '$', ',', '.', ':', ';', '?', '@', '[', ']', '\\', '`', '{', '}', '~'});

        private string _GetWord(char* ptr)
        {
            char* ptr0 = ptr;
            while (*ptr != '\0')
            {
                if (char.IsWhiteSpace(*ptr))
                    break;

                ptr++;
            }

            return new string(ptr0, 0, (int)(ptr - ptr0));
        }

        #region Events ...

        /// <summary>
        /// 生成新的表达式
        /// </summary>
        public event CharacterAnalyserStackNewExpressionEventHandler NewExpression;

        /// <summary>
        /// 生成新的操作符
        /// </summary>
        public event CharacterAnalyserStackNewOperatorEventHandler NewOperator;

        /// <summary>
        /// 生成新的函数表达式
        /// </summary>
        public event CharacterAnalyserStackNewDynamicExpressionEventHandler NewDynamicExpression;

        /// <summary>
        /// 开始新的组
        /// </summary>
        public event EventHandler BeginNewExpressionGroup;

        /// <summary>
        /// 结束新的组
        /// </summary>
        public event EventHandler EndNewExpressionGroup;

        /// <summary>
        /// 结束当前的表达式
        /// </summary>
        public event EventHandler EndExpression;

        private void _RaiseBeginNewExpressionGroupEvent()
        {
            var eh = BeginNewExpressionGroup;
            if (eh != null)
                eh(this, EventArgs.Empty);
        }

        private void _RaiseEndNewExpressionGroupEvent()
        {
            var eh = EndNewExpressionGroup;
            if (eh != null)
                eh(this, EventArgs.Empty);
        }

        private void _RaiseEndExpressionEvent()
        {
            var eh = EndExpression;
            if (eh != null)
                eh(this, EventArgs.Empty);
        }

        private void _RaiseNewExpressionEvent(Expression exp)
        {
            _PreviousIsExpression = true;

            var eh = NewExpression;
            if (eh != null)
                eh(this, new CharacterAnalyserStackNewExpressionEventArgs(exp));
        }

        private void _RaiseNewOperatorEvent(OperatorInfo info)
        {
            _PreviousIsExpression = false;

            var eh = NewOperator;
            if (eh != null)
                eh(this, new CharacterAnalyserStackNewOperatorEventArgs(info));
        }

        private void _RaiseNewDynamicExpressionEvent(string functionName)
        {
            _PreviousIsExpression = false;

            var eh = NewDynamicExpression;
            if (eh != null)
                eh(this, new CharacterAnalyserStackNewDynamicExpressionEventArgs(functionName));
        }

        #endregion

    }
}
