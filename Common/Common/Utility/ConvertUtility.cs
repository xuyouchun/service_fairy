using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Common.Utility
{
    /// <summary>
    /// 数据转换工具类
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public static class ConvertUtility
    {
        /// <summary>
        /// 转换为Int16
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt16(this string value, short defaultValue = 0)
        {
            short result;
            if (short.TryParse(value, out result))
                return result;

            return defaultValue;
        }

        /// <summary>
        /// 转换为Int32
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt32(this string value, int defaultValue = 0)
        {
            int result;
            if (int.TryParse(value, out result))
                return result;

            return defaultValue;
        }

        /// <summary>
        /// 转换为Int64
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ToInt64(this string value, long defaultValue = 0)
        {
            long result;
            if (long.TryParse(value, out result))
                return result;

            return defaultValue;
        }

        /// <summary>
        /// 转换为System.Signle
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float ToSingle(this string value, float defaultValue = 0.0f)
        {
            float result;
            if (float.TryParse(value, out result))
                return result;

            return defaultValue;
        }

        /// <summary>
        /// 转换为System.Double
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(this string value, double defaultValue = 0.0)
        {
            double result;
            if (double.TryParse(value, out result))
                return result;

            return defaultValue;
        }

        /// <summary>
        /// 转化为指定的类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static object ToType(this object value, Type type, object defaultValue)
        {
            object result;
            if (TryConvert(value, type, out result))
                return result ?? defaultValue;

            return defaultValue;
        }

        /// <summary>
        /// 转化为指定的类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToTypeWithError(this object value, Type type)
        {
            object result;
            if (TryConvert(value, type, out result))
                return result;

            throw new FormatException(string.Format("无法将“{0}”转化为指定的类型“{1}”",
                value.ToStringIgnoreNull(), type.ToStringIgnoreNull()));
        }

        /// <summary>
        /// 转化为指定的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToTypeWithError<T>(this object value)
        {
            return (T)ToTypeWithError(value, typeof(T));
        }

        /// <summary>
        /// 尝试转换为指定的格式
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryConvert(object value, Type type, out object result)
        {
            Contract.Requires(type != null);

            if (value == null)
            {
                result = null;
                return !type.IsValueType;
            }

            if (value.GetType() == type)
            {
                result = value;
                return true;
            }

            try
            {
                if (type.BaseType == typeof(System.Enum))
                {
                    result = Enum.Parse(type, value.ToString(), true);
                    return true;
                }

                if (type == typeof(Guid))
                {
                    Guid guid;
                    if (Guid.TryParse(value.ToString(), out guid))
                    {
                        result = guid;
                        return true;
                    }
                }

                if (!(value is IConvertible))
                {
                    goto _error;
                }

                result = Convert.ChangeType(value, type);
                return true;
            }
            catch
            {
                goto _error;
            }

        _error:
            result = null;
            return false;
        }

        /// <summary>
        /// 转化为指定的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T ToType<T>(this object value, T defaultValue = default(T))
        {
            return (T)ToType(value, typeof(T), defaultValue);
        }
    }
}
