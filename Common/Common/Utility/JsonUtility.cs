using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Diagnostics.Contracts;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Common.Utility
{
    /// <summary>
    /// JSON的工具集
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public static class JsonUtility
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="obj"></param>
        public static void Serialize(Stream stream, object obj)
        {
            Contract.Requires(stream != null && obj != null);
            stream.Write(SerializeToBytes(obj));
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToString(object obj)
        {
            Contract.Requires(obj != null);
            return JsonConvert.SerializeObject(obj, _settings);
        }

        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings {
            Converters = new[] { new MyJsonConverter() }, NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat, DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
                MissingMemberHandling = MissingMemberHandling.Ignore, 
        };

        #region Class MyJsonConverter ...

        class MyJsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType.IsEnum || objectType.IsDefined(typeof(JsonSerializerAttributeBase), true);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JsonSerializerAttributeBase serAttr;
                string value = reader.Value.ToStringIgnoreNull(null);

                if (objectType.IsEnum)
                {
                    return Enum.Parse(objectType, value);
                }
                else if ((serAttr = objectType.GetAttribute<JsonSerializerAttributeBase>(true)) != null)
                {
                    return serAttr.ToObject(value);
                }

                return null;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                Type objectType = value.GetType();
                JsonSerializerAttributeBase serAttr;

                if (objectType.IsEnum)
                {
                    writer.WriteValue(value.ToString());
                }
                else if ((serAttr = objectType.GetAttribute<JsonSerializerAttributeBase>(true)) != null)
                {
                    writer.WriteValue(serAttr.ToString(value));
                }
            }
        }

        #endregion

        /// <summary>
        /// 序列化为字节流
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] SerializeToBytes(object obj)
        {
            Contract.Requires(obj != null);
            return Encoding.UTF8.GetBytes(SerializeToString(obj));
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T Deserialize<T>(Stream stream)
        {
            Contract.Requires(stream != null);
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(stream.ToBytes()), _settings);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, Stream stream)
        {
            Contract.Requires(type != null && stream != null);
            string json = Encoding.UTF8.GetString(stream.ToBytes());
            return JsonConvert.DeserializeObject(json, type, _settings);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] buffer, int start, int count)
        {
            Contract.Requires(buffer != null);
            return (T)Deserialize(typeof(T), buffer, start, count);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="buffer"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, byte[] buffer, int start, int count)
        {
            Contract.Requires(type != null && buffer != null);
            return Deserialize(type, Encoding.UTF8.GetString(buffer, start, count));
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] buffer)
        {
            Contract.Requires(buffer != null);
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(buffer));
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, byte[] buffer)
        {
            Contract.Requires(type != null);
            return Deserialize(type, Encoding.UTF8.GetString(buffer));
        }

        /// <summary>
        /// 逆序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            Contract.Requires(json != null);
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }

        /// <summary>
        /// 逆序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, string json)
        {
            Contract.Requires(type != null && json != null);
            return JsonConvert.DeserializeObject(json, type, _settings);
        }

        /*
        /// <summary>
        /// 选取一个JSON节点
        /// </summary>
        /// <param name="p"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public unsafe static byte* PickJsonNode(byte* p, int length)
        {
            Contract.Requires(p != null && length >= 0);

            byte* pEnd = p + length;
            while (p < pEnd)
            {
                if (*p == (byte)'{')
                {
                    
                }
                else if (*p == (byte)'"')
                {
                    
                }
            }

            return null;
        }*/

        private static void _BuildJsonSampleForCollection(Type type, string path, StringBuilder sb, HashSet<Type> types, JsonSampleValueCretor sampleValueCreator)
        {
            sb.Append("[ ");

            _BuildJsonSample(type, path, sb, types, sampleValueCreator);
            sb.Append(", ...");

            sb.Append(" ]");
        }

        private static void _BuildJsonSample(Type type, string path, StringBuilder sb, HashSet<Type> types, JsonSampleValueCretor sampleValueCreator)
        {
            if (types.Contains(type))
            {
                sb.AppendFormat("<recursive_type:{0}>", type.ToString());
                return;
            }

            try
            {
                types.Add(type);  // 防止类型嵌套定义造成的无限循环

                string sampleValue = sampleValueCreator(type, path) ?? _CreateJsonSampleValue(type, path);
                if (sampleValue != null)
                {
                    sb.Append(sampleValue);
                    return;
                }

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    _BuildJsonSampleForCollection(type.GetGenericArguments()[0], path, sb, types, sampleValueCreator);
                }
                else if (type.HasElementType)
                {
                    _BuildJsonSampleForCollection(type.GetElementType(), path, sb, types, sampleValueCreator);
                }
                else
                {
                    sb.Append("{ ");

                    int index = 0;
                    foreach (MemberInfo mInfo in type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                        .Where(mInfo => mInfo is PropertyInfo || mInfo is FieldInfo))
                    {
                        Type t = (mInfo is PropertyInfo) ? ((PropertyInfo)mInfo).PropertyType : ((FieldInfo)mInfo).FieldType;
                        if (mInfo.GetAttribute<IgnoreDataMemberAttribute>() != null)
                            continue;

                        DataMemberAttribute attr = mInfo.GetAttribute<DataMemberAttribute>();
                        if (attr == null && (mInfo is FieldInfo || !((PropertyInfo)mInfo).CanWrite))
                            continue;

                        if (index++ > 0)
                            sb.Append(", ");

                        string name = (attr == null) ? mInfo.Name : attr.Name ?? mInfo.Name;

                        sb.Append(_ReviseJsonNodeName(name)).Append(": ");
                        _BuildJsonSample(t, (path == "/") ? ("/" + name) : (path + "/" + name), sb, types, sampleValueCreator);
                    }

                    sb.Append(" }");
                }
            }
            finally
            {
                types.Remove(type);
            }
        }

        private static string _ReviseJsonNodeName(string name)
        {
            return "\"" + name.Replace("\"", "\\\"") + "\"";
        }

        private static string _CreateJsonSampleValue(Type type, string path)
        {
            string shortName = _GetElementShortName(ReflectionUtility.GetTypeShortName(type));
            if (shortName == "object")
                return null;

            return "<" + shortName + ">";
        }

        private static string _GetElementShortName(string shortName)
        {
            int k;
            if (shortName == null || (k = shortName.IndexOf('[')) < 0)
                return shortName;

            return shortName.Substring(0, k);
        }

        /// <summary>
        /// 获取示例数据并转换为json格式
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sampleValueCreator">提供示例的值</param>
        /// <returns></returns>
        public static string GetJsonSampleForType(Type type, JsonSampleValueCretor sampleValueCreator = null)
        {
            Contract.Requires(type != null);

            StringBuilder sb = new StringBuilder();
            HashSet<Type> types = new HashSet<Type>();
            _BuildJsonSample(type, "/", sb, types, sampleValueCreator ?? ((t, p) => null));
            return sb.ToString();
        }
    }

    public delegate string JsonSampleValueCretor(Type type, string path);

    /// <summary>
    /// 自定义JSON的序列化与反序列化操作
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public abstract class JsonSerializerAttributeBase : Attribute
    {
        public JsonSerializerAttributeBase()
        {

        }

        /// <summary>
        /// 获取指定对象的值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public new abstract string ToString(object obj);

        /// <summary>
        /// 转换为对象
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public abstract object ToObject(string str);
    }
}
