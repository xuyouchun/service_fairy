using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace Common.Utility
{
    /// <summary>
    /// 枚举的工具类
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public static class EnumUtility
    {
        class EnumItem
        {
            public EnumItem(string desc, FieldInfo fieldInfo)
            {
                Desc = desc;
                FieldInfo = fieldInfo;
            }

            public string Desc { get; private set; }

            public FieldInfo FieldInfo { get; private set; }
        }

        private static readonly Dictionary<Type, Dictionary<Enum, EnumItem>> _enumValueDict = new Dictionary<Type, Dictionary<Enum, EnumItem>>();

        private static Dictionary<Enum, EnumItem> _LoadEnumDescDict(Type type)
        {
            var list = from fInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public)
                       let attr = fInfo.GetAttribute<DescAttributeBase>(true)
                       let desc = (attr == null) ? fInfo.GetValue(null).ToString() : attr.GetDesc()
                       select new { Value = fInfo.GetValue(null), EnumItem = new EnumItem(desc, fInfo) };

            return list.ToDictionary(v => (Enum)v.Value, v => v.EnumItem, true);
        }

        private static EnumItem _GetEnumItem(Enum enumValue)
        {
            Dictionary<Enum, EnumItem> fInfos = _enumValueDict.GetOrSet(enumValue.GetType(), _LoadEnumDescDict);
            EnumItem enumItem;
            fInfos.TryGetValue(enumValue, out enumItem);
            return enumItem;
        }

        /// <summary>
        /// 获取枚举成员的描述
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDesc(this Enum enumValue)
        {
            Contract.Requires(enumValue != null);

            EnumItem item = _GetEnumItem(enumValue);
            return item == null ? null : item.Desc;
        }

        public static FieldInfo GetFieldInfo(this Enum enumValue)
        {
            Contract.Requires(enumValue != null);

            EnumItem item = _GetEnumItem(enumValue);
            return item == null ? null : item.FieldInfo;
        }
    }
}
