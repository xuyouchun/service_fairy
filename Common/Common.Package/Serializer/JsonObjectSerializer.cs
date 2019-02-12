using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using System.IO;
using Common.Utility;
using System.Diagnostics.Contracts;

namespace Common.Package.Serializer
{
    /// <summary>
    /// JSON序列化器
    /// </summary>
    class JsonObjectSerializer : IObjectSerializer
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
                JsonUtility.Serialize(outputStream, obj);
        }

        /// <summary>
        /// 逆序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public object Deserialize(Type type, Stream inputStream)
        {
            Contract.Requires(type != null && inputStream != null);
            return JsonUtility.Deserialize(type, inputStream);
        }

        public static readonly JsonObjectSerializer Instance = new JsonObjectSerializer();
    }
}
