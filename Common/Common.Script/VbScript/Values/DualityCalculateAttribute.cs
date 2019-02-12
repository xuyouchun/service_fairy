using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    sealed class DualityCalculateAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type1">第一个参数的类型</param>
        /// <param name="type2">第二个参数的类型</param>
        public DualityCalculateAttribute(Type type1, Type type2)
        {
            Type1 = type1;
            Type2 = type2;
        }

        /// <summary>
        /// 类型1
        /// </summary>
        public Type Type1 { get; private set; }

        /// <summary>
        /// 类型2
        /// </summary>
        public Type Type2 { get; private set; }
    }
}
