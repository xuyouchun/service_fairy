using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Xml;
using Common.Utility;
using Common.Contracts;

namespace Common.Package
{
    /// <summary>
    /// 配置文件读取器
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class XmlConfigurationReader : IConfiguration, IEnumerable<string>
    {
        public XmlConfigurationReader(string xml)
        {
            Contract.Requires(xml != null);

            _content = xml;
        }

        private readonly string _content;
        private volatile Dictionary<string, string> _dict;
        private readonly object _thisLocker = new object();

        private void _EnsureInit()
        {
            if (_dict == null)
            {
                lock (_thisLocker)
                {
                    if (_dict != null)
                        return;

                    if (string.IsNullOrEmpty(_content))
                    {
                        _dict = new Dictionary<string, string>();
                    }
                    else
                    {
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.LoadXml(_content);
                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        _ReadSections(xDoc.DocumentElement.SelectSingleNode("/configuration") as XmlElement, "", dict);
                        _dict = dict;
                    }
                }
            }
        }

        /// <summary>
        /// 获取所有的key
        /// </summary>
        /// <returns></returns>
        public string[] GetAllKeys()
        {
            _EnsureInit();

            return _dict.Keys.ToArray();
        }

        /// <summary>
        /// 获取指定键的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            Contract.Requires(key != null);
            _EnsureInit();

            return _dict.GetOrDefault(key);
        }

        /// <summary>
        /// 区码取指定键的值，并转化为指定的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T Get<T>(string key, T defaultValue = default(T))
        {
            string value = Get(key);
            if (value == null)
                return defaultValue;

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        private void _ReadSections(XmlElement element, string path, Dictionary<string, string> dict)
        {
            if (element == null)
                return;

            foreach (XmlElement e in element.SelectNodes("add"))
            {
                string key = e.GetAttribute("key"), value = e.HasAttribute("value") ? e.GetAttribute("value") : e.InnerXml;
                if (key == null)
                    continue;

                if (!string.IsNullOrEmpty(path))
                    key = path + "/" + key;

                dict[key] = value;
            }

            foreach (XmlElement e in element.SelectNodes("section"))
            {
                string sectionName = e.GetAttribute("name");
                if (string.IsNullOrEmpty(sectionName))
                    continue;

                if (!string.IsNullOrEmpty(path))
                    sectionName = path + "/" + sectionName;

                _ReadSections(e, sectionName, dict);
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _dict.Keys.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
