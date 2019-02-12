using System;
using System.Collections.Generic;
using System.Text;
using Common.Script.VbScript.Values;

namespace Common.Script.VbScript
{
    /// <summary>
    /// 值
    /// </summary>
    public class Value
    {
        #region Constructors ...

        public Value(object obj)
        {
            InnerValue = obj;
        }

        public Value(byte value)
        {
            InnerValue = value;
        }

        public Value(short value)
        {
            InnerValue = value;
        }

        public Value(int value)
        {
            InnerValue = value;
        }

        public Value(long value)
        {
            InnerValue = value;
        }

        public Value(float value)
        {
            InnerValue = value;
        }

        public Value(double value)
        {
            InnerValue = value;
        }

        public Value(string value)
        {
            InnerValue = value;
        }

        public Value(decimal value)
        {
            InnerValue = value;
        }

        public Value(bool value)
        {
            InnerValue = value;
        }

        public Value(DateTime dateTime)
        {
            InnerValue = dateTime;
        }

        #endregion

        /// <summary>
        /// 内部值
        /// </summary>
        public object InnerValue { get; protected set; }

        /// <summary>
        /// 获取指定类型的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal T GetValue<T>()
        {
            if (InnerValue == null)
                return default(T);

            if (typeof(T) == InnerValue.GetType())
                return (T)InnerValue;

            if (typeof(T) == typeof(string))
                return (T)(object)ToString();

            try
            {
                return (T)Convert.ChangeType(InnerValue, typeof(T));
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 获取值的类型
        /// </summary>
        /// <returns></returns>
        public Type GetValueType()
        {
            if (InnerValue == null)
                return typeof(object);

            return InnerValue.GetType();
        }

        public bool IsEmpty()
        {
            return InnerValue == null;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            if (InnerValue == null)
                return base.GetHashCode();

            return InnerValue.GetHashCode();
        }

        #region Operators ...

        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static Value operator >(Value value1, Value value2)
        {
            return CalculateManager.GetCaculate(value1, value2).GreaterThan(value1, value2);
        }

        /// <summary>
        /// 小于
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static Value operator <(Value value1, Value value2)
        {
            return CalculateManager.GetCaculate(value1, value2).LessThan(value1, value2);
        }

        /// <summary>
        /// 等于
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static Value operator ==(Value value1, Value value2)
        {
            if ((object)value1 == null || (object)value2 == null)
                return (object)value1 == null && (object)value2 == null;

            if (object.ReferenceEquals(value1, value2))
                return true;

            if (value1.IsEmpty() || value2.IsEmpty())
                return value1.IsEmpty() && value2.IsEmpty();

            return CalculateManager.GetCaculate(value1, value2).Equality(value1, value2);
        }

        /// <summary>
        /// 不等于
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static Value operator !=(Value value1, Value value2)
        {
            if ((object)value1 == null || (object)value2 == null)
                return (object)value1 != null || (object)value2 != null;

            if (object.ReferenceEquals(value1, value2))
                return false;

            return CalculateManager.GetCaculate(value1, value2).InEnqulity(value1, value2);
        }

        /// <summary>
        /// 大于等于
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static Value operator >=(Value value1, Value value2)
        {
            return CalculateManager.GetCaculate(value1, value2).GreaterOrEqual(value1, value2);
        }

        /// <summary>
        /// 小于等于
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static Value operator <=(Value value1, Value value2)
        {
            return CalculateManager.GetCaculate(value1, value2).LessOrEqual(value1, value2);
        }

        /// <summary>
        /// 与
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static Value operator &(Value value1, Value value2)
        {
            return CalculateManager.GetCaculate(value1, value2).And(value1, value2);
        }

        /// <summary>
        /// 或
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static Value operator |(Value value1, Value value2)
        {
            return CalculateManager.GetCaculate(value1, value2).Or(value1, value2);
        }

        /// <summary>
        /// 非
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Value operator !(Value value)
        {
            return CalculateManager.GetCaculate(value).Not(value);
        }

        /// <summary>
        /// 取负
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Value operator -(Value value)
        {
            return CalculateManager.GetCaculate(value).Minus(value);
        }

        /// <summary>
        /// 加
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static Value operator +(Value value1, Value value2)
        {
            return CalculateManager.GetCaculate(value1, value2).Addition(value1, value2);
        }

        /// <summary>
        /// 减
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static Value operator -(Value value1, Value value2)
        {
            return CalculateManager.GetCaculate(value1, value2).Subtraction(value1, value2);
        }

        /// <summary>
        /// 乘
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static Value operator *(Value value1, Value value2)
        {
            return CalculateManager.GetCaculate(value1, value2).Multiplication(value1, value2);
        }

        /// <summary>
        /// 除
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static Value operator /(Value value1, Value value2)
        {
            return CalculateManager.GetCaculate(value1, value2).Division(value1, value2);
        }

        /// <summary>
        /// 取余
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static Value operator %(Value value1, Value value2)
        {
            return CalculateManager.GetCaculate(value1, value2).Modulus(value1, value2);
        }

        /// <summary>
        /// 异或
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static Value operator ^(Value value1, Value value2)
        {
            return CalculateManager.GetCaculate(value1, value2).Xor(value1, value2);
        }

        /// <summary>
        /// 求幂
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static Value Pow(Value value1, Value value2)
        {
            return CalculateManager.GetCaculate(value1, value2).Pow(value1, value2);
        }

        public static bool operator ==(Value value, object obj)
        {
            return object.ReferenceEquals(value, obj);
        }

        public static bool operator !=(Value value, object obj)
        {
            return !object.ReferenceEquals(value, obj);
        }

        #endregion

        #region 隐式转换 ...

        public static implicit operator string(Value value)
        {
            return value.GetValue<string>();
        }

        public static implicit operator byte(Value value)
        {
            return value.GetValue<byte>();
        }

        public static implicit operator short(Value value)
        {
            return value.GetValue<short>();
        }

        public static implicit operator int(Value value)
        {
            return value.GetValue<int>();
        }

        public static implicit operator long(Value value)
        {
            return value.GetValue<long>();
        }

        public static implicit operator float(Value value)
        {
            return value.GetValue<float>();
        }

        public static implicit operator double(Value value)
        {
            return value.GetValue<double>();
        }

        public static implicit operator decimal(Value value)
        {
            return value.GetValue<decimal>();
        }

        public static implicit operator bool(Value value)
        {
            return value.GetValue<bool>();
        }

        public static implicit operator DateTime(Value value)
        {
            return value.GetValue<DateTime>();
        }

        public static bool operator true(Value value)
        {
            return (bool)value;
        }

        public static bool operator false(Value value)
        {
            return !(bool)value;
        }

        public static implicit operator Value(string value)
        {
            return new Value(value);
        }

        public static implicit operator Value(byte value)
        {
            return new Value(value);
        }

        public static implicit operator Value(short value)
        {
            return new Value(value);
        }

        public static implicit operator Value(int value)
        {
            return new Value(value);
        }

        public static implicit operator Value(long value)
        {
            return new Value(value);
        }

        public static implicit operator Value(float value)
        {
            return new Value(value);
        }

        public static implicit operator Value(double value)
        {
            return new Value(value);
        }

        public static implicit operator Value(decimal value)
        {
            return new Value(value);
        }

        public static implicit operator Value(bool value)
        {
            return new Value(value);
        }

        public static implicit operator Value(DateTime value)
        {
            return new Value(value);
        }

        #endregion

        /// <summary>
        /// 获取类型的字符串表示形式
        /// </summary>
        /// <returns></returns>
        internal string GetValueTypeString()
        {
            return GetValueType().Name;
        }

        public override string ToString()
        {
            if (InnerValue == null)
                return string.Empty;

            return InnerValue.ToString();
        }

        public static readonly Value Void = new Value(null);

        /// <summary>
        /// 是否为整型
        /// </summary>
        /// <returns></returns>
        internal bool IsInteger()
        {
            Type type = GetValueType();
            TypeCode code = Type.GetTypeCode(type);

            return code <= TypeCode.UInt64 && code >= TypeCode.Byte;
        }

        /// <summary>
        /// 是否为数字类型
        /// </summary>
        /// <returns></returns>
        internal bool IsNumber()
        {
            Type type = GetValueType();
            TypeCode code = Type.GetTypeCode(type);

            return code <= TypeCode.Decimal && code >= TypeCode.Byte;
        }

        /// <summary>
        /// 是否为浮点型
        /// </summary>
        /// <returns></returns>
        internal bool IsFloat()
        {
            Type type = GetValueType();
            TypeCode code = Type.GetTypeCode(type);

            return code <= TypeCode.Decimal && code >= TypeCode.Single;
        }
    }
}
