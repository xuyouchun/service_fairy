using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using System.IO;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace Common.Package.Serializer
{
    /// <summary>
    /// XML对象序列化器
    /// </summary>
    class XmlObjectSerializer : IObjectSerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="outputStream"></param>
        public void Serialize(object obj, Stream outputStream)
        {
            Contract.Requires(outputStream != null);

            if (obj != null)
                XmlUtility.Serialize(obj, outputStream);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public object Deserialize(Type type, Stream inputStream)
        {
            Contract.Requires(inputStream != null);

            return XmlUtility.Deserialize(type, inputStream);
        }

        public static readonly XmlObjectSerializer Instance = new XmlObjectSerializer();
    }
}
