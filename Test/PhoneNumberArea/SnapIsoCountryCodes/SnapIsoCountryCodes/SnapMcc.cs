using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using Common.Utility;

namespace SnapIsoCountryCodes
{
    class SnapMcc
    {
        const string url = @"http://zh.wikipedia.org/wiki/%E7%A7%BB%E5%8A%A8%E7%BD%91%E7%BB%9C%E4%BB%A3%E7%A0%81";

        public static void ToXml()
        {
            string html = File.ReadAllText("MNC.txt", Encoding.UTF8);

            string pattern = @"(?:>([^<]*)(?:</a>)?\s*-\s*([A-Z]+)\s*(?:<sup\s+.*?</sup>)?</span></h4>\s*)<table\s+class=""wikitable""\s+width=""100%"">\s*<tr>(?:\s*<th\s+[^>]*>[^<]*</th>\s*){7}\s*</tr>\s*"
                  + @"((?:<tr>\s*(?:<td>.*?</td>\s*)*</tr>\s*)*?)</table>";

            //string pattern = @"<table\s+class=""wikitable""\s+width=""100%"">\s*<tr>(?:\s*<th\s+[^>]*>[^<]*</th>\s*){7}\s*</tr>";

            MatchCollection ms = Regex.Matches(html, pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            List<Country> countries = new List<Country>();
            foreach (Match m in ms)
            {
                string countryName = m.Groups[1].Value.Trim('(', ')', ' ');
                string isoCode = m.Groups[2].Value;
                string data = m.Groups[3].Value;

                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml("<root>" + data + "</root>");

                List<Operation> operations = new List<Operation>();
                foreach (XmlElement tr in xDoc.SelectNodes("/root/tr"))
                {
                    string mcc = _GetInnerText(tr, 0);
                    string mnc = _GetInnerText(tr, 1);
                    string brand = _GetInnerText(tr, 2);
                    string name = _GetInnerText(tr, 3);
                    string state = _GetInnerText(tr, 4);
                    string mhz = _GetInnerText(tr, 5);
                    string remark = _GetInnerText(tr, 6);

                    Operation op = new Operation { MCC = mcc, MNC = mnc, Brand = brand, OpName = name, State = state, MHZ = mhz, Remark = remark };
                    operations.Add(op);
                }

                countries.Add(new Country() { CountryName = countryName, IosCode = isoCode, Operations = operations.ToArray() });
            }

            using (XmlTextWriter xw = new XmlTextWriter(@"D:\Work\Dev\Test\PhoneNumberArea\Data\operations.xml", Encoding.UTF8))
            {
                xw.Formatting = Formatting.Indented;

                xw.WriteStartDocument();
                xw.WriteStartElement("root");

                xw.WriteStartElement("countries");
                foreach (Country country in countries)
                {
                    xw.WriteStartElement("country");
                    xw.WriteAttributeString("name", country.CountryName);
                    xw.WriteAttributeString("isoCode", country.IosCode);
                    xw.WriteStartElement("operations");

                    foreach (Operation op in country.Operations)
                    {
                        xw.WriteStartElement("operation");

                        xw.WriteAttributeString("mcc", op.MCC);
                        xw.WriteAttributeString("mnc", op.MNC);
                        xw.WriteAttributeString("brand", op.Brand);
                        xw.WriteAttributeString("op_name", op.OpName);
                        xw.WriteAttributeString("state", op.State);
                        xw.WriteAttributeString("mhz", op.MHZ);
                        xw.WriteAttributeString("remark", op.Remark);

                        xw.WriteEndElement();
                    }

                    xw.WriteEndElement();
                    xw.WriteEndElement();
                }
                xw.WriteEndElement();

                xw.WriteEndElement();
                xw.WriteEndDocument();
            }
        }

        private static string _GetInnerText(XmlElement e, int index)
        {
            if (index >= e.ChildNodes.Count)
                return "";

            return e.ChildNodes[index].InnerText.Trim();
        }

        class Operation
        {
            public string MCC, MNC;
            public string Brand, OpName, State, MHZ, Remark;
        }

        class Country
        {
            public string IosCode;

            public string CountryName;

            public Operation[] Operations;
        }

    }
}
