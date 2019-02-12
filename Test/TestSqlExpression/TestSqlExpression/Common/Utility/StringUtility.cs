using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.IO;

namespace Common.Utility
{
    /// <summary>
    /// 字符串的工具类
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public unsafe static class StringUtility
    {
        /// <summary>
        /// 截取从开始到指定字符的子串
        /// </summary>
        /// <param name="s"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string TruncateTo(this string s, char c)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            int index = s.IndexOf(c);
            if (index < 0)
                return s;

            return s.Substring(0, index);
        }

        /// <summary>
        /// 截取指定字符之后的字符串
        /// </summary>
        /// <param name="s"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string TruncateFrom(this string s, char c)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            int index = s.IndexOf(c);
            if (index < 0)
                return s;

            return s.Substring(index + 1);
        }

        /// <summary>
        /// 截取指定字符之后的字符串，该字符是从后面开始寻找
        /// </summary>
        /// <param name="s"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string TruncateFromLast(this string s, char c)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            int index = s.LastIndexOf(c);
            if (index < 0)
                return s;

            return s.Substring(index + 1);
        }

        /// <summary>
        /// 如果长度过长，则截取到指定的长度
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <param name="postfix"></param>
        /// <returns></returns>
        public static string TruncateLength(this string s, int length, string postfix = "...")
        {
            postfix = postfix ?? "...";

            if (string.IsNullOrEmpty(s) || (length - postfix.Length) < 0)
                return string.Empty;

            if (s.Length <= length - postfix.Length)
                return s;

            return s.Substring(length - postfix.Length) + postfix;
        }

        /// <summary>
        /// 去除字符串的后缀
        /// </summary>
        /// <param name="s"></param>
        /// <param name="postfixes"></param>
        /// <returns></returns>
        public static string TrimEnd(this string s, params string[] postfixes)
        {
            return TrimEnd(s, postfixes, StringComparison.CurrentCulture);
        }

        /// <summary>
        /// 去除字符串的后缀
        /// </summary>
        /// <param name="s"></param>
        /// <param name="postfixes"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static string TrimEnd(this string s, string[] postfixes, StringComparison comparison)
        {
            Contract.Requires(s != null && postfixes != null);

            bool found = true;

            while (found)
            {
                found = false;
                foreach (string postfix in postfixes)
                {
                    if (s.EndsWith(postfix, comparison))
                    {
                        s = s.Substring(0, s.Length - postfix.Length);
                        found = true;
                    }
                }
            }

            return s;
        }

        /// <summary>
        /// 去除字符串的前缀
        /// </summary>
        /// <param name="s"></param>
        /// <param name="prefixes"></param>
        /// <returns></returns>
        public static string TrimStart(this string s, params string[] prefixes)
        {
            return TrimStart(s, prefixes, StringComparison.CurrentCulture);
        }


        /// <summary>
        /// 去除字符串的前缀
        /// </summary>
        /// <param name="s"></param>
        /// <param name="prefixes"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static string TrimStart(this string s, string[] prefixes, StringComparison comparison)
        {
            Contract.Requires(s != null && prefixes != null);

            bool found = true;

            while (found)
            {
                found = false;
                foreach (string prefix in prefixes)
                {
                    if (s.StartsWith(prefix, comparison))
                    {
                        s = s.Substring(0, s.Length - prefix.Length);
                        found = true;
                    }
                }
            }

            return s;
        }

        /// <summary>
        /// 将Byte数组读取为字符串，自动辨别其编码
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string ReadBuffer(byte[] buffer, int start, int count)
        {
            Contract.Requires(buffer != null && start >= 0 && count >= 0);

            using (StreamReader sr = new StreamReader(new MemoryStream(buffer, start, count)))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// 将字符串转换为字节流，并在开头附加其编码标识
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] ToBytes(string s, Encoding encoding)
        {
            using (MemoryStream mStream = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(mStream, encoding))
            {
                sw.Write(s);
                sw.Flush();
                return mStream.ToArray();
            }
        }

        /// <summary>
        /// 将Byte数组读取为字符串，自动辨别其编码
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string ReadBuffer(byte[] buffer)
        {
            Contract.Requires(buffer != null);

            return ReadBuffer(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 判断字符串是否以指定的字符开始
        /// </summary>
        /// <param name="s"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool StartsWith(this string s, char c)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            return s[0] == c;
        }

        /// <summary>
        /// 判断字符串是否以指定的字符结尾
        /// </summary>
        /// <param name="s"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool EndsWith(this string s, char c)
        {
            if (string.IsNullOrEmpty(s))
                return false;

            return s[s.Length - 1] == c;
        }

        /// <summary>
        /// 转换为字符串，忽略空对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nullObjStr">当为空对象时返回的字符串</param>
        /// <returns></returns>
        public static string ToStringIgnoreNull(this object obj, string nullObjStr = "")
        {
            if (obj == null)
                return nullObjStr;

            return obj.ToString();
        }

        public static string Join(string separator, bool ignoreNull, params string[] values)
        {
            StringBuilder sb = new StringBuilder();
            for (int k = 0; k < values.Length; k++)
            {
                string item = values[k];
                if (item != null)
                {
                    if (k > 0)
                        sb.Append(separator);

                    sb.Append(item);
                }
            }

            return sb.ToString();
        }

        public static string Join(string seprator, string str1, string str2, bool ignoreNull = false)
        {
            if (ignoreNull && (str1 == null || str2 == null))
                return str1 ?? str2;

            return str1 + seprator + str2;
        }

        /// <summary>
        /// 将集合用分隔符连接起来
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string JoinBy<T>(this IEnumerable<T> collection, string separator, bool ignoreNull = false)
        {
            if (collection == null)
                return "";

            StringBuilder sb = new StringBuilder();
            int index = 0;

            foreach (T item in collection)
            {
                if (ignoreNull && item == null)
                    continue;

                if (index++ > 0)
                    sb.Append(separator);

                sb.Append(item.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取第一个非空白字符
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static char GetFirstNotWhiteSpaceChar(string s)
        {
            Contract.Requires(s != null);

            for (int k = 0; k < s.Length; k++)
            {
                char c = s[k];
                if (!char.IsWhiteSpace(c))
                    return c;
            }

            return default(char);
        }

        /// <summary>
        /// 获取第一个非空字符串
        /// </summary>
        /// <param name="ss"></param>
        /// <returns></returns>
        public static string GetFirstNotNullOrWhiteSpaceString(params string[] ss)
        {
            for (int k = 0; k < ss.Length; k++)
            {
                string s = ss[k];
                if (!string.IsNullOrWhiteSpace(s))
                    return s;
            }

            return null;
        }

        /// <summary>
        /// 获取第一个非空字符串
        /// </summary>
        /// <param name="ss"></param>
        /// <returns></returns>
        public static string GetFirstNotNullOrEmptyString(params string[] ss)
        {
            for (int k = 0; k < ss.Length; k++)
            {
                string s = ss[k];
                if (!string.IsNullOrEmpty(s))
                    return s;
            }

            return null;
        }

        /// <summary>
        /// 读取CSV文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string[][] ReadCsvFile(string file, Encoding encoding)
        {
            Contract.Requires(file != null);

            return ReadCsv(File.ReadAllText(file, encoding));
        }

        /// <summary>
        /// 读取CSV文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string[][] ReadCsvFile(string file)
        {
            Contract.Requires(file != null);

            return ReadCsv(File.ReadAllText(file));
        }

        /// <summary>
        /// 分析CSV文本
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string[][] ReadCsv(string text)
        {
            Contract.Requires(text != null);

            fixed (char* fixedPtr = text)
            {
                return _ReadCsvLines(fixedPtr);
            }
        }

        private static string[][] _ReadCsvLines(char* p)
        {
            List<string[]> list = new List<string[]>();
            List<string> strs = new List<string>();

            if (*p == '\0')
                return new string[0][];

            while (true)
            {
                char c = *p;
                if (c == '\r' || c == '\n' || c == '\0')
                {
                    list.Add(strs.ToArray());
                    strs.Clear();
                    if (c == '\0')
                        break;

                    _SkipLineChar(ref p);
                }
                else if (char.IsWhiteSpace(c))
                {
                    p++;
                }
                else if (c == '"')
                {
                    char* p0 = ++p;
                    JumpToNextChar(ref p, '"', '\r', '\n');
                    if (*p != '"')
                        throw new FormatException(string.Format("第{0}行上缺少引号", list.Count + 1));

                    strs.Add(new string(p0, 0, (int)(p - p0)));
                    _JumpCsvNextCommaAfterQuot(ref p, list.Count + 1);
                }
                else
                {
                    char* p0 = p;
                    JumpToNextChar(ref p, ',', '\r', '\n');
                    strs.Add(new string(p0, 0, (int)(p - p0)).Trim());
                    p++;
                }
            }

            return list.ToArray();
        }

        private static void _SkipLineChar(ref char* p, int count = 1)
        {
            char c;
            while ((c = *p++) != '\0')
            {
                if (c == '\n')
                {
                    if (--count <= 0)
                        return;
                }
            }
        }

        private static void _JumpCsvNextCommaAfterQuot(ref char* p, int lineNumber)
        {
            char c;
            while ((c = *++p) != '\0')
            {
                if (c == ',')
                {
                    p++;
                    break;
                }

                if (c == '\r' || c == '\n')
                {
                    break;
                }

                if (!char.IsWhiteSpace(c))
                    throw new FormatException(string.Format("第{0}行上缺少逗号", lineNumber));
            }
        }

        /// <summary>
        /// 跳过空白字符
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool SkipWhitieSpace(ref char *p)
        {
            char c;
            while ((c = *p) != '\0')
            {
                if (!char.IsWhiteSpace(c))
                    return true;

                p++;
            }

            return false;
        }

        /// <summary>
        /// 跳过指定的字符
        /// </summary>
        /// <param name="p"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool SkipChar(ref char* p, char ch)
        {
            char c;
            while ((c = *p) != '\0')
            {
                if (c != ch)
                    return true;

                p++;
            }

            return false;
        }

        /// <summary>
        /// 跳过指定的字符
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ch1"></param>
        /// <param name="ch2"></param>
        /// <returns></returns>
        public static bool SkipChar(ref char* p, char ch1, char ch2)
        {
            char c;
            while ((c = *p) != '\0')
            {
                if (c != ch1 && c != ch2)
                    return true;

                p++;
            }

            return false;
        }

        /// <summary>
        /// 跳过指定的字符
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ch1"></param>
        /// <param name="ch2"></param>
        /// <param name="ch3"></param>
        /// <returns></returns>
        public static bool SkipChar(ref char* p, char ch1, char ch2, char ch3)
        {
            char c;
            while ((c = *p) != '\0')
            {
                if (c != ch1 && c != ch2 && c != ch3)
                    return true;

                p++;
            }

            return false;
        }

        /// <summary>
        /// 跳过指定的字符
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ch1"></param>
        /// <param name="ch2"></param>
        /// <param name="ch3"></param>
        /// <param name="ch4"></param>
        /// <returns></returns>
        public static bool SkipChar(ref char* p, char ch1, char ch2, char ch3, char ch4)
        {
            char c;
            while ((c = *p) != '\0')
            {
                if (c != ch1 && c != ch2 && c != ch3 && c != ch4)
                    return true;

                p++;
            }

            return false;
        }

        /// <summary>
        /// 跳转到下一个指定的字符
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool JumpToNextChar(ref char* p, char ch)
        {
            char c;
            while ((c = *p) != '\0')
            {
                if (c == ch)
                    return true;

                p++;
            }

            return false;
        }

        /// <summary>
        /// 跳转到下一个指定的字符
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ch1"></param>
        /// <param name="ch2"></param>
        /// <returns></returns>
        public static bool JumpToNextChar(ref char* p, char ch1, char ch2)
        {
            char c;
            while ((c = *p) != '\0')
            {
                if (c == ch1 || c == ch2)
                    return true;

                p++;
            }

            return false;
        }

        /// <summary>
        /// 跳转到下一个指定的字符
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ch1"></param>
        /// <param name="ch2"></param>
        /// <param name="ch3"></param>
        /// <returns></returns>
        public static bool JumpToNextChar(ref char* p, char ch1, char ch2, char ch3)
        {
            char c;
            while ((c = *p) != '\0')
            {
                if (c == ch1 || c == ch2 || c == ch3)
                    return true;

                p++;
            }

            return false;
        }

        /// <summary>
        /// 跳转到下一个指定的字符
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ch1"></param>
        /// <param name="ch2"></param>
        /// <param name="ch3"></param>
        /// <param name="ch4"></param>
        /// <returns></returns>
        public static bool JumpToNextChar(ref char* p, char ch1, char ch2, char ch3, char ch4)
        {
            char c;
            while ((c = *p) != '\0')
            {
                if (c == ch1 || c == ch2 || c == ch3 || c == ch4)
                    return true;

                p++;
            }

            return false;
        }

        /// <summary>
        /// 跳转到下一个指定的字符
        /// </summary>
        /// <param name="p"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static bool JumpToNextChar(ref char* p, params char[] chars)
        {
            char c;
            fixed (char* pCharStart = chars)
            {
                char* pCharEnd = pCharStart + chars.Length;
                while ((c = *p) != '\0')
                {
                    for (char* pChar = pCharStart; pChar < pCharEnd; pChar++)
                    {
                        if (*pChar == c)
                            return true;
                    }

                    p++;
                }
            }

            return false;
        }

        /// <summary>
        /// 将字符串的第一个字母变为大写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UpperFirstChar(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            if (!char.IsLetter(s[0]))
                return s;

            return char.ToUpper(s[0]) + s.Substring(1);
        }

        /// <summary>
        /// 将字符串的第一个字母变为小写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string LowerFirstChar(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            if (!char.IsLetter(s[0]))
                return s;

            return char.ToLower(s[0]) + s.Substring(1);
        }

        /// <summary>
        /// 在字符串右侧添加指定个数的句点
        /// </summary>
        /// <param name="s"></param>
        /// <param name="count"></param>
        /// <param name="whiteSpaces"></param>
        /// <returns></returns>
        public static string PadDots(this string s, int count, int whiteSpaces = 1)
        {
            if (count <= 0)
                return s ?? string.Empty;

            if (whiteSpaces > 0)
                s += new string(' ', whiteSpaces);

            return s + new string('.', count);
        }

        /// <summary>
        /// 对两个字符串进行比较，返回能够匹配的最大长度
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static int Compare(char* p1, char* p2, bool ignoreCase = false)
        {
            char* p0 = p1;
            char c1, c2;

            if (!ignoreCase)
            {
                while ((c1 = *p1) != '\0' && (c2 = *p2) != '\0')
                {
                    if (c1 == c2)
                    {
                        p1++;
                        p2++;
                        continue;
                    }

                    break;
                }
            }
            else
            {
                while ((c1 = *p1) != '\0' && (c2 = *p2) != '\0')
                {
                    if (c1 == c2 || char.ToLower(c1) == char.ToLower(c2))
                    {
                        p1++;
                        p2++;
                        continue;
                    }

                    break;
                }
            }

            return (int)(p1 - p0);
        }

        /// <summary>
        /// 将字节流大小转换为字符串的表示形式
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string GetSizeString(long size)
        {
            if (size < 0)
                return "";

            long b = size % 1024;
            size >>= 10;

            if (size == 0)
                return b.ToString();

            long k = size % 1024;
            size >>= 10;

            if (size == 0)
                return k.ToString() + "." + (b * 100 / 1024) + "K";

            long m = size % 1024;
            size >>= 10;

            if (size == 0)
                return m.ToString() + "." + (k * 100 / 1024) + "M";

            long g = size % 1024;
            size >>= 10;

            if (size == 0)
                return g.ToString() + "." + (m * 100 / 1024) + "G";
            
            return size.ToString() + "." + (g * 100 / 1024) + "T";
        }
    }
}
