using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MobileAreaTestData
{
    public static class ConvertTestDataUtility
    {
        private const string _path = @"D:\Work\Data\PhoneNumberAreaData\2012-05-09";
        private static readonly Dictionary<string, int> _provinceIndexes = new Dictionary<string, int>();
        private static readonly List<string> _provinceNames = new List<string>();
        private static readonly List<Dictionary<string, int>> _cityIndexes = new List<Dictionary<string, int>>();
        private static readonly Dictionary<string, DataItem> _mses = new Dictionary<string, DataItem>();

        class DataItem
        {
            public readonly MemoryStream Ms = new MemoryStream();
            public byte LastProvinceIndex, LastCityIndex;
            public int Count;
        }

        private static int _GetProvinceIndex(string province)
        {
            int index;
            if (_provinceIndexes.TryGetValue(province, out index))
                return index;

            _provinceIndexes.Add(province, _provinceIndexes.Count);
            _cityIndexes.Add(new Dictionary<string, int>());
            _provinceNames.Add(province);
            return _provinceIndexes.Count - 1;
        }

        private static int _GetCityIndex(string province, string city)
        {
            int provinceIndex = _GetProvinceIndex(province);
            Dictionary<string, int> cityIndex = _cityIndexes[provinceIndex];

            int index;
            if (cityIndex.TryGetValue(city, out index))
                return index;

            cityIndex.Add(city, cityIndex.Count);
            return cityIndex.Count - 1;
        }

        private static void _Append(string opNum, ushort area, byte provinceIndex, byte cityIndex)
        {
            DataItem d;
            if (!_mses.TryGetValue(opNum, out d))
                _mses.Add(opNum, d = new DataItem());

            if (d.Count++ > 0 && d.LastProvinceIndex == provinceIndex && d.LastCityIndex == cityIndex)
                return;

            d.Ms.WriteByte((byte)area);
            d.Ms.WriteByte((byte)(area >> 8));
            d.Ms.WriteByte(provinceIndex);
            d.Ms.WriteByte(cityIndex);

            d.LastProvinceIndex = provinceIndex;
            d.LastCityIndex = cityIndex;
        }

        public static void Run()
        {
            // 手机号
            foreach (string fileName in new[] { "PhoneNumberArea.txt" })
            {
                string filePath = Path.Combine(_path, fileName);
                foreach (string s in File.ReadAllLines(filePath))
                {
                    string[] parts = s.Split(' ');
                    if (parts.Length != 4)
                        continue;

                    string opNum = parts[0].Substring(0, 3), area = parts[0].Substring(3, 4), opName = parts[1];
                    string province = parts[2], city = parts[3];
                    if (string.IsNullOrWhiteSpace(province))
                        province = city;

                    int provinceIndex = _GetProvinceIndex(province), cityIndex = _GetCityIndex(province, city);
                    _Append(opNum, ushort.Parse(area), (byte)provinceIndex, (byte)cityIndex);
                }
            }

            // 座机号
            foreach (QuhaoItem item in _ReadQuhaoItems(Path.Combine(_path, "区号.txt")).OrderBy(v => v.Code))
            {
                _AppendQuhao(item);
            }

            _SaveAll();
        }

        private static void _AppendQuhao(QuhaoItem item)
        {
            DataItem d;
            if (!_mses.TryGetValue("areacode", out d))
                _mses.Add("areacode", d = new DataItem());

            d.Ms.WriteByte((byte)item.Code);
            d.Ms.WriteByte((byte)(item.Code >> 8));
            d.Ms.WriteByte((byte)item.ProvinceIndex);
            d.Ms.WriteByte((byte)item.CityIndex);
        }

        private static IEnumerable<QuhaoItem> _ReadQuhaoItems(string path)
        {
            HashSet<ushort> codes = new HashSet<ushort>();
            foreach (string line in File.ReadAllLines(path, Encoding.Default))
            {
                string[] parts = line.Split('\t');
                if (parts.Length != 5)
                    throw new FormatException("");

                string[] postfix = new string[] { "省", "市", "地区", "维吾尔自治区", "回族自治区", "壮族自治区", "自治区" };
                string province = parts[3].Trim('"').TrimEnd(postfix);
                string city = parts[2].Trim('"').TrimEnd(postfix);
                ushort code = ushort.Parse(parts[4]);
                if (!codes.Contains(code))
                {
                    codes.Add(code);
                    yield return new QuhaoItem { ProvinceIndex = _GetProvinceIndex(province), CityIndex = _GetCityIndex(province, city), Code = code };
                }
                else
                {
                    continue;
                }
            }
        }

        private static string TrimEnd(this string s, params string[] ss)
        {
            foreach (string item in ss)
            {
                if (s.EndsWith(item))
                {
                    return s.Substring(0, s.Length - item.Length);
                }
            }

            return s;
        }

        class QuhaoItem
        {
            public int ProvinceIndex, CityIndex, Code;
        }

        private static void _SaveAll()
        {
            string dir = Path.Combine(_path, "bin");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            foreach (KeyValuePair<string, DataItem> item in _mses)
            {
                string file = Path.Combine(dir, item.Key + ".bin");
                byte[] bytes = item.Value.Ms.ToArray();

                File.WriteAllBytes(file, bytes);
            }

            StringBuilder sb_c = new StringBuilder();
            int index = 0;
            sb_c.AppendLine("const char *provinces[] = { " + string.Join(", ", _provinceIndexes.OrderBy(v => v.Value).Select(v => "\"" + v.Key + "\"")) + " };");
            sb_c.AppendLine("const char *cities[][" + (_cityIndexes.Max(v => v.Values.Count)) + "] = {");
            sb_c.AppendLine(string.Join(",\r\n", _cityIndexes.Select(v => "/*" + _provinceNames[index++] + "*/ { " + string.Join(", ", v.OrderBy(a => a.Value).Select(a => "\"" + a.Key + "\"")) + " }")));
            sb_c.AppendLine("};");
            File.WriteAllText(Path.Combine(_path, "bin\\code_c.txt"), sb_c.ToString(), Encoding.UTF8);

            index = 0;
            StringBuilder sb_csharp = new StringBuilder();
            sb_csharp.AppendLine("string provinces[] = new string[] { " + string.Join(", ", _provinceIndexes.OrderBy(v => v.Value).Select(v => "\"" + v.Key + "\"")) + " };");
            sb_csharp.AppendLine("string cities[][] = new string[][] {");
            sb_csharp.AppendLine(string.Join(",\r\n", _cityIndexes.Select(v => "/*" + _provinceNames[index++] + "*/ new string[]{ " + string.Join(", ", v.OrderBy(a => a.Value).Select(a => "\"" + a.Key + "\"")) + " }")));
            sb_csharp.AppendLine("};");
            File.WriteAllText(Path.Combine(_path, "bin\\code_csharp.txt"), sb_csharp.ToString(), Encoding.UTF8);

            index = 0;
            StringBuilder sb_java = new StringBuilder();
            sb_java.AppendLine("string provinces[] = new string[] { " + string.Join(", ", _provinceIndexes.OrderBy(v => v.Value).Select(v => "\"" + v.Key + "\"")) + " };");
            sb_java.AppendLine("string cities[][] = new string[][] {");
            sb_java.AppendLine(string.Join(",\r\n", _cityIndexes.Select(v => "/*" + _provinceNames[index++] + "*/ { " + string.Join(", ", v.OrderBy(a => a.Value).Select(a => "\"" + a.Key + "\"")) + " }")));
            sb_java.AppendLine("};");
            File.WriteAllText(Path.Combine(_path, "bin\\code_java.txt"), sb_java.ToString(), Encoding.UTF8);

            /*
            StringBuilder sb_object_c = new StringBuilder();
            sb_object_c.AppendLine("NSArray *operations = [[NSArray alloc] initWithObjects: " + string.Join(", ", _provinceIndexes.OrderBy(v => v.Value).Select(v => "@\"" + v.Key + "\"")) + "];");
            sb_object_c.AppendLine("NSArray *cities = [[NSArray alloc] initWithObjects: ");
            sb_object_c.AppendLine(string.Join(",\r\n", _cityIndexes.Select(v => "[[NSArray alloc] initWithObjects: " + string.Join(", ", v.OrderBy(a => a.Value).Select(a => "@\"" + a.Key + "\"")) + " ,nil ]")));
            sb_object_c.AppendLine(" ,nil];");
            File.WriteAllText(Path.Combine(_path, "bin\\code_object_c.txt"), sb_object_c.ToString(), Encoding.UTF8);
             * */

            index = 0;
            StringBuilder sb_object_c = new StringBuilder();
            sb_object_c.AppendLine("NSString *provinces[] = { " + string.Join(", ", _provinceIndexes.OrderBy(v => v.Value).Select(v => "@\"" + v.Key + "\"")) + " };");
            sb_object_c.AppendLine("NSString *cities[][" + (_cityIndexes.Max(v => v.Values.Count)) + "] = {");
            sb_object_c.AppendLine(string.Join(",\r\n", _cityIndexes.Select(v => "/*" + _provinceNames[index++] + "*/ { " + string.Join(", ", v.OrderBy(a => a.Value).Select(a => "@\"" + a.Key + "\"")) + " }")));
            sb_object_c.AppendLine("};");
            File.WriteAllText(Path.Combine(_path, "bin\\code_object_c.txt"), sb_object_c.ToString(), Encoding.UTF8);
        }
    }
}
