using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Expressions.Functions
{
    /// <summary>
    /// 数字类型
    /// </summary>
    static class ValueTypes
    {
        public class NumberType { }
        public class IntegerType { }

        /// <summary>
        /// 数字类型
        /// </summary>
        public static readonly Type Number = typeof(NumberType);

        public static readonly Type Integer = typeof(IntegerType);

        /// <summary>
        /// 是否为正确的类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsCorrectType(Type type, Value value)
        {
            if (type == typeof(object))
                return true;

            if (value == null || value.InnerValue == null)
                return false;

            Type valueType = value.GetType(), innerValueType = value.InnerValue.GetType();
            if (type == valueType || type == innerValueType)
                return true;

            if (type == Number)
            {
                TypeCode tcode = Type.GetTypeCode(innerValueType), tcode2 = Type.GetTypeCode(type);
                return tcode <= TypeCode.Decimal && tcode >= TypeCode.Byte || tcode2 <= TypeCode.Decimal && tcode2 >= TypeCode.Byte;
            }

            if (type == Integer)
            {
                TypeCode tcode = Type.GetTypeCode(innerValueType), tcode2 = Type.GetTypeCode(type);
                if (tcode <= TypeCode.UInt64 && tcode >= TypeCode.Byte || tcode2 <= TypeCode.UInt64 && tcode2 >= TypeCode.Byte)
                    return true;

                if (tcode <= TypeCode.Decimal && tcode >= TypeCode.Single)
                {
                    return (decimal)value == (long)value;
                }

                if (tcode2 <= TypeCode.Decimal && tcode2 >= TypeCode.Single)
                {
                    return (decimal)value == (long)value;
                }
            }

            return false;
        }
    }
}
