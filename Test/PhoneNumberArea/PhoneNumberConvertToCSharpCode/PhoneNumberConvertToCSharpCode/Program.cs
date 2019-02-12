using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Utility;
using System.IO;
using System.Text.RegularExpressions;

namespace PhoneNumberConvertToCSharpCode
{
    class Program
    {
        const string file1 = @"D:\Work\Dev\Test\PhoneNumberArea\Data\国家中文名称、长途电话区号、短名称、前缀.csv";
        const string file2 = @"D:\Work\Dev\Test\PhoneNumberArea\Data\国家中文名称，英文名称，长途电话区号，时差.csv";
        const string file3 = @"D:\Work\Dev\Test\PhoneNumberArea\Data\data.txt";
        const string file = @"D:\Work\Dev\Test\PhoneNumberArea\Data\all.txt";
        static void Main(string[] args)
        {
            StringBuilder errors1 = new StringBuilder(), errors3 = new StringBuilder();
            StringBuilder datas = new StringBuilder();
            StringBuilder csCode = new StringBuilder();

            string[][] items1 = StringUtility.ReadCsvFile(file1, Encoding.UTF8), items2 = StringUtility.ReadCsvFile(file2, Encoding.UTF8);
            string[][] items3 = File.ReadAllLines(file3).ToArray(v => v.Split('\t'));
            Dictionary<string, string[]> dict1_ByCnName = items1.ToDictionary(item => item[1], true);  // 以短名作为Key：nationalCode, cnName, short2, innerCode, outerCode
            Dictionary<string, string[]> dict1_ByNationalCode = items1.ToDictionary(item => item[0], true);  // 以NationalCode作为Key：nationalCode, cnName, short2, innerCode, outerCode
            //Dictionary<string, string[]> dict2_ByNationalCode = items2.ToDictionary(item => item[2]);  // 以国际区号作为Key：cnName, enName, nationalCode, diffTime
            string[] duplex = items3.Select(item => item[3]).GroupBy(v => v).Where(v => v.Count() > 1).Select(v => v.Key).ToArray();
            Dictionary<string, string[]> dict3_ByShort2 = items3.ToDictionary(item => item[3], true);     // 以短名作为Key：nationalCode, cnName, enName, short2, short3, 
            Dictionary<string, string[]> dict3_ByNationalCode = items3.ToDictionary(item => item[0], true);// 以国际区号作为Key：nationalCode, cnName, enName, short2, short3, 
            foreach (var item2 in items2)
            {
                string enName = item2[1], nationalCode = item2[2], diffTime = item2[3];
                string[] item1;
                if (!dict1_ByCnName.TryGetValue(item2[0], out item1) && !dict1_ByNationalCode.TryGetValue(nationalCode, out item1))
                {
                    errors1.AppendFormat("{0}\t{1}\t{2}\r\n", nationalCode, item2[0], enName);
                    continue;
                }

                string cnName = item1[1], short2 = item1[2], innerCode = item1[3], outerCode = item1[4];

                string[] item3;
                if (!dict3_ByShort2.TryGetValue(short2, out item3) && !dict3_ByNationalCode.TryGetValue(nationalCode, out item3))
                {
                    errors3.AppendFormat("{0}\t{1}\t{2}\t{3}\r\n", nationalCode, cnName, enName, short2);
                    continue;
                }

                string short3 = item3[4];

                datas.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\r\n", enName, cnName, nationalCode, short2, short3, innerCode, outerCode, diffTime);

                string vaName = Regex.Replace(enName, @"\([^\)]*\)", "");
                vaName = Regex.Replace(vaName, @"\s+Is?\.", "", RegexOptions.IgnoreCase);
                vaName = string.Join("", Regex.Split(vaName, @"\s+").Select(s => StringUtility.UpperFirstChar(s)));
                vaName = Regex.Replace(vaName, @"\W", "");
                csCode.AppendLine("/// <summary>");
                csCode.AppendFormat("/// {0} ({1})\r\n", cnName, enName);
                csCode.AppendLine("/// </summary>");
                csCode.AppendFormat("[Country(\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", {5}f)]\r\n", enName, short2, short3, cnName, nationalCode, diffTime);
                csCode.AppendFormat("public const string {0} = \"{1}\";\r\n\r\n", vaName, enName);
            }

            string dataStr = datas.ToString(), error1Str = errors1.ToString(), error3Str = errors3.ToString();
            string csCodeStr = csCode.ToString();
            
            File.WriteAllText(file, dataStr);
            return;
        }
    }
}
