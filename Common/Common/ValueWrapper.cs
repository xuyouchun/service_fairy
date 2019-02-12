using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 数值包装器，用于一些不允许传递ref或out参数的场合，例如yield return函数中
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValueWrapper<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ValueWrapper()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        public ValueWrapper(T value)
        {
            Value = value;
        }

        /// <summary>
        /// 值
        /// </summary>
        public T Value;
    }
}
