using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Contracts;
using System.Diagnostics.Contracts;
using System.Xml;

namespace Common.Package
{
    public class XmlConfigurationBuilder
    {
        public XmlConfigurationBuilder()
        {
            _xmlDoc = new XmlDocument();
            _xmlDoc.LoadXml("<configuration></configuration>");
        }

        private readonly XmlDocument _xmlDoc;

        /// <summary>
        /// 将另一个配置文件添加进来
        /// </summary>
        /// <param name="configuration"></param>
        public void Append(IConfiguration configuration)
        {
            Contract.Requires(configuration != null);

            foreach (string key in configuration.GetAllKeys())
            {
                Append(key, configuration.Get(key));
            }
        }

        /// <summary>
        /// 添加一个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Append(string key, string value)
        {
            Contract.Requires(key != null);

            if (value == null)
            {
                Remove(key);
                return;
            }

            XmlElement element = _FindElement(key, true);
            element.SetAttribute("value", value);
        }

        /// <summary>
        /// 删除一项配置
        /// </summary>
        /// <param name="key"></param>
        private void Remove(string key)
        {
            Contract.Requires(key != null);

            XmlElement element = _FindElement(key, false);
            if (element != null)
                element.ParentNode.RemoveChild(element);
        }

        private XmlElement _FindElement(string path, bool autoCreate = false)
        {
            string[] parts = path.Trim('/').Split(new[] { '/' });
            if (parts.Length == 0)
                return null;

            XmlElement current = _xmlDoc.DocumentElement, element;
            for (int k = 0; k < parts.Length; k++)
            {
                string name = parts[k];
                bool isKeyElement = (k == parts.Length - 1);
                string xPath = isKeyElement ? ("add[@key='" + name + "']") : ("section[@name='" + name + "']");
                element = current.SelectSingleNode(xPath) as XmlElement;
                if (element == null)
                {
                    if (!autoCreate)
                        return null;

                    current.AppendChild(element = _xmlDoc.CreateElement(isKeyElement ? "add" : "section"));
                    element.SetAttribute(isKeyElement ? "key" : "name", name);
                }

                current = element;
            }

            return current;
        }

        public override string ToString()
        {
            return _xmlDoc.OuterXml;
        }
    }
}
