using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Script.VbScript.Values
{
    /// <summary>
    /// 计算器的管理器
    /// </summary>
    static class CalculateManager
    {
        static CalculateManager()
        {
            _LoadCalculates();
        }

        static readonly Dictionary<TypePair, DualityCalculate> _Dict1 = new Dictionary<TypePair, DualityCalculate>();
        static readonly Dictionary<Type, UnitaryCalculate> _Dict2 = new Dictionary<Type, UnitaryCalculate>();

        private static void _LoadCalculates()
        {
            foreach (Type type in typeof(CalculateManager).Assembly.GetTypes())
            {
                object[] attrs = type.GetCustomAttributes(typeof(DualityCalculateAttribute), false);
                if (attrs.Length > 0)
                {
                    DualityCalculate obj = Activator.CreateInstance(type) as DualityCalculate;
                    foreach (DualityCalculateAttribute attr in attrs)
                    {
                        _Dict1.Add(new TypePair(attr.Type1, attr.Type2), obj);
                    }
                }

                attrs = type.GetCustomAttributes(typeof(UnitaryCalculateAttribute), false);
                if (attrs.Length > 0)
                {
                    UnitaryCalculate obj = Activator.CreateInstance(type) as UnitaryCalculate;
                    foreach (UnitaryCalculateAttribute attr in attrs)
                    {
                        _Dict2.Add(attr.Type, obj);
                    }
                }
            }
        }

        /// <summary>
        /// 根据类型获取二元计算器
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public static DualityCalculate GetCaculate(Type type1, Type type2)
        {
            DualityCalculate v;
            _Dict1.TryGetValue(new TypePair(type1, type2), out v);
            return v ?? _DefaultDualityCalculate;
        }

        private static readonly DefaultDualityCalculate _DefaultDualityCalculate = new DefaultDualityCalculate();
        private static readonly DefaultUnitaryCalculate _DefaultUnitaryCalculate = new DefaultUnitaryCalculate();

        /// <summary>
        /// 根据类型获取二元计算器
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static DualityCalculate GetCaculate(Value value1, Value value2)
        {
            Type type1 = value1.IsEmpty() ? typeof(NullValue) : value1.GetValueType();
            Type type2 = value2.IsEmpty() ? typeof(NullValue) : value2.GetValueType();

            return GetCaculate(type1, type2);
        }

        /// <summary>
        /// 根据类型获取一元计算器
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static UnitaryCalculate GetCaculate(Type type)
        {
            UnitaryCalculate v;
            _Dict2.TryGetValue(type, out v);

            return v;
        }

        /// <summary>
        /// 根据类型获取一元计算器
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static UnitaryCalculate GetCaculate(Value value)
        {
            if (value.IsEmpty())
                return _DefaultUnitaryCalculate;

            return GetCaculate(value.GetValueType());
        }

        class TypePair
        {
            public TypePair(Type type1, Type type2)
            {
                Type1 = type1;
                Type2 = type2;
            }

            public Type Type1 { get; private set; }

            public Type Type2 { get; private set; }

            public override int GetHashCode()
            {
                return Type1.GetHashCode() ^ Type2.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                TypePair v2 = (TypePair)obj;
                return v2.Type1 == Type1 && v2.Type2 == Type2;
            }
        }
    }
}
