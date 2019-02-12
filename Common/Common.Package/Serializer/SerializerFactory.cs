using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;

namespace Common.Package.Serializer
{
    /// <summary>
    /// 序列化器的创建器
    /// </summary>
    public static class SerializerFactory
    {
        /// <summary>
        /// 创建序列化器
        /// </summary>
        /// <param name="dataFormat"></param>
        /// <returns></returns>
        public static IObjectSerializer CreateSerializer(DataFormat dataFormat)
        {
            switch (dataFormat.Revise())
            {
                case DataFormat.Json:
                    return JsonObjectSerializer.Instance;

                case DataFormat.Xml:
                    return XmlObjectSerializer.Instance;

                case DataFormat.Binary:
                    return BinaryObjectSerializer.Instance;
            }

            throw new NotSupportedException("不支持序列化方法:" + dataFormat);
        }
    }
}
