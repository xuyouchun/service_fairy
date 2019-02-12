using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Common.Contracts;

namespace Common.Communication.Wcf.Encoders
{
    static class EncoderFactory
    {
        /// <summary>
        /// 根据字节流决定编码格式
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static IWcfMessageEncoder CreateEncoder(byte[] buffer, int offset, int count)
        {
            Contract.Requires(buffer != null);

            for (int k = offset, end = count + offset; k < end; k++)
            {
                char c = (char)buffer[k];
                if (char.IsWhiteSpace(c))
                    continue;

                switch (c)
                {
                    case '{':
                        return new JsonWcfMessageEncoder();

                    case '<':
                        return new XmlWcfMessageEncoder();

                    case (char)1:
                        return new BinaryWcfMessageEncoder();
                }

                break;
            }

            throw new NotSupportedException("不支持的编码格式");
        }

        /// <summary>
        /// 创建指定类型的编码器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public static IWcfMessageEncoder CreateEncoder(DataFormat type, bool throwError = true)
        {
            switch (type.Revise())
            {
                case DataFormat.Binary:
                    return new BinaryWcfMessageEncoder();

                case DataFormat.Json:
                    return new JsonWcfMessageEncoder();

                case DataFormat.Xml:
                    return new XmlWcfMessageEncoder();

                case DataFormat.Unknown:
                    if (throwError)
                        throw new InvalidOperationException("未指定编码格式");

                    break;

                default:
                    if (throwError)
                        throw new NotSupportedException("不支持的编码方式");
                    break;
            }

            return null;
        }
    }
}
