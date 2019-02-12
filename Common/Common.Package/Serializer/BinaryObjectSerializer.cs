using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Linq.Expressions;
using Common.Contracts;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics.Contracts;
using Common.Utility;

namespace Common.Package.Serializer
{
    /// <summary>
    /// 对象序列化/反序列化器
    /// </summary>
    class BinaryObjectSerializer : IObjectSerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="outputStream"></param>
        public void Serialize(object obj, Stream outputStream)
        {
            Contract.Requires(outputStream != null);
            if (obj == null)
                return;

            StreamUtility.Serialize(outputStream, obj);
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

            return StreamUtility.Deserialize(inputStream);
        }

        /// <summary>
        /// 单例
        /// </summary>
        public static readonly BinaryObjectSerializer Instance = new BinaryObjectSerializer();
    }
}
