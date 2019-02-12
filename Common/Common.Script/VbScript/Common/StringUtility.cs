using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript
{
    unsafe static class StringUtility
    {
        /// <summary>
        /// 跳过空字符
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public static bool SkipWhiteSpace(ref char* ptr)
        {
            char ch;
            while ((ch = *ptr) != '\0')
            {
                if (!char.IsWhiteSpace(ch))
                    return true;

                ptr++;
            }

            return false;
        }

        /// <summary>
        /// 跳过回车换行键
        /// </summary>
        /// <param name="ptr"></param>
        public static void SkipLineChar(ref char* ptr)
        {
            char ch;

            while ((ch = *ptr) != '\0')
            {
                if (ch == '\r' || ch == '\n')
                {
                    ptr++;
                    continue;
                }

                return;
            }
        }

        /// <summary>
        /// 跳至指定的字符
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool SkipTo(ref char* ptr, char c)
        {
            char ch;
            while ((ch = *ptr) != '\0')
            {
                if (ch == c)
                    return true;

                ptr++;
            }

            return false;
        }

        /// <summary>
        /// 读取之后的一个字符，忽略空白字符
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public static char GetNextCharSkipWhiteSpace(char* ptr)
        {
            char ch;

            while ((ch = *ptr) != '\0')
            {
                if (!char.IsWhiteSpace(ch))
                    return ch;

                ptr++;
            }

            return '\0';
        }

        /// <summary>
        /// 读取第一个空白字符之前的字符串
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string GetStringBeforeWhiteSpace(char* ptr, int count)
        {
            char ch;
            char* ptr0 = ptr, pEnd = ptr + count;

            while ((ch = *ptr) != '\0')
            {
                if (char.IsWhiteSpace(ch))
                {
                    return new string(ptr, 0, (int)(ptr0 - ptr));
                }

                if (++ptr >= pEnd)
                    return null;
            }

            return null;
        }

        public static unsafe bool StartWith(char* p, char* ptrEnd, string str, bool ignoreCase)
        {
            if (ptrEnd - p < str.Length)
                return false;

            for (int k = 0; k < str.Length; k++)
            {
                if (ignoreCase)
                {
                    if (char.ToUpper(str[k]) != char.ToUpper(*p))
                        return false;
                }
                else
                {
                    if (str[k] != *p)
                        return false;
                }

                p++;
            }

            return true;
        }

        /// <summary>
        /// 跳过双引号
        /// </summary>
        /// <returns></returns>
        public static bool SkipDlbQuot(ref char* ptr, char* endPtr)
        {
            while (ptr < endPtr)
            {
                if (*ptr == '"')
                {
                    ptr++;
                    return true;
                }

                ptr++;
            }

            return false;
        }

        public static bool SkipDlbQuot(ref char* ptr)
        {
            char ch;
            while ((ch = *ptr) != '\0')
            {
                if (*ptr == '"')
                {
                    ptr++;
                    return true;
                }

                ptr++;
            }

            return false;
        }

        /// <summary>
        /// 是否合法的变量名字字符
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool IsVariableChar(char ch)
        {
            return ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z' || ch == '_' || ch > 127;
        }
    }
}
