using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics.Contracts;
using Common.Collection;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Concurrent;

namespace Common.Utility
{
    [System.Diagnostics.DebuggerStepThrough]
    public static class ReflectionUtility
    {
        /// <summary>
        /// 从程序集中寻找标有指定属性的类型
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="inherit"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Type[] SearchTypes<TAttribute>(this Assembly assembly, bool inherit = false) where TAttribute : Attribute
        {
            Contract.Requires(assembly != null);

            List<Type> types = new List<Type>();
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsDefined(typeof(TAttribute), inherit))
                    types.Add(type);
            }

            return types.ToArray();
        }

        /// <summary>
        /// 获取指定程序集的所有标有指定Attribute的类型
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TAttribute">Attribute类型</typeparam>
        /// <param name="assembly">程序集</param>
        /// <param name="keyCreator">键的创建方式</param>
        /// <param name="inherit">是否继承</param>
        /// <returns></returns>
        public static Dictionary<TKey, Type> SearchTypes<TKey, TAttribute>(this Assembly assembly, Func<TAttribute[], Type, TKey> keyCreator, bool inherit = false)
            where TAttribute : Attribute
        {
            Contract.Requires(assembly != null && keyCreator != null);

            return SearchTypes(new[] { assembly }, keyCreator, inherit);
        }
        
        /// <summary>
        /// 获取指定程序集的所有标有指定Attribute的类型
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TAttribute">Attribute类型</typeparam>
        /// <param name="assemblies">程序集</param>
        /// <param name="keyCreator">键的创建方式</param>
        /// <param name="inherit">是否继续</param>
        /// <returns></returns>
        public static Dictionary<TKey, Type> SearchTypes<TKey, TAttribute>(this IEnumerable<Assembly> assemblies,
            Func<TAttribute[], Type, TKey> keyCreator, bool inherit = false)
            where TAttribute : Attribute
        {
            Contract.Requires(assemblies != null && keyCreator != null);

            Dictionary<TKey, Type> dict = new Dictionary<TKey, Type>();

            foreach (Assembly assembly in assemblies.Distinct())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    TAttribute[] attrs = (TAttribute[])type.GetCustomAttributes(typeof(TAttribute), inherit);
                    if (attrs.Length > 0)
                    {
                        TKey key = keyCreator(attrs, type);
                        if (key != null)
                            dict.Add(key, type);
                    }
                }
            }

            return dict;
        }

        /// <summary>
        /// 获取类型中带有指定Attribute的内嵌类型
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="types"></param>
        /// <param name="keyCreator"></param>
        /// <param name="flags"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static Dictionary<TKey, Type> SearchNestedTypes<TKey, TAttribute>(this IEnumerable<Type> types,
            Func<TAttribute[], TKey> keyCreator, BindingFlags flags = BindingFlags.Default, bool inherit = false)
            where TAttribute : Attribute
        {
            Contract.Requires(types != null);

            Dictionary<TKey, Type> dict = new Dictionary<TKey, Type>();
            foreach (Type type in types)
            {
                foreach (Type nestedType in type.GetNestedTypes(flags))
                {
                    TAttribute[] attrs = (TAttribute[])nestedType.GetCustomAttributes(typeof(TAttribute), inherit);
                    if (attrs.Length > 0)
                    {
                        TKey key = keyCreator(attrs);
                        if (key != null)
                            dict.Add(key, nestedType);
                    }
                }
            }

            return dict;
        }

        /// <summary>
        /// 获取类型中带有指定Attribute的内嵌类型
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="type"></param>
        /// <param name="keyCreator"></param>
        /// <param name="flags"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static Dictionary<TKey, Type> SearchNestedTypes<TKey, TAttribute>(this Type type,
            Func<TAttribute[], TKey> keyCreator, BindingFlags flags = BindingFlags.Default, bool inherit = false)
            where TAttribute : Attribute
        {
            Contract.Requires(type != null);

            return SearchNestedTypes(new[] { type }, keyCreator, flags, inherit);
        }

        /// <summary>
        /// 从指定的类型中寻找标有指定类型属性的成员
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="type"></param>
        /// <param name="flags"></param>
        /// <param name="inherit"></param>
        /// <param name="keyCreator"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> 
            SearchMembers<TKey, TValue, TAttribute>(this Type type,
            Func<TAttribute[], MemberInfo, TKey> keyCreator, Func<TAttribute[], MemberInfo, TValue> valueCreator, BindingFlags flags = BindingFlags.Default, bool inherit = false)
            where TAttribute : Attribute
        {
            Contract.Requires(type != null && keyCreator != null);

            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            foreach (MemberInfo mInfo in type.GetMembers(flags))
            {
                TAttribute[] attrs = (TAttribute[])mInfo.GetCustomAttributes(typeof(TAttribute), inherit);
                if (attrs.Length > 0)
                    dict[keyCreator(attrs, mInfo)] = valueCreator(attrs, mInfo);
            }

            return dict;
        }

        /// <summary>
        /// 从指定的类型中寻找标有指定类型属性的成员
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="types"></param>
        /// <param name="flags"></param>
        /// <param name="inherit"></param>
        /// <param name="keyCreator"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue>
            SearchMembers<TKey, TValue, TAttribute>(this IEnumerable<Type> types,
            Func<TAttribute[], MemberInfo, TKey> keyCreator, Func<TAttribute[], MemberInfo, TValue> valueSelector, BindingFlags flags = BindingFlags.Default, bool inherit = false)
            where TAttribute : Attribute
        {
            Contract.Requires(types != null && keyCreator != null);

            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            foreach (Type type in types)
            {
                foreach (MemberInfo mInfo in type.GetMembers(flags))
                {
                    TAttribute[] attrs = (TAttribute[])mInfo.GetCustomAttributes(typeof(TAttribute), inherit);
                    if (attrs.Length > 0)
                        dict[keyCreator(attrs, mInfo)] = valueSelector(attrs, mInfo);
                }
            }

            return dict;
        }

        public static IEnumerable<T> SearchMembers<T, TAttribute>(this Type type,
            Func<TAttribute[], MemberInfo, T> creator, BindingFlags flags = BindingFlags.Default, bool inherit = false)
            where TAttribute : Attribute
        {
            Contract.Requires(type != null && creator != null);

            foreach (MemberInfo mInfo in type.GetMembers(flags))
            {
                TAttribute[] attrs = (TAttribute[])mInfo.GetCustomAttributes(typeof(TAttribute), inherit);
                if (attrs.Length > 0)
                    yield return creator(attrs, mInfo);
            }
        }

        /// <summary>
        /// 判断指定的类型是否派生自某接口
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <param name="interfaceType">接口类型</param>
        /// <returns></returns>
        public static bool IsImplementsInterface(this Type objectType, Type interfaceType)
        {
            if (objectType == null || interfaceType == null || !interfaceType.IsInterface)
                return false;

            return ((IList<Type>)objectType.GetInterfaces()).Contains(interfaceType);
        }

        /// <summary>
        /// 是否具有默认构造函数
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <returns></returns>
        public static bool HasDefaultConstructor(this Type objectType)
        {
            Contract.Requires(objectType != null);
            return objectType.GetConstructor(_EmptyTypeArray) != null;
        }

        private static readonly Type[] _EmptyTypeArray = new Type[0];

        /// <summary>
        /// 判断该类型是否可以通过new直接创建
        /// </summary>
        /// <param name="objectType">对象类型</param>
        /// <returns></returns>
        public static bool CanCreateDirect(this Type objectType)
        {
            Contract.Requires(objectType != null);

            return !objectType.IsAbstract && !objectType.IsArray && !objectType.IsInterface && !objectType.IsPointer
                && !objectType.IsGenericTypeDefinition && !objectType.IsEnum && objectType.HasDefaultConstructor();
        }

        /// <summary>
        /// 获取指定类型的指定Attribute
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="customAttributeProvider"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static TAttribute GetAttribute<TAttribute>(this ICustomAttributeProvider customAttributeProvider, bool inherit = false)
            where TAttribute : Attribute
        {
            Contract.Requires(customAttributeProvider != null);

            TAttribute[] attrs = customAttributeProvider.GetCustomAttributes(typeof(TAttribute), inherit).OfType<TAttribute>().ToArray();
            return attrs.Length > 0 ? attrs[0] : null;
        }

        /// <summary>
        /// 获取指定成员的所有指定Attribute
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="objectType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static TAttribute[] GetAttributes<TAttribute>(this ICustomAttributeProvider customAttributeProvider, bool inherit = false)
            where TAttribute : Attribute
        {
            Contract.Requires(customAttributeProvider != null);

            return customAttributeProvider.GetCustomAttributes(typeof(TAttribute), inherit).OfType<TAttribute>().ToArray();
        }

        /// <summary>
        /// 判断指定的方法是否与指定的委托相匹配
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="delegateType"></param>
        /// <returns></returns>
        public static bool IsMatch(this MethodInfo methodInfo, Type delegateType)
        {
            Contract.Requires(methodInfo != null && delegateType != null && delegateType.IsSubclassOf(typeof(Delegate)));

            MethodInfo mInfo = delegateType.GetMethod("Invoke");
            ParameterInfo[] params1, params2;
            if ((params1 = methodInfo.GetParameters()).Length != (params2 = mInfo.GetParameters()).Length)
                return false;

            for (int k = 0, length = params1.Length; k < length; k++)
            {
                Type type1 = params1[k].ParameterType, type2 = params2[k].ParameterType;
                if (type1 != type2 && !type2.IsSubclassOf(type1))
                    return false;
            }

            Type returnType1 = methodInfo.ReturnType, returnType2 = mInfo.ReturnType;
            return returnType1 == returnType2 || returnType1.IsSubclassOf(returnType2);
        }

        /// <summary>
        /// 安全卸载程序集
        /// </summary>
        /// <param name="domain"></param>
        public static void SafeUnload(this AppDomain domain)
        {
            if (domain == null)
                return;

            try
            {
                if (!domain.IsFinalizingForUnload())
                    AppDomain.Unload(domain);
            }
            catch { }
        }

        /// <summary>
        /// 获取类成员的描述
        /// </summary>
        /// <param name="mInfo"></param>
        /// <returns></returns>
        public static string GetDesc(this MemberInfo mInfo)
        {
            Contract.Requires(mInfo != null);

            IDescProvider descProvider = mInfo.GetCustomAttributes(true).OfType<IDescProvider>().FirstOrDefault();
            return descProvider == null ? string.Empty : descProvider.GetDesc();
        }

        /// <summary>
        /// 判断是否为数值型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumber(this Type type)
        {
            Contract.Requires(type != null);
            TypeCode typeCode = Type.GetTypeCode(type);
            return (int)typeCode >= (int)TypeCode.SByte && (int)typeCode <= (int)TypeCode.Decimal;
        }

        public static Type GetDeclaringType(this ICustomAttributeProvider attrProvider)
        {
            Contract.Requires(attrProvider != null);

            Type type = attrProvider as Type;
            if (type != null)
                return type;

            MemberInfo mInfo = attrProvider as MemberInfo;
            if (mInfo != null)
                return mInfo.DeclaringType;

            ParameterInfo pInfo = attrProvider as ParameterInfo;
            if (pInfo != null)
                return pInfo.Member.DeclaringType;

            return null;
        }

        public static string GetName(this ICustomAttributeProvider attrProvider)
        {
            Contract.Requires(attrProvider != null);

            MemberInfo mInfo = attrProvider as MemberInfo;
            if (mInfo != null)
                return mInfo.Name;

            Type type = attrProvider as Type;
            if (type != null)
                return type.Name;

            ParameterInfo pInfo = attrProvider as ParameterInfo;
            if (pInfo != null)
                return pInfo.Name;

            Assembly assembly = attrProvider as Assembly;
            if (assembly != null)
                return assembly.FullName;

            return null;
        }

        private static volatile Dictionary<Type, object> _defaultValues = new Dictionary<Type, object>() {
            { typeof(int), default(int) }, { typeof(uint), default(uint) }, { typeof(short), default(short) }, { typeof(ushort), default(ushort) },
            { typeof(long), default(long) }, { typeof(ulong), default(ulong) }, { typeof(byte), default(byte) }, { typeof(sbyte), default(sbyte) },
            { typeof(char), default(char) }, { typeof(decimal), default(decimal) }, { typeof(float), default(float) }, { typeof(double), default(double) },
            { typeof(bool), default(bool) }, { typeof(DateTime), default(DateTime) }, { typeof(TimeSpan), default(TimeSpan) }, { typeof(string), default(string) },
        };

        /// <summary>
        /// 获取指定类型的默认值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetDefaultValue(this Type type)
        {
            Contract.Requires(type != null);

            if(!type.IsValueType)
                return null;

            object value;
            if (_defaultValues.TryGetValue(type, out value))
                return value;

            lock (_defaultValues)
            {
                if (!_defaultValues.TryGetValue(type, out value))
                {
                    Dictionary<Type, object> dict = new Dictionary<Type, object>(_defaultValues);
                    dict.Add(type, value = Activator.CreateInstance(type));
                    _defaultValues = dict;
                }

                return value;
            }
        }

        /// <summary>
        /// 将对象克隆到指定的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        public static void Clone(object src, object dst)
        {
            Contract.Requires(src != null && dst != null);

            BindingFlags flag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            Type srcType = src.GetType(), dstType = dst.GetType();
            foreach (FieldInfo srcInfo in srcType.GetFields(flag))
            {
                FieldInfo dstInfo = dstType.GetField(srcInfo.Name, flag);
                if (dstInfo != null)
                    dstInfo.SetValue(dst, srcInfo.GetValue(src));
            }
        }

        /// <summary>
        /// 将对象按属性名称克隆到指定的对象
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        public static void CloneByProperty(object src, object dst)
        {
            Contract.Requires(src != null && dst != null);

            BindingFlags flag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            Type srcType = src.GetType(), dstType = dst.GetType();
            foreach (PropertyInfo srcInfo in srcType.GetProperties(flag))
            {
                PropertyInfo dstInfo = dstType.GetProperty(srcInfo.Name, flag);
                if (dstInfo != null)
                    dstInfo.SetValue(dst, srcInfo.GetValue(src, null), null);
            }
        }

        /// <summary>
        /// 获取类型的全名，（带有程序集名称）
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeFullName(Type type)
        {
            Contract.Requires(type != null);

            return type.FullName + "," + type.Assembly.FullName;
        }

        /// <summary>
        /// 获取指定字典类型的键类型与值类型，如果指定类型并非字典，则返回false
        /// </summary>
        /// <param name="type"></param>
        /// <param name="keyType"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        public static bool GetKeyValueType(Type type, out Type keyType, out Type valueType)
        {
            foreach (Type interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                {
                    Type[] typeParameters = interfaceType.GetGenericArguments();
                    keyType = typeParameters[0];
                    valueType = typeParameters[1];
                    return true;
                }
            }

            keyType = null;
            valueType = null;
            return false;
        }

        /// <summary>
        /// 获取类型的短名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeShortName(Type type)
        {
            Contract.Requires(type != null);

            if (type.IsEnum)
                return "enum";

            Type elementType = GetElementType(type);
            if (elementType != type)
                return GetTypeShortName(elementType) + "[]";

            if (type == typeof(Guid))
                return "guid";

            if (type == typeof(TimeSpan))
                return "timespan";

            TypeNameAttribute attr = type.GetAttribute<TypeNameAttribute>(false);
            if (attr != null && !string.IsNullOrEmpty(attr.ShortName))
                return attr.ShortName;

            Type keyType, valueType;
            if (GetKeyValueType(type, out keyType, out valueType))
                return "{" + GetTypeShortName(keyType) + "," + GetTypeShortName(valueType) + "}";

            TypeCode typeCode = Type.GetTypeCode(type);
            return typeCode.ToString().ToLower();
        }

        /// <summary>
        /// 获取集合的元素类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static Type GetElementType(Type type, bool recursive = false)
        {
            if (type == null)
                return null;

            if (type.IsGenericType)
            {
                Type typeDef = type.GetGenericTypeDefinition();
                if (typeDef == typeof(List<>) || typeDef == typeof(Collection<>))
                {
                    type = GetElementType(type.GetGenericArguments()[0]);
                    goto _end;
                }
            }
            else if (type.IsArray)
            {
                type = GetElementType(type.GetElementType());
                goto _end;
            }

            return type;

        _end:
            return recursive ? GetElementType(type, recursive) : type;
        }

        /// <summary>
        /// 尝试通过反射获取字段的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ReflectionInvokeResult TryGetFieldValue(object obj, string fieldName, out object value)
        {
            Contract.Requires(obj != null && !string.IsNullOrEmpty(fieldName));

            FieldInfo fInfo = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (fInfo == null)
            {
                value = null;
                return ReflectionInvokeResult.MissingMember;
            }

            value = fInfo.GetValue(obj);
            return ReflectionInvokeResult.Succeed;
        }

        /// <summary>
        /// 通过反射获取字段的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <param name="throwError"></param>
        /// <returns></returns>
        public static object GetFieldValue(object obj, string fieldName)
        {
            object value;
            if (TryGetFieldValue(obj, fieldName, out value) == ReflectionInvokeResult.MissingMember)
                throw new MissingFieldException("字段" + fieldName + "不存在");

            return value;
        }

        /// <summary>
        /// 尝试通过反射获取字段的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ReflectionInvokeResult TryGetPropertyValue(object obj, string propertyName, out object value)
        {
            Contract.Requires(obj != null && !string.IsNullOrEmpty(propertyName));

            PropertyInfo pInfo = obj.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (pInfo == null)
            {
                value = null;
                return ReflectionInvokeResult.MissingMember;
            }

            value = pInfo.GetValue(obj, null);
            return ReflectionInvokeResult.Succeed;
        }

        /// <summary>
        /// 通过反射获取属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(object obj, string propertyName)
        {
            object value;

            if (TryGetPropertyValue(obj, propertyName, out value) == ReflectionInvokeResult.MissingMember)
                throw new MissingMemberException("属性" + propertyName + "不存在");

            return value;
        }

        /// <summary>
        /// 通过反射获取字段或属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ReflectionInvokeResult TryGetValue(object obj, string expression, out object value)
        {
            string errorMember;
            return TryGetValue(obj, expression, out value, out errorMember);
        }

        /// <summary>
        /// 通过反射获取字段或属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <param name="errorMember"></param>
        /// <returns></returns>
        public static ReflectionInvokeResult TryGetValue(object obj, string expression, out object value, out string errorMember)
        {
            Contract.Requires(obj != null && expression != null);

            value = obj;
            foreach (string name in expression.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (TryGetPropertyValue(obj, name, out value) == ReflectionInvokeResult.MissingMember
                    && TryGetFieldValue(obj, name, out value) == ReflectionInvokeResult.MissingMember)
                {
                    errorMember = name;
                    return ReflectionInvokeResult.MissingMember;
                }

                if ((obj = value) == null)
                {
                    errorMember = name;
                    return ReflectionInvokeResult.NullValue;
                }
            }

            errorMember = null;
            return ReflectionInvokeResult.Succeed;
        }

        /// <summary>
        /// 通过反射获取字段或属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static object GetValue(object obj, string expression)
        {
            Contract.Requires(obj != null && expression != null);

            object value;
            string errorMember;
            if (TryGetValue(obj, expression, out value, out errorMember) == ReflectionInvokeResult.MissingMember)
                throw new MissingMemberException("属性或字段" + errorMember + "不存在");

            return value;
        }

        /// <summary>
        /// 通过反射获取字段或属性的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static T GetValue<T>(object obj, string expression)
        {
            return (T)GetValue(obj, expression);
        }

        /// <summary>
        /// 尝试设置字段的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ReflectionInvokeResult TrySetFieldValue(object obj, string fieldName, object value)
        {
            Contract.Requires(obj != null && string.IsNullOrEmpty(fieldName));

            FieldInfo fInfo = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.NonPublic | BindingFlags.Instance);
            if (fInfo == null)
                return ReflectionInvokeResult.MissingMember;

            fInfo.SetValue(obj, value);
            return ReflectionInvokeResult.Succeed;
        }

        /// <summary>
        /// 设置字段的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <param name="fieldName"></param>
        public static void SetFieldValue(object obj, object value, string fieldName)
        {
            if (TrySetFieldValue(obj, fieldName, value) == ReflectionInvokeResult.MissingMember)
                throw new MissingFieldException("字段" + fieldName + "不存在");
        }

        /// <summary>
        /// 尝试设置属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ReflectionInvokeResult TrySetPropertyValue(object obj, string propertyName, object value)
        {
            Contract.Requires(obj != null && propertyName != null);

            PropertyInfo pInfo = obj.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.NonPublic | BindingFlags.Instance);
            if (pInfo == null)
                return ReflectionInvokeResult.MissingMember;

            pInfo.SetValue(propertyName, value, null);
            return ReflectionInvokeResult.Succeed;
        }

        /// <summary>
        /// 设置字段的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(object obj, string propertyName, object value)
        {
            if (TrySetPropertyValue(obj, propertyName, value) == ReflectionInvokeResult.MissingMember)
                throw new MissingMemberException("属性" + propertyName + "不存在");
        }

        /// <summary>
        /// 设置属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ReflectionInvokeResult TrySetValue(object obj, string expression, object value)
        {
            string errorMember;
            return TrySetValue(obj, expression, value, out errorMember);
        }

        /// <summary>
        /// 设置属性的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <param name="errorMember"></param>
        /// <returns></returns>
        public static ReflectionInvokeResult TrySetValue(object obj, string expression, object value, out string errorMember)
        {
            Contract.Requires(obj != null && expression != null);

            int index = expression.LastIndexOf('.');
            object obj0 = obj;
            ReflectionInvokeResult r;

            if (index >= 0)
            {
                string exp = expression.Substring(0, index);
                r = TryGetValue(obj, exp, out obj0, out errorMember);
                if (r != ReflectionInvokeResult.Succeed)
                    return r;
            }

            errorMember = null;
            string name = expression.Substring(index + 1);
            if (TrySetPropertyValue(obj0, name, value) == ReflectionInvokeResult.Succeed)
                return ReflectionInvokeResult.Succeed;

            if ((r = TrySetFieldValue(obj0, name, value)) == ReflectionInvokeResult.Succeed)
                return ReflectionInvokeResult.Succeed;

            errorMember = name;
            return r;
        }

        /// <summary>
        /// 设置字段的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetValue(object obj, string expression, object value)
        {
            string errorMember;
            if (TrySetValue(obj, expression, value, out errorMember) == ReflectionInvokeResult.MissingMember)
                throw new MissingMemberException("属性或字段" + errorMember + "不存在");
        }

        /// <summary>
        /// 获取指定值的文本形式
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetObjectString(object obj)
        {
            if (obj == null)
                return "";

            Type type = obj.GetType();
            TypeCode typeCode = Type.GetTypeCode(type);
            if (typeCode != TypeCode.Object)
            {
                return obj.ToString();
            }

            BindingFlags f = BindingFlags.Public | BindingFlags.Instance;
            MethodInfo mInfo = type.GetMethod("ToString", f, null, Array<Type>.Empty, null);
            if (mInfo.DeclaringType != typeof(object))
            {
                return obj.ToString();
            }

            string name;
            int count = _GetCollectionCount(obj, out name);
            if (count >= 0)
                return "{" + type.Name + ": Count = " + count + "}";

            return "{" + obj.ToString() + "}";
        }

        private static int _GetCollectionCount(object obj, out string name)
        {
            Type type = obj.GetType();

            IDictionary dictionary = obj as IDictionary;
            if (dictionary != null)
            {
                name = "Dictionary";
                return dictionary.Count;
            }

            IList list = obj as IList;
            if (list != null)
            {
                name = type.IsArray ? "Array" : "List";
                return list.Count;
            }

            if (type.IsGenericType)
            {
                Type tempDef = type.GetGenericTypeDefinition();

                if (tempDef == typeof(HashSet<>))
                {
                    name = "HashSet";
                    return ((dynamic)obj).Count;
                }

                if (tempDef == typeof(Queue<>))
                {
                    name = "Queue";
                    return ((ICollection)obj).Count;
                }

                if (tempDef == typeof(Stack<>))
                {
                    name = "Stack";
                    return ((ICollection)obj).Count;
                }
            }

            ICollection collection = obj as ICollection;
            if (collection != null)
            {
                name = "Collection";
                return collection.Count;
            }

            name = null;
            return -1;
        }

        private static readonly ConcurrentDictionary<Enum, FieldInfo> _enumFieldInfos = new ConcurrentDictionary<Enum, FieldInfo>();

        /// <summary>
        /// 获取枚举类型指定值的字段
        /// </summary>
        /// <param name="enumValue">枚举值</param>
        /// <returns></returns>
        public static FieldInfo GetEnumFieldInfo(Enum enumValue)
        {
            Contract.Requires(enumValue != null);

            return _enumFieldInfos.GetOrAdd(enumValue, (ev) => {
                return ev.GetType().GetFields(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(f => object.Equals(f.GetValue(null), ev));
            });
        }
    }

    public enum ReflectionInvokeResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Succeed,

        /// <summary>
        /// 不存在字段或属性
        /// </summary>
        MissingMember,

        /// <summary>
        /// 遇到空值
        /// </summary>
        NullValue,
    }

    /// <summary>
    /// 标注类型的名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Delegate)]
    public class TypeNameAttribute : Attribute
    {
        public TypeNameAttribute(string shortName)
        {
            ShortName = shortName;
        }

        /// <summary>
        /// 短名
        /// </summary>
        public string ShortName { get; set; }
    }
}
