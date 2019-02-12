using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Common.Utility;

namespace SnapIsoCountryCodes
{
    class InsertMccToCountryNames
    {
        public static void Execute()
        {
            string csharpCode = File.ReadAllText("CountryNames.txt");

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"D:\Work\Dev\Test\PhoneNumberArea\Data\operations.xml");
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (XmlElement countryElement in xDoc.SelectNodes("//country"))
            {
                string isoCode = countryElement.GetAttribute("isoCode");
                XmlElement opElement = countryElement.SelectSingleNode("operations/operation") as XmlElement;
                if (opElement != null)
                {
                    string mcc = opElement.GetAttribute("mcc");
                    dict.Add(isoCode.ToUpper(), mcc);
                }
            }

            StringBuilder notFound = new StringBuilder();
            string pattern = @"\[Country\(""([^""]*)"",\s*""([^""]*)"",\s*""([^""]*)"",\s*""([^""]*)"",\s*""[^""]*"",\s*[^\)]*\)\]";
            string newCSharpCode = Regex.Replace(csharpCode, pattern, delegate(Match m) {
                string line = m.Value;
                string enName = m.Groups[1].Value.ToUpper(), isoCode = m.Groups[2].Value, cnName = m.Groups[4].Value;
                string mcc;
                if (isoCode == "")
                    mcc = "";
                else if (!dict.TryGetValue(isoCode, out mcc))
                {
                    mcc = "";
                    notFound.AppendFormat("{0}\t{1}\t{2}\r\n", isoCode, cnName, enName);
                }

                return line.Insert(line.LastIndexOf(','), ", \"" + mcc + "\"");
            }, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            string notFounds = notFound.ToString();

            return;
        }
    }
}
