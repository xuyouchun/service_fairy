using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common.Utility;
using System.Text.RegularExpressions;
using System.Xml;

namespace SnapIsoCountryCodes
{
    class ConvertChinaToXml
    {
        const string _mobile_file = @"D:\Work\Dev\Test\PhoneNumberArea\Data\china\PhoneNumberArea.txt";
        const string _fixed_file = @"D:\Work\Dev\Test\PhoneNumberArea\Data\china\区号.txt";
        const string _dst = @"D:\Work\Dev\Test\PhoneNumberArea\Data\data";

        public static void Execute()
        {
            // 手机
            foreach (string line in File.ReadAllLines(_mobile_file))
            {
                string[] arr = Regex.Split(line, @"\s+");
                string phoneNumber = arr[0], operationName = arr[1], province = arr[2], city = arr.Length >= 4 ? arr[3] : "";
                string prefix = phoneNumber.Substring(0, 3), areaCode = phoneNumber.Substring(3, 4);
                _Append(prefix, areaCode, operationName, province, city);
            }

            // 固定电话
            foreach (string line in File.ReadAllLines(_fixed_file))
            {
                string[] arr = line.Split('\t');
                string cityName = arr[2], provinceName = arr[3], areaCode = arr[4].TrimStart('0');
                provinceName = provinceName.TrimEnd(new string[] { "省", "市", "自治区", "维吾尔", "壮族", "回族", "地区" });
                if (cityName == provinceName && cityName != "吉林")
                    cityName = "";

                _Append("", areaCode, "铁通", provinceName, cityName);
            }

            using (XmlTextWriter xw = new XmlTextWriter(Path.Combine(_dst, "cn.xml"), Encoding.UTF8))
            {
                xw.Formatting = Formatting.Indented;
                xw.WriteStartDocument();
                xw.WriteStartElement("root");

                // 基础信息
                xw.WriteStartElement("basic_info");
                xw.WriteElement("cn_name", "中国");
                xw.WriteElement("en_name", "China");
                xw.WriteElement("national_code", "86");
                xw.WriteElement("mcc", "460");
                xw.WriteEndElement();

                // 运营商
                xw.WriteStartElement("operations");
                foreach (OperationInfo opInfo in _operationDict.Values.Select(v => v.Desc)
                    .Distinct().Select(desc => OperationInfo.GetByDesc(desc)).OrderBy(opInfo => opInfo.OpCode))
                {
                    xw.WriteStartElement("operation");
                    xw.WriteAttributeString("op_code", opInfo.OpCode);
                    xw.WriteAttributeString("mncs", string.Join(",", opInfo.Mncs));
                    xw.WriteAttributeString("national_prefix", opInfo.NationalPrefix);
                    xw.WriteAttributeString("internal_prefix", opInfo.InternalPrefix);
                    xw.WriteAttributeString("desc", opInfo.Desc);
                    xw.WriteEndElement();
                }
                xw.WriteEndElement();

                // 各运营商数据
                xw.WriteStartElement("area_datas");
                foreach (OperationData data in _operationDict.Values.OrderBy(v => v.OpPrefix))
                {
                    OperationInfo opInfo = OperationInfo.GetByDesc(data.Desc);
                    xw.WriteStartElement("area_data");
                    xw.WriteAttributeString("storage_model", opInfo.StorageModel);
                    xw.WriteStartElement("prefixes");

                    xw.WriteStartElement("prefix");
                    xw.WriteAttributeString("op_code", opInfo.OpCode);
                    xw.WriteAttributeString("op_prefix", data.OpPrefix);
                    xw.WriteEndElement();

                    xw.WriteEndElement();

                    xw.WriteStartElement("cities");
                    foreach (KeyValuePair<City, HashSet<string>> item in data.Codes)
                    {
                        City city = item.Key;
                        xw.WriteStartElement("city");
                        xw.WriteAttributeString("province", city.ProvinceName);
                        xw.WriteAttributeString("city", city.CityName);
                        xw.WriteAttributeString("area_code", string.Join(",", item.Value.OrderBy(v => v)));
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

        #region Class StringTable ...

        class StringTable
        {
            private readonly Dictionary<string, StringTableItem> _dict = new Dictionary<string, StringTableItem>();

            public int GetStringID(string str)
            {
                return _dict.GetOrSet(str, s => new StringTableItem() { ID = _dict.Count, Text = s }).ID;
            }

            public StringTableItem[] GetAll()
            {
                return _dict.Values.ToArray();
            }
        }

        class StringTableItem
        {
            public int ID;
            public string Text;
        }

        #endregion

        #region Class Data ...

        class OperationData
        {
            public string Desc, OpPrefix;

            public Dictionary<City, HashSet<string>> Codes = new Dictionary<City, HashSet<string>>();
        }

        #endregion

        #region Class OperationInfo ...

        class OperationInfo
        {
            public string OpCode, Desc, StorageModel;
            public string[] Mncs;
            public string InternalPrefix, NationalPrefix;

            private static readonly OperationInfo _chinaMobile = new OperationInfo {
                Mncs = new[] { "00", "02", "07" }, OpCode = "China Mobile", Desc = "移动", StorageModel = "ellipsis", InternalPrefix = "", NationalPrefix = "00",
            };

            private static readonly OperationInfo _chinaUnion = new OperationInfo {
                Mncs = new[] { "01", "06" }, OpCode = "China Unicom", Desc = "联通", StorageModel = "ellipsis", InternalPrefix = "", NationalPrefix = "00",
            };

            private static readonly OperationInfo _chinaTelecom = new OperationInfo {
                Mncs = new[] { "03", "05", }, OpCode = "China Telecom", Desc = "电信", StorageModel = "ellipsis", InternalPrefix = "", NationalPrefix = "00",
            };

            private static readonly OperationInfo _chinaTietong = new OperationInfo {
                Mncs = new[] { "20" }, OpCode = "China Tietong", Desc = "铁通", StorageModel = "full", InternalPrefix = "0", NationalPrefix = "00"
            };

            public static OperationInfo GetByDesc(string desc)
            {
                switch (desc)
                {
                    case "移动":
                        return _chinaMobile;

                    case "联通":
                        return _chinaUnion;

                    case "电信":
                        return _chinaTelecom;

                    case "铁通":
                        return _chinaTietong;
                }

                throw new NotSupportedException("");
            }
        }

        #endregion

        #region City ...

        class City
        {
            public string ProvinceName;
            public string CityName;

            public override int GetHashCode()
            {
                return ProvinceName.GetHashCode() ^ CityName.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                City c = (City)obj;
                return c.ProvinceName == ProvinceName && c.CityName == CityName;
            }
        }

        #endregion

        private static readonly Dictionary<string, OperationData> _operationDict = new Dictionary<string, OperationData>();

        private static void _Append(string op_prefix, string areaCode, string operationName, string provinceName, string cityName)
        {
            OperationData operation = _operationDict.GetOrSet(op_prefix, prefix0 => new OperationData() { Desc = operationName, OpPrefix = prefix0 });
            City city = new City() { ProvinceName = provinceName, CityName = cityName };
            operation.Codes.GetOrSet(city).Add(areaCode);
        }
    }
}
