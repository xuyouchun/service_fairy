using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common.Contracts
{
    /// <summary>
    /// 对象的序列化与反序列化策略
    /// </summary>
    public interface IObjectSerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="outputStream">输出流</param>
        void Serialize(object obj, Stream outputStream);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="inputStream">输入流</param>
        /// <returns></returns>
        object Deserialize(Type type, Stream inputStream);
    }
}
