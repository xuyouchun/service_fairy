using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Common.Utility;

namespace SnapIsoCountryCodes
{
    class Old
    {
        public static void Execute()
        {
            Country[] items = _GetItems().ToArray();
            NationalItem[] nationalItems = _GetNationalItems().ToArray();
            Dictionary<string, NationalItem> nDict = nationalItems.ToDictionary(v => v.CnName);

            using (StreamWriter sw = new StreamWriter(@"d:\\temp\data.txt", false, Encoding.UTF8))
            using (StreamWriter swErr = new StreamWriter(@"d:\\temp\error.txt", false, Encoding.UTF8))
            using (StreamWriter swObjectC = new StreamWriter(@"d:\\temp\data-objectc.txt", false, Encoding.UTF8))
            {
                foreach (Country item in items)
                {
                    Console.Write(item.Name + ":");
                    NationalItem nItem = _GetNationalItem(nDict, item.Name);
                    if (nItem == null)
                    {
                        swErr.WriteLine(string.Format("{0}\t{1}\t{2}", item.NationalCode, item.Name, "未找到国家名称"));
                        swObjectC.WriteLine(string.Format("{{ {0},  \"{1}\",    \"{2}\"    \"{3}\"  \"{4}\" }}, /*未找到国家名称*/",
                            item.NationalCode, item.Name, "X", "X", "X"));
                        Console.WriteLine("未找到国家名称");
                        continue;
                    }

                    CountryEx ex;
                    try
                    {
                        ex = _GetItemEx(item.Name);
                        if (ex == null)
                        {
                            swErr.WriteLine(string.Format("{0}\t{1}\t{2}", item.NationalCode, item.Name, "未找到长途电话前缀"));
                            swObjectC.WriteLine(string.Format("{{ {0},  \"{1}\",    \"{2}\"    \"{3}\"  \"{4}\" }}, /*未找到长途电话前缀*/",
                                item.NationalCode, item.Name, "X", "X", "X"));
                            Console.WriteLine("未找到长途电话前缀");
                            continue;
                        }
                    }
                    catch (Exception ex0)
                    {
                        swErr.WriteLine(string.Format("{0}\t{1}\t{2}", item.NationalCode, item.Name, "出错: " + ex0.Message));
                        swObjectC.WriteLine(string.Format("{{ {0},  \"{1}\",    \"{2}\"    \"{3}\"  \"{4}\" }}, /*{5}*/",
                                item.NationalCode, item.Name, "X", "X", "X", ex0.Message));
                        continue;
                    }

                    sw.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}",
                        item.NationalCode, item.Name, nItem.EnName, nItem.Letter2, nItem.Letter3, nItem.Number, nItem.Iso3166, ex.InnerCode, ex.OuterCode));

                    swObjectC.WriteLine(string.Format("{{ {0},  \"{1}\",    \"{2}\",    \"{3}\",  \"{4}\" }},",
                        item.NationalCode, item.Name, nItem.Letter2, ex.InnerCode, ex.OuterCode));

                    sw.Flush();
                    swObjectC.Flush();
                    Console.WriteLine("OK!");
                }
            }

            return;
        }

        private static NationalItem _GetNationalItem(Dictionary<string, NationalItem> dict, string name)
        {
            foreach (string n in new[] { name, name + "国", name.TrimEnd('国') })
            {
                NationalItem nItem;
                if (dict.TryGetValue(n, out nItem))
                    return nItem;
            }

            return null;
        }

        private static CountryEx _GetItemEx(string name)
        {
            string html = Utility.GetFromUrl("http://www.chahaoba.com/" + name, Encoding.UTF8);

            Match m = Regex.Match(html, @"拨出内部直拨长途电话前缀：\s*(\d*).*?拨出国际直拨长途电话前缀：\s*(\d*)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (m.Success)
            {
                return new CountryEx() { InnerCode = m.Groups[1].Value, OuterCode = m.Groups[2].Value };
            }

            return null;
        }

        class Country
        {
            public string Name { get; set; }

            public string Short2 { get; set; }

            public string NationalCode { get; set; }

            public string InternalCallPrefix { get; set; }

            public string NationalCallPrefix { get; set; }
        }

        class CountryEx
        {
            public string InnerCode { get; set; }

            public string OuterCode { get; set; }
        }

        private static IEnumerable<Country> _GetItems()
        {
            string[][] items = StringUtility.ReadCsvFile(@"D:\Work\Dev\Test\PhoneNumberArea\Data\国家中文名称、长途电话区号、短名称、前缀.csv", Encoding.UTF8);

            foreach (string[] item in items)
            {
                yield return new Country() { NationalCode = item[0], Name = item[1], Short2 = item[2], InternalCallPrefix = item[3], NationalCallPrefix = item[4] };
            }
        }

        class NationalItem
        {
            public string Letter2 { get; set; }

            public string Letter3 { get; set; }

            public string Number { get; set; }

            public string Iso3166 { get; set; }

            public string EnName { get; set; }

            public string CnName { get; set; }
        }

        private static IEnumerable<NationalItem> _GetNationalItems()
        {
            string html = Utility.GetFromUrl("http://www.360doc.com/content/11/0228/15/3810344_96885463.shtml", Encoding.UTF8);
            string regex = @"<tr>\s+<td>([A-Z]*)</td>\s+<td>([A-Z]*)</td>\s+<td>(\d*)</td>\s+<td>[^>]*><font\s*color=[^>]*>([^<]*)</font></a></td>"
                + @"\s+<td>(?:[^>]*>){5}([^<]*)</font></a></td>\s+<td><a\s+(?:[^>]*>){2}([^<]*)<";
            MatchCollection ms = Regex.Matches(html, regex, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            foreach (Match m in ms)
            {
                var g = m.Groups;
                yield return new NationalItem() {
                    Letter2 = g[1].Value.Trim(),
                    Letter3 = g[2].Value.Trim(),
                    Number = g[3].Value.Trim(),
                    Iso3166 = g[4].Value.Trim(),
                    EnName = g[5].Value.Trim(),
                    CnName = g[6].Value.Trim(),
                };
            }
        }
    }
}
