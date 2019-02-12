using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;

namespace Common.Communication.Wcf.Encoders
{
    unsafe static class JsonWcfMessageEncoderHelper
    {
        /// <summary>
        /// 寻找指定名称的结点的位置
        /// </summary>
        /// <param name="p"></param>
        /// <param name="count"></param>
        /// <param name="name"></param>
        /// <param name="pNameStart"></param>
        /// <param name="pNameEnd"></param>
        /// <param name="pBodyStart"></param>
        /// <param name="pBodyEnd"></param>
        /// <returns></returns>
        public static bool SearchNode(ref byte* p, int count, string name, out byte *pNameStart, out byte *pNameEnd, out byte* pBodyStart, out byte* pBodyEnd)
        {
            byte* p2 = p + count;
            while (SearchNextNode(ref p, (int)(p2 - p), out pNameStart, out pNameEnd, out pBodyStart, out pBodyEnd))
            {
                string name0 = BufferUtility.GetString(pNameStart, (int)(pNameEnd - pNameStart), Encoding.UTF8);
                if (name == name0.Trim('\'', '"'))
                    return true;

                if (!BufferUtility.SkipWhiteSpace(ref p, (int)(p2 - p)) && *p != (byte)',')
                    return false;

                p++;
            }

            return false;
        }

        /// <summary>
        /// 寻找下一个节点的位置
        /// </summary>
        /// <param name="p"></param>
        /// <param name="count"></param>
        /// <param name="pNameStart"></param>
        /// <param name="pNameEnd"></param>
        /// <param name="pBodyStart"></param>
        /// <param name="pBodyEnd"></param>
        /// <returns></returns>
        public static bool SearchNextNode(ref byte* p, int count, out byte *pNameStart, out byte *pNameEnd, out byte* pBodyStart, out byte* pBodyEnd)
        {
            byte* p2 = p + count;
            if (!BufferUtility.SkipWhiteSpace(ref p, (int)(p2 - p)))
                goto _end;

            pNameStart = p;
            int k = BufferUtility.FindIndex(p, (int)(p2 - p), (byte)':');
            if (k < 0)
                goto _end;

            p += k;
            pNameEnd = p;
            p++;
            if (!BufferUtility.SkipWhiteSpace(ref p, (int)(p2 - p)))
                goto _end;

            pBodyStart = p;
            bool r = _Jump(ref p, (int)(p2 - p));
            pBodyEnd = p;

            return r;

        _end:
            pNameStart = pNameEnd = pBodyStart = pBodyEnd = null;
            return false;
        }

        private static bool _Jump(ref byte* p, int count)
        {
            byte b = *p++;
            count--;

            switch (b)
            {
                case (byte)'{':
                    return _JumpBracket(ref p, count);

                case (byte)'"':
                    return _JumpDoubleQuote(ref p, count);

                case (byte)'\'':
                    return _JumpQuote(ref p, count);

                default:
                    if (BufferUtility.JumpTo(ref p, count, (byte)',', (byte)'}'))
                    {
                        //p++;
                        return true;
                    }

                    return false;
            }
        }

        private static bool _JumpDoubleQuote(ref byte* p, int count)
        {
            byte* pEnd = p + count;
            while (p < pEnd)
            {
                byte b = *p;
                if (b == (byte)'"')
                {
                    p++;
                    return true;
                }
                else if (b == (byte)'\\')
                {
                    p += 2;
                    continue;
                }
                else
                {
                    p++;
                }
            }

            return false;
        }

        private static bool _JumpQuote(ref byte* p, int count)
        {
            byte* pEnd = p + count;
            while (p < pEnd)
            {
                byte b = *p;
                if (b == '\'')
                {
                    p++;
                    return true;
                }
                else if (b == (byte)'\\')
                {
                    p += 2;
                    continue;
                }
                else
                {
                    p++;
                }
            }

            return false;
        }

        private static bool _JumpBracket(ref byte* p, int count)
        {
            byte* p2 = p + count;
            while (p < p2)
            {
                byte b = *p;
                if (b >= 128)
                {
                    p++;
                    continue;
                }

                switch (b)
                {
                    case (byte)'}':
                        p++;
                        return true;

                    case (byte)'{':
                        p++;
                        if (!_JumpBracket(ref p, (int)(p2 - p)))
                            return false;
                        continue;

                    case (byte)'"':
                        p++;
                        if (!_JumpDoubleQuote(ref p, (int)(p2 - p)))
                            return false;
                        continue;

                    case (byte)'\'':
                        p++;
                        if (!_JumpQuote(ref p, (int)(p2 - p)))
                            return false;
                        continue;

                    default:
                        p++;
                        continue;
                }
            }

            return false;
        }
    }
}
