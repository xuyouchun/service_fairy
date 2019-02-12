using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.Serialization;
using Common.Utility;
using System.Collections;

namespace Common.Contracts
{
    /// <summary>
    /// 文档工具类
    /// </summary>
    public static class DocUtility
    {
        /// <summary>
        /// 根据实体类的类型生成文档
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static TypeDoc GetDoc(Type entityType, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {
            Contract.Requires(entityType != null);

            return _GetDoc(entityType, flags, new Dictionary<Type, TypeDoc>());
        }

        private static TypeDoc _GetDoc(Type entityType, BindingFlags flags, Dictionary<Type, TypeDoc> cache)
        {
            TypeDoc typeDoc = null;
            if (entityType == null || (typeDoc = _GetBasicTypeDoc(entityType)) != null ||
                ((typeDoc = cache.GetOrDefault(entityType)) != null && typeDoc != RecursiveType.Doc))
            {
                if (entityType != null && typeDoc != null)
                    cache[entityType] = typeDoc;

                return typeDoc;
            }

            var list = from memberInfo in entityType.GetMembers()
                       where memberInfo is PropertyInfo || memberInfo is FieldInfo
                       let attr = memberInfo.GetAttribute<DataMemberAttribute>(true)
                       where (attr != null || memberInfo is PropertyInfo) && !memberInfo.IsDefined(typeof(IgnoreDataMemberAttribute), true)
                       let doc = _GetFieldDoc(memberInfo, flags)
                       where doc != null
                       select doc;

            return new TypeDoc() {
                Name = GetName(entityType), Summary = GetSummary(entityType), Remarks = GetRemarks(entityType), CustomFlags = GetCustomFlagArray(entityType),
                TypeShortName = "object", Example = GetExample(entityType), FieldDocs = list.ToArray(),
                TypeFullName = _GetTypeFullName(entityType), TypeName = entityType.Name,
            };
        }

        private static Type _GetElementType(Type entityType)
        {
            return ReflectionUtility.GetElementType(entityType, true);
        }

        /// <summary>
        /// 获取文档树
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static TypeDocTree GetDocTree(Type entityType, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {
            Contract.Requires(entityType != null);

            TypeDoc typeDoc = GetDoc(entityType, flags);
            Dictionary<Type, TypeDoc> dict = new Dictionary<Type, TypeDoc>();
            _CollectionTypeDocs(entityType, dict, flags);
            return new TypeDocTree() { Root = typeDoc, SubTypeDocs = dict.Values.ToArray() };
        }

        class RecursiveType
        {
            public static readonly TypeDoc Doc = GetDoc(typeof(RecursiveType));
        }

        private static void _CollectionTypeDocs(Type type, Dictionary<Type, TypeDoc> cache, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {
            foreach (MemberInfo mInfo in type.GetMembers(flags))
            {
                Type memberType = _GetElementType(_GetFieldOrPropertyType(mInfo));
                if (memberType != null)
                {
                    if (!cache.ContainsKey(memberType))
                    {
                        cache[memberType] = RecursiveType.Doc;  // 此处防止递归调用导致的堆栈溢出
                        TypeDoc typeDoc = _GetDoc(memberType, flags, cache);
                        cache[memberType] = typeDoc;

                        if (_GetElementShortName(typeDoc.TypeShortName) == "object")
                            _CollectionTypeDocs(memberType, cache, flags);
                    }
                }
            }
        }

        private static TypeDoc _GetBasicTypeDoc(Type entityType)
        {
            string shortName = ReflectionUtility.GetTypeShortName(entityType);
            if (_GetElementShortName(shortName) != "object" || entityType == typeof(object))
            {
                return new TypeDoc() {
                    Name = GetName(entityType), TypeShortName = shortName, TypeName = entityType.Name, CustomFlags = GetCustomFlagArray(entityType),
                    Example = GetExample(entityType), Remarks = GetRemarks(entityType), Summary = GetSummary(entityType),
                    TypeFullName = _GetTypeFullName(entityType),
                };
            }

            return null;
        }

        private static string _GetElementShortName(string shortName)
        {
            int k = shortName.IndexOf('[');
            if (k < 0)
                return shortName;

            return shortName.Substring(0, k);
        }

        /// <summary>
        /// 根据类成员的信息生成文档
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static FieldDoc GetFieldDoc(MemberInfo memberInfo, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {
            Contract.Requires(memberInfo != null);

            return _GetFieldDoc(memberInfo, flags);
        }

        private static FieldDoc _GetFieldDoc(MemberInfo memberInfo, BindingFlags flags)
        {
            return new FieldDoc() {
                Name = GetName(memberInfo), Summary = GetSummary(memberInfo) ?? string.Empty, Remarks = GetRemarks(memberInfo) ?? string.Empty,
                PossibleValues = GetPossiableValues(memberInfo), Example = GetExample(memberInfo) ?? string.Empty, CustomFlags = GetCustomFlagArray(memberInfo),
                TypeFullName = _GetTypeFullName(memberInfo), TypeShortName = _GetTypeShortName(memberInfo), TypeName = _GetTypeName(memberInfo)
            };
        }

        private static string _GetTypeName(MemberInfo mInfo)
        {
            Type type = _GetFieldOrPropertyType(mInfo);
            if (type == null)
                return null;

            Type keyType, valueType;
            if (ReflectionUtility.GetKeyValueType(type, out keyType, out valueType))
            {
                return "{" + _GetTypeName(keyType) + "," + _GetTypeName(valueType) + "}";
            }

            return type.Name;
        }

        private static string _GetTypeFullName(MemberInfo mInfo)
        {
            Type type = _GetFieldOrPropertyType(mInfo);
            if (type == null)
                return null;

            return ReflectionUtility.GetTypeFullName(_GetElementType(type));
        }

        private static string _GetTypeShortName(MemberInfo mInfo)
        {
            Type type = _GetFieldOrPropertyType(mInfo);
            if (type == null)
                return null;

            return ReflectionUtility.GetTypeShortName(type);
        }

        private static Type _GetFieldOrPropertyType(MemberInfo mInfo)
        {
            Type t = mInfo as Type;
            if (t != null)
                return t;

            FieldInfo fInfo = mInfo as FieldInfo;
            PropertyInfo pInfo;

            if (fInfo != null)
                return fInfo.FieldType;
            else if ((pInfo = mInfo as PropertyInfo) != null)
                return pInfo.PropertyType;

            return null;
        }

        /// <summary>
        /// 获取名称
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static string GetName(MemberInfo memberInfo)
        {
            Contract.Requires(memberInfo != null);

            DataMemberAttribute attr = memberInfo.GetAttribute<DataMemberAttribute>(true);
            return (attr != null) ? (attr.Name ?? memberInfo.Name) : memberInfo.Name;
        }

        private static Type _IsNeedSearchFromType(string s, MemberInfo memberInfo, bool alsoSearchType)
        {
            Type t;
            if (s == null && alsoSearchType && (t = _GetFieldOrPropertyType(memberInfo)) != memberInfo && t != null)
                return _GetElementType(t);

            return null;
        }

        private static Type _IsNeedSearchFromType(IList arr, MemberInfo memberInfo, bool alsoSearchType)
        {
            Type t;
            if (arr == null && alsoSearchType && (t = _GetFieldOrPropertyType(memberInfo)) != memberInfo && t != null)
                return _GetElementType(t);

            return null;
        }

        /// <summary>
        /// 获取摘要
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="alsoSearchType"></param>
        /// <returns></returns>
        public static string GetSummary(MemberInfo memberInfo, bool alsoSearchType = true)
        {
            Contract.Requires(memberInfo != null);

            string s = _GetSummary(memberInfo);
            Type t = _IsNeedSearchFromType(s, memberInfo, alsoSearchType);
            return t == null ? s : _GetSummary(t);
        }

        private static TDocProvider _SearchDocProviderByRank<TDocProvider>(IEnumerable<TDocProvider> providers) where TDocProvider : class, IDocProvider
        {
            TDocProvider pMax = null;
            foreach (TDocProvider p in providers)
            {
                if (pMax == null || pMax.GetRank() < p.GetRank())
                    pMax = p;
            }

            return pMax;
        }

        private static string _GetSummary(MemberInfo memberInfo)
        {
            object[] attrs = memberInfo.GetCustomAttributes(true);
            IDocSummaryProvider p = _SearchDocProviderByRank(attrs.OfType<IDocSummaryProvider>());
            return p == null ? null : p.GetSummary();
        }

        /// <summary>
        /// 获取详细描述
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="alsoSearchType"></param>
        /// <returns></returns>
        public static string GetRemarks(MemberInfo memberInfo, bool alsoSearchType = true)
        {
            Contract.Requires(memberInfo != null);

            string s = _GetRemarks(memberInfo);
            Type t = _IsNeedSearchFromType(s, memberInfo, alsoSearchType);
            return t == null ? s : _GetRemarks(t);
        }

        private static string _GetRemarks(MemberInfo memberInfo)
        {
            object[] attrs = memberInfo.GetCustomAttributes(true);
            IDocRemarksProvider p = _SearchDocProviderByRank(attrs.OfType<IDocRemarksProvider>());
            return p == null ? null : p.GetRemarks();
        }

        /// <summary>
        /// 获取可能的值
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static PossibleValue[] GetPossiableValues(MemberInfo memberInfo, bool alsoSearchType = true)
        {
            Contract.Requires(memberInfo != null);

            PossibleValue[] values = _GetPossiableValues(memberInfo);
            Type t = _IsNeedSearchFromType(values, memberInfo, alsoSearchType);
            return t == null ? values : _GetPossiableValues(t);
        }

        private static PossibleValue[] _GetPossiableValues(MemberInfo memberInfo)
        {
            object[] attrs = memberInfo.GetCustomAttributes(true);
            IDocPossibleValueProvider p = _SearchDocProviderByRank(attrs.OfType<IDocPossibleValueProvider>());
            if (p != null)
                return p.GetPossibleValues();

            if (memberInfo is Type && ((Type)memberInfo).IsEnum)  // 枚举成员
            {
                List<PossibleValue> pvs = new List<PossibleValue>();
                foreach (FieldInfo fInfo in ((Type)memberInfo).GetFields(BindingFlags.Static | BindingFlags.Public))
                {
                    string name = fInfo.Name;
                    string desc = StringUtility.GetFirstNotNullOrEmptyString(fInfo.GetDesc(), GetRemarks(fInfo));

                    pvs.Add(new PossibleValue(name, desc));
                }

                return pvs.ToArray();
            }

            return null;
        }

        /// <summary>
        /// 获取示例
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="alsoSearchType"></param>
        /// <returns></returns>
        public static string GetExample(MemberInfo memberInfo, bool alsoSearchType = true)
        {
            Contract.Requires(memberInfo != null);
            string s = _GetExample(memberInfo);
            Type t = _IsNeedSearchFromType(s, memberInfo, alsoSearchType);
            return t == null ? s : _GetExample(t);
        }

        private static string _GetExample(MemberInfo memberInfo)
        {
            object[] attrs = memberInfo.GetCustomAttributes(true);
            IDocExampleProvider p = _SearchDocProviderByRank(attrs.OfType<IDocExampleProvider>());
            return p == null ? null : p.GetExample();
        }

        public static CustomFlag[] GetCustomFlagArray(MemberInfo memberInfo, bool alsoSearchType = true)
        {
            return GetCustomFlags(memberInfo, alsoSearchType).ToArray(item => new CustomFlag { Key = item.Key, Value = item.Value });
        }

        /// <summary>
        /// 获取自定义的标识
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="alsoSearchType"></param>
        /// <returns></returns>
        public static IDictionary<string, string> GetCustomFlags(MemberInfo memberInfo, bool alsoSearchType = true)
        {
            Contract.Requires(memberInfo != null);

            Type t;
            IDocCustomFlagProvider[] p1s = _GetCustomFlags(memberInfo),
                p2s = (alsoSearchType && (t = _GetFieldOrPropertyType(memberInfo)) != memberInfo && t != null) ? _GetCustomFlags(t) : null;

            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (IDocCustomFlagProvider p in new[] { p1s, p2s }.WhereNotNull().SelectMany().OrderBy(p => p.GetRank()))
            {
                foreach (string key in p.GetAllKeys())
                {
                    string value = p.GetFlag(key);
                    if (value != null)
                        dict[key] = value;
                }
            }

            return dict;
        }

        private static IDocCustomFlagProvider[] _GetCustomFlags(MemberInfo memberInfo)
        {
            object[] attrs = memberInfo.GetCustomAttributes(true);
            IDocCustomFlagProvider[] ps = attrs.OfType<IDocCustomFlagProvider>().ToArray();
            return ps;
        }
    }
}
