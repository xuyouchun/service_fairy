using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using System.Reflection;

namespace Common.Data.SqlExpressions.Analyzer
{
    /// <summary>
    /// 符号表
    /// </summary>
    unsafe static class SymbolReader
    {
        static SymbolReader()
        {
            var list = from op in SqlExpressionOperatorAttribute.GetAllOperators()
                       let attr = SqlExpressionOperatorAttribute.GetOpAttr(op)
                       group new OperatorSymbol(attr.OpText, op, attr.Weight, attr.OpCount) by attr.OpText[0];

            _symbolDict = list.ToDictionary(g => char.ToLower(g.Key), g => g.OrderByDescending(v => v.Text.Length).ToArray());
        }

        private static readonly Dictionary<char, OperatorSymbol[]> _symbolDict;

        /// <summary>
        /// 从此处读取符号
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Symbol ReadNextSymbol(ref char* p)
        {
            if (!StringUtility.SkipWhitieSpace(ref p))
                return null;

            char c = *p;

            // 判断是否为括号
            if (c == '(')
            {
                p++;
                return Symbol.LeftBricket;
            }

            if (c == ')')
            {
                p++;
                return Symbol.RightBricket;
            }

            if (c == ',')
            {
                p++;
                return Symbol.Comma;
            }

            // 尝试读取运算符
            OperatorSymbol[] symbols;
            if (_symbolDict.TryGetValue(char.ToLower(c), out symbols))
            {
                for (int k = 0; k < symbols.Length; k++)
                {
                    OperatorSymbol symbol = symbols[k];
                    fixed (char* p2 = symbol.Text)
                    {
                        int count = StringUtility.Compare(p, p2, true);
                        if (count == symbol.Text.Length && _IsOperatorBorder(p + count))
                        {
                            p += count;
                            return symbol;
                        }
                    }
                }
            }

            // 尝试读取变量名或函数名
            string valiableName = _ReadValiableName(ref p);
            if (!string.IsNullOrEmpty(valiableName))
            {
                if (string.Compare(valiableName, "null", true) == 0)
                    return new ValueSymbol("NULL", DbColumnType.DBNull);

                if (_TryReadFuncLeftBricket(ref p))
                    return new FunctionSymbol(valiableName);
                
                return new VariableSymbol(valiableName);
            }

            // 尝试读取参数名
            string parameterName = _ReadParameterName(ref p);
            if (!string.IsNullOrEmpty(parameterName))
            {
                return new ParameterSymbol(parameterName);
            }

            // 尝试读取常量
            DbColumnType dataType;
            object value = _ReadValue(ref p, out dataType);
            if (value != null)
                return new ValueSymbol(value, dataType);

            throw new FormatException("表达式格式错误");
        }

        private static bool _IsOperatorBorder(char* p)
        {
            char c0 = *(p - 1), c = *p;
            if (char.IsLetter(c0) || char.IsNumber(c0) || c0 == '_')  // 字符或下划线
            {
                return !char.IsLetter(c) && !char.IsNumber(c) && c != '_';
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 读取变量名称或函数名
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static string _ReadValiableName(ref char* p)
        {
            char c = *p;

            if (c == '[')
            {
                char* p0 = p + 1;
                while ((c = *++p) != '\0')
                {
                    if (c == ']')
                    {
                        return new string(p0, 0, (int)((p++) - p0));
                    }
                }

                throw new FormatException("表达式格式错误，变量的中括号没有闭合");
            }
            else
            {
                if (!_IsValiableFirstChar(c))
                    return null;

                char* p0 = p;
                while ((c = *++p) != '\0')
                {
                    if (!_IsValiableChar(c))
                        break;
                }

                return new string(p0, 0, (int)(p - p0));
            }
        }

        // 尝试读取左括号
        private static bool _TryReadFuncLeftBricket(ref char* p)
        {
            char* p0 = p;
            if (!StringUtility.SkipWhitieSpace(ref p))
                return false;

            if (*p == '(')
            {
                p++;
                return true;
            }

            p = p0;
            return false;
        }

        private static bool _IsValiableFirstChar(char c)
        {
            return char.IsLetter(c) || c == '_';
        }

        private static bool _IsValiableChar(char c)
        {
            return char.IsLetter(c) || char.IsNumber(c) || c == '_' || c == '.';
        }

        // 读取参数名称
        private static string _ReadParameterName(ref char* p)
        {
            if (*p != '@')
                return null;

            char c = *p;
            char* p0 = p + 1;
            while ((c = *++p) != '\0')
            {
                if (!_IsValiableChar(c))
                    break;
            }

            return new string(p0, 0, (int)(p - p0));
        }

        /// <summary>
        /// 读取值
        /// </summary>
        /// <param name="p"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        private static object _ReadValue(ref char* p, out DbColumnType dataType)
        {
            char c = *p;
            char* p0 = p;

            dataType = DbColumnType.Unknown;
            if (c == '\'')
            {
                dataType = DbColumnType.AnsiString;
                return _ReadString(ref p);
            }

            if (char.IsNumber(c) || c == '-')
            {
                return _ReadNumber(ref p, out dataType);
            }

            return null;
        }

        // 读取数值
        private static object _ReadNumber(ref char* p, out DbColumnType fieldType)
        {
            char c = *p;
            bool isNegative = false, hasMetDot = false;
            if (c == '-')
            {
                isNegative = true;
                if (!StringUtility.SkipWhitieSpace(ref p))
                    throw new FormatException("数字格式错误");

                p++;
            }

            char* p0 = p;
            while ((c = *p) != '\0')
            {
                if (c == '.')
                {
                    if (hasMetDot)
                        throw new FormatException("数字格式错误");

                    hasMetDot = true;
                }
                else if (!char.IsNumber(c))
                {
                    break;
                }

                p++;
            }

            if (p == p0)
                throw new FormatException("数字格式错误");

            string str = new string(p0, 0, (int)(p - p0));

            if (hasMetDot)
            {
                double value = double.Parse(str);
                if (value <= float.MaxValue)
                {
                    fieldType = DbColumnType.Single;
                    return isNegative ? -(float)value : (float)value;
                }
                else
                {
                    fieldType = DbColumnType.Double;
                    return isNegative ? -value : value;
                }
            }
            else
            {
                long value = long.Parse(str);
                if (value <= int.MaxValue)
                {
                    fieldType = DbColumnType.Int32;
                    return isNegative ? -(int)value : (int)value;
                }
                else
                {
                    fieldType = DbColumnType.Int64;
                    return isNegative ? -value : value;
                }
            }
        }

        // 读取字符串
        private static string _ReadString(ref char* p)
        {
            char c;
            char* p0 = p + 1;
            while ((c = *++p) != '\0')
            {
                if (c == '\'')
                {
                    if (*(p + 1) == '\'')
                    {
                        p++;
                        continue;
                    }

                    break;
                }
            }

            if (c != '\'')
                throw new FormatException("字符串格式错误");

            p++;
            return new string(p0, 0, (int)(p - p0 - 1));
        }
    }
}
