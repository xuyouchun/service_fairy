using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Xml.XPath;

namespace TestXPath
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 1000;
            XmlDocument xDoc = _CreateDocument(count);

            Stopwatch sw = Stopwatch.StartNew();

            for (int k = 0; k < count; k++)
            {
                string xPath = "/root/element[@name='" + k + "']";
                xDoc.SelectSingleNode(xPath);
            }

            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        private static XmlDocument _CreateDocument(int count)
        {
            XPathDocument xDoc = new XPathDocument();
            xDoc.CreateNavigator().Select(
            xDoc.LoadXml("<root></root>");

            for (int k = 0; k < count; k++)
            {
                XmlElement e = xDoc.CreateElement("element");
                XmlAttribute attr = xDoc.CreateAttribute("name");
                attr.Value = k.ToString();
                e.Attributes.Append(attr);
                xDoc.DocumentElement.AppendChild(e);
            }

            return xDoc;
        }
    }
}
