using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics.Contracts;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;

namespace Common.Utility
{
    /// <summary>
    /// XML的工具类
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public static class XmlUtility
    {
        /// <summary>
        /// 跳过当前的节点
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        public static void Skip(this XmlReader reader, Func<XmlReader, object, bool> callback = null, object state = null)
        {
            Contract.Requires(reader != null);

            if (reader.NodeType != XmlNodeType.Element || reader.IsEmptyElement)
                return;

            int depth = reader.Depth;
            if (callback == null)
            {
                while (reader.Read() && reader.Depth > depth) ;
            }
            else
            {
                while(reader.Read() && reader.Depth > depth)
                {
                    while (callback(reader, state))
                    {
                        if (reader.Depth <= depth)
                            goto end;
                    }
                }
            end: ;
            }
        }

        /// <summary>
        /// 写入一个节点
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="cdata"></param>
        public static void WriteElement(this XmlWriter writer, string name, object value, bool cdata = false)
        {
            Contract.Requires(writer != null && name != null);
            string s = (value == null) ? "" : value.ToString();

            writer.WriteStartElement(name);
            if (cdata)
                writer.WriteCData(s);
            else
                writer.WriteValue(s);
            writer.WriteEndElement();
        }

        /// <summary>
        /// 根据xpath读取属性值
        /// </summary>
        /// <param name="e"></param>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ReadAttribute(this XmlNode e, string path, string name)
        {
            Contract.Requires(e != null && path != null && name != null);

            XmlAttribute a;
            XmlNode n = e.SelectSingleNode(path);
            if (n == null || (a = n.Attributes[name]) == null)
                return null;

            return n.Value;
        }

        /// <summary>
        /// 根据xpath读取InnerXml
        /// </summary>
        /// <param name="e"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadInnerXml(this XmlNode e, string path)
        {
            Contract.Requires(e != null && path != null);

            XmlNode n = e.SelectSingleNode(path);
            if (n == null)
                return null;

            return n.InnerXml;
        }

        /// <summary>
        /// 根据xpath读取InnerText
        /// </summary>
        /// <param name="e"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadInnerText(this XmlNode e, string path)
        {
            Contract.Requires(e != null && path != null);

            XmlNode n = e.SelectSingleNode(path);
            if (n == null)
                return null;

            return n.InnerText;
        }

        /// <summary>
        /// 将对象序列化到流中
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="stream"></param>
        /// <param name="useDataContract"></param>
        public static void Serialize(object obj, Stream stream, bool useDataContract = true)
        {
            Contract.Requires(obj != null && stream != null);

            if (useDataContract)
            {
                DataContractSerializer ser = new DataContractSerializer(obj.GetType());
                ser.WriteObject(stream, obj);
            }
            else
            {
                XmlSerializer ser = new XmlSerializer(obj.GetType());
                ser.Serialize(stream, obj);
            }
        }

        /// <summary>
        /// 从流中反序列化对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="stream"></param>
        /// <param name="useDataContract"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, Stream stream, bool useDataContract = true)
        {
            Contract.Requires(type != null && stream != null);

            if (useDataContract)
            {
                DataContractSerializer ser = new DataContractSerializer(type);
                return ser.ReadObject(stream);
            }
            else
            {
                XmlSerializer ser = new XmlSerializer(type);
                return ser.Deserialize(stream);
            }
        }

        /// <summary>
        /// 从流中反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="useDataContract"></param>
        /// <returns></returns>
        public static T Deserialize<T>(Stream stream, bool useDataContract = true)
        {
            Contract.Requires(stream != null);

            return (T)Deserialize(typeof(T), stream, useDataContract);
        }

        /// <summary>
        /// 序列化对象为字符数组
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="useDataContract"></param>
        /// <returns></returns>
        public static byte[] SerializeToBytes(object obj, bool useDataContract = true)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serialize(obj, ms, useDataContract);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 从字符数组中反序列化为对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bytes"></param>
        /// <param name="count"></param>
        /// <param name="start"></param>
        /// <param name="useDataContract"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, byte[] bytes, int start, int count, bool useDataContract = true)
        {
            Contract.Requires(type != null && bytes != null);

            using (MemoryStream ms = new MemoryStream(bytes, start, count))
            {
                return Deserialize(type, ms, useDataContract);
            }
        }

        /// <summary>
        /// 从字符数组中反序列化为对象
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bytes"></param>
        /// <param name="useDataContract"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, byte[] bytes, bool useDataContract = true)
        {
            Contract.Requires(type != null && bytes != null);
            return Deserialize(type, bytes, 0, bytes.Length, useDataContract);
        }

        /// <summary>
        /// 从字符数组中反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="useDataContract"></param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] bytes, int start, int count, bool useDataContract = true)
        {
            return (T)Deserialize(typeof(T), bytes, start, count, useDataContract);
        }

        /// <summary>
        /// 从字符数组中反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <param name="useDataContract"></param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] bytes, bool useDataContract = true)
        {
            Contract.Requires(bytes != null);
            return Deserialize<T>(bytes, 0, bytes.Length, useDataContract);
        }

        /// <summary>
        /// 将指定的对象序列化为XML字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="useDataContract"></param>
        /// <param name="formatting"></param>
        /// <returns></returns>
        public static string SerializeToString(object obj, bool useDataContract = true, Formatting formatting = Formatting.None)
        {
            Contract.Requires(obj != null);

            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            xw.Formatting = formatting;

            if (useDataContract)
            {
                DataContractSerializer ser = new DataContractSerializer(obj.GetType());
                ser.WriteObject(xw, obj);
            }
            else
            {
                XmlSerializer ser = new XmlSerializer(obj.GetType());
                ser.Serialize(xw, obj);
            }

            xw.Flush();
            return sw.ToString();
        }

        /// <summary>
        /// 将对象序列化到文件中
        /// </summary>
        /// <param name="useDataContract"></param>
        /// <param name="packageInfoFile"></param>
        /// <param name="obj"></param>
        public static void SerializeToFile(object obj, string packageInfoFile, bool useDataContract = true)
        {
            using (FileStream fs = new FileStream(packageInfoFile, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                Serialize(obj, fs, useDataContract);
            }
        }

        /// <summary>
        /// 将xml字符串反序列化为实体
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <param name="useDataContract"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml, bool useDataContract = true)
        {
            Contract.Requires(type != null && xml != null);

            StringReader sr = new StringReader(xml);
            XmlTextReader xr = new XmlTextReader(sr);

            if (useDataContract)
            {
                DataContractSerializer ser = new DataContractSerializer(type);
                return ser.ReadObject(xr);
            }
            else
            {
                XmlSerializer ser = new XmlSerializer(type);
                return ser.Deserialize(xr);
            }
        }

        /// <summary>
        /// 将xml字符串反序列化为实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <param name="useDataConract"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string xml, bool useDataConract = true)
        {
            Contract.Requires(xml != null);

            return (T)Deserialize(typeof(T), xml, useDataConract);
        }

        /// <summary>
        /// 从文件中反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="file"></param>
        /// <param name="useDataContract"></param>
        /// <returns></returns>
        public static object DeserailizeFromFile(Type type, string file, bool useDataContract = true)
        {
            Contract.Requires(type != null && file != null);
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Deserialize(type, fs, useDataContract);
            }
        }

        /// <summary>
        /// 从文件中反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <param name="useDataContract"></param>
        /// <returns></returns>
        public static T DeserailizeFromFile<T>(string file, bool useDataContract = true)
        {
            Contract.Requires(file != null);

            return (T)DeserailizeFromFile(typeof(T), file, useDataContract);
        }

        /// <summary>
        /// 获取XML的示例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetXmlSampleForType(Type type)
        {
            Contract.Requires(type != null);

            return "XML Document";
        }
    }
}
