using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Common.Utility;
using System.Xml;
using Common.Contracts.Entities;
using Common.Mobile;

namespace SnapIsoCountryCodes
{
    class SnapNationalAreaCodes
    {
        private static void _Go(int a)
        {
            Console.WriteLine(a);
        }

        private static readonly OperationCountryManager _ocMgr
            = new OperationCountryManager(@"D:\Work\Dev\Test\PhoneNumberArea\Data\data\operations.xml");

        public static void Execute()
        {
            Country[] countries = _GetCountries().ToArray();

            _Snap(countries.First(c => c.CnName == "美国"));
            return;

            foreach (Country country in countries)
            {
                try
                {
                    Console.Write(country.CnName);
                    _Snap(country);
                    Console.WriteLine(" - OK!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" - ERROR: " + ex.Message);

                    File.AppendAllText(Path.Combine(_data_path, "error.txt"),
                        string.Format("{0}: {1}\r\n", country.CnName, ex.Message), Encoding.UTF8);
                }
            }
        }

        private const string _data_path = @"D:\Work\Dev\Test\PhoneNumberArea\Data\data_xml";

        private static void _Snap(Country country)
        {
            string path = "http://www.chahaoba.com/" + country.CnName;
            string html = Utility.GetFromUrl(path, Encoding.UTF8);
            int index = html.IndexOf("<b>中文名称");
            if (index >= 0)
                html = html.Substring(index);

            string file = Path.Combine(_data_path, country.CnName + ".xml");
            using (XmlTextWriter w = new XmlTextWriter(file, Encoding.UTF8))
            {
                w.Formatting = Formatting.Indented;
                w.WriteStartDocument();
                w.WriteStartElement("root");

                CountryInfo cInfo = _GetBaseInfos(html, w);
                _GetOperationInfos(cInfo, html, w);

                if (country.CnName == "美国")
                    _GetInternalCodes_American(html, w, cInfo);
                else
                    _GetInternalCodes(html, w, cInfo);

                _GetMobileInfos(html, w);
                _GetCommonlyUsed(html, w);

                w.WriteEndElement();
                w.WriteEndDocument();
            }
        }

        private static MatchCollection _Matches(string s, string pattern)
        {
            MatchCollection m = Regex.Matches(s, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return m;
        }

        private static Match _Match(string s, string pattern, bool throwError = false)
        {
            Match m = Regex.Match(s, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (throwError && !m.Success)
                throw new FormatException("格式错误，正则表达式未匹配到任何项：" + pattern);

            return m;
        }

        private static string _MatchValue(string s, string pattern, bool throwError = false)
        {
            Match m = _Match(s, pattern, throwError);
            return m.Success ? m.Groups[1].Value.Trim() : "";
        }

        // 基础信息
        private static CountryInfo _GetBaseInfos(string html, XmlTextWriter w)
        {
            string cn_name = _MatchValue(html, @"<b>中文名称：</b>\s*<a[^>]*>([^<]*)</a>");
            string en_name = _MatchValue(html, @"<b>英文名称：</b>\s*<a[^>]*>([^<]*)</a>");
            string national_code = _MatchValue(html, @"<b>国际区号：</b>\s*<a[^>]*>([^<]*)</a>");
            string example = _MatchValue(html, @"<b>拨号举例：</b>\s*([^<]*)</li>");
            string call_display = _MatchValue(html, @"<b>来电显示：</b>\s*([^<]*)</li>");
            string alias = _MatchValue(html, @"<b>别名：</b>\s*<a[^>]*>([^<]*)</a>");

            w.WriteStartElement("basic_info");

            CountryInfo cInfo = CountryInfo.FromChineseName(en_name) ?? CountryInfo.FromChineseName(cn_name) ?? CountryInfo.FromNationalCode(national_code).FirstOrDefault();
            if (cInfo == null)
                throw new InvalidDataException("未找到该国家");

            w.WriteElement("cn_name", cn_name);
            w.WriteElement("en_name", en_name);
            w.WriteElement("national_code", cInfo.NationalCode);
            w.WriteElement("mcc", cInfo.Mcc);
            w.WriteElement("example", example, true);
            w.WriteElement("call_display", call_display, true);
            w.WriteElement("alias", alias);

            w.WriteEndElement();

            return cInfo;
        }

        // 运营商信息
        private static void _GetOperationInfos(CountryInfo cInfo, string html, XmlTextWriter w)
        {
            OperationCountry ocCountry = _ocMgr.GetOperationCountry(cInfo);
            if (ocCountry == null)
                return;

            w.WriteStartElement("operations");

            foreach (var g in ocCountry.OperationDetails.GroupBy(od => od.GetOpCode()))
            {
                string brand = g.Key;
                OperationDetail od = g.First();

                string nationalPrefix, internalPrefix;
                if (!_GetPrefixes(html, out nationalPrefix, out internalPrefix))
                {
                    nationalPrefix = internalPrefix = "??";
                }

                w.WriteStartElement("operation");

                w.WriteAttributeString("op_code", brand);
                w.WriteAttributeString("mncs", string.Join(",", g.Select(v => v.Mnc)));
                w.WriteAttributeString("national_prefix", nationalPrefix);
                w.WriteAttributeString("internal_prefix", internalPrefix);
                w.WriteAttributeString("desc", brand);

                w.WriteEndElement();
            }

            w.WriteEndElement();
        }

        private static void _WriteError(XmlTextWriter w, string error_msg)
        {
            w.WriteElement("error", error_msg);
        }

        private static void _AppendOpPrefixes(XmlTextWriter w, CountryInfo countryInfo)
        {
            w.WriteStartElement("prefixes");
            OperationCountry oc = _ocMgr.GetOperationCountry(countryInfo);
            if (oc != null)
            {
                foreach (OperationDetail od in oc.OperationDetails)
                {
                    w.WriteStartElement("prefix");
                    w.WriteAttributeString("op_code", od.GetOpCode());
                    w.WriteAttributeString("op_prefix", "");
                    w.WriteEndElement();
                }
            }
            w.WriteEndElement();
        }

        // 国内区号
        private static void _GetInternalCodes(string html, XmlTextWriter w, CountryInfo countryInfo)
        {
            int index = html.IndexOf("内部长途电话区号");
            string s;
            if (index < 0 || (s = _MatchValue((html=html.Substring(index + 8)), @"<table\s+(.*?)</table>")) == "")
            {
                if ((s = _MatchValue(html, @"(<ul.*?</ul>)")) == "")
                {
                    _WriteError(w, "未匹配成功");
                }
                else
                {
                    w.WriteStartElement("remarks");
                    MatchCollection ms = _Matches(s, "<li>(.*?)</li>");
                    foreach (Match m in ms)
                    {
                        w.WriteElement("remark", m.Groups[1].Value.Trim());
                    }
                    w.WriteEndElement();
                }
            }
            else
            {
                MatchCollection ms = _Matches(s, @"<tr>\s*<td>\s*(?:<a\s+[^>]*>)?([^<]*)(?:</a>)?(?:<[^>]*>){2}(?:<a\s+[^>]*>)?([^<]*)(?:</a>)?</td><td>(?:<a[^>]*>)?([^<]*)(?:</a>)?");
                w.WriteStartElement("area_datas");
                w.WriteStartElement("area_data");
                w.WriteAttributeString("storage_model", "full");

                _AppendOpPrefixes(w, countryInfo);

                w.WriteStartElement("cities");
                foreach (Match m in ms)
                {
                    string en_name = m.Groups[1].Value.Trim(), cn_name = m.Groups[2].Value.Trim(), code = m.Groups[3].Value.Trim();
                    w.WriteStartElement("city");
                    w.WriteAttributeString("province", _GetProvinceName(cn_name, en_name));
                    w.WriteAttributeString("city", "");
                    w.WriteAttributeString("area_code", code);
                    w.WriteAttributeString("cn_name", cn_name);
                    w.WriteAttributeString("en_name", en_name);
                    w.WriteEndElement();
                }

                w.WriteEndElement();

                w.WriteEndElement();
                w.WriteEndElement();
            }
        }

        private static string _GetProvinceName(string cn_name, string en_name)
        {
            if (string.IsNullOrWhiteSpace(cn_name) || cn_name.IndexOf("中文") >= 0)
                return en_name ?? string.Empty;

            return cn_name.Trim();
        }

        private static void _GetInternalCodes_American(string html, XmlTextWriter w, CountryInfo countryInfo)
        {
            int index = html.IndexOf("内部长途电话区号");
            if (index < 0)
            {
                _WriteError(w, "未匹配成功");
            }
            else
            {
                string s = html.Substring(index + 8);
                MatchCollection ms = _Matches(s, @"<li>\s*(?:<a[^>]*>)?(\d+)(?:</a>)?：<strong\s+class=""selflink"">[^<]*</strong>\s*-\s*<a[^>]*>([^>]*)</a>\s*([^<]*)</li>");

                w.WriteStartElement("area_datas");
                w.WriteStartElement("area_data");
                w.WriteAttributeString("storage_model", "full");

                _AppendOpPrefixes(w, countryInfo);

                w.WriteStartElement("cities");

                foreach (Match m in ms)
                {
                    string code = m.Groups[1].Value.Trim(), cn_name = m.Groups[2].Value.Trim(), remarks = m.Groups[3].Value.Trim();
                    w.WriteStartElement("city");
                    w.WriteAttributeString("province", cn_name);
                    w.WriteAttributeString("city", "");
                    w.WriteAttributeString("area_code", code);
                    w.WriteEndElement();
                }

                w.WriteEndElement();

                w.WriteEndElement();
                w.WriteEndElement();
            }
        }

        // 移动电话信息
        private static void _GetMobileInfos(string html, XmlTextWriter w)
        {
            w.WriteStartElement("mobile");
            int index = html.IndexOf("移动电话号码前缀");
            string s;
            if (index < 0 || (s = _MatchValue(html.Substring(index + 8), @"<table\s+(.*?)</table>")) == "")
            {
                _WriteError(w, "未匹配成功");
            }
            else
            {
                MatchCollection ms = _Matches(s, @"<tr>\s*<td>(?:<a[^>]*>)?([^<]*)(?:</a>)?</td>\s*<td>(?:<a[^>]*>)?([^<]*)(?:</a>)?</td>\s*<td>(?:<a[^>]*>)?([^<]*)(?:</a>)?</td>\s*</tr>");
                w.WriteStartElement("ops");
                foreach (Match m in ms)
                {
                    w.WriteStartElement("op");
                    string op_name = m.Groups[1].Value.Trim(), wireless_type = m.Groups[2].Value.Trim(), prefix = m.Groups[3].Value.Trim();
                    w.WriteElement("op_name", op_name);
                    w.WriteElement("wireless_type", wireless_type);
                    w.WriteElement("prefix", prefix);
                    w.WriteEndElement();
                }
                w.WriteEndElement();
            }

            w.WriteEndElement();
        }

        // 拨出长途电话前缀
        private static bool _GetPrefixes(string html, out string national_prefix, out string internal_prefix)
        {
            int index = html.IndexOf("拨出长途电话前缀");
            if (index < 0)
            {
                national_prefix = internal_prefix = "";
                return false;
            }
            else
            {
                string s = html.Substring(index + 8);
                internal_prefix = _MatchValue(html, @"拨出内部直拨长途电话前缀：(\d*)");
                national_prefix = _MatchValue(html, @"拨出国际直拨长途电话前缀：(\d*)");
                return true;
            }
        }

        // 常用紧急电话号码
        private static void _GetCommonlyUsed(string html, XmlTextWriter w)
        {
            w.WriteStartElement("commonly_used");
            int index = html.IndexOf("常用紧急电话号码");
            string s;
            if (index < 0 || (s = _MatchValue(html, @"<ul>(.*?)</ul>")) == "")
            {
                _WriteError(w, "未匹配成功");
            }
            else
            {
                MatchCollection ms = _Matches(s, @"<li>([^：]*)：([^<]*)</li>");
                w.WriteStartElement("phones");
                foreach (Match m in ms)
                {
                    w.WriteStartElement("phone");
                    string name = m.Groups[1].Value.Trim(), number = m.Groups[2].Value.Trim();
                    w.WriteElement("name", name);
                    w.WriteElement("number", number);
                    w.WriteEndElement();
                }
                w.WriteEndElement();
            }

            w.WriteEndElement();
        }

        #region Class Country ...

        class Country
        {
            public string CnName { get; set; }

            public string Short2 { get; set; }

            public string NationalCode { get; set; }

            public string InternalCallPrefix { get; set; }

            public string NationalCallPrefix { get; set; }

            public override string ToString()
            {
                return CnName;
            }
        }

        #endregion

        private static IEnumerable<Country> _GetCountries()
        {
            string[][] items = StringUtility.ReadCsvFile(@"D:\Work\Dev\Test\PhoneNumberArea\Data\国家中文名称、长途电话区号、短名称、前缀.csv", Encoding.UTF8);

            foreach (string[] item in items)
            {
                yield return new Country() { NationalCode = item[0], CnName = item[1], Short2 = item[2], InternalCallPrefix = item[3], NationalCallPrefix = item[4] };
            }
        }
    }
}
